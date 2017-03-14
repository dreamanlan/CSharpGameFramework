using System;
using System.Collections.Generic;
using Lidgren.Network;
using Messenger;
using GameFrameworkMessage;
using GameFramework;

namespace GameFramework
{
    /// <remarks>
    /// 注意这个类的public方法，都应考虑跨线程调用是否安全！！！
    /// </remarks>
    public class RoomManager
    {
        public RoomManager(uint maxusernum, uint thread_amount, uint room_amount)
        {
            s_Instance = this;

            m_ThreadAmount = thread_amount;
            m_RoomAmount = room_amount;
            m_RoomThreadList = new RoomThread[thread_amount];
            m_UserPoolSize = maxusernum;
            m_UserPool = new UserPool();
            m_ThreadTickInterval = 50;
        }

        public RoomManager(uint maxusernum, uint thread_amount, uint room_amount, uint tick_interval, Connector conn)
        {
            s_Instance = this;

            m_ThreadAmount = thread_amount;
            m_RoomAmount = room_amount;
            m_RoomThreadList = new RoomThread[thread_amount];
            m_UserPoolSize = maxusernum;
            m_UserPool = new UserPool();
            m_Dispatcher = new Dispatcher();
            m_ThreadTickInterval = tick_interval;
            m_Connector = conn;
        }

