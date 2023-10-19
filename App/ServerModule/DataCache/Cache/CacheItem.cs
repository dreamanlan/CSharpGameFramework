using System;
using System.Collections.Generic;

namespace GameFramework.DataCache
{
    internal enum DirtyState
    {
        Unsaved = -1,
        Saving = 0,
        Saved = 1
    }
    /// <summary>
    /// 数据存储缓存数据条目类型
    /// Protobuf对象对应数据库表中的一行数据
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
                    //不允许由Unsaved状态直接变成Saved状态
                    m_DirtyState = DirtyState.Unsaved;
                    //LogSys.Log(ServerLogType.MONITOR, "Switch DirtyState from Unsaved to Saved is forbidden! DataMessage:{0}", m_DataMessage.GetType().Name);
                } else {
                    m_DirtyState = value;
                }
                if (m_DirtyState == DirtyState.Unsaved) {
                    //只有数据为脏时重置生命计数
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

        private static int s_MaxLifeCount = 10;     //数据的最大生命计数,无更新的缓存数据大约在10*10分钟后被清除

        private bool m_Valid = true;                //数据是否有效标识
        private DirtyState m_DirtyState = DirtyState.Unsaved;
        private long m_CacheVersion = InnerCacheSystem.InitialCacheVersion;   //缓存数据数据版本号:-1 > n > ... > 0
        private int m_LifeCount = s_MaxLifeCount;   //数据生命计数，数据被更新时（m_Dirty=true）计数设置为max，每个Tick中减1   
        private byte[] m_DataMessage = null;        //数据protobuf字节流,TODO:改成对象?
    }
}
