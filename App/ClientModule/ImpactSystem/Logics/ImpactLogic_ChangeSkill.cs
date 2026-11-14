using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 4: switch the skill category while active and restore it on stop.
    internal sealed class ImpactLogic_ChangeSkill : AbstractImpactLogic
    {
        private sealed class ChangeSkillState
        {
            public List<KeyValuePair<int, int>> OldCategories = new List<KeyValuePair<int, int>>();
            public int NewCategory;
        }
        public override void StartImpact(EntityInfo obj, int impactId)
        {
            if (null == obj) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info || null == info.Cfg()) return;
            int category = info.Cfg().GetIntParam(0, 1);
            int headSkillId = info.Cfg().GetIntParam(1, 0);
            ChangeSkillState state = info.LogicDatas.GetOrNewData<ChangeSkillState>();
            state.NewCategory = category;
            state.OldCategories.Clear();
            List<SkillInfo> skills = obj.GetSkillStateInfo().GetAllSkill();
            for (int i = 0; i < skills.Count; ++i) {
                if (headSkillId > 0 && skills[i].SkillId != headSkillId) continue;
                state.OldCategories.Add(new KeyValuePair<int, int>(skills[i].SkillId, skills[i].Category));
                skills[i].Category = category;
            }
            base.StartImpact(obj, impactId);
        }
        public override void Tick(EntityInfo obj, int impactId)
        {
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null != info && info.IsActivated && ImpactTimeUtility.NowMs() > info.StartTime + info.DurationTime) {
                StopImpact(obj, impactId);
                info.IsActivated = false;
            }
        }
        public override void StopImpact(EntityInfo obj, int impactId)
        {
            if (null == obj) return;
            ImpactInfo info = obj.GetSkillStateInfo().GetImpactInfoById(impactId);
            if (null == info) return;
            ChangeSkillState state = info.LogicDatas.GetData<ChangeSkillState>();
            if (null != state) {
                for (int i = 0; i < state.OldCategories.Count; ++i) {
                    SkillInfo skill = obj.GetSkillStateInfo().GetSkillInfoById(state.OldCategories[i].Key);
                    if (null != skill) skill.Category = state.OldCategories[i].Value;
                }
            }
            base.StopImpact(obj, impactId);
        }
    }
}
