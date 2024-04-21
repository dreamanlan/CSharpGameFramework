using System;
using System.Collections.Generic;
using GameFramework;

namespace GameFramework.DataCache
{
    /// <summary>
    /// Data storage cache data table type
    /// Data table corresponding to the database
    /// </summary>
    internal class InnerCacheTable
    {
        /// <summary>
        /// Primary key query
        /// </summary>
        /// <param name="key">primary key</param>
        /// <returns>If the unique corresponding data value is found, return null otherwise.</returns>
        internal InnerCacheItem Find(KeyString key)
        {
            InnerCacheItem cacheItem = null;
            m_PrimaryDict.TryGetValue(key, out cacheItem);
            if (cacheItem != null && cacheItem.Valid) {
                return cacheItem;
            } else {
                return null;
            }
        }
        /// <summary>
        /// Foreign key query
        /// </summary>
        /// <param name="foreignKey">foreign key</param>
        /// <returns>List of data values corresponding to the foreign key</returns>
        internal List<InnerCacheItem> FindByForeignKey(KeyString foreignKey)
        {
            List<InnerCacheItem> cacheItemList = new List<InnerCacheItem>();
            HashSet<KeyString> associateKeys = null;
            m_ForeignPrimaryDict.TryGetValue(foreignKey, out associateKeys);
            if (associateKeys != null) {
                foreach (var key in associateKeys) {
                    InnerCacheItem cacheItem = this.Find(key);
                    if (cacheItem != null && cacheItem.Valid) {
                        cacheItemList.Add(cacheItem);
                    }
                }
            }
            return cacheItemList;
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="key">primary key</param>
        /// <returns>Returns true if the deletion is successful, false if it fails.</returns>
        internal bool Remove(KeyString key)
        {
            KeyString foreignKey = null;
            m_PrimaryForeignDict.TryGetValue(key, out foreignKey);
            if (!KeyString.IsNullOrEmpty(foreignKey)) {
                HashSet<KeyString> priKeys = null;
                m_ForeignPrimaryDict.TryGetValue(foreignKey, out priKeys);
                if (priKeys != null) {
                    priKeys.Remove(key);
                }
                m_ForeignPrimaryDict.Remove(foreignKey);
            }
            m_PrimaryForeignDict.Remove(key);
            return m_PrimaryDict.Remove(key);
        }
        /// <summary>
        /// add or update
        /// </summary>
        /// <param name="key">Primary key, cannot be null</param>
        /// <param name="foreignKey">Foreign key, can be null</param>
        /// <param name="dataMessage">data value</param>
        /// <param name="dataVersion">Data version</param>
        internal void AddOrUpdate(KeyString key, KeyString foreignKey, byte[] dataMessage, long cacheVersion)
        {
            InnerCacheItem cacheItem = null;
            m_PrimaryDict.TryGetValue(key, out cacheItem);
            if (cacheItem == null) {
                //Data is not in cache
                if (dataMessage == null) {
                    //Deleted data
                    return;
                }
                cacheItem = new InnerCacheItem(dataMessage);
                m_PrimaryDict.Add(key, cacheItem);
                if (!KeyString.IsNullOrEmpty(foreignKey)) {
                    m_PrimaryForeignDict[foreignKey] = key;
                    HashSet<KeyString> associateKeys = null;
                    m_ForeignPrimaryDict.TryGetValue(foreignKey, out associateKeys);
                    if (associateKeys != null) {
                        associateKeys.Add(key);
                    } else {
                        associateKeys = new HashSet<KeyString>();
                        associateKeys.Add(key);
                        m_ForeignPrimaryDict.Add(foreignKey, associateKeys);
                    }
                }
            } else {
                //Data is already in cache
                if (CompareDataVersion(cacheVersion, cacheItem.CacheVersion)) {
                    //refresh cache
                    cacheItem.DirtyState = DirtyState.Unsaved;
                    cacheItem.Valid = true;
                    cacheItem.CacheVersion = cacheVersion;
                    if (dataMessage == null) {
                        cacheItem.Valid = false;
                    } else {
                        cacheItem.DataMessage = dataMessage;
                    }
                    //Update foreign key relationship
                    if (!KeyString.IsNullOrEmpty(foreignKey)) {
                        m_PrimaryForeignDict[foreignKey] = key;
                        HashSet<KeyString> associateKeys = null;
                        m_ForeignPrimaryDict.TryGetValue(foreignKey, out associateKeys);
                        if (associateKeys != null) {
                            associateKeys.Add(key);
                        } else {
                            associateKeys = new HashSet<KeyString>();
                            associateKeys.Add(key);
                            m_ForeignPrimaryDict.Add(foreignKey, associateKeys);
                        }
                    }
                } else {
                    //The dataVersion of the new data is low, the cache is not updated, and a warning log is output.
                    LogSys.Log(ServerLogType.WARN, "Update cache version WARN. Key:{0}, NewCacheVersion:{1}, OldCacheVersion:{2}", key, cacheVersion, cacheItem.CacheVersion);
                }
            }
        }
        /// <summary>
        /// Return dirty data collection
        /// </summary>
        /// <returns></returns>
        internal List<InnerCacheItem> GetDirtyCacheItems()
        {
            List<InnerCacheItem> dirtyCacheItemList = new List<InnerCacheItem>();
            foreach (var dvPair in m_PrimaryDict) {
                if (dvPair.Value.DirtyState == DirtyState.Unsaved) {
                    dirtyCacheItemList.Add(dvPair.Value);
                    dvPair.Value.DirtyState = DirtyState.Saving;
                }
            }
            return dirtyCacheItemList;
        }

        /// <summary>
        /// Clean expired data cache
        /// </summary>
        /// <returns>Number of caches cleared</returns>
        internal int CleanExpiredItems()
        {
            List<KeyString> deleteKeys = new List<KeyString>();
            foreach (var data in m_PrimaryDict) {
                data.Value.DecreaseLifeCount();
                if (data.Value.DirtyState == DirtyState.Saved && (data.Value.Valid == false || data.Value.LifeCount < 0)) {
                    deleteKeys.Add(data.Key);
                }
            }
            foreach (var key in deleteKeys) {
                this.Remove(key);
            }
            return deleteKeys.Count;
        }
        /// <summary>
        /// Compare the priority of two DataVersion
        /// </summary>
        /// <param name="newVersion">DataVersion of new data</param>
        /// <param name="oldVersion">DataVersion of existing data</param>
        /// <returns>true: the new data version is higher; false: the new data version is lower</returns>
        private bool CompareDataVersion(long newVersion, long oldVersion)
        {
            if (newVersion == InnerCacheSystem.UltimateCacheVersion) {
                return true;
            } else if (newVersion >= oldVersion) {
                return true;
            } else {
                return false;
            }
        }

        private Dictionary<KeyString, InnerCacheItem> m_PrimaryDict = new Dictionary<KeyString, InnerCacheItem>();                //primaryKey-DataItem
        private Dictionary<KeyString, KeyString> m_PrimaryForeignDict = new Dictionary<KeyString, KeyString>();                   //primaryKey-foreignKey
        private Dictionary<KeyString, HashSet<KeyString>> m_ForeignPrimaryDict = new Dictionary<KeyString, HashSet<KeyString>>(); //foreignKey-primaryKeys
    }
}
