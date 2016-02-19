using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace GameFramework
{
  public class MovementSystem
  {
    public const float c_ForecastDistance = 0.75f;
    public MovementSystem()
    {
    }
    public void Reset()
    {
      m_LastTickTime = 0;
    }
    public void SetEntityManager(EntityManager npcMgr)
    {
      m_EntityMgr = npcMgr;
    }
    public void Tick()
    {
      if (0 == m_LastTickTime) {
        m_LastTickTime = TimeUtility.GetLocalMilliseconds();
        LogSystem.Info("MovementSystem LastTickTime:{0}", m_LastTickTime);
      } else {
        long delta = TimeUtility.GetLocalMilliseconds() - m_LastTickTime;
        m_LastTickTime = TimeUtility.GetLocalMilliseconds();
        if (delta > 1000)
          delta = 1000;
        if (null != m_EntityMgr) {
          for (LinkedListNode<EntityInfo> node = m_EntityMgr.Entities.FirstValue; null != node; node = node.Next) {
            EntityInfo npc = node.Value;
            if (null != npc) {
              MoveNpc(npc, delta);
            }
          }
        }
      }
    }

    private void MoveNpc(EntityInfo obj, long deltaTime)
    {
      if (obj.IsHaveStateFlag(CharacterState_Type.CST_Sleep)
        || obj.IsHaveStateFlag(CharacterState_Type.CST_FixedPosition)) {
        return;
      }
      MovementStateInfo msi = obj.GetMovementStateInfo();
      //npc执行移动时忽略阻挡与避让，这些行为由ai模块在规划其路径时执行。
      if (!obj.IsDead() && obj.CanMove && msi.IsMoving && !msi.IsSkillMoving) {
        ScriptRuntime.Vector3 pos = msi.GetPosition3D();
        float cos_angle = (float)msi.MoveDirCosAngle;
        float sin_angle = (float)msi.MoveDirSinAngle;
        float speed = (float)obj.GetActualProperty().MoveSpeed;
        float distance = (speed * (float)(int)deltaTime) / 1000.0f;
        
        //LogSystem.Debug("MovementSystem npc:{0} speed:{1} deltaTime:{2} distance:{3}", obj.GetId(), speed, deltaTime, distance);

        float x = 0, y = 0;
        if (msi.CalcDistancSquareToTarget() < distance * distance) {
          x = msi.TargetPosition.X;
          y = msi.TargetPosition.Z;
          ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(x, y);
          msi.SetPosition2D(newPos);
        } else {
          float len = pos.Length();
          y = pos.Z + distance * cos_angle;
          x = pos.X + distance * sin_angle;
          ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(x, y);
          msi.SetPosition2D(newPos);
        }
      }
    }

    private long m_LastTickTime = 0;
    private EntityManager m_EntityMgr = null;
  }
}
