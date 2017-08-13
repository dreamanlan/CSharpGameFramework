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
                    ParticleSystem[] pss = m_EffectObject[i].GetComponentsInChildren<ParticleSystem>();
                    for (int j = 0; j < pss.Length; j++) {
                        var main = pss[j].main;
                        main.simulationSpeed = speed;
                    }
                }
            }
        }
        public void PauseEffects(bool pause)
        {
            for (int i = 0; i < m_EffectObject.Count; i++) {
                if (m_EffectObject[i].activeSelf) {
                    ParticleSystem[] pss = m_EffectObject[i].GetComponentsInChildren<ParticleSystem>();
                    for (int j = 0; j < pss.Length; j++) {
                        if (pause)
                            pss[j].Pause();
                        else
                            pss[j].Play();
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
        protected override ISkillTriger OnClone()
        {
            SelfEffectTriger triger = new SelfEffectTriger();
            triger.m_EffectPath.CopyFrom(m_EffectPath);
            triger.m_AttachPath.CopyFrom(m_AttachPath);
            triger.m_DeleteTime.CopyFrom(m_DeleteTime);
            
            triger.m_IsAttach = m_IsAttach;
            triger.m_Pos = m_Pos;
            triger.m_Dir = m_Dir;
            triger.m_Scale = m_Scale;
            
            triger.m_RealDeleteTime = m_RealDeleteTime;
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
                    m_RealDeleteTime = m_DeleteTime.Get(instance);
                    if (m_RealDeleteTime <= 0) {
                        m_RealDeleteTime = EntityController.Instance.GetImpactDuration(senderObj.ObjId, senderObj.SkillId, senderObj.Seq);
                    }
                    if (m_RealDeleteTime <= 0) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] selfeffect deleteTime <= 0.", senderObj.SkillId, instance.DslSkillId);
                        return false;
                    }
                    string effectPath = m_EffectPath.Get(instance, senderObj.ConfigData.resources);
                    string attachPath = m_AttachPath.Get(instance);
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
                        Transform bone = Utility.FindChildRecursive(obj.transform, attachPath);
                        if (null == bone) {
                            bone = obj.transform;
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] selfeffect bone {2} can't find.", senderObj.SkillId, instance.DslSkillId, attachPath);
                        }
                        effectObj.SetActive(false);
                        if (null != bone) {
                            effectObj.transform.SetParent(bone);
                            effectObj.transform.localPosition = m_Pos;
                            object effectRotation;
                            if (instance.Variables.TryGetValue("hitEffectRotation", out effectRotation)) {
                                effectObj.transform.SetParent(null);
                                effectObj.transform.rotation = (Quaternion)effectRotation;
                            } else {
                                effectObj.transform.localRotation = m_Dir;
                            }                            
                            effectObj.transform.localScale = m_Scale;
                            if (!m_IsAttach) {
                                effectObj.transform.SetParent(null);
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
        protected override void OnInitProperties()
        {
            AddProperty("EffectPath", () => { return m_EffectPath.EditableValue; }, (object val) => { m_EffectPath.EditableValue = val; });
            AddProperty("DeleteTime", () => { return m_DeleteTime.EditableValue; }, (object val) => { m_DeleteTime.EditableValue = val; });
            AddProperty("AttachPath", () => { return m_AttachPath.EditableValue; }, (object val) => { m_AttachPath.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_EffectPath.Set(callData.GetParam(0));
            }
            if (num > 1) {
                m_DeleteTime.Set(callData.GetParam(1));
            }
            if (num > 2) {
                m_AttachPath.Set(callData.GetParam(2));
            }
            if (num > 3) {
                StartTime = long.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                m_IsAttach = bool.Parse(callData.GetParamId(4));
            }
        }
        protected override void Load(Dsl.FunctionData funcData, SkillInstance instance)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, instance);
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
        private SkillResourceParam m_EffectPath = new SkillResourceParam();
        private SkillStringParam m_AttachPath = new SkillStringParam();
        private SkillNonStringParam<long> m_DeleteTime = new SkillNonStringParam<long>();
        private bool m_IsAttach = true;
        private Vector3 m_Pos = Vector3.zero;
        private Quaternion m_Dir = Quaternion.identity;
        private Vector3 m_Scale = Vector3.one;
        
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
        protected override ISkillTriger OnClone()
        {
            TargetEffectTriger triger = new TargetEffectTriger();
            triger.m_EffectPath.CopyFrom(m_EffectPath);
            triger.m_AttachPath.CopyFrom(m_AttachPath);
            triger.m_DeleteTime.CopyFrom(m_DeleteTime);
            
            triger.m_IsAttach = m_IsAttach;
            triger.m_Pos = m_Pos;
            triger.m_Dir = m_Dir;
            triger.m_Scale = m_Scale;
            
            triger.m_RealDeleteTime = m_RealDeleteTime;
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
                if (curSectionTime >= StartTime) {
                    GameObject target_obj = senderObj.TargetGfxObj;
                    if (null == target_obj) {
                        return false;
                    }
                    m_RealDeleteTime = m_DeleteTime.Get(instance);
                    if (m_RealDeleteTime <= 0) {
                        m_RealDeleteTime = EntityController.Instance.GetImpactDuration(senderObj.ObjId, senderObj.SkillId, senderObj.Seq);
                    }
                    if (m_RealDeleteTime <= 0) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] targeteffect deleteTime <= 0.", senderObj.SkillId, instance.DslSkillId);
                        return false;
                    }
                    string effectPath = m_EffectPath.Get(instance, senderObj.ConfigData.resources);
                    string attachPath = m_AttachPath.Get(instance);
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
                        Transform bone = Utility.FindChildRecursive(target_obj.transform, attachPath);
                        if (null == bone) {
                            bone = obj.transform;
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] targeteffect bone {2} can't find.", senderObj.SkillId, instance.DslSkillId, attachPath);
                        }
                        effectObj.SetActive(false);
                        if (null != bone) {
                            effectObj.transform.SetParent(bone);
                            effectObj.transform.localPosition = m_Pos;
                            effectObj.transform.localRotation = m_Dir;
                            effectObj.transform.localScale = m_Scale;
                            if (!m_IsAttach) {
                                effectObj.transform.SetParent(null);
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
        protected override void OnInitProperties()
        {
            AddProperty("EffectPath", () => { return m_EffectPath.EditableValue; }, (object val) => { m_EffectPath.EditableValue = val; });
            AddProperty("DeleteTime", () => { return m_DeleteTime.EditableValue; }, (object val) => { m_DeleteTime.EditableValue = val; });
            AddProperty("AttachPath", () => { return m_AttachPath.EditableValue; }, (object val) => { m_AttachPath.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_EffectPath.Set(callData.GetParam(0));
            }
            if (num > 1) {
                m_DeleteTime.Set(callData.GetParam(1));
            }
            if (num > 2) {
                m_AttachPath.Set(callData.GetParam(2));
            }
            if (num > 3) {
                StartTime = long.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                m_IsAttach = bool.Parse(callData.GetParamId(4));
            }
        }
        protected override void Load(Dsl.FunctionData funcData, SkillInstance instance)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, instance);
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
        private SkillResourceParam m_EffectPath = new SkillResourceParam();
        private SkillStringParam m_AttachPath = new SkillStringParam();
        private SkillNonStringParam<long> m_DeleteTime = new SkillNonStringParam<long>();
        private bool m_IsAttach = true;
        private Vector3 m_Pos = Vector3.zero;
        private Quaternion m_Dir = Quaternion.identity;
        private Vector3 m_Scale = Vector3.one;
        
        private long m_RealDeleteTime = 0;
    }
    /// <summary>
    /// sceneeffect(effect_path,delete_time[,vector3(x,y,z)[,start_time[,eular(rx,ry,rz)[,vector3(sx,sy,sz)[,isRotateRelativeUser]]]]]);
    /// </summary>
    internal class SceneEffectTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            SceneEffectTriger triger = new SceneEffectTriger();
            triger.m_EffectPath.CopyFrom(m_EffectPath);
            triger.m_DeleteTime.CopyFrom(m_DeleteTime);
            triger.m_Pos = m_Pos;
            triger.m_Dir = m_Dir;
            triger.m_Scale = m_Scale;
            
            triger.m_IsRotateRelativeUser = m_IsRotateRelativeUser;
            
            triger.m_RealDeleteTime = m_RealDeleteTime;
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
                    m_RealDeleteTime = m_DeleteTime.Get(instance);
                    if (m_RealDeleteTime <= 0) {
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] sceneeffect deleteTime <= 0.", senderObj.SkillId, instance.DslSkillId);
                        return false;
                    }
                    string effectPath = m_EffectPath.Get(instance, senderObj.ConfigData.resources);
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
                            effectObj.transform.SetParent(obj.transform);
                            effectObj.transform.localRotation = m_Dir;
                            effectObj.transform.SetParent(null);
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
        protected override void OnInitProperties()
        {
            AddProperty("EffectPath", () => { return m_EffectPath.EditableValue; }, (object val) => { m_EffectPath.EditableValue = val; });
            AddProperty("DeleteTime", () => { return m_DeleteTime.EditableValue; }, (object val) => { m_DeleteTime.EditableValue = val; });
            //AddProperty("Position", () => { return m_Pos; }, (object val) => { m_Pos = (Vector3)val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_EffectPath.Set(callData.GetParam(0));
            }
            if (num > 1) {
                m_DeleteTime.Set(callData.GetParam(1));
            }
            if (num > 2) {
                m_Pos = DslUtility.CalcVector3(callData.GetParam(2) as Dsl.CallData);
            }
            if (num > 3) {
                StartTime = long.Parse(callData.GetParamId(3));
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
        }
        private SkillResourceParam m_EffectPath = new SkillResourceParam();
        private SkillNonStringParam<long> m_DeleteTime = new SkillNonStringParam<long>();
        private Vector3 m_Pos = Vector3.zero;
        private Quaternion m_Dir = Quaternion.identity;
        private Vector3 m_Scale = Vector3.one;
        private bool m_IsRotateRelativeUser = false;
        
        private long m_RealDeleteTime = 0;
    }
    /// <summary>
    /// emiteffect(effect_path,emit_bone,emit_impact,emit_speed[,start_time[,is_external_impact]]);
    /// 
    /// or
    /// 
    /// emiteffect(effect_path,emit_bone,emit_impact,emit_speed[,start_time[,is_external_impact]])
    /// {
    ///   transform(vector3(0,1,0)[,eular(0,0,0)[,vector3(1,1,1)]]);
    /// };
    /// </summary>
    internal class EmitEffectTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            EmitEffectTriger triger = new EmitEffectTriger();
            triger.m_EffectPath.CopyFrom(m_EffectPath);
            triger.m_EmitBone.CopyFrom(m_EmitBone);
            triger.m_EmitSpeed.CopyFrom(m_EmitSpeed);
            triger.m_EmitImpact = m_EmitImpact;
            triger.m_IsExternalImpact = m_IsExternalImpact;
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
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (curSectionTime >= StartTime) {
                    int senderId;
                    int targetId;
                    EntityController.Instance.CalcSenderAndTarget(senderObj, out senderId, out targetId);
                    string effectPath = m_EffectPath.Get(instance, senderObj.ConfigData.resources);
                    string emitBone = m_EmitBone.Get(instance);
                    float emitSpeed = m_EmitSpeed.Get(instance);
                    if (!string.IsNullOrEmpty(effectPath)) {
                        GameObject target = EntityController.Instance.GetGameObject(targetId);
                        if (null != target) {
                            int emitImpact = m_EmitImpact;
                            if (!m_IsExternalImpact) {
                                emitImpact = SkillInstance.GenInnerEmitSkillId(m_EmitImpact);
                            }
                            int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                            Dictionary<string, object> args;
                            TriggerUtil.CalcImpactConfig(emitImpact, impactId, instance, senderObj.ConfigData, out args);
                            Dictionary<string, object> addArgs = new Dictionary<string, object>() { { "emitEffect", effectPath }, { "emitSpeed", emitSpeed }, { "emitDir", m_Dir }, { "emitScale", m_Scale } };
                            foreach (var pair in addArgs) {
                                if (args.ContainsKey(pair.Key)) {
                                    args[pair.Key] = pair.Value;
                                } else {
                                    args.Add(pair.Key, pair.Value);
                                }
                            }
                            EntityController.Instance.TrackImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ObjId, senderId, targetId, emitBone, emitImpact, m_Pos, IsFinal, args);
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
        protected override void OnInitProperties()
        {
            AddProperty("EffectPath", () => { return m_EffectPath.EditableValue; }, (object val) => { m_EffectPath.EditableValue = val; });
            AddProperty("EmitBone", () => { return m_EmitBone.EditableValue; }, (object val) => { m_EmitBone.EditableValue = val; });
            AddProperty("EmitSpeed", () => { return m_EmitSpeed.EditableValue; }, (object val) => { m_EmitSpeed.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_EffectPath.Set(callData.GetParam(0));
            }
            if (num > 1) {
                m_EmitBone.Set(callData.GetParam(1));
            }
            if (num > 2) {
                m_EmitImpact = int.Parse(callData.GetParamId(2));
            }
            if (num > 3) {
                m_EmitSpeed.Set(callData.GetParam(3));
            }
            if (num > 4) {
                StartTime = long.Parse(callData.GetParamId(4));
            }
            if (num > 5) {
                m_IsExternalImpact = callData.GetParamId(5) == "true";
            }
            instance.AddImpactForInit(this, m_EmitImpact, m_IsExternalImpact);
        }
        protected override void Load(Dsl.FunctionData funcData, SkillInstance instance)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, instance);
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

        private SkillResourceParam m_EffectPath = new SkillResourceParam();
        private SkillStringParam m_EmitBone = new SkillStringParam();
        private SkillNonStringParam<float> m_EmitSpeed = new SkillNonStringParam<float>();
        private int m_EmitImpact = 0;
        private bool m_IsExternalImpact = false;
        private Vector3 m_Pos = Vector3.zero;
        private Quaternion m_Dir = Quaternion.identity;
        private Vector3 m_Scale = Vector3.one;        
    }
    /// <summary>
    /// aoeemiteffect(effect_path,emit_bone,center_x,center_y,center_z,relativeToTarget,emit_impact,emit_speed[,start_time[,is_external_impact]]);
    /// 
    /// or
    /// 
    /// aoeemiteffect(effect_path,emit_bone,center_x,center_y,center_z,relativeToTarget,emit_impact,emit_speed[,start_time[,is_external_impact]])
    /// {
    ///   transform(vector3(0,1,0)[,eular(0,0,0)[,vector3(1,1,1)]]);
    /// };
    /// </summary>
    internal class AoeEmitEffectTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AoeEmitEffectTriger triger = new AoeEmitEffectTriger();
            triger.m_EffectPath.CopyFrom(m_EffectPath);
            triger.m_EmitBone.CopyFrom(m_EmitBone);
            triger.m_EmitSpeed.CopyFrom(m_EmitSpeed);
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_EmitImpact = m_EmitImpact;
            triger.m_IsExternalImpact = m_IsExternalImpact;
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
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (curSectionTime >= StartTime) {
                    int targetType = EntityController.Instance.GetTargetType(senderObj.ObjId, senderObj.ConfigData, senderObj.Seq);
                    int senderId = 0;
                    if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                        senderId = senderObj.ObjId;
                    } else {
                        senderId = senderObj.TargetObjId;
                    }
                    string effectPath = m_EffectPath.Get(instance, senderObj.ConfigData.resources);
                    string emitBone = m_EmitBone.Get(instance);
                    float emitSpeed = m_EmitSpeed.Get(instance);
                    int emitImpact = m_EmitImpact;
                    if (!m_IsExternalImpact) {
                        emitImpact = SkillInstance.GenInnerEmitSkillId(m_EmitImpact);
                    }
                    int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                    int ct = 0;
                    TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                        Dictionary<string, object> args;
                        TriggerUtil.CalcImpactConfig(emitImpact, impactId, instance, senderObj.ConfigData, out args);
                        Dictionary<string, object> addArgs = new Dictionary<string, object>() { { "emitEffect", effectPath }, { "emitSpeed", emitSpeed }, { "emitDir", m_Dir }, { "emitScale", m_Scale } };
                        foreach (var pair in addArgs) {
                            if (args.ContainsKey(pair.Key)) {
                                args[pair.Key] = pair.Value;
                            } else {
                                args.Add(pair.Key, pair.Value);
                            }
                        }
                        EntityController.Instance.TrackImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ObjId, senderId, objId, emitBone, emitImpact, m_Pos, IsFinal, args);
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
        protected override void OnInitProperties()
        {
            AddProperty("EffectPath", () => { return m_EffectPath.EditableValue; }, (object val) => { m_EffectPath.EditableValue = val; });
            AddProperty("EmitBone", () => { return m_EmitBone.EditableValue; }, (object val) => { m_EmitBone.EditableValue = val; });
            AddProperty("EmitSpeed", () => { return m_EmitSpeed.EditableValue; }, (object val) => { m_EmitSpeed.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_EffectPath.Set(callData.GetParam(0));
            }
            if (num > 1) {
                m_EmitBone.Set(callData.GetParamId(1));
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
                m_EmitSpeed.Set(callData.GetParam(7));
            }
            if (num > 8) {
                StartTime = long.Parse(callData.GetParamId(8));
            }
            if (num > 9) {
                m_IsExternalImpact = callData.GetParamId(9) == "true";
            }
            instance.AddImpactForInit(this, m_EmitImpact, m_IsExternalImpact);     
        }
        protected override void Load(Dsl.FunctionData funcData, SkillInstance instance)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, instance);
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
        private SkillResourceParam m_EffectPath = new SkillResourceParam();
        private SkillStringParam m_EmitBone = new SkillStringParam();
        private SkillNonStringParam<float> m_EmitSpeed = new SkillNonStringParam<float>();
        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        private int m_EmitImpact = 0;
        private bool m_IsExternalImpact = false;
        private Vector3 m_Pos = Vector3.zero;
        private Quaternion m_Dir = Quaternion.identity;
        private Vector3 m_Scale = Vector3.one;
        
    }
    /// <summary>
    /// lockframe(startime, lock_speed, locktime[,
    ///          after_lock_anim_speed, restore_time[, is_effect_skill_time,
    ///          after_lock_effect_speed, after_lock_move_speed,
    ///          after_lock_skill_speed]])
    /// {
    ///     keyframe(time, value, inTangent, outTangent);
    ///     ...
    /// };
    /// </summary>
    internal class LockFrameTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            LockFrameTriger triger = new LockFrameTriger();
            
            triger.m_LockSpeed = m_LockSpeed;
            triger.m_LockTime = m_LockTime;
            triger.m_AfterLockAnimSpeed = m_AfterLockAnimSpeed;
            triger.m_AfterLockEffectSpeed = m_AfterLockEffectSpeed;
            triger.m_AfterLockMoveSpeed = m_AfterLockMoveSpeed;
            triger.m_AfterLockSkillSpeed = m_AfterLockSkillSpeed;
            triger.m_RestoreTime = m_RestoreTime;
            triger.m_IsEffectSkillTime = m_IsEffectSkillTime;
            triger.m_Curve = m_Curve;
            
            return triger;
        }
        public override void Reset()
        {
            m_IsInited = false;
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj)
                return false;
            if (null != senderObj.TrackEffectObj)
                obj = senderObj.TrackEffectObj;
            if (curSectionTime < StartTime) {
                return true;
            }
            Animator animator = obj.GetComponentInChildren<Animator>();
            if (animator == null) {
                return false;
            }
            if (!m_IsInited) {
                Init();
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
                        animator.speed = anim_speed;
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
        protected override void OnInitProperties()
        {
            AddProperty("LockSpeed", () => { return m_LockSpeed; }, (object val) => { m_LockSpeed = (float)Convert.ChangeType(val, typeof(float)); });
            AddProperty("LockTime", () => { return m_LockTime; }, (object val) => { m_LockTime = (long)Convert.ChangeType(val, typeof(long)); });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            int index = 0;
            if (num >= 3) {
                StartTime = long.Parse(callData.GetParamId(index++));
                m_LockSpeed = float.Parse(callData.GetParamId(index++));
                m_LockTime = long.Parse(callData.GetParamId(index++));
            }
            if (num >= 5) {
                m_AfterLockAnimSpeed = float.Parse(callData.GetParamId(index++));
                m_RestoreTime = long.Parse(callData.GetParamId(index++));
            }
            if (num >= 9) {
                m_IsEffectSkillTime = bool.Parse(callData.GetParamId(index++));
                m_AfterLockEffectSpeed = float.Parse(callData.GetParamId(index++));
                m_AfterLockMoveSpeed = float.Parse(callData.GetParamId(index++));
                m_AfterLockSkillSpeed = float.Parse(callData.GetParamId(index++));
            }
            
        }
        protected override void Load(Dsl.FunctionData funcData, SkillInstance instance)
        {
            Dsl.CallData callData = funcData.Call;
            if (null == callData) {
                return;
            }
            Load(callData, instance);
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
            if (m_IsEffectSkillTime) {
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
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime)
                return true;
            instance.SetVariable("hitEffect", SkillParamUtility.RefixResourceVariable(m_HitEffect, instance, senderObj.ConfigData.resources));
            instance.SetVariable("hitEffectBone", m_HitEffectBone);
            instance.SetVariable("hitEffectStartTime", m_HitEffectStartTime);
            instance.SetVariable("hitEffectDeleteTime", m_HitEffectDeleteTime);
            instance.SetVariable("hitAnim", m_HitAnim);
            instance.SetVariable("hitAnimTime", m_HitAnimTime);
            instance.SetVariable("hitDelayTime", m_HitDelayTime);
            return false;
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
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
