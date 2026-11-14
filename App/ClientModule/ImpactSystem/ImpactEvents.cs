using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    public delegate void SendImpactEventHandler(EntityInfo sender, int targetId, int impactId, Vector3 srcPos, float srcDir);

    public delegate void ImpactEventHandler(EntityInfo sender, int targetId, int impactId);

    public delegate void ImpactLogicDamageDelegate(EntityInfo entity, int attackerId, int damage, bool isKiller, bool isCritical, bool isOrdinary);

    public delegate void ImpactLogicSkillDelegate(EntityInfo entity, int skillId);

    public delegate void ImpactLogicEffectDelegate(EntityInfo entity, string effectPath, string bonePath, float recycleTime);

    public delegate void ImpactLogicScreenTipDelegate(EntityInfo entity, string tip);

    public delegate void ImpactLogicRageDelegate(EntityInfo entity, int rage);

    public delegate void ImpactLogicRefreshSkillDelegate(EntityInfo entity);

    public delegate void ImpactHitRecoverDelegate(EntityInfo entity, string type, int val);

    public static class ImpactEvents
    {
        public static SendImpactEventHandler EventSendImpact;
        public static ImpactEventHandler EventStopImpact;
        public static ImpactEventHandler EventStopAllImpact;
        public static ImpactEventHandler EventGfxStopImpact;
        public static ImpactLogicDamageDelegate EventImpactLogicDamage;
        public static ImpactLogicSkillDelegate EventImpactLogicSkill;
        public static ImpactLogicEffectDelegate EventImpactLogicEffect;
        public static ImpactLogicScreenTipDelegate EventImpactLogicScreenTip;
        public static ImpactLogicRageDelegate EventImpactLogicRage;
        public static ImpactLogicRefreshSkillDelegate EventRefreshSkill;
        public static ImpactEventHandler EventStartSuperArmorShield;
        public static ImpactEventHandler EventUpdateSuperArmorShield;
        public static ImpactEventHandler EventStopSuperArmorShield;
        public static ImpactEventHandler EventStartDamageImmunityShiled;
        public static ImpactEventHandler EventUpdateDamageImmunityShiled;
        public static ImpactEventHandler EventStopDamageImmunityShiled;
        public static ImpactHitRecoverDelegate EventImpactHitRecover;
    }
}
