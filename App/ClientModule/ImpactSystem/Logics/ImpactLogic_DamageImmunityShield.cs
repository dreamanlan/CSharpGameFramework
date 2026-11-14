using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 9: damage immunity shield with N charges and refresh interval.
    internal sealed class ImpactLogic_DamageImmunityShield : AbstractImpactLogic
    {
        private sealed class ShieldState
        {
            public int CountRemain;
            public int CountMax;
            public int RefreshInterval;
            public string EffectPath;
            public int RemainTime;
            public long LastHitTime;
        }
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            if (null == obj) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || null == info.Cfg()) return;
            if (info.ExtraParams.Count > 3) {
                ShieldState state = info.LogicDatas.GetOrNewData<ShieldState>();
                state.CountRemain = info.Cfg().GetIntParam(0, 1);
                state.CountMax = state.CountRemain;
                state.RefreshInterval = info.Cfg().GetIntParam(1, 0);
                state.EffectPath = info.ExtraParams[2];
                state.RemainTime = info.Cfg().GetIntParam(3, -1);
                state.LastHitTime = ImpactTimeUtility.NowMs();
            }
            if (null != ImpactEvents.EventStartDamageImmunityShiled) ImpactEvents.EventStartDamageImmunityShiled(null, obj.GetId(), impactId);
            base.StartImpact(obj, impactId);
        }
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || !info.IsActivated) return;
            ShieldState state = info.LogicDatas.GetData<ShieldState>();
            if (null == state) return;
            long now = ImpactTimeUtility.NowMs();
            if (state.RefreshInterval > 0 && now - state.LastHitTime > state.RefreshInterval) {
                state.CountRemain = state.CountMax;
                state.LastHitTime = now;
                if (null != ImpactEvents.EventUpdateDamageImmunityShiled) ImpactEvents.EventUpdateDamageImmunityShiled(null, obj.GetId(), impactId);
            }
            if (ImpactCompat.IsDead(obj) || (state.RemainTime >= 0 && now > info.StartTime + state.RemainTime)) {
                FireStop(obj, impactId);
                info.IsActivated = false;
            }
        }
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            ImpactInfo info = null != obj ? obj.GetSkillStateInfo().GetImpactInfoById(impactId) : null;
            if (null == info) return hpDamage;
            ShieldState state = info.LogicDatas.GetData<ShieldState>();
            // With no shield params configured the effect keeps permanent immunity.
            if (null == state) return 0;
            --state.CountRemain;
            if (state.CountRemain > 0) {
                if (null != ImpactEvents.EventUpdateDamageImmunityShiled) ImpactEvents.EventUpdateDamageImmunityShiled(null, obj.GetId(), impactId);
                if (null != ImpactEvents.EventImpactLogicDamage && null != obj) {
                    ImpactEvents.EventImpactLogicDamage(obj, senderId, 0, false, false, false);
                }
                return 0;
            }
            FireStop(obj, impactId);
            return hpDamage;
        }
        public override void StopImpact(EntityInfo obj, int impactId)
        {
            FireStop(obj, impactId);
            base.StopImpact(obj, impactId);
        }
        private static void FireStop(EntityInfo obj, int impactId)
        {
            if (null != obj && null != ImpactEvents.EventStopDamageImmunityShiled) {
                ImpactEvents.EventStopDamageImmunityShiled(null, obj.GetId(), impactId);
            }
        }
    }
}
