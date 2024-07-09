using System;
using System.Collections;

namespace ScriptableFramework
{
  internal sealed class SceneLogicViewModelManager
  {
    internal void Init()
    {
        //Add each view instance
        m_Views.Add(new SceneLogicViewModel_General());
    }

    private SceneLogicViewModelManager() { }

    private ArrayList m_Views = new ArrayList();

    internal static SceneLogicViewModelManager Instance
    {
      get { return s_Instance; }
    }
    private static SceneLogicViewModelManager s_Instance = new SceneLogicViewModelManager();
  }
}
