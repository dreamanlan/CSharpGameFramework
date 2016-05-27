using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// adjustsectionduration(type[, start_time[, delta_time]]);
    /// </summary>
    public class AdjustSectionDurationTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AdjustSectionDurationTrigger copy = new AdjustSectionDurationTrigger();
            
            copy.m_Type = m_Type;
            copy.m_DeltaTime = m_DeltaTime;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (0 == m_Type.CompareTo("anim")) {
            } else if (0 == m_Type.CompareTo("impact")) {
                int time = scene.EntityController.GetImpactDuration(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                if (time > 0) {
                    instance.SetCurSectionDuration((long)time + m_DeltaTime);
                } else {
                    LogSystem.Warn("adjustsectionduration impact duration is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                }
            } else {
                int time = TryGetTimeFromConfig(instance.LocalVariables, senderObj.ConfigData);
                if (time > 0) {
                    instance.SetCurSectionDuration((long)time + m_DeltaTime);
                } else {
                    LogSystem.Warn("adjustsectionduration variable time is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() > 0) {
                m_Type = callData.GetParamId(0);
            }
            if (callData.GetParamNum() > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            }
            if (callData.GetParamNum() > 2) {
                m_DeltaTime = long.Parse(callData.GetParamId(2));
            }
            m_RealStartTime = StartTime;
        }

        private int TryGetTimeFromConfig(Dictionary<string, object> variables, TableConfig.Skill cfg)
        {
            return TriggerUtil.RefixIntVariable(m_Type, variables, cfg);
        }

        private string m_Type = "anim";//anim/impact
        private long m_DeltaTime = 50;

        private long m_RealStartTime = 0;
    }

    /// <summary>
    /// keepsectionforbuff(internal_time[, start_time[, delta_time]]);
    /// </summary>
    public class KeepSectionForBuffTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            KeepSectionForBuffTrigger copy = new KeepSectionForBuffTrigger();
            
            copy.m_Interval = m_Interval;
            copy.m_DeltaTime = m_DeltaTime;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = StartTime;
            m_LastKeepTime = 0;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (m_LastKeepTime <= 0 || m_LastKeepTime + m_Interval >= curSectionTime) {
                m_LastKeepTime = curSectionTime;
                int time = scene.EntityController.GetImpactDuration(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                if (time > 0) {
                    instance.SetCurSectionDuration((long)time + m_DeltaTime);
                } else {
                    LogSystem.Warn("adjustsectionduration impact duration is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                }
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() > 0) {
                m_Interval = long.Parse(callData.GetParamId(0));
            }
            if (callData.GetParamNum() > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            }
            if (callData.GetParamNum() > 2) {
                m_DeltaTime = long.Parse(callData.GetParamId(2));
            }
            m_RealStartTime = StartTime;
        }
        
        private long m_Interval = 100;
        private long m_DeltaTime = 50;

        private long m_RealStartTime = 0;
        private long m_LastKeepTime = 0;
    }

    /// <summary>
    /// stopsectionif(type[, start_time]);
    /// </summary>
    public class StopSectionIfTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            StopSectionIfTrigger copy = new StopSectionIfTrigger();
            copy.m_Type = m_Type;
            
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Type = callData.GetParamId(0);
            }
            if (num > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            } else {
                StartTime = 0;
            }
            m_RealStartTime = StartTime;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (0 == m_Type.CompareTo("shield") && scene.EntityController.HaveShield(senderObj.ActorId)) {
                return true;
            }
            instance.StopCurSection();
            return false;
        }

        private string m_Type = "shield";

        private long m_RealStartTime = 0;
    }
}
