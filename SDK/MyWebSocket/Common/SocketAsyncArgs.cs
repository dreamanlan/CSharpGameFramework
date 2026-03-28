using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace SuperSocket.ClientEngine
{
  public delegate void ConnectedCallback(Socket socket, object state, SocketAsyncArgs e);
  class DnsConnectState
  {
    public IPAddress[] Addresses { get; set; }

    public int NextAddressIndex { get; set; }

    public int Port { get; set; }

    public Socket Socket4 { get; set; }

    public Socket Socket6 { get; set; }

    public object State { get; set; }

    public ConnectedCallback Callback { get; set; }
  }
  public class SocketAsyncArgs
  {
    public SocketAsyncArgs()
    {	
			AcceptSocket = null;
      RemoteEndPoint = null;
      State = null;
      Callback = null;
			Buffer = null;		
			BytesTransferred = 0;
			Count = 0;			
			Offset = 0;			
			SocketError = SocketError.Success;
    }
    public Socket AcceptSocket { get; set; }
    public EndPoint RemoteEndPoint { get; set; }
    public object State { get; set; }
    public ConnectedCallback Callback { get; set; }
    public SocketError SocketError { get; set; }
    public byte[] Buffer { get; set; }
    public int Count { get; set; }
    public int Offset { get; set; }
    public int BytesTransferred { get;  set; }
    public void SetBuffer(byte[] buffer, int offset, int count)
    {
      if (buffer != null)
      {
        int buflen = buffer.Length;
        if (offset < 0 || (offset != 0 && offset >= buflen))
          throw new ArgumentOutOfRangeException("offset");

        if (count < 0 || count > buflen - offset)
          throw new ArgumentOutOfRangeException("count");

        Count = count;
        Offset = offset;
      }
      Buffer = buffer;
    }    
  }
}
