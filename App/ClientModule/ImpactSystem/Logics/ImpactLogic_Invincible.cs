using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 3: invincibility for the impact lifetime.
    internal sealed class ImpactLogic_Invincible : AbstractImpactLogic
    {
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || !info.IsActivated) return;
            if (!info.HasEffectApplyed) {
                ImpactCompat.SetState(obj, CharacterPropertyEnum.x3009_无敌, true);
                info.HasEffectApplyed = true;
            }
            if (ImpactTimeUtility.NowMs() > info.StartTime + info.DurationTime) {
                ImpactCompat.SetState(obj, CharacterPropertyEnum.x3009_无敌, false);
                info.IsActivated = false;
            }
        }
        public override void OnInterrupted(EntityInfo obj, int impactId)
        {
            ImpactCompat.SetState(obj, CharacterPropertyEnum.x3009_无敌, false);
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            if (null != info) info.IsActivated = false;
            base.OnInterrupted(obj, impactId);
        }
    }
}
