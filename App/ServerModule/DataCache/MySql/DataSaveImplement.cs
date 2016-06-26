using System;
using System.Text;
using System.Data;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using GameFramework.DataCache;
using GameFramework;

internal static class DataSaveImplement
{
    //向DB中写入一条数据
    internal static int SingleSaveItem(int msgId, InnerCacheItem cacheItem, int dataVersion)
    {
        if (DataCacheConfig.IsPersistent) {
            try {
                DataDML.Save(msgId, cacheItem.Valid, dataVersion, cacheItem.DataMessage);
            } catch (Exception ex) {
                DBConn.Close();
                LogSys.Log(LOG_TYPE.ERROR, "SingleSaveItem ERROR. MsgId:{0}, Error:{1}\nStacktrace:{2}", msgId, ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        return 1;
    }
    //向DB中写入多条数据
    internal static int BatchSaveItemsProc(int msgId, List<InnerCacheItem> cacheItemList, int dataVersion)
    {
        if (DataCacheConfig.IsPersistent) {
            try {
                foreach (var cacheItem in cacheItemList) {
                    DataDML.Save(msgId, cacheItem.Valid, dataVersion, cacheItem.DataMessage);
                }
                LogSys.Log(LOG_TYPE.MONITOR, "BatchSaveItemsProc SUCCESS. MsgId:{0}, DataCount:{1}, DataVersion:{2}", msgId, cacheItemList.Count, dataVersion);
            } catch (Exception ex) {
                DBConn.Close();
                LogSys.Log(LOG_TYPE.ERROR, "BatchSaveItemsProc ERROR. MsgId:{0}, Error:{1}\nStacktrace:{2}", msgId, ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        return cacheItemList.Count;
    }

    //向DB中写入多条数据
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
                LogSys.Log(LOG_TYPE.MONITOR, "BatchSaveItemsSql SUCCESS. MsgId:{0}, DataCount:{1}, DataVersion:{2}", msgId, cacheItemList.Count, dataVersion);
            } catch (Exception ex) {
                DBConn.Close();
                LogSys.Log(LOG_TYPE.ERROR, "BatchSaveItemsSql ERROR. MsgId:{0}, Error:{1}\nStacktrace:{2}", msgId, ex.Message, ex.StackTrace);
                throw ex;
            }
        } else {
            count = cacheItemList.Count;
        }
        return count;
    }

    /// <summary>
    /// 检测sql值字符串是否合法
    /// </summary>
    /// <param name="statement">待检测的sql值字符串</param>
    /// <returns>合法返回true,非法返回false </returns>
    private static bool ValidateSQLValue(string valueStr)
    {
        bool ret = false;
        int quoteIndex = valueStr.IndexOf('\'');
        if (quoteIndex == -1) {
            ret = true;
        }
        return ret;
    }

    private const int c_SqlCommandTimeout = 120;  //数据库操作超时时间
}