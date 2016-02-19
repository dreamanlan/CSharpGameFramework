using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
  internal sealed class AiViewManager
  {
    internal void Init()
    {
      m_AiViews.Add(new AiView_NpcGeneral());
    }
    private ArrayList m_AiViews = new ArrayList();
    internal static AiViewManager Instance
    {
      get { return s_Instance; }
    }
    private static AiViewManager s_Instance = new AiViewManager();
  }
}
