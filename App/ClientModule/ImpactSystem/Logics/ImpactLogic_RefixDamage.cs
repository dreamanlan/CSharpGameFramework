using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 6: scale incoming damage (param0 = rate).
    internal sealed class ImpactLogic_RefixDamage : AbstractImpactLogic
    {
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            if (null == info || null == info.Cfg()) return hpDamage;
            return (int)(hpDamage * info.Cfg().GetFloatParam(0, 1.0f));
        }
    }
}
