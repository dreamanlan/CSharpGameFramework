using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    public abstract class AbstractImpactLogic : IImpactLogic
    {
        public virtual void StartImpact(EntityInfo obj, int impactId)
        {
            if (null == obj) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info) return;
            if (null != info.Cfg() && info.Cfg().BreakSuperArmor) ImpactCompat.SetSuperArmor(obj, false);
            ImpactCompat.SetHateTarget(obj, info.ImpactSenderId);
        }
        public virtual void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != info && info.IsActivated && ImpactTimeUtility.NowMs() > info.StartTime + info.DurationTime) {
                info.IsActivated = false;
            }
        }
        public virtual void StopImpact(EntityInfo obj, int impactId) { }
        public virtual void OnInterrupted(EntityInfo obj, int impactId) { StopImpact(obj, impactId); }
        public virtual int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            return hpDamage;
        }
        public virtual void OnAddImpact(EntityInfo obj, int impactId, int addImpactId) { }
        public virtual bool CanInterrupt(EntityInfo obj, int impactId) { return true; }

        protected bool IsImpactDamageOrdinary(EntityInfo target, int impactId)
        {
            if (null == target) return false;
            ImpactInfo info = target.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info) return false;
            EntityInfo sender = ImpactCompat.GetEntity(target.SceneContext, info.ImpactSenderId);
            if (null == sender) return false;
            SkillInfo skill = sender.GetSkillStateInfo().GetSkillInfoById(info.SkillId);
            return null != skill && skill.Category == 1;
        }

        protected void ApplyDamage(EntityInfo obj, EntityInfo sender, int damage)
        {
            if (ImpactCompat.IsDead(obj) || !ImpactCompat.CanSettleDamage(obj)) return;
            if (damage < 1) return;
            int real = -damage;
            if (obj.Hp + real < 0) real = -obj.Hp;
            obj.Hp = obj.Hp + real;
            bool isKiller = obj.IsDead();
            if (null != ImpactEvents.EventImpactLogicDamage && null != sender) {
                ImpactEvents.EventImpactLogicDamage(obj, sender.GetId(), -damage, isKiller, false, true);
            }
        }

        protected void ApplyDamage(EntityInfo obj, int impactId, out int damage)
        {
            damage = 0;
            if (ImpactCompat.IsDead(obj) || !ImpactCompat.CanSettleDamage(obj)) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info) return;
            EntityInfo sender = ImpactCompat.GetEntity(obj.SceneContext, info.ImpactSenderId);
            ApplyDamageImpl(sender, obj, info, out damage, null != info.Cfg() && info.Cfg().IsIgnorePassiveCheck);
        }

        protected void ApplyDamageImpl(EntityInfo sender, EntityInfo obj, ImpactInfo info, out int damage, bool isIgnorePassiveCheck)
        {
            damage = 0;
            if (null == sender || null == info || null == info.Cfg()) return;
            int skillLevel = 0;
            bool isOrdinary = false;
            float rate = 1.0f;
            SkillInfo skill = sender.GetSkillStateInfo().GetSkillInfoById(info.SkillId);
            if (null != skill) {
                skillLevel = skill.SkillLevel;
                isOrdinary = skill.Category == 1;
                if (!isOrdinary) rate += ImpactCompat.TalentRate(sender, "AddAllSkillDamageRate");
            }
            bool isCritical;
            ImpactLogicData cfg = info.Cfg();
            int curDamage = ImpactCompat.CalcDamage(sender, obj, cfg, out isCritical);
            if (skillLevel > 0) curDamage += (int)(curDamage * cfg.LevelRate * skillLevel);
            List<ImpactInfo> targetImpacts = obj.GetSkillStateInfo().GetAllImpact();
            for (int i = 0; i < targetImpacts.Count; ++i) {
                ImpactInfo ti = targetImpacts[i];
                if (isIgnorePassiveCheck && ti.ImpactType == (int)ImpactType.PASSIVE) continue;
                if (null == ti.Cfg()) continue;
                IImpactLogic logic = ImpactLogicManager.Instance.GetImpactLogic(ti.Cfg().LogicId);
                if (null != logic) {
                    curDamage = logic.RefixHpDamage(obj, ti.ImpactId, curDamage, sender.GetId(), ref isCritical, obj.GetId());
                }
            }
            if (!isIgnorePassiveCheck) {
                List<ImpactInfo> senderImpacts = sender.GetSkillStateInfo().GetAllImpact();
                for (int i = 0; i < senderImpacts.Count; ++i) {
                    ImpactInfo si = senderImpacts[i];
                    if (si.ImpactType != (int)ImpactType.PASSIVE || null == si.Cfg()) continue;
                    IImpactLogic logic = ImpactLogicManager.Instance.GetImpactLogic(si.Cfg().LogicId);
                    if (null != logic) {
                        curDamage = logic.RefixHpDamage(obj, si.ImpactId, curDamage, sender.GetId(), ref isCritical, sender.GetId());
                    }
                }
            }
            curDamage = (int)(curDamage * rate);
            damage = curDamage;
            OnCharacterDamage(sender, obj, curDamage, isCritical, isOrdinary);
        }

        public void OnCharacterDamage(EntityInfo sender, EntityInfo obj, int curDamage, bool isCritical, bool isOrdinary)
        {
            if (curDamage < 1 || null == sender) return;
            int real = -curDamage;
            if (obj.Hp + real < 0) real = -obj.Hp;
            obj.Hp = obj.Hp + real;
            bool isKiller = obj.IsDead();
            ImpactHitCounter.OnHit(sender, curDamage, isKiller);
            if (null != ImpactEvents.EventImpactLogicDamage) {
                ImpactEvents.EventImpactLogicDamage(obj, sender.GetId(), -curDamage, isKiller, isCritical, isOrdinary);
            }
        }
    }
}
