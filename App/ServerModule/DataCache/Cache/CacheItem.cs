using System;
using System.Collections.Generic;

namespace ScriptableFramework.DataCache
{
    internal enum DirtyState
    {
        Unsaved = -1,
        Saving = 0,
        Saved = 1
    }
    /// <summary>
    /// Datastore cache data entry type
    /// The Protobuf object corresponds to a row of data in the database table
    /// </summary>
    internal class InnerCacheItem
    {
        internal InnerCacheItem(byte[] dataMessage)
        {
            m_DataMessage = dataMessage;
            m_Valid = true;
            m_CacheVersion = InnerCacheSystem.InitialCacheVersion;
            m_LifeCount = s_MaxLifeCount;
        }
        internal bool Valid
        {
            get { return m_Valid; }
            set { m_Valid = value; }
        }
        internal DirtyState DirtyState
        {
            get { return m_DirtyState; }
            set
            {
                if (value == DirtyState.Saved && m_DirtyState == DirtyState.Unsaved) {
                    //It is not allowed to change directly from Unsaved state to Saved state.
                    m_DirtyState = DirtyState.Unsaved;
                    //LogSys.Log(ServerLogType.MONITOR, "Switch DirtyState from Unsaved to Saved is forbidden! DataMessage:{0}", m_DataMessage.GetType().Name);
                } else {
                    m_DirtyState = value;
                }
                if (m_DirtyState == DirtyState.Unsaved) {
                    //Only reset life count when data is dirty
                    m_LifeCount = s_MaxLifeCount;
                }
            }
        }
        internal long CacheVersion
        {
            get { return m_CacheVersion; }
            set { m_CacheVersion = value; }
        }
        internal int LifeCount
        {
            get { return m_LifeCount; }
        }
        internal byte[] DataMessage
        {
            get { return m_DataMessage; }
            set { m_DataMessage = value; }
        }
        internal void DecreaseLifeCount()
        {
            m_LifeCount--;
        }

        private static int s_MaxLifeCount = 10;     //The maximum life count of data. Unupdated cached data will be cleared after approximately 10*10 minutes.

        private bool m_Valid = true;                //Identification of whether the data is valid
        private DirtyState m_DirtyState = DirtyState.Unsaved;
        private long m_CacheVersion = InnerCacheSystem.InitialCacheVersion;   //Cache data data version number:-1 > n > ... > 0
        private int m_LifeCount = s_MaxLifeCount;   //Data life count, when the data is updated (m_Dirty=true) the count is set to max, decremented by 1 for each Tick
        private byte[] m_DataMessage = null;        //Data protobuf byte stream, TODO: change it to object?
    }
}
