using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 13: every Nth hit appends damage and an extra impact.
    internal sealed class ImpactLogic_AppendDamage : AbstractImpactLogic
    {
        public override int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId)
        {
            if (null == obj || impactOwnerId != senderId) return hpDamage;
            EntityInfo sender = ImpactCompat.GetEntity(obj.SceneContext, senderId);
            if (null == sender || !ImpactCompat.IsUser(sender)) return hpDamage;
            ImpactInfo info = sender.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || null == info.Cfg() || info.ExtraParams.Count < 1) return hpDamage;
            int triggerCount = info.Cfg().GetIntParam(0, 0);
            int count = ImpactHitCounter.CountOf(senderId);
            if (triggerCount > 0 && count != 0 && count % triggerCount == 0) {
                AppendDamage(sender, obj, info, hpDamage);
                AppendImpact(sender, obj, info);
            }
            if (info.ExtraParams.Count > 3 && !string.IsNullOrEmpty(info.ExtraParams[2]) && !string.IsNullOrEmpty(info.ExtraParams[3])
                && null != ImpactEvents.EventImpactLogicEffect) {
                ImpactEvents.EventImpactLogicEffect(obj, info.ExtraParams[2], info.ExtraParams[3], 2.0f);
            }
            return hpDamage;
        }
        private void AppendDamage(EntityInfo sender, EntityInfo receiver, ImpactInfo info, int parentDamage)
        {
            int appendDamage = (int)(info.Cfg().DamageRate * parentDamage + info.Cfg().DamageValue);
            if (info.ExtraParams.Count > 4) appendDamage = info.Cfg().GetIntParam(4, appendDamage);
            if (appendDamage <= 0) return;
            bool isCritical = false;
            List<ImpactInfo> infos = receiver.GetSkillStateInfo().GetAllImpact();
            for (int i = 0; i < infos.Count; ++i) {
                IImpactLogic logic = infos[i].ImpactLogicId > 0 ? ImpactLogicManager.Instance.GetImpactLogic(infos[i].ImpactLogicId) : null;
                if (null != logic) {
                    appendDamage = logic.RefixHpDamage(receiver, infos[i].ImpactId, appendDamage, sender.GetId(), ref isCritical, receiver.GetId());
                }
            }
            OnCharacterDamage(sender, receiver, appendDamage, isCritical, false);
        }
        private static void AppendImpact(EntityInfo sender, EntityInfo target, ImpactInfo info)
        {
            int appendImpactId = info.Cfg().GetIntParam(1, 0);
            if (appendImpactId <= 0) return;
            ImpactLogicData cfg = ImpactLogicDataProvider.Instance.GetImpactLogicData(appendImpactId);
            if (null == cfg) return;
            cfg.IsIgnorePassiveCheck = true;
            ImpactSystem.Instance.SendImpactToCharacter(sender, appendImpactId, target.GetId(), -1, -1,
                ImpactCompat.GetPosition(sender), ImpactCompat.GetFaceDir(sender));
        }
    }
}
