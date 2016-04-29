using System;
using System.Collections.Generic;
using Lidgren.Network;
using Messenger;
using GameFrameworkMessage;
using RoomServer;

namespace GameFramework
{
    /// <remarks>
    /// 注意这个类的internal方法，都应考虑跨线程调用是否安全！！！
    /// </remarks>
    internal class RoomManager
    {
        internal RoomManager(uint maxusernum, uint thread_amount, uint room_amount)
        {
            thread_amount_ = thread_amount;
            room_amount_ = room_amount;
            roomthread_list_ = new RoomThread[thread_amount];
            user_pool_size_ = maxusernum;
            user_pool_ = new UserPool();
            thread_tick_interval_ = 50;
        }

        internal RoomManager(uint maxusernum, uint thread_amount, uint room_amount, uint tick_interval, Connector conn)
        {
            thread_amount_ = thread_amount;
            room_amount_ = room_amount;
            roomthread_list_ = new RoomThread[thread_amount];
            user_pool_size_ = maxusernum;
            user_pool_ = new UserPool();
            dispatcher_ = new Dispatcher();
            thread_tick_interval_ = tick_interval;
            connector_ = conn;
        }

        internal bool Init(string roomServerName)
        {
            lock_obj_ = new object();
            active_rooms_ = new Dictionary<int, int>();
            user_pool_.Init(user_pool_size_);
            
            // 初始化场景房间
            int startId = 1;
            MyDictionary<int, object> scenes = TableConfig.LevelProvider.Instance.LevelMgr.GetData();
            foreach (KeyValuePair<int, object> pair in scenes) {
                TableConfig.Level cfg = pair.Value as TableConfig.Level;
                if (null != cfg && cfg.type == (int)SceneTypeEnum.Room) {
                    foreach (string roomSvrName in cfg.RoomServer) {
                        for (int ix = 0; ix < cfg.ThreadCountPerScene; ++ix) {
                            if (0 == roomServerName.CompareTo(roomSvrName)) {
                                RoomThread fieldThread = new RoomThread(this);
                                fieldThread.Init(thread_tick_interval_, (uint)cfg.RoomCountPerThread, user_pool_, connector_);
                                for (int rix = 0; rix < cfg.RoomCountPerThread; ++rix) {
                                    fieldThread.ActiveFieldRoom(startId, cfg.id);
                                    AddActiveRoom(startId, c_field_thread_index_mask | field_roomthread_list_.Count);
                                    AddFieldSceneRoomId(cfg.id, startId);
                                    ++startId;
                                }
                                field_roomthread_list_.Add(fieldThread);
                            } else {
                                for (int rix = 0; rix < cfg.RoomCountPerThread; ++rix) {
                                    ++startId;
                                }
                            }
                        }
                    }
                }
            }
            // 初始化房间线程
            for (int i = 0; i < thread_amount_; ++i) {
                roomthread_list_[i] = new RoomThread(this);
                roomthread_list_[i].Init(thread_tick_interval_, room_amount_, user_pool_, connector_);
            }
            return true;
        }

        internal void StartRoomThread()
        {
            SceneLoadThread.Instance.Start();
            for (int i = 0; i < field_roomthread_list_.Count; ++i) {
                field_roomthread_list_[i].Start();
            }
            for (int i = 0; i < thread_amount_; ++i) {
                roomthread_list_[i].Start();
            }
        }

        internal void StopRoomThread()
        {
            for (int i = 0; i < thread_amount_; ++i) {
                roomthread_list_[i].Stop();
            }
            for (int i = 0; i < field_roomthread_list_.Count; ++i) {
                field_roomthread_list_[i].Stop();
            }
            SceneLoadThread.Instance.Stop();
        }

        internal int GetIdleRoomCount()
        {
            int count = 0;
            for (int i = 0; i < thread_amount_; ++i) {
                count += roomthread_list_[i].IdleRoomCount();
            }
            return count;
        }

        internal int GetUserCount()
        {
            return user_pool_.GetUsedCount();
        }

        internal int GetActiveRoomThreadIndex(int roomid, out bool isFieldThread)
        {
            isFieldThread = false;
            int ix = -1;
            lock (lock_obj_) {
                if (active_rooms_.TryGetValue(roomid, out ix)) {
                    if ((ix & c_field_thread_index_mask) == c_field_thread_index_mask) {
                        isFieldThread = true;
                        ix = ix & (~c_field_thread_index_mask);
                    }
                } else {
                    ix = -1;
                }
            }
            return ix;
        }

