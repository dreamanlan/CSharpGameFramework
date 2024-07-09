using System;
using System.Text;
using System.Data;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ScriptableFramework.DataCache;
using ScriptableFramework;

internal static class DataSaveImplement
{
    //Write a piece of data to DB
    internal static int SingleSaveItem(int msgId, InnerCacheItem cacheItem, int dataVersion)
    {
        if (DataCacheConfig.IsPersistent) {
            try {
                DataDML.Save(msgId, cacheItem.Valid, dataVersion, cacheItem.DataMessage);
            } catch (Exception ex) {
                DBConn.Close();
                LogSys.Log(ServerLogType.ERROR, "SingleSaveItem ERROR. MsgId:{0}, Error:{1}\nStacktrace:{2}", msgId, ex.Message, ex.StackTrace);
                throw;
            }
        }
        return 1;
    }
    //Write multiple pieces of data to DB
    internal static int BatchSaveItemsProc(int msgId, List<InnerCacheItem> cacheItemList, int dataVersion)
    {
        if (DataCacheConfig.IsPersistent) {
            try {
                foreach (var cacheItem in cacheItemList) {
                    DataDML.Save(msgId, cacheItem.Valid, dataVersion, cacheItem.DataMessage);
                }
                LogSys.Log(ServerLogType.MONITOR, "BatchSaveItemsProc SUCCESS. MsgId:{0}, DataCount:{1}, DataVersion:{2}", msgId, cacheItemList.Count, dataVersion);
            } catch (Exception ex) {
                DBConn.Close();
                LogSys.Log(ServerLogType.ERROR, "BatchSaveItemsProc ERROR. MsgId:{0}, Error:{1}\nStacktrace:{2}", msgId, ex.Message, ex.StackTrace);
                throw;
            }
        }
        return cacheItemList.Count;
    }

    //Write multiple pieces of data to DB
    internal static int BatchSaveItemsSql(int msgId, List<InnerCacheItem> cacheItemList, int dataVersion)
    {
        int count = 0;
        if (DataCacheConfig.IsPersistent) {
            try {
                List<bool> validList = new List<bool>();
                List<byte[]> dataList = new List<byte[]>();
                foreach (var cacheItem in cacheItemList) {
                    validList.Add(cacheItem.Valid);
                    dataList.Add(cacheItem.DataMessage);
                }
                count = DataDML.BatchSave(msgId, validList, dataList, dataVersion);
                LogSys.Log(ServerLogType.MONITOR, "BatchSaveItemsSql SUCCESS. MsgId:{0}, DataCount:{1}, DataVersion:{2}", msgId, cacheItemList.Count, dataVersion);
            } catch (Exception ex) {
                DBConn.Close();
                LogSys.Log(ServerLogType.ERROR, "BatchSaveItemsSql ERROR. MsgId:{0}, Error:{1}\nStacktrace:{2}", msgId, ex.Message, ex.StackTrace);
                throw;
            }
        } else {
            count = cacheItemList.Count;
        }
        return count;
    }

    /// <summary>
    /// Check whether the sql value string is legal
    /// </summary>
    /// <param name="statement">SQL value string to be detected</param>
    /// <returns>Return true if legal, false if illegal</returns>
    private static bool ValidateSQLValue(string valueStr)
    {
        bool ret = false;
        int quoteIndex = valueStr.IndexOf('\'');
        if (quoteIndex == -1) {
            ret = true;
        }
        return ret;
    }

    private const int c_SqlCommandTimeout = 120;  //Database operation timeout
}