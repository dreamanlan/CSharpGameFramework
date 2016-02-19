using System;
using System.Collections.Generic;
using System.Text;
using RoomServer;
using GameFrameworkMessage;

namespace GameFramework
{
  internal class Observer
  {
    internal Observer()
    {
      peer_ = new RoomPeer();
      is_entered_ = false;
      is_idle_ = true;
    }

    internal void Reset()
    {
      peer_.Reset();
      user_guid_ = 0;
      user_name_ = "";
      is_entered_ = false;
      is_idle_ = true;
    }

    internal bool SetKey(uint key)
    {
      return peer_.SetKey(key);
    }
    internal uint GetKey()
    {
      return peer_.GetKey();
    }

    internal bool ReplaceDroppedObserver(ulong replacer, uint key)
    {
      Guid = replacer;
      return peer_.UpdateKey(key);
    }

    internal RoomPeer GetPeer()
    {
      return peer_;
    }

    internal bool IsConnected()
    {
      return peer_.IsConnected();
    }

    internal bool IsTimeout()
    {
      return peer_.IsTimeout();
    }

    internal void Disconnect()
    {
      peer_.Disconnect();
    }

    internal long GetElapsedDroppedTime()
    {
      return peer_.GetElapsedDroppedTime();
    }

    internal void SendMessage(RoomMessageDefine id, object msg)
    {
      if (is_entered_) {
        peer_.SendMessage(id, msg);
      }
    }

    internal void Tick()
    {
      try {
        int id = 0;
        object msg = null;
        while ((msg = peer_.PeekLogicMsg(out id)) != null) {
          //观察者只处理2个消息，进入与退出
          Msg_CR_Observer observerMsg = msg as Msg_CR_Observer;
          if (null != observerMsg) {
            IsEntered = true;
            Scene scene = OwnRoom.GetActiveScene();
            if (null != scene) {
              scene.SyncForNewObserver(this);
            }
            LogSys.Log(LOG_TYPE.DEBUG, "Msg_CR_Observer from observer {0}({1})", Guid, Name);
          } else {
            Msg_CR_Quit quitMsg = msg as Msg_CR_Quit;
            if (null != quitMsg) {
              OwnRoom.DropObserver(this);
              LogSys.Log(LOG_TYPE.DEBUG, "Msg_CR_Quit from observer {0}({1})", Guid, Name);
              break;
            } else {
              //LogSys.Log(LOG_TYPE.DEBUG, "msg {0} from observer {1}({2})", msg.GetType().Name, Guid, Name);
            }
          }
        }
      } catch (Exception ex) {
        LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
      }
    }

    internal Room OwnRoom
    {
      get { return own_room_; }
      set { own_room_ = value; }
    }
    internal string Name
    {
      set
      {
        user_name_ = value;
      }
      get
      {
        return user_name_;
      }
    }
    internal ulong Guid
    {
      set
      {
        user_guid_ = value;
        peer_.Guid = user_guid_;
      }
      get
      {
        return user_guid_;
      }
    }
    internal long EnterRoomTime
    {
      get
      {
        return peer_.EnterRoomTime;
      }
      set
      {
        peer_.EnterRoomTime = value;
      }
    }
    internal bool IsEntered
    {
      get { return is_entered_; }
      set { is_entered_ = value; }
    }
    internal bool IsIdle
    {
      get { return is_idle_; }
      set { is_idle_ = value; }
    }

    private RoomPeer peer_;
    private Room own_room_;

    private string user_name_;
    private ulong user_guid_;
    private bool is_entered_;
    private bool is_idle_;
  }
}
