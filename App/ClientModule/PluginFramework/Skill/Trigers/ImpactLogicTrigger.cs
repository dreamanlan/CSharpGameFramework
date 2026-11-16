using System;
using UnityEngine;
using SkillSystem;
using ScriptableFramework;

namespace ScriptableFramework.Skill.Trigers
{
    // Skill DSL stays flexible; impacts are selected by a restricted logic id from ImpactData.txt.
    internal abstract class ImpactLogicTriggerBase : AbstractStatelessSkillTriger
    {
        protected int m_ImpactId = 0;
        protected int m_Duration = -1;
        protected void LoadParams(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            StartTime = num > 0 ? long.Parse(callData.GetParamId(0)) : 0;
            if (num > 1) m_ImpactId = int.Parse(callData.GetParamId(1));
            if (num > 2) m_Duration = int.Parse(callData.GetParamId(2));
        }
        protected void CopyTo(ImpactLogicTriggerBase copy)
        {
            copy.m_ImpactId = m_ImpactId;
            copy.m_Duration = m_Duration;
        }
        protected bool Resolve(object sender, long curSectionTime, out GfxSkillSenderInfo senderObj, out int senderId, out int targetId)
        {
            senderObj = sender as GfxSkillSenderInfo;
            senderId = 0;
            targetId = 0;
            if (null == senderObj || curSectionTime < StartTime) return false;
            EntityController.Instance.CalcSenderAndTarget(senderObj, out senderId, out targetId);
            return true;
        }
        public override void Reset() { }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance) { LoadParams(callData); }
    }

    /// <summary>add_impact_to_self(start_time, impact_id[, duration]);</summary>
    internal sealed class AddImpactToSelfTrigger : ImpactLogicTriggerBase
    {
        protected override ISkillTriger OnClone() { var copy = new AddImpactToSelfTrigger(); CopyTo(copy); return copy; }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj;
            int senderId, targetId;
            if (!Resolve(sender, curSectionTime, out senderObj, out senderId, out targetId)) return curSectionTime < StartTime;
            EntityController.Instance.SendImpactLogic(senderId, senderId, m_ImpactId, senderObj.SkillId, m_Duration);
            return false;
        }
    }

    /// <summary>add_impact_to_target(start_time, impact_id[, duration]);</summary>
    internal sealed class AddImpactToTargetTrigger : ImpactLogicTriggerBase
    {
        protected override ISkillTriger OnClone() { var copy = new AddImpactToTargetTrigger(); CopyTo(copy); return copy; }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj;
            int senderId, targetId;
            if (!Resolve(sender, curSectionTime, out senderObj, out senderId, out targetId)) return curSectionTime < StartTime;
            EntityController.Instance.SendImpactLogic(senderId, targetId, m_ImpactId, senderObj.SkillId, m_Duration);
            return false;
        }
    }

    /// <summary>state_impact(start_time, impact_id[, duration]); hit-side impact trigger.</summary>
    internal sealed class StateImpactTrigger : ImpactLogicTriggerBase
    {
        protected override ISkillTriger OnClone() { var copy = new StateImpactTrigger(); CopyTo(copy); return copy; }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj;
            int senderId, targetId;
            if (!Resolve(sender, curSectionTime, out senderObj, out senderId, out targetId)) return curSectionTime < StartTime;
            EntityController.Instance.SendImpactLogic(senderId, targetId, m_ImpactId, senderObj.SkillId, m_Duration);
            return false;
        }
    }

    /// <summary>stop_impact(start_time, impact_id);</summary>
    internal sealed class StopImpactTrigger : ImpactLogicTriggerBase
    {
        protected override ISkillTriger OnClone() { var copy = new StopImpactTrigger(); CopyTo(copy); return copy; }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj;
            int senderId, targetId;
            if (!Resolve(sender, curSectionTime, out senderObj, out senderId, out targetId)) return curSectionTime < StartTime;
            EntityController.Instance.StopImpactLogic(targetId, m_ImpactId);
            return false;
        }
    }

    /// <summary>remove_impact(start_time); clear all active impacts on the target.</summary>
    internal sealed class RemoveImpactTrigger : ImpactLogicTriggerBase
    {
        protected override ISkillTriger OnClone() { var copy = new RemoveImpactTrigger(); CopyTo(copy); return copy; }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj;
            int senderId, targetId;
            if (!Resolve(sender, curSectionTime, out senderObj, out senderId, out targetId)) return curSectionTime < StartTime;
            EntityController.Instance.StopImpactLogic(targetId, -1);
            return false;
        }
    }
}
