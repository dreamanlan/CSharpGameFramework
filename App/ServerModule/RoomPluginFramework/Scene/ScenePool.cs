using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
  public sealed class ScenePool
  {
    public Scene NewScene()
    {
      Scene scene = null;
      if (m_UnusedScenes.Count > 0) {
        scene = m_UnusedScenes.Dequeue();
      } else {
        scene = new Scene();
      }
      return scene;
    }
    public void RecycleScene(Scene scene)
    {
      m_UnusedScenes.Enqueue(scene);
    }

    private Queue<Scene> m_UnusedScenes = new Queue<Scene>();
  }
}
