using System;
using System.Collections.Generic;
using GameFramework;

namespace GameFramework.DataCache
{
    /// <summary>
    /// 数据存储缓存数据表类型
    /// 对应数据库的数据表
    /// </summary>
    internal class InnerCacheTable
    {
        /// <summary>
        /// 主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>若查找到返回唯一对应数据值，否则返回null</returns>
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
        /// 外键查询
        /// </summary>
        /// <param name="foreignKey">外键</param>
        /// <returns>外键所对应的数据值列表</returns>
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
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>删除成功返回true，失败为false</returns>
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
        /// 添加或更新
        /// </summary>
        /// <param name="key">主键,不能为空</param>
        /// <param name="foreignKey">外键，可以为空</param>
        /// <param name="dataMessage">数据值</param>
        /// <param name="dataVersion">数据版本</param>
        internal void AddOrUpdate(KeyString key, KeyString foreignKey, byte[] dataMessage, long cacheVersion)
        {
            InnerCacheItem cacheItem = null;
            m_PrimaryDict.TryGetValue(key, out cacheItem);
            if (cacheItem == null) {
                //数据不在缓存中
                if (dataMessage == null) {
                    //被删除的数据
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
                //数据已在缓存中
                if (CompareDataVersion(cacheVersion, cacheItem.CacheVersion)) {
                    //更新缓存
                    cacheItem.DirtyState = DirtyState.Unsaved;
                    cacheItem.Valid = true;
                    cacheItem.CacheVersion = cacheVersion;
                    if (dataMessage == null) {
                        cacheItem.Valid = false;
                    } else {
                        cacheItem.DataMessage = dataMessage;
                    }
                    //更新外键关系
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
                    //新数据的dataVersion低,不更新缓存,输出警告日志
                    LogSys.Log(ServerLogType.WARN, "Update cache version WARN. Key:{0}, NewCacheVersion:{1}, OldCacheVersion:{2}", key, cacheVersion, cacheItem.CacheVersion);
                }
            }
        }
        /// <summary>
        /// 返回脏数据集合
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
        /// 清理过期的数据缓存
        /// </summary>
        /// <returns>清理的缓存数目</returns>
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
        /// 比较两个DataVersion的优先级
        /// </summary>
        /// <param name="newVersion">新数据的DataVersion</param>
        /// <param name="oldVersion">已有数据的DataVersion</param>
        /// <returns>true:新数据版本更高;false:新数据的版本低</returns>
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
