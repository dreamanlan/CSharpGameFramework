using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 8: super armor shield, breaks after absorbing configured HpMax fraction.
    internal sealed class ImpactLogic_SuperArmorShield : AbstractImpactLogic
    {
        private sealed class ShieldState { public int DamageRemain; public int DamageMax; public string EffectPath; public int RemainTime; }
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            if (null == obj) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || null == info.Cfg()) return;
            ImpactCompat.SetUltraArmor(obj, true);
            if (info.ExtraParams.Count > 2) {
                ShieldState state = info.LogicDatas.GetOrNewData<ShieldState>();
                state.DamageRemain = (int)(info.Cfg().GetFloatParam(0, 0.0f) * obj.HpMax);
                state.DamageMax = state.DamageRemain;
                state.EffectPath = info.ExtraParams[1];
                state.RemainTime = info.Cfg().GetIntParam(2, -1);
            }
            if (null != ImpactEvents.EventStartSuperArmorShield) ImpactEvents.EventStartSuperArmorShield(null, obj.GetId(), impactId);
            base.StartImpact(obj, impactId);
        }
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || !info.IsActivated) return;
            if (ImpactCompat.IsDead(obj) || IsTimeUp(info)) {
                FireStop(obj, impactId);
                ImpactCompat.SetUltraArmor(obj, false);
                info.IsActivated = false;
            } else {
                ImpactCompat.SetUltraArmor(obj, true);
            }
        }
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            if (null == info) return hpDamage;
            ShieldState state = info.LogicDatas.GetData<ShieldState>();
            if (null == state) return hpDamage;
            state.DamageRemain -= hpDamage;
            if (state.DamageRemain > 0) {
                if (null != ImpactEvents.EventUpdateSuperArmorShield) ImpactEvents.EventUpdateSuperArmorShield(null, obj.GetId(), impactId);
                if (!ImpactCompat.UltraArmor(obj)) ImpactCompat.SetUltraArmor(obj, true);
            } else {
                FireStop(obj, impactId);
                info.IsActivated = false;
                ImpactCompat.SetUltraArmor(obj, false);
            }
            return hpDamage;
        }
        public override void StopImpact(EntityInfo obj, int impactId)
        {
            FireStop(obj, impactId);
            ImpactCompat.SetUltraArmor(obj, false);
            base.StopImpact(obj, impactId);
        }
        private static bool IsTimeUp(ImpactInfo info)
        {
            ShieldState state = info.LogicDatas.GetData<ShieldState>();
            if (null == state || state.RemainTime < 0) return false;
            return ImpactTimeUtility.NowMs() > info.StartTime + state.RemainTime;
        }
        private static void FireStop(EntityInfo obj, int impactId)
        {
            if (null != obj && null != ImpactEvents.EventStopSuperArmorShield) {
                ImpactEvents.EventStopSuperArmorShield(null, obj.GetId(), impactId);
            }
        }
    }
}
