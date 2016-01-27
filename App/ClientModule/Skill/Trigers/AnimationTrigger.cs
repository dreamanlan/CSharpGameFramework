using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// animation(anim_name[,start_time]);
    /// 
    /// or
    /// 
    /// animation(anim_name[,start_time])
    /// {
    ///   speed(0.6, isEffectSkillTime);
    ///   playmode(1, crossFadeTime);
    /// };
    /// </summary>
    internal class AnimationTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            AnimationTriger triger = new AnimationTriger();
            triger.m_AnimName = m_AnimName;
            triger.m_StartTime = m_StartTime;
            triger.m_Speed = m_Speed;
            triger.m_IsEffectSkillTime = m_IsEffectSkillTime;
            triger.m_PlayMode = m_PlayMode;
            triger.m_CrossFadeTime = m_CrossFadeTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {
                    Animator animator = obj.GetComponent<Animator>();
                    if (null != animator) {
                        string anim = TriggerUtil.RefixAnimByConfig(m_AnimName, instance.LocalVariables, senderObj.ConfigData);
                        if (!string.IsNullOrEmpty(anim)) {
                            try {
                                if (m_PlayMode == 0) {
                                    animator.Play(anim);
                                } else {
                                    animator.CrossFade(anim, m_CrossFadeTime / 1000.0f);
                                }
                                animator.speed = m_Speed;
                                if (m_IsEffectSkillTime) {
                                    instance.TimeScale = m_Speed;
                                }                             
                            } catch (Exception ex) {
                                LogSystem.Error("[skill:{0} dsl skill id:{1}] play animation {2} throw exception:{3}.", senderObj.SkillId, instance.DslSkillId, anim, ex.Message);
                            }
                        } else {                                
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] animation is null.", senderObj.SkillId, instance.DslSkillId);
                        }
                    }
                    return false;
                } else {
                    return true;
                }
            } else {
                return false;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_AnimName = callData.GetParamId(0);
            }
            if (num > 1) {
                m_StartTime = long.Parse(callData.GetParamId(1));
            }
            m_RealStartTime = m_StartTime;
        }

        protected override void Load(Dsl.FunctionData funcData, int dslSkillId)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, dslSkillId);

                foreach (Dsl.ISyntaxComponent statement in funcData.Statements) {
                    Dsl.CallData stCall = statement as Dsl.CallData;
                    if (null != stCall && stCall.GetParamNum() > 0) {
                        string id = stCall.GetId();
                        string param = stCall.GetParamId(0);

                        if (id == "speed") {
                            m_Speed = float.Parse(param);
                            if (stCall.GetParamNum() >= 2) {
                                m_IsEffectSkillTime = bool.Parse(stCall.GetParamId(1));
                            }
                        } else if (id == "playmode") {
                            m_PlayMode = int.Parse(param);
                            if (stCall.GetParamNum() >= 2) {
                                m_CrossFadeTime = long.Parse(stCall.GetParamId(1));
                            }
                        }
                    }
                }
            }
        }

        private string m_AnimName = "";

        private float m_Speed = 1.0f;
        private bool m_IsEffectSkillTime = false;
        private float m_Weight = 1.0f;
        private int m_Layer = 0;
        private int m_WrapMode = (int)WrapMode.ClampForever;
        private int m_PlayMode = 0;
        private int m_BlendMode = 0;
        private string m_MixingNode = "";
        private long m_CrossFadeTime = 300;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// animationspeed(start_time, speed [, is_effect_skill_time]);
    /// </summary>
    public class AnimationSpeedTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            AnimationSpeedTriger copy = new AnimationSpeedTriger();
            copy.m_StartTime = m_StartTime;
            copy.m_Speed = m_Speed;
            copy.m_IsEffectSkillTime = m_IsEffectSkillTime;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) {
                return false;
            }
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            Animator animator = obj.GetComponent<Animator>();
            if (animator != null) {
                float passed_ms = curSectionTime - m_RealStartTime;
                if (passed_ms > 0) {
                    float old_speed = animator.speed;
                    float time = animator.playbackTime;
                    time -= old_speed * passed_ms / 1000.0f;
                    time += m_Speed * passed_ms / 1000.0f;
                    if (time < 0) {
                        time = 0;
                    }
                    animator.playbackTime = time;
                }
                animator.speed = m_Speed;
                if (m_IsEffectSkillTime) {
                    instance.TimeScale = m_Speed;
                }
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_Speed = float.Parse(callData.GetParamId(1));
            }
            if (num >= 3) {
                m_IsEffectSkillTime = bool.Parse(callData.GetParamId(2));
            }
            m_RealStartTime = m_StartTime;
        }

        private float m_Speed = 1.0f;
        private bool m_IsEffectSkillTime = false;

        private long m_RealStartTime = 0;
    }
}