using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class SceneLogicConfig
    {
        public int m_ConfigId = 0;
        public int m_LogicId = 0;
        public string[] m_Params = null;
    }
    public enum SceneLogicId
    {
        TIME_OUT = 10001,
        SAND_CLOCK = 10002,
    }

    public class SceneLogicInfo
    {
        public int DataId
        {
            get { return m_DataId; }
            set { m_DataId = value; }
        }
        public int GetId()
        {
            return m_Id;
        }
        public int ConfigId
        {
            get
            {
                int id = 0;
                if (null != m_SceneLogicConfig) {
                    id = m_SceneLogicConfig.m_LogicId;
                }
                return id;
            }
        }
        public int LogicId
        {
            get
            {
                int id = 0;
                if (null != m_SceneLogicConfig) {
                    id = m_SceneLogicConfig.m_LogicId;
                }
                return id;
            }
        }
        public SceneLogicConfig SceneLogicConfig
        {
            get { return m_SceneLogicConfig; }
            set { m_SceneLogicConfig = value; }
        }
        public bool IsLogicFinished
        {
            get { return m_IsLogicFinished; }
            set { m_IsLogicFinished = value; }
        }
        public bool IsLogicPaused
        {
            get { return m_IsLogicPaused; }
            set { m_IsLogicPaused = value; }
        }
        public SceneContextInfo SceneContext
        {
            get { return m_SceneContext; }
            set { m_SceneContext = value; }
        }
        public SceneLogicInfoManager SceneLogicInfoManager
        {
            get
            {
                SceneLogicInfoManager mgr = null;
                if (null != m_SceneContext) {
                    mgr = m_SceneContext.SceneLogicInfoManager;
                }
                return mgr;
            }
        }
        public EntityManager NpcManager
        {
            get
            {
                EntityManager mgr = null;
                if (null != m_SceneContext) {
                    mgr = m_SceneContext.EntityManager;
                }
                return mgr;
            }
        }
        public BlackBoard BlackBoard
        {
            get
            {
                BlackBoard blackBoard = null;
                if (null != m_SceneContext) {
                    blackBoard = m_SceneContext.BlackBoard;
                }
                return blackBoard;
            }
        }
        public long Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }
        public TypedDataCollection LogicDatas
        {
            get { return m_LogicDatas; }
        }
        public SceneLogicInfo(int id)
        {
            m_Id = id;
            m_IsLogicFinished = false;
        }
        public void InitId(int id)
        {
            m_Id = id;
        }
        public void Reset()
        {
            m_Time = 0;
            m_IsLogicFinished = false;
            m_LogicDatas.Clear();
        }
        private int m_Id = 0;
        private int m_DataId = 0;
        private SceneLogicConfig m_SceneLogicConfig = null;
        private bool m_IsLogicFinished = false;
        private bool m_IsLogicPaused = false;
        private SceneContextInfo m_SceneContext = null;
        private long m_Time = 0;//由于场景逻辑主要在Tick里工作，通常需要限制工作的频率，这一数据用于此目的（由于LogicDatas的读取比较费，所以抽出来放公共里）
        private TypedDataCollection m_LogicDatas = new TypedDataCollection();
    }
}
