using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    public enum ImpactLogicId
    {
        ImpactLogic_General = 1,
        ImpactLogic_SuperArmor = 2,
        ImpactLogic_Invincible = 3,
        ImpactLogic_ChangeSkill = 4,
        ImpactLogic_StopImpact = 5,
        ImpactLogic_RefixDamage = 6,
        ImpactLogic_BlockAndBeat = 7,
        ImpactLogic_SuperArmorShield = 8,
        ImpactLogic_DamageImmunityShield = 9,
        ImpactLogic_Thorns = 10,
        ImpactLogic_IceArmor = 11,
        ImpactLogic_HitCritical = 12,
        ImpactLogic_AppendDamage = 13,
        ImpactLogic_HitRecover = 14,
        ImpactLogic_Heal = 15,
        ImpactLogic_Undead = 16,
        ImpactLogic_KillAddDamage = 17,
        ImpactLogic_BlockDamage = 18,
        ImpactLogic_Taunt = 19,
    }

    public enum ImpactGfxLogicId
    {
        Default = 0,
        Stiffness = 1,
        HitFly = 2,
        KnockDown = 3,
        Grab = 4,
    }

    public enum ImpactDamageFlag
    {
        IS_KILLER = 1 << 0,
        IS_CRITICAL = 1 << 1,
        IS_ORDINARY = 1 << 2,
    }
}
