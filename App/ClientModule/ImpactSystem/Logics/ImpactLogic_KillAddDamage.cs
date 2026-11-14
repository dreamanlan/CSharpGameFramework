using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 17: bonus damage on the hits following a kill (param0 = bonus, param1 = number of hits).
    internal sealed class ImpactLogic_KillAddDamage : AbstractImpactLogic
    {
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            if (null != info && null != info.Cfg()) {
                ImpactHitCounter.SetKillBonus(info.ImpactSenderId, info.Cfg().GetIntParam(0, 0), info.Cfg().GetIntParam(1, 1));
            }
            base.StartImpact(obj, impactId);
        }
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            if (null == obj || impactOwnerId != senderId) return hpDamage;
            int hitId = ImpactHitCounter.LastHitIdOf(senderId);
            int add = ImpactHitCounter.TakeKillAddDamage(senderId, hitId);
            if (add > 0) return hpDamage + add;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != info) info.IsActivated = false;
            return hpDamage;
        }
        public override void StopImpact(EntityInfo obj, int impactId)
        {
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            if (null == info) return;
            ImpactHitCounter.ClearKillBonus(info.ImpactSenderId);
            info.IsActivated = false;
            base.StopImpact(obj, impactId);
        }
    }
}
