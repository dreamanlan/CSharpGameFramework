using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public sealed class SceneLogicInfoManager
    {
        public SceneLogicInfoManager(int poolSize)
        {
            m_SceneLogicInfoPoolSize = poolSize;
        }

        public void SetSceneContext(SceneContextInfo context)
        {
            m_SceneContext = context;
        }

        public LinkedListDictionary<int, SceneLogicInfo> SceneLogicInfos
        {
            get { return m_SceneLogicInfos; }
        }

        public SceneLogicInfo GetSceneLogicInfo(int id)
        {
            SceneLogicInfo info;
            SceneLogicInfos.TryGetValue(id, out info);
            return info;
        }

        public SceneLogicInfo GetSceneLogicInfoByConfigId(int id)
        {
            SceneLogicInfo ret = null;
            for (LinkedListNode<SceneLogicInfo> linkNode = m_SceneLogicInfos.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                SceneLogicInfo info = linkNode.Value;
                if (info.ConfigId == id) {
                    ret = info;
                    break;
                }
            }
            return ret;
        }

        public SceneLogicInfo AddSceneLogicInfo(int logicId)
        {
            SceneLogicInfo info = NewSceneLogicInfo();
            info.SceneContext = m_SceneContext;
            info.SceneLogicConfig = new SceneLogicConfig();
            info.SceneLogicConfig.m_LogicId = logicId;
            m_SceneLogicInfos.AddLast(info.GetId(), info);
            return info;
        }

        public SceneLogicInfo AddSceneLogicInfo(int id, int logicId)
        {
            SceneLogicInfo info = NewSceneLogicInfo(id);
            info.SceneContext = m_SceneContext;
            info.SceneLogicConfig = new SceneLogicConfig();
            info.SceneLogicConfig.m_LogicId = logicId;
            SceneLogicInfo oldInfo;
            if (m_SceneLogicInfos.TryGetValue(id, out oldInfo)) {
                LogSystem.Error("AddSceneLogicInfo error, Id={0} was exist, LogicId={1}, NewLogicId={2}", id, oldInfo.LogicId, logicId);
            }
            m_SceneLogicInfos.AddLast(info.GetId(), info);
            return info;
        }

        public SceneLogicInfo AddSceneLogicInfo(SceneLogicConfig cfg)
        {
            SceneLogicInfo info = NewSceneLogicInfo();
            info.SceneContext = m_SceneContext;
            info.SceneLogicConfig = cfg;
            m_SceneLogicInfos.AddLast(info.GetId(), info);
            return info;
        }

        public SceneLogicInfo AddSceneLogicInfo(int id, SceneLogicConfig cfg)
        {
            SceneLogicInfo info = NewSceneLogicInfo(id);
            info.SceneContext = m_SceneContext;
            info.SceneLogicConfig = cfg;
            m_SceneLogicInfos.AddLast(info.GetId(), info);
            return info;
        }

        public SceneLogicInfo DelayAddSceneLogicInfo(int logicId)
        {
            SceneLogicInfo info = NewSceneLogicInfo();
            info.SceneContext = m_SceneContext;
            info.SceneLogicConfig = new SceneLogicConfig();
            info.SceneLogicConfig.m_LogicId = logicId;
            m_DelayAdd.Add(info);
            return info;
        }

        public SceneLogicInfo DelayAddSceneLogicInfo(int id, int logicId)
        {
            SceneLogicInfo info = NewSceneLogicInfo(id);
            info.SceneContext = m_SceneContext;
            info.SceneLogicConfig = new SceneLogicConfig();
            info.SceneLogicConfig.m_LogicId = logicId;
            m_DelayAdd.Add(info);
            return info;
        }

        public SceneLogicInfo DelayAddSceneLogicInfo(SceneLogicConfig cfg)
        {
            SceneLogicInfo info = NewSceneLogicInfo();
            info.SceneContext = m_SceneContext;
            info.SceneLogicConfig = cfg;
            m_DelayAdd.Add(info);
            return info;
        }

        public SceneLogicInfo DelayAddSceneLogicInfo(int id, SceneLogicConfig cfg)
        {
            SceneLogicInfo info = NewSceneLogicInfo(id);
            info.SceneContext = m_SceneContext;
            info.SceneLogicConfig = cfg;
            m_DelayAdd.Add(info);
            return info;
        }

        public void ExecuteDelayAdd()
        {
            for (int ix = 0; ix < m_DelayAdd.Count; ++ix) {
                var i = m_DelayAdd[ix];
                m_SceneLogicInfos[i.GetId()] = i;
            }
            m_DelayAdd.Clear();
        }

        public void RemoveSceneLogicInfo(int id)
        {
            SceneLogicInfo info = GetSceneLogicInfo(id);
            if (null != info) {
                m_SceneLogicInfos.Remove(id);
                info.SceneContext = null;
                RecycleSceneLogicInfo(info);
            }
        }

        public void Reset()
        {
            m_SceneLogicInfos.Clear();
            m_DelayAdd.Clear();
            m_UnusedSceneLogicInfos.Clear();
            m_NextInfoId = c_StartId;
        }

        private SceneLogicInfo NewSceneLogicInfo()
        {
            SceneLogicInfo info = null;
            int id = GenNextId();
            if (m_UnusedSceneLogicInfos.Count > 0) {
                info = m_UnusedSceneLogicInfos.Dequeue();
                info.Reset();
                info.InitId(id);
            } else {
                info = new SceneLogicInfo(id);
            }
            return info;
        }

        private SceneLogicInfo NewSceneLogicInfo(int id)
        {
            SceneLogicInfo info = null;
            if (m_UnusedSceneLogicInfos.Count > 0) {
                info = m_UnusedSceneLogicInfos.Dequeue();
                info.Reset();
                info.InitId(id);
            } else {
                info = new SceneLogicInfo(id);
            }
            return info;
        }

        private void RecycleSceneLogicInfo(SceneLogicInfo logicInfo)
        {
            if (null != logicInfo && m_UnusedSceneLogicInfos.Count < m_SceneLogicInfoPoolSize) {
                logicInfo.Reset();
                m_UnusedSceneLogicInfos.Enqueue(logicInfo);
            }
        }

        private int GenNextId()
        {
            int startId = c_StartId;
            if (GlobalVariables.Instance.IsClient) {
                startId = c_StartId_Client;
            }
            int id = 0;
            for (int i = 0; i < c_MaxIdNum; ++i) {
                id = (m_NextInfoId + i - startId) % c_MaxIdNum + startId;
                if (!m_SceneLogicInfos.Contains(id))
                    break;
            }
            if (id > 0) {
                m_NextInfoId = (id + 1 - startId) % c_MaxIdNum + startId;
            }
            return id;
        }

        private LinkedListDictionary<int, SceneLogicInfo> m_SceneLogicInfos = new LinkedListDictionary<int, SceneLogicInfo>();
        private List<SceneLogicInfo> m_DelayAdd = new List<SceneLogicInfo>();
        private Queue<SceneLogicInfo> m_UnusedSceneLogicInfos = new Queue<SceneLogicInfo>();
        private int m_SceneLogicInfoPoolSize = 128;

        private const int c_StartId = 1100;
        private const int c_StartId_Client = 3000;
        private const int c_MaxIdNum = 100;
        private int m_NextInfoId = c_StartId;

        private SceneContextInfo m_SceneContext = null;
    }
}
