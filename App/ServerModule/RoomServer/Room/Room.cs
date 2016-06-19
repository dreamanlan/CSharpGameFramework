using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using GameFrameworkMessage;

namespace GameFramework
{
    internal enum RoomState
    {
        Unuse,
        Active,
        Finish,
        Deactive,
    }

    internal class Room
    {
        internal bool IsFieldRoom
        {
            get { return m_IsFieldRoom; }
            set { m_IsFieldRoom = value; }
        }
        internal RoomState CurrentState
        { get; set; }
        internal Room()
        {
            dispatcher_ = new Dispatcher();
            seat_player_dict_ = new MyDictionary<string, uint>();
            disconnected_users_ = new List<User>();
            request_delete_users_ = new List<User>();
            can_close_time_ = 0;
            room_users_ = new List<User>();

            for (int i = 0; i < room_observers_.Length; ++i) {
                room_observers_[i] = new Observer();
                room_observers_[i].OwnRoom = this;
            }
        }
        internal List<User> RoomUsers
        {
            get { return room_users_; }
        }
        internal Observer[] RoomObservers
        {
            get { return room_observers_; }
        }
        internal int RoomUserCount
        {
            get
            {
                return room_users_.Count;
            }
        }
        internal int GetAiRoomUserCount()
        {
            int count = 0;
            foreach (User us in room_users_) {
                if ((int)UserControlState.Ai == us.UserControlState) {
                    ++count;
                }
            }
            return count;
        }
        internal int GetActiveRoomUserCount()
        {
            int count = 0;
            foreach (User us in room_users_) {
                if ((int)UserControlState.User == us.UserControlState) {
                    ++count;
                }
            }
            return count;
        }
        internal long GetMinimizeElapsedDroppedTime()
        {
            long time = long.MaxValue;
            foreach (User us in room_users_) {
                if (null != us && (int)UserControlState.UserDropped == us.UserControlState) {
                    long _time = us.GetElapsedDroppedTime();
                    if (time > _time)
                        time = _time;
                }
            }
            return time;
        }
        internal User GetUserByGuid(ulong guid)
        {
            foreach (User us in room_users_) {
                if (us != null && us.Guid == guid) {
                    return us;
                }
            }
            return null;
        }
        internal User GetUserByRoleID(int roleId)
        {
            foreach (User us in room_users_) {
                if (us != null && us.RoleId == roleId) {
                    return us;
                }
            }
            return null;
        }
        internal int RoomID
        {
            get
            {
                return cur_room_id_;
            }
        }
        internal uint LocalID
        { set; get; }
        internal bool IsIdle
        { set; get; }
        internal bool CanClose
        {
            get
            {
                return can_close_time_ > 0 && can_close_time_ + c_close_wait_time_ < TimeUtility.GetLocalMilliseconds();
            }
        }
        internal Scene GetActiveScene()
        {
            return m_ActiveScene;
        }
        internal GameFramework.ScenePool ScenePool
        {
            get { return m_ScenePool; }
            set { m_ScenePool = value; }
        }
        internal bool UserIsAllReady
        { get; set; }
        internal bool Init(int room_id, int scene_type, UserPool userpool, Connector conn)
        {
            LogSys.Log(LOG_TYPE.INFO, "[0] Room.Init {0} scene {1}", room_id, scene_type);
            cur_room_id_ = room_id;
            user_pool_ = userpool;
            connector_ = conn;
            can_close_time_ = 0;
            m_ActiveScene = m_ScenePool.NewScene();
            LogSys.Log(LOG_TYPE.INFO, "[1] Room.Init {0} scene {1}", room_id, scene_type);
            m_ActiveScene.SetRoom(this);
            //场景数据加载由加载线程执行（注：场景没有加载完成，场景状态仍然是sleep，Scene.Tick不会有实际的动作）
            SceneLoadThread.Instance.QueueAction(m_ActiveScene.LoadData, scene_type);
            this.CurrentState = RoomState.Active;
            this.UserIsAllReady = false;
            LogSys.Log(LOG_TYPE.DEBUG, "Room Initialize: {0}  Scene: {1}", room_id, scene_type);
            return true;
        }

