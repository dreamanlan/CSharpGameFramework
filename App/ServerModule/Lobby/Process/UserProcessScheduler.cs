using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using GameFrameworkData;
using GameFramework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CSharpCenterClient;
using GameFrameworkMessage;

namespace Lobby
{
    /// <summary>
    /// Player data processing scheduler, player data requests will be processed in several parallel threads.
    /// There are 2 types of threads:
    /// 1. For operations initiated by DispatchAction calls, the execution thread cannot be specified at this time.
    /// 2. Instantiate a thread internally in the scheduler to perform operations that must be performed in a thread. (The interface is not provided to the outside world. It is currently assumed to be used for orderly services required for operations in 1.)
    /// </summary>
    /// <remarks>
    /// This class uses multi-threading to operate data, and all members cannot assume the thread in which they work.
    /// Please note four constraints:
    /// 1. Once UserInfo is instantiated, the memory will not be released (it will only be recycled into the pool for reuse, the same is true for RoomInfo).
    /// 2. For functions that only operate data less than or equal to the machine word length, no locking is performed (the operation is inherently atomic).
    /// 3. If the operated data is larger than the machine word length and must ensure transactional updates, locking is required. Each UserInfo has a Lock attribute (mono's read-write lock has a deadlock bug, so ordinary locks are used directly here. ). Properties with complex structures held on UserInfo,
    /// If the structure/class involves collection operations, the data of the structure/class should be encapsulated and the security of multi-threaded operations should be ensured through internal locking or lockfree mechanisms.
    /// 4. Except for methods starting with Get, such methods are usually initiated through DispatchAction calls. The specific thread allocation is considered as follows:
    /// a. After the player enters the room, basically only the room thread will modify the player data, so RoomProcessThread will directly modify the player data (usually simple data or status modification).
    /// b. When the player is in the lobby but has not entered the room, the Node sends a message to the Lobby, and then calls various methods through DispatchAction for processing.
    /// c. When the player plays the game, the RoomServer will need to modify the player data. At this time, it will send a message to the Lobby, and then call various methods through DispatchAction for processing.
    /// </remarks>
    internal sealed class UserProcessScheduler : MyServerTaskDispatcher
    {
        internal UserProcessScheduler()
          : base(12, true, 10, 4096)
        {
            m_Thread = new MyServerThread();
            m_Thread.TickSleepTime = 10;
            m_Thread.ActionNumPerTick = 10240;
            m_Thread.OnTickEvent += this.OnTick;

            m_NodeMessageManager.Init(3, 10, 4096);
        }
        internal void Start()
        {
            m_Thread.Start();
            m_NodeMessageManager.Start();
        }
        internal void Stop()
        {
            m_NodeMessageManager.Stop();
            StopTaskThreads();
            m_Thread.Stop();
        }
        internal void DispatchJsonMessage(uint seq, ulong sourceHandle, ulong destHandle, byte[] data)
        {
            m_NodeMessageManager.DispatchMessage(seq, sourceHandle, destHandle, data);
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //Methods for direct external calls need to ensure multi-thread safety.
        //--------------------------------------------------------------------------------------------------------------------------
        internal void VisitUsers(MyAction<UserInfo> visitor)
        {
            foreach (KeyValuePair<ulong, UserInfo> pair in m_UserInfos) {
                UserInfo userInfo = pair.Value;
                visitor(userInfo);
            }
        }
        //Find UserGuid based on Nickname among current online players
        internal ulong GetGuidByNickname(string nickname)
        {
            ulong guid = 0;
            m_GuidByNickname.TryGetValue(nickname, out guid);
            return guid;
        }
        internal UserInfo GetUserInfo(ulong guid)
        {
            UserInfo info = null;
            m_UserInfos.TryGetValue(guid, out info);
            return info;
        }
        internal int GetUserCount()
        {
            return m_UserInfos.Count;
        }
        internal void DoCloseServers()
        {
            CenterClientApi.SendCommandByName("NodeJs1", "QuitNodeJs");
            CenterClientApi.SendCommandByName("NodeJs2", "QuitNodeJs");
            CenterClientApi.SendCommandByName("NodeJs3", "QuitNodeJs");
            CenterClientApi.SendCommandByName("NodeJs4", "QuitNodeJs");
            CenterClientApi.SendCommandByName("NodeJs5", "QuitNodeJs");
            CenterClientApi.SendCommandByName("NodeJs6", "QuitNodeJs");
            CenterClientApi.SendCommandByName("NodeJs7", "QuitNodeJs");
            CenterClientApi.SendCommandByName("NodeJs8", "QuitNodeJs");
            CenterClientApi.SendCommandByName("NodeJs9", "QuitNodeJs");
            CenterClientApi.SendCommandByName("RoomSvr1", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr2", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr3", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr4", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr5", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr6", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr11", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr12", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr13", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr14", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr15", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr16", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr21", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr22", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr23", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr24", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr25", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr26", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr31", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr32", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr33", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr34", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr35", "QuitRoomServer");
            CenterClientApi.SendCommandByName("RoomSvr36", "QuitRoomServer");
            CenterClientApi.SendCommandByName("GmServer", "QuitGmServer");
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //Methods for external calls through DispatchAction will be executed in different threads, and multi-thread safety needs to be ensured.
        //--------------------------------------------------------------------------------------------------------------------------
        internal void RequestEnterScene(Msg_LB_RequestEnterScene msg)
        {
            //Add/update player data
            UserInfo info = AddOrUpdateUserInfo(msg.BaseInfo, msg.User, UserInfo.c_LifeTimeWaitStartGame);

            //Start the process of entering the wild
            RoomProcessThread roomProcess = LobbyServer.Instance.RoomProcessThread;
            roomProcess.QueueAction(roomProcess.RequestEnterScene, info.Guid, msg.SceneId, msg.WantRoomId, msg.FromSceneId);
        }

        internal void BroadcastText(ulong guid, BroadcastType type, string content, int roll_ct)
        {
            UserInfo info = GetUserInfo(guid);
            if (null != info) {
                Msg_BL_BroadcastText builder = new Msg_BL_BroadcastText();
                builder.BroadcastType = (int)type;
                builder.Content = content;
                builder.RollCount = roll_ct;
                LobbyServer.Instance.UserChannel.Send(info.UserSvrName, builder);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        //These methods are tool methods, and concurrency-related processing needs to be reconsidered later.
        //--------------------------------------------------------------------------------------------------------------------------
        internal UserInfo AddOrUpdateUserInfo(Msg_LB_BigworldUserBaseInfo baseUserInfo, Msg_LR_RoomUserInfo roomUserInfo, int leftLifeTime)
        {
            ulong guid = roomUserInfo.Guid;
            UserInfo info;
            if (!m_UserInfos.TryGetValue(guid, out info)) {
                info = NewUserInfo();
                info.Key = GenerateKey();
            }
            info.BaseUserInfo = baseUserInfo;
            info.RoomUserInfo = roomUserInfo;

            info.LeftLife = leftLifeTime;
            info.CurrentState = UserState.Online;

            m_UserInfos.AddOrUpdate(guid, info, (g, u) => info);
            m_GuidByNickname.AddOrUpdate(info.Nickname, guid, (n, g) => guid);

            ActivateUserGuid(guid);
            return info;
        }
        private bool IsPveScene(int sceneId)
        {
            bool ret = false;
            TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(sceneId);
            if (null != cfg && (int)SceneTypeEnum.Story == cfg.type) {
                ret = true;
            }
            return ret;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //The following methods are all executed in internal threads, do not involve multi-threaded operations, do not require locking, and are executed serially.
        //--------------------------------------------------------------------------------------------------------------------------
        private void OnTick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();

            int elapsedTickTime = m_Thread.TickSleepTime;
            if (m_LastTickTime > 0) {
                elapsedTickTime = (int)(curTime - m_LastTickTime);
                if (elapsedTickTime < 0) {
                    elapsedTickTime = m_Thread.TickSleepTime;
                }
            }
            m_LastTickTime = curTime;

            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;

                DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "UserProcessScheduler.DispatchActionQueue {0}", msg);
                });
                DebugActionCount((string msg) => {
                    LogSys.Log(ServerLogType.MONITOR, "UserProcessScheduler.DispatchActionQueue {0}", msg);
                });
                m_Thread.DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "UserProcessScheduler.ThreadActionQueue {0}", msg);
                });
                LogSys.Log(ServerLogType.MONITOR, "UserProcessScheduler.ThreadActionQueue Current Action {0}", m_Thread.CurActionNum);

