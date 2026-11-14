using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 18: scale damage by skill level (param0 = sign, param1 = basis, param2 = level add).
    internal sealed class ImpactLogic_BlockDamage : AbstractImpactLogic
    {
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            float rate = 1.0f;
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            if (null == info || null == info.Cfg() || info.ExtraParams.Count <= 2) return hpDamage;
            int sign = info.Cfg().GetIntParam(0, 0);
            float basis = info.Cfg().GetFloatParam(1, 0.0f);
            float levelAdd = info.Cfg().GetFloatParam(2, 0.0f);
            EntityInfo owner = 1 == sign ? obj : ImpactCompat.GetEntity(obj.SceneContext, info.ImpactSenderId);
            SkillInfo skill = null != owner ? owner.GetSkillStateInfo().GetSkillInfoById(info.SkillId) : null;
            if (null != skill) {
                if (1 == sign) rate -= ((skill.SkillLevel - 1) * levelAdd + basis) + ImpactCompat.TalentRate(owner, "AvoidanceRate");
                else rate += ((skill.SkillLevel - 1) * levelAdd + basis) + ImpactCompat.TalentRate(owner, "AddPBDamageRate");
            }
            return (int)(hpDamage * rate);
        }
    }
}
