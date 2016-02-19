using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpCenterClient;
using Messenger;

namespace GameFramework
{
  internal class Connector
  {
    internal Connector(PBChannel channel)
    {
      channel_ = channel;
    }

    internal void SendMsgToLobby(object msg)
    {
      channel_.Send(msg);
    }
    
    private PBChannel channel_;
  }
}  // namespace GameFramework
