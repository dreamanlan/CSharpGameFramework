using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Messenger;
using ScriptableFramework;
using GameFrameworkMessage;
using GameFrameworkData;

namespace ScriptableFramework
{
    /// <summary>
    /// Data access layer thread. This class is mainly used for operations related to data storage
    /// </summary>  
    internal sealed partial class DataCacheThread : MyServerTaskDispatcher
    {
        private class LoadRequestInfo : IServerConcurrentPoolAllocatedObject<LoadRequestInfo>
        {
            internal long m_LastSendTime;
            internal Msg_LD_Load m_Request;
            internal MyAction<Msg_DL_LoadResult> m_Callback;

            public void InitPool(ServerConcurrentObjectPool<LoadRequestInfo> pool)
            {
                m_Pool = pool;
            }
            public LoadRequestInfo Downcast()
            {
                return this;
            }

            ServerConcurrentObjectPool<LoadRequestInfo> m_Pool = null;
        }
        private class SaveRequestInfo : IServerConcurrentPoolAllocatedObject<SaveRequestInfo>
        {
            internal long m_LastSendTime;
            internal Msg_LD_Save m_Request;
            internal MyAction<Msg_DL_SaveResult> m_Callback;

            public void InitPool(ServerConcurrentObjectPool<SaveRequestInfo> pool)
            {
                m_Pool = pool;
            }
            public SaveRequestInfo Downcast()
            {
                return this;
            }

            ServerConcurrentObjectPool<SaveRequestInfo> m_Pool = null;
        }
        private enum ConnectStatus
        {
            None = 0,
            Connecting,
            Connected,
            Disconnect,
        }
        //Global data loading status
        internal enum DataInitStatus
        {
            Unload = 0,   //not loaded
            Loading,      //Loading from DS
            Done          //Complete loading
        }
        internal DataCacheThread()
            : base(3)
        {
            m_Thread = new MyServerThread();
            m_Thread.TickSleepTime = 10;
            m_Thread.ActionNumPerTick = 4096;
            m_Thread.OnStartEvent += this.OnStart;
            m_Thread.OnTickEvent += this.OnTick;
        }
        internal void Init(PBChannel channel)
        {
            m_DataStoreChannel = channel;

            channel.Register<Msg_DL_Connect>(OnConnectDataStore);
            channel.Register<Msg_DL_LoadResult>(OnLoadReply);
            channel.Register<Msg_DL_SaveResult>(OnSaveReply);
        }
        internal void Start()
        {
            m_Thread.Start();
        }
        internal void Stop()
        {
            StopTaskThreads();
            m_Thread.Stop();
        }
        internal bool DataStoreAvailable
        {
            get { return m_DataStoreAvailable; }
        }

        internal static char[] StringSeparator = new char[] { '|' };
        internal static string ListJoinSeparator = "|";
        internal static string DateTimeStringFormat = "yyyyMMddHHmmss";
        internal const int UltimateSaveCount = -1;         //The store count of the last store operation

