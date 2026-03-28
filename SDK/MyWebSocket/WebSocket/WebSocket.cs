using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using SuperSocket.ClientEngine;
using SuperSocket.ClientEngine.Protocol;
using WebSocket4Net.Protocol;

namespace WebSocket4Net
{
  public delegate void LogHandler(object sender, string msg);
  public partial class WebSocket : IDisposable
  {
    /// <summary>
    /// Gets the version of the websocket protocol.
    /// </summary>
    public WebSocketVersion Version { get; private set; }
    /// <summary>
    /// Gets the last active time of the websocket.
    /// </summary>
    public DateTime LastActiveTime { get; internal set; }
    /// <summary>
    /// Gets or sets a value indicating whether [enable auto send ping].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [enable auto send ping]; otherwise, <c>false</c>.
    /// </value>
    public bool EnableAutoSendPing { get; set; }
    /// <summary>
    /// Gets or sets the interval of ping auto sending, in seconds.
    /// </summary>
    /// <value>
    /// The auto send ping internal.
    /// </value>
    public int AutoSendPingInterval { get; set; }
    public bool SupportBinary
    {
      get { return ProtocolProcessor.SupportBinary; }
    }
    public WebSocketState State
    {
      get { return (WebSocketState)m_StateCode; }
    }
    public bool IsSocketConnected
    {
      get { return null != Client; }
    }
    public bool Handshaked { get; private set; }
#if !__IOS__
    public bool NoDelay { get; set; }
#endif
    public EventHandler Opened { get; set; }
    public EventHandler<DataReceivedEventArgs> DataReceived { get; set; }
    public EventHandler<MessageReceivedEventArgs> MessageReceived { get; set; }
    public EventHandler Closed { get; set; }
    public LogHandler Log { get; set; }
    public EventHandler<ErrorEventArgs> Error { get; set; }

    internal Uri TargetUri { get; private set; }
    internal string SubProtocol { get; private set; }
    internal IDictionary<string, object> Items { get; private set; }
    internal List<KeyValuePair<string, string>> Cookies { get; private set; }
    internal List<KeyValuePair<string, string>> CustomHeaderItems { get; private set; }
    internal int StateCode
    {
      get { return m_StateCode; }
    }
    internal TcpClientSession Client { get; private set; }
    internal IProtocolProcessor ProtocolProcessor { get; private set; }
    internal bool NotSpecifiedVersion { get; private set; }
    internal string LastPongResponse { get; set; }
    internal string HandshakeHost { get; private set; }
    internal string Origin { get; private set; }
    protected IClientCommandReader<WebSocketCommandInfo> CommandReader { get; private set; }

    protected const string UserAgentKey = "UserAgent";

    private int m_StateCode;
    private Dictionary<string, ICommand<WebSocket, WebSocketCommandInfo>> m_CommandDict
        = new Dictionary<string, ICommand<WebSocket, WebSocketCommandInfo>>(StringComparer.OrdinalIgnoreCase);
    /// <summary>
    /// It is used for ping/pong and closing handshake checking
    /// </summary>
    private Timer m_WebSocketTimer;
    private string m_LastPingRequest;

    private const string m_UriScheme = "ws";
    private const string m_UriPrefix = m_UriScheme + "://";
    private const string m_SecureUriScheme = "wss";
    private const int m_SecurePort = 443;
    private const string m_SecureUriPrefix = m_SecureUriScheme + "://";

    private static ProtocolProcessorFactory m_ProtocolProcessorFactory;
    
    static WebSocket()
    {
      m_ProtocolProcessorFactory = new ProtocolProcessorFactory(new Rfc6455Processor(), new DraftHybi10Processor(), new DraftHybi00Processor());
    }

