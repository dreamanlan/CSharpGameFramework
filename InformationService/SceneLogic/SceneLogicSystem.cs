using System;
using System.Collections.Generic;

namespace GameFramework
{
    public sealed class SceneLogicSystem
    {
        public void SetSceneLogicInfoManager(SceneLogicInfoManager mgr)
        {
            m_SceneLogicInfoMgr = mgr;
        }
        public SceneLogicInfoManager GetSceneLogicInfoManager()
        {
            return m_SceneLogicInfoMgr;
        }
        public void Reset()
        {
            m_LastTickTime = 0;
        }
        public void Tick()
        {
            if (null == m_SceneLogicInfoMgr)
                return;
            if (0 == m_LastTickTime) {
                m_LastTickTime = TimeUtility.GetLocalMilliseconds();
            } else {
                long delta = TimeUtility.GetLocalMilliseconds() - m_LastTickTime;
                m_LastTickTime = TimeUtility.GetLocalMilliseconds();
                for (LinkedListNode<SceneLogicInfo> node = m_SceneLogicInfoMgr.SceneLogicInfos.FirstValue; null != node; node = node.Next) {
                    SceneLogicInfo info = node.Value;
                    if (null != info) {
                        ISceneLogic logic = SceneLogicManager.Instance.GetSceneLogic(info.LogicId);
                        if (null != logic) {
                            logic.Execute(info, delta);
                        }
                        if (info.IsLogicFinished) {
                            m_SceneLogicInfos.Add(info);
                        }
                    }
                }
                for (int i = 0; i < m_SceneLogicInfos.Count; i++) {
                    m_SceneLogicInfoMgr.RemoveSceneLogicInfo(m_SceneLogicInfos[i].GetId());
                }
                m_SceneLogicInfos.Clear();
                m_SceneLogicInfoMgr.ExecuteDelayAdd();
            }
        }

        private long m_LastTickTime = 0;
        private SceneLogicInfoManager m_SceneLogicInfoMgr = null;
        private List<SceneLogicInfo> m_SceneLogicInfos = new List<SceneLogicInfo>();
    }
}
