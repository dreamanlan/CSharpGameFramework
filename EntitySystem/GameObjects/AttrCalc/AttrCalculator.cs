using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public sealed class AttrCalculator
    {
        public static void ResetBaseProperty(EntityInfo obj)
        {
            obj.CalcBaseAttr();
        }
        public static void RefixAttrByImpact(EntityInfo obj)
        {
            List<ImpactInfo> impacts = obj.GetSkillStateInfo().GetAllImpact();
            for (int i = 0; i < impacts.Count; ++i) {
                ImpactInfo impact = impacts[i];
                impact.RefixCharacterProperty(obj);
            }
        }
        public static void Calc(EntityInfo entity)
        {
            ResetBaseProperty(entity);
            RefixAttrByImpact(entity);

            int hpMax = entity.GetActualProperty().HpMax;
            entity.GetActualProperty().SetHpMax(Operate_Type.OT_Absolute, hpMax);
        }
    }
}
