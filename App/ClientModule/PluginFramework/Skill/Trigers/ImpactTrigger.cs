using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// bufftotarget(starttime);
    /// </summary>
    public class BuffToTargetTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            BuffToTargetTrigger copy = new BuffToTargetTrigger();
            return copy;
        }
        public override void Reset()
        {

        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            int senderId;
            int targetId;
            EntityController.Instance.CalcSenderAndTarget(senderObj, out senderId, out targetId);
            int impactId = senderObj.ConfigData.impact;
            if (senderObj.ConfigData.type != (int)SkillOrImpactType.Skill) {
                if (impactId <= 0) {
                    int skillId = EntityController.Instance.GetImpactSkillId(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                    TableConfig.Skill cfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                    if (null != cfg) {
                        impactId = cfg.impact;
                    }
                }
            }
            Dictionary<string, object> args;
            TriggerUtil.CalcImpactConfig(0, 0, instance, senderObj.ConfigData, out args);
            EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, true, args);
            return false;
        }
        protected override void OnInitProperties()
        {
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
        }
    }
    /// <summary>
    /// bufftoself(starttime[,centerx,centery,centerz,radius,angle_or_length,aoetype,maxCount,relativeToTarget]);
    /// </summary>
    public class BuffToSelfTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            BuffToSelfTrigger copy = new BuffToSelfTrigger();
            copy.m_RelativeCenter = m_RelativeCenter;
            copy.m_Radius = m_Radius;
            copy.m_AngleOrLength = m_AngleOrLength;
            copy.m_AoeType = m_AoeType;
            copy.m_MaxCount = m_MaxCount;
            copy.m_RelativeToTarget = m_RelativeToTarget;

            return copy;
        }
        public override void Reset()
        {

        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            int senderId;
            int targetId;
            EntityController.Instance.CalcSenderAndTarget(senderObj, out senderId, out targetId);
            int impactId = senderObj.ConfigData.impacttoself;
            if (senderObj.ConfigData.type != (int)SkillOrImpactType.Skill) {
                if (impactId <= 0) {
                    int skillId = EntityController.Instance.GetImpactSkillId(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                    TableConfig.Skill cfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                    if (null != cfg) {
                        impactId = cfg.impacttoself;
                    }
                }
            }
            if (m_Radius <= Geometry.c_FloatPrecision) {
                Dictionary<string, object> args;
                TriggerUtil.CalcImpactConfig(0, 0, instance, senderObj.ConfigData, out args);
                EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, senderId, impactId, true, args);
            } else {
                GameObject targetObj = senderObj.TargetGfxObj;
                int ct = 0;
                List<int> targetIds = new List<int>();
                TriggerUtil.AoeQuery(obj, targetObj, m_AoeType, m_Radius, m_AngleOrLength, instance, senderId, (int)SkillTargetType.Friend, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    Dictionary<string, object> args;
                    TriggerUtil.CalcImpactConfig(0, 0, instance, senderObj.ConfigData, out args);
                    EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, impactId, true, args);
                    targetIds.Add(objId);
                    ++ct;
                    if (m_MaxCount <= 0 || ct < m_MaxCount) {
                        return true;
                    } else {
                        return false;
                    }
                });
            }
            return false;
        }
        protected override void OnInitProperties()
        {
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num >= 9) {
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_Radius = float.Parse(callData.GetParamId(4));
                m_AngleOrLength = float.Parse(callData.GetParamId(5));
                m_AoeType = int.Parse(callData.GetParamId(6));
                m_MaxCount = int.Parse(callData.GetParamId(7));
                m_RelativeToTarget = callData.GetParamId(8) == "true";
            }
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private float m_Radius = 0;
        private float m_AngleOrLength = 0;
        private int m_AoeType = 0;
        private int m_MaxCount = 0;
        private bool m_RelativeToTarget = false;
    }
    /// <summary>
    /// impact(starttime[,centerx,centery,centerz,relativeToTarget]);
    /// </summary>
    public class ImpactTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ImpactTrigger copy = new ImpactTrigger();
            copy.m_RelativeCenter = m_RelativeCenter;
            copy.m_RelativeToTarget = m_RelativeToTarget;
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
            int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
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
            Dictionary<string, object> args;
            TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
            EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, IsFinal, args);
            return false;
        }
        protected override void OnInitProperties()
        {
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            instance.AddImpactForInit(this);
            int num = callData.GetParamNum();
            if (num >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num >= 5) {
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;        
    }
    /// <summary>
    /// aoeimpact(start_time, center_x, center_y, center_z, relativeToTarget);
    /// </summary>
    internal class AoeImpactTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AoeImpactTriger triger = new AoeImpactTriger();

            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            
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
            if (null == obj) {
                return false;
            }
            if (curSectionTime >= StartTime) {
                int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                int senderId = 0;
                if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                    senderId = senderObj.ActorId;
                } else {
                    senderId = senderObj.TargetActorId;
                }
                int ct = 0;
                List<int> targetIds = new List<int>();
                TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    Dictionary<string, object> args;
                    TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                    EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, impactId, IsFinal, args);
                    targetIds.Add(objId);
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
        protected override void OnInitProperties()
        {
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            instance.AddImpactForInit(this);
            int num = callData.GetParamNum();
            if (num >= 5) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        
    }
    /// <summary>
    /// chainaoeimpact(start_time, center_x, center_y, center_z, relativeToTarget, duration, interval);
    /// </summary>
    internal class ChainAoeImpactTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ChainAoeImpactTriger triger = new ChainAoeImpactTriger();

            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_DurationTime.CopyFrom(m_DurationTime);
            triger.m_IntervalTime.CopyFrom(m_IntervalTime);
            
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
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            long durationTime = m_DurationTime.Get(instance);
            long intervalTime = m_IntervalTime.Get(instance);
            if (curSectionTime > StartTime + durationTime) {
                return false;
            }
            if (m_LastTime + intervalTime < curSectionTime) {
                m_LastTime = curSectionTime;
                int ct = m_Targets.Count;
                if (ct <= 0) {
                    int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                    int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
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
                    if (senderObj.ConfigData.maxAoeTargetCount > 0 && vals.Count > senderObj.ConfigData.maxAoeTargetCount) {
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
                    Dictionary<string, object> args;
                    TriggerUtil.CalcImpactConfig(0, m_ImpactId, instance, senderObj.ConfigData, out args);
                    EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, m_SenderId, m_Targets[m_CurTargetIndex], m_ImpactId, IsFinal, args);
                    ++m_CurTargetIndex;
                } else {
                    return false;
                }
            }
            return true;
        }
        protected override void OnInitProperties()
        {
            AddProperty("Duration", () => { return m_DurationTime.EditableValue; }, (object val) => { m_DurationTime.EditableValue = val; });
            AddProperty("Interval", () => { return m_IntervalTime.EditableValue; }, (object val) => { m_IntervalTime.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            instance.AddImpactForInit(this);
            int num = callData.GetParamNum();
            if (num >= 7) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime.Set(callData.GetParam(5));
                m_IntervalTime.Set(callData.GetParam(6));
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        private SkillNonStringParam<long> m_DurationTime = new SkillNonStringParam<long>();
        private SkillNonStringParam<long> m_IntervalTime = new SkillNonStringParam<long>();
        
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
        protected override ISkillTriger OnClone()
        {
            PeriodicallyImpactTrigger copy = new PeriodicallyImpactTrigger();

            copy.m_RelativeCenter = m_RelativeCenter;
            copy.m_RelativeToTarget = m_RelativeToTarget;
            copy.m_DurationTime.CopyFrom(m_DurationTime);
            copy.m_IntervalTime.CopyFrom(m_IntervalTime);
            
            return copy;
        }
        public override void Reset()
        {
            m_LastTime = 0;
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            long durationTime = m_DurationTime.Get(instance);
            long intervalTime = m_IntervalTime.Get(instance);
            if (curSectionTime < StartTime) {
                return true;
            }
            if (curSectionTime > StartTime + durationTime) {
                return false;
            }
            if (m_LastTime + intervalTime < curSectionTime) {
                m_LastTime = curSectionTime;
                int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
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
                Dictionary<string, object> args;
                TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, IsFinal, args);
            }
            return true;
        }
        protected override void OnInitProperties()
        {
            AddProperty("Duration", () => { return m_DurationTime.EditableValue; }, (object val) => { m_DurationTime.EditableValue = val; });
            AddProperty("Interval", () => { return m_IntervalTime.EditableValue; }, (object val) => { m_IntervalTime.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            instance.AddImpactForInit(this);
            int num = callData.GetParamNum();
            if (num >= 7) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime.Set(callData.GetParam(5));
                m_IntervalTime.Set(callData.GetParam(6));
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        private SkillNonStringParam<long> m_DurationTime = new SkillNonStringParam<long>();
        private SkillNonStringParam<long> m_IntervalTime = new SkillNonStringParam<long>();
        private long m_LastTime = 0;
        
    }
    /// <summary>
    /// periodicallyaoeimpact(start_time, center_x, center_y, center_z, relativeToTarget, duration, interval);
    /// </summary>
    internal class PeriodicallyAoeImpactTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            PeriodicallyAoeImpactTriger triger = new PeriodicallyAoeImpactTriger();

            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_DurationTime.CopyFrom(m_DurationTime);
            triger.m_IntervalTime.CopyFrom(m_IntervalTime);
            
            return triger;
        }
        public override void Reset()
        {
            m_LastTime = 0;
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            long duration = m_DurationTime.Get(instance);
            long interval = m_IntervalTime.Get(instance);
            if (curSectionTime > StartTime + duration) {
                return false;
            }
            if (m_LastTime + interval < curSectionTime) {
                m_LastTime = curSectionTime;
                int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                int senderId = 0;
                if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                    senderId = senderObj.ActorId;
                } else {
                    senderId = senderObj.TargetActorId;
                }
                int ct = 0;
                List<int> targetIds = new List<int>();
                TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    Dictionary<string, object> args;
                    TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                    EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, impactId, IsFinal, args);
                    targetIds.Add(objId);
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
        protected override void OnInitProperties()
        {
            AddProperty("Duration", () => { return m_DurationTime.EditableValue; }, (object val) => { m_DurationTime.EditableValue = val; });
            AddProperty("Interval", () => { return m_IntervalTime.EditableValue; }, (object val) => { m_IntervalTime.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            instance.AddImpactForInit(this);
            int num = callData.GetParamNum();
            if (num >= 6) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime.Set(callData.GetParam(5));
                m_IntervalTime.Set(callData.GetParam(6));
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private bool m_RelativeToTarget = false;
        private SkillNonStringParam<long> m_DurationTime = new SkillNonStringParam<long>();
        private SkillNonStringParam<long> m_IntervalTime = new SkillNonStringParam<long>();
        private long m_LastTime = 0;
        
    }
    /// <summary>
    /// track(trackBone[,no_impact[,start_time[,duration[,not_move]]]]);
    /// </summary>
    internal class TrackTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            TrackTriger triger = new TrackTriger();

            triger.m_TrackBone.CopyFrom(m_TrackBone);
            triger.m_NoImpact = m_NoImpact;
            triger.m_Duration.CopyFrom(m_Duration);
            triger.m_NotMove = m_NotMove;
            return triger;
        }
        public override void Reset()
        {
            m_IsStarted = false;
            m_Effect = null;
            m_BoneTransform = null;
            m_Lifetime = 0;
            m_IsHit = false;
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
                if (curSectionTime >= StartTime) {
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
                            LogSystem.Warn("[skill:{0} dsl skill id:{1}] track bone {2} can't find.", senderObj.SkillId, instance.DslSkillId, trackBone);
                        }
                        m_StartPos = EntityController.Instance.GetImpactSenderPosition(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
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
                        long newSectionDuration = StartTime + (long)(m_Lifetime * 1000);
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
                        } else {
                            if (string.IsNullOrEmpty(effectPath)) {
                                LogSystem.Warn("[skill:{0} dsl skill id:{1}] track effect is empty.", senderObj.SkillId, instance.DslSkillId);
                            } else {
                                LogSystem.Warn("[skill:{0} dsl skill id:{1}] track effect {2} can't find.", senderObj.SkillId, instance.DslSkillId, effectPath);
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
                            m_Effect.transform.position = Utility.GetBezierPoint(m_StartPos, m_ControlPos, dest, (curSectionTime - StartTime) / 1000.0f / m_Lifetime);
                            if ((dest - m_Effect.transform.position).sqrMagnitude <= 0.01f) {
                                m_HitEffectRotation = Quaternion.LookRotation(m_StartPos - dest);
                                if (m_NoImpact) {
                                    instance.SetVariable("hitEffectRotation", m_HitEffectRotation);
                                } else {
                                    int impactId = EntityController.Instance.GetTrackSendImpact(senderObj.ActorId, senderObj.Seq, instance.Variables);
                                    Dictionary<string, object> args;
                                    TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                                    if (args.ContainsKey("hitEffectRotation"))
                                        args["hitEffectRotation"] = m_HitEffectRotation;
                                    else
                                        args.Add("hitEffectRotation", m_HitEffectRotation);
                                    EntityController.Instance.TrackSendImpact(senderObj.ActorId, senderObj.SkillId, senderObj.Seq, impactId, args);
                                    int senderId, targetId;
                                    EntityController.Instance.CalcSenderAndTarget(senderObj, out senderId, out targetId);
                                }
                                m_IsHit = true;
                            }
                        }
                        if (curSectionTime > StartTime + m_Lifetime * 1000) {
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
        protected override void OnInitProperties()
        {
            AddProperty("TrackBone", () => { return m_TrackBone.EditableValue; }, (object val) => { m_TrackBone.EditableValue = val; });
            AddProperty("Duration", () => { return m_Duration.EditableValue; }, (object val) => { m_Duration.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum(); 
            if (num > 0) {
                m_TrackBone.Set(callData.GetParam(0));
            }
            if (num > 1) {
                m_NoImpact = callData.GetParamId(1) == "true";
            }
            if (num > 2) {
                StartTime = long.Parse(callData.GetParamId(2));
            }
            if (num > 3) {
                m_Duration.Set(callData.GetParam(3));
            }            
            if (num > 4) {
                m_NotMove = callData.GetParamId(4) == "true";
            }            
        }

        private SkillStringParam m_TrackBone = new SkillStringParam();
        private bool m_NoImpact = false;
        private SkillNonStringParam<long> m_Duration = new SkillNonStringParam<long>();
        private bool m_NotMove = false;
        
        private Vector3 m_StartPos = Vector3.zero;
        private Vector3 m_ControlPos = Vector3.zero;
        private float m_Speed = 10f;
        private float m_Lifetime = 1.0f;
        private bool m_IsStarted = false;
        private Quaternion m_HitEffectRotation;
        private GameObject m_Effect;
        private Transform m_BoneTransform;
        private bool m_IsHit = false;
    }
    /// <summary>
    /// colliderimpact(start_time, center_x, center_y, center_z, duration[, finishOnCollide, singleHit]);
    /// </summary>
    internal class ColliderImpactTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ColliderImpactTriger triger = new ColliderImpactTriger();

            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_DurationTime.CopyFrom(m_DurationTime);
            triger.m_FinishOnCollide = m_FinishOnCollide;
            triger.m_SingleHit = m_SingleHit;
            
            return triger;
        }
        public override void Reset()
        {
            m_IsStarted = false;
            m_LastPos = Vector3.zero;
            
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
            if (null != senderObj.TrackEffectObj)
                obj = senderObj.TrackEffectObj;
            if (curSectionTime < StartTime) {
                return true;
            }
            int senderId = 0;
            int targetType = EntityController.Instance.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
            int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
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
            long duration = m_DurationTime.Get(instance);
            float angle = Geometry.DegreeToRadian(obj.transform.localRotation.eulerAngles.y);
            Vector3 center = obj.transform.TransformPoint(m_RelativeCenter);
            if (!m_IsStarted) {
                m_IsStarted = true;
                m_LastPos = center;
            } else if ((center - m_LastPos).sqrMagnitude >= 0.25f || StartTime + duration < curSectionTime) {
                Vector3 c = (m_LastPos + center) / 2;
                Vector3 angleu = center - m_LastPos;
                float queryRadius = range + angleu.magnitude / 2;
                int ct = 0;
                bool isCollide = false;
                List<int> targetIds = new List<int>();
                GameFramework.PluginFramework.Instance.KdTree.QueryWithFunc(c.x, c.y, c.z, queryRadius, (float distSqr, GameFramework.KdTreeObject kdTreeObj) => {
                    int targetId = kdTreeObj.Object.GetId();
                    if (targetType == (int)SkillTargetType.Enemy && CharacterRelation.RELATION_ENEMY == EntityController.Instance.GetRelation(senderId, targetId) ||
                        targetType == (int)SkillTargetType.Friend && CharacterRelation.RELATION_FRIEND == EntityController.Instance.GetRelation(senderId, targetId)) {
                            bool isMatch = Geometry.IsCapsuleDiskIntersect(new ScriptRuntime.Vector2(m_LastPos.x, m_LastPos.z), new ScriptRuntime.Vector2(angleu.x, angleu.z), range, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
                        if (isMatch) {
                            isCollide = true;
                            if (!m_SingleHit || !m_Targets.Contains(targetId)) {
                                m_Targets.Add(targetId);
                                Dictionary<string, object> args;
                                TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                                EntityController.Instance.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, IsFinal, args);
                                targetIds.Add(targetId);
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
                m_LastPos = center;
                if (isCollide && m_FinishOnCollide) {
                    return false;
                }
            }
            if (StartTime + duration < curSectionTime) {
                instance.StopCurSection();
                return false;
            }
            return true;
        }
        protected override void OnInitProperties()
        {
            AddProperty("Duration", () => { return m_DurationTime.EditableValue; }, (object val) => { m_DurationTime.EditableValue = val; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            instance.AddImpactForInit(this);
            int num = callData.GetParamNum();
            if (num >= 5) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.x = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.z = float.Parse(callData.GetParamId(3));
                m_DurationTime.Set(callData.GetParam(4));
            }
            if (num >= 6) {
                m_FinishOnCollide = callData.GetParamId(5) == "true";
            }
            if (num >= 7) {
                m_SingleHit = callData.GetParamId(6) == "true";
            }

        }

        private Vector3 m_RelativeCenter = Vector3.zero;
        private SkillNonStringParam<long> m_DurationTime = new SkillNonStringParam<long>();
        private bool m_FinishOnCollide = false;
        private bool m_SingleHit = false;
        
        private bool m_IsStarted = false;
        private Vector3 m_LastPos = Vector3.zero;
        private HashSet<int> m_Targets = new HashSet<int>();
    }
}