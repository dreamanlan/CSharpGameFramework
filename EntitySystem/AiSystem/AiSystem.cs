using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public sealed class AiSystem
    {
        public void Reset()
        {
            m_LastTickTime = 0;
        }
        public void SetEntityManager(EntityManager npcMgr)
        {
            m_NpcMgr = npcMgr;
        }
        public void Tick()
        {
            if (0 == m_LastTickTime) {
                m_LastTickTime = TimeUtility.GetLocalMilliseconds();
            } else {
                long delta = TimeUtility.GetLocalMilliseconds() - m_LastTickTime;
                m_LastTickTime = TimeUtility.GetLocalMilliseconds();
                if (null != m_NpcMgr) {
                    for (LinkedListNode<EntityInfo> linkNode = m_NpcMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                        EntityInfo entity = linkNode.Value;
                        TickNpc(entity, delta);
                    }
                }
            }
        }

        private void TickNpc(EntityInfo entity, long delta)
        {
            IAiStateLogic logic = AiLogicManager.Instance.GetNpcStateLogic(entity.GetAiStateInfo().AiLogic);
            if (null != logic) {
                logic.Execute(entity, delta);
            }
        }

        private long m_LastTickTime = 0;
        private EntityManager m_NpcMgr = null;
    }
}
