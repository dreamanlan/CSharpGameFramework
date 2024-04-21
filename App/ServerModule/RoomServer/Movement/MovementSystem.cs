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
                    for (LinkedListNode<EntityInfo> node = m_EntityMgr.Entities.FirstNode; null != node; node = node.Next) {
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
            if (obj.HaveState(CharacterPropertyEnum.x3002_昏睡)
              || obj.HaveState(CharacterPropertyEnum.x3001_眩晕)) {
                return;
            }
            MovementStateInfo msi = obj.GetMovementStateInfo();
            //NPCs ignore blocking and avoidance when performing movement.
            //These behaviors are performed by the AI module when planning its path.
            if (!obj.IsDead() && obj.CanMove && msi.IsMoving) {
                ScriptRuntime.Vector3 pos = msi.GetPosition3D();
                float speed = (float)obj.ActualProperty.GetFloat(CharacterPropertyEnum.x2011_最终速度);
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
