using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using GameFramework.Skill.Trigers;
using SkillSystem;

public class TrackBulletTrigger : ISkillTriggerPlugin
{
    public void SetProxy(SkillTriggerProxy triggerProxy)
    {
        m_TriggerProxy = triggerProxy;
    }
    public ISkillTriggerPlugin Clone()
    {
        TrackBulletTrigger triger = new TrackBulletTrigger();

        triger.m_TrackBone.CopyFrom(m_TrackBone);
        triger.m_NoImpact = m_NoImpact;
        triger.m_Duration.CopyFrom(m_Duration);
        triger.m_NotMove = m_NotMove;
        triger.m_EffectPath.CopyFrom(m_EffectPath);
        triger.m_DeleteTime.CopyFrom(m_DeleteTime);
        return triger;
    }
    public void Reset()
    {
        m_IsStarted = false;
        m_Effect = null;
        m_BoneTransform = null;
        m_Lifetime = 0;
        m_IsHit = false;
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

                    //LogSystem.Warn("trackbullet start.");

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
                    dest = Utility.FrontOfTarget(m_StartPos, dest, 0.1f);
                    object speedObj;
                    if (instance.Variables.TryGetValue("emitSpeed", out speedObj)) {
                        m_Speed = (float)speedObj;
                    } else {
                        return false;
                    }
                    float duration = m_Duration.Get(instance);
                    if (duration > Geometry.c_FloatPrecision) {
                        float d = duration / 1000.0f;
                        m_Lifetime = d;
                        m_Speed = (dest - m_StartPos).magnitude / m_Lifetime;
                    } else {
                        m_Lifetime = 1.0f;
                        if (m_Speed > Geometry.c_FloatPrecision) {
                            m_Lifetime = (dest - m_StartPos).magnitude / m_Speed;
                        }
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
                        //LogSystem.Warn("trackbullet effect {0} {1}", effectPath, m_Lifetime);

                        BulletManager.Instance.AddBullet(m_Effect);
                        senderObj.TrackEffectObj = m_Effect;
                        TriggerUtil.SetObjVisible(m_Effect, true);
                        m_Effect.SetActive(false);
                        m_Effect.transform.position = m_StartPos;
                        m_Effect.transform.localRotation = q;
                        m_Effect.SetActive(true);

                        //LogSystem.Warn("trackbullet effect actived {0} {1} pos {2} {3} {4}", effectPath, m_Lifetime, m_StartPos.x, m_StartPos.y, m_StartPos.z);
                    } else {
                        if (string.IsNullOrEmpty(effectPath)) {
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] trackbullet effect is empty.", senderObj.SkillId, instance.DslSkillId);
                        } else {
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] trackbullet effect {2} can't find.", senderObj.SkillId, instance.DslSkillId, effectPath);
                        }
                    }
                } else if (null != m_Effect) {
                    if (!m_NotMove && !m_IsHit) {
                        Vector3 dest;
                        if (null != m_BoneTransform) {
                            dest = m_BoneTransform.position;
                        } else {
                            dest = obj.transform.position;
                            dest.y += 1.5f;
                        }
                        dest = Utility.FrontOfTarget(m_StartPos, dest, 0.1f);
                        //m_Effect.transform.position = Vector3.MoveTowards(m_Effect.transform.position, dest, m_RealSpeed * Time.deltaTime);
                        m_Effect.transform.position = Utility.GetBezierPoint(m_StartPos, m_ControlPos, dest, (curSectionTime - m_TriggerProxy.StartTime) / 1000.0f / m_Lifetime);
                        var pos = m_Effect.transform.position;
                        if (CheckCollide(senderObj, instance, obj)) {
                            return true;
                        }
                        //LogSystem.Warn("trackbullet effect move to {0} {1} {2}", pos.x, pos.y, pos.z);
                        if ((dest - m_Effect.transform.position).sqrMagnitude <= 0.01f) {
                            m_HitEffectRotation = Quaternion.LookRotation(m_StartPos - dest);
                            if (m_NoImpact) {
                                instance.SetVariable("hitEffectRotation", m_HitEffectRotation);
                            } else {
                                int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                                Dictionary<string, object> args;
                                TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                                if (args.ContainsKey("hitEffectRotation"))
                                    args["hitEffectRotation"] = m_HitEffectRotation;
                                else
                                    args.Add("hitEffectRotation", m_HitEffectRotation);
                                EntityController.Instance.TrackSendImpact(senderObj.ObjId, senderObj.SkillId, senderObj.Seq, impactId, args);

                                //LogSystem.Warn("trackbullet effect hit target {0} {1} {2}", pos.x, pos.y, pos.z);
                            }
                            m_IsHit = true;
                        }
                    }
                    BulletManager.Instance.UpdatePos(m_Effect);
                    if (CheckCollide(senderObj, instance, obj)) {
                        return true;
                    }
                    if (m_IsHit || curSectionTime > m_TriggerProxy.StartTime + m_Lifetime * 1000) {
                        m_Effect.SetActive(false);
                        ResourceSystem.Instance.RecycleObject(m_Effect);
                        BulletManager.Instance.RemoveBullet(m_Effect);
                        m_Effect = null;
                        instance.StopCurSection();
                        
                        //LogSystem.Warn("trackbullet effect finish.");
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
            m_NoImpact = callData.GetParamId(1) == "true";
        }
        if (num > 2) {
            m_TriggerProxy.StartTime = long.Parse(callData.GetParamId(2));
        }
        if (num > 3) {
            m_Duration.Set(callData.GetParam(3));
        }
        if (num > 4) {
            m_NotMove = callData.GetParamId(4) == "true";
        }
        if (num > 5) {
            m_EffectPath.Set(callData.GetParam(5));
        }
        if (num > 6) {
            m_DeleteTime.Set(callData.GetParam(6));
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

    private void ShowExplodeEffect(GameObject obj, GfxSkillSenderInfo senderObj, SkillInstance instance)
    {
        float deleteTime = m_DeleteTime.Get(instance);
        if (deleteTime <= 0) {
            LogSystem.Warn("[skill:{0} dsl skill id:{1}] explode effect deleteTime <= 0.", senderObj.SkillId, instance.DslSkillId);
            return;
        }
        string effectPath = m_EffectPath.Get(instance, senderObj.ConfigData.resources);
        GameObject effectObj = null;
        if (string.IsNullOrEmpty(effectPath)) {
            LogSystem.Warn("[skill:{0} dsl skill id:{1}] explode effect is empty.", senderObj.SkillId, instance.DslSkillId);
        } else {
            effectObj = ResourceSystem.Instance.NewObject(effectPath, deleteTime / 1000.0f) as GameObject;
            if (null == effectObj) {
                LogSystem.Warn("[skill:{0} dsl skill id:{1}] selfeffect effect {2} can't find.", senderObj.SkillId, instance.DslSkillId, effectPath);
            }
        }
        if (null != effectObj) {
            TriggerUtil.SetObjVisible(effectObj, true);
            effectObj.SetActive(false);
            if (null != obj.transform) {
                effectObj.transform.SetParent(obj.transform);
                effectObj.transform.localPosition = Vector3.zero;
                effectObj.transform.localScale = Vector3.one;
                effectObj.transform.rotation = m_HitEffectRotation;
                effectObj.transform.SetParent(null);

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
    }

    private bool CheckCollide(GfxSkillSenderInfo senderObj, SkillInstance instance, GameObject obj)
    {
        var other = BulletManager.Instance.GetCollideObject(m_Effect);
        if (null != other && !m_IsHit) {
            m_IsHit = true;
            m_HitEffectRotation = Quaternion.LookRotation(m_StartPos - obj.transform.position);
            ShowExplodeEffect(m_Effect, senderObj, instance);

            var pos1 = m_Effect.transform.position;
            var pos2 = other.transform.position;
            //LogSystem.Warn("trackbullet effect explode {0}({1} {2} {3}) with {4}({5} {6} {7})", m_Effect.name, pos1.x, pos1.y, pos1.z, other.name, pos2.x, pos2.y, pos2.z);
            return true;
        }
        return false;
    }

    private SkillStringParam m_TrackBone = new SkillStringParam();
    private bool m_NoImpact = false;
    private SkillLongParam m_Duration = new SkillLongParam();
    private bool m_NotMove = false;
    private SkillResourceParam m_EffectPath = new SkillResourceParam();
    private SkillLongParam m_DeleteTime = new SkillLongParam();

    private Vector3 m_StartPos = Vector3.zero;
    private Vector3 m_ControlPos = Vector3.zero;
    private float m_Speed = 10f;
    private float m_Lifetime = 1.0f;
    private bool m_IsStarted = false;
    private Quaternion m_HitEffectRotation;
    private GameObject m_Effect;
    private Transform m_BoneTransform;
    private bool m_IsHit = false;

    SkillTriggerProxy m_TriggerProxy = null;
}
