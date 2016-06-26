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
                                    AddActiveRoom(startId, field_roomthread_list_.Count, true);
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
            next_room_id_ = startId;
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

        internal void AddActiveRoom(int roomid, int roomthreadindex, bool isFieldScene)
        {
            if (isFieldScene) {
                roomthreadindex |= c_field_thread_index_mask;
            }
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

        internal void ChangeRoomScene(int roomid, int sceneId)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(roomid, out isFieldThread);
            if (ix >= 0 && !isFieldThread) {
                RoomThread thread = roomthread_list_[ix];
                thread.QueueAction(thread.ChangeRoomScene, roomid, sceneId, (MyAction<bool>)((bool success) => {

                }));
            }
        }
        
        internal void PlayerRequestActiveRoom(int targetSceneId, params ulong[] guids)
        {
            Msg_RL_ActiveScene builder = new Msg_RL_ActiveScene();
            builder.UserGuids.AddRange(guids);
            builder.SceneID = targetSceneId;
            connector_.SendMsgToLobby(builder);
        }

        internal void PlayerRequestChangeRoom(int targetSceneId, params ulong[] guids)
        {
            Msg_RL_ChangeScene builder = new Msg_RL_ChangeScene();
            builder.UserGuids.AddRange(guids);
            builder.SceneID = targetSceneId;
            connector_.SendMsgToLobby(builder);
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
            ulong guid = msg.UserGuid;
            int roomId = msg.RoomID;
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(roomId, out isFieldThread);
            if (ix < 0) {
                Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                replyBuilder.UserGuid = guid;
                replyBuilder.RoomID = roomId;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            } else {
                RoomThread roomThread;
                if (isFieldThread) {
                    roomThread = field_roomthread_list_[ix];
                } else {
                    roomThread = roomthread_list_[ix];
                }
                Msg_LR_RoomUserInfo rui = msg.UserInfo;

                User rsUser = user_pool_.NewUser();
                LogSys.Log(LOG_TYPE.INFO, "NewUser {0} for {1} {2}", rsUser.LocalID, rui.Guid, rui.Key);
                rsUser.Init();
                if (!rsUser.SetKey(rui.Key)) {
                    LogSys.Log(LOG_TYPE.WARN, "user who's key is {0} already in room!", rui.Key);
                    LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for {1} {2}, [RoomManager.HandleEnterScene]", rsUser.LocalID, rui.Guid, rui.Key);
                    user_pool_.FreeUser(rsUser.LocalID);
                                        
                    Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                    replyBuilder.UserGuid = guid;
                    replyBuilder.RoomID = roomId;
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
                        
                    roomThread.QueueAction(roomThread.AddUser, rsUser, roomId, (MyAction<bool, int, User>)((bool success, int sceneId, User user) => {
                        if (success) {
                            Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                            replyBuilder.UserGuid = guid;
                            replyBuilder.RoomID = roomId;
                            replyBuilder.Result = (int)SceneOperationResultEnum.Success;
                            channel.Send(replyBuilder);
                        } else {
                            Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                            replyBuilder.UserGuid = guid;
                            replyBuilder.RoomID = roomId;
                            replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                            channel.Send(replyBuilder);
                        }
                    }));
                }
            }
        }
        private void HandleChangeScene(Msg_LR_ChangeScene msg, PBChannel channel, int handle, uint seq)
        {
            ulong guid = msg.UserGuid;
            int roomid = msg.RoomID;
            int targetRoomId = msg.TargetRoomID;
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
                RoomThread roomThread;
                if (isFieldThread) {
                    roomThread = field_roomthread_list_[ix];
                } else {
                    roomThread = roomthread_list_[ix];
                }
                bool targetIsFieldThread;
                int targetIx = GetActiveRoomThreadIndex(targetRoomId, out targetIsFieldThread);
                if (null != roomThread) {
                    if (targetIx >= 0) {
                        //同服切场景
                        roomThread.QueueAction(roomThread.RemoveUser, guid, roomid, false, (MyAction<bool, int, User>)((bool success, int sceneId, User user) => {
                            if (success) {
                                PlayerGotoRoom(user, roomid, targetRoomId);
                            } else {
                                Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                                replyBuilder.UserGuid = guid;
                                replyBuilder.RoomID = roomid;
                                replyBuilder.TargetRoomID = targetRoomId;
                                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                                channel.Send(replyBuilder);
                            }
                        }));
                    } else {
                        //跨服切场景
                        roomThread.QueueAction(roomThread.RemoveUser, guid, roomid, true, (MyAction<bool, int, User>)((bool success, int sceneId, User user) => {
                            if (success) {
                                Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                                EntityInfo info = user.Info;
                                if (null != info) {
                                    replyBuilder.HP = info.Hp;
                                    replyBuilder.MP = info.Energy;
                                }
                                replyBuilder.UserGuid = guid;
                                replyBuilder.RoomID = roomid;
                                replyBuilder.TargetRoomID = targetRoomId;
                                replyBuilder.Result = (int)SceneOperationResultEnum.Success;
                                channel.Send(replyBuilder);
                            } else {
                                Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                                replyBuilder.UserGuid = guid;
                                replyBuilder.RoomID = roomid;
                                replyBuilder.TargetRoomID = targetRoomId;
                                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                                channel.Send(replyBuilder);
                            }
                        }));
                    }
                }
            }
        }
        private void HandleActiveScene(Msg_LR_ActiveScene msg, PBChannel channel, int handle, uint seq)
        {
            int roomid = msg.RoomID;
            int sceneId = msg.SceneID;
            List<ulong> users = msg.UserGuids;
            int thread_id = GetIdleThread();
            if (thread_id < 0) {
                LogSys.Log(LOG_TYPE.ERROR, "all room are using, active room failed!");
                Msg_RL_ActiveSceneResult retMsg = new Msg_RL_ActiveSceneResult();
                retMsg.UserGuids.AddRange(users);
                retMsg.RoomID = roomid;
                retMsg.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                return;
            }
            RoomThread roomThread = roomthread_list_[thread_id];
            AddActiveRoom(roomid, thread_id, false);
            roomThread.PreActiveRoom();
            LogSys.Log(LOG_TYPE.INFO, "queue active room {0} scene {1} thread {2}", roomid, sceneId, thread_id);
            roomThread.QueueAction(roomThread.ActiveRoom, roomid, sceneId, (MyAction<bool>)((bool val) => {
                Msg_RL_ActiveSceneResult retMsg = new Msg_RL_ActiveSceneResult();
                retMsg.UserGuids.AddRange(users);
                retMsg.RoomID = roomid;
                retMsg.Result = val ? (int)SceneOperationResultEnum.Success : (int)SceneOperationResultEnum.Cant_Find_Room;
            }));
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
        private void PlayerGotoRoom(User user, int roomId, int targetRoomId)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(targetRoomId, out isFieldThread);
            if (ix < 0)
                return;

            RoomThread roomThread;
            if (isFieldThread) {
                roomThread = field_roomthread_list_[ix];
            } else {
                roomThread = roomthread_list_[ix];
            }
            roomThread.QueueAction(roomThread.AddUser, user, targetRoomId, (MyAction<bool, int, User>)((bool ret, int sceneId, User successUser) => {
                if (ret) {
                    Msg_RC_ChangeScene msg = new Msg_RC_ChangeScene();
                    msg.target_scene_id = sceneId;
                    successUser.SendMessage(RoomMessageDefine.Msg_RC_ChangeScene, msg);

                    Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                    EntityInfo info = user.Info;
                    if (null != info) {
                        replyBuilder.HP = info.Hp;
                        replyBuilder.MP = info.Energy;
                    }
                    replyBuilder.UserGuid = user.Guid;
                    replyBuilder.RoomID = roomId;
                    replyBuilder.TargetRoomID = targetRoomId;
                    replyBuilder.Result = (int)SceneOperationResultEnum.Success;
                    connector_.SendMsgToLobby(replyBuilder);
                } else {
                    Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                    replyBuilder.UserGuid = user.Guid;
                    replyBuilder.RoomID = roomId;
                    replyBuilder.TargetRoomID = targetRoomId;
                    replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                    connector_.SendMsgToLobby(replyBuilder);
                }
            }));
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

        private int next_room_id_ = 1;

        //demo使用
        private static uint s_next_key_ = 1;
        private static ulong s_next_guid_ = 1;
    }

}
