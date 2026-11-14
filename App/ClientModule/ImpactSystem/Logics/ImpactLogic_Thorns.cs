using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 10: reflect part of incoming damage back to the attacker (param0 = rate).
    internal sealed class ImpactLogic_Thorns : AbstractImpactLogic
    {
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            if (null == info || null == info.Cfg()) return hpDamage;
            float rate = info.Cfg().GetFloatParam(0, 1.0f);
            EntityInfo sender = ImpactCompat.GetEntity(obj.SceneContext, senderId);
            ApplyDamage(sender, obj, (int)(hpDamage * rate));
            return hpDamage;
        }
    }
}
