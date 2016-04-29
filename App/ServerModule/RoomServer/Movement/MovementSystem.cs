using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using GameFrameworkMessage;

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
                float speed = (float)obj.GetActualProperty().MoveSpeed;
                float distance = (speed * (float)(int)deltaTime) / 1000.0f;
                ScriptRuntime.Vector3 dir = msi.TargetDir;
                
        //LogSystem.Debug("MovementSystem npc:{0} speed:{1} deltaTime:{2} distance:{3}", obj.GetId(), speed, deltaTime, distance);

                float x = 0, y = 0;
                if (msi.CalcDistancSquareToTarget() < distance * distance) {
                    x = msi.TargetPosition.X;
                    y = msi.TargetPosition.Z;
                    ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(x, y);
                    msi.SetPosition2D(newPos);

                    msi.IsMoving = false;
                    User user = obj.CustomData as User;
                    if (null != user) {
                        Msg_RC_NpcMove npcMoveBuilder = DataSyncUtility.BuildNpcMoveMessage(obj);
                        if (null != npcMoveBuilder) {
                            Scene scene = user.OwnRoom.GetActiveScene();
                            if (null != scene) {
                                scene.NotifyAllUser(RoomMessageDefine.Msg_RC_NpcMove, npcMoveBuilder);
                            }
                        }
                    }
                } else {
                    ScriptRuntime.Vector3 tpos = pos + dir * distance;
                    msi.SetPosition(tpos);
                }
            }
        }

    private long m_LastTickTime = 0;
    private EntityManager m_EntityMgr = null;
  }
}