        internal void AddActiveRoom(int roomid, int roomthreadindex)
        {
            lock (lock_obj_) {
                if (active_rooms_.ContainsKey(roomid)) {
                    active_rooms_[roomid] = roomthreadindex;
                } else {
                    active_rooms_.Add(roomid, roomthreadindex);
                }
            }
        }

        internal void RemoveActiveRoom(int roomid)
        {
            lock (lock_obj_) {
                active_rooms_.Remove(roomid);
            }
        }

        internal bool ActiveRoom(int roomid, int scenetype, User[] users, List<int> monsters = null, List<int> hps = null)
        {
            int thread_id = GetIdleThread();
            if (thread_id < 0) {
                LogSys.Log(LOG_TYPE.ERROR, "all room are using, active room failed!");
                foreach (User u in users) {
                    LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for {1} {2}, [RoomManager.ActiveRoom]", u.LocalID, u.Guid, u.GetKey());
                    user_pool_.FreeUser(u.LocalID);
                }
                return false;
            }
            RoomThread roomThread = roomthread_list_[thread_id];
            AddActiveRoom(roomid, thread_id);
            roomThread.PreActiveRoom();
            LogSys.Log(LOG_TYPE.INFO, "queue active room {0} scene {1} thread {2} for {3} users", roomid, scenetype, thread_id, users.Length);
            roomThread.QueueAction(roomThread.ActiveRoom, roomid, scenetype, users, monsters, hps);
            return true;
        }

        internal void ChangeRoomScene(int roomid, int scenetype)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(roomid, out isFieldThread);
            if (ix >= 0 && !isFieldThread) {
                RoomThread thread = roomthread_list_[ix];
                thread.QueueAction(thread.ChangeRoomScene, roomid, scenetype);
            }
        }

        internal void AddPlayer(string nick, int heroId, Lidgren.Network.NetConnection conn)
        {            
            int level = 1;
            int sceneId = 4;
            int roomId = GetFieldSceneFirstRoomId(sceneId);

            uint key = s_next_key_++;
            ulong guid = s_next_guid_++;
            int campId = (int)CampIdEnum.Blue;
            
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(roomId, out isFieldThread);
            if (ix < 0)
                return;
            //先检查是否玩家已经在room上。
            if (RoomPeerMgr.Instance.IsKeyExist(key)) {
                LogSys.Log(LOG_TYPE.WARN, "User {0} is already in room. Key:{1}", guid, key);
                return;
            }
            List<User> users = new List<User>();
            User rsUser = user_pool_.NewUser();
            LogSys.Log(LOG_TYPE.INFO, "NewUser {0} for {1} {2}", rsUser.LocalID, guid, key);
            rsUser.Init();
            if (!rsUser.SetKey(key)) {
                LogSys.Log(LOG_TYPE.WARN, "user who's key is {0} already in room!", key);
                LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for {1} {2}, [RoomManager.HandleCreateBattleRoom]", rsUser.LocalID, guid, key);
                user_pool_.FreeUser(rsUser.LocalID);
                return;
            }

            Msg_LR_RoomUserInfo lobbyUserInfo = new Msg_LR_RoomUserInfo();
            lobbyUserInfo.Guid = guid;
            lobbyUserInfo.Nick = nick;
            lobbyUserInfo.Hero = heroId;
            lobbyUserInfo.Camp = campId;
            lobbyUserInfo.Level = level;
            rsUser.LobbyUserData = lobbyUserInfo;
            rsUser.UserControlState = (int)UserControlState.User;

            users.Add(rsUser);
            LogSys.Log(LOG_TYPE.DEBUG, "enter room {0} scene {1} user info guid={2}, name={3}, key={4}, camp={5}", roomId, sceneId, guid, nick, key, campId);

            RoomThread roomThread = field_roomthread_list_[ix];
            roomThread.QueueAction(roomThread.AddUser, guid, roomId, rsUser);
        }

