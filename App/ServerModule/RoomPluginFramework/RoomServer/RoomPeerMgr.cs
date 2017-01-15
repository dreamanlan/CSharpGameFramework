using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Lidgren.Network;
using GameFrameworkMessage;

namespace GameFramework
{
  public class RoomPeerMgr
  {
#region 
    private static RoomPeerMgr s_Instance = new RoomPeerMgr();
    public static RoomPeerMgr Instance
    {
      get { return s_Instance; }
    }
#endregion
    
    public bool Init()
    {
      return true;
    }

    public bool DispatchPeerMsg(RoomPeer peer, int id, object msg)
    {
      if (peer == null) {
        return false;
      }
      peer.InsertLogicMsg(id, msg);
      return true;
    }

    public bool IsKeyExist(uint key)
    {
      return m_KeyPeer.ContainsKey(key);
    }

    public bool OnSetKey(uint key, RoomPeer peer)
    {
      if (m_KeyPeer.ContainsKey(key)) {
        return false;
      }
      if (!m_KeyPeer.TryAdd(key, peer)) {
        AddToNeedAdd(key, peer);
      }
      return true;
    }

    public bool OnUpdateKey(uint newKey, RoomPeer peer)
    {
      bool ret = false;
      uint oldKey = peer.GetKey();
      RoomPeer dummy;
      if (m_KeyPeer.TryGetValue(oldKey, out dummy)) {
        if (peer==dummy && m_KeyPeer.TryRemove(oldKey, out dummy)) {
          if (m_KeyPeer.TryAdd(newKey, peer)) {
            ret = true;
          }
        }
      }
      return ret;
    }

    public void OnPeerDestroy(RoomPeer peer)
    {
      if (peer == null) {
        return;
      }
      RoomPeer dummy = null;
      if (m_KeyPeer.TryRemove(peer.GetKey(), out dummy)) {
        if (peer.GetConnection() != null) {
          if (!m_DicPeer.TryRemove(peer.GetConnection(), out dummy)) {
            AddToNeedDelete(peer.GetConnection());
          }
        }
      } else {
        AddToNeedDelete(peer.GetKey());
      }
    }

    // 玩家连接认证，成功设置peer的连接并返回true，失败返回false
    public bool OnPeerShakeHands(uint authKey, NetConnection conn)
    {
      RoomPeer peer;
      if (m_KeyPeer.TryGetValue(authKey, out peer)) {
        if (!peer.IsConnected()) {
          if (peer.GetKey() == authKey) {
            peer.Init(conn);
            if (!m_DicPeer.TryAdd(conn, peer)) {
              AddToNeedAdd(conn, peer);
            }
            LogSys.Log(LOG_TYPE.DEBUG, "OnPeerShakeHands success, key:{0}(user:{1}) from {2}", authKey, peer.Guid, conn.RemoteEndPoint.ToString());
            return true;
          } else {
            LogSys.Log(LOG_TYPE.DEBUG, "OnPeerShakeHands failed because key error, right key is {0}(user:{1}), error key:{2} from {3}, ", peer.GetKey(), peer.Guid, authKey, conn.RemoteEndPoint.ToString());
            return false;
          }
        } else {
          LogSys.Log(LOG_TYPE.DEBUG, "OnPeerShakeHands failed because peer.IsConnected(), key:{0}(user:{1}) from {2}", authKey, peer.Guid, conn.RemoteEndPoint.ToString());
          return false;
        }
      }
      LogSys.Log(LOG_TYPE.DEBUG, "OnPeerShakeHands failed because can't find peer, key:{0} from {1}", authKey, conn.RemoteEndPoint.ToString());
      return false;
    }

    public RoomPeer GetPeerByConnection(NetConnection conn)
    {
      RoomPeer peer = null;
      m_DicPeer.TryGetValue(conn, out peer);
      return peer;
    }

