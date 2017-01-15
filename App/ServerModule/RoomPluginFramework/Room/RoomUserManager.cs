using System;
using System.Collections.Generic;
using GameFrameworkMessage;

namespace GameFramework
{
    public class RoomUserManager
    {
        public UserPool UserPool
        {
            get { return m_UserPool; }
            set { m_UserPool = value; }
        }
        public Connector Connector
        {
            get { return m_Connector; }
            set { m_Connector = value; }
        }
        public int RoomId
        {
            get
            {
                return m_RoomId;
            }
            set
            {
                m_RoomId = value;
            }
        }
        public int LocalRoomId
        {
            get { return m_LocalRoomId; }
            set { m_LocalRoomId = value; }
        }
        public bool IsFieldRoom
        {
            get { return m_IsFieldRoom; }
            set { m_IsFieldRoom = value; }
        }
        public Scene ActiveScene
        {
            get { return m_ActiveScene; }
            set { m_ActiveScene = value; }
        }
        public List<User> RoomUsers
        {
            get { return m_RoomUsers; }
        }
        public Observer[] RoomObservers
        {
            get { return m_RoomObservers; }
        }
        public int RoomUserCount
        {
            get
            {
                return m_RoomUsers.Count;
            }
        }
        public int GetAiRoomUserCount()
        {
            int count = 0;
            foreach (User us in m_RoomUsers) {
                if ((int)UserControlState.Ai == us.UserControlState) {
                    ++count;
                }
            }
            return count;
        }
        public int GetActiveRoomUserCount()
        {
            int count = 0;
            foreach (User us in m_RoomUsers) {
                if ((int)UserControlState.User == us.UserControlState) {
                    ++count;
                }
            }
            return count;
        }
        public long GetMinimizeElapsedDroppedTime()
        {
            long time = long.MaxValue;
            foreach (User us in m_RoomUsers) {
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
            foreach (User us in m_RoomUsers) {
                if (us != null && us.Guid == guid) {
                    return us;
                }
            }
            return null;
        }
        public User GetUserByRoleID(int roleId)
        {
            foreach (User us in m_RoomUsers) {
                if (us != null && us.RoleId == roleId) {
                    return us;
                }
            }
            return null;
        }

        public void PickMoney(User user, int money)
        {
            Msg_RL_PickMoney builder = new Msg_RL_PickMoney();
            builder.UserGuid = user.Guid;
            builder.Num = money;
            m_Connector.SendMsgToLobby(builder);
        }
        public void PickItem(User user, int itemId, string model, string particle)
        {
            Msg_RL_PickItem builder = new Msg_RL_PickItem();
            builder.UserGuid = user.Guid;
            builder.RoomId = RoomId;
            builder.ItemId = itemId;
            builder.Model = model;
            builder.Particle = particle;
            m_Connector.SendMsgToLobby(builder);
        }
        public void SendServerMessage(object msg)
        {
            m_Connector.SendMsgToLobby(msg);
        }
        public void PlayerRequestActiveRoom(int targetSceneId, params ulong[] guids)
        {
            Msg_RL_ActiveScene builder = new Msg_RL_ActiveScene();
            builder.UserGuids.AddRange(guids);
            builder.SceneId = targetSceneId;
            m_Connector.SendMsgToLobby(builder);
        }
        public void PlayerRequestChangeRoom(int targetSceneId, params ulong[] guids)
        {
            Msg_RL_ChangeScene builder = new Msg_RL_ChangeScene();
            builder.UserGuids.AddRange(guids);
            builder.SceneId = targetSceneId;
            m_Connector.SendMsgToLobby(builder);
        }

        public bool AddNewUser(User newUser)
        {
            foreach (User us in m_RoomUsers) {
                if (us != null && us.Guid == newUser.Guid) {
                    //当前玩家已在游戏房间内
                    if (us.GetKey() == newUser.GetKey()) {
                        LogSys.Log(LOG_TYPE.DEBUG, "Add user success: User already in the room! RoomId:{0}, Guid:{1}, OldUser[{2}]({3}) NewUser[{4}]({5}) ",
                        m_RoomId, us.Guid, us.LocalID, us.GetKey(), newUser.LocalID, newUser.GetKey());
                        LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for new {1} {2}, [Room.AddNewUser]", newUser.LocalID, newUser.Guid, newUser.GetKey());
                        m_UserPool.FreeUser(newUser.LocalID);
                        return true;
                    } else if (us.UserControlState != (int)UserControlState.User) {
                        LogSys.Log(LOG_TYPE.DEBUG, "Add user success: User already in the room! RoomId:{0}, Guid:{1}, OldUser[{2}]({3}) NewUser[{4}]({5}) ",
                        m_RoomId, us.Guid, us.LocalID, us.GetKey(), newUser.LocalID, newUser.GetKey());
                        LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for old {1} {2}, [Room.AddNewUser]", us.LocalID, us.Guid, us.GetKey());
                        RemoveUser(us);
                        break;
                    } else {
                        LogSys.Log(LOG_TYPE.DEBUG, "Add user false: User already in the room and online! RoomId:{0}, Guid:{1}, OldUser[{2}]({3}) NewUser[{4}]({5}) ",
                        m_RoomId, us.Guid, us.LocalID, us.GetKey(), newUser.LocalID, newUser.GetKey());
                        LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for new {1} {2}, [Room.AddNewUser]", newUser.LocalID, newUser.Guid, newUser.GetKey());
                        m_UserPool.FreeUser(newUser.LocalID);
                        return false;
                    }
                }
            }
            newUser.EnterRoomTime = TimeUtility.GetLocalMilliseconds();
            newUser.OwnRoomUserManager = this;
            newUser.RegisterObservers(m_RoomObservers);
            newUser.CharacterCreateTime = TimeUtility.GetLocalMilliseconds();
            newUser.TimeCounter = 0;
            if ((int)UserControlState.Ai == newUser.UserControlState) {
                newUser.IsEntered = true;
            }
            if (null != m_ActiveScene && m_ActiveScene.SceneState == SceneState.Running) {
                Scene scene = m_ActiveScene;
                scene.EnterScene(newUser);
            }
            foreach (User otheruser in m_RoomUsers) {
                if (otheruser != null) {
                    otheruser.AddSameRoomUser(newUser);
                    newUser.AddSameRoomUser(otheruser);
                }
            }
            m_RoomUsers.Add(newUser);
            LogSys.Log(LOG_TYPE.DEBUG, "Add user success ! RoomId:{0} , UserGuid:{1}({2})",
              m_RoomId, newUser.Guid, newUser.GetKey());
            return true;
        }
        public void DeleteUser(User user)
        {
            if (null != user) {
                user.UserControlState = (int)UserControlState.Remove;
                if (null != user.Info) {
                    //user.Info.Suicide();
                }
                LogSys.Log(LOG_TYPE.DEBUG, "Room {0} User {1}({2}) deleted.", RoomId, user.Guid, user.GetKey());
            }
        }
        public void DropUser(User user)
        {
            //向Lobby发送玩家掉线消息
            Msg_RL_UserDrop uaqBuilder = new Msg_RL_UserDrop();
            uaqBuilder.RoomId = RoomId;
            uaqBuilder.UserGuid = user.Guid;
            uaqBuilder.IsBattleEnd = false;
            m_Connector.SendMsgToLobby(uaqBuilder);
            user.LastNotifyUserDropTime = TimeUtility.GetLocalMilliseconds();
            //控制状态改为掉线
            user.UserControlState = (int)UserControlState.UserDropped;
            if (null != user.Info) {
                // user.Info.Suicide();
            }
            LogSys.Log(LOG_TYPE.DEBUG, "Room {0} User {1}({2}) dropped.", RoomId, user.Guid, user.GetKey());
        }
        public void RemoveUser(User user)
        {
            RemoveUser(user, true);
        }
        public void RemoveUser(User user, bool free)
        {
            if (user == null) {
                return;
            }
            foreach (User otheruser in m_RoomUsers) {
                if (null != otheruser && otheruser != user) {
                    otheruser.RemoveSameRoomUser(user);
                }
            }
            user.ClearSameRoomUser();
            if (null != m_ActiveScene) {
                Scene scene = m_ActiveScene;
                scene.LeaveScene(user);
            }
            LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for {1} {2}, [Room.RemoveUser]", user.LocalID, user.Guid, user.GetKey());
            m_RoomUsers.Remove(user);
            if (free) {
                m_UserPool.FreeUser(user.LocalID);
            }
        }
        public bool AddObserver(ulong guid, string name, uint key)
        {
            bool ret = false;
            Observer observer = GetUnusedObserver();
            if (null != observer) {
                observer.IsIdle = false;
                observer.Guid = guid;
                observer.Name = name;
                observer.SetKey(key);
                ret = true;
            }
            return ret;
        }
        public void DropObserver(Observer observer)
        {
            if (observer.IsConnected()) {
                observer.Disconnect();
            }
            observer.IsEntered = false;
        }

        public RoomUserManager()
        {
            m_RoomUsers = new List<User>();
            for (int i = 0; i < m_RoomObservers.Length; ++i) {
                m_RoomObservers[i] = new Observer();
                m_RoomObservers[i].OwnRoomUserManager = this;
            }
        }

        private Observer GetUnusedObserver()
        {
            Observer ret = null;
            for (int i = 0; i < m_RoomObservers.Length; ++i) {
                Observer observer = m_RoomObservers[i];
                if (observer.IsIdle) {
                    ret = observer;
                    break;
                }
            }
            return ret;
        }

        //-------------------------------------------------------------------------
        private Connector m_Connector = null;
        private int m_RoomId = 0;
        private int m_LocalRoomId = 0;
        private bool m_IsFieldRoom = false;
        private Scene m_ActiveScene = null;
        private List<User> m_RoomUsers = null;
        //每个房间固定几个观察者，就不另外使用内存池了(如果需要多人观战功能，应该独立开发观战服务器，观战服务器作观察者)
        private const int c_MaxObserverNum = 5;
        private Observer[] m_RoomObservers = new Observer[c_MaxObserverNum];

        private UserPool m_UserPool = null;
    }
}
