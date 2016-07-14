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
                        return copy;
        }

        public override void Reset()
        {
            
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
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
                int time = TryGetTimeFromConfig(instance);
                if (time > 0) {
                    instance.SetCurSectionDuration((long)time + m_DeltaTime);
                } else {
                    LogSystem.Warn("adjustsectionduration variable time is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
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
            
        }

        private int TryGetTimeFromConfig(SkillInstance inst)
        {
            return SkillParamUtility.RefixNonStringVariable<int>(m_Type, inst);
        }

        private string m_Type = "anim";//anim/impact
        private long m_DeltaTime = 50;

        
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
            return copy;
        }

        public override void Reset()
        {
            
            m_LastKeepTime = 0;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
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

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
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
            
        }
        
        private long m_Interval = 100;
        private long m_DeltaTime = 50;

        
        private long m_LastKeepTime = 0;
    }

    /// <summary>
    /// stopsection([start_time])[if(type)];
    /// stopsection([start_time])[ifnot(type)];
    /// </summary>
    public class StopSectionTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            StopSectionTrigger copy = new StopSectionTrigger();
            copy.m_Type = m_Type;
            copy.m_HaveIsContinue = m_HaveIsContinue;
            return copy;
        }
        public override void Reset()
        {            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            if (m_HaveIsContinue) {
                if (0 == m_Type.CompareTo("shield")) {
                    if (scene.EntityController.HaveShield(senderObj.ActorId))
                        return true;
                } else {
                    if (scene.EntityController.HaveState(senderObj.ActorId, m_Type))
                        return true;
                }
            } else {
                if (0 == m_Type.CompareTo("shield")) {
                    if (!scene.EntityController.HaveShield(senderObj.ActorId))
                        return true;
                } else {
                    if (!scene.EntityController.HaveState(senderObj.ActorId, m_Type))
                        return true;
                }
            }
            instance.StopCurSection();
            return false;
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
        }
        protected override void Load(Dsl.StatementData statementData, SkillInstance instance)
        {
            Dsl.FunctionData func1 = statementData.First;
            Dsl.FunctionData func2 = statementData.Second;
            if (null != func1 && null != func2) {
                Load(func1.Call, instance);
                LoadIf(func2.Call, instance);
            }
        }
        private void LoadIf(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Type = callData.GetParamId(0);
            }
            m_HaveIsContinue = callData.GetId() != "if";
        }

        private string m_Type = "shield";
        private bool m_HaveIsContinue = true;
    }
}