    public void Tick()
    {
      try {
        if(m_NeedAddKey2Peers.Count>0){
          lock (m_LockObj) {
            foreach (KeyValuePair<uint, RoomPeer> pair in m_NeedAddKey2Peers) {
              if (m_KeyPeer.ContainsKey(pair.Key) || m_KeyPeer.TryAdd(pair.Key, pair.Value)) {
                m_DeletedAddKeys.Add(pair.Key);
              }
            }
          }
        }
        if(m_NeedAddConnection2Peers.Count>0){
          lock (m_LockObj) {
            foreach (KeyValuePair<NetConnection, RoomPeer> pair in m_NeedAddConnection2Peers) {
              if (m_DicPeer.ContainsKey(pair.Key) || m_DicPeer.TryAdd(pair.Key, pair.Value)) {
                m_DeletedAddConnections.Add(pair.Key);
              }
            }
          }
        }
        if(m_DeletedAddKeys.Count>0){
          lock (m_LockObj) {
            foreach (uint key in m_DeletedAddKeys) {
              m_NeedAddKey2Peers.Remove(key);
            }
            m_DeletedAddKeys.Clear();
          }
        }
        if (m_DeletedAddConnections.Count > 0) {
          lock (m_LockObj) {
            foreach (NetConnection conn in m_DeletedAddConnections) {
              m_NeedAddConnection2Peers.Remove(conn);
            }
            m_DeletedAddConnections.Clear();
          }
        }
        if (m_NeedDeleteKeys.Count > 0) {
          lock (m_LockObj) {
            foreach (uint key in m_NeedDeleteKeys) {
              RoomPeer peer = null;
              if (!m_KeyPeer.ContainsKey(key)) {
                m_DeletedKeys.Add(key);
              } else if (m_KeyPeer.TryRemove(key, out peer)) {
                m_DeletedKeys.Add(key);
                if (null != peer) {
                  m_NeedDeleteConnections.Add(peer.GetConnection());
                }
              }
            }
          }
        }
        if (m_NeedDeleteConnections.Count > 0) {
          lock (m_LockObj) {
            foreach (NetConnection conn in m_NeedDeleteConnections) {
              RoomPeer peer = null;
              if (!m_DicPeer.ContainsKey(conn)) {
                m_DeletedConnections.Add(conn);
              } else if (m_DicPeer.TryRemove(conn, out peer)) {
                m_DeletedConnections.Add(conn);
              }
            }
          }
        }
        if (m_DeletedKeys.Count > 0) {
          lock (m_LockObj) {
            foreach (uint key in m_DeletedKeys) {
              m_NeedDeleteKeys.Remove(key);
            }
            m_DeletedKeys.Clear();
          }
        }
        if (m_DeletedConnections.Count > 0) {
          lock (m_LockObj) {
            foreach (NetConnection conn in m_DeletedConnections) {
              m_NeedDeleteConnections.Remove(conn);
            }
            m_DeletedConnections.Clear();
          }
        }
      } catch (Exception ex) {
        LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
      }
    }

    private void AddToNeedAdd(uint key, RoomPeer peer)
    {
      lock (m_LockObj) {
        if (!m_NeedAddKey2Peers.ContainsKey(key)) {
          m_NeedAddKey2Peers.Add(key, peer);
        }
      }
    }

    private void AddToNeedAdd(NetConnection conn, RoomPeer peer)
    {
      lock (m_LockObj) {
        if (!m_NeedAddConnection2Peers.ContainsKey(conn)) {
          m_NeedAddConnection2Peers.Add(conn, peer);
        }
      }
    }

    private void AddToNeedDelete(uint key)
    {
      lock (m_LockObj) {
        if (!m_NeedDeleteKeys.Contains(key))
          m_NeedDeleteKeys.Add(key);
      }
    }

    private void AddToNeedDelete(NetConnection conn)
    {
      lock (m_LockObj) {
        if (!m_NeedDeleteConnections.Contains(conn))
          m_NeedDeleteConnections.Add(conn);
      }
    }

    //允许跨线程访问不加锁的数据
    private ConcurrentDictionary<NetConnection, RoomPeer> m_DicPeer = new ConcurrentDictionary<NetConnection, RoomPeer>();
    private ConcurrentDictionary<uint, RoomPeer> m_KeyPeer = new ConcurrentDictionary<uint, RoomPeer>();
    //跨线程访问要加锁的数据
    private object m_LockObj = new object();
    private MyDictionary<uint, RoomPeer> m_NeedAddKey2Peers = new MyDictionary<uint, RoomPeer>();
    private MyDictionary<NetConnection, RoomPeer> m_NeedAddConnection2Peers = new MyDictionary<NetConnection, RoomPeer>();
    private List<uint> m_DeletedAddKeys = new List<uint>();
    private List<NetConnection> m_DeletedAddConnections = new List<NetConnection>();
    private HashSet<uint> m_NeedDeleteKeys = new HashSet<uint>();
    private HashSet<NetConnection> m_NeedDeleteConnections = new HashSet<NetConnection>();
    private List<uint> m_DeletedKeys = new List<uint>();
    private List<NetConnection> m_DeletedConnections = new List<NetConnection>();
  } // end class
}