        internal void Destroy()
        {
            LogSys.Log(LOG_TYPE.INFO, "room {0}({1}) destroy.", RoomID, LocalID);
            m_ActiveScene.Reset();
            m_ScenePool.RecycleScene(m_ActiveScene);
            m_ActiveScene = null;

            this.CurrentState = RoomState.Unuse;
            this.UserIsAllReady = false;
            int userCt = room_users_.Count;
            for (int i = userCt - 1; i >= 0; --i) {
                User user = room_users_[i];
                if (null != user) {
                    LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for {1} {2}, [Room.Destroy]", user.LocalID, user.Guid, user.GetKey());
                    user.Reset();
                    user_pool_.FreeUser(user.LocalID);
                    room_users_.RemoveAt(i);
                }
            }
            for (int i = 0; i < room_observers_.Length; ++i) {
                room_observers_[i].Reset();
            }
        }

        internal void ChangeScene(int scene_type)
        {
            m_ActiveScene.Reset();
            m_ScenePool.RecycleScene(m_ActiveScene);
            m_ActiveScene = null;

            foreach (User us in room_users_) {
                us.IsEntered = false;
            }
            this.UserIsAllReady = false;

            can_close_time_ = 0;
            m_ActiveScene = m_ScenePool.NewScene();
            m_ActiveScene.SetRoom(this);
            //场景数据加载由加载线程执行（注：场景没有加载完成，场景状态仍然是sleep，Scene.Tick不会有实际的动作）
            SceneLoadThread.Instance.QueueAction(m_ActiveScene.LoadData, scene_type);

            Msg_RC_ChangeScene msg = new Msg_RC_ChangeScene();
            msg.target_scene_id = scene_type;
            foreach (User us in room_users_) {
                us.SendMessage(RoomMessageDefine.Msg_RC_ChangeScene, msg);
            }

            LogSys.Log(LOG_TYPE.INFO, "Room.ChangeScene {0} scene {1}", cur_room_id_, scene_type);
        }

