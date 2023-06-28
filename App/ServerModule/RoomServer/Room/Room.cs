using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using GameFrameworkMessage;

namespace GameFramework
{
    public enum RoomState
    {
        Unuse,
        Active,
        Finish,
        Deactive,
    }

    public class Room
    {
        public bool CanFinish
        {
            get { return m_CanFinish || m_ActiveTime+c_WaitUserTime<TimeUtility.GetLocalMilliseconds(); }
        }
        public RoomState CurrentState
        {
            get { return m_RoomState; }
            set { m_RoomState = value; }
        }
        public bool IsFieldRoom
        {
            get { return m_RoomUserMgr.IsFieldRoom; }
            set { m_RoomUserMgr.IsFieldRoom = value; }
        }
        public Scene ActiveScene
        {
            get { return m_RoomUserMgr.ActiveScene; }
        }
        public List<User> RoomUsers
        {
            get { return m_RoomUserMgr.RoomUsers; }
        }
        public Observer[] RoomObservers
        {
            get { return m_RoomUserMgr.RoomObservers; }
        }
        public int RoomUserCount
        {
            get
            {
                return m_RoomUserMgr.RoomUserCount;
            }
        }
        public int RoomId
        {
            get
            {
                return m_RoomId;
            }
        }
        public uint LocalID
        { 
            get { return m_LocalID; }
            set { m_LocalID = value; }
        }
        public bool IsIdle
        {
            get { return m_IsIdle; }
            set { m_IsIdle = value; }
        }
        public bool CanClose
        {
            get
            {
                return m_CanCloseTime > 0 && m_CanCloseTime + c_CloseWaitTime < TimeUtility.GetLocalMilliseconds();
            }
        }
        public ScenePool ScenePool
        {
            get { return m_ScenePool; }
            set { m_ScenePool = value; }
        }
        public int GetAiRoomUserCount()
        {
            int count = 0;
            foreach (User us in m_RoomUserMgr.RoomUsers) {
                if ((int)UserControlState.Ai == us.UserControlState) {
                    ++count;
                }
            }
            return count;
        }
        public int GetActiveRoomUserCount()
        {
            int count = 0;
            foreach (User us in m_RoomUserMgr.RoomUsers) {
                if ((int)UserControlState.User == us.UserControlState) {
                    ++count;
                }
            }
            return count;
        }
        public long GetMinimizeElapsedDroppedTime()
        {
            long time = long.MaxValue;
            foreach (User us in m_RoomUserMgr.RoomUsers) {
                if (null != us && (int)UserControlState.UserDropped == us.UserControlState) {
                    long _time = us.GetElapsedDroppedTime();
                    if (time > _time)
                        time = _time;
                }
            }
            return time;
        }
        public User GetUserByGuid(ulong guid)
        {
            foreach (User us in m_RoomUserMgr.RoomUsers) {
                if (us != null && us.Guid == guid) {
                    return us;
                }
            }
            return null;
        }
        public User GetUserByRoleID(int roleId)
        {
            foreach (User us in m_RoomUserMgr.RoomUsers) {
                if (us != null && us.RoleId == roleId) {
                    return us;
                }
            }
            return null;
        }
        public bool Init(int room_id, int scene_type, UserPool userpool, Connector conn)
        {
            LogSys.Log(ServerLogType.INFO, "[0] Room.Init {0} scene {1}", room_id, scene_type);
            m_RoomId = room_id;
            m_UserPool = userpool;
            m_Connector = conn;
            m_CanCloseTime = 0;
            m_RoomUserMgr.Connector = conn;
            m_RoomUserMgr.RoomId = room_id;
            m_RoomUserMgr.LocalRoomId = (int)m_LocalID;
            m_RoomUserMgr.UserPool = userpool;
            m_RoomUserMgr.ActiveScene = m_ScenePool.NewScene();
            LogSys.Log(ServerLogType.INFO, "[1] Room.Init {0} scene {1}", room_id, scene_type);
            m_RoomUserMgr.ActiveScene.SetRoomUserManager(m_RoomUserMgr);
            m_RoomUserMgr.ActiveScene.Init(scene_type);
            //场景数据加载由加载线程执行（注：场景没有加载完成，场景状态仍然是sleep，Scene.Tick不会有实际的动作）
            SceneLoadThread.Instance.QueueAction(m_RoomUserMgr.ActiveScene.LoadData, scene_type);
            OnInit();
            m_ActiveTime = TimeUtility.GetLocalMilliseconds();
            CurrentState = RoomState.Active;
            m_CanFinish = false;
            LogSys.Log(ServerLogType.DEBUG, "Room Initialize: {0}  Scene: {1}", room_id, scene_type);
            return true;
        }
        public void Destroy()
        {
            LogSys.Log(ServerLogType.INFO, "room {0}({1}) destroy.", RoomId, LocalID);
            OnDestroy();
            m_RoomUserMgr.ActiveScene.Reset();
            m_ScenePool.RecycleScene(m_RoomUserMgr.ActiveScene);
            m_RoomUserMgr.ActiveScene = null;

            this.CurrentState = RoomState.Unuse;
            int userCt = m_RoomUserMgr.RoomUsers.Count;
            for (int i = userCt - 1; i >= 0; --i) {
                User user = m_RoomUserMgr.RoomUsers[i];
                if (null != user) {
                    LogSys.Log(ServerLogType.INFO, "FreeUser {0} for {1} {2}, [Room.Destroy]", user.LocalID, user.Guid, user.GetKey());
                    user.Reset();
                    m_UserPool.FreeUser(user.LocalID);
                    m_RoomUserMgr.RoomUsers.RemoveAt(i);
                }
            }
            for (int i = 0; i < m_RoomUserMgr.RoomObservers.Length; ++i) {
                m_RoomUserMgr.RoomObservers[i].Reset();
            }
        }
        public void ChangeScene(int scene_type)
        {
            m_RoomUserMgr.ActiveScene.Reset();
            m_ScenePool.RecycleScene(m_RoomUserMgr.ActiveScene);
            m_RoomUserMgr.ActiveScene = null;

            foreach (User us in m_RoomUserMgr.RoomUsers) {
                us.IsEntered = false;
            }

            m_CanCloseTime = 0;
            m_RoomUserMgr.ActiveScene = m_ScenePool.NewScene();
            m_RoomUserMgr.ActiveScene.SetRoomUserManager(m_RoomUserMgr);
            //场景数据加载由加载线程执行（注：场景没有加载完成，场景状态仍然是sleep，Scene.Tick不会有实际的动作）
            SceneLoadThread.Instance.QueueAction(m_RoomUserMgr.ActiveScene.LoadData, scene_type);

            OnChangeScene();

            Msg_RC_ChangeScene msg = new Msg_RC_ChangeScene();
            msg.target_scene_id = scene_type;
            foreach (User us in m_RoomUserMgr.RoomUsers) {
                us.SendMessage(RoomMessageDefine.Msg_RC_ChangeScene, msg);
            }

            LogSys.Log(ServerLogType.INFO, "Room.ChangeScene {0} scene {1}", m_RoomId, scene_type);
        }

