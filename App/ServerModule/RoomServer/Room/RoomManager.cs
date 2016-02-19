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
        internal RoomManager(uint maxUserNum)
        {
            user_pool_size_ = maxUserNum;
            user_pool_ = new UserPool();
            thread_tick_interval_ = 50;
        }

        internal RoomManager(uint maxUserNum, uint tick_interval, Connector conn)
        {
            user_pool_size_ = maxUserNum;
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
                                    fieldThread.ActiveRoom(startId, cfg.id);
                                    AddActiveRoom(startId, roomthread_list_.Count);
                                    ++startId;
                                }
                                roomthread_list_.Add(fieldThread);
                            } else {
                                for (int rix = 0; rix < cfg.RoomCountPerThread; ++rix) {
                                    ++startId;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        internal void StartRoomThread()
        {
            SceneLoadThread.Instance.Start();
            for (int i = 0; i < roomthread_list_.Count; ++i) {
                roomthread_list_[i].Start();
            }
        }

        internal void StopRoomThread()
        {
            for (int i = 0; i < roomthread_list_.Count; ++i) {
                roomthread_list_[i].Stop();
            }
            SceneLoadThread.Instance.Stop();
        }

        internal int GetIdleRoomCount()
        {
            int count = 0;
            for (int i = 0; i < roomthread_list_.Count; ++i) {
                count += roomthread_list_[i].IdleRoomCount();
            }
            return count;
        }

        internal int GetUserCount()
        {
            return user_pool_.GetUsedCount();
        }

        internal int GetActiveRoomThreadIndex(int roomid)
        {
            int ix = -1;
            lock (lock_obj_) {
                if (!active_rooms_.TryGetValue(roomid, out ix)) {
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
        
        internal void ChangeRoomScene(int roomid, int scenetype)
        {
            int ix = GetActiveRoomThreadIndex(roomid);
            if (ix >= 0) {
                RoomThread thread = roomthread_list_[ix];
                thread.QueueAction(thread.ChangeRoomScene, roomid, scenetype);
            }
        }

        internal void AddPlayer(uint key, ulong guid, int campId)
        {            
            int level = 1;
            int heroId = 1;
            int roomId = 1;
            int sceneId = 3;
            string nick = "Nick";

            int ix = GetActiveRoomThreadIndex(roomId);
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

            RoomThread roomThread = roomthread_list_[ix];
            roomThread.QueueAction(roomThread.AddUser, guid, roomId, rsUser);
        }

        //--------------------------------------
        internal void RegisterMsgHandler(PBChannel channel)
        {
            channel.Register<Msg_LR_EnterScene>(HandleEnterScene);
            channel.Register<Msg_LR_ChangeScene>(HandleChangeScene);
            channel.Register<Msg_LR_ReconnectUser>(HandleReconnectUser);
            channel.Register<Msg_LR_UserReLive>(HandleUserRelive);
            channel.Register<Msg_LR_UserQuit>(HandleUserQuit);
            channel.Register<Msg_LR_ReclaimItem>(HandleReclaimItem);
            channel.Register<Msg_LRL_StoryMessage>(HandleRoomStoryMessage);
        }
        //--------------------------------------
        private void HandleEnterScene(Msg_LR_EnterScene msg, PBChannel channel, int handle, uint seq)
        {
            int ix = GetActiveRoomThreadIndex(msg.RoomID);
            if (ix < 0) {
                Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                replyBuilder.UserGuid = msg.UserGuid;
                replyBuilder.RoomID = msg.RoomID;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            } else {
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

                    RoomThread roomThread = roomthread_list_[ix];
                    roomThread.QueueAction(roomThread.HandleEnterScene, rui.Guid, msg.RoomID, rsUser, channel, handle, seq);
                }
            }
        }
        private void HandleChangeScene(Msg_LR_ChangeScene msg, PBChannel channel, int handle, uint seq)
        {
            int ix = GetActiveRoomThreadIndex(msg.RoomID);
            if (ix < 0) {
                Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                replyBuilder.UserGuid = msg.UserGuid;
                replyBuilder.RoomID = msg.RoomID;
                replyBuilder.TargetRoomID = msg.TargetRoomID;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            } else {
                RoomThread roomThread = roomthread_list_[ix];
                roomThread.QueueAction(roomThread.HandleChangeScene, msg.UserGuid, msg.RoomID, msg.TargetRoomID, channel, handle, seq);                
            }
        }
        private void HandleReconnectUser(Msg_LR_ReconnectUser urMsg, PBChannel channel, int handle, uint seq)
        {
            int ix = GetActiveRoomThreadIndex(urMsg.RoomID);
            if (ix < 0) {
                Msg_RL_ReplyReconnectUser replyBuilder = new Msg_RL_ReplyReconnectUser();
                replyBuilder.UserGuid = urMsg.UserGuid;
                replyBuilder.RoomID = urMsg.RoomID;
                replyBuilder.Result = (int)Msg_RL_ReplyReconnectUser.ReconnectResultEnum.NotExist;
                channel.Send(replyBuilder);
            } else {
                RoomThread roomThread = roomthread_list_[ix];
                roomThread.QueueAction(roomThread.HandleReconnectUser, urMsg, channel, handle, seq);
            }
        }

        private void HandleUserRelive(Msg_LR_UserReLive msg, PBChannel channel, int handle, uint seq)
        {
            int ix = GetActiveRoomThreadIndex(msg.RoomID);
            if (ix >= 0) {
                RoomThread roomThread = roomthread_list_[ix];
                roomThread.QueueAction(roomThread.HandleUserRelive, msg);
            }
        }

        private void HandleUserQuit(Msg_LR_UserQuit msg, PBChannel channel, int handle, uint seq)
        {
            int ix = GetActiveRoomThreadIndex(msg.RoomID);
            if (ix >= 0) {
                RoomThread roomThread = roomthread_list_[ix];
                roomThread.QueueAction(roomThread.HandleUserQuit, msg, channel);
            } else {
                Msg_RL_UserQuit replyBuilder = new Msg_RL_UserQuit();
                replyBuilder.UserGuid = msg.UserGuid;
                replyBuilder.RoomID = msg.RoomID;
                channel.Send(replyBuilder);
            }
        }

        private void HandleReclaimItem(Msg_LR_ReclaimItem msg, PBChannel channel, int handle, uint seq)
        {
            int ix = GetActiveRoomThreadIndex(msg.RoomID);
            if (ix >= 0) {
                RoomThread roomThread = roomthread_list_[ix];
                roomThread.QueueAction(roomThread.HandleReclaimItem, msg, channel);
            }
        }

        private void HandleRoomStoryMessage(Msg_LRL_StoryMessage msg, PBChannel channel, int handle, uint seq)
        {
            int ix = GetActiveRoomThreadIndex(msg.RoomId);
            if (ix >= 0) {
                RoomThread roomThread = roomthread_list_[ix];
                roomThread.QueueAction(roomThread.HandleRoomStoryMessage, msg, channel);
            }
        }
        
        private uint thread_tick_interval_;
        private uint user_pool_size_;
        private Dispatcher dispatcher_;
        private UserPool user_pool_;
        private Connector connector_;

        private object lock_obj_;
        private Dictionary<int, int> active_rooms_;
        private List<RoomThread> roomthread_list_ = new List<RoomThread>();
    }

}