    public int ReceiveBufferSize
    {
      get { return Client.ReceiveBufferSize; }
      set { Client.ReceiveBufferSize = value; }
    }
    public void Open()
    {
      m_StateCode = WebSocketStateConst.Connecting;
      
#if !__IOS__
      Client.NoDelay = NoDelay;
#endif

#if SILVERLIGHT
#if !WINDOWS_PHONE
            Client.ClientAccessPolicyProtocol = ClientAccessPolicyProtocol;
#endif
#endif
      Client.Connect();
    }
    public void Send(string message)
    {
      if (!EnsureWebSocketOpen())
        return;

      ProtocolProcessor.SendMessage(this, message);
    }
    public void Send(byte[] data, int offset, int length)
    {
      if (!EnsureWebSocketOpen())
        return;

      ProtocolProcessor.SendData(this, data, offset, length);
    }
    public void Send(IList<ArraySegment<byte>> segments)
    {
      if (!EnsureWebSocketOpen())
        return;

      ProtocolProcessor.SendData(this, segments);
    }
    public void Close()
    {
      Close(false);
    }
    public void Close(bool closeImmediately)
    {
      Close(string.Empty, closeImmediately);
    }
    public void Close(string reason, bool closeImmediately)
    {
      Close(ProtocolProcessor.CloseStatusCode.NormalClosure, reason, closeImmediately);
    }

    internal void Close(int statusCode, string reason)
    {
      Close(statusCode, reason, false);
    }
    internal void Close(int statusCode, string reason, bool closeImmediately)
    {
      OnLog(string.Format("Close: CloseImmediately {0} statusCode {1} m_StateCode {2}", closeImmediately, statusCode, m_StateCode));

      //The websocket never be opened
      if (Interlocked.CompareExchange(ref m_StateCode, WebSocketStateConst.Closed, WebSocketStateConst.None)
              == WebSocketStateConst.None) {
        OnClosed();
        return;
      }

      //The websocket is connecting or in handshake
      if (Interlocked.CompareExchange(ref m_StateCode, WebSocketStateConst.Closing, WebSocketStateConst.Connecting)
              == WebSocketStateConst.Connecting) {
        var client = Client;

        if (client != null) {
          client.Close();
          return;
        }

        OnClosed();
        return;
      }

      if (m_StateCode == WebSocketStateConst.Open || m_StateCode == WebSocketStateConst.Closing && closeImmediately) {
        m_StateCode = WebSocketStateConst.Closing;

        //Disable auto ping
        ClearTimer();

        var client = Client;
        if (client != null) {
          if (closeImmediately) {
            client.Close();
            OnClosed();
          } else {
            //Set closing hadnshake checking timer
            m_WebSocketTimer = new Timer(CheckCloseHandshake, null, 2000, Timeout.Infinite);
            ProtocolProcessor.SendCloseHandshake(this, statusCode, reason);
          }
        }
      }
    }
    internal void CloseWithoutHandshake()
    {
      var client = Client;
      if (client != null) {
        client.Close();
      }
    }
    internal bool GetAvailableProcessor(int[] availableVersions)
    {
      var processor = m_ProtocolProcessorFactory.GetPreferedProcessorFromAvialable(availableVersions);

      if (processor == null)
        return false;

      this.ProtocolProcessor = processor;
      return true;
    }
    internal void OnHandshaked()
    {
      m_StateCode = WebSocketStateConst.Open;

      Handshaked = true;

      if (Opened == null)
        return;

      Opened(this, EventArgs.Empty);

      if (EnableAutoSendPing && ProtocolProcessor.SupportPingPong) {
        //Ping auto sending interval's default value is 60 seconds
        if (AutoSendPingInterval <= 0)
          AutoSendPingInterval = 60;

        m_WebSocketTimer = new Timer(OnPingTimerCallback, ProtocolProcessor, AutoSendPingInterval * 1000, AutoSendPingInterval * 1000);
      }
    }
    internal void FireMessageReceived(string message)
    {
      if (MessageReceived == null)
        return;

      MessageReceived(this, new MessageReceivedEventArgs(message));
    }
    internal void FireDataReceived(byte[] data)
    {
      if (DataReceived == null)
        return;

      DataReceived(this, new DataReceivedEventArgs(data));
    }
    internal void FireLog(string msg)
    {
      OnLog(msg);
    }
    internal void FireError(Exception error)
    {
      OnError(error);
    }

