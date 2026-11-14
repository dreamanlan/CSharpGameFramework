using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 7: counter-attack on a new impact from another sender, within max distance.
    internal sealed class ImpactLogic_BlockAndBeat : AbstractImpactLogic
    {
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != info && info.IsActivated && ImpactTimeUtility.NowMs() > info.StartTime + info.DurationTime) {
                info.IsActivated = false;
            }
        }
        public override void OnAddImpact(EntityInfo obj, int impactId, int addImpactId)
        {
            if (null == obj) return;
            ImpactInfo self = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            ImpactInfo added = obj.GetSkillStateInfo().GetImpactInfoById(addImpactId);
            if (null == self || null == added || !self.IsActivated || null == self.Cfg()) return;
            if (added.ImpactSenderId == obj.GetId()) return;
            if (self.ExtraParams.Count < 4) return;
            int beatSkillId = self.Cfg().GetIntParam(0, 0);
            int blockImpactId = self.Cfg().GetIntParam(1, 0);
            string effect = self.ExtraParams[2];
            float maxDis = self.Cfg().GetFloatParam(3, 0.0f);
            EntityInfo sender = ImpactCompat.GetEntity(obj.SceneContext, added.ImpactSenderId);
            if (null == sender) return;
            if (maxDis > 0 && ImpactCompat.DistanceSquare(obj, sender) > maxDis * maxDis) return;
            ImpactSystem.Instance.SendImpactToCharacter(obj, blockImpactId, added.ImpactSenderId, -1, -1,
                ImpactCompat.GetPosition(obj), ImpactCompat.GetFaceDir(obj));
            if (beatSkillId > 0 && null != ImpactEvents.EventImpactLogicSkill) {
                ImpactEvents.EventImpactLogicSkill(obj, beatSkillId);
            }
            ImpactSystem.Instance.StopImpactById(obj, impactId);
            if (!string.IsNullOrEmpty(effect)) {
                string[] parts = effect.Split('|');
                if (parts.Length >= 2 && null != ImpactEvents.EventImpactLogicEffect) {
                    ImpactEvents.EventImpactLogicEffect(obj, parts[0], parts[1], 2.0f);
                }
            }
        }
    }
}
