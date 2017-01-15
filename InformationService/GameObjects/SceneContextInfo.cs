using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public delegate void HighlightPromptDelegation(int userId, string dict, object[] args);
    public sealed class SceneContextInfo
    {
        public HighlightPromptDelegation OnHighlightPrompt;

        public KdObjectTree KdTree
        {
            get { return m_KdTree; }
            set { m_KdTree = value; }
        }
        public SceneLogicInfoManager SceneLogicInfoManager
        {
            get { return m_SceneLogicInfoManager; }
            set { m_SceneLogicInfoManager = value; }
        }
        public EntityManager EntityManager
        {
            get { return m_EntityMgr; }
            set { m_EntityMgr = value; }
        }
        public BlackBoard BlackBoard
        {
            get { return m_BlackBoard; }
            set { m_BlackBoard = value; }
        }
        public List<CalculatorCommandInfo> CommandInfos
        {
            get { return m_CommandInfos; }
        }
        public int SceneResId
        {
            get { return m_SceneResId; }
            set { m_SceneResId = value; }
        }
        public bool IsRunWithRoomServer
        {
            get { return m_IsRunWithRoomServer; }
            set { m_IsRunWithRoomServer = value; }
        }
        public long StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }
        public object CustomData
        {
            get { return m_CustomData; }
            set { m_CustomData = value; }
        }
        public EntityInfo GetEntityById(int id)
        {
            EntityInfo info = null;
            if (null != m_EntityMgr) {
                info = m_EntityMgr.GetEntityInfo(id);
            }
            return info;
        }
        public EntityInfo GetEntityByUnitId(int unitId)
        {
            EntityInfo info = null;
            if (null != m_EntityMgr) {
                info = m_EntityMgr.GetEntityInfoByUnitId(unitId);
            }
            return info;
        }
        public void HighlightPromptAll(string dict, params object[] args)
        {
            HighlightPrompt(0, dict, args);
        }
        public void HighlightPrompt(int userId, string dict, params object[] args)
        {
            if (null != OnHighlightPrompt) {
                OnHighlightPrompt(userId, dict, args);
            }
        }
        public void ResetUniqueId()
        {
            m_NextSceneUniqueId = 1;
            m_ObjectLocalVariables.Clear();
        }
        public int GenUniqueId()
        {
            return m_NextSceneUniqueId++;
        }
        public void ObjectSet(int id, string name, object val)
        {
            Dictionary<string, object> vals;
            if (m_ObjectLocalVariables.TryGetValue(id, out vals)) {
                if (vals.ContainsKey(name)) {
                    vals[name] = val;
                } else {
                    vals.Add(name, val);
                }
            } else {
                vals = new Dictionary<string, object>();
                vals.Add(name, val);
                m_ObjectLocalVariables.Add(id, vals);
            }
        }
        public bool ObjectTryGet(int id, string name, out object val)
        {
            bool ret = false;
            Dictionary<string, object> vals;
            if (m_ObjectLocalVariables.TryGetValue(id, out vals)) {
                ret = vals.TryGetValue(name, out val);
            } else {
                val = null;
            }
            return ret;
        }

        private int m_NextSceneUniqueId = 1;
        private Dictionary<int, Dictionary<string, object>> m_ObjectLocalVariables = new Dictionary<int, Dictionary<string, object>>();
        private List<CalculatorCommandInfo> m_CommandInfos = new List<CalculatorCommandInfo>();

        private KdObjectTree m_KdTree = null;
        private SceneLogicInfoManager m_SceneLogicInfoManager = null;
        private EntityManager m_EntityMgr = null;
        private BlackBoard m_BlackBoard = null;
        private int m_SceneResId = 0;
        private bool m_IsRunWithRoomServer = true;
        private long m_StartTime = 0;
        private object m_CustomData = null;
    }
}