        //--------------------------------------
        internal void RegisterMsgHandler(PBChannel channel)
        {
            channel.Register<Msg_LR_EnterScene>(HandleEnterScene);
            channel.Register<Msg_LR_ChangeScene>(HandleChangeScene);
            channel.Register<Msg_LR_ActiveScene>(HandleActiveScene);
            channel.Register<Msg_LR_ReconnectUser>(HandleReconnectUser);
            channel.Register<Msg_LR_UserReLive>(HandleUserRelive);
            channel.Register<Msg_LR_UserQuit>(HandleUserQuit);
            channel.Register<Msg_LR_ReclaimItem>(HandleReclaimItem);
            channel.Register<Msg_LRL_StoryMessage>(HandleRoomStoryMessage);
        }
        //--------------------------------------
        private void HandleEnterScene(Msg_LR_EnterScene msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomID, out isFieldThread);
            if (ix < 0) {
                Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                replyBuilder.UserGuid = msg.UserGuid;
                replyBuilder.RoomID = msg.RoomID;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            } else {
                if (isFieldThread) {
                    Msg_LR_RoomUserInfo rui = msg.UserInfo;

                    User rsUser = user_pool_.NewUser();
                    LogSys.Log(LOG_TYPE.INFO, "NewUser {0} for {1} {2}", rsUser.LocalID, rui.Guid, rui.Key);
                    rsUser.Init();
                    if (!rsUser.SetKey(rui.Key)) {
                        LogSys.Log(LOG_TYPE.WARN, "user who's key is {0} already in room!", rui.Key);
                        LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for {1} {2}, [RoomManager.HandleEnterScene]", rsUser.LocalID, rui.Guid, rui.Key);
                        user_pool_.FreeUser(rsUser.LocalID);

                        Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                        replyBuilder.UserGuid = msg.UserGuid;
                        replyBuilder.RoomID = msg.RoomID;
                        replyBuilder.Result = (int)SceneOperationResultEnum.User_Key_Exist;
                        channel.Send(replyBuilder);
                    } else {
                        rsUser.LobbyUserData = rui;
                        if (rui.IsMachine == true)
                            rsUser.UserControlState = (int)UserControlState.Ai;
                        else
                            rsUser.UserControlState = (int)UserControlState.User;
                        if (msg.HP > 0 && msg.MP > 0) {
                            rsUser.SetHpArmor(msg.HP, msg.MP);
                        }

                        if (rui.EnterX > 0 && rui.EnterY > 0) {
                            rsUser.SetEnterPoint(rui.EnterX, rui.EnterY);
                        }

                        RoomThread roomThread = field_roomthread_list_[ix];
                        roomThread.QueueAction(roomThread.HandleEnterScene, rui.Guid, msg.RoomID, rsUser, channel, handle, seq);
                    }
                } else {
                    Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                    replyBuilder.UserGuid = msg.UserGuid;
                    replyBuilder.RoomID = msg.RoomID;
                    replyBuilder.Result = (int)SceneOperationResultEnum.Not_Field_Room;
                    channel.Send(replyBuilder);
                }
            }
        }
        private void HandleChangeScene(Msg_LR_ChangeScene msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomID, out isFieldThread);
            if (ix < 0) {
                Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                replyBuilder.UserGuid = msg.UserGuid;
                replyBuilder.RoomID = msg.RoomID;
                replyBuilder.TargetRoomID = msg.TargetRoomID;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            } else {
                if (isFieldThread) {
                    RoomThread roomThread = field_roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleChangeScene, msg.UserGuid, msg.RoomID, msg.TargetRoomID, channel, handle, seq);
                } else {
                    Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                    replyBuilder.UserGuid = msg.UserGuid;
                    replyBuilder.RoomID = msg.RoomID;
                    replyBuilder.TargetRoomID = msg.TargetRoomID;
                    replyBuilder.Result = (int)SceneOperationResultEnum.Not_Field_Room;
                    channel.Send(replyBuilder);
                }
            }
        }
        private void HandleActiveScene(Msg_LR_ActiveScene msg, PBChannel channel, int handle, uint seq)
        {

        }
        private void HandleReconnectUser(Msg_LR_ReconnectUser urMsg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(urMsg.RoomID, out isFieldThread);
            if (ix < 0) {
                Msg_RL_ReplyReconnectUser replyBuilder = new Msg_RL_ReplyReconnectUser();
                replyBuilder.UserGuid = urMsg.UserGuid;
                replyBuilder.RoomID = urMsg.RoomID;
                replyBuilder.Result = (int)Msg_RL_ReplyReconnectUser.ReconnectResultEnum.NotExist;
                channel.Send(replyBuilder);
            } else {
                if (isFieldThread) {
                    RoomThread roomThread = field_roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleReconnectUser, urMsg, channel, handle, seq);
                } else {
                    RoomThread roomThread = roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleReconnectUser, urMsg, channel, handle, seq);
                }
            }
        }
        private void HandleUserRelive(Msg_LR_UserReLive msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomID, out isFieldThread);
            if (ix >= 0) {
                if (isFieldThread) {
                    RoomThread roomThread = field_roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleUserRelive, msg);
                } else {
                    RoomThread roomThread = roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleUserRelive, msg);
                }
            }
        }
        private void HandleUserQuit(Msg_LR_UserQuit msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomID, out isFieldThread);
            if (ix >= 0) {
                if (isFieldThread) {
                    RoomThread roomThread = field_roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleUserQuit, msg, channel);
                } else {
                    RoomThread roomThread = roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleUserQuit, msg, channel);
                }
            } else {
                Msg_RL_UserQuit replyBuilder = new Msg_RL_UserQuit();
                replyBuilder.UserGuid = msg.UserGuid;
                replyBuilder.RoomID = msg.RoomID;
                channel.Send(replyBuilder);
            }
        }

        private void HandleReclaimItem(Msg_LR_ReclaimItem msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomID, out isFieldThread);
            if (ix >= 0) {
                if (isFieldThread) {
                    RoomThread roomThread = field_roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleReclaimItem, msg, channel);
                } else {
                    RoomThread roomThread = roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleReclaimItem, msg, channel);
                }
            }
        }
        private void HandleRoomStoryMessage(Msg_LRL_StoryMessage msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldRoom;
            int ix = GetActiveRoomThreadIndex(msg.RoomId, out isFieldRoom);
            if (ix >= 0) {
                if (isFieldRoom) {
                    RoomThread roomThread = field_roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleRoomStoryMessage, msg, channel);
                } else {
                    RoomThread roomThread = roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleRoomStoryMessage, msg, channel);
                }
            }
        }
        // private functions--------------------
        private int GetIdleThread()
        {
            int most_idle_thread_id = 0;
            for (int i = 1; i < thread_amount_; ++i) {
                if (roomthread_list_[most_idle_thread_id].IdleRoomCount() <
                    roomthread_list_[i].IdleRoomCount()) {
                    most_idle_thread_id = i;
                }
            }
            if (roomthread_list_[most_idle_thread_id].IdleRoomCount() < GlobalVariables.c_PreservedRoomCountPerThread) {
                return -1;
            }
            return most_idle_thread_id;
        }
        private void AddFieldSceneRoomId(int sceneId, int roomId)
        {
            HashSet<int> roomIds;
            if (field_scene_rooms_.TryGetValue(sceneId, out roomIds)) {
                roomIds.Add(roomId);
            } else {
                roomIds = new HashSet<int>();
                roomIds.Add(roomId);
                field_scene_rooms_.Add(sceneId, roomIds);
            }
        }
        private int GetFieldSceneFirstRoomId(int sceneId)
        {
            int roomId = 0;
            HashSet<int> roomIds;
            if (field_scene_rooms_.TryGetValue(sceneId, out roomIds)) {
                var enumer = roomIds.GetEnumerator();
                enumer.MoveNext();
                roomId = enumer.Current;
            }
            return roomId;
        }

        // private attributes-------------------
        private const int c_field_thread_index_mask = 0x10000000;
        private List<RoomThread> field_roomthread_list_ = new List<RoomThread>();     // 野外房间线程列表
        private Dictionary<int, HashSet<int>> field_scene_rooms_ = new Dictionary<int, HashSet<int>>();

        private uint thread_amount_;                                                  // 线程数
        private uint room_amount_;                                                    // 每线程房间数
        private RoomThread[] roomthread_list_;                                        // 线程列表

        private uint thread_tick_interval_;                                           // 线程心跳间隔
        private uint user_pool_size_;
        private Dispatcher dispatcher_;
        private UserPool user_pool_;
        private Connector connector_;

        private object lock_obj_;
        private Dictionary<int, int> active_rooms_;

        //demo使用
        private static uint s_next_key_ = 1;
        private static ulong s_next_guid_ = 1;
    }

}
