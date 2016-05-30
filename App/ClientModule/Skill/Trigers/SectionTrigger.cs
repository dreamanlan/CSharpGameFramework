using System;
using System.Collections.Generic;
using UnityEngine;
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
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            if (0 == m_Type.CompareTo("anim")) {
                Animator animator = obj.GetComponent<Animator>();
                if (null != animator) {
                    float length = 0;
                    int layerCount = animator.layerCount;
                    for (int i = 0; i < layerCount; ++i) {
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(i);
                        LogSystem.Info("adjustsectionduration anim, skill id:{0} dsl skill id:{1}, length:{2} normalized time:{3} loop:{4} layer:{5} layer count:{6}", senderObj.SkillId, instance.DslSkillId, stateInfo.length, stateInfo.normalizedTime, stateInfo.loop, i, layerCount);
                        
                        if (length < stateInfo.length && !float.IsInfinity(stateInfo.length))
                            length = stateInfo.length;
                    }
                    if (length > Geometry.c_FloatPrecision) {
                        instance.SetCurSectionDuration((long)(length * 1000) + m_DeltaTime);
                    } else {
                        LogSystem.Warn("adjustsectionduration anim length is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                    }
                }
            } else if (0 == m_Type.CompareTo("impact")) {
                int time = EntityController.Instance.GetImpactDuration(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
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
            
        }
        private int TryGetTimeFromConfig(SkillInstance instance)
        {
            return SkillParamUtility.RefixNonStringVariable<int>(m_Type, instance);
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
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            if (m_LastKeepTime <= 0 || m_LastKeepTime + m_Interval >= curSectionTime) {
                m_LastKeepTime = curSectionTime;
                int time = EntityController.Instance.GetImpactDuration(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
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
            
        }
        
        private long m_Interval = 100;
        private long m_DeltaTime = 50;
        
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
            
            
            return copy;
        }
        public override void Reset()
        {
            
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
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            if (0 == m_Type.CompareTo("shield") && EntityController.Instance.HaveShield(senderObj.ActorId)) {
                return true;
            }
            instance.StopCurSection();
            return false;
        }
        private string m_Type = "shield";
        
    }
}