        internal int CalcLoadRequestCount()
        {
            int ct = 0;
            foreach (var pair in m_LoadRequests) {
                ct += pair.Value.Count;
            }
            return ct;
        }
        internal int CalcSaveRequestCount()
        {
            int ct = 0;
            foreach (var pair in m_SaveRequestQueues) {
                foreach (var recPair in pair.Value) {
                    ct += recPair.Value.Count;
                }
            }
            return ct;
        }
        internal void RequestLoad(Msg_LD_Load msg, MyAction<Msg_DL_LoadResult> callback)
        {
            msg.SerialNo = GenNextSerialNo();
            KeyString key = KeyString.Wrap(msg.PrimaryKeys);
            ConcurrentDictionary<KeyString, LoadRequestInfo> dict = null;
            if (!m_LoadRequests.TryGetValue(msg.MsgId, out dict)) {
                dict = m_LoadRequests.AddOrUpdate(msg.MsgId, new ConcurrentDictionary<KeyString, LoadRequestInfo>(), (k, v) => v);
            }
            LoadRequestInfo info;
            if (!dict.TryGetValue(key, out info)) {
                info = dict.AddOrUpdate(key, m_LoadRequestPool.Alloc(), (k, v) => v);
            }
            info.m_LastSendTime = TimeUtility.GetLocalMilliseconds();
            info.m_Request = msg;
            info.m_Callback = callback;
            m_DataStoreChannel.Send(msg);
        }
        internal void RequestSave(Msg_LD_Save msg, MyAction<Msg_DL_SaveResult> callback = null)
        {
            lock (m_SaveRequestQueuesLock) {
                msg.SerialNo = GenNextSerialNo();
                KeyString key = KeyString.Wrap(msg.PrimaryKeys);
                ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>> dict;
                if (!m_SaveRequestQueues.TryGetValue(msg.MsgId, out dict)) {
                    dict = m_SaveRequestQueues.AddOrUpdate(msg.MsgId, new ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>>(), (k, v) => v);
                }
                ConcurrentQueue<SaveRequestInfo> queue;
                if (!dict.TryGetValue(key, out queue)) {
                    queue = dict.AddOrUpdate(key, new ConcurrentQueue<SaveRequestInfo>(), (k, v) => v);
                }
                SaveRequestInfo info = m_SaveRequestPool.Alloc();
                info.m_Request = msg;
                info.m_Callback = callback;
                if (queue.Count == 0) {
                    //Send message directly when the current queue is empty
                    info.m_LastSendTime = TimeUtility.GetLocalMilliseconds();
                    m_DataStoreChannel.Send(msg);
                }
                queue.Enqueue(info);
            }
        }
        //Establish a connection with DataCache
        private void ConnectDataStore()
        {
            Msg_LD_Connect connectMsg = new Msg_LD_Connect();
            connectMsg.ClientName = "UserSvr";
            if (m_DataStoreChannel.Send(connectMsg)) {
                m_CurrentStatus = ConnectStatus.Connecting;
            }
        }
        private void OnConnectDataStore(Msg_DL_Connect msg, PBChannel channel, ulong src, uint session)
        {
            LoadGlobalData();
        }
        private void OnLoadReply(Msg_DL_LoadResult msg, PBChannel channel, ulong src, uint session)
        {
            KeyString key = KeyString.Wrap(msg.PrimaryKeys);
            ConcurrentDictionary<KeyString, LoadRequestInfo> dict;
            if (m_LoadRequests.TryGetValue(msg.MsgId, out dict)) {
                LoadRequestInfo info;
                if (dict.TryGetValue(key, out info)) {
                    if (info.m_Request.SerialNo == msg.SerialNo) {
                        if (null != info.m_Callback) {
                            info.m_Callback(msg);
                        }
                        LoadRequestInfo delInfo;
                        if (dict.TryRemove(key, out delInfo)) {
                            m_LoadRequestPool.Recycle(delInfo);
                        }
                    }
                }
            }
        }
        private void OnSaveReply(Msg_DL_SaveResult msg, PBChannel channel, ulong src, uint session)
        {
            KeyString key = KeyString.Wrap(msg.PrimaryKeys);
            ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>> dict;
            if (m_SaveRequestQueues.TryGetValue(msg.MsgId, out dict)) {
                ConcurrentQueue<SaveRequestInfo> reqQueue;
                if (dict.TryGetValue(key, out reqQueue)) {
                    SaveRequestInfo info;
                    if (reqQueue.TryPeek(out info)) {
                        if (info.m_Request.SerialNo == msg.SerialNo) {
                            if (null != info.m_Callback) {
                                info.m_Callback(msg);
                            }
                            SaveRequestInfo delInfo;
                            if (reqQueue.TryDequeue(out delInfo)) {
                                m_SaveRequestPool.Recycle(delInfo);
                            }
                            //Send the next message in the queue
                            if (reqQueue.TryPeek(out info)) {
                                info.m_LastSendTime = TimeUtility.GetLocalMilliseconds();
                                m_DataStoreChannel.Send(info.m_Request);
                            }
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        //For internal methods called through DispatchAction(, the actual execution thread is DataCacheThread.
        //--------------------------------------------------------------------------------------------------------------------------
        private void SaveInternal(Msg_LD_Save msg)
        {
            try {
                RequestSave(msg, (ret) => {
                    KeyString primaryKey = KeyString.Wrap(msg.PrimaryKeys);
                    if (ret.ErrorNo == Msg_DL_SaveResult.ErrorNoEnum.Success) {
                        LogSys.Log(ServerLogType.INFO, "Save data success. MsgId:{0}, SerialNo:{1}, Key:{2}", msg.MsgId, msg.SerialNo, primaryKey);
                    } else {
                        LogSys.Log(ServerLogType.ERROR, "Save data failed. MsgId:{0}, SerialNo:{1}, Key:{2}, Error:{3}, ErrorInfo:{4}",
                                                    msg.MsgId, msg.SerialNo, primaryKey, ret.ErrorNo, ret.ErrorInfo);
                    }
                });
            } catch (Exception e) {
                LogSys.Log(ServerLogType.ERROR, "DataCache Save ERROR:{0}, Stacktrace:{1}", e.Message, e.StackTrace);
            }
        }
        //--------------------------------------------------------------------------------------
        //Internal thread, used for logic that needs to be queued.
        //--------------------------------------------------------------------------------------
        private void OnStart()
        {
            m_DataStoreAvailable = UserServerConfig.DataStoreAvailable;
            if (!m_DataStoreAvailable) {
                LoadGlobalData();
            }
        }
        private void OnTick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime != 0) {
                long elapsedTickTime = curTime - m_LastTickTime;
                if (elapsedTickTime > c_WarningTickTime) {
                    LogSys.Log(ServerLogType.MONITOR, "DataCacheThread Tick:{0}", elapsedTickTime);
                }
            }
            m_LastTickTime = curTime;

            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;
                DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "DataCacheThread.DispatchActionQueue {0}", msg);
                });
                DebugActionCount((string msg) => {
                    LogSys.Log(ServerLogType.MONITOR, "DataCacheThread.DispatchActionQueue {0}", msg);
                });
                LogSys.Log(ServerLogType.MONITOR, "DataCacheThread.ThreadActionQueue Current Action {0}", m_Thread.CurActionNum);
                LogSys.Log(ServerLogType.MONITOR, "DataCacheThread Load Request Count {0} Save Request Count {1}", CalcLoadRequestCount(), CalcSaveRequestCount());
            }
            if (UserServerConfig.DataStoreAvailable == true) {
                if (m_LastOperateTickTime + c_OperateTickInterval < curTime) {
                    m_LastOperateTickTime = curTime;
                    //Load operation
                    foreach (var pair in m_LoadRequests) {
                        int msgId = pair.Key;
                        ConcurrentDictionary<KeyString, LoadRequestInfo> dict = pair.Value;
                        foreach (var reqPair in dict) {
                            KeyString primaryKey = reqPair.Key;
                            LoadRequestInfo req = reqPair.Value;
                            if (req.m_LastSendTime + UserServerConfig.DSRequestTimeout < curTime) {
                                m_WaitDeletedLoadRequests.Add(primaryKey, req.m_Request.SerialNo);
                                Msg_DL_LoadResult result = new Msg_DL_LoadResult();
                                result.MsgId = msgId;
                                result.PrimaryKeys.AddRange(primaryKey.Keys);
                                result.SerialNo = req.m_Request.SerialNo;
                                result.ErrorNo = Msg_DL_LoadResult.ErrorNoEnum.TimeoutError;
                                result.ErrorInfo = "Timeout Error";
                                if (null != req.m_Callback) {
                                    DispatchAction(req.m_Callback, result);
                                }
                            }
                        }
                        foreach (var delPair in m_WaitDeletedLoadRequests) {
                            LoadRequestInfo info;
                            if (dict.TryRemove(delPair.Key, out info)) {
                                if (info.m_Request.SerialNo == delPair.Value) {
                                    m_LoadRequestPool.Recycle(info);
                                } else {
                                    dict.TryAdd(delPair.Key, info);
                                }
                            }
                        }
                        m_WaitDeletedLoadRequests.Clear();
                    }
                    //Save operation
                    foreach (var pair in m_SaveRequestQueues) {
                        int msgId = pair.Key;
                        ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>> dict = pair.Value;
                        foreach (var reqPair in dict) {
                            KeyString primaryKey = reqPair.Key;
                            ConcurrentQueue<SaveRequestInfo> saveReqQueue = reqPair.Value;
                            SaveRequestInfo req;
                            if (saveReqQueue.TryPeek(out req)) {
                                if (req.m_LastSendTime + UserServerConfig.DSRequestTimeout < curTime) {
                                    //time out, resend
                                    LogSys.Log(ServerLogType.ERROR, "DataCacheThread. SaveRequest timeout. MsgId:{0}, PrimaryKey:{1}, SerialNo:{2}",
                                                                  msgId, KeyString.Wrap(req.m_Request.PrimaryKeys), req.m_Request.SerialNo);
                                    m_DataStoreChannel.Send(req.m_Request);
                                    req.m_LastSendTime = curTime;
                                }
                            }
                            if (dict.Count > c_MaxRecordPerMessage && saveReqQueue.Count == 0) {
                                m_WaitDeleteSaveRequests.Add(primaryKey);
                            }
                        }
                        foreach (KeyString key in m_WaitDeleteSaveRequests) {
                            lock (m_SaveRequestQueuesLock) {
                                ConcurrentQueue<SaveRequestInfo> queue;
                                if (dict.TryGetValue(key, out queue)) {
                                    if (queue.Count == 0) {
                                        ConcurrentQueue<SaveRequestInfo> dummy;
                                        dict.TryRemove(key, out dummy);
                                    }
                                }
                            }
                        }
                        m_WaitDeleteSaveRequests.Clear();
                    }
                }
                if (m_LastConnectTime + c_ConnectInterval < curTime) {
                    m_LastConnectTime = curTime;
                    if (m_CurrentStatus != ConnectStatus.Connected) {
                        ConnectDataStore();
                    }
                }
            }
        }
        
