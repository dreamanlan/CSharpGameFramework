using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 16: cannot die while active.
    internal sealed class ImpactLogic_Undead : AbstractImpactLogic
    {
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            if (null != obj) obj.CanDead = false;
            base.StartImpact(obj, impactId);
        }
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != info && info.IsActivated && obj.Hp <= 0) obj.Hp = 1;
        }
        public override void OnInterrupted(EntityInfo obj, int impactId)
        {
            if (null != obj) obj.CanDead = true;
            base.OnInterrupted(obj, impactId);
        }
        public override bool CanInterrupt(EntityInfo obj, int impactId) { return false; }
    }
}
