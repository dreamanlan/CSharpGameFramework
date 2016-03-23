using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// emiteffect(effect_path,emit_bone,emit_impact,emit_speed[,start_time]);
    /// </summary>
    internal class EmitEffectTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            EmitEffectTriger triger = new EmitEffectTriger();
            triger.m_EffectPath = m_EffectPath;
            triger.m_EmitBone = m_EmitBone;
            triger.m_EmitImpact = m_EmitImpact;
            triger.m_EmitSpeed = m_EmitSpeed;
            triger.m_StartTime = m_StartTime;
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixEffectStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {
                    int senderId;
                    int targetId;
                    scene.EntityController.CalcSenderAndTarget(senderObj, out senderId, out targetId);
                    string effectPath = TriggerUtil.RefixResourceByConfig(m_EffectPath, instance.LocalVariables, senderObj.ConfigData);
                    string emitBone = TriggerUtil.RefixStringVariable(m_EmitBone, instance.LocalVariables, senderObj.ConfigData);
                    if (!string.IsNullOrEmpty(effectPath)) {
                        EntityInfo target = scene.EntityController.GetGameObject(targetId);
                        if (null != target) {
                            string hitEffect;
                            string hitEffectBone;
                            int hitEffectStartTime;
                            int hitEffectDeleteTime;
                            string hitAnim;
                            int hitAnimTime;
                            TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                            scene.EntityController.TrackImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, effectPath, emitBone, m_EmitImpact, m_EmitSpeed, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
                        }
                    } else {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] emit effect is empty.", senderObj.SkillId, instance.DslSkillId);
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
                m_EffectPath = callData.GetParamId(0);
            }
            if (num > 1) {
                m_EmitBone = callData.GetParamId(1);
            }
            if (num > 2) {
                m_EmitImpact = int.Parse(callData.GetParamId(2));
            }
            if (num > 3) {
                m_EmitSpeed = float.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                m_StartTime = long.Parse(callData.GetParamId(4));
            }
            m_RealStartTime = m_StartTime;
        }
        
        private string m_EffectPath = "";
        private string m_EmitBone = "";
        private int m_EmitImpact = 0;
        private float m_EmitSpeed = 10.0f;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// aoeemiteffect(effect_path,emit_bone,center_x,center_y,center_z,relativeToTarget,emit_impact,emit_speed[,start_time]);
    /// </summary>
    internal class AoeEmitEffectTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            AoeEmitEffectTriger triger = new AoeEmitEffectTriger();
            triger.m_EffectPath = m_EffectPath;
            triger.m_EmitBone = m_EmitBone;
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_EmitImpact = m_EmitImpact;
            triger.m_EmitSpeed = m_EmitSpeed;
            triger.m_StartTime = m_StartTime;
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixEffectStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {
                    int targetType = scene.EntityController.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                    int senderId = 0;
                    if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                        senderId = senderObj.ActorId;
                    } else {
                        senderId = senderObj.TargetActorId;
                    }
                    string effectPath = TriggerUtil.RefixResourceByConfig(m_EffectPath, instance.LocalVariables, senderObj.ConfigData);
                    string emitBone = TriggerUtil.RefixStringVariable(m_EmitBone, instance.LocalVariables, senderObj.ConfigData);
                    int ct = 0;
                    TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                        string hitEffect;
                        string hitEffectBone;
                        int hitEffectStartTime;
                        int hitEffectDeleteTime;
                        string hitAnim;
                        int hitAnimTime;
                        TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                        scene.EntityController.TrackImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, effectPath, emitBone, m_EmitImpact, m_EmitSpeed, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
                        ++ct;
                        if (senderObj.ConfigData.maxAoeTargetCount <= 0 || ct < senderObj.ConfigData.maxAoeTargetCount) {
                            return true;
                        } else {
                            return false;
                        }
                    });
                    //GameFramework.LogSystem.Debug("AoeEmitEffectTriger:{0}", m_EffectPath);
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
                m_EffectPath = callData.GetParamId(0);
            }
            if (num > 1) {
                m_EmitBone = callData.GetParamId(1);
            }
            if (num > 5) {
                m_RelativeCenter.X = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.Y = float.Parse(callData.GetParamId(3));
                m_RelativeCenter.Z = float.Parse(callData.GetParamId(4));
                m_RelativeToTarget = callData.GetParamId(5) == "true";
            }
            if (num > 6) {
                m_EmitImpact = int.Parse(callData.GetParamId(6));
            }
            if (num > 7) {
                m_EmitImpact = int.Parse(callData.GetParamId(7));
            }
            if (num > 8) {
                m_StartTime = long.Parse(callData.GetParamId(8));
            }
            m_RealStartTime = m_StartTime;
        }
        
        private string m_EffectPath = "";
        private string m_EmitBone = "";
        private Vector3 m_RelativeCenter = Vector3.Zero;
        private bool m_RelativeToTarget = false;
        private int m_EmitImpact = 0;
        private float m_EmitSpeed = 10.0f;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// hiteffect(hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime[, startTime]);
    /// </summary>
    internal class HitEffectTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            HitEffectTriger triger = new HitEffectTriger();
            triger.m_HitEffect = m_HitEffect;
            triger.m_HitEffectBone = m_HitEffectBone;
            triger.m_HitEffectStartTime = m_HitEffectStartTime;
            triger.m_HitEffectDeleteTime = m_HitEffectDeleteTime;
            triger.m_HitAnim = m_HitAnim;
            triger.m_HitAnimTime = m_HitAnimTime;
            return triger;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < m_StartTime)
                return true;
            instance.SetLocalVariable("hitEffect", TriggerUtil.RefixResourceByConfig(m_HitEffect, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitEffectBone", TriggerUtil.RefixStringVariable(m_HitEffectBone, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitEffectStartTime", TriggerUtil.RefixEffectStartTime(m_HitEffectStartTime, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitEffectDeleteTime", TriggerUtil.RefixEffectDeleteTime(m_HitEffectDeleteTime, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitAnim", TriggerUtil.RefixStringVariable(m_HitAnim, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitAnimTime", TriggerUtil.RefixAnimTime(m_HitAnimTime, instance.LocalVariables, senderObj.ConfigData));
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_HitEffect = callData.GetParamId(0);
            }
            if (num > 1) {
                m_HitEffectBone = callData.GetParamId(1);
            }
            if (num > 2) {
                m_HitEffectStartTime = int.Parse(callData.GetParamId(2));
            }
            if (num > 3) {
                m_HitEffectDeleteTime = int.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                m_HitAnim = callData.GetParamId(4);
            }
            if (num > 5) {
                m_HitAnimTime = int.Parse(callData.GetParamId(5));
            }
            if (num > 6) {
                m_StartTime = long.Parse(callData.GetParamId(6));
            } else {
                m_StartTime = 0;
            }
        }

        private string m_HitEffect = string.Empty;
        private string m_HitEffectBone = string.Empty;
        private int m_HitEffectStartTime = 0;
        private int m_HitEffectDeleteTime = 1000;
        private string m_HitAnim = string.Empty;
        private int m_HitAnimTime = 1000;
    }
}
