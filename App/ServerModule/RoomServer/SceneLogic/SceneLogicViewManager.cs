using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace GameFramework
{
  internal sealed class SceneLogicViewManager
  {
    internal void Init()
    {
      //添加各个view实例
      m_Views.Add(new SceneLogicView_General());
    }

    private SceneLogicViewManager() { }

    private ArrayList m_Views = new ArrayList();

    internal static SceneLogicViewManager Instance
    {
      get { return s_Instance; }
    }
    private static SceneLogicViewManager s_Instance = new SceneLogicViewManager();
  }
}
