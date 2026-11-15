using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // Maps effect-related APIs (Rage, Talent, CombatStatistic, StateFlag, Equipment) onto this framework.
    internal static class ImpactCompat
    {
        public static EntityInfo GetEntity(SceneContextInfo ctx, int id)
        {
            return null != ctx ? ctx.GetEntityById(id) : null;
        }
        public static bool IsUser(EntityInfo obj) { return null != obj && obj.EntityType == (int)EntityTypeEnum.Hero; }
        public static bool IsNpc(EntityInfo obj) { return null != obj && !IsUser(obj); }
        public static bool IsDead(EntityInfo obj) { return null == obj || obj.IsDead(); }
        public static bool IsInvincible(EntityInfo obj) { return null != obj && obj.HaveState(CharacterPropertyEnum.x3009_无敌); }
        public static bool SuperArmor(EntityInfo obj) { return null != obj && obj.HaveState(CharacterPropertyEnum.x3008_霸体); }
        public static bool UltraArmor(EntityInfo obj) { return null != obj && obj.HaveState(CharacterPropertyEnum.x3012_超级霸体); }
        public static void SetState(EntityInfo obj, CharacterPropertyEnum id, bool on)
        {
            if (null == obj) return;
            if (on) { if (!obj.HaveState(id)) obj.AddState(id); }
            else { if (obj.HaveState(id)) obj.RemoveState(id); }
        }
        public static void SetSuperArmor(EntityInfo obj, bool on) { SetState(obj, CharacterPropertyEnum.x3008_霸体, on); }
        public static void SetUltraArmor(EntityInfo obj, bool on) { SetState(obj, CharacterPropertyEnum.x3012_超级霸体, on); }
        public static int Rage(EntityInfo obj)
        {
            return null == obj ? 0 : obj.ActualProperty.GetInt(CharacterPropertyEnum.x5007_怒气当前值);
        }
        public static void AddRage(EntityInfo obj, int delta)
        {
            if (null == obj) return;
            int max = obj.ActualProperty.GetInt(CharacterPropertyEnum.x5008_怒气最大值);
            int val = Rage(obj) + delta;
            if (val < 0) val = 0;
            if (max > 0 && val > max) val = max;
            obj.BaseProperty.SetInt(CharacterPropertyEnum.x5007_怒气当前值, val);
            obj.ActualProperty.SetInt(CharacterPropertyEnum.x5007_怒气当前值, val);
            if (null != ImpactEvents.EventImpactLogicRage) ImpactEvents.EventImpactLogicRage(obj, val);
        }
        // Combat runs on the client; the server keeps data logic and broadcasts only.
        public static bool CanSettleDamage(EntityInfo obj)
        {
            return null != obj;
        }
        public static Vector3 GetPosition(EntityInfo obj)
        {
            return null == obj ? new Vector3() : obj.GetMovementStateInfo().GetPosition3D();
        }
        public static float GetFaceDir(EntityInfo obj)
        {
            return null == obj ? 0.0f : obj.GetMovementStateInfo().GetFaceDir();
        }
        public static float DistanceSquare(EntityInfo a, EntityInfo b)
        {
            Vector3 p1 = GetPosition(a);
            Vector3 p2 = GetPosition(b);
            float dx = p1.X - p2.X;
            float dz = p1.Z - p2.Z;
            return dx * dx + dz * dz;
        }
        public static int CalcDamage(EntityInfo sender, EntityInfo target, ImpactLogicData cfg, out bool isCritical)
        {
            isCritical = false;
            if (null == sender || null == target || null == cfg || null == target.SceneContext) return 0;
            return CalcDamage(sender, target, (long)(cfg.DamageRate * 1000), cfg.DamageValue, cfg.DamageType, out isCritical);
        }
        // Damage numbers come from the skill's SkillDamageData (multiple/damage per hit) instead of
        // an ImpactData config; the formula itself is shared with the impact-driven path.
        // damageType: 0 = physical only, 1 = magical only, 2 = both.
        public static int CalcDamage(EntityInfo sender, EntityInfo target, long multiple, long damage, int damageType, out bool isCritical)
        {
            isCritical = false;
            if (null == sender || null == target || null == target.SceneContext) return 0;
            long critrate = AttrCalculator.Calc(target.SceneContext, sender.ActualProperty, target.ActualProperty, "critrate");
            long blockrate = AttrCalculator.Calc(target.SceneContext, sender.ActualProperty, target.ActualProperty, "blockrate");
            long rnd = Helper.Random.Next();
            long critonoff = rnd <= critrate ? 1 : 0;
            long blockonoff = (rnd > critrate && rnd <= critrate + blockrate) ? 1 : 0;
            isCritical = critonoff > 0;
            long phy = AttrCalculator.Calc(target.SceneContext, sender.ActualProperty, target.ActualProperty, "phydamage", multiple, damage, critonoff, blockonoff);
            long mag = AttrCalculator.Calc(target.SceneContext, sender.ActualProperty, target.ActualProperty, "magdamage", multiple, damage, critonoff, blockonoff);
            long total = 0 == damageType ? phy : (1 == damageType ? mag : phy + mag);
            return (int)total;
        }
        // Extension point for talent-like modifiers; the default framework has no talent system.
        public static float TalentRate(EntityInfo obj, string key) { return 0.0f; }
        public static void SetHateTarget(EntityInfo npc, int senderId)
        {
            if (null == npc || !IsNpc(npc)) return;
            AiStateInfo ai = npc.GetAiStateInfo();
            if (null != ai && 0 == ai.HateTarget && senderId != npc.GetId()) ai.HateTarget = senderId;
        }
    }
}