    private EndPoint ResolveUri(string uri, int defaultPort, out int port)
    {
      TargetUri = new Uri(uri);

      IPAddress ipAddress;

      EndPoint remoteEndPoint;

      port = TargetUri.Port;

      if (port <= 0)
        port = defaultPort;

      if (IPAddress.TryParse(TargetUri.Host, out ipAddress))
        remoteEndPoint = new IPEndPoint(ipAddress, port);
      else
        remoteEndPoint = new DnsEndPoint(TargetUri.Host, port);

      return remoteEndPoint;
    }
    private TcpClientSession CreateClient(string uri)
    {
      int port;
      var targetEndPoint = ResolveUri(uri, 80, out port);

      if (port == 80)
        HandshakeHost = TargetUri.Host;
      else
        HandshakeHost = TargetUri.Host + ":" + port;
#if USE_IL2CPP
      return new TcpClientSession(targetEndPoint);
#else
      return new AsyncTcpSession(targetEndPoint);
#endif
    }
    private TcpClientSession CreateSecureClient(string uri)
    {
#if USE_IL2CPP
      throw new Exception("WebSocket(for IL2Cpp) don't support CreateSecureClient !");
#else
      int hostPos = uri.IndexOf('/', m_SecureUriPrefix.Length);

      if (hostPos < 0)//wss://localhost or wss://localhost:xxxx
            {
        hostPos = uri.IndexOf(':', m_SecureUriPrefix.Length, uri.Length - m_SecureUriPrefix.Length);

        if (hostPos < 0)
          uri = uri + ":" + m_SecurePort + "/";
        else
          uri = uri + "/";
      } else if (hostPos == m_SecureUriPrefix.Length)//wss://
            {
        throw new ArgumentException("Invalid uri", "uri");
      } else//wss://xxx/xxx
            {
        int colonPos = uri.IndexOf(':', m_SecureUriPrefix.Length, hostPos - m_SecureUriPrefix.Length);

        if (colonPos < 0) {
          uri = uri.Substring(0, hostPos) + ":" + m_SecurePort + uri.Substring(hostPos);
        }
      }

      int port;
      var targetEndPoint = ResolveUri(uri, m_SecurePort, out port);

      if (port == m_SecurePort)
        HandshakeHost = TargetUri.Host;
      else
        HandshakeHost = TargetUri.Host + ":" + port;

      return new SslStreamTcpSession(targetEndPoint);
#endif
    }
    private void Initialize(string uri, string subProtocol, List<KeyValuePair<string, string>> cookies, List<KeyValuePair<string, string>> customHeaderItems, string userAgent, string origin, WebSocketVersion version)
    {
      if (version == WebSocketVersion.None) {
        NotSpecifiedVersion = true;
        version = WebSocketVersion.Rfc6455;
      }

      Version = version;
      ProtocolProcessor = GetProtocolProcessor(version);

      Cookies = cookies;

      Origin = origin;

      if (!string.IsNullOrEmpty(userAgent)) {
        if (customHeaderItems == null)
          customHeaderItems = new List<KeyValuePair<string, string>>();

        customHeaderItems.Add(new KeyValuePair<string, string>(UserAgentKey, userAgent));
      }

      if (customHeaderItems != null && customHeaderItems.Count > 0)
        CustomHeaderItems = customHeaderItems;

      var handshakeCmd = new Command.Handshake();
      m_CommandDict.Add(handshakeCmd.Name, handshakeCmd);
      var textCmd = new Command.Text();
      m_CommandDict.Add(textCmd.Name, textCmd);
      var dataCmd = new Command.Binary();
      m_CommandDict.Add(dataCmd.Name, dataCmd);
      var closeCmd = new Command.Close();
      m_CommandDict.Add(closeCmd.Name, closeCmd);
      var pingCmd = new Command.Ping();
      m_CommandDict.Add(pingCmd.Name, pingCmd);
      var pongCmd = new Command.Pong();
      m_CommandDict.Add(pongCmd.Name, pongCmd);
      var badRequestCmd = new Command.BadRequest();
      m_CommandDict.Add(badRequestCmd.Name, badRequestCmd);

      m_StateCode = WebSocketStateConst.None;

      SubProtocol = subProtocol;

      Items = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

      TcpClientSession client;

      if (uri.StartsWith(m_UriPrefix, StringComparison.OrdinalIgnoreCase)) {
        client = CreateClient(uri);
      } else if (uri.StartsWith(m_SecureUriPrefix, StringComparison.OrdinalIgnoreCase)) {
        client = CreateSecureClient(uri);
      } else {
        throw new ArgumentException("Invalid uri", "uri");
      }

      client.Connected += new EventHandler(client_Connected);
      client.Closed += new EventHandler(client_Closed);
      client.Error += new EventHandler<ErrorEventArgs>(client_Error);
      client.DataReceived += new EventHandler<DataEventArgs>(client_DataReceived);

      Client = client;

      //Ping auto sending is enabled by default
      EnableAutoSendPing = true;
    }
    private void client_DataReceived(object sender, DataEventArgs e)
    {
      OnDataReceived(e.Data, e.Offset, e.Length);
    }
    private void client_Error(object sender, ErrorEventArgs e)
    {
      OnError(e);

      //Also fire close event if the connection fail to connect
      if (m_StateCode == WebSocketStateConst.Connecting) {
        m_StateCode = WebSocketStateConst.Closing;
        OnClosed();
      }
    }
    private void client_Closed(object sender, EventArgs e)
    {
      OnClosed();
    }
    private void client_Connected(object sender, EventArgs e)
    {
      OnConnected();
    }
    private void OnConnected()
    {
      CommandReader = ProtocolProcessor.CreateHandshakeReader(this);

      if (Items.Count > 0) {
        Items.Clear();

        FireLog("OnConnected Items.Clear");
      }

      ProtocolProcessor.SendHandshake(this);
    }
    private void OnPingTimerCallback(object state)
    {
      if (!string.IsNullOrEmpty(m_LastPingRequest) && !m_LastPingRequest.Equals(LastPongResponse)) {
        //have not got last response
        return;
      }

      var protocolProcessor = state as IProtocolProcessor;
      m_LastPingRequest = DateTime.Now.ToString();

      try {
        protocolProcessor.SendPing(this, m_LastPingRequest);
      } catch (Exception e) {
        OnError(e);
      }
    }
    private bool EnsureWebSocketOpen()
    {
      if (!Handshaked) {
        OnError(new Exception(m_NotOpenSendingMessage));
        return false;
      }

      return true;
    }
    private void OnClosed()
    {
      var fireBaseClose = false;

      if (m_StateCode == WebSocketStateConst.Closing || m_StateCode == WebSocketStateConst.Open)
        fireBaseClose = true;

      m_StateCode = WebSocketStateConst.Closed;

      if (fireBaseClose)
        FireClosed();
    }
    private void CheckCloseHandshake(object state)
    {
      OnLog(string.Format("CheckCloseHandshake: m_StateCode {0}", m_StateCode));

      if (m_StateCode == WebSocketStateConst.Closed)
        return;

      try {
        CloseWithoutHandshake();
      } catch (Exception e) {
        OnError(e);
      }
    }
    private void ExecuteCommand(WebSocketCommandInfo commandInfo)
    {
      ICommand<WebSocket, WebSocketCommandInfo> command;

      if (m_CommandDict.TryGetValue(commandInfo.Key, out command)) {
        command.ExecuteCommand(this, commandInfo);
      }
    }
    private void OnDataReceived(byte[] data, int offset, int length)
    {
      while (true) {
        int left;

        var commandInfo = CommandReader.GetCommandInfo(data, offset, length, out left);

        if (CommandReader.NextCommandReader != null)
          CommandReader = CommandReader.NextCommandReader;

        if (commandInfo == null)
          break;

        ExecuteCommand(commandInfo);

        if (left <= 0)
          break;

        offset = offset + length - left;
        length = left;
      }
    }    
    private void ClearTimer()
    {
      if (m_WebSocketTimer != null) {
        m_WebSocketTimer.Change(Timeout.Infinite, Timeout.Infinite);
        m_WebSocketTimer.Dispose();
        m_WebSocketTimer = null;
      }
    }
    private void FireClosed()
    {
      ClearTimer();

      var handler = Closed;

      if (handler != null)
        handler(this, EventArgs.Empty);
    }
    private void OnLog(string msg)
    {
      if (null != Log) {
        Log(this, msg);
      }
    }
    private void OnError(ErrorEventArgs e)
    {
      if (Error == null)
        return;

      Error(this, e);
    }
    private void OnError(Exception e)
    {
      OnError(new ErrorEventArgs(e));
    }

    private static IProtocolProcessor GetProtocolProcessor(WebSocketVersion version)
    {
      var processor = m_ProtocolProcessorFactory.GetProcessorByVersion(version);

      if (processor == null)
        throw new ArgumentException("Invalid websocket version");

      return processor;
    }
    private const string m_NotOpenSendingMessage = "You must send data by websocket after websocket is opened!";

    void IDisposable.Dispose()
    {
      var client = Client;

      if (client != null) {
        client.Close();
        Client = null;
      }
    }
  }
}
