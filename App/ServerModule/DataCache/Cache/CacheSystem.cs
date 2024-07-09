using System;
using System.Collections.Generic;
using ScriptableFramework;

namespace ScriptableFramework.DataCache
{
    internal class InnerCacheSystem
    {
        internal const long UltimateCacheVersion = -1;         //CacheVersion of the highest priority cache data
        internal const long InitialCacheVersion = 0;           //Initial cache data CacheVersion
        /// <summary>
        /// primary key lookup
        /// </summary>
        /// <param name="msgId">Data type ID</param>
        /// <param name="key">primary key</param>
        /// <returns>If the search is successful, the corresponding data object is returned. If it does not exist, null is returned.</returns>
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
        /// foreign key lookup
        /// </summary>
        /// <param name="msgId">Data type ID</param>
        /// <param name="key">foreign key</param>
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
        /// delete
        /// </summary>
        /// <param name="msgId">Data type ID</param>
        /// <param name="key">Key</param>
        /// <returns>Returns true if deletion is successful, false if failed.</returns>
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
        /// add or update
        /// </summary>
        /// <param name="msgId">Data type ID</param>
        /// <param name="key">Key</param>
        /// <param name="dataMessage">Data objects to be added</param>
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
        /// Extract dirty data from cache
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
