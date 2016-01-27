using System;
using System.Collections;

namespace GameFramework
{
  internal sealed class SceneLogicViewModelManager
  {
    internal void Init()
    {
      //添加各个view实例
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
