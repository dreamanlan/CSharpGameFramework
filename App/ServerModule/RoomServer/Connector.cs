using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpCenterClient;
using Messenger;

namespace GameFramework
{
    public class Connector
    {
        public Connector(PBChannel channel)
        {
            channel_ = channel;
        }

        public void SendMsgToLobby(object msg)
        {
            channel_.Send(msg);
        }

        private PBChannel channel_;
    }
}  // namespace GameFramework
