using System;
using System.Collections.Generic;

namespace GameFramework
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
      // 在这里初始化所有场景逻辑，并注册到全局字典里。
      m_SceneLogics.Add((int)SceneLogicId.TIME_OUT, new SceneLogic_Timeout());
      m_SceneLogics.Add((int)SceneLogicId.SAND_CLOCK, new SceneLogic_SandClock());
    }

    private MyDictionary<int, ISceneLogic> m_SceneLogics = new MyDictionary<int, ISceneLogic>(); // 场景逻辑容器

    #region Singleton
    private static SceneLogicManager s_Instance = new SceneLogicManager();
    public static SceneLogicManager Instance
    {
      get { return s_Instance; }
    }
    #endregion
  }
}
