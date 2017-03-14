using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Lidgren.Network;
using GameFramework;
using GameFrameworkMessage;

namespace GameFramework
{
  public class RoomPeer
  {
    private class MessageInfo : IServerConcurrentPoolAllocatedObject<MessageInfo>
    {
      public int m_MsgId;
      public object m_Msg;

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

    public void RegisterObservers(IList<Observer> observers)
    {
      m_Observers = observers;
    }

    public uint GetKey ()
    {
      return m_Key;
    }

    public ulong Guid { set; get; }

    public int RoleId
    {
      set;
      get;
    }

    public ScriptRuntime.Vector3 Position
    {
      get;
      set;
    }

    public float FaceDir
    {
      get;
      set;
    }
    public float WantFaceDir
    {
      get;
      set;
    }

    //这里没有加锁但是会在多个线程操作（写的时间是错开的）
    public long EnterRoomTime
    {
      get { return m_EnterRoomTime; }
      set { m_EnterRoomTime = value; }
    }

    public bool IsTimeout()
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

    public long GetElapsedDroppedTime()
    {
      long time = 0;
      if (IsTimeout()) {
        long current_time = TimeUtility.GetLocalMilliseconds();
        time = current_time - m_LastPingTime - m_ConnectionOverTime;
      }
      return time;
    }

    public bool IsConnected()
    {
      bool ret = false;
      if (null != m_Connection)
        ret = (NetConnectionStatus.Connected == m_Connection.Status);
      return ret;
    }

    public void Disconnect()
    {
      if (null != m_Connection && NetConnectionStatus.Connected==m_Connection.Status) {
        m_Connection.Disconnect("disconnect");
        SetLastPingTime(TimeUtility.GetLocalMilliseconds() - m_ConnectionOverTime);
      }
    }

    public void SetLastPingTime(long pingtime)
    {
      m_LastPingTime = pingtime;
    }

    public void Init(NetConnection conn)
    {
      m_EnterRoomTime = TimeUtility.GetLocalMilliseconds();
      m_Connection = conn;
    }

    public void Reset() {
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

    public NetConnection GetConnection()
    {
      return m_Connection;
    }

    public void SendMessage(RoomMessageDefine id, object msg)
    {
      IOManager.Instance.SendPeerMessage(this, id, msg);
    }

    public void BroadCastMsgToCareList(RoomMessageDefine id, object msg, bool exclude_me = true)
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

    public void BroadCastMsgToRoom(RoomMessageDefine id, object msg, bool exclude_me = true)
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

    public void NotifyObservers(RoomMessageDefine id, object msg)
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
        
    public bool SetKey(uint key)
    {
      if (m_PeerMgr.OnSetKey(key, this)) {        
        m_Key = key;
        return true;
      } else {
        return false;
      }
    }

    public bool UpdateKey(uint key)
    {
      if (m_PeerMgr.OnUpdateKey(key, this)) {
        m_Key = key;
        return true;
      } else {
        return false;
      }
    }

    public void AddSameRoomPeer(RoomPeer peer)
    {
      lock (m_LockObj) {
        m_SameRoomPeerList.Add(peer);
      }
    }

    public void RemoveSameRoomPeer(RoomPeer peer)
    {
      lock (m_LockObj) {
        m_SameRoomPeerList.Remove(peer);
      }
    }

    public void ClearSameRoomPeer()
    {
      lock (m_LockObj) {
        m_SameRoomPeerList.Clear();
      }
    }

    public void AddCareMePeer(RoomPeer peer)
    {
      lock (m_LockObj)
      {
        m_CareList.Add(peer);
      }
    }

    public void RemoveCareMePeer(RoomPeer peer)
    {
      lock (m_LockObj)
      {
        m_CareList.Remove(peer);
      }
    }

    public void ClearCareList()
    {
      lock (m_LockObj) {
        m_CareList.Clear();
      }
    }

    public void InsertLogicMsg(int id, object msg)
    {
      MessageInfo info = m_MessageInfoPool.Alloc();
      info.m_MsgId = id;
      info.m_Msg = msg;
      m_LogicQueue.Enqueue(info);
    }

    public object PeekLogicMsg(out int id)
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
