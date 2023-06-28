using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameFrameworkMessage;

namespace GameFramework
{
  public class Observer
  {
    public Observer()
    {
      m_Peer = new RoomPeer();
      m_IsEntered = false;
      m_IsIdle = true;
    }

    public void Reset()
    {
      m_Peer.Reset();
      m_UserGuid = 0;
      m_UserName = "";
      m_IsEntered = false;
      m_IsIdle = true;
    }

    public bool SetKey(uint key)
    {
      return m_Peer.SetKey(key);
    }
    public uint GetKey()
    {
      return m_Peer.GetKey();
    }

    public bool ReplaceDroppedObserver(ulong replacer, uint key)
    {
      Guid = replacer;
      return m_Peer.UpdateKey(key);
    }

    public RoomPeer GetPeer()
    {
      return m_Peer;
    }

    public bool IsConnected()
    {
      return m_Peer.IsConnected();
    }

    public bool IsTimeout()
    {
      return m_Peer.IsTimeout();
    }

    public void Disconnect()
    {
      m_Peer.Disconnect();
    }

    public long GetElapsedDroppedTime()
    {
      return m_Peer.GetElapsedDroppedTime();
    }

    public void SendMessage(RoomMessageDefine id, object msg)
    {
      if (m_IsEntered) {
        m_Peer.SendMessage(id, msg);
      }
    }

    public void Tick()
    {
      try {
        int id = 0;
        object msg = null;
        while ((msg = m_Peer.PeekLogicMsg(out id)) != null) {
          //观察者只处理2个消息，进入与退出
          Msg_CR_Observer observerMsg = msg as Msg_CR_Observer;
          if (null != observerMsg) {
            IsEntered = true;
            Scene scene = OwnRoomUserManager.ActiveScene;
            if (null != scene) {
              scene.SyncForNewObserver(this);
            }
            LogSys.Log(ServerLogType.DEBUG, "Msg_CR_Observer from observer {0}({1})", Guid, Name);
          } else {
            Msg_CR_Quit quitMsg = msg as Msg_CR_Quit;
            if (null != quitMsg) {
              OwnRoomUserManager.DropObserver(this);
              LogSys.Log(ServerLogType.DEBUG, "Msg_CR_Quit from observer {0}({1})", Guid, Name);
              break;
            } else {
              //LogSys.Log(LOG_TYPE.DEBUG, "msg {0} from observer {1}({2})", msg.GetType().Name, Guid, Name);
            }
          }
        }
      } catch (Exception ex) {
        LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
      }
    }

    public RoomUserManager OwnRoomUserManager
    {
      get { return m_OwnRoomUserMgr; }
      set { m_OwnRoomUserMgr = value; }
    }
    public string Name
    {
      set
      {
        m_UserName = value;
      }
      get
      {
        return m_UserName;
      }
    }
    public ulong Guid
    {
      set
      {
        m_UserGuid = value;
        m_Peer.Guid = m_UserGuid;
      }
      get
      {
        return m_UserGuid;
      }
    }
    public long EnterRoomTime
    {
      get
      {
        return m_Peer.EnterRoomTime;
      }
      set
      {
        m_Peer.EnterRoomTime = value;
      }
    }
    public bool IsEntered
    {
      get { return m_IsEntered; }
      set { m_IsEntered = value; }
    }
    public bool IsIdle
    {
      get { return m_IsIdle; }
      set { m_IsIdle = value; }
    }

    private RoomPeer m_Peer;
    private RoomUserManager m_OwnRoomUserMgr;

    private string m_UserName;
    private ulong m_UserGuid;
    private bool m_IsEntered;
    private bool m_IsIdle;
  }
}
