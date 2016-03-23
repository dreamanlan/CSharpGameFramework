using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// impact(starttime[,centerx,centery,centerz,relativeToTarget]);
    /// </summary>
    public class ImpactTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            ImpactTrigger copy = new ImpactTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_RelativeCenter = m_RelativeCenter;
            copy.m_RelativeToTarget = m_RelativeToTarget;
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
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
            int impactId = 0;
            if (targetType == (int)SkillTargetType.Self)
                impactId = senderObj.ConfigData.impactToSelf;
            else
                impactId = senderObj.ConfigData.impactToTarget;
            int senderId;
            int targetId;
            EntityController.Instance.CalcSenderAndTarget(senderObj, out senderId, out targetId);
            if (senderObj.ConfigData.aoeType != (int)SkillAoeType.Unknown) {
                float minDistSqr = float.MaxValue;
                TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    if (distSqr < minDistSqr) {
                        minDistSqr = distSqr;
                        targetId = objId;
                    }
                    return true;
                });
            }
            string hitEffect;
            string hitEffectBone;
            int hitEffectStartTime;
            int hitEffectDeleteTime;
            string hitAnim;
            int hitAnimTime;
            TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
            EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                m_StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num >= 5) {
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
            }
            m_RealStartTime = m_StartTime;
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// aoeimpact(start_time, center_x, center_y, center_z, relativeToTarget);
    /// </summary>
    internal class AoeImpactTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            AoeImpactTriger triger = new AoeImpactTriger();
            triger.m_StartTime = m_StartTime;
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
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
            if (null == obj) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = 0;
                if (targetType == (int)SkillTargetType.Self)
                    impactId = senderObj.ConfigData.impactToSelf;
                else
                    impactId = senderObj.ConfigData.impactToTarget;
                int senderId = 0;
                if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                    senderId = senderObj.ActorId;
                } else {
                    senderId = senderObj.TargetActorId;
                }
                int ct = 0;
                TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    string hitEffect;
                    string hitEffectBone;
                    int hitEffectStartTime;
                    int hitEffectDeleteTime;
                    string hitAnim;
                    int hitAnimTime;
                    TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                    EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, impactId, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
                    ++ct;
                    if (senderObj.ConfigData.maxAoeTargetCount <= 0 || ct < senderObj.ConfigData.maxAoeTargetCount) {
                        return true;
                    } else {
                        return false;
                    }
                });
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 5) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
            }
            m_RealStartTime = m_StartTime;
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// chainaoeimpact(start_time, center_x, center_y, center_z, relativeToTarget, duration, interval);
    /// </summary>
    internal class ChainAoeImpactTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            ChainAoeImpactTriger triger = new ChainAoeImpactTriger();
            triger.m_StartTime = m_StartTime;
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_DurationTime = m_DurationTime;
            triger.m_IntervalTime = m_IntervalTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_LastTime = 0;
            m_SortedTargets.Clear();
            m_Targets.Clear();
            m_CurTargetIndex = 0;
            m_SenderId = 0;
            m_ImpactId = 0;
            m_RealStartTime = m_StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (curSectionTime > m_RealStartTime + m_DurationTime) {
                return false;
            }
            if (m_LastTime + m_IntervalTime < curSectionTime) {
                m_LastTime = curSectionTime;

                int ct = m_Targets.Count;
                if (ct <= 0) {
                    int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                    if (targetType == (int)SkillTargetType.Self)
                        m_ImpactId = senderObj.ConfigData.impactToSelf;
                    else
                        m_ImpactId = senderObj.ConfigData.impactToTarget;
                    if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                        m_SenderId = senderObj.ActorId;
                    } else {
                        m_SenderId = senderObj.TargetActorId;
                    }
                    TriggerUtil.AoeQuery(senderObj, instance, m_SenderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                        m_SortedTargets.Add((int)(distSqr * c_MaxObjectId) * c_MaxObjectId + objId, objId);
                        return true;
                    });
                    var vals = m_SortedTargets.Values;
                    if (vals.Count > senderObj.ConfigData.maxAoeTargetCount) {
                        var enumerator = vals.GetEnumerator();
                        for (int ix = 0; ix < senderObj.ConfigData.maxAoeTargetCount; ++ix) {
                            enumerator.MoveNext();
                            m_Targets.Add(enumerator.Current);
                        }
                    } else {
                        m_Targets.AddRange(vals);
                    }
                    m_CurTargetIndex = 0;
                    ct = m_Targets.Count;
                }
                if (ct > 0 && m_CurTargetIndex < ct) {
                    string hitEffect;
                    string hitEffectBone;
                    int hitEffectStartTime;
                    int hitEffectDeleteTime;
                    string hitAnim;
                    int hitAnimTime;
                    TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                    EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, m_SenderId, m_Targets[m_CurTargetIndex], m_ImpactId, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
                    ++m_CurTargetIndex;
                } else {
                    return false;
                }
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 7) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime = long.Parse(callData.GetParamId(5));
                m_IntervalTime = long.Parse(callData.GetParamId(6));
            }
            m_RealStartTime = m_StartTime;
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        private long m_DurationTime = 0;
        private long m_IntervalTime = 0;

        private long m_RealStartTime = 0;

        private long m_LastTime = 0;
        private int m_SenderId = 0;
        private int m_ImpactId = 0;
        private SortedDictionary<int, int> m_SortedTargets = new SortedDictionary<int, int>();
        private List<int> m_Targets = new List<int>();
        private int m_CurTargetIndex = 0;

        private const int c_MaxObjectId = 1000;
    }
    /// <summary>
    /// periodicallyimpact(starttime, center_x, center_y, center_z, relativeToTarget, duration, interval);
    /// </summary>
    public class PeriodicallyImpactTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            PeriodicallyImpactTrigger copy = new PeriodicallyImpactTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_RelativeCenter = m_RelativeCenter;
            copy.m_RelativeToTarget = m_RelativeToTarget;
            copy.m_DurationTime = m_DurationTime;
            copy.m_IntervalTime = m_IntervalTime;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_LastTime = 0;
            m_RealStartTime = m_StartTime;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            long durationTime = m_DurationTime;
            long intervalTime = m_IntervalTime;
            if (durationTime <= 0) {
                durationTime = (long)(senderObj.ConfigData.duration * 1000.0f);
            }
            if (intervalTime <= 0) {
                intervalTime = (long)(senderObj.ConfigData.interval * 1000.0f);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (curSectionTime > m_RealStartTime + durationTime) {
                return false;
            }
            if (m_LastTime + intervalTime < curSectionTime) {
                m_LastTime = curSectionTime;

                int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = 0;
                if (targetType == (int)SkillTargetType.Self)
                    impactId = senderObj.ConfigData.impactToSelf;
                else
                    impactId = senderObj.ConfigData.impactToTarget;
                int senderId;
                int targetId;
                EntityController.Instance.CalcSenderAndTarget(senderObj, out senderId, out targetId);
                if (senderObj.ConfigData.aoeType != (int)SkillAoeType.Unknown) {
                    float minDistSqr = float.MaxValue;
                    TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                        if (distSqr < minDistSqr) {
                            minDistSqr = distSqr;
                            targetId = objId;
                        }
                        return true;
                    });
                }
                string hitEffect;
                string hitEffectBone;
                int hitEffectStartTime;
                int hitEffectDeleteTime;
                string hitAnim;
                int hitAnimTime;
                TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 7) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime = long.Parse(callData.GetParamId(5));
                m_IntervalTime = long.Parse(callData.GetParamId(6));
            }
            m_RealStartTime = m_StartTime;
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        private long m_DurationTime = 0;
        private long m_IntervalTime = 0;

        private long m_LastTime = 0;
        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// periodicallyaoeimpact(start_time, center_x, center_y, center_z, relativeToTarget, duration, interval);
    /// </summary>
    internal class PeriodicallyAoeImpactTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            PeriodicallyAoeImpactTriger triger = new PeriodicallyAoeImpactTriger();
            triger.m_StartTime = m_StartTime;
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_DurationTime = m_DurationTime;
            triger.m_IntervalTime = m_IntervalTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_LastTime = 0;
            m_RealStartTime = m_StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (curSectionTime > m_RealStartTime + m_DurationTime) {
                return false;
            }
            if (m_LastTime + m_IntervalTime < curSectionTime) {
                m_LastTime = curSectionTime;

                int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = 0;
                if (targetType == (int)SkillTargetType.Self)
                    impactId = senderObj.ConfigData.impactToSelf;
                else
                    impactId = senderObj.ConfigData.impactToTarget;
                int senderId = 0;
                if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                    senderId = senderObj.ActorId;
                } else {
                    senderId = senderObj.TargetActorId;
                }
                int ct = 0;
                TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    string hitEffect;
                    string hitEffectBone;
                    int hitEffectStartTime;
                    int hitEffectDeleteTime;
                    string hitAnim;
                    int hitAnimTime;
                    TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                    EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, impactId, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
                    ++ct;
                    if (senderObj.ConfigData.maxAoeTargetCount <= 0 || ct < senderObj.ConfigData.maxAoeTargetCount) {
                        return true;
                    } else {
                        return false;
                    }
                });
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 6) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime = long.Parse(callData.GetParamId(5));
                m_IntervalTime = long.Parse(callData.GetParamId(6));
            }
            m_RealStartTime = m_StartTime;
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        private long m_DurationTime = 0;
        private long m_IntervalTime = 0;
        private long m_LastTime = 0;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// track(speed[, start_time]);
    /// </summary>
    internal class TrackTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            TrackTriger triger = new TrackTriger();
            triger.m_Speed = m_Speed;
            triger.m_StartTime = m_StartTime;
            triger.m_RealStartTime = m_RealStartTime;
            triger.m_RealSpeed = m_RealSpeed;
            return triger;
        }
        public override void Reset()
        {
            m_IsStarted = false;
            m_Effect = null;
            m_BoneTransform = null;
            m_RealStartTime = m_StartTime;
            m_RealSpeed = m_Speed;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) {
                return false;
            }
            if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                return false;//track只能在impact或buff里使用
            }
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {

                    if (!m_IsStarted) {
                        m_IsStarted = true;

                        Vector3 dest;
                        string trackBone = TriggerUtil.RefixStringVariable("hitEffectBone", instance.LocalVariables, senderObj.ConfigData);
                        m_BoneTransform = Utility.FindChildRecursive(obj.transform, trackBone);
                        if (null != m_BoneTransform) {
                            dest = m_BoneTransform.position;
                        } else {
                            dest = obj.transform.position;
                            dest.y += 1.5f;
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] track bone {2} can't find.", senderObj.SkillId, instance.DslSkillId, trackBone);
                        }

                        Vector3 pos = EntityController.Instance.GetImpactSenderPosition(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                        dest = Utility.FrontOfTarget(pos, dest, 0.3f);

                        if (m_RealSpeed < Geometry.c_FloatPrecision) {
                            object speedObj;
                            if (instance.LocalVariables.TryGetValue("emitSpeed", out speedObj)) {
                                m_RealSpeed = (float)speedObj;
                            }
                        }
                        float lifetime = 1.0f;
                        if (m_RealSpeed >= Geometry.c_FloatPrecision) {
                            lifetime = (dest - pos).magnitude / m_RealSpeed;
                        }

                        string effectPath = TriggerUtil.RefixResourceByConfig("emitEffect", instance.LocalVariables, senderObj.ConfigData);
                        m_Effect = ResourceSystem.Instance.NewObject(effectPath, lifetime) as GameObject;
                        if (null != m_Effect) {
                            TriggerUtil.SetObjVisible(m_Effect, true);
                            m_Effect.SetActive(false);
                            m_Effect.transform.position = pos;
                            m_Effect.transform.localRotation = Quaternion.LookRotation(dest - pos);
                            m_Effect.SetActive(true);
                        } else {
                            if (string.IsNullOrEmpty(effectPath)) {
                                LogSystem.Warn("[skill:{0} dsl skill id:{1}] track effect is empty.", senderObj.SkillId, instance.DslSkillId);
                            } else {
                                LogSystem.Warn("[skill:{0} dsl skill id:{1}] track effect {2} can't find.", senderObj.SkillId, instance.DslSkillId, effectPath);
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
                        Vector3 pos = EntityController.Instance.GetImpactSenderPosition(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                        dest = Utility.FrontOfTarget(pos, dest, 0.1f);
                        m_Effect.transform.position = Vector3.MoveTowards(m_Effect.transform.position, dest, m_RealSpeed * Time.deltaTime);

                        if ((dest - m_Effect.transform.position).sqrMagnitude <= 0.01f) {
                            string trackBone = TriggerUtil.RefixStringVariable("hitEffectBone", instance.LocalVariables, senderObj.ConfigData);
                            m_HitEffectRotation = Quaternion.LookRotation(pos - dest);

                            string hitEffect;
                            string hitEffectBone;
                            int hitEffectStartTime;
                            int hitEffectDeleteTime;
                            string hitAnim;
                            int hitAnimTime;
                            TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                            EntityController.Instance.TrackSendImpact(senderObj.ActorId, senderObj.SkillId, senderObj.Seq, hitEffect, trackBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime, m_HitEffectRotation);

                            m_Effect.SetActive(false);
                            ResourceSystem.Instance.RecycleObject(m_Effect);
                            m_Effect = null;

                            instance.StopCurSection();
                            return false;
                        }
                    } else {
                        return false;
                    }

                    //GameFramework.LogSystem.Debug("EmitEffectTriger:{0}", m_EffectPath);
                    return true;
                } else {
                    return true;
                }
            } else {
                instance.StopCurSection();
                return false;
            }
        }
        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Speed = float.Parse(callData.GetParamId(0));
            }
            if (num > 1) {
                m_StartTime = long.Parse(callData.GetParamId(1));
            }
            m_RealStartTime = m_StartTime;
            m_RealSpeed = m_Speed;
        }

        private float m_Speed = 10f;

        private long m_RealStartTime = 0;
        private float m_RealSpeed = 10f;

        private bool m_IsStarted = false;
        private Quaternion m_HitEffectRotation;
        private GameObject m_Effect;
        private Transform m_BoneTransform;
    }
    /// <summary>
    /// colliderimpact(start_time, center_x, center_y, center_z, duration[, finishOnCollide, singleHit]);
    /// </summary>
    internal class ColliderImpactTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            ColliderImpactTriger triger = new ColliderImpactTriger();
            triger.m_StartTime = m_StartTime;
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_DurationTime = m_DurationTime;
            triger.m_FinishOnCollide = m_FinishOnCollide;
            triger.m_SingleHit = m_SingleHit;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_IsStarted = false;
            m_LastPos = Vector3.zero;
            m_RealStartTime = m_StartTime;
            m_Targets.Clear();
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (m_RealStartTime + m_DurationTime < curSectionTime) {
                instance.StopCurSection();
                return false;
            }
            int impactId = 0;
            int senderId = 0;
            int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
            if (targetType == (int)SkillTargetType.Self)
                impactId = senderObj.ConfigData.impactToSelf;
            else
                impactId = senderObj.ConfigData.impactToTarget;
            if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                senderId = senderObj.ActorId;
            } else {
                senderId = senderObj.TargetActorId;
            }

            float range = 0;
            TableConfig.Skill cfg = senderObj.ConfigData;
            if (null != cfg) {
                range = cfg.aoeSize;
            }
            float angle = Utility.DegreeToRadian(obj.transform.localRotation.eulerAngles.y);
            Vector3 center = obj.transform.TransformPoint(m_RelativeCenter);
            if (!m_IsStarted) {
                m_IsStarted = true;
                m_LastPos = center;
            } else if ((center - m_LastPos).sqrMagnitude >= 0.25f) {
                Vector3 c = (m_LastPos + center) / 2;
                Vector3 angleu = center - m_LastPos;
                float queryRadius = range + angleu.magnitude / 2;

                int ct = 0;
                bool isCollide = false;
                ClientModule.Instance.KdTree.Query(c.x, c.y, c.z, queryRadius, (float distSqr, KdTreeObject kdTreeObj) => {
                    int targetId = kdTreeObj.Object.GetId();
                    if (targetType == (int)SkillTargetType.Enemy && CharacterRelation.RELATION_ENEMY == EntityController.Instance.GetRelation(senderId, targetId) ||
                        targetType == (int)SkillTargetType.Friend && CharacterRelation.RELATION_FRIEND == EntityController.Instance.GetRelation(senderId, targetId)) {
                        bool isMatch = Geometry.IsCapsuleDiskIntersect(new ScriptRuntime.Vector2(center.x, center.z), new ScriptRuntime.Vector2(angleu.x, angleu.z), range, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
                        if (isMatch) {
                            isCollide = true;
                            if (!m_Targets.Contains(targetId)) {
                                m_Targets.Add(targetId);

                                string hitEffect;
                                string hitEffectBone;
                                int hitEffectStartTime;
                                int hitEffectDeleteTime;
                                string hitAnim;
                                int hitAnimTime;
                                TriggerUtil.CalcHitConfig(instance.LocalVariables, senderObj.ConfigData, out hitEffect, out hitEffectBone, out hitEffectStartTime, out hitEffectDeleteTime, out hitAnim, out hitAnimTime);
                                EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, hitEffect, hitEffectBone, hitEffectStartTime, hitEffectDeleteTime, hitAnim, hitAnimTime);
                                if (m_SingleHit) {
                                    return false;
                                }
                                ++ct;
                                if (senderObj.ConfigData.maxAoeTargetCount <= 0 || ct < senderObj.ConfigData.maxAoeTargetCount) {
                                    return true;
                                } else {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                });
                if (isCollide && m_FinishOnCollide) {
                    return false;
                }
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 5) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_DurationTime = long.Parse(callData.GetParamId(4));
            }
            if (num >= 6) {
                m_FinishOnCollide = callData.GetParamId(5) == "true";
            }
            if (num >= 7) {
                m_SingleHit = callData.GetParamId(6) == "true";
            }
            m_RealStartTime = m_StartTime;
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private long m_DurationTime = 1000;
        private bool m_FinishOnCollide = false;
        private bool m_SingleHit = false;

        private long m_RealStartTime = 0;

        private bool m_IsStarted = false;
        private Vector3 m_LastPos = Vector3.zero;
        private HashSet<int> m_Targets = new HashSet<int>();
    }
}