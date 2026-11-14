using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 15: heal (param0: 0 = max Hp ratio, 1 = calc by damage formula, 2 = flat; param1 = value).
    internal sealed class ImpactLogic_Heal : AbstractImpactLogic
    {
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            if (null == obj || obj.IsDead()) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || null == info.Cfg()) return;
            EntityInfo sender = ImpactCompat.GetEntity(obj.SceneContext, info.ImpactSenderId);
            if (null == sender) return;
            int type = info.Cfg().GetIntParam(0, 0);
            float value = info.Cfg().GetFloatParam(1, 0.0f);
            float recover = 0.0f;
            if (0 == type) recover = obj.HpMax * value;
            else if (1 == type) {
                bool isCritical;
                recover = ImpactCompat.CalcDamage(sender, obj, info.Cfg(), out isCritical);
            } else recover = value;
            DoRecover(obj, info.ImpactSenderId, recover);
        }
        private static void DoRecover(EntityInfo obj, int senderId, float value)
        {
            if (obj.Hp + value > obj.HpMax) value = obj.HpMax - obj.Hp;
            obj.Hp = obj.Hp + (int)value;
            if (null != ImpactEvents.EventImpactLogicDamage) {
                ImpactEvents.EventImpactLogicDamage(obj, senderId, (int)value, false, false, true);
            }
        }
    }
}
