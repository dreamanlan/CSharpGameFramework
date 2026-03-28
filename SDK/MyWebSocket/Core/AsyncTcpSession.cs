#if !USE_IL2CPP
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace SuperSocket.ClientEngine
{
  public class AsyncTcpSession : TcpClientSession
  {
    public AsyncTcpSession(EndPoint remoteEndPoint)
      : base(remoteEndPoint)
    {
    }
    public AsyncTcpSession(EndPoint remoteEndPoint, int receiveBufferSize)
      : base(remoteEndPoint, receiveBufferSize)
    {
    }
    //==========================================================
    //计数器
    //当异步网络连接成功时
    protected override void OnGetSocket(SocketAsyncArgs e)
    {
      if (Buffer.Array == null) {
        Buffer = new ArraySegment<byte>(new byte[ReceiveBufferSize], 0, ReceiveBufferSize);
      }
      e.SetBuffer(Buffer.Array, Buffer.Offset, Buffer.Count);
      OnConnected();

      //开始接收消息
      StartReceive(e);
    }
    //异步请求发送消息
    protected override void SendInternal(ArraySegment<byte> segment)
    {
      try {
        if (null == Client) {
          OnError(new Exception("socket is null"));
          return;
        }
        NetworkStream ns;
        lock (Client) {
          ns = new NetworkStream(Client);
        }
        if (ns.CanWrite) {
          ns.BeginWrite(segment.Array, segment.Offset, segment.Count, new System.AsyncCallback(Sending_Completed), ns);
        }
      } catch (SocketException exc) {
        if (!IsIgnorableSocketError(exc.ErrorCode))
          OnError(exc);
        if (EnsureSocketClosed())
          OnClosed();
        return;
      } catch (Exception e) {
        if (!IsIgnorableException(e))
          OnError(e);
        if (EnsureSocketClosed())
          OnClosed();
        return;
      }
    }
    //异步请求接收消息
    private void StartReceive(SocketAsyncArgs e)
    {
      var client = Client;
      if (client == null)
        return;
      try {
        client.BeginReceive(e.Buffer, e.Offset, e.Count, 0, new AsyncCallback(ReceiveCallback), e);
      } catch (SocketException exc) {
        if (!IsIgnorableSocketError(exc.ErrorCode))
          OnError(exc);
        if (EnsureSocketClosed(client))
          OnClosed();
        return;
      } catch (Exception ex) {
        if (!IsIgnorableException(ex))
          OnError(ex);
        if (EnsureSocketClosed(client))
          OnClosed();
        return;
      }
    }
    //异步接收消息回调方法
    private void ReceiveCallback(IAsyncResult ar)
    {
      SocketAsyncArgs e = (SocketAsyncArgs)ar.AsyncState;
      if (e.SocketError != SocketError.Success) {
        if (!IsIgnorableSocketError((int)e.SocketError))
          OnError(new SocketException((int)e.SocketError));
        if (EnsureSocketClosed())
          OnClosed();
        return;
      }
      var client = Client;
      if (client == null)
        return;
      //接收数据
      try {
        e.BytesTransferred = e.AcceptSocket.EndReceive(ar);
        if (e.BytesTransferred == 0) {
          if (EnsureSocketClosed())
            OnClosed();
          return;
        }
        OnDataReceived(e.Buffer, e.Offset, e.BytesTransferred);
      } catch (SocketException exc) {
        if (!IsIgnorableSocketError(exc.ErrorCode))
          OnError(exc);
        if (EnsureSocketClosed())
          OnClosed();
        return;
      } catch (Exception ex) {
        if (!IsIgnorableException(ex))
          OnError(ex);
        if (EnsureSocketClosed())
          OnClosed();
        return;
      }
      //下一轮接收数据
      StartReceive(e);
    }
    //异步发送消息回调方法
    private void Sending_Completed(System.IAsyncResult ar)
    {
      NetworkStream ns = (NetworkStream)ar.AsyncState;
      try {
        ns.EndWrite(ar);
        ns.Flush();
        ns.Close();
      } catch (System.Exception ex) {
        if (EnsureSocketClosed() && !IsIgnorableException(ex))
          OnError(ex);
        return;
      }
      DequeueSend();
    }
  }
}
#endif