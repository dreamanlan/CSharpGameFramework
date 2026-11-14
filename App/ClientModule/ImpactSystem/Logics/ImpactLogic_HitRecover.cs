using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 14: every Nth hit restores HP/energy (param0 = N, param1 = HP/MP, param2 = max ratio, param3 = flat value).
    internal sealed class ImpactLogic_HitRecover : AbstractImpactLogic
    {
        private sealed class RecoverState { public long LastRecoverTime; public int LastRecoverHitCount; }
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            if (null == obj || senderId != impactOwnerId) return hpDamage;
            EntityInfo sender = ImpactCompat.GetEntity(obj.SceneContext, senderId);
            if (null == sender || !ImpactCompat.IsUser(sender)) return hpDamage;
            ImpactInfo info = sender.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || null == info.Cfg()) return hpDamage;
            int triggerCount = info.Cfg().GetIntParam(0, 0);
            int count = ImpactHitCounter.CountOf(senderId);
            if (triggerCount > 0 && count != 0 && count % triggerCount == 0) {
                Recover(sender, info, count, ImpactHitCounter.LastHitTimeOf(senderId));
            }
            return hpDamage;
        }
        private static void Recover(EntityInfo sender, ImpactInfo info, int hitCount, long hitTime)
        {
            if (info.ExtraParams.Count < 4) return;
            RecoverState state = info.LogicDatas.GetOrNewData<RecoverState>();
            if (state.LastRecoverHitCount == hitCount && state.LastRecoverTime == hitTime) return;
            string attribute = info.ExtraParams[1];
            float ratio = info.Cfg().GetFloatParam(2, 0.0f);
            int flat = info.Cfg().GetIntParam(3, 0);
            if (attribute == "HP") {
                int shouldAdd = (int)(sender.HpMax * ratio) + flat;
                int finalAdd = Math.Min(shouldAdd, Math.Max(0, sender.HpMax - sender.Hp));
                sender.Hp = sender.Hp + finalAdd;
                state.LastRecoverHitCount = hitCount;
                state.LastRecoverTime = hitTime;
                if (null != ImpactEvents.EventImpactHitRecover) ImpactEvents.EventImpactHitRecover(sender, "HP", shouldAdd);
            } else if (attribute == "MP") {
                // This framework has no MP attribute; SC is used as the energy counterpart.
                int finalAdd = (int)(sender.HpMax * ratio) + flat;
                sender.ActualProperty.IncreaseInt(CharacterPropertyEnum.x5004_SC当前值, finalAdd);
                sender.BaseProperty.IncreaseInt(CharacterPropertyEnum.x5004_SC当前值, finalAdd);
                state.LastRecoverHitCount = hitCount;
                state.LastRecoverTime = hitTime;
                if (null != ImpactEvents.EventImpactHitRecover) ImpactEvents.EventImpactHitRecover(sender, "MP", finalAdd);
            }
        }
    }
}
