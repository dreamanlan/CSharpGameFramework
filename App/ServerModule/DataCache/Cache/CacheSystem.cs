using System;
using System.Collections.Generic;
using GameFramework;

namespace GameFramework.DataCache
{
    internal class InnerCacheSystem
    {
        internal const long UltimateCacheVersion = -1;         //优先级最高的缓存数据CacheVersion 
        internal const long InitialCacheVersion = 0;           //初始的缓存数据CacheVersion 
        /// <summary>
        /// 主键查找
        /// </summary>
        /// <param name="msgId">数据类型ID</param>
        /// <param name="key">主键</param>
        /// <returns>查找成功返回对应数据对象，不存在返回null</returns>
        internal InnerCacheItem Find(int msgId, KeyString key)
        {
            InnerCacheTable cacheTable = null;
            m_CacheTableDict.TryGetValue(msgId, out cacheTable);
            if (cacheTable != null) {
                return cacheTable.Find(key);
            }
            return null;
        }
        /// <summary>
        /// 外键查找
        /// </summary>
        /// <param name="msgId">数据类型ID</param>
        /// <param name="key">外键</param>
        /// <returns></returns>
        internal List<InnerCacheItem> FindByForeignKey(int msgId, KeyString foreignKey)
        {
            InnerCacheTable cacheTable = null;
            m_CacheTableDict.TryGetValue(msgId, out cacheTable);
            if (cacheTable != null) {
                return cacheTable.FindByForeignKey(foreignKey);
            }
            return new List<InnerCacheItem>();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="msgId">数据类型ID</param>
        /// <param name="key">Key</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        internal bool Remove(int msgId, KeyString key)
        {
            InnerCacheTable cacheTable = null;
            m_CacheTableDict.TryGetValue(msgId, out cacheTable);
            if (cacheTable != null) {
                return cacheTable.Remove(key);
            }
            return false;
        }
        /// <summary>
        /// 添加或更新
        /// </summary>
        /// <param name="msgId">数据类型ID</param>
        /// <param name="key">Key</param>
        /// <param name="dataMessage">待添加的数据对象</param>
        internal void AddOrUpdate(int msgId, KeyString key, KeyString foreignKey, byte[] dataMessage, long cacheVersion = InnerCacheSystem.InitialCacheVersion)
        {
            InnerCacheTable tableCache = null;
            m_CacheTableDict.TryGetValue(msgId, out tableCache);
            if (tableCache != null) {
                tableCache.AddOrUpdate(key, foreignKey, dataMessage, cacheVersion);
            } else {
                InnerCacheTable newTableCache = new InnerCacheTable();
                newTableCache.AddOrUpdate(key, foreignKey, dataMessage, cacheVersion);
                m_CacheTableDict.Add(msgId, newTableCache);
            }
        }
        /// <summary>
        /// 从缓存中抽取出脏数据
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, List<InnerCacheItem>> FetchDirtyCacheItems()
        {
            Dictionary<int, List<InnerCacheItem>> dirtySet = new Dictionary<int, List<InnerCacheItem>>();
            foreach (var table in m_CacheTableDict) {
                List<InnerCacheItem> dirtyList = table.Value.GetDirtyCacheItems();
                dirtySet.Add(table.Key, dirtyList);
            }
            return dirtySet;
        }

        internal void Tick()
        {
            foreach (var table in m_CacheTableDict) {
                int deleteCount = table.Value.CleanExpiredItems();
                LogSys.Log(ServerLogType.INFO, "Clean invalid or expired cache items. Msg:{0} Count:{1}", table.Key, deleteCount);
            }
        }

        private Dictionary<int, InnerCacheTable> m_CacheTableDict = new Dictionary<int, InnerCacheTable>();
    }
}
