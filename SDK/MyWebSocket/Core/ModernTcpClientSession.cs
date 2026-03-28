#if !USE_IL2CPP
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Concurrent;

namespace SuperSocket.ClientEngine
{
  public abstract class TcpClientSession
  {
    public EventHandler Connected { get; set; }
    public EventHandler Closed { get; set; }
    public EventHandler<ErrorEventArgs> Error { get; set; }
    public EventHandler<DataEventArgs> DataReceived { get; set; }
    public Socket Client { get; set; }
#if !__IOS__
    public bool NoDelay { get; set; }
#endif
    //==========================================================  
    protected ArraySegment<byte> Buffer { get; set; }
    protected string HostName { get; private set; }
    protected volatile bool IsSending = false;
    //==========================================================  
    private EndPoint RemoteEndPoint { get; set; }
    private ConcurrentQueue<ArraySegment<byte>> m_SendingQueue = new ConcurrentQueue<ArraySegment<byte>>();
    private DataEventArgs m_DataArgs = new DataEventArgs();
    private int m_ReceiveBufferSize;
    //==========================================================  
    public TcpClientSession(EndPoint remoteEndPoint)
      : this(remoteEndPoint, 1024)
    {
    }
    public TcpClientSession(EndPoint remoteEndPoint, int receiveBufferSize)
    {
      RemoteEndPoint = remoteEndPoint;
      ReceiveBufferSize = receiveBufferSize;

      var dnsEndPoint = remoteEndPoint as DnsEndPoint;

      if (dnsEndPoint != null) {
        HostName = dnsEndPoint.Host;
        return;
      }

      var ipEndPoint = remoteEndPoint as IPEndPoint;

      if (ipEndPoint != null)
        HostName = ipEndPoint.Address.ToString();
    }
    public int ReceiveBufferSize
    {
      get
      {
        return m_ReceiveBufferSize;
      }

      set
      {
        if (Buffer.Array != null)
          throw new Exception("ReceiveBufferSize cannot be set after the socket has been connected!");

        m_ReceiveBufferSize = value;
      }
    }    
    public void Connect()
    {
      if (Client != null)
        throw new Exception("The socket is connected, you neednt' connect again!");

      //采用Begin-End模式实现
      ConnectAsyncInternal(RemoteEndPoint, ProcessConnect, null);
    }
    public void Send(byte[] data, int offset, int length)
    {
      DetectConnected();

      m_SendingQueue.Enqueue(new ArraySegment<byte>(data, offset, length));

      if (!IsSending) {
        DequeueSend();
      }
    }
    public void Send(IList<ArraySegment<byte>> segments)
    {
      DetectConnected();

      for (var i = 0; i < segments.Count; i++)
        m_SendingQueue.Enqueue(segments[i]);

      if (!IsSending) {
        DequeueSend();
      }
    }
    public void Close()
    {
      if (EnsureSocketClosed())
        OnClosed();
    }
    //==========================================================  
    protected bool EnsureSocketClosed()
    {
      return EnsureSocketClosed(null);
    }
    protected bool EnsureSocketClosed(Socket prevClient)
    {
      var client = Client;
      if (client == null)
        return false;

      var fireOnClosedEvent = true;

      if (prevClient != null && prevClient != client) {
        //originalClient is previous disconnected socket, so we needn't fire event for it
        client = prevClient;
        fireOnClosedEvent = false;
      } else {
        Client = null;
        IsSending = false;
      }

      if (client.Connected) {
        try {
          client.Shutdown(SocketShutdown.Both);
        } catch {

        } finally {
          try {
            client.Close();
          } catch {

          }
        }
      }

      return fireOnClosedEvent;
    }
    protected bool DequeueSend()
    {
      IsSending = true;
      ArraySegment<byte> segment;

      if (!m_SendingQueue.TryDequeue(out segment)) {
        IsSending = false;
        return false;
      }

      //SendInternal(segment);
      SendInternal(segment);
      return true;
    }
    protected bool IsIgnorableSocketError(int errorCode)
    {
      //SocketError.Shutdown = 10058
      //SocketError.ConnectionAborted = 10053
      //SocketError.ConnectionReset = 10054
      if (errorCode == 10058 || errorCode == 10053 || errorCode == 10054)
        return true;

      return false;
    }
    //========================================================== 
    private void SetBuffer(ArraySegment<byte> bufferSegment)
    {
      Buffer = bufferSegment;
    }
    private void DetectConnected()
    {
      if (Client != null)
        return;

      throw new Exception("The socket is not connected!", new SocketException((int)SocketError.NotConnected));
    }
    //异步请求连接网络回调方法
    private void ProcessConnect(Socket socket, object state, SocketAsyncArgs e)
    {
      if (e != null && e.SocketError != SocketError.Success) {
        OnError(new SocketException((int)e.SocketError));
        Client = null;
        return;
      }
      if (socket == null) {
        OnError(new SocketException((int)SocketError.ConnectionAborted));
        Client = null;
        return;
      }
      Client = socket;
      Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
      //连接成功，子类具体响应处理
      OnGetSocket(e);
    }
    //========================================================
    protected virtual bool IsIgnorableException(Exception e)
    {
      if (e is System.ObjectDisposedException)
        return true;

      if (e is NullReferenceException)
        return true;

      return false;
    }      
    protected virtual void OnClosed()
    {
      var handler = Closed;

      if (handler != null)
        handler(this, EventArgs.Empty);
    }
    protected virtual void OnError(Exception e)
    {
      var handler = Error;
      if (handler == null)
        return;

      handler(this, new ErrorEventArgs(e));
    }
    protected virtual void OnConnected()
    {
      m_SendingQueue.Clear();
      var client = Client;

#if !__IOS__
      if (client != null) {
        if (client.NoDelay != NoDelay)
          client.NoDelay = NoDelay;
      }
#endif

      var handler = Connected;
      if (handler == null)
        return;

      handler(this, EventArgs.Empty);
    }
    protected virtual void OnDataReceived(byte[] data, int offset, int length)
    {
      var handler = DataReceived;
      if (handler == null)
        return;

      m_DataArgs.Data = data;
      m_DataArgs.Offset = offset;
      m_DataArgs.Length = length;

      handler(this, m_DataArgs);
    }
    //========================================================
    protected abstract void OnGetSocket(SocketAsyncArgs e);
    protected abstract void SendInternal(ArraySegment<byte> segment);

