using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    public class EffectManager
    {
        public void AddEffect(GameObject obj)
        {
            m_EffectObject.Add(obj);
        }

        public void SetParticleSpeed(float speed)
        {
            for (int i = 0; i < m_EffectObject.Count; i++) {
                if (m_EffectObject[i].activeSelf) {
                    ParticleSystem[] ps = m_EffectObject[i].GetComponentsInChildren<ParticleSystem>();
                    for (int j = 0; j < ps.Length; j++) {
                        ps[j].playbackSpeed = speed;
                    }
                }
            }
        }

        public void StopEffects()
        {
            for (int i = 0; i < m_EffectObject.Count; i++) {
                ResourceSystem.Instance.RecycleObject(m_EffectObject[i]);
            }
            m_EffectObject.Clear();
        }
        private List<GameObject> m_EffectObject = new List<GameObject>();
    }
    /// <summary>
    /// selfeffect(effect_path,delete_time,attach_bone[,start_time[,is_attach]]);
    /// 
    /// or
    /// 
    /// selfeffect(effect_path,delete_time,attach_bone[,start_time[,is_attach]])
    /// {
    ///   transform(vector3(0,1,0)[,eular(0,0,0)[,vector3(1,1,1)]]);
    /// };
    /// </summary>
    internal class SelfEffectTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            SelfEffectTriger triger = new SelfEffectTriger();
            triger.m_EffectPath = m_EffectPath;
            triger.m_AttachPath = m_AttachPath;
            triger.m_DeleteTime = m_DeleteTime;
            triger.m_StartTime = m_StartTime;
            triger.m_IsAttach = m_IsAttach;
            triger.m_Pos = m_Pos;
            triger.m_Dir = m_Dir;
            triger.m_Scale = m_Scale;
            triger.m_RealStartTime = m_RealStartTime;
            triger.m_RealDeleteTime = m_RealDeleteTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
            m_RealDeleteTime = m_DeleteTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixEffectStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {
                    if (m_RealDeleteTime < 0) {
                        m_RealDeleteTime = TriggerUtil.RefixEffectDeleteTimeByConfig((int)m_DeleteTime, instance.LocalVariables, senderObj.ConfigData);
                    }
                    if (m_RealDeleteTime <= 0) {
                        m_RealDeleteTime = (long)(senderObj.ConfigData.duration * 1000);
                        if (m_RealDeleteTime <= 0) {
                            m_RealDeleteTime = EntityController.Instance.GetImpactDuration(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                        }
                    }
                    if (m_RealDeleteTime <= 0) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] selfeffect deleteTime <= 0.", senderObj.SkillId, instance.DslSkillId);
                        return false;
                    }
                    string effectPath = TriggerUtil.RefixEffectByConfig(m_EffectPath, instance.LocalVariables, senderObj.ConfigData);
                    string attachPath = TriggerUtil.RefixBoneByConfig(m_AttachPath, instance.LocalVariables, senderObj.ConfigData);
                    GameObject effectObj = null;
                    if (string.IsNullOrEmpty(effectPath)) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] selfeffect effect is empty.", senderObj.SkillId, instance.DslSkillId);
                    } else {
                        effectObj = ResourceSystem.Instance.NewObject(effectPath, m_RealDeleteTime / 1000.0f) as GameObject;
                        if (null == effectObj) {
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] selfeffect effect {2} can't find.", senderObj.SkillId, instance.DslSkillId, effectPath);
                        }
                    }
                    if (null != effectObj) {
                        TriggerUtil.SetObjVisible(effectObj, true);
                        Transform bone = GameFramework.Utility.FindChildRecursive(obj.transform, attachPath);
                        if (null == bone) {
                            bone = obj.transform;
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] selfeffect bone {2} can't find.", senderObj.SkillId, instance.DslSkillId, attachPath);
                        }
                        effectObj.SetActive(false);
                        if (null != bone) {
                            effectObj.transform.parent = bone;
                            effectObj.transform.localPosition = m_Pos;
                            object effectRotation;
                            if (instance.LocalVariables.TryGetValue("hitEffectRotation", out effectRotation)) {
                                effectObj.transform.localRotation = (Quaternion)effectRotation;
                            } else {
                                effectObj.transform.localRotation = m_Dir;
                            }
                            effectObj.transform.localScale = m_Scale;
                            if (!m_IsAttach) {
                                effectObj.transform.parent = null;
                            }
                            EffectManager em = instance.CustomDatas.GetData<EffectManager>();
                            if (em == null) {
                                em = new EffectManager();
                                instance.CustomDatas.AddData<EffectManager>(em);
                            }
                            em.AddEffect(effectObj);
                            em.SetParticleSpeed(instance.EffectScale);
                        }
                        effectObj.SetActive(true);
                    }

                    //GameFramework.LogSystem.Debug("SelfEffectTriger:{0}", m_EffectPath);
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
                m_DeleteTime = long.Parse(callData.GetParamId(1));
            }
            if (num > 2) {
                m_AttachPath = callData.GetParamId(2);
            }
            if (num > 3) {
                m_StartTime = long.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                m_IsAttach = bool.Parse(callData.GetParamId(4));
            }
            m_RealStartTime = m_StartTime;
            m_RealDeleteTime = m_DeleteTime;
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
        private string m_AttachPath = "";
        private long m_DeleteTime = 0;
        private bool m_IsAttach = true;

        private Vector3 m_Pos = Vector3.zero;
        private Quaternion m_Dir = Quaternion.identity;
        private Vector3 m_Scale = Vector3.one;

        private long m_RealStartTime = 0;
        private long m_RealDeleteTime = 0;
    }
    /// <summary>
    /// targeteffect(effect_path,delete_time,attach_bone[,start_time[,is_attach]]);
    /// 
    /// or
    /// 
    /// targeteffect(effect_path,delete_time,attach_bone[,start_time[,is_attach]])
    /// {
    ///   transform(vector3(0,1,0)[,eular(0,0,0)[,vector3(1,1,1)]]);
    /// };
    /// </summary>
    internal class TargetEffectTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            TargetEffectTriger triger = new TargetEffectTriger();
            triger.m_EffectPath = m_EffectPath;
            triger.m_AttachPath = m_AttachPath;
            triger.m_DeleteTime = m_DeleteTime;
            triger.m_StartTime = m_StartTime;
            triger.m_IsAttach = m_IsAttach;
            triger.m_Pos = m_Pos;
            triger.m_Dir = m_Dir;
            triger.m_Scale = m_Scale;
            triger.m_RealStartTime = m_RealStartTime;
            triger.m_RealDeleteTime = m_RealDeleteTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
            m_RealDeleteTime = m_DeleteTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixEffectStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {
                    GameObject target_obj = senderObj.TargetGfxObj;
                    if (null == target_obj) {
                        return false;
                    }
                    if (m_RealDeleteTime < 0) {
                        m_RealDeleteTime = TriggerUtil.RefixEffectDeleteTimeByConfig((int)m_DeleteTime, instance.LocalVariables, senderObj.ConfigData);
                    }
                    if (m_RealDeleteTime <= 0) {
                        m_RealDeleteTime = (long)(senderObj.ConfigData.duration * 1000);
                        if (m_RealDeleteTime <= 0) {
                            m_RealDeleteTime = EntityController.Instance.GetImpactDuration(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                        }
                    }
                    if (m_RealDeleteTime <= 0) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] targeteffect deleteTime <= 0.", senderObj.SkillId, instance.DslSkillId);
                        return false;
                    }
                    string effectPath = TriggerUtil.RefixEffectByConfig(m_EffectPath, instance.LocalVariables, senderObj.ConfigData);
                    string attachPath = TriggerUtil.RefixBoneByConfig(m_AttachPath, instance.LocalVariables, senderObj.ConfigData);
                    GameObject effectObj = null;
                    if (string.IsNullOrEmpty(effectPath)) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] targeteffect effect is empty.", senderObj.SkillId, instance.DslSkillId);
                    } else {
                        effectObj = ResourceSystem.Instance.NewObject(effectPath, m_RealDeleteTime / 1000.0f) as GameObject;
                        if (null == effectObj) {
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] targeteffect effect {2} can't find.", senderObj.SkillId, instance.DslSkillId, effectPath);
                        }
                    }
                    if (null != effectObj) {
                        TriggerUtil.SetObjVisible(effectObj, true);
                        Transform bone = GameFramework.Utility.FindChildRecursive(target_obj.transform, attachPath);
                        if (null == bone) {
                            bone = obj.transform;
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] targeteffect bone {2} can't find.", senderObj.SkillId, instance.DslSkillId, attachPath);
                        }
                        effectObj.SetActive(false);
                        if (null != bone) {
                            effectObj.transform.parent = bone;
                            effectObj.transform.localPosition = m_Pos;
                            effectObj.transform.localRotation = m_Dir;
                            effectObj.transform.localScale = m_Scale;
                            if (!m_IsAttach) {
                                effectObj.transform.parent = null;
                            }
                            EffectManager em = instance.CustomDatas.GetData<EffectManager>();
                            if (em == null) {
                                em = new EffectManager();
                                instance.CustomDatas.AddData<EffectManager>(em);
                            }
                            em.AddEffect(effectObj);
                            em.SetParticleSpeed(instance.EffectScale);
                        }
                        effectObj.SetActive(true);
                    }

                    //GameFramework.LogSystem.Debug("TargetEffectTriger:{0}", m_EffectPath);
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
                m_DeleteTime = long.Parse(callData.GetParamId(1));
            }
            if (num > 2) {
                m_AttachPath = callData.GetParamId(2);
            }
            if (num > 3) {
                m_StartTime = long.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                m_IsAttach = bool.Parse(callData.GetParamId(4));
            }
            m_RealStartTime = m_StartTime;
            m_RealDeleteTime = m_DeleteTime;
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
        private string m_AttachPath = "";
        private long m_DeleteTime = 0;
        private bool m_IsAttach = true;

        private Vector3 m_Pos = Vector3.zero;
        private Quaternion m_Dir = Quaternion.identity;
        private Vector3 m_Scale = Vector3.one;

        private long m_RealStartTime = 0;
        private long m_RealDeleteTime = 0;
    }
    /// <summary>
    /// sceneeffect(effect_path,delete_time[,vector3(x,y,z)[,start_time[,eular(rx,ry,rz)[,vector3(sx,sy,sz)]]]]);
    /// </summary>
    internal class SceneEffectTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            SceneEffectTriger triger = new SceneEffectTriger();
            triger.m_EffectPath = m_EffectPath;
            triger.m_Pos = m_Pos;
            triger.m_Dir = m_Dir;
            triger.m_Scale = m_Scale;
            triger.m_DeleteTime = m_DeleteTime;
            triger.m_StartTime = m_StartTime;
            triger.m_IsRotateRelativeUser = m_IsRotateRelativeUser;
            triger.m_RealStartTime = m_RealStartTime;
            triger.m_RealDeleteTime = m_RealDeleteTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
            m_RealDeleteTime = m_DeleteTime;
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
                    if (m_RealDeleteTime < 0) {
                        m_RealDeleteTime = TriggerUtil.RefixEffectDeleteTimeByConfig((int)m_DeleteTime, instance.LocalVariables, senderObj.ConfigData);
                    }
                    if (m_RealDeleteTime <= 0) {
                        m_RealDeleteTime = (long)(senderObj.ConfigData.duration * 1000);
                    }
                    if (m_RealDeleteTime <= 0) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] sceneeffect deleteTime <= 0.", senderObj.SkillId, instance.DslSkillId);
                        return false;
                    }
                    string effectPath = TriggerUtil.RefixEffectByConfig(m_EffectPath, instance.LocalVariables, senderObj.ConfigData);
                    GameObject effectObj = null;
                    if (string.IsNullOrEmpty(effectPath)) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] sceneeffect effect is empty.", senderObj.SkillId, instance.DslSkillId);
                    } else {
                        effectObj = ResourceSystem.Instance.NewObject(effectPath, m_RealDeleteTime / 1000.0f) as GameObject;
                        if (null == effectObj) {
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] sceneeffect effect {2} can't find.", senderObj.SkillId, instance.DslSkillId, effectPath);
                        }
                    }
                    if (null != effectObj) {
                        TriggerUtil.SetObjVisible(effectObj, true);
                        effectObj.SetActive(false);
                        Vector3 pos = obj.transform.TransformPoint(m_Pos);
                        effectObj.transform.position = pos;
                        effectObj.transform.localScale = m_Scale;
                        if (m_IsRotateRelativeUser) {
                            effectObj.transform.parent = obj.transform;
                            effectObj.transform.localRotation = m_Dir;
                            effectObj.transform.parent = null;
                        } else {
                            effectObj.transform.localRotation = m_Dir;
                        }
                        EffectManager em = instance.CustomDatas.GetData<EffectManager>();
                        if (em == null) {
                            em = new EffectManager();
                            instance.CustomDatas.AddData<EffectManager>(em);
                        }
                        em.AddEffect(effectObj);
                        em.SetParticleSpeed(instance.EffectScale);
                        effectObj.SetActive(true);
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
                m_DeleteTime = long.Parse(callData.GetParamId(1));
            }
            if (num > 2) {
                m_Pos = DslUtility.CalcVector3(callData.GetParam(2) as Dsl.CallData);
            }
            if (num > 3) {
                m_StartTime = long.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                m_Dir = DslUtility.CalcEularRotation(callData.GetParam(4) as Dsl.CallData);
            }
            if (num > 5) {
                m_Scale = DslUtility.CalcVector3(callData.GetParam(5) as Dsl.CallData);
            }
            if (num > 6) {
                m_IsRotateRelativeUser = bool.Parse(callData.GetParamId(6));
            }
            m_RealStartTime = m_StartTime;
            m_RealDeleteTime = m_DeleteTime;
        }

        private string m_EffectPath = "";
        private Vector3 m_Pos = Vector3.zero;
        private Quaternion m_Dir = Quaternion.identity;
        private Vector3 m_Scale = Vector3.one;
        private long m_DeleteTime = 0;
        private bool m_IsRotateRelativeUser = false;

        private long m_RealStartTime = 0;
        private long m_RealDeleteTime = 0;
    }
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
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixEffectStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {
                    int senderId;
                    int targetId;
                    EntityController.Instance.CalcSenderAndTarget(senderObj, out senderId, out targetId);
                    string effectPath = TriggerUtil.RefixEffectByConfig(m_EffectPath, instance.LocalVariables, senderObj.ConfigData);
                    string emitBone = TriggerUtil.RefixBoneByConfig(m_EmitBone, instance.LocalVariables, senderObj.ConfigData);
                    if (!string.IsNullOrEmpty(effectPath)) {
                        GameObject target = EntityController.Instance.GetGameObject(targetId);
                        if (null != target) {
                            string hitEffect;
                            string hitEffectBone;
                            int hitEffectStartTime;
                            int hitEffectDeleteTime;
                            string hitAnim;
                            int hitAnimTime;
                            TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                            EntityController.Instance.TrackImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, effectPath, emitBone, m_EmitImpact, m_EmitSpeed, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
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
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixEffectStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {
                    int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                    int senderId = 0;
                    if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                        senderId = senderObj.ActorId;
                    } else {
                        senderId = senderObj.TargetActorId;
                    }
                    string effectPath = TriggerUtil.RefixEffectByConfig(m_EffectPath, instance.LocalVariables, senderObj.ConfigData);
                    string emitBone = TriggerUtil.RefixBoneByConfig(m_EmitBone, instance.LocalVariables, senderObj.ConfigData);
                    int ct = 0;
                    TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                        string hitEffect;
                        string hitEffectBone;
                        int hitEffectStartTime;
                        int hitEffectDeleteTime;
                        string hitAnim;
                        int hitAnimTime;
                        TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                        EntityController.Instance.TrackImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, effectPath, emitBone, m_EmitImpact, m_EmitSpeed, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
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
                m_RelativeCenter.x = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(3));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(4));
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
        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        private int m_EmitImpact = 0;
        private float m_EmitSpeed = 10.0f;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// lockframe(startime, is_need_hit, lock_speed, locktime, after_lock_speed, restore_time);
    /// </summary>
    internal class LockFrameTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            LockFrameTriger triger = new LockFrameTriger();
            triger.m_StartTime = m_StartTime;
            triger.m_NeedHit = m_NeedHit;
            triger.m_LockSpeed = m_LockSpeed;
            triger.m_LockTime = m_LockTime;
            triger.m_AfterLockAnimSpeed = m_AfterLockAnimSpeed;
            triger.m_AfterLockEffectSpeed = m_AfterLockEffectSpeed;
            triger.m_AfterLockMoveSpeed = m_AfterLockMoveSpeed;
            triger.m_AfterLockSkillSpeed = m_AfterLockSkillSpeed;
            triger.m_RestoreTime = m_RestoreTime;
            triger.m_IsEffectSkillTime = m_IsEffectSkillTime;
            triger.m_Curve = m_Curve;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }

        public override void Reset()
        {
            m_IsInited = false;
            m_RealStartTime = m_StartTime;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj)
                return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            Animator animator = obj.GetComponent<Animator>();
            if (animator == null) {
                return false;
            }

            if (!m_IsInited) {
                Init();
            }

            if (m_NeedHit) {
            }
            if (Time.time <= m_WorldStartTime + m_LockTime / 1000.0f) {
                if (animator.speed != m_LockSpeed) {
                    animator.speed = m_LockSpeed;
                    SetSkillTimeScale(instance, animator.speed, animator.speed, animator.speed);
                }
            } else {
                if (m_RestoreTime > 0 && Time.time < m_WorldStartTime + (m_LockTime + m_RestoreTime) / 1000.0f) {
                    float time_percent = (Time.time - m_WorldStartTime - m_LockTime / 1000.0f) / (m_RestoreTime / 1000.0f);
                    if (m_Curve != null && m_Curve.keys.Length > 0) {
                        float scale = m_Curve.Evaluate(time_percent);
                        float anim_speed = scale * m_AfterLockAnimSpeed;
                        float effect_speed = scale * m_AfterLockEffectSpeed;
                        float move_speed = scale * m_AfterLockMoveSpeed;
                        float skill_speed = scale * m_AfterLockSkillSpeed;
                        animator.speed = anim_speed;
                        SetSkillTimeScale(instance, effect_speed, move_speed, skill_speed);
                    } else {
                        float anim_speed = m_LockSpeed + time_percent * (m_AfterLockAnimSpeed - m_LockSpeed);
                        float effect_speed = m_LockSpeed + time_percent * (m_AfterLockEffectSpeed - m_LockSpeed);
                        float move_speed = m_LockSpeed + time_percent * (m_AfterLockMoveSpeed - m_LockSpeed);
                        float skill_speed = m_LockSpeed + time_percent * (m_AfterLockSkillSpeed - m_LockSpeed);
                        SetSkillTimeScale(instance, effect_speed, move_speed, skill_speed);
                    }
                } else {
                    animator.speed = m_AfterLockAnimSpeed;
                    SetSkillTimeScale(instance, m_AfterLockEffectSpeed, m_AfterLockMoveSpeed, m_AfterLockSkillSpeed);
                    return false;
                }
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            int index = 0;
            if (num >= 6) {
                m_StartTime = long.Parse(callData.GetParamId(index++));
                m_NeedHit = bool.Parse(callData.GetParamId(index++));
                m_LockSpeed = float.Parse(callData.GetParamId(index++));
                m_LockTime = long.Parse(callData.GetParamId(index++));
                m_AfterLockAnimSpeed = float.Parse(callData.GetParamId(index++));
                m_RestoreTime = long.Parse(callData.GetParamId(index++));
            }
            if (num >= 10) {
                m_IsEffectSkillTime = bool.Parse(callData.GetParamId(index++));
                m_AfterLockEffectSpeed = float.Parse(callData.GetParamId(index++));
                m_AfterLockMoveSpeed = float.Parse(callData.GetParamId(index++));
                m_AfterLockSkillSpeed = float.Parse(callData.GetParamId(index++));
            }
            m_RealStartTime = m_StartTime;
        }

        protected override void Load(Dsl.FunctionData funcData, int dslSkillId)
        {
            Dsl.CallData callData = funcData.Call;
            if (null == callData) {
                return;
            }
            Load(callData, dslSkillId);
            LoadKeyFrames(funcData.Statements);
        }

        private void LoadKeyFrames(List<Dsl.ISyntaxComponent> statements)
        {
            m_Curve = new AnimationCurve();
            for (int i = 0; i < statements.Count; i++) {
                Dsl.CallData stCall = statements[i] as Dsl.CallData;
                if (stCall.GetId() == "keyframe") {
                    if (stCall.GetParamNum() >= 4) {
                        float time = float.Parse(stCall.GetParamId(0));
                        float value = float.Parse(stCall.GetParamId(1));
                        float inTangent = float.Parse(stCall.GetParamId(2));
                        float outTangent = float.Parse(stCall.GetParamId(3));
                        Keyframe keyframe = new Keyframe(time, value, inTangent, outTangent);
                        m_Curve.AddKey(keyframe);
                    }
                }
            }
        }

        private void SetSkillTimeScale(SkillInstance instance, float effect_speed, float move_speed, float skill_speed)
        {
            if (m_IsEffectSkillTime){
                instance.TimeScale = skill_speed;
                instance.EffectScale = effect_speed;
                instance.MoveScale = move_speed;
                EffectManager em = instance.CustomDatas.GetData<EffectManager>();
                if (em != null) {
                    em.SetParticleSpeed(effect_speed);
                }
            }
        }

        private void Init()
        {
            m_WorldStartTime = Time.time;
            m_IsInited = true;
        }

        private bool m_NeedHit = false;
        private float m_LockSpeed = 0;
        private long m_LockTime = 0;
        private float m_AfterLockAnimSpeed = 1;
        private float m_AfterLockEffectSpeed = 1;
        private float m_AfterLockMoveSpeed = 1;
        private float m_AfterLockSkillSpeed = 1;
        private long m_RestoreTime = 0;
        private bool m_IsEffectSkillTime = false;

        private bool m_IsInited = false;
        private float m_WorldStartTime = 0;
        private AnimationCurve m_Curve;

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
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < m_StartTime)
                return true;
            instance.SetLocalVariable("hitEffect", TriggerUtil.RefixEffectByConfig(m_HitEffect, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitEffectBone", TriggerUtil.RefixBoneByConfig(m_HitEffectBone, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitEffectStartTime", TriggerUtil.RefixEffectStartTimeByConfig(m_HitEffectStartTime, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitEffectDeleteTime", TriggerUtil.RefixEffectDeleteTimeByConfig(m_HitEffectDeleteTime, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitAnim", TriggerUtil.RefixAnimByConfig(m_HitAnim, instance.LocalVariables, senderObj.ConfigData));
            instance.SetLocalVariable("hitAnimTime", TriggerUtil.RefixAnimTimeByConfig(m_HitAnimTime, instance.LocalVariables, senderObj.ConfigData));
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
