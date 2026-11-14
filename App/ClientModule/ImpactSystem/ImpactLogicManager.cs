using System;
using System.Collections.Generic;

namespace ScriptableFramework
{
    // Restricted extension point: new business categories are added here, not as free-form DSL.
    public sealed class ImpactLogicManager
    {
        public IImpactLogic GetImpactLogic(int id)
        {
            IImpactLogic logic;
            m_ImpactLogics.TryGetValue(id, out logic);
            return logic;
        }
        private ImpactLogicManager()
        {
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_General, new ImpactLogic_General());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_SuperArmor, new ImpactLogic_SuperArmor());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_Invincible, new ImpactLogic_Invincible());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_ChangeSkill, new ImpactLogic_ChangeSkill());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_StopImpact, new ImpactLogic_StopImpact());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_RefixDamage, new ImpactLogic_RefixDamage());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_BlockAndBeat, new ImpactLogic_BlockAndBeat());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_SuperArmorShield, new ImpactLogic_SuperArmorShield());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_DamageImmunityShield, new ImpactLogic_DamageImmunityShield());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_Thorns, new ImpactLogic_Thorns());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_IceArmor, new ImpactLogic_IceArmor());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_HitCritical, new ImpactLogic_HitCriticalTrigger());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_AppendDamage, new ImpactLogic_AppendDamage());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_HitRecover, new ImpactLogic_HitRecover());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_Heal, new ImpactLogic_Heal());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_Undead, new ImpactLogic_Undead());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_KillAddDamage, new ImpactLogic_KillAddDamage());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_BlockDamage, new ImpactLogic_BlockDamage());
            m_ImpactLogics.Add((int)ImpactLogicId.ImpactLogic_Taunt, new ImpactLogic_Taunt());
        }
        private Dictionary<int, IImpactLogic> m_ImpactLogics = new Dictionary<int, IImpactLogic>();
        public static ImpactLogicManager Instance { get { return s_Instance; } }
        private static ImpactLogicManager s_Instance = new ImpactLogicManager();
    }
}
