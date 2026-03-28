#if USE_IL2CPP
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SuperSocket.ClientEngine
{
  public class TcpClientSession
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
    private ArraySegment<byte> Buffer { get; set; }
    private string HostName { get; set; }
    //==========================================================  
    private EndPoint RemoteEndPoint { get; set; }
    private SimpleConcurrentQueue<ArraySegment<byte>> m_SendingQueue = new SimpleConcurrentQueue<ArraySegment<byte>>();
    private SimpleConcurrentQueue<Action> m_SocketActionQueue = new SimpleConcurrentQueue<Action>();
    private DataEventArgs m_DataArgs = new DataEventArgs();
    private int m_ReceiveBufferSize;

    //==========================================================  
    private Thread m_Thread = null;
    private const int c_MaxActionNumPerTick = 1024;
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

      if (Buffer.Array == null) {
        Buffer = new ArraySegment<byte>(new byte[ReceiveBufferSize], 0, ReceiveBufferSize);
      }
      try {
        Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_Thread = new Thread(this.Tick);
        m_Thread.Start();
        QueueAction(DoConnect);
      } catch (Exception ex) {
        OnError(ex);
        Client = null;
      }
    }
    public void Send(byte[] data, int offset, int length)
    {
      DetectConnected();
      m_SendingQueue.Enqueue(new ArraySegment<byte>(data, offset, length));
    }
    public void Send(IList<ArraySegment<byte>> segments)
    {
      DetectConnected();
      for (var i = 0; i < segments.Count; i++)
        m_SendingQueue.Enqueue(segments[i]);
    }
    public void Close()
    {
      if (null != Client) {
        QueueAction(DoClose);
        m_Thread.Join(1000);
      }
    }
    //========================================================
    private void DoConnect()
    {
      try {
        Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
        Client.Connect(RemoteEndPoint);
        Client.Blocking = false;
        OnConnected();
      } catch (SocketException exc) {
        if (!IsIgnorableSocketError(exc.ErrorCode))
          OnError(exc);
        if (EnsureSocketClosed())
          OnClosed();
      } catch (Exception e) {
        if (!IsIgnorableException(e))
          OnError(e);
        if (EnsureSocketClosed())
          OnClosed();
      }
    }
    private void DoClose()
    {
      if (EnsureSocketClosed())
        OnClosed();
    }

    //========================================================
    private void QueueAction(Action action)
    {
      m_SocketActionQueue.Enqueue(action);
    }
    private void Tick()
    {
      while (null != Client) {
        try {
          DoSocketActions();
          if (null != Client) {
            bool result = Client.Poll(1000, SelectMode.SelectRead);
            if (result) {
              while (Client.Available > 0) {
                int size = Client.Receive(Buffer.Array);
                OnDataReceived(Buffer.Array, 0, size);
              }
            }
            result = Client.Poll(1000, SelectMode.SelectWrite);
            if (result) {
              DequeueSend();
            }
          }
        } catch (SocketException exc) {
          if (!IsIgnorableSocketError(exc.ErrorCode))
            OnError(exc);
          if (EnsureSocketClosed())
            OnClosed();
        } catch (Exception e) {
          if (!IsIgnorableException(e))
            OnError(e);
          if (EnsureSocketClosed())
            OnClosed();
        }
      }
    }
    private void DoSocketActions()
    {
      try {
        for (int i = 0; i < c_MaxActionNumPerTick; ++i) {
          if (m_SocketActionQueue.Count > 0) {
            Action action = null;
            m_SocketActionQueue.TryDequeue(out action);
            if (null != action) {
              try {
                action();
              } catch (Exception ex) {
                OnError(ex);
              }
            }
          } else {
            break;
          }
        }
      } catch (Exception ex) {
        OnError(ex);
      }
    }
    //========================================================
    private bool IsIgnorableException(Exception e)
    {
      /*
      if (e is System.ObjectDisposedException)
        return true;

      if (e is NullReferenceException)
        return true;
      */
      return false;
    }
    private bool IsIgnorableSocketError(int errorCode)
    {
      //SocketError.Shutdown = 10058
      //SocketError.ConnectionAborted = 10053
      //SocketError.ConnectionReset = 10054
      if (errorCode == 10058 || errorCode == 10053 || errorCode == 10054)
        return true;

      return false;
    }
    //========================================================
    private bool EnsureSocketClosed()
    {
      return EnsureSocketClosed(null);
    }
    private bool EnsureSocketClosed(Socket prevClient)
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
    private bool DequeueSend()
    {
      ArraySegment<byte> segment;

      if (!m_SendingQueue.TryDequeue(out segment)) {
        return false;
      }

      try {
        if (null == Client) {
          OnError(new Exception("socket is null"));
          return false;
        }
        int size = Client.Send(segment.Array, segment.Offset, segment.Count, SocketFlags.None);
        if (size != segment.Count - segment.Offset) {
          OnError(new Exception(string.Format("Failed send data, expect {0} bytes send {1} bytes", segment.Count - segment.Offset, size)));
          return false;
        }
      } catch (SocketException exc) {
        if (!IsIgnorableSocketError(exc.ErrorCode))
          OnError(exc);
        if (EnsureSocketClosed())
          OnClosed();
        return false;
      } catch (Exception e) {
        if (!IsIgnorableException(e))
          OnError(e);
        if (EnsureSocketClosed())
          OnClosed();
        return false;
      }
      return true;
    }
    private void OnClosed()
    {
      var handler = Closed;

      if (handler != null)
        handler(this, EventArgs.Empty);
    }
    private void OnError(Exception e)
    {
      var handler = Error;
      if (handler == null)
        return;

      handler(this, new ErrorEventArgs(e));
    }
    private void OnConnected()
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
    private void OnDataReceived(byte[] data, int offset, int length)
    {
      var handler = DataReceived;
      if (handler == null)
        return;

      m_DataArgs.Data = data;
      m_DataArgs.Offset = offset;
      m_DataArgs.Length = length;

      handler(this, m_DataArgs);
    }
    private void DetectConnected()
    {
      if (Client != null)
        return;

      throw new Exception("The socket is not connected!", new SocketException((int)SocketError.NotConnected));
    }
  }
}
#endif