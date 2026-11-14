using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 2: super armor (param0: 0 = super armor, 1 = ultra armor).
    internal sealed class ImpactLogic_SuperArmor : AbstractImpactLogic
    {
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            int type = TypeOf(obj, impactId);
            if (0 == type) ImpactCompat.SetSuperArmor(obj, true);
            else if (1 == type) ImpactCompat.SetUltraArmor(obj, true);
            base.StartImpact(obj, impactId);
        }
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != info && info.IsActivated && ImpactTimeUtility.NowMs() > info.StartTime + info.DurationTime) {
                ClearArmor(obj, impactId);
                info.IsActivated = false;
            }
        }
        public override void StopImpact(EntityInfo obj, int impactId)
        {
            ClearArmor(obj, impactId);
            base.StopImpact(obj, impactId);
        }
        private static void ClearArmor(EntityInfo obj, int impactId)
        {
            int type = TypeOf(obj, impactId);
            if (0 == type) ImpactCompat.SetSuperArmor(obj, false);
            else if (1 == type) ImpactCompat.SetUltraArmor(obj, false);
        }
        private static int TypeOf(EntityInfo obj, int impactId)
        {
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            return null != info && null != info.Cfg() ? info.Cfg().GetIntParam(0, 0) : 0;
        }
    }
}