        public void Tick()
        {
            try {
                if (this.CurrentState != RoomState.Active && this.CurrentState != RoomState.Finish)
                    return;

                long curTime = TimeUtility.GetLocalMilliseconds();
                if (m_LastLogTime + 60000 < curTime) {
                    m_LastLogTime = curTime;

                    LogSys.Log(ServerLogType.INFO, "Room.Tick {0}", RoomId);
                }

                if (this.CurrentState == RoomState.Active) {
                    Scene scene = ActiveScene;
                    if (null != scene) {
                        scene.Tick();
                        OnTick();
                    }
                    m_DisconnectedUsers.Clear();
                    m_RequestDeleteUsers.Clear();
                    foreach (User user in m_RoomUserMgr.RoomUsers) {
                        if (user != null) {
                            user.Tick();
                            if (user.IsTimeout()) {
                                if (user.UserControlState == (int)UserControlState.User) {
                                    m_DisconnectedUsers.Add(user);
                                } else if (user.UserControlState == (int)UserControlState.Remove) {
                                    m_RequestDeleteUsers.Add(user);
                                } else if (user.UserControlState == (int)UserControlState.UserDropped) {
                                    if (user.LastNotifyUserDropTime + c_NotifyUserDropInterval < curTime) {
                                        Msg_RL_UserDrop uaqBuilder = new Msg_RL_UserDrop();
                                        uaqBuilder.RoomId = m_RoomId;
                                        uaqBuilder.UserGuid = user.Guid;
                                        uaqBuilder.IsBattleEnd = false;
                                        m_Connector.SendMsgToLobby(uaqBuilder);
                                        
                                        user.LastNotifyUserDropTime = curTime;
                                    }
                                    //临时处理，踢掉断线的玩家
                                    user.UserControlState = (int)UserControlState.Remove;
                                }
                            }
                        }
                    }
                    foreach (User user in m_DisconnectedUsers) {
                        m_RoomUserMgr.DropUser(user);
                    }
                    foreach (User user in m_RequestDeleteUsers) {
                        m_RoomUserMgr.RemoveUser(user);
                    }
                    //todo:观察者掉线处理
                    for (int i = 0; i < m_RoomUserMgr.RoomObservers.Length; ++i) {
                        Observer observer = m_RoomUserMgr.RoomObservers[i];
                        if (!observer.IsIdle) {
                            observer.Tick();
                        }
                    }
                    if (!IsFieldRoom) {
                        int userCount = GetActiveRoomUserCount();
                        if (userCount <= 0 && CanFinish) {
                            if (GetMinimizeElapsedDroppedTime() > c_FinishTimeForNoUsers) {
                                //若房间内玩家数目为0，结束战斗，关闭房间
                                Finish((int)CampIdEnum.Unkown);
                            }
                        }
                    }
                    //每个Tick结束，将空间属性同步给Peer，用于Peer转发消息
                    foreach (User user in m_RoomUserMgr.RoomUsers) {
                        if (null != user && null != user.Info && null != user.Info.GetMovementStateInfo()) {
                            GameFramework.RoomPeer peer = user.GetPeer();
                            if (null != peer) {
                                MovementStateInfo info = user.Info.GetMovementStateInfo();
                                peer.Position = info.GetPosition3D();
                                peer.FaceDir = info.GetFaceDir();
                            }
                        }
                    }
                } else if (m_FinishTime + c_DeactiveWaitTime < curTime) {
                    Deactive();
                }
            } catch (Exception ex) {
                LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public bool AddNewUser(User newUser)
        {
            bool ret = m_RoomUserMgr.AddNewUser(newUser);
            if (ret) {
                m_CanCloseTime = 0;
                m_CanFinish = true;
            }
            return ret;
        }
        public void RemoveUserFromRoomThread(User user, bool free)
        {
            m_RoomUserMgr.RemoveUser(user, free);
        }
        public void DeleteUser(User user)
        {
            m_RoomUserMgr.DeleteUser(user);
        }
        public void Finish(int winnerCampID)
        {
            if (IsFieldRoom) {
                return;
            }
            if (this.CurrentState == RoomState.Finish || this.CurrentState == RoomState.Deactive) {
                return;
            }
            foreach (User user in m_RoomUserMgr.RoomUsers) {
                if (user != null) {
                    Msg_RL_UserDrop unqBuilder = new Msg_RL_UserDrop();
                    unqBuilder.RoomId = m_RoomId;
                    unqBuilder.UserGuid = user.Guid;
                    unqBuilder.IsBattleEnd = true;
                    m_Connector.SendMsgToLobby(unqBuilder);
                    user.LastNotifyUserDropTime = TimeUtility.GetLocalMilliseconds();
                }
            }
            this.CurrentState = RoomState.Finish;
            m_FinishTime = TimeUtility.GetLocalMilliseconds();
            LogSys.Log(ServerLogType.DEBUG, "Room {0}({1}) EndBattle.", RoomId, LocalID);
        }        
        public void NoticeRoomClosing()
        {
            foreach (User user in m_RoomUserMgr.RoomUsers) {
                if (user != null && (int)UserControlState.UserDropped == user.UserControlState) {
                    Msg_RL_UserDrop unqBuilder = new Msg_RL_UserDrop();
                    unqBuilder.RoomId = m_RoomId;
                    unqBuilder.UserGuid = user.Guid;
                    unqBuilder.IsBattleEnd = true;
                    m_Connector.SendMsgToLobby(unqBuilder);
                    user.LastNotifyUserDropTime = TimeUtility.GetLocalMilliseconds();
                }
            }
        }
        public Room()
        {
            m_Dispatcher = new Dispatcher();
            m_DisconnectedUsers = new List<User>();
            m_RequestDeleteUsers = new List<User>();
            m_CanCloseTime = 0;

            m_RoomUserMgr = new RoomUserManager();
        }

        private void Deactive()
        {
            //准备关闭房间
            for (int index = m_RoomUserMgr.RoomUsers.Count - 1; index >= 0; --index) {
                User user = m_RoomUserMgr.RoomUsers[index];
                m_RoomUserMgr.RemoveUser(user);
            }
            this.CurrentState = RoomState.Deactive;
            m_CanCloseTime = TimeUtility.GetLocalMilliseconds();
            LogSys.Log(ServerLogType.DEBUG, "Room {0}({1}) Deactive.", RoomId, LocalID);
        }

        private void OnInit()
        {
            var scene = ActiveScene;
            if (null != scene) {
            }
        }
        private void OnChangeScene()
        {
            var scene = ActiveScene;
            if (null != scene) {
            }
        }
        private void OnTick()
        {
            var scene = ActiveScene;
            if (null != scene) {
            }
        }
        private void OnDestroy()
        {
        }
        
        //-------------------------------------------------------------------------
        private const long c_CloseWaitTime = 1000;
        private const long c_FinishTimeForNoUsers = 1000;
        private int m_RoomId;
        private Dispatcher m_Dispatcher;
        private RoomUserManager m_RoomUserMgr;

        private UserPool m_UserPool;
        private ScenePool m_ScenePool = null;

        private List<User> m_DisconnectedUsers;
        private List<User> m_RequestDeleteUsers;
        private Connector m_Connector;
        private long m_CanCloseTime;
        private bool m_CanFinish = false;

        private const long c_WaitUserTime = 60000;
        private const long c_NotifyUserDropInterval = 20000;
        private const long c_DeactiveWaitTime = 3000;
        private long m_ActiveTime = 0;
        private long m_FinishTime = 0;
        private long m_LastLogTime = 0;

        private RoomState m_RoomState = RoomState.Unuse;
        private uint m_LocalID = 0;
        private bool m_IsIdle = false;
    }
}
