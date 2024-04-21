using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataCache;
using System.Threading;
using System.Text;
using Messenger;
using GameFrameworkMessage;
using GameFrameworkData;

internal class DataCacheSystem : MyServerThread
{
    internal GameFramework.ServerAsyncActionProcessor LoadActionQueue
    {
        get { return m_LoadActionQueue; }
    }
    internal GameFramework.ServerAsyncActionProcessor SaveActionQueue
    {
        get { return m_SaveActionQueue; }
    }
    //==================================================================
    internal void Init()
    {
        PersistentSystem.Instance.Init();
        Start();
        LogSys.Log(ServerLogType.INFO, "DataCacheSystem initialized");
    }
    //==========================Methods called through QueueAction===========================================
    //Note! The callback function may currently be called in both the cache thread and the db thread. The implementation of the callback function needs to be thread-safe (currently, messages are generally sent to meet this condition).
    internal void Load(Msg_LD_Load msg, PBChannel channel, ulong handle)
    {
        //First, search the data in the cache. If it is not found, search it in the DB.
        bool isLoadCache = true;
        Msg_DL_LoadResult ret = new Msg_DL_LoadResult();
        ret.MsgId = msg.MsgId;
        ret.PrimaryKeys.AddRange(msg.PrimaryKeys);
        ret.SerialNo = msg.SerialNo;
        ret.ErrorNo = 0;
        for (int i = 0; i < msg.LoadRequests.Count; ++i) {
            Msg_LD_SingleLoadRequest req = msg.LoadRequests[i];
            KeyString loadKey = KeyString.Wrap(req.Keys);
            switch (req.LoadType) {
                case Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadAll: {
                        isLoadCache = false;
                    }
                    break;
                case Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadSingle: {
                        InnerCacheItem item = m_InnerCacheSystem.Find(req.MsgId, loadKey);
                        if (item != null) {
                            Msg_DL_SingleRowResult result = new Msg_DL_SingleRowResult();
                            result.MsgId = req.MsgId;
                            result.PrimaryKeys.AddRange(req.Keys);
                            result.DataVersion = 0;         //TODO: Is this DataVersion useful?
                            result.Data = item.DataMessage;
                            ret.Results.Add(result);
                        } else {
                            isLoadCache = false;
                        }
                    }
                    break;
                case Msg_LD_SingleLoadRequest.LoadTypeEnum.LoadMulti: {
                        List<InnerCacheItem> itemList = m_InnerCacheSystem.FindByForeignKey(req.MsgId, loadKey);
                        foreach (var item in itemList) {
                            Msg_DL_SingleRowResult result = new Msg_DL_SingleRowResult();
                            result.MsgId = req.MsgId;
                            result.PrimaryKeys.AddRange(req.Keys);
                            result.DataVersion = 0;         //TODO: Is this DataVersion useful?
                            result.Data = item.DataMessage;
                            ret.Results.Add(result);
                        }
                    }
                    break;
            }
        }
        if (isLoadCache) {
            channel.Send(ret);
            LogSys.Log(ServerLogType.INFO, "Load data from cache. MsgId:{0}, Key:{1}", msg.MsgId, KeyString.Wrap(msg.PrimaryKeys).ToString());
        } else {
            //Search DB and hand it over to DBLoad thread operation
            DbThreadManager.Instance.LoadActionQueue.QueueAction(DataLoadImplement.Load, msg, (MyAction<Msg_DL_LoadResult>)((Msg_DL_LoadResult result) => {
                if (result.ErrorNo == Msg_DL_LoadResult.ErrorNoEnum.Success) {
                    foreach (Msg_DL_SingleRowResult row in result.Results) {
                        m_InnerCacheSystem.AddOrUpdate(row.MsgId, KeyString.Wrap(row.PrimaryKeys), KeyString.Wrap(row.ForeignKeys), row.Data);
                    }
                }
                channel.Send(result);
                LogSys.Log(ServerLogType.INFO, "Load data from database. MsgId:{0}, Key:{1}", msg.MsgId, KeyString.Wrap(msg.PrimaryKeys).ToString());
            }));
        }
    }
    internal void Save(int msgId, List<string> primaryKey, List<string> foreignKey, byte[] dataBytes, long serialNo)
    {
        //refresh cache
        m_InnerCacheSystem.AddOrUpdate(msgId, KeyString.Wrap(primaryKey), KeyString.Wrap(foreignKey), dataBytes, serialNo);
    }
    internal void DoLastSave()
    {
        PersistentSystem.Instance.LastSaveToDB();
    }
    //==========================Methods that can only be called in this thread===========================================
    internal Dictionary<int, List<InnerCacheItem>> FetchDirtyCacheItems()
    {
        return m_InnerCacheSystem.FetchDirtyCacheItems();
    }
    //===================================================================================================================
    protected override void OnStart()
    {
        TickSleepTime = 10;
        ActionNumPerTick = 8192;
    }
    protected override void OnTick()
    {
        try {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime != 0) {
                long elapsedTickTime = curTime - m_LastTickTime;
                if (elapsedTickTime > c_WarningTickTime) {
                    LogSys.Log(ServerLogType.MONITOR, "DataCacheSystem Tick:{0}", elapsedTickTime);
                }
            }
            m_LastTickTime = curTime;

            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;
                DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "DataCacheSystem.ActionQueue {0}", msg);
                });
                m_LoadActionQueue.DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "DataCacheSystem.LoadActionQueue {0}", msg);
                });
                m_SaveActionQueue.DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "DataCacheSystem.SaveActionQueue {0}", msg);
                });
                LogSys.Log(ServerLogType.MONITOR, "DataCacheSystem.ActionQueue Current Action {0}", this.CurActionNum);
            }

            if (curTime - m_LastCacheTickTime > c_CacheTickInterval) {
                m_InnerCacheSystem.Tick();
                m_LastCacheTickTime = curTime;
            }

            m_LoadActionQueue.HandleActions(4096);
            m_SaveActionQueue.HandleActions(4096);
            PersistentSystem.Instance.Tick();
        } catch (Exception ex) {
            LogSys.Log(ServerLogType.ERROR, "DataCacheSystem ERROR:{0} \n StackTrace:{1}", ex.Message, ex.StackTrace);
            if (ex.InnerException != null) {
                LogSys.Log(ServerLogType.ERROR, "DataCacheSystem INNER ERROR:{0} \n StackTrace:{1}", ex.InnerException.Message, ex.InnerException.StackTrace);
            }
        }
    }

    private ServerAsyncActionProcessor m_LoadActionQueue = new ServerAsyncActionProcessor();
    private ServerAsyncActionProcessor m_SaveActionQueue = new ServerAsyncActionProcessor();
    private InnerCacheSystem m_InnerCacheSystem = new InnerCacheSystem();
    private long m_LastCacheTickTime = 0;
    private const long c_CacheTickInterval = 60000;     //Tick cycle of InnerCache:10min
    private long m_LastLogTime = 0;
    private const long c_WarningTickTime = 1000;
    private long m_LastTickTime = 0;

    internal static DataCacheSystem Instance
    {
        get { return s_Instance; }
    }
    private static DataCacheSystem s_Instance = new DataCacheSystem();
}
