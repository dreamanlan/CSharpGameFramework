using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace GameFramework
{
    public sealed class SceneLogicViewManager
    {
        public void Init()
        {
            //Add each view instance
            m_Views.Add(new SceneLogicView_General());
        }

        private SceneLogicViewManager() { }

        private ArrayList m_Views = new ArrayList();

        public static SceneLogicViewManager Instance
        {
            get { return s_Instance; }
        }
        private static SceneLogicViewManager s_Instance = new SceneLogicViewManager();
    }
}