        private long GenNextSerialNo()
        {
            return Interlocked.Increment(ref m_NextSerialNo);
        }

        private long m_NextSerialNo = 0;
        private bool m_DataStoreAvailable = false;

        private PBChannel m_DataStoreChannel = null;
        private ConnectStatus m_CurrentStatus = ConnectStatus.None;
        private ConcurrentDictionary<int, ConcurrentDictionary<KeyString, LoadRequestInfo>> m_LoadRequests = new ConcurrentDictionary<int, ConcurrentDictionary<KeyString, LoadRequestInfo>>();
        private ConcurrentDictionary<int, ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>>> m_SaveRequestQueues = new ConcurrentDictionary<int, ConcurrentDictionary<KeyString, ConcurrentQueue<SaveRequestInfo>>>();
        private ServerConcurrentObjectPool<LoadRequestInfo> m_LoadRequestPool = new ServerConcurrentObjectPool<LoadRequestInfo>();
        private ServerConcurrentObjectPool<SaveRequestInfo> m_SaveRequestPool = new ServerConcurrentObjectPool<SaveRequestInfo>();
        private Dictionary<KeyString, long> m_WaitDeletedLoadRequests = new Dictionary<KeyString, long>();
        private List<KeyString> m_WaitDeleteSaveRequests = new List<KeyString>();
        private object m_SaveRequestQueuesLock = new object();
        private const int c_MaxRecordPerMessage = 1000;

        private long m_LastOperateTickTime = 0;
        private const long c_OperateTickInterval = 1000;    //Time interval of timeout Tick
        private long m_LastConnectTime = 0;
        private const long c_ConnectInterval = 5000;      //Time interval for connecting to DataCache

        private const long c_WarningTickTime = 1000;
        private long m_LastTickTime = 0;
        private long m_LastLogTime = 0;

        private MyServerThread m_Thread = null;
    }
}
