using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 19: force nearby hostile NPCs to attack the owner (param0 = radius).
    internal sealed class ImpactLogic_Taunt : AbstractImpactLogic
    {
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != info && info.IsActivated && null != obj.SceneContext && null != obj.SceneContext.EntityManager) {
                float radius = null != info.Cfg() ? info.Cfg().GetFloatParam(0, 0.0f) : 0.0f;
                EntityManager mgr = obj.SceneContext.EntityManager;
                for (LinkedListNode<EntityInfo> node = mgr.Entities.FirstNode; null != node; node = node.Next) {
                    EntityInfo npc = node.Value;
                    if (null == npc || npc == obj || !ImpactCompat.IsNpc(npc)
                        || EntityInfo.GetRelation(npc.GetCampId(), obj.GetCampId()) != CharacterRelation.RELATION_ENEMY) continue;
                    if (radius <= 0.0f || ImpactCompat.DistanceSquare(obj, npc) <= radius * radius) {
                        AiStateInfo ai = npc.GetAiStateInfo();
                        if (null != ai) ai.HateTarget = obj.GetId();
                    }
                }
            }
            base.Tick(obj, impactId);
        }
    }
}