        internal void Tick()
        {
            try {
                if (this.CurrentState != RoomState.Active && this.CurrentState != RoomState.Finish)
                    return;

                long curTime = TimeUtility.GetLocalMilliseconds();
                if (m_LastLogTime + 60000 < curTime) {
                    m_LastLogTime = curTime;

                    LogSys.Log(LOG_TYPE.INFO, "Room.Tick {0}", RoomID);
                }

                if (this.CurrentState == RoomState.Active) {
                    Scene scene = GetActiveScene();
                    if (null != scene) {
                        scene.Tick();
                    }
                    disconnected_users_.Clear();
                    request_delete_users_.Clear();
                    foreach (User user in room_users_) {
                        if (user != null) {
                            user.Tick();
                            if (user.IsTimeout()) {
                                if (user.UserControlState == (int)UserControlState.User) {
                                    disconnected_users_.Add(user);
                                } else if (user.UserControlState == (int)UserControlState.Remove) {
                                    request_delete_users_.Add(user);
                                } else if (user.UserControlState == (int)UserControlState.UserDropped) {
                                    if (user.LastNotifyUserDropTime + c_NotifyUserDropInterval < curTime) {
                                        Msg_RL_UserDrop uaqBuilder = new Msg_RL_UserDrop();
                                        uaqBuilder.RoomID = cur_room_id_;
                                        uaqBuilder.UserGuid = user.Guid;
                                        uaqBuilder.IsBattleEnd = false;
                                        connector_.SendMsgToLobby(uaqBuilder);
                                        
                                        user.LastNotifyUserDropTime = curTime;
                                    }
                                    //临时处理，踢掉断线的玩家
                                    user.UserControlState = (int)UserControlState.Remove;
                                }
                            }
                        }
                    }
                    foreach (User user in disconnected_users_) {
                        DropUser(user);
                    }
                    foreach (User user in request_delete_users_) {
                        RemoveUser(user);
                    }
                    //todo:观察者掉线处理
                    for (int i = 0; i < room_observers_.Length; ++i) {
                        Observer observer = room_observers_[i];
                        if (!observer.IsIdle) {
                            observer.Tick();
                        }
                    }
                    if (!IsFieldRoom) {
                        int userCount = GetActiveRoomUserCount();
                        if (userCount <= 0) {
                            if (GetMinimizeElapsedDroppedTime() > c_finish_time_for_no_users_) {
                                //若房间内玩家数目为0，结束战斗，关闭房间
                                EndBattle((int)CampIdEnum.Unkown);
                            }
                        }
                    }
                    //每个Tick结束，将空间属性同步给Peer，用于Peer转发消息
                    foreach (User user in room_users_) {
                        if (null != user && null != user.Info && null != user.Info.GetMovementStateInfo()) {
                            RoomServer.RoomPeer peer = user.GetPeer();
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
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        internal bool AddNewUser(User newUser)
        {
            foreach (User us in room_users_) {
                if (us != null && us.Guid == newUser.Guid) {
                    //当前玩家已在游戏房间内
                    if (us.GetKey() == newUser.GetKey()) {
                        LogSys.Log(LOG_TYPE.DEBUG, "Add user success: User already in the room! RoomID:{0}, Guid:{1}, OldUser[{2}]({3}) NewUser[{4}]({5}) ",
                        cur_room_id_, us.Guid, us.LocalID, us.GetKey(), newUser.LocalID, newUser.GetKey());
                        LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for new {1} {2}, [Room.AddNewUser]", newUser.LocalID, newUser.Guid, newUser.GetKey());
                        user_pool_.FreeUser(newUser.LocalID);
                        return true;
                    } else if (us.UserControlState != (int)UserControlState.User) {
                        LogSys.Log(LOG_TYPE.DEBUG, "Add user success: User already in the room! RoomID:{0}, Guid:{1}, OldUser[{2}]({3}) NewUser[{4}]({5}) ",
                        cur_room_id_, us.Guid, us.LocalID, us.GetKey(), newUser.LocalID, newUser.GetKey());
                        LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for old {1} {2}, [Room.AddNewUser]", us.LocalID, us.Guid, us.GetKey());
                        RemoveUser(us);
                        break;
                    } else {
                        LogSys.Log(LOG_TYPE.DEBUG, "Add user false: User already in the room and online! RoomID:{0}, Guid:{1}, OldUser[{2}]({3}) NewUser[{4}]({5}) ",
                        cur_room_id_, us.Guid, us.LocalID, us.GetKey(), newUser.LocalID, newUser.GetKey());
                        LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for new {1} {2}, [Room.AddNewUser]", newUser.LocalID, newUser.Guid, newUser.GetKey());
                        user_pool_.FreeUser(newUser.LocalID);
                        return false;
                    }
                }
            }
            can_close_time_ = 0;
            newUser.EnterRoomTime = TimeUtility.GetLocalMilliseconds();
            newUser.OwnRoom = this;
            newUser.RegisterObservers(room_observers_);
            newUser.CharacterCreateTime = TimeUtility.GetLocalMilliseconds();
            newUser.TimeCounter = 0;
            if ((int)UserControlState.Ai == newUser.UserControlState) {
                newUser.IsEntered = true;
            }
            if (null != m_ActiveScene && m_ActiveScene.SceneState == SceneState.Running) {
                Scene scene = m_ActiveScene;
                scene.EnterScene(newUser);
            }
            foreach (User otheruser in room_users_) {
                if (otheruser != null) {
                    otheruser.AddSameRoomUser(newUser);
                    newUser.AddSameRoomUser(otheruser);
                }
            }
            room_users_.Add(newUser);
            LogSys.Log(LOG_TYPE.DEBUG, "Add user success ! RoomID:{0} , UserGuid:{1}({2})",
              cur_room_id_, newUser.Guid, newUser.GetKey());

            return true;
        }

        internal void PickMoney(User user, int money)
        {
            Msg_RL_PickMoney builder = new Msg_RL_PickMoney();
            builder.UserGuid = user.Guid;
            builder.Num = money;
            connector_.SendMsgToLobby(builder);
        }

        internal void PickItem(User user, int itemId, string model, string particle)
        {
            Msg_RL_PickItem builder = new Msg_RL_PickItem();
            builder.UserGuid = user.Guid;
            builder.RoomID = RoomID;
            builder.ItemId = itemId;
            builder.Model = model;
            builder.Particle = particle;
            connector_.SendMsgToLobby(builder);
        }

        internal void SendServerMessage(object msg)
        {
            connector_.SendMsgToLobby(msg);
        }

        internal void RemoveUserFromRoomThread(User user, bool free)
        {
            RemoveUser(user, free);
        }
                
        internal void DeleteUser(User user)
        {
            if (null != user) {
                user.UserControlState = (int)UserControlState.Remove;
                if (null != user.Info) {
                    //user.Info.Suicide();
                }
                LogSys.Log(LOG_TYPE.DEBUG, "Room {0} User {1}({2}) deleted.", RoomID, user.Guid, user.GetKey());
            }
        }

        internal void DropUser(User user)
        {
            //向Lobby发送玩家掉线消息
            Msg_RL_UserDrop uaqBuilder = new Msg_RL_UserDrop();
            uaqBuilder.RoomID = cur_room_id_;
            uaqBuilder.UserGuid = user.Guid;
            uaqBuilder.IsBattleEnd = false;
            connector_.SendMsgToLobby(uaqBuilder);
            user.LastNotifyUserDropTime = TimeUtility.GetLocalMilliseconds();
            //控制状态改为掉线
            user.UserControlState = (int)UserControlState.UserDropped;
            if (null != user.Info) {
                // user.Info.Suicide();
            }
            LogSys.Log(LOG_TYPE.DEBUG, "Room {0} User {1}({2}) dropped.", RoomID, user.Guid, user.GetKey());
        }

        internal void EndBattle(int winnerCampID)
        {
            if (IsFieldRoom) {
                return;
            }
            if (this.CurrentState == RoomState.Finish || this.CurrentState == RoomState.Deactive) {
                return;
            }
            foreach (User user in room_users_) {
                if (user != null) {
                    Msg_RL_UserDrop unqBuilder = new Msg_RL_UserDrop();
                    unqBuilder.RoomID = cur_room_id_;
                    unqBuilder.UserGuid = user.Guid;
                    unqBuilder.IsBattleEnd = true;
                    connector_.SendMsgToLobby(unqBuilder);
                    user.LastNotifyUserDropTime = TimeUtility.GetLocalMilliseconds();
                }
            }
            //向Lobby发送战斗结束消息

            this.CurrentState = RoomState.Finish;
            m_FinishTime = TimeUtility.GetLocalMilliseconds();
            LogSys.Log(LOG_TYPE.DEBUG, "Room {0}({1}) EndBattle.", RoomID, LocalID);
        }

        internal bool AddObserver(ulong guid, string name, uint key)
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

        internal void DropObserver(Observer observer)
        {
            if (observer.IsConnected()) {
                observer.Disconnect();
            }
            observer.IsEntered = false;
            //observer.IsIdle = true;
        }
        internal void NoticeRoomClosing()
        {
            foreach (User user in room_users_) {
                if (user != null && (int)UserControlState.UserDropped == user.UserControlState) {
                    Msg_RL_UserDrop unqBuilder = new Msg_RL_UserDrop();
                    unqBuilder.RoomID = cur_room_id_;
                    unqBuilder.UserGuid = user.Guid;
                    unqBuilder.IsBattleEnd = true;
                    connector_.SendMsgToLobby(unqBuilder);
                    user.LastNotifyUserDropTime = TimeUtility.GetLocalMilliseconds();
                }
            }
        }

        private void Deactive()
        {
            //准备关闭房间
            for (int index = room_users_.Count - 1; index >= 0; --index) {
                User user = room_users_[index];
                RemoveUser(user);
            }
            this.CurrentState = RoomState.Deactive;
            can_close_time_ = TimeUtility.GetLocalMilliseconds();
            LogSys.Log(LOG_TYPE.DEBUG, "Room {0}({1}) Deactive.", RoomID, LocalID);
        }

        private void RemoveUser(User user)
        {
            RemoveUser(user, true);
        }
        private void RemoveUser(User user, bool free)
        {
            if (user == null) {
                return;
            }
            foreach (User otheruser in room_users_) {
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
            room_users_.Remove(user);
            if (free) {
                user_pool_.FreeUser(user.LocalID);
            }
        }
        private Observer GetUnusedObserver()
        {
            Observer ret = null;
            for (int i = 0; i < room_observers_.Length; ++i) {
                Observer observer = room_observers_[i];
                if (observer.IsIdle) {
                    ret = observer;
                    break;
                }
            }
            return ret;
        }

        //-------------------------------------------------------------------------
        private const long c_close_wait_time_ = 1000;
        private const long c_finish_time_for_no_users_ = 1000;
        private int cur_room_id_;
        private MyDictionary<string, uint> seat_player_dict_;
        private Dispatcher dispatcher_;

        private List<User> room_users_;

        private UserPool user_pool_;
        private List<User> disconnected_users_;
        private List<User> request_delete_users_;
        private Connector connector_;
        private long can_close_time_;

        //每个房间固定几个观察者，就不另外使用内存池了(如果需要多人观战功能，应该独立开发观战服务器，观战服务器作观察者)
        private const int c_max_observer_num_ = 5;
        private Observer[] room_observers_ = new Observer[c_max_observer_num_];

        private bool m_IsFieldRoom = false;
        private Scene m_ActiveScene = null;
        private ScenePool m_ScenePool = null;

        private const long c_NotifyUserDropInterval = 20000;
        private const long c_DeactiveWaitTime = 3000;
        private long m_FinishTime = 0;
        private long m_LastLogTime = 0;
    }
}
