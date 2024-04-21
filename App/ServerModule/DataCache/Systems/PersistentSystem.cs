using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using GameFramework.DataCache;
using GameFramework;
using System.Collections.Concurrent;

internal class PersistentSystem
{
    internal enum SaveState
    {
        Initial = -2,
        Failed = -1,
        Saving = 0,
        Success = 1
    }
    internal void Init()
    {
        m_LastTickTime = TimeUtility.GetLocalMilliseconds();
        m_SaveDataToDBInterval = DataCacheConfig.PersistentInterval;
        m_TablePieceCapacity = DataCacheConfig.TablePieceCapacity;
        LogSys.Log(ServerLogType.INFO, "PersistentSystem initialized");
    }
    internal void Tick()
    {
        try {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime + m_SaveDataToDBInterval < curTime) {
                m_LastTickTime = curTime;
                if (m_NextSaveCount != c_UltimateSaveCount && m_CurrentDBSaveState != SaveState.Saving) {
                    m_CurrentDBSaveState = SaveState.Saving;
                    SaveDirtyDataToDB(m_NextSaveCount, DataOpSystem.Instance.GlobalDBDataVersion);
                    DataOpSystem.Instance.IncreaseGlobalDataVersion();
                    m_NextSaveCount++;
                }
            }
            if (curTime - m_LastCheckTime > c_SaveDataFinishInterval) {
                m_LastCheckTime = curTime;
                if (m_CurrentDBSaveState == SaveState.Saving) {
                    int unfinishedCount = 0;
                    int failedCount = 0;
                    foreach (var pair in m_CurrentSaveCounts) {
                        int saveCount = pair.Value;
                        if (saveCount == c_InitialSaveCount) {
                            unfinishedCount++;
                        } else if (saveCount == c_FailedSaveCount) {
                            failedCount++;
                        }
                    }
                    if (unfinishedCount == 0) {
                        //Storage DB operation completed
                        if (failedCount == 0) {
                            m_CurrentDBSaveState = SaveState.Success;    //Storage DB operation successful
                        } else {
                            m_CurrentDBSaveState = SaveState.Failed;     //Storage DB operation failed
                        }
                        LogSys.Log(ServerLogType.MONITOR, "SaveDirtyDataToDB finished. Result:{0}, NextSaveCount:{1}, CurrentSaveCounts:{2}, FailedCount:{3}",
                          m_CurrentDBSaveState, m_NextSaveCount, m_CurrentSaveCounts.Count, failedCount);
                    }
                }
                if (m_LastSaveState == SaveState.Saving && m_NextSaveCount == c_UltimateSaveCount) {
                    //Last stored result
                    if (m_CurrentDBSaveState == SaveState.Success) {
                        m_LastSaveState = SaveState.Success;
                        LogSys.Log(ServerLogType.INFO, "LastSaveFinished Result:{0}", m_LastSaveState);
                    } else if (m_CurrentDBSaveState == SaveState.Failed) {
                        m_LastSaveState = SaveState.Failed;
                        LogSys.Log(ServerLogType.INFO, "LastSaveFinished Result:{0}", m_LastSaveState);
                    }
                }
            }
        } catch (Exception ex) {
            LogSys.Log(ServerLogType.ERROR, "DataCacheSystem ERROR:{0} \n StackTrace:{1}", ex.Message, ex.StackTrace);
        }
    }
    internal void LastSaveToDB()
    {
        m_StartLastSaveResult = SaveState.Initial;
        if (m_LastSaveState != SaveState.Initial || m_CurrentDBSaveState == SaveState.Saving) {
            LogSys.Log(ServerLogType.ERROR, "Start LastSaveToDB failed because the previous operation is not finish");
            m_StartLastSaveResult = SaveState.Failed;
            return;
        }
        m_StartLastSaveResult = SaveState.Success;
        m_LastSaveState = SaveState.Saving;
        m_CurrentDBSaveState = SaveState.Saving;
        m_NextSaveCount = c_UltimateSaveCount;
        SaveDirtyDataToDB(m_NextSaveCount, DataOpSystem.Instance.GlobalDBDataVersion);
        DataOpSystem.Instance.IncreaseGlobalDataVersion();
    }
    internal SaveState LastSaveState
    {
        get { return m_LastSaveState; }
        set { m_LastSaveState = value; }
    }
    internal SaveState StartLastSaveResult
    {
        get { return m_StartLastSaveResult; }
    }

    private void SaveDirtyDataToDB(int saveCount, int dbDataVersion)
    {
        Dictionary<int, List<InnerCacheItem>> dataSet = DataCacheSystem.Instance.FetchDirtyCacheItems();
        int dirtyCount = 0;
        foreach (var pair in dataSet) {
            var cacheItemList = pair.Value;
            dirtyCount += cacheItemList.Count;
        }
        LogSys.Log(ServerLogType.MONITOR, "SaveDirtyDataToDB Start. SaveCount:{0}, DirtyCount:{1}, DataVersion:{2}", saveCount, dirtyCount, dbDataVersion);
        m_CurrentSaveCounts.Clear();
        foreach (var dataTable in dataSet) {
            int batchCount = (int)(dataTable.Value.Count / m_TablePieceCapacity) + 1;
            LogSys.Log(ServerLogType.MONITOR, "Start SaveDirtyTable. MsgId:{0}, SaveCount:{1}, DataCount:{2}, BatchCount:{3}, DataVersion:{4}",
                                          dataTable.Key, saveCount, dataTable.Value.Count, batchCount, dbDataVersion);
            int beginIndex = 0;
            int endIndex = m_TablePieceCapacity;
            for (int i = 0; i < batchCount; ++i) {
                if (endIndex > dataTable.Value.Count) {
                    endIndex = dataTable.Value.Count;
                }
                List<InnerCacheItem> batchItemList = dataTable.Value.GetRange(beginIndex, endIndex - beginIndex);
                if (batchItemList.Count <= 0) {
                    continue;
                }
                //DBThread batch processing by table
                string saveCountKey = string.Format("{0}:{1}", dataTable.Key.ToString(), i);
                m_CurrentSaveCounts.AddOrUpdate(saveCountKey, c_InitialSaveCount, (g, u) => c_InitialSaveCount);
                LogSys.Log(ServerLogType.MONITOR, "Start SaveTablePiece. MsgId:{0}, DataCount:{1}, SaveCountKey:{2}, DataVersion:{3}",
                                dataTable.Key, batchItemList.Count, saveCountKey, dbDataVersion);
                DbThreadManager.Instance.SaveActionQueue.QueueAction(SaveDirtyTableToDB, dataTable.Key, batchItemList, saveCountKey, saveCount, dbDataVersion);
                beginIndex = endIndex;
                endIndex += m_TablePieceCapacity;
            }
        }
    }
    //Executed in DBThread thread
    private void SaveDirtyItemToDB(int msgId, InnerCacheItem cacheItem, string saveCountKey, int saveCount, int dbDataVersion)
    {
        try {
            DataSaveImplement.SingleSaveItem(msgId, cacheItem, dbDataVersion);
            //Write to DB successfully
            DataCacheSystem.Instance.QueueAction(() => {
                cacheItem.DirtyState = DirtyState.Saved;
            });
        } catch (Exception e) {
            //Writing to DB failed
            DataCacheSystem.Instance.QueueAction(() => {
                cacheItem.DirtyState = DirtyState.Unsaved;
            });
            LogSys.Log(ServerLogType.ERROR, "Save to MySQL ERROR:{0}, \nStacktrace:{1}", e.Message, e.StackTrace);
        }
    }
    private void SaveDirtyTableToDB(int msgId, List<InnerCacheItem> cacheItemList, string saveCountKey, int saveCount, int dbDataVersion)
    {
        try {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //Temporary test code
            if (m_TablePieceCapacity % 2 == 0) {
                DataSaveImplement.BatchSaveItemsProc(msgId, cacheItemList, dbDataVersion);
            } else {
                DataSaveImplement.BatchSaveItemsSql(msgId, cacheItemList, dbDataVersion);
            }
            //Stored procedure implementation
            //DataSaveImplement.BatchSaveItemsProc(msgId, cacheItemList, dbDataVersion);
            //splicing sql implementation
            //DataSaveImplement.BatchSaveItemsSql(msgId, cacheItemList, dbDataVersion);

            DataCacheSystem.Instance.QueueAction(() => {
                foreach (var dataValue in cacheItemList) {
                    dataValue.DirtyState = DirtyState.Saved;
                }
            });

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            LogSys.Log(ServerLogType.MONITOR, "SaveTablePiece Success. MsgId:{0}, SaveCountKey:{1}, SaveCount:{2}, BatchDataCount:{3}, DataVersion:{4}, DBThreadId:{5}, Time:{6}",
                          msgId, saveCountKey, saveCount, cacheItemList.Count, dbDataVersion, Thread.CurrentThread.ManagedThreadId, ts.TotalMilliseconds);

            m_CurrentSaveCounts.AddOrUpdate(saveCountKey, saveCount, (g, u) => saveCount);
            //LogSys.Log(ServerLogType.MONITOR, "SaveTablePiece Success. MsgId:{0}, SaveCountKey:{1}, SaveCount:{2}, BatchDataCount:{3}, DataVersion:{4}, DBThreadId:{5}",
            //             msgId, saveCountKey, saveCount, cacheItemList.Count, dbDataVersion, Thread.CurrentThread.ManagedThreadId);
        } catch (Exception e) {
            //Writing to DB failed
            DataCacheSystem.Instance.QueueAction(() => {
                foreach (var dataValue in cacheItemList) {
                    dataValue.DirtyState = DirtyState.Unsaved;
                }
            });
            m_CurrentSaveCounts.AddOrUpdate(saveCountKey, c_FailedSaveCount, (g, u) => c_FailedSaveCount);
            LogSys.Log(ServerLogType.MONITOR, "SaveTablePiece ERROR. MsgId:{0}, SaveCountKey:{1}, SaveCount:{2}, BatchDataCount:{3}, DataVersion:{4}, DBThreadId:{5}",
                    msgId, saveCountKey, saveCount, cacheItemList.Count, dbDataVersion, Thread.CurrentThread.ManagedThreadId);
            LogSys.Log(ServerLogType.ERROR, "Save to MySQL ERROR:{0}, \nStacktrace:{1}", e.Message, e.StackTrace);
        }
    }

    private SaveState m_CurrentDBSaveState = SaveState.Failed;     //Store DB operation status
    private SaveState m_LastSaveState = SaveState.Initial;         //Last stored operation status
    private SaveState m_StartLastSaveResult = SaveState.Initial;   //Initiate the last stored result
    private long m_LastTickTime = 0;
    private uint m_SaveDataToDBInterval = 360000;           //The time interval for periodic storage, the default is 6min
    private long m_LastCheckTime = 0;                       //The time when the store was last checked for completion
    private const int c_SaveDataFinishInterval = 1000;      //The time interval to check whether a storage operation is completed
    private int m_NextSaveCount = 1;                        //Next store count
    private ConcurrentDictionary<string, int> m_CurrentSaveCounts = new ConcurrentDictionary<string, int>();   //Current storage count
    private const int c_InitialSaveCount = 0;               //initial storage count
    private const int c_UltimateSaveCount = -1;             //final storage count
    private const int c_FailedSaveCount = -2;               //failed storage count
    private int m_TablePieceCapacity = 10000;               //Number of pieces of data in shards

    internal static PersistentSystem Instance
    {
        get { return s_Instance; }
    }
    private static PersistentSystem s_Instance = new PersistentSystem();
}
