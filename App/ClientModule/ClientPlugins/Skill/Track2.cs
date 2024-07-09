using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework.Plugin;
using ScriptableFramework.Skill;
using ScriptableFramework.Skill.Trigers;
using DotnetSkillScript;

public class Track2Trigger : ISkillTriggerPlugin
{
    public void SetProxy(SkillTriggerProxy triggerProxy)
    {
        m_TriggerProxy = triggerProxy;
    }
    public ISkillTriggerPlugin Clone()
    {
        Track2Trigger triger = new Track2Trigger();
        triger.m_TrackBone.CopyFrom(m_TrackBone);
        triger.m_Duration.CopyFrom(m_Duration);
        triger.m_DamageInterval.CopyFrom(m_DamageInterval);
        triger.m_BulletRadius = m_BulletRadius;
        triger.m_BulletRadiusSquare = m_BulletRadiusSquare;
        return triger;
    }
    public void Reset()
    {
        m_IsStarted = false;
        m_Effect = null;
        m_BoneTransform = null;
        m_Lifetime = 0;
        m_IsHit = false;

        m_LastPos = Vector3.zero;
        m_LastTime = 0.0f;
    }
    public bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
        GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
        if (null == senderObj) {
            return false;
        }
        if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
            return false;//track can only be used in impact or buff
        }
        GameObject obj = senderObj.GfxObj;
        if (null != obj) {
            if (curSectionTime >= m_TriggerProxy.StartTime) {
                if (!m_IsStarted) {
                    m_IsStarted = true;

                    Vector3 dest;
                    string trackBone = m_TrackBone.Get(instance);
                    m_BoneTransform = Utility.FindChildRecursive(obj.transform, trackBone);
                    if (null != m_BoneTransform) {
                        dest = m_BoneTransform.position;
                    } else {
                        dest = obj.transform.position;
                        dest.y += 1.5f;
                        LogSystem.Warn("[skill:{0} dsl skill id:{1}] trackbullet bone {2} can't find.", senderObj.SkillId, instance.DslSkillId, trackBone);
                    }
                    m_StartPos = EntityController.Instance.GetImpactSenderPosition(senderObj.ObjId, senderObj.SkillId, senderObj.Seq);
                    object speedObj;
                    if (instance.Variables.TryGetValue("emitSpeed", out speedObj)) {
                        m_Speed = (float)speedObj;
                    } else {
                        return false;
                    }
                    long duration = m_Duration.Get(instance);
                    m_Lifetime = duration / 1000.0f;
                    if (Geometry.DistanceSquare(m_StartPos.x, m_StartPos.z, dest.x, dest.z) > 0.01f) {
                        m_TargetPos = Utility.FrontOfTarget(dest, m_StartPos, m_Speed * m_Lifetime);
                    } else {
                        m_TargetPos = obj.transform.TransformPoint(0, 0, m_Speed * m_Lifetime);
                    }

                    long newSectionDuration = m_TriggerProxy.StartTime + (long)(m_Lifetime * 1000);
                    if (instance.CurSectionDuration < newSectionDuration) {
                        instance.SetCurSectionDuration(newSectionDuration);
                    }
                    Quaternion dir;
                    object dirObj;
                    if (instance.Variables.TryGetValue("emitDir", out dirObj)) {
                        dir = (Quaternion)dirObj;
                    } else {
                        dir = Quaternion.identity;
                    }
                    Vector3 scale;
                    object scaleObj;
                    if (instance.Variables.TryGetValue("emitScale", out scaleObj)) {
                        scale = (Vector3)scaleObj;
                    } else {
                        scale = Vector3.one;
                    }
                    Vector3 lookDir = dest - m_StartPos;
                    Quaternion q = Quaternion.LookRotation(lookDir);
                    m_ControlPos = m_StartPos + Vector3.Scale(q * dir * Vector3.forward, scale * lookDir.magnitude * 0.5f);
                    string effectPath = SkillParamUtility.RefixResourceVariable("emitEffect", instance, senderObj.ConfigData.resources);
                    m_Effect = ResourceSystem.Instance.NewObject(effectPath, m_Lifetime) as GameObject;
                    if (null != m_Effect) {
                        senderObj.TrackEffectObj = m_Effect;
                        TriggerUtil.SetObjVisible(m_Effect, true);
                        m_Effect.SetActive(false);
                        m_Effect.transform.position = m_StartPos;
                        m_Effect.transform.localRotation = q;
                        m_Effect.SetActive(true);

                        EffectManager em = instance.CustomDatas.GetData<EffectManager>();
                        if (em == null) {
                            em = new EffectManager();
                            instance.CustomDatas.AddData<EffectManager>(em);
                        }
                        em.AddEffect(m_Effect);
                        em.SetParticleSpeed(instance.EffectScale);
                    } else {
                        if (string.IsNullOrEmpty(effectPath)) {
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] trackbullet effect is empty.", senderObj.SkillId, instance.DslSkillId);
                        } else {
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] trackbullet effect {2} can't find.", senderObj.SkillId, instance.DslSkillId, effectPath);
                        }
                    }
                } else if (null != m_Effect) {
                    Vector3 dest;
                    if (null != m_BoneTransform) {
                        dest = m_BoneTransform.position;
                    } else {
                        dest = obj.transform.position;
                        dest.y += 1.5f;
                    }
                    dest = Utility.FrontOfTarget(m_StartPos, dest, 0.1f);
                    //m_Effect.transform.position = Vector3.MoveTowards(m_Effect.transform.position, m_TargetPos, m_RealSpeed * Time.deltaTime);
                    m_Effect.transform.position = Utility.GetBezierPoint(m_StartPos, m_ControlPos, m_TargetPos, (curSectionTime - m_TriggerProxy.StartTime) / 1000.0f / m_Lifetime);
                    var pos = m_Effect.transform.position;
                    if (!m_IsHit) {
                        float distSqr = float.MaxValue;
                        if (m_LastPos.sqrMagnitude > Geometry.c_FloatPrecision) {
                            ScriptRuntime.Vector2 np;
                            ScriptRuntime.Vector2 targetPos = new ScriptRuntime.Vector2(dest.x, dest.z);
                            ScriptRuntime.Vector2 lastPos = new ScriptRuntime.Vector2(m_LastPos.x, m_LastPos.z);
                            distSqr = Geometry.PointToLineSegmentDistanceSquare(targetPos, lastPos, new ScriptRuntime.Vector2(pos.x, pos.z), out np);
                        } else {
                            distSqr = (dest - pos).sqrMagnitude;
                        }
                        m_LastPos = pos;
                        if (distSqr <= m_BulletRadiusSquare) {
                            float curTime = Time.time;
                            float interval = m_DamageInterval.Get(instance) / 1000.0f;
                            if (m_LastTime + interval <= curTime) {
                                m_LastTime = curTime;

                                m_HitEffectRotation = Quaternion.LookRotation(m_StartPos - dest);
                                int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                                Dictionary<string, object> args;
                                TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                                if (args.ContainsKey("hitEffectRotation"))
                                    args["hitEffectRotation"] = m_HitEffectRotation;
                                else
                                    args.Add("hitEffectRotation", m_HitEffectRotation);
                                EntityController.Instance.TrackSendImpact(senderObj.ObjId, senderObj.SkillId, senderObj.Seq, impactId, args);
                                //m_IsHit = true;
                            }
                        }
                    }
                    if (curSectionTime > m_TriggerProxy.StartTime + m_Lifetime * 1000) {
                        m_Effect.SetActive(false);
                        ResourceSystem.Instance.RecycleObject(m_Effect);
                        m_Effect = null;
                        instance.StopCurSection();
                        return false;
                    }
                } else {
                    return false;
                }
                return true;
            } else {
                return true;
            }
        } else {
            instance.StopCurSection();
            return false;
        }
    }
    public void OnInitProperties()
    {
        m_TriggerProxy.AddProperty("TrackBone", () => { return m_TrackBone.EditableValue; }, (object val) => { m_TrackBone.EditableValue = val; });
        m_TriggerProxy.AddProperty("Duration", () => { return m_Duration.EditableValue; }, (object val) => { m_Duration.EditableValue = val; });
    }
    public void LoadCallData(Dsl.FunctionData callData, SkillInstance instance)
    {
        int num = callData.GetParamNum();
        if (num > 0) {
            m_TrackBone.Set(callData.GetParam(0));
        }
        if (num > 1) {
            m_Duration.Set(callData.GetParam(1));
        }
        if (num > 2) {
            m_DamageInterval.Set(callData.GetParam(2));
        }
        if (num > 3) {
            m_BulletRadius = float.Parse(callData.GetParamId(3));
            m_BulletRadiusSquare = m_BulletRadius * m_BulletRadius;
        }
        if (num > 4) {
            m_TriggerProxy.StartTime = long.Parse(callData.GetParamId(4));
        }
    }

    [Cs2Dsl.Ignore]
    public void LoadFuncData(Dsl.FunctionData funcData, SkillInstance instance)
    {
    }

    [Cs2Dsl.Ignore]
    public void LoadStatementData(Dsl.StatementData statementData, SkillInstance instance)
    {
    }

    private SkillStringParam m_TrackBone = new SkillStringParam();
    private SkillLongParam m_Duration = new SkillLongParam();
    private SkillLongParam m_DamageInterval = new SkillLongParam();
    private float m_BulletRadius = 0.1f;
    private float m_BulletRadiusSquare = 0.01f;

    private Vector3 m_StartPos = Vector3.zero;
    private Vector3 m_ControlPos = Vector3.zero;
    private Vector3 m_TargetPos = Vector3.zero;
    private Vector3 m_LastPos = Vector3.zero;
    private float m_LastTime = 0.0f;

    private float m_Speed = 10f;
    private float m_Lifetime = 1.0f;
    private bool m_IsStarted = false;
    private Quaternion m_HitEffectRotation;
    private GameObject m_Effect;
    private Transform m_BoneTransform;
    private bool m_IsHit = false;

    SkillTriggerProxy m_TriggerProxy = null;
}
