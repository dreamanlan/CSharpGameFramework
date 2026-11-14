using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 11: on a new hostile impact, add a slow impact to its sender (param0 = slow impact id).
    internal sealed class ImpactLogic_IceArmor : AbstractImpactLogic
    {
        public override void OnAddImpact(EntityInfo obj, int impactId, int addImpactId)
        {
            ImpactInfo impactInfo = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == impactInfo) return;
            ImpactInfo addImpactInfo = obj.GetSkillStateInfo().GetImpactInfoById(addImpactId);
            if (null == addImpactInfo || addImpactInfo.ImpactSenderId == obj.GetId()) return;
            int slowImpactId = null != impactInfo.Cfg() ? impactInfo.Cfg().GetIntParam(0, 0) : 0;
            if (slowImpactId <= 0) return;
            ImpactSystem.Instance.SendImpactToCharacter(obj, slowImpactId, addImpactInfo.ImpactSenderId, -1, -1,
                ImpactCompat.GetPosition(obj), ImpactCompat.GetFaceDir(obj));
        }
    }
}