    //========================================================
    //异步创建网络连接
    private static void ConnectAsyncInternal(EndPoint remoteEndPoint, ConnectedCallback callback, object state)
    {
      if (remoteEndPoint is DnsEndPoint) {
        var dnsEndPoint = (DnsEndPoint)remoteEndPoint;

        var asyncResult = Dns.BeginGetHostAddresses(dnsEndPoint.Host, OnGetHostAddresses,
            new DnsConnectState {
              Port = dnsEndPoint.Port,
              Callback = callback,
              State = state
            });

        if (asyncResult.CompletedSynchronously)
          OnGetHostAddresses(asyncResult);
      } else {
        var e = CreateSocketAsyncArgs(remoteEndPoint, callback, state);
        var socket = new Socket(remoteEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        e.AcceptSocket = socket;
        socket.BeginConnect(remoteEndPoint, new AsyncCallback(ConnectionCallback), e);
      }
    }
    static SocketAsyncArgs CreateSocketAsyncArgs(EndPoint remoteEndPoint, ConnectedCallback callback, object state)
    {
      var e = new SocketAsyncArgs();
      e.State = state;
      e.Callback = callback;
      e.RemoteEndPoint = remoteEndPoint;
      return e;
    }
    //异步创建连接的回调方法
    private static void ConnectionCallback(IAsyncResult ar)
    {
      SocketAsyncArgs args = (SocketAsyncArgs)ar.AsyncState;
      try {
        args.AcceptSocket.EndConnect(ar);
        //args.AcceptSocket.SendTimeout = 3;
        //args.AcceptSocket.ReceiveTimeout = 3;
        args.SocketError = SocketError.Success;
        //连接成功，主动调用原事件            
        args.Callback(args.AcceptSocket, args.State, args);
      } catch (System.Exception ex) {
        // 错误处理
        if (ex.GetType() == typeof(SocketException)) {
          if (((SocketException)ex).SocketErrorCode == SocketError.ConnectionRefused) {
            //连接被服务器拒绝
            args.SocketError = SocketError.ConnectionRefused;
          } else {
            //连接丢失
            args.SocketError = SocketError.ConnectionAborted;
          }
        } else {
          args.SocketError = SocketError.ConnectionAborted;
        }
        if (args.AcceptSocket.Connected) {
          args.AcceptSocket.Shutdown(SocketShutdown.Receive);
          args.AcceptSocket.Close(0);
        } else {
          args.AcceptSocket.Close();
        }
        args.Callback(args.AcceptSocket, args.State, args);
      }
    }
    private static void OnGetHostAddresses(IAsyncResult result)
    {
      var connectState = result.AsyncState as DnsConnectState;

      IPAddress[] addresses;

      try {
        addresses = Dns.EndGetHostAddresses(result);
      } catch {
        connectState.Callback(null, connectState.State, null);
        return;
      }

      if (addresses == null || addresses.Length <= 0) {
        connectState.Callback(null, connectState.State, null);
        return;
      }

      connectState.Addresses = addresses;

      CreateAttempSocket(connectState);

      Socket attempSocket;

      var address = GetNextAddress(connectState, out attempSocket);

      if (address == null) {
        connectState.Callback(null, connectState.State, null);
        return;
      }

      var ipEndPoint = new IPEndPoint(address, connectState.Port);
      var e = CreateSocketAsyncArgs(ipEndPoint, connectState.Callback, null);
      var socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
      e.AcceptSocket = socket;
      socket.BeginConnect(ipEndPoint, new AsyncCallback(ConnectionCallback), e);
    }
    private static void CreateAttempSocket(DnsConnectState connectState)
    {
      connectState.Socket4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }    
    private static IPAddress GetNextAddress(DnsConnectState state, out Socket attempSocket)
    {
      IPAddress address = null;
      attempSocket = null;

      var currentIndex = state.NextAddressIndex;

      while (attempSocket == null) {
        if (currentIndex >= state.Addresses.Length)
          return null;

        address = state.Addresses[currentIndex++];

        if (address.AddressFamily == AddressFamily.InterNetworkV6) {
          attempSocket = state.Socket6;
        } else if (address.AddressFamily == AddressFamily.InterNetwork) {
          attempSocket = state.Socket4;
        }
      }

      state.NextAddressIndex = currentIndex;
      return address;
    }
  }
}
#endif