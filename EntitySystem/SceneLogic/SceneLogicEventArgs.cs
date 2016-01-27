using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashFire
{
  public class SceneLogicEventArgs : EventArgs
  {
    public int Index = 0;
    public int EntityId = -1;
    public int SkillId = -1;
    public List<int> CharacterList = new List<int>();
    public int TargetId = -1;
    public double DeltaTime;
    public ScriptRuntime.Vector3 TargetPos;
  }
}
