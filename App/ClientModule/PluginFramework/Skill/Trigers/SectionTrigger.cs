using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// adjustsectionduration(type[, start_time[, delta_time]]);
    /// </summary>
    internal class AdjustSectionDurationTrigger : AbstractSkillTriger
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
                Animator animator = obj.GetComponentInChildren<Animator>();
                if (null != animator) {
                    float length = 0;
                    int layerCount = animator.layerCount;
                    for (int i = 0; i < layerCount; ++i) {
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(i);
                        //LogSystem.Info("adjustsectionduration anim, skill id:{0} dsl skill id:{1}, length:{2} normalized time:{3} loop:{4} layer:{5} layer count:{6}", senderObj.SkillId, instance.DslSkillId, stateInfo.length, stateInfo.normalizedTime, stateInfo.loop, i, layerCount);
                        
                        if (length < stateInfo.length && !float.IsInfinity(stateInfo.length))
                            length = stateInfo.length;
                    }
                    if (length > Geometry.c_FloatPrecision) {
                        long newDuration = (long)(length * 1000) + m_DeltaTime;
                        if (instance.CurSectionDuration < newDuration)
                            instance.SetCurSectionDuration(newDuration);
                    } else {
                        //LogSystem.Warn("adjustsectionduration anim length is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                        return true;
                    }
                }
            } else if (0 == m_Type.CompareTo("impact")) {
                int time = EntityController.Instance.GetImpactDuration(senderObj.ObjId, senderObj.SkillId, senderObj.Seq);
                if (time > 0) {
                    long newDuration = (long)time + m_DeltaTime;
                    if (instance.CurSectionDuration < newDuration)
                        instance.SetCurSectionDuration(newDuration);
                } else {
                    LogSystem.Warn("adjustsectionduration impact duration is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                }
            } else {
                bool handled = false;
                Animator animator = obj.GetComponentInChildren<Animator>();
                if (null != animator && null != animator.runtimeAnimatorController) {
                    float length = 0;
                    var clips = animator.runtimeAnimatorController.animationClips;
                    for (int i = 0; i < clips.Length; ++i) {
                        if (clips[i].name == m_Type) {
                            length = clips[i].length;
                            handled = true;
                            break;
                        }
                    }
                    if (length > Geometry.c_FloatPrecision) {
                        long newDuration = (long)(length * 1000) + m_DeltaTime;
                        if (instance.CurSectionDuration < newDuration)
                            instance.SetCurSectionDuration(newDuration);
                    } else {
                        LogSystem.Warn("adjustsectionduration variable time is 0, skill id:{0} dsl skill id:{1} type:{2}", senderObj.SkillId, instance.DslSkillId, m_Type);
                    }
                }
                if (!handled) {
                    int time = TryGetTimeFromConfig(instance);
                    if (time > 0) {
                        long newDuration = (long)time + m_DeltaTime;
                        if (instance.CurSectionDuration < newDuration)
                            instance.SetCurSectionDuration(newDuration);
                    } else {
                        LogSystem.Warn("adjustsectionduration variable time is 0, skill id:{0} dsl skill id:{1} type:{2}", senderObj.SkillId, instance.DslSkillId, m_Type);
                    }
                }
            }
            return false;
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
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
        private long m_DeltaTime = 0;
        
    }
    /// <summary>
    /// keepsectionforbuff(internal_time[, start_time[, delta_time]]);
    /// </summary>
    internal class KeepSectionForBuffTrigger : AbstractSkillTriger
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
                int time = EntityController.Instance.GetImpactDuration(senderObj.ObjId, senderObj.SkillId, senderObj.Seq);
                if (time > 0) {
                    instance.SetCurSectionDuration((long)time + m_DeltaTime);
                } else {
                    LogSystem.Warn("adjustsectionduration impact duration is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                }
            }
            return true;
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
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
    internal class StopSectionTrigger : AbstractSkillTriger
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
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            if (m_HaveIsContinue) {
                if (0 == m_Type.CompareTo("shield")) {
                    if (EntityController.Instance.HaveShield(senderObj.ObjId))
                        return true;
                } else {
                    if (EntityController.Instance.HaveState(senderObj.ObjId, m_Type))
                        return true;
                }
            } else {
                if (0 == m_Type.CompareTo("shield")) {
                    if (!EntityController.Instance.HaveShield(senderObj.ObjId))
                        return true;
                } else {
                    if (!EntityController.Instance.HaveState(senderObj.ObjId, m_Type))
                        return true;
                }
            }
            instance.StopCurSection();
            return false;
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
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
            Dsl.FunctionData func1 = statementData.First.AsFunction;
            Dsl.FunctionData func2 = statementData.Second.AsFunction;
            if (null != func1 && null != func2) {
                Load(func1, instance);
                LoadIf(func2, instance);
            }
        }
        private void LoadIf(Dsl.FunctionData callData, SkillInstance instance)
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
    /// <summary>
    /// gotosection(starttime,sectionnum);
    /// </summary>
    internal class GotoSectionTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            GotoSectionTrigger copy = new GotoSectionTrigger();
            copy.m_SectionNum = m_SectionNum;
            return copy;
        }

        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
        {
            if (callData.GetParamNum() >= 2) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_SectionNum = int.Parse(callData.GetParamId(1));
            }
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            if (curSectionTime < StartTime) {
                return true;
            }
            GameObject obj = sender as GameObject;
            if (obj == null) {
                return false;
            }
            if (m_SectionNum < 0) {
                return false;
            }
            instance.GoToSection = m_SectionNum;
            return false;
        }

        private int m_SectionNum = -1;
    }
}
