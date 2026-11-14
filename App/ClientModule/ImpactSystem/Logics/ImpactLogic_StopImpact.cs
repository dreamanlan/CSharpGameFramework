using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 5: dispel impacts (param0 = id, param1 = 0 by impact id, 1 by logic id, 2 by gfx id, -1 dispels all).
    internal sealed class ImpactLogic_StopImpact : AbstractImpactLogic
    {
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            if (null == obj) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || null == info.Cfg()) return;
            int id = info.Cfg().GetIntParam(0, -1);
            int removeType = info.Cfg().GetIntParam(1, 0);
            if (-1 == id) { ImpactSystem.Instance.StopAllImpact(obj); return; }
            List<ImpactInfo> infos = obj.GetSkillStateInfo().GetAllImpact();
            for (int i = 0; i < infos.Count; ++i) {
                ImpactInfo target = infos[i];
                if (target.ImpactId == impactId || target.ImpactType == (int)ImpactType.PASSIVE || null == target.Cfg()) continue;
                bool match = 0 == removeType ? target.ImpactId == id :
                    1 == removeType ? target.Cfg().LogicId == id : target.Cfg().GfxLogicId == id;
                if (match) ImpactSystem.Instance.StopImpactById(obj, target.ImpactId);
            }
        }
    }
}
