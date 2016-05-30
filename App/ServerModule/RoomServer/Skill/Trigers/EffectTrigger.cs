using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// emiteffect(effect_path,emit_bone,emit_impact,emit_speed[,start_time]);
    /// 
    /// or
    /// 
    /// emiteffect(effect_path,emit_bone,emit_impact,emit_speed[,start_time])
    /// {
    ///   transform(vector3(0,1,0)[,eular(0,0,0)[,vector3(1,1,1)]]);
    /// };
    /// </summary>
    internal class EmitEffectTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            EmitEffectTriger triger = new EmitEffectTriger();
            triger.m_EffectPath = m_EffectPath;
            triger.m_EmitBone = m_EmitBone;
            triger.m_EmitImpact = m_EmitImpact;
            triger.m_EmitSpeed = m_EmitSpeed;
            triger.m_Pos = m_Pos;
            triger.m_Dir = m_Dir;
            triger.m_Scale = m_Scale;
            
                        return triger;
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
            if (null != obj) {
                if (curSectionTime >= StartTime) {
                    int senderId;
                    int targetId;
                    scene.EntityController.CalcSenderAndTarget(senderObj, out senderId, out targetId);
                    string effectPath = SkillParamUtility.RefixResourceVariable(m_EffectPath, instance, senderObj.ConfigData.resources);
                    string emitBone = SkillParamUtility.RefixStringVariable(m_EmitBone, instance);
                    if (!string.IsNullOrEmpty(effectPath)) {
                        EntityInfo target = scene.EntityController.GetGameObject(targetId);
                        if (null != target) {
                            Dictionary<string, object> args;
                            TriggerUtil.CalcImpactConfig(instance, senderObj.ConfigData, out args);
                            Dictionary<string, object> addArgs = new Dictionary<string, object>() { { "emitEffect", effectPath }, { "emitSpeed", m_EmitSpeed }, { "emitDir", m_Dir }, { "emitScale", m_Scale } };
                            foreach (var pair in addArgs) {
                                if (args.ContainsKey(pair.Key)) {
                                    args[pair.Key] = pair.Value;
                                } else {
                                    args.Add(pair.Key, pair.Value);
                                }
                            }
                            scene.EntityController.TrackImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, emitBone, m_EmitImpact, m_Pos, args);
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
                StartTime = long.Parse(callData.GetParamId(4));
            }
            
        }

        protected override void Load(Dsl.FunctionData funcData, int dslSkillId)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, dslSkillId);

                Dsl.ISyntaxComponent statement = funcData.Statements.Find(st => st.GetId() == "transform");
                if (null != statement) {
                    Dsl.CallData stCall = statement as Dsl.CallData;
                    if (null != stCall) {
                        if (stCall.GetParamNum() > 0) {
                            Dsl.CallData param0 = stCall.GetParam(0) as Dsl.CallData;
                            if (null != param0)
                                m_Pos = DslUtility.CalcVector3(param0);
                        }
                        if (stCall.GetParamNum() > 1) {
                            Dsl.CallData param1 = stCall.GetParam(1) as Dsl.CallData;
                            if (null != param1)
                                m_Dir = DslUtility.CalcEularRotation(param1);
                        }
                        if (stCall.GetParamNum() > 2) {
                            Dsl.CallData param2 = stCall.GetParam(2) as Dsl.CallData;
                            if (null != param2)
                                m_Scale = DslUtility.CalcVector3(param2);
                        }
                    }
                }
            }
        }

        private string m_EffectPath = "";
        private string m_EmitBone = "";
        private int m_EmitImpact = 0;
        private float m_EmitSpeed = 10.0f;

        private Vector3 m_Pos = Vector3.Zero;
        private Quaternion m_Dir = Quaternion.Identity;
        private Vector3 m_Scale = Vector3.One;

        
    }
    /// <summary>
    /// aoeemiteffect(effect_path,emit_bone,center_x,center_y,center_z,relativeToTarget,emit_impact,emit_speed[,start_time]);
    /// 
    /// or
    /// 
    /// aoeemiteffect(effect_path,emit_bone,center_x,center_y,center_z,relativeToTarget,emit_impact,emit_speed[,start_time])
    /// {
    ///   transform(vector3(0,1,0)[,eular(0,0,0)[,vector3(1,1,1)]]);
    /// };
    /// </summary>
    internal class AoeEmitEffectTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AoeEmitEffectTriger triger = new AoeEmitEffectTriger();
            triger.m_EffectPath = m_EffectPath;
            triger.m_EmitBone = m_EmitBone;
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_EmitImpact = m_EmitImpact;
            triger.m_EmitSpeed = m_EmitSpeed;
            triger.m_Pos = m_Pos;
            triger.m_Dir = m_Dir;
            triger.m_Scale = m_Scale;
            
                        return triger;
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
            if (null != obj) {
                if (curSectionTime >= StartTime) {
                    int targetType = scene.EntityController.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                    int senderId = 0;
                    if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                        senderId = senderObj.ActorId;
                    } else {
                        senderId = senderObj.TargetActorId;
                    }
                    string effectPath = SkillParamUtility.RefixResourceVariable(m_EffectPath, instance, senderObj.ConfigData.resources);
                    string emitBone = SkillParamUtility.RefixStringVariable(m_EmitBone, instance);
                    int ct = 0;
                    TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                        Dictionary<string, object> args;
                        TriggerUtil.CalcImpactConfig(instance, senderObj.ConfigData, out args);
                        Dictionary<string, object> addArgs = new Dictionary<string, object>() { { "emitEffect", effectPath }, { "emitSpeed", m_EmitSpeed }, { "emitDir", m_Dir }, { "emitScale", m_Scale } };
                        foreach (var pair in addArgs) {
                            if (args.ContainsKey(pair.Key)) {
                                args[pair.Key] = pair.Value;
                            } else {
                                args.Add(pair.Key, pair.Value);
                            }
                        }
                        scene.EntityController.TrackImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, emitBone, m_EmitImpact, m_Pos, args);
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
                StartTime = long.Parse(callData.GetParamId(8));
            }
            
        }

        protected override void Load(Dsl.FunctionData funcData, int dslSkillId)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, dslSkillId);

                Dsl.ISyntaxComponent statement = funcData.Statements.Find(st => st.GetId() == "transform");
                if (null != statement) {
                    Dsl.CallData stCall = statement as Dsl.CallData;
                    if (null != stCall) {
                        if (stCall.GetParamNum() > 0) {
                            Dsl.CallData param0 = stCall.GetParam(0) as Dsl.CallData;
                            if (null != param0)
                                m_Pos = DslUtility.CalcVector3(param0);
                        }
                        if (stCall.GetParamNum() > 1) {
                            Dsl.CallData param1 = stCall.GetParam(1) as Dsl.CallData;
                            if (null != param1)
                                m_Dir = DslUtility.CalcEularRotation(param1);
                        }
                        if (stCall.GetParamNum() > 2) {
                            Dsl.CallData param2 = stCall.GetParam(2) as Dsl.CallData;
                            if (null != param2)
                                m_Scale = DslUtility.CalcVector3(param2);
                        }
                    }
                }
            }
        }

        private string m_EffectPath = "";
        private string m_EmitBone = "";
        private Vector3 m_RelativeCenter = Vector3.Zero;
        private bool m_RelativeToTarget = false;
        private int m_EmitImpact = 0;
        private float m_EmitSpeed = 10.0f;

        private Vector3 m_Pos = Vector3.Zero;
        private Quaternion m_Dir = Quaternion.Identity;
        private Vector3 m_Scale = Vector3.One;


        
    }
    /// <summary>
    /// hiteffect(hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime[, startTime]);
    /// </summary>
    internal class HitEffectTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            HitEffectTriger triger = new HitEffectTriger();
            triger.m_HitEffect = m_HitEffect;
            triger.m_HitEffectBone = m_HitEffectBone;
            triger.m_HitEffectStartTime = m_HitEffectStartTime;
            triger.m_HitEffectDeleteTime = m_HitEffectDeleteTime;
            triger.m_HitAnim = m_HitAnim;
            triger.m_HitAnimTime = m_HitAnimTime;
            triger.m_HitDelayTime = m_HitDelayTime;
            return triger;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime)
                return true;
            instance.SetLocalVariable("hitEffect", SkillParamUtility.RefixResourceVariable(m_HitEffect, instance, senderObj.ConfigData.resources));
            instance.SetLocalVariable("hitEffectBone", m_HitEffectBone);
            instance.SetLocalVariable("hitEffectStartTime", m_HitEffectStartTime);
            instance.SetLocalVariable("hitEffectDeleteTime", m_HitEffectDeleteTime);
            instance.SetLocalVariable("hitAnim", m_HitAnim);
            instance.SetLocalVariable("hitAnimTime", m_HitAnimTime);
            instance.SetLocalVariable("hitDelayTime", m_HitDelayTime);
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
                m_HitDelayTime = int.Parse(callData.GetParamId(6));
            } else {
                m_HitDelayTime = 0;
            }
            if (num > 7) {
                StartTime = long.Parse(callData.GetParamId(7));
            } else {
                StartTime = 0;
            }
        }

        private string m_HitEffect = string.Empty;
        private string m_HitEffectBone = string.Empty;
        private int m_HitEffectStartTime = 0;
        private int m_HitEffectDeleteTime = 1000;
        private string m_HitAnim = string.Empty;
        private int m_HitAnimTime = 1000;
        private int m_HitDelayTime = 0;
    }
}
