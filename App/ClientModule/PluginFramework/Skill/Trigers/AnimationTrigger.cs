using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// animation(anim_name[,start_time[,normalized_anim_start_time]]);
    /// 
    /// or
    /// 
    /// animation(anim_name[,start_time[,normalized_anim_start_time]])
    /// {
    ///   speed(0.6, isEffectSkillTime);
    ///   playmode(1, crossFadeTime);
    /// };
    /// </summary>
    internal class AnimationTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AnimationTriger triger = new AnimationTriger();
            triger.m_AnimName.CopyFrom(m_AnimName);
            triger.m_NormalizedAnimStartTime.CopyFrom(m_NormalizedAnimStartTime);
            triger.m_Speed = m_Speed;
            triger.m_IsEffectSkillTime = m_IsEffectSkillTime;
            triger.m_PlayMode = m_PlayMode;
            triger.m_CrossFadeTime = m_CrossFadeTime;
            
            return triger;
        }
        public override void Reset()
        {
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (null != senderObj.TrackEffectObj)
                    obj = senderObj.TrackEffectObj;
                if (curSectionTime >= StartTime) {
                    Animator animator = obj.GetComponentInChildren<Animator>();
                    if (null != animator) {
                        string anim = m_AnimName.Get(instance);
                        if (!string.IsNullOrEmpty(anim)) {
                            try {
                                if (m_PlayMode == 0) {
                                    animator.Play(anim, -1, m_NormalizedAnimStartTime.Get(instance));
                                } else {
                                    animator.CrossFade(anim, m_CrossFadeTime / 1000.0f, -1, m_NormalizedAnimStartTime.Get(instance));
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
        protected override void OnInitProperties()
        {
            AddProperty("AnimName", () => { return m_AnimName.EditableValue; }, (object val) => { m_AnimName.EditableValue = val; });
            AddProperty("NormalizedAnimStartTime", () => { return m_NormalizedAnimStartTime.EditableValue; }, (object val) => { m_NormalizedAnimStartTime.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance )
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_AnimName.Set(callData.GetParam(0));
            }
            if (num > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            } else {
                StartTime = 0;
            }
            if (num > 2) {
                m_NormalizedAnimStartTime.Set(callData.GetParam(2));
            } else {
                m_NormalizedAnimStartTime.Set(0.0f);
            }
            
        }
        protected override void Load(Dsl.FunctionData funcData, SkillInstance instance )
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, instance);
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

        private SkillStringParam m_AnimName = new SkillStringParam();
        private SkillNonStringParam<float> m_NormalizedAnimStartTime = new SkillNonStringParam<float>();
        private float m_Speed = 1.0f;
        private bool m_IsEffectSkillTime = false;
        private int m_PlayMode = 0;
        private long m_CrossFadeTime = 300;
        
    }
    /// <summary>
    /// animationspeed(start_time, speed [, is_effect_skill_time]);
    /// </summary>
    internal class AnimationSpeedTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AnimationSpeedTriger copy = new AnimationSpeedTriger();
            
            copy.m_Speed = m_Speed;
            copy.m_IsEffectSkillTime = m_IsEffectSkillTime;
            
            return copy;
        }
        public override void Reset()
        {
            
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
            if (null != senderObj.TrackEffectObj)
                obj = senderObj.TrackEffectObj;
            if (curSectionTime < StartTime) {
                return true;
            }
            Animator animator = obj.GetComponentInChildren<Animator>();
            if (animator != null) {
                float passed_ms = curSectionTime - StartTime;
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
        protected override void OnInitProperties()
        {
            AddProperty("Speed", () => { return m_Speed; }, (object val) => { m_Speed = (float)Convert.ChangeType(val, typeof(float)); });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance )
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_Speed = float.Parse(callData.GetParamId(1));
            }
            if (num >= 3) {
                m_IsEffectSkillTime = bool.Parse(callData.GetParamId(2));
            }
            
        }
        private float m_Speed = 1.0f;
        private bool m_IsEffectSkillTime = false;        
    }
    /// <summary>
    /// animationparameter([start_time])
    /// {
    ///     float(name,val);
    ///     int(name,val);
    ///     bool(name,val);
    ///     trigger(name,val);
    /// };
    /// </summary>
    internal class AnimationParameterTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AnimationParameterTriger triger = new AnimationParameterTriger();
            triger.m_Params = new Dictionary<string, object>(m_Params);
            return triger;
        }
        public override void Reset()
        {

        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (null != senderObj.TrackEffectObj)
                    obj = senderObj.TrackEffectObj;
                if (curSectionTime >= StartTime) {
                    Animator animator = obj.GetComponentInChildren<Animator>();
                    if (null != animator) {
                        foreach (var pair in m_Params) {
                            string key = pair.Key;
                            object val = pair.Value;
                            if (val is int) {
                                animator.SetInteger(key, (int)val);
                            } else if (val is bool) {
                                animator.SetBool(key, (bool)val);
                            } else if (val is float) {
                                animator.SetFloat(key, (float)val);
                            } else if (val is string) {
                                string v = val as string;
                                if (v == "false")
                                    animator.ResetTrigger(key);
                                else
                                    animator.SetTrigger(key);
                            }
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
        protected override void OnInitProperties()
        {
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance )
        {
            m_Params = new Dictionary<string, object>();
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
        }

        protected override void Load(Dsl.FunctionData funcData, SkillInstance instance )
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, instance);
                for (int i = 0; i < funcData.Statements.Count; ++i) {
                    Dsl.ISyntaxComponent statement = funcData.Statements[i];
                    Dsl.CallData stCall = statement as Dsl.CallData;
                    if (null != stCall) {
                        string id = stCall.GetId();
                        string key = stCall.GetParamId(0);
                        object val = string.Empty;
                        if (id == "int") {
                            val = int.Parse(stCall.GetParamId(1));
                        } else if (id == "float") {
                            val = float.Parse(stCall.GetParamId(1));
                        } else if (id == "bool") {
                            val = bool.Parse(stCall.GetParamId(1));
                        } else if (id == "trigger") {
                            val = stCall.GetParamId(1);
                        }
                        m_Params.Add(key, val);
                    }
                }
            }
        }

        private Dictionary<string, object> m_Params = null;
    }
    /// <summary>
    /// animationevent(anim_name_or_tag, normalized_fire_event_time, message[, start_time]);
    /// 
    /// or
    /// 
    /// animationevent(anim_name_or_tag, normalized_fire_event_time, message[, start_time]);
    /// {
    ///     int(name,value);
    ///     long(name,value);
    ///     float(name,value);
    ///     double(name,value);
    ///     string(name,value);
    ///     ...
    /// };
    /// </summary>
    internal class AnimationEventTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AnimationEventTriger triger = new AnimationEventTriger();
            triger.m_AnimName.CopyFrom(m_AnimName);
            triger.m_NormalizedFireEventTime.CopyFrom(m_NormalizedFireEventTime);
            triger.m_MsgId.CopyFrom(m_MsgId);
            triger.m_Params = new Dictionary<string, object>(m_Params);
            return triger;
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
            if (null != senderObj.TrackEffectObj)
                obj = senderObj.TrackEffectObj;
            if (curSectionTime >= StartTime) {
                Animator animator = obj.GetComponentInChildren<Animator>();
                if (null != animator) {
                    string anim = m_AnimName.Get(instance);
                    string msgId = m_MsgId.Get(instance);
                    float time = m_NormalizedFireEventTime.Get(instance);
                    for (int ix = 0; ix < animator.layerCount; ++ix) {
                        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(ix);
                        if (!string.IsNullOrEmpty(anim) && !string.IsNullOrEmpty(msgId) && (stateInfo.IsName(anim) || stateInfo.IsTag(anim))) {
                            if ((stateInfo.normalizedTime % 1.0f) >= time) {
                                GfxSkillSystem.Instance.SendMessage(senderObj.ObjId, senderObj.SkillId, senderObj.Seq, msgId, m_Params);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        protected override void OnInitProperties()
        {
            AddProperty("AnimName", () => { return m_AnimName.EditableValue; }, (object val) => { m_AnimName.EditableValue = val; });
            AddProperty("NormalizedFireEventTime", () => { return m_NormalizedFireEventTime.EditableValue; }, (object val) => { m_NormalizedFireEventTime.EditableValue = val; });
            AddProperty("MessageName", () => { return m_MsgId.EditableValue; }, (object val) => { m_MsgId.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance )
        {
            m_Params = new Dictionary<string, object>();
            int num = callData.GetParamNum();
            if (num > 0) {
                m_AnimName.Set(callData.GetParam(0));
            }
            if (num > 1) {
                m_NormalizedFireEventTime.Set(callData.GetParam(1));
            } else {
                m_NormalizedFireEventTime.Set(0.0f);
            }
            if (num > 2) {
                m_MsgId.Set(callData.GetParam(2));
            }
            if (num > 3) {
                StartTime = long.Parse(callData.GetParamId(3));
            } else {
                StartTime = 0;
            }
        }
        protected override void Load(Dsl.FunctionData funcData, SkillInstance instance )
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, instance);

                for (int i = 0; i < funcData.Statements.Count; ++i) {
                    Dsl.ISyntaxComponent statement = funcData.Statements[i];
                    Dsl.CallData stCall = statement as Dsl.CallData;
                    if (null != stCall) {
                        string id = stCall.GetId();
                        string key = stCall.GetParamId(0);
                        object val = string.Empty;
                        if (id == "int") {
                            val = int.Parse(stCall.GetParamId(1));
                        } else if (id == "long") {
                            val = long.Parse(stCall.GetParamId(1));
                        } else if (id == "float") {
                            val = float.Parse(stCall.GetParamId(1));
                        } else if (id == "double") {
                            val = double.Parse(stCall.GetParamId(1));
                        } else if (id == "string") {
                            val = stCall.GetParamId(1);
                        }
                        m_Params.Add(key, val);
                    }
                }
            }
        }

        private SkillStringParam m_AnimName = new SkillStringParam();
        private SkillNonStringParam<float> m_NormalizedFireEventTime = new SkillNonStringParam<float>();
        private SkillStringParam m_MsgId = new SkillStringParam();
        private Dictionary<string, object> m_Params = null;
    }
}