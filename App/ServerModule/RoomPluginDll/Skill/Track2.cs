using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using GameFramework.Skill.Trigers;
using SkillSystem;
using ScriptRuntime;

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
        m_Effect = Vector3.Zero;
        m_Target = null;
        m_Lifetime = 0;
        m_IsHit = false;

        m_LastPos = Vector3.Zero;
        m_LastTime = 0.0f;
    }
    public bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
        Scene scene = instance.Context as Scene;
        if (null != scene) {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) {
                return false;
            }
            if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                return false;//track只能在impact或buff里使用
            }
            EntityInfo obj = senderObj.GfxObj;
            if (null != obj) {
                if (curSectionTime >= m_TriggerProxy.StartTime) {
                    if (!m_IsStarted) {
                        m_IsStarted = true;

                        Vector3 dest;
                        string trackBone = m_TrackBone.Get(instance);
                        m_Target = senderObj.TargetGfxObj;
                        if (null != m_Target) {
                            dest = m_Target.GetMovementStateInfo().GetPosition3D();
                        } else {
                            dest = obj.GetMovementStateInfo().GetPosition3D();
                            dest.Y += 1.5f;
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] trackbullet bone {2} can't find.", senderObj.SkillId, instance.DslSkillId, trackBone);
                        }
                        m_StartPos = scene.EntityController.GetImpactSenderPosition(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                        object speedObj;
                        if (instance.Variables.TryGetValue("emitSpeed", out speedObj)) {
                            m_Speed = (float)speedObj;
                        } else {
                            return false;
                        }
                        long duration = m_Duration.Get(instance);
                        m_Lifetime = duration / 1000.0f;
                        if (Geometry.DistanceSquare(m_StartPos.X, m_StartPos.Z, dest.X, dest.Z) > 0.01f) {
                            m_TargetPos = Geometry.FrontOfTarget(dest, m_StartPos, m_Speed * m_Lifetime);
                        } else {
                            Vector3 pos = obj.GetMovementStateInfo().GetPosition3D();
                            Vector3 faceDir = obj.GetMovementStateInfo().GetFaceDir3D();
                            m_TargetPos = pos + faceDir * m_Speed * m_Lifetime;
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
                            dir = Quaternion.Identity;
                        }
                        Vector3 scale;
                        object scaleObj;
                        if (instance.Variables.TryGetValue("emitScale", out scaleObj)) {
                            scale = (Vector3)scaleObj;
                        } else {
                            scale = Vector3.One;
                        }
                        Vector3 lookDir = dest - m_StartPos;
                        float radian = Geometry.GetYRadian(m_StartPos, dest);
                        m_ControlPos = m_StartPos + lookDir / 2;
                        string effectPath = SkillParamUtility.RefixResourceVariable("emitEffect", instance, senderObj.ConfigData.resources);
                        m_Effect = m_StartPos;
                    } else {
                        Vector3 dest;
                        if (null != m_Target) {
                            dest = m_Target.GetMovementStateInfo().GetPosition3D();
                        } else {
                            dest = obj.GetMovementStateInfo().GetPosition3D();
                            dest.Y += 1.5f;
                        }
                        dest = Geometry.FrontOfTarget(m_StartPos, dest, 0.1f);
                        m_Effect = Geometry.GetBezierPoint(m_StartPos, m_ControlPos, m_TargetPos, (curSectionTime - m_TriggerProxy.StartTime) / 1000.0f / m_Lifetime);
                        var pos = m_Effect;
                        if (!m_IsHit) {
                            float distSqr = float.MaxValue;
                            if (m_LastPos.LengthSquared() > Geometry.c_FloatPrecision) {
                                ScriptRuntime.Vector2 np;
                                ScriptRuntime.Vector2 targetPos = new ScriptRuntime.Vector2(dest.X, dest.Z);
                                ScriptRuntime.Vector2 lastPos = new ScriptRuntime.Vector2(m_LastPos.X, m_LastPos.Z);
                                distSqr = Geometry.PointToLineSegmentDistanceSquare(targetPos, lastPos, new ScriptRuntime.Vector2(pos.X, pos.Z), out np);
                            } else {
                                distSqr = (dest - pos).LengthSquared();
                            }
                            m_LastPos = pos;
                            if (distSqr <= m_BulletRadiusSquare) {
                                float curTime = TimeUtility.GetLocalMilliseconds() / 1000.0f;
                                float interval = m_DamageInterval.Get(instance) / 1000.0f;
                                if (m_LastTime + interval <= curTime) {
                                    m_LastTime = curTime;

                                    //m_HitEffectRotation = Quaternion.CreateFromAxisAngle(m_StartPos - dest);
                                    int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                                    Dictionary<string, object> args;
                                    TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                                    if (args.ContainsKey("hitEffectRotation"))
                                        args["hitEffectRotation"] = m_HitEffectRotation;
                                    else
                                        args.Add("hitEffectRotation", m_HitEffectRotation);
                                    scene.EntityController.TrackSendImpact(senderObj.ActorId, senderObj.SkillId, senderObj.Seq, impactId, args);
                                    //m_IsHit = true;
                                }
                            }
                        }
                        if (curSectionTime > m_TriggerProxy.StartTime + m_Lifetime * 1000) {
                            m_Effect = Vector3.Zero;
                            instance.StopCurSection();
                            return false;
                        }
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
        return false;
    }
    public void OnInitProperties()
    {
        m_TriggerProxy.AddProperty("TrackBone", () => { return m_TrackBone.EditableValue; }, (object val) => { m_TrackBone.EditableValue = val; });
        m_TriggerProxy.AddProperty("Duration", () => { return m_Duration.EditableValue; }, (object val) => { m_Duration.EditableValue = val; });
    }
    public void LoadCallData(Dsl.CallData callData, SkillInstance instance)
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

    public void LoadFuncData(Dsl.FunctionData funcData, SkillInstance instance)
    {
    }

    public void LoadStatementData(Dsl.StatementData statementData, SkillInstance instance)
    {
    }

    private SkillStringParam m_TrackBone = new SkillStringParam();
    private SkillLongParam m_Duration = new SkillLongParam();
    private SkillLongParam m_DamageInterval = new SkillLongParam();
    private float m_BulletRadius = 0.1f;
    private float m_BulletRadiusSquare = 0.01f;

    private Vector3 m_StartPos = Vector3.Zero;
    private Vector3 m_ControlPos = Vector3.Zero;
    private Vector3 m_TargetPos = Vector3.Zero;
    private Vector3 m_LastPos = Vector3.Zero;
    private float m_LastTime = 0.0f;

    private float m_Speed = 10f;
    private float m_Lifetime = 1.0f;
    private bool m_IsStarted = false;
    private Quaternion m_HitEffectRotation;
    private Vector3 m_Effect;
    private EntityInfo m_Target;
    private bool m_IsHit = false;

    SkillTriggerProxy m_TriggerProxy = null;
}
