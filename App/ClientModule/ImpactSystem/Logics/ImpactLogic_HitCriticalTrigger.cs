using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 12: every Nth hit is a guaranteed critical (param0 = N).
    internal sealed class ImpactLogic_HitCriticalTrigger : AbstractImpactLogic
    {
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            if (senderId != impactOwnerId) return hpDamage;
            EntityInfo sender = null != obj ? ImpactCompat.GetEntity(obj.SceneContext, senderId) : null;
            if (null == sender || !ImpactCompat.IsUser(sender)) return hpDamage;
            int triggerCount = Param(obj, impactId);
            if (triggerCount <= 0) return hpDamage;
            int count = ImpactHitCounter.CountOf(senderId);
            if (count != 0 && count % triggerCount == 0 && !isCritical) {
                float pow = sender.ActualProperty.GetFloat(CharacterPropertyEnum.x1033_暴击伤害);
                if (pow < 0.01f) pow = 1.5f;
                hpDamage = (int)(hpDamage * pow);
                isCritical = true;
            }
            return hpDamage;
        }
        private static int Param(EntityInfo obj, int impactId)
        {
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            return null != info && null != info.Cfg() ? info.Cfg().GetIntParam(0, 0) : 0;
        }
    }
}
