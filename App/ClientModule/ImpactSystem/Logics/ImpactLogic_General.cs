using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 1: plain damage + rage, optional delayed hit (param0 = delay seconds, param2 = rage).
    internal sealed class ImpactLogic_General : AbstractImpactLogic
    {
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            if (null == obj) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != info && info.IsActivated) {
                float delay = Delay(info);
                if (delay < 0.01f && !ImpactCompat.IsInvincible(obj)) {
                    int damage;
                    ApplyDamage(obj, impactId, out damage);
                }
                ApplyRage(obj, info);
                info.HasEffectApplyed = true;
            }
            base.StartImpact(obj, impactId);
        }
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || !info.IsActivated) return;
            float delay = Delay(info);
            if (delay > 0.01f && !info.HasEffectApplyed && ImpactTimeUtility.NowMs() > info.StartTime + delay * 1000) {
                if (!ImpactCompat.IsInvincible(obj)) {
                    int damage;
                    ApplyDamage(obj, impactId, out damage);
                }
                ApplyRage(obj, info);
                info.HasEffectApplyed = true;
            }
            if (ImpactTimeUtility.NowMs() > info.StartTime + info.DurationTime) info.IsActivated = false;
        }
        private static float Delay(ImpactInfo info)
        {
            return null != info.Cfg() ? info.Cfg().GetFloatParam(0, 0.0f) : 0.0f;
        }
        private static void ApplyRage(EntityInfo obj, ImpactInfo info)
        {
            int rage = null != info.Cfg() ? info.Cfg().GetIntParam(2, 0) : 0;
            if (rage <= 0) return;
            if (ImpactCompat.IsUser(obj)) ImpactCompat.AddRage(obj, rage);
            else {
                EntityInfo sender = ImpactCompat.GetEntity(obj.SceneContext, info.ImpactSenderId);
                ImpactCompat.AddRage(sender, rage);
            }
        }
    }
}
