using System;
using System.Collections.Generic;
using ScriptRuntime;
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            int senderId;
            int targetId;
            scene.EntityController.CalcSenderAndTarget(senderObj, out senderId, out targetId);
            int impactId = senderObj.ConfigData.impact;
            if (senderObj.ConfigData.type != (int)SkillOrImpactType.Skill) {
                if (impactId <= 0) {
                    int skillId = scene.EntityController.GetImpactSkillId(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                    TableConfig.Skill cfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                    if (null != cfg) {
                        impactId = cfg.impact;
                    }
                }
            }
            Dictionary<string, object> args;
            TriggerUtil.CalcImpactConfig(0, 0, instance, senderObj.ConfigData, out args);
            scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, args);
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            int senderId = senderObj.ActorId;
            if (senderObj.ConfigData.type != (int)SkillOrImpactType.Skill) {
                senderId = senderObj.TargetActorId;
            }
            int impactId = senderObj.ConfigData.impacttoself;
            if (senderObj.ConfigData.type != (int)SkillOrImpactType.Skill) {
                if (impactId <= 0) {
                    int skillId = scene.EntityController.GetImpactSkillId(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                    TableConfig.Skill cfg = TableConfig.SkillProvider.Instance.GetSkill(skillId);
                    if (null != cfg) {
                        impactId = cfg.impacttoself;
                    }
                }
            }
            if (m_Radius <= Geometry.c_FloatPrecision) {
                Dictionary<string, object> args;
                TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, senderId, impactId, args);
            } else {
                EntityInfo targetObj = senderObj.TargetGfxObj;
                int ct = 0;
                List<int> targetIds = new List<int>();
                TriggerUtil.AoeQuery(scene, obj, targetObj, m_AoeType, m_Radius, m_AngleOrLength, instance, senderId, (int)SkillTargetType.Friend, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    Dictionary<string, object> args;
                    TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                    scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, impactId, args);
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
                m_RelativeCenter.X = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.Y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.Z = float.Parse(callData.GetParamId(3));
                m_Radius = float.Parse(callData.GetParamId(4));
                m_AngleOrLength = float.Parse(callData.GetParamId(5));
                m_AoeType = int.Parse(callData.GetParamId(6));
                m_MaxCount = int.Parse(callData.GetParamId(7));
                m_RelativeToTarget = callData.GetParamId(8) == "true";
            }
        }

        private Vector3 m_RelativeCenter = Vector3.Zero;
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            int targetType = scene.EntityController.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
            int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
            int senderId;
            int targetId;
            scene.EntityController.CalcSenderAndTarget(senderObj, out senderId, out targetId);
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
            scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, args);
            return false;
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num >= 5) {
                m_RelativeCenter.X = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.Y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.Z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.Zero;
        private bool m_RelativeToTarget = false;

        
    }
    /// <summary>
    /// aoeimpact(start_time, center_x, center_y, center_z, relativeToTarget);
    /// </summary>
    public class AoeImpactTriger : AbstractSkillTriger
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) {
                return false;
            }
            if (curSectionTime >= StartTime) {
                int targetType = scene.EntityController.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                int senderId = 0;
                if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                    senderId = senderObj.ActorId;
                } else {
                    senderId = senderObj.TargetActorId;
                }
                int ct = 0;
                TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    Dictionary<string, object> args;
                    TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                    scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, impactId, args);
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

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 5) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.X = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.Y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.Z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.Zero;
        private bool m_RelativeToTarget = false;

        
    }
    /// <summary>
    /// chainaoeimpact(start_time, center_x, center_y, center_z, relativeToTarget, duration, interval);
    /// </summary>
    public class ChainAoeImpactTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ChainAoeImpactTriger triger = new ChainAoeImpactTriger();
            
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_DurationTime = m_DurationTime;
            triger.m_IntervalTime = m_IntervalTime;
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            if (curSectionTime > StartTime + m_DurationTime) {
                return false;
            }
            if (m_LastTime + m_IntervalTime < curSectionTime) {
                m_LastTime = curSectionTime;

                int ct = m_Targets.Count;
                if (ct <= 0) {
                    int targetType = scene.EntityController.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
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
                    scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, m_SenderId, m_Targets[m_CurTargetIndex], m_ImpactId, args);
                    ++m_CurTargetIndex;
                } else {
                    return false;
                }
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 7) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.X = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.Y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.Z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime = long.Parse(callData.GetParamId(5));
                m_IntervalTime = long.Parse(callData.GetParamId(6));
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.Zero;
        private bool m_RelativeToTarget = false;
        private long m_DurationTime = 0;
        private long m_IntervalTime = 0;

        

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
            copy.m_DurationTime = m_DurationTime;
            copy.m_IntervalTime = m_IntervalTime;
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            long durationTime = m_DurationTime;
            long intervalTime = m_IntervalTime;
            if (curSectionTime < StartTime) {
                return true;
            }
            if (curSectionTime > StartTime + durationTime) {
                return false;
            }
            if (m_LastTime + intervalTime < curSectionTime) {
                m_LastTime = curSectionTime;

                int targetType = scene.EntityController.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                int senderId;
                int targetId;
                scene.EntityController.CalcSenderAndTarget(senderObj, out senderId, out targetId);
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
                scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, args);
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 7) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.X = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.Y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.Z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime = long.Parse(callData.GetParamId(5));
                m_IntervalTime = long.Parse(callData.GetParamId(6));
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.Zero;
        private bool m_RelativeToTarget = false;
        private long m_DurationTime = 0;
        private long m_IntervalTime = 0;

        private long m_LastTime = 0;
        
    }
    /// <summary>
    /// periodicallyaoeimpact(start_time, center_x, center_y, center_z, relativeToTarget, duration, interval);
    /// </summary>
    public class PeriodicallyAoeImpactTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            PeriodicallyAoeImpactTriger triger = new PeriodicallyAoeImpactTriger();
            
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_RelativeToTarget = m_RelativeToTarget;
            triger.m_DurationTime = m_DurationTime;
            triger.m_IntervalTime = m_IntervalTime;
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            if (curSectionTime > StartTime + m_DurationTime) {
                return false;
            }
            if (m_LastTime + m_IntervalTime < curSectionTime) {
                m_LastTime = curSectionTime;

                int targetType = scene.EntityController.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                int impactId = TriggerUtil.GetSkillImpactId(instance.Variables, senderObj.ConfigData);
                int senderId = 0;
                if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                    senderId = senderObj.ActorId;
                } else {
                    senderId = senderObj.TargetActorId;
                }
                int ct = 0;
                TriggerUtil.AoeQuery(senderObj, instance, senderId, targetType, m_RelativeCenter, m_RelativeToTarget, (float distSqr, int objId) => {
                    Dictionary<string, object> args;
                    TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                    scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, objId, impactId, args);
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

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 6) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.X = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.Y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.Z = float.Parse(callData.GetParamId(3));
                m_RelativeToTarget = callData.GetParamId(4) == "true";
                m_DurationTime = long.Parse(callData.GetParamId(5));
                m_IntervalTime = long.Parse(callData.GetParamId(6));
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.Zero;
        private bool m_RelativeToTarget = false;
        private long m_DurationTime = 0;
        private long m_IntervalTime = 0;
        private long m_LastTime = 0;

        
    }
    /// <summary>
    /// track(trackBone[,no_impact[,start_time[,duration[,not_move]]]]);
    /// </summary>
    public class TrackTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            TrackTriger triger = new TrackTriger();
            triger.m_TrackBone = m_TrackBone;
            triger.m_NoImpact = m_NoImpact;
            triger.m_Duration = m_Duration;
            triger.m_NotMove = m_NotMove;
            return triger;
        }
        public override void Reset()
        {
            m_IsStarted = false;
            m_Lifetime = 0;
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null != obj) {
                if (curSectionTime >= StartTime) {

                    if (!m_IsStarted) {
                        m_IsStarted = true;

                        Vector3 dest = obj.GetMovementStateInfo().GetPosition3D();
                        dest.Y += 1.5f;

                        Vector3 pos = scene.EntityController.GetImpactSenderPosition(senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                        object speedObj;
                        if (instance.Variables.TryGetValue("emitSpeed", out speedObj)) {
                            m_Speed = (float)speedObj;
                        } else {
                            return false;
                        }

                        float duration = m_Duration;
                        if (duration > Geometry.c_FloatPrecision) {
                            float d = duration / 1000.0f;
                            m_Lifetime = d;
                            m_Speed = (dest - m_StartPos).Length() / m_Lifetime;
                        } else {
                            m_Lifetime = 1.0f;
                            if (m_Speed > Geometry.c_FloatPrecision) {
                                m_Lifetime = (dest - m_StartPos).Length() / m_Speed;
                            }
                        }

                    } else if (curSectionTime > StartTime + (long)(m_Lifetime * 1000)) {
                        m_HitEffectRotation = Quaternion.Identity;
                        if (m_NoImpact) {
                            instance.SetVariable("hitEffectRotation", m_HitEffectRotation);
                        } else {
                            int impactId = scene.EntityController.GetTrackSendImpact(senderObj.ActorId, senderObj.Seq, instance.Variables);
                            Dictionary<string, object> args;
                            TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                            if (args.ContainsKey("hitEffectRotation"))
                                args["hitEffectRotation"] = m_HitEffectRotation;
                            else
                                args.Add("hitEffectRotation", m_HitEffectRotation);
                            scene.EntityController.TrackSendImpact(senderObj.ActorId, senderObj.SkillId, senderObj.Seq, impactId, args);
                            int senderId, targetId;
                            scene.EntityController.CalcSenderAndTarget(senderObj, out senderId, out targetId);
                        }
                        instance.StopCurSection();
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
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum(); 
            if (num > 0) {
                m_TrackBone = callData.GetParamId(0);
            }
            if (num > 1) {
                m_NoImpact = callData.GetParamId(1) == "true";
            }
            if (num > 2) {
                StartTime = long.Parse(callData.GetParamId(2));
            }
            if (num > 3) {
                m_Duration = long.Parse(callData.GetParamId(3));
            }            
            if (num > 4) {
                m_NotMove = callData.GetParamId(4) == "true";
            }            
        }

        private string m_TrackBone = string.Empty;
        private bool m_NoImpact = false;
        private long m_Duration = 0;
        private bool m_NotMove = false;
        
        private Vector3 m_StartPos = Vector3.Zero;
        private Vector3 m_ControlPos = Vector3.Zero;
        private float m_Speed = 10f;
        private float m_Lifetime = 1.0f;
        private bool m_IsStarted = false;
        private Quaternion m_HitEffectRotation;
    }
    /// <summary>
    /// colliderimpact(start_time, center_x, center_y, center_z, duration[, finishOnCollide, singleHit]);
    /// </summary>
    public class ColliderImpactTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ColliderImpactTriger triger = new ColliderImpactTriger();
            
            triger.m_RelativeCenter = m_RelativeCenter;
            triger.m_DurationTime = m_DurationTime;
            triger.m_FinishOnCollide = m_FinishOnCollide;
            triger.m_SingleHit = m_SingleHit;
                        return triger;
        }
        public override void Reset()
        {
            m_IsStarted = false;
            m_LastPos = Vector3.Zero;
            
            m_Targets.Clear();
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (obj == null) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            int senderId = 0;
            int targetType = scene.EntityController.GetTargetType(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
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
            float angle = obj.GetMovementStateInfo().GetFaceDir();
            Vector3 center = Geometry.TransformPoint(obj.GetMovementStateInfo().GetPosition3D(), m_RelativeCenter, angle);
            if (!m_IsStarted) {
                m_IsStarted = true;
                m_LastPos = center;
            } else if ((center - m_LastPos).LengthSquared() >= 0.25f || StartTime + m_DurationTime < curSectionTime) {
                Vector3 c = (m_LastPos + center) / 2;
                Vector3 angleu = center - m_LastPos;
                float queryRadius = range + angleu.Length() / 2;

                int ct = 0;
                bool isCollide = false;
                scene.KdTree.QueryWithFunc(c.X, c.Y, c.Z, queryRadius, (float distSqr, KdTreeObject kdTreeObj) => {
                    int targetId = kdTreeObj.Object.GetId();
                    if (targetType == (int)SkillTargetType.Enemy && CharacterRelation.RELATION_ENEMY == scene.EntityController.GetRelation(senderId, targetId) ||
                        targetType == (int)SkillTargetType.Friend && CharacterRelation.RELATION_FRIEND == scene.EntityController.GetRelation(senderId, targetId)) {
                        bool isMatch = Geometry.IsCapsuleDiskIntersect(new ScriptRuntime.Vector2(center.X, center.Z), new ScriptRuntime.Vector2(angleu.X, angleu.Z), range, new ScriptRuntime.Vector2(kdTreeObj.Position.X, kdTreeObj.Position.Z), kdTreeObj.Radius);
                        if (isMatch) {
                            isCollide = true;
                            if (!m_SingleHit || !m_Targets.Contains(targetId)) {
                                m_Targets.Add(targetId);
                                Dictionary<string, object> args;
                                TriggerUtil.CalcImpactConfig(0, impactId, instance, senderObj.ConfigData, out args);
                                scene.EntityController.SendImpact(senderObj.ConfigData, senderObj.Seq, senderObj.ActorId, senderId, targetId, impactId, args);
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
            if (StartTime + m_DurationTime < curSectionTime) {
                instance.StopCurSection();
                return false;
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 5) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeCenter.X = float.Parse(callData.GetParamId(1));
                m_RelativeCenter.Y = float.Parse(callData.GetParamId(2));
                m_RelativeCenter.Z = float.Parse(callData.GetParamId(3));
                m_DurationTime = long.Parse(callData.GetParamId(4));
            }
            if (num >= 6) {
                m_FinishOnCollide = callData.GetParamId(5) == "true";
            }
            if (num >= 7) {
                m_SingleHit = callData.GetParamId(6) == "true";
            }
            
        }

        private Vector3 m_RelativeCenter = Vector3.Zero;
        private long m_DurationTime = 1000;
        private bool m_FinishOnCollide = false;
        private bool m_SingleHit = false;

        

        private bool m_IsStarted = false;
        private Vector3 m_LastPos = Vector3.Zero;
        private HashSet<int> m_Targets = new HashSet<int>();
    }
}