                m_NodeMessageManager.TickMonitor();

                LogSys.Log(ServerLogType.MONITOR, "Lobby User Count:{0} ElapsedTickTime:{1}", m_ActiveUserGuids.Count, elapsedTickTime);
            }

            m_DeactiveUserGuids.Clear();
            foreach (var guidPair in m_ActiveUserGuids) {
                ulong guid = guidPair.Key;
                UserInfo user = GetUserInfo(guid);
                if (user == null) {
                    m_DeactiveUserGuids.Add(guid);
                }
                else {
                    user.LeftLife -= elapsedTickTime;
                    if (user.LeftLife <= 0) {
                        if (UserState.Room != user.CurrentState) {
                            if (user.Room != null) {
                                RoomProcessThread roomProcess = LobbyServer.Instance.RoomProcessThread;
                                roomProcess.QueueAction(roomProcess.QuitRoom, guid, true, (ulong)0);
                            }
                            m_DeactiveUserGuids.Add(guid);
                        }
                        else {
                            user.LeftLife = UserInfo.c_NextLifeTime;
                        }
                    }
                }
            }
            int tmpValue = 0;
            foreach (ulong guid in m_DeactiveUserGuids) {
                m_ActiveUserGuids.TryRemove(guid, out tmpValue);
                AddWaitRecycleUser(guid);
            }

            m_DeactiveUserGuids.Clear();
            foreach (ulong guid in m_WaitRecycleUsers) {
                UserInfo user = GetUserInfo(guid);
                if (user == null) {
                    m_DeactiveUserGuids.Add(guid);
                }
                else {
                    FreeKey(user.Key);
                    ulong g = 0;
                    m_GuidByNickname.TryRemove(user.Nickname, out g);
                    UserInfo tmp;
                    m_UserInfos.TryRemove(guid, out tmp);
                    RecycleUserInfo(user);
                    m_DeactiveUserGuids.Add(guid);
                }
            }
            foreach (ulong guid in m_DeactiveUserGuids) {
                m_WaitRecycleUsers.Remove(guid);
            }
        }
        private void AddWaitRecycleUser(ulong guid)
        {
            if (!m_WaitRecycleUsers.Contains(guid)) {
                m_WaitRecycleUsers.Add(guid);
            }
        }
        //-----------------------------------------------------------------------------
        //These methods now allow execution in multiple threads simultaneously
        //-----------------------------------------------------------------------------
        private UserInfo NewUserInfo()
        {
            UserInfo info = null;
            if (m_UnusedUserInfos.IsEmpty) {
                info = new UserInfo();
            }
            else {
                if (!m_UnusedUserInfos.TryDequeue(out info)) {
                    info = new UserInfo();
                }
                else {
                    info.IsRecycled = false;
                }
            }
            return info;
        }
        private void RecycleUserInfo(UserInfo info)
        {
            info.IsRecycled = true;
            info.Reset();
            m_UnusedUserInfos.Enqueue(info);
        }
        private void ActivateUserGuid(ulong guid)
        {
            m_ActiveUserGuids.AddOrUpdate(guid, 1, (g, v) => 1);
        }
        private uint GenerateKey()
        {
            uint key = 0;
            for (; ; ) {
                key = (uint)(m_Random.NextDouble() * 0x0fffffff);
                if (!m_Keys.ContainsKey(key)) {
                    m_Keys.AddOrUpdate(key, true, (k, v) => true);
                    break;
                }
            }
            return key;
        }
        private void FreeKey(uint key)
        {
            bool nouse = false;
            m_Keys.TryRemove(key, out nouse);
        }

        private NodeMessageManager m_NodeMessageManager = new NodeMessageManager();

        private ConcurrentDictionary<ulong, UserInfo> m_UserInfos = new ConcurrentDictionary<ulong, UserInfo>();
        private ConcurrentDictionary<string, ulong> m_GuidByNickname = new ConcurrentDictionary<string, ulong>();
        private ConcurrentQueue<UserInfo> m_UnusedUserInfos = new ConcurrentQueue<UserInfo>();
        private ConcurrentDictionary<uint, bool> m_Keys = new ConcurrentDictionary<uint, bool>();

        private ConcurrentDictionary<ulong, int> m_ActiveUserGuids = new ConcurrentDictionary<ulong, int>();
        private List<ulong> m_DeactiveUserGuids = new List<ulong>();
        private List<ulong> m_WaitRecycleUsers = new List<ulong>();

        private Random m_Random = new Random();
        private MyServerThread m_Thread = null;

        private long m_LastTickTime = 0;
        private long m_LastLogTime = 0;
    }
}