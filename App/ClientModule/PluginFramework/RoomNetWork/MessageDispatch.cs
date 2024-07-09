using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Lidgren.Network;
using GameFrameworkMessage;

namespace ScriptableFramework.Network
{
  class MessageDispatch
  {
    internal delegate void MsgHandler(object msg, NetConnection user);
    MyDictionary<int, MsgHandler> m_DicHandler = new MyDictionary<int, MsgHandler>();
    internal void RegisterHandler(RoomMessageDefine id, MsgHandler handler)
    {
      m_DicHandler[(int)id] = handler;
    }
    internal bool Dispatch(int id, object msg, NetConnection conn)
    {
      MsgHandler msghandler;
      if (m_DicHandler.TryGetValue(id, out msghandler))
      {
        msghandler(msg, conn);
        return true;
      }
      return false;
    }
  }
}
