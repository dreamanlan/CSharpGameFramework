using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Lidgren.Network;
using GameFramework;
using GameFrameworkMessage;

namespace RoomServer
{
  internal class RoomPeer
  {
    private class MessageInfo : IServerConcurrentPoolAllocatedObject<MessageInfo>
    {
      internal int m_MsgId;
      internal object m_Msg;

      public void InitPool(ServerConcurrentObjectPool<MessageInfo> pool)
      {
        m_Pool = pool;
      }
      public MessageInfo Downcast()
      {
        return this;
      }
      private ServerConcurrentObjectPool<MessageInfo> m_Pool;
    }

    private IList<Observer> m_Observers = null;
    private List<RoomPeer> m_SameRoomPeerList = new List<RoomPeer>();
    private List<RoomPeer> m_CareList = new List<RoomPeer>();
    private NetConnection m_Connection;
    private object m_LockObj = new object();
    private ConcurrentQueue<MessageInfo> m_LogicQueue = new ConcurrentQueue<MessageInfo>();
    private ServerConcurrentObjectPool<MessageInfo> m_MessageInfoPool = new ServerConcurrentObjectPool<MessageInfo>();
    private uint m_Key = 0;
    private RoomPeerMgr m_PeerMgr = RoomPeerMgr.Instance;
    private long m_LastPingTime;
    private long m_EnterRoomTime;        // 进入房间的时间
    private const int m_ConnectionOverTime = 20000;
    private const int m_FirstEnterWaitTime = 45000;    //第一次接入等待时间，不计算超时

    internal void RegisterObservers(IList<Observer> observers)
    {
      m_Observers = observers;
    }

    internal uint GetKey ()
    {
      return m_Key;
    }

    internal ulong Guid { set; get; }

    internal int RoleId
    {
      set;
      get;
    }

    internal ScriptRuntime.Vector3 Position
    {
      get;
      set;
    }

    internal float FaceDir
    {
      get;
      set;
    }
    internal float WantFaceDir
    {
      get;
      set;
    }

    //这里没有加锁但是会在多个线程操作（写的时间是错开的）
    internal long EnterRoomTime
    {
      get { return m_EnterRoomTime; }
      set { m_EnterRoomTime = value; }
    }

    internal bool IsTimeout()
    {
      long current_time = TimeUtility.GetLocalMilliseconds();
      if (current_time <= m_EnterRoomTime + m_FirstEnterWaitTime) {
        return false;
      }
      if (current_time - m_LastPingTime > m_ConnectionOverTime) {
        return true;
      }
      return false;
    }

    internal long GetElapsedDroppedTime()
    {
      long time = 0;
      if (IsTimeout()) {
        long current_time = TimeUtility.GetLocalMilliseconds();
        time = current_time - m_LastPingTime - m_ConnectionOverTime;
      }
      return time;
    }

    internal bool IsConnected()
    {
      bool ret = false;
      if (null != m_Connection)
        ret = (NetConnectionStatus.Connected == m_Connection.Status);
      return ret;
    }

    internal void Disconnect()
    {
      if (null != m_Connection && NetConnectionStatus.Connected==m_Connection.Status) {
        m_Connection.Disconnect("disconnect");
        SetLastPingTime(TimeUtility.GetLocalMilliseconds() - m_ConnectionOverTime);
      }
    }

    internal void SetLastPingTime(long pingtime)
    {
      m_LastPingTime = pingtime;
    }

    internal void Init(NetConnection conn)
    {
      m_EnterRoomTime = TimeUtility.GetLocalMilliseconds();
      m_Connection = conn;
    }

    internal void Reset() {
      Disconnect();
      // this call must before other operation
      m_PeerMgr.OnPeerDestroy(this);
      m_Observers = null;
      m_LastPingTime = 0;
      m_EnterRoomTime = 0;
      m_Connection = null;
      m_Key = 0;
      ClearSameRoomPeer();
      ClearCareList();
      ClearLogicQueue();
    }

    internal NetConnection GetConnection()
    {
      return m_Connection;
    }

    internal void SendMessage(RoomMessageDefine id, object msg)
    {
      IOManager.Instance.SendPeerMessage(this, id, msg);
    }

    internal void BroadCastMsgToCareList(RoomMessageDefine id, object msg, bool exclude_me = true)
    {
      lock (m_LockObj)
      {
        if (!exclude_me)
          SendMessage(id, msg);

        foreach (RoomPeer peer in m_CareList)
        {
          if (peer.GetConnection() != null) {
            peer.SendMessage(id, msg);
          }
        }
      }
      NotifyObservers(id, msg);
    }

    internal void BroadCastMsgToRoom(RoomMessageDefine id, object msg, bool exclude_me = true)
    {
      lock (m_LockObj) {
        if (!exclude_me)
          SendMessage(id, msg);

        foreach (RoomPeer peer in m_SameRoomPeerList) {
          if (peer.GetConnection() != null) {
            peer.SendMessage(id, msg);
          }
        }
      }
      NotifyObservers(id, msg);
    }

    internal void NotifyObservers(RoomMessageDefine id, object msg)
    {
      if (null != m_Observers) {
        IList<Observer> observers = m_Observers;
        for (int i = 0; i < observers.Count; ++i) {
          Observer observer = observers[i];
          if (null!=observer && !observer.IsIdle) {
            observer.SendMessage(id, msg);
          }
        }
      }
    }
        
    internal bool SetKey(uint key)
    {
      if (m_PeerMgr.OnSetKey(key, this)) {        
        m_Key = key;
        return true;
      } else {
        return false;
      }
    }

    internal bool UpdateKey(uint key)
    {
      if (m_PeerMgr.OnUpdateKey(key, this)) {
        m_Key = key;
        return true;
      } else {
        return false;
      }
    }

    internal void AddSameRoomPeer(RoomPeer peer)
    {
      lock (m_LockObj) {
        m_SameRoomPeerList.Add(peer);
      }
    }

    internal void RemoveSameRoomPeer(RoomPeer peer)
    {
      lock (m_LockObj) {
        m_SameRoomPeerList.Remove(peer);
      }
    }

    internal void ClearSameRoomPeer()
    {
      lock (m_LockObj) {
        m_SameRoomPeerList.Clear();
      }
    }

    internal void AddCareMePeer(RoomPeer peer)
    {
      lock (m_LockObj)
      {
        m_CareList.Add(peer);
      }
    }

    internal void RemoveCareMePeer(RoomPeer peer)
    {
      lock (m_LockObj)
      {
        m_CareList.Remove(peer);
      }
    }

    internal void ClearCareList()
    {
      lock (m_LockObj) {
        m_CareList.Clear();
      }
    }

    internal void InsertLogicMsg(int id, object msg)
    {
      MessageInfo info = m_MessageInfoPool.Alloc();
      info.m_MsgId = id;
      info.m_Msg = msg;
      m_LogicQueue.Enqueue(info);
    }

    internal object PeekLogicMsg(out int id)
    {
      object msg = null;
      id = 0;
      if (m_LogicQueue.Count > 0) {
        MessageInfo info;
        if (m_LogicQueue.TryDequeue(out info)) {
          id = info.m_MsgId;
          msg = info.m_Msg;
          m_MessageInfoPool.Recycle(info);
        }
      }
      return msg;
    }

    private void ClearLogicQueue()
    {
      while (!m_LogicQueue.IsEmpty) {
        MessageInfo info;
        if (m_LogicQueue.TryDequeue(out info)) {
          m_MessageInfoPool.Recycle(info);
        }
      }
    }
  }
}
