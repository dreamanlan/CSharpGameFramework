using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFrameworkMessage;
using GameFramework;

namespace GameFramework
{
  delegate void ClientMsgHandler(object msg, User user);
  class Dispatcher
  {
    internal Dispatcher()
    {
      client_msg_handlers_ = new MyDictionary<int, ClientMsgHandler>();
    }

    internal bool RegClientMsgHandler(RoomMessageDefine msg, ClientMsgHandler handler)
    {
      if (client_msg_handlers_.ContainsKey((int)msg)) {
        return false;
      }
      client_msg_handlers_.Add((int)msg, handler);
      return true;
    }

    internal void HandleClientMsg(int id, object msg, User user)
    {
      if (msg == null) {
        LogSys.Log(LOG_TYPE.ERROR, "{0}", "can't handle null msg");
        return;
      }
      ClientMsgHandler handler;
      if (!client_msg_handlers_.TryGetValue(id, out handler))
      {
        if (client_default_handler_ != null) {
          client_default_handler_(msg, user);
        } else {
          LogSys.Log(LOG_TYPE.ERROR, "{0}", "message no deal&default handler!");
        }
        return;
      }
      if (handler != null)
      {
        handler(msg, user);
      }
    }

    internal void SetClientDefaultHandler(ClientMsgHandler handler)
    {
      client_default_handler_ = handler;
    }

    private MyDictionary<int, ClientMsgHandler> client_msg_handlers_;
    private ClientMsgHandler client_default_handler_;
  }
}