        public bool Init(string roomServerName)
        {
            m_Lock = new object();
            m_ActiveRooms = new Dictionary<int, int>();
            m_UserPool.Init(m_UserPoolSize);
            
            // 初始化场景房间
            int startId = 1;
            MyDictionary<int, object> scenes = TableConfig.LevelProvider.Instance.LevelMgr.GetData();
            foreach (KeyValuePair<int, object> pair in scenes) {
                TableConfig.Level cfg = pair.Value as TableConfig.Level;
                if (null != cfg && cfg.type == (int)SceneTypeEnum.Story) {
                    foreach (string roomSvrName in cfg.RoomServer) {
                        for (int ix = 0; ix < cfg.ThreadCountPerScene; ++ix) {
                            if (0 == roomServerName.CompareTo(roomSvrName)) {
                                RoomThread fieldThread = new RoomThread(this);
                                fieldThread.Init(m_ThreadTickInterval, (uint)cfg.RoomCountPerThread, m_UserPool, m_Connector);
                                for (int rix = 0; rix < cfg.RoomCountPerThread; ++rix) {
                                    fieldThread.ActiveFieldRoom(startId, cfg.id);
                                    AddActiveRoom(startId, m_FieldRoomthreadList.Count, true);
                                    AddFieldSceneRoomId(cfg.id, startId);
                                    ++startId;
                                }
                                m_FieldRoomthreadList.Add(fieldThread);
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
            for (int i = 0; i < m_ThreadAmount; ++i) {
                m_RoomThreadList[i] = new RoomThread(this);
                m_RoomThreadList[i].Init(m_ThreadTickInterval, m_RoomAmount, m_UserPool, m_Connector);
            }
            m_NextRoomId = startId;
            return true;
        }

        public void StartRoomThread()
        {
            SceneLoadThread.Instance.Start();
            for (int i = 0; i < m_FieldRoomthreadList.Count; ++i) {
                m_FieldRoomthreadList[i].Start();
            }
            for (int i = 0; i < m_ThreadAmount; ++i) {
                m_RoomThreadList[i].Start();
            }
        }

        public void StopRoomThread()
        {
            for (int i = 0; i < m_ThreadAmount; ++i) {
                m_RoomThreadList[i].Stop();
            }
            for (int i = 0; i < m_FieldRoomthreadList.Count; ++i) {
                m_FieldRoomthreadList[i].Stop();
            }
            SceneLoadThread.Instance.Stop();
        }

        public int GetIdleRoomCount()
        {
            int count = 0;
            for (int i = 0; i < m_ThreadAmount; ++i) {
                count += m_RoomThreadList[i].IdleRoomCount();
            }
            return count;
        }

        public int GetUserCount()
        {
            return m_UserPool.GetUsedCount();
        }

        public int GetActiveRoomThreadIndex(int roomid, out bool isFieldThread)
        {
            isFieldThread = false;
            int ix = -1;
            lock (m_Lock) {
                if (m_ActiveRooms.TryGetValue(roomid, out ix)) {
                    if ((ix & c_FieldThreadIndexMask) == c_FieldThreadIndexMask) {
                        isFieldThread = true;
                        ix = ix & (~c_FieldThreadIndexMask);
                    }
                } else {
                    ix = -1;
                }
            }
            return ix;
        }

        public void AddActiveRoom(int roomid, int roomthreadindex, bool isFieldScene)
        {
            if (isFieldScene) {
                roomthreadindex |= c_FieldThreadIndexMask;
            }
            lock (m_Lock) {
                if (m_ActiveRooms.ContainsKey(roomid)) {
                    m_ActiveRooms[roomid] = roomthreadindex;
                } else {
                    m_ActiveRooms.Add(roomid, roomthreadindex);
                }
            }
        }

        public void RemoveActiveRoom(int roomid)
        {
            lock (m_Lock) {
                m_ActiveRooms.Remove(roomid);
            }
        }

        public void ChangeRoomScene(int roomid, int sceneId)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(roomid, out isFieldThread);
            if (ix >= 0 && !isFieldThread) {
                RoomThread thread = m_RoomThreadList[ix];
                thread.QueueAction(thread.ChangeRoomScene, roomid, sceneId, (MyAction<bool>)((bool success) => {

                }));
            }
        }

        public Room GetRoomByRoomIdAndLocalId(int roomId, int localId)
        {
            Room room = null;
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(roomId, out isFieldThread);
            if (ix >= 0) {
                RoomThread roomThread;
                if (isFieldThread) {
                    roomThread = m_FieldRoomthreadList[ix];
                } else {
                    roomThread = m_RoomThreadList[ix];
                }
                if (null != roomThread) {
                    room = roomThread.GetRoomByLocalId(localId);
                }
            }
            return room;
        }

        //--------------------------------------
        public void RegisterMsgHandler(PBChannel channel)
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
            int roomId = msg.RoomId;
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(roomId, out isFieldThread);
            if (ix < 0) {
                Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                replyBuilder.UserGuid = guid;
                replyBuilder.RoomId = roomId;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            } else {
                RoomThread roomThread;
                if (isFieldThread) {
                    roomThread = m_FieldRoomthreadList[ix];
                } else {
                    roomThread = m_RoomThreadList[ix];
                }
                Msg_LR_RoomUserInfo rui = msg.UserInfo;

                User rsUser = m_UserPool.NewUser();
                LogSys.Log(LOG_TYPE.INFO, "NewUser {0} for {1} {2}", rsUser.LocalID, rui.Guid, rui.Key);
                rsUser.Init();
                if (!rsUser.SetKey(rui.Key)) {
                    LogSys.Log(LOG_TYPE.WARN, "user who's key is {0} already in room!", rui.Key);
                    LogSys.Log(LOG_TYPE.INFO, "FreeUser {0} for {1} {2}, [RoomManager.HandleEnterScene]", rsUser.LocalID, rui.Guid, rui.Key);
                    m_UserPool.FreeUser(rsUser.LocalID);
                                        
                    Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                    replyBuilder.UserGuid = guid;
                    replyBuilder.RoomId = roomId;
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
                            replyBuilder.RoomId = roomId;
                            replyBuilder.Result = (int)SceneOperationResultEnum.Success;
                            channel.Send(replyBuilder);
                        } else {
                            Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                            replyBuilder.UserGuid = guid;
                            replyBuilder.RoomId = roomId;
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
            int roomid = msg.RoomId;
            int targetRoomId = msg.TargetRoomId;
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomId, out isFieldThread);
            if (ix < 0) {
                Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                replyBuilder.UserGuid = msg.UserGuid;
                replyBuilder.RoomId = msg.RoomId;
                replyBuilder.TargetRoomId = msg.TargetRoomId;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            } else {
                RoomThread roomThread;
                if (isFieldThread) {
                    roomThread = m_FieldRoomthreadList[ix];
                } else {
                    roomThread = m_RoomThreadList[ix];
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
                                replyBuilder.RoomId = roomid;
                                replyBuilder.TargetRoomId = targetRoomId;
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
                                replyBuilder.RoomId = roomid;
                                replyBuilder.TargetRoomId = targetRoomId;
                                replyBuilder.Result = (int)SceneOperationResultEnum.Success;
                                channel.Send(replyBuilder);
                            } else {
                                Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                                replyBuilder.UserGuid = guid;
                                replyBuilder.RoomId = roomid;
                                replyBuilder.TargetRoomId = targetRoomId;
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
            int roomid = msg.RoomId;
            int sceneId = msg.SceneId;
            List<ulong> users = msg.UserGuids;
            int thread_id = GetIdleThread();
            if (thread_id < 0) {
                LogSys.Log(LOG_TYPE.ERROR, "all room are using, active room failed!");
                Msg_RL_ActiveSceneResult retMsg = new Msg_RL_ActiveSceneResult();
                retMsg.UserGuids.AddRange(users);
                retMsg.RoomId = roomid;
                retMsg.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                return;
            }
            RoomThread roomThread = m_RoomThreadList[thread_id];
            AddActiveRoom(roomid, thread_id, false);
            roomThread.PreActiveRoom();
            LogSys.Log(LOG_TYPE.INFO, "queue active room {0} scene {1} thread {2}", roomid, sceneId, thread_id);
            roomThread.QueueAction(roomThread.ActiveRoom, roomid, sceneId, (MyAction<bool>)((bool val) => {
                Msg_RL_ActiveSceneResult retMsg = new Msg_RL_ActiveSceneResult();
                retMsg.UserGuids.AddRange(users);
                retMsg.RoomId = roomid;
                retMsg.Result = val ? (int)SceneOperationResultEnum.Success : (int)SceneOperationResultEnum.Cant_Find_Room;
            }));
        }
        private void HandleReconnectUser(Msg_LR_ReconnectUser urMsg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(urMsg.RoomId, out isFieldThread);
            if (ix < 0) {
                Msg_RL_ReplyReconnectUser replyBuilder = new Msg_RL_ReplyReconnectUser();
                replyBuilder.UserGuid = urMsg.UserGuid;
                replyBuilder.RoomId = urMsg.RoomId;
                replyBuilder.Result = (int)Msg_RL_ReplyReconnectUser.ReconnectResultEnum.NotExist;
                channel.Send(replyBuilder);
            } else {
                if (isFieldThread) {
                    RoomThread roomThread = m_FieldRoomthreadList[ix];
                    roomThread.QueueAction(roomThread.HandleReconnectUser, urMsg, channel, handle, seq);
                } else {
                    RoomThread roomThread = m_RoomThreadList[ix];
                    roomThread.QueueAction(roomThread.HandleReconnectUser, urMsg, channel, handle, seq);
                }
            }
        }
        private void HandleUserRelive(Msg_LR_UserReLive msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomId, out isFieldThread);
            if (ix >= 0) {
                if (isFieldThread) {
                    RoomThread roomThread = m_FieldRoomthreadList[ix];
                    roomThread.QueueAction(roomThread.HandleUserRelive, msg);
                } else {
                    RoomThread roomThread = m_RoomThreadList[ix];
                    roomThread.QueueAction(roomThread.HandleUserRelive, msg);
                }
            }
        }
        private void HandleUserQuit(Msg_LR_UserQuit msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomId, out isFieldThread);
            if (ix >= 0) {
                if (isFieldThread) {
                    RoomThread roomThread = m_FieldRoomthreadList[ix];
                    roomThread.QueueAction(roomThread.HandleUserQuit, msg, channel);
                } else {
                    RoomThread roomThread = m_RoomThreadList[ix];
                    roomThread.QueueAction(roomThread.HandleUserQuit, msg, channel);
                }
            } else {
                Msg_RL_UserQuit replyBuilder = new Msg_RL_UserQuit();
                replyBuilder.UserGuid = msg.UserGuid;
                replyBuilder.RoomId = msg.RoomId;
                channel.Send(replyBuilder);
            }
        }
        private void HandleReclaimItem(Msg_LR_ReclaimItem msg, PBChannel channel, int handle, uint seq)
        {
            bool isFieldThread;
            int ix = GetActiveRoomThreadIndex(msg.RoomId, out isFieldThread);
            if (ix >= 0) {
                if (isFieldThread) {
                    RoomThread roomThread = m_FieldRoomthreadList[ix];
                    roomThread.QueueAction(roomThread.HandleReclaimItem, msg, channel);
                } else {
                    RoomThread roomThread = m_RoomThreadList[ix];
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
                    RoomThread roomThread = m_FieldRoomthreadList[ix];
                    roomThread.QueueAction(roomThread.HandleRoomStoryMessage, msg, channel);
                } else {
                    RoomThread roomThread = m_RoomThreadList[ix];
                    roomThread.QueueAction(roomThread.HandleRoomStoryMessage, msg, channel);
                }
            }
        }
        // private functions--------------------
        private int GetIdleThread()
        {
            int most_idle_thread_id = 0;
            for (int i = 1; i < m_ThreadAmount; ++i) {
                if (m_RoomThreadList[most_idle_thread_id].IdleRoomCount() <
                    m_RoomThreadList[i].IdleRoomCount()) {
                    most_idle_thread_id = i;
                }
            }
            if (m_RoomThreadList[most_idle_thread_id].IdleRoomCount() < GlobalVariables.c_PreservedRoomCountPerThread) {
                return -1;
            }
            return most_idle_thread_id;
        }
        private void AddFieldSceneRoomId(int sceneId, int roomId)
        {
            HashSet<int> roomIds;
            if (m_FieldSceneRooms.TryGetValue(sceneId, out roomIds)) {
                roomIds.Add(roomId);
            } else {
                roomIds = new HashSet<int>();
                roomIds.Add(roomId);
                m_FieldSceneRooms.Add(sceneId, roomIds);
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
                roomThread = m_FieldRoomthreadList[ix];
            } else {
                roomThread = m_RoomThreadList[ix];
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
                    replyBuilder.RoomId = roomId;
                    replyBuilder.TargetRoomId = targetRoomId;
                    replyBuilder.Result = (int)SceneOperationResultEnum.Success;
                    m_Connector.SendMsgToLobby(replyBuilder);
                } else {
                    Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                    replyBuilder.UserGuid = user.Guid;
                    replyBuilder.RoomId = roomId;
                    replyBuilder.TargetRoomId = targetRoomId;
                    replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                    m_Connector.SendMsgToLobby(replyBuilder);
                }
            }));
        }

        // private attributes-------------------
        private const int c_FieldThreadIndexMask = 0x10000000;
        private List<RoomThread> m_FieldRoomthreadList = new List<RoomThread>();     // 野外房间线程列表
        private Dictionary<int, HashSet<int>> m_FieldSceneRooms = new Dictionary<int, HashSet<int>>();

        private uint m_ThreadAmount;                                                  // 线程数
        private uint m_RoomAmount;                                                    // 每线程房间数
        private RoomThread[] m_RoomThreadList;                                        // 线程列表

        private uint m_ThreadTickInterval;                                           // 线程心跳间隔
        private uint m_UserPoolSize;
        private Dispatcher m_Dispatcher;
        private UserPool m_UserPool;
        private Connector m_Connector;

        private object m_Lock;
        private Dictionary<int, int> m_ActiveRooms;

        private int m_NextRoomId = 1;

        public static RoomManager Instance
        {
            get { return s_Instance; }
        }
        private static RoomManager s_Instance = null;
    }

}
