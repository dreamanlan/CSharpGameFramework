using System;
using System.Collections.Generic;

namespace ScriptableFramework
{
    public sealed class SceneLogicManager
    {
        public ISceneLogic GetSceneLogic(int id)
        {
            ISceneLogic logic = null;
            m_SceneLogics.TryGetValue(id, out logic);
            return logic;
        }

        private SceneLogicManager()
        {
            // All scene logic is initialized here and registered in the global dictionary.
            m_SceneLogics.Add((int)SceneLogicId.TIME_OUT, new SceneLogic_Timeout());
            m_SceneLogics.Add((int)SceneLogicId.SAND_CLOCK, new SceneLogic_SandClock());
        }

        private MyDictionary<int, ISceneLogic> m_SceneLogics = new MyDictionary<int, ISceneLogic>(); // Scene logic container

        #region Singleton
        private static SceneLogicManager s_Instance = new SceneLogicManager();
        public static SceneLogicManager Instance
        {
            get { return s_Instance; }
        }
        #endregion
    }
}
