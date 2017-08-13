using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    internal enum StopAtTargetType
    {
        NoStop = 0,
        AdjustVelocity,
        AdjustTime,
    }
    internal class MoveSectionInfo
    {
        public MoveSectionInfo Clone()
        {
            MoveSectionInfo copy = new MoveSectionInfo();
            copy.moveTime = moveTime;
            copy.speedVect = speedVect;
            copy.accelVect = accelVect;
            return copy;
        }
        public float moveTime;
        public Vector3 speedVect;
        public Vector3 accelVect;
        public float startTime = 0;
        public float lastUpdateTime = 0;
        public Vector3 curSpeedVect = Vector3.zero;
    }
    /// <summary>
    /// enablemoveagent(true_or_false[,start_time]);
    /// </summary>
    internal class EnableMoveAgentTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            EnableMoveAgentTriger triger = new EnableMoveAgentTriger();
            triger.m_IsEnable = m_IsEnable;
            
            
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
                EnableMoveAgent(obj, m_IsEnable);
                return false;
            } else {
                return true;
            }
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_IsEnable = callData.GetParamId(0) == "true";
            } else {
                m_IsEnable = true;
            }
            if (num > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            } else {
                StartTime = 0;
            }
            
        }
        private void EnableMoveAgent(GameObject obj, bool isEnable)
        {
            try {
                EntityViewModel npcView = (EntityViewModel)EntityController.Instance.GetEntityView(obj);
                if (npcView != null) {
                    if (isEnable) {
                        //贴地
                        var pos = obj.transform.position;
                        TriggerUtil.GetRayCastPosInNavMesh(pos + UnityEngine.Vector3.up * 500, pos + UnityEngine.Vector3.down * 500, ref pos);
                        obj.transform.position = pos;
                    }
                    npcView.SetMoveAgentEnable(isEnable);
                }
            } catch (Exception ex) {
                LogSystem.Warn("SetMoveAgentEnable exception:{0}", ex.Message);
            }
        }
        private bool m_IsEnable = true;
        
    }
    /// <summary>
    /// curvemove(triggertime, [movetime, speedx, speedy, speedz, accelx, accely, accelz]+, [moveType])
    /// </summary>
    internal class CurveMovementTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            CurveMovementTrigger copy = new CurveMovementTrigger();
            
            copy.m_SectionList.AddRange(m_SectionList);
            copy.m_IsCurveMoving = true;
            copy.m_IsFreeMove = m_IsFreeMove;
            return copy;
        }
        public override void Reset()
        {
            m_IsCurveMoving = true;
            m_IsInited = false;
            GameObject.Destroy(m_StartTransform);            
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = int.Parse(callData.GetParamId(0));
            }            
            m_SectionList.Clear();
            int nextParamIndex = 1;
            int section_num = 0;
            while (num >= nextParamIndex + 7) {
                MoveSectionInfo section = new MoveSectionInfo();
                section.moveTime = (float)System.Convert.ToDouble(callData.GetParamId(nextParamIndex));
                ++nextParamIndex;
                section.speedVect.x = (float)System.Convert.ToDouble(callData.GetParamId(nextParamIndex));
                ++nextParamIndex;
                section.speedVect.y = (float)System.Convert.ToDouble(callData.GetParamId(nextParamIndex));
                ++nextParamIndex;
                section.speedVect.z = (float)System.Convert.ToDouble(callData.GetParamId(nextParamIndex));
                ++nextParamIndex;
                section.accelVect.x = (float)System.Convert.ToDouble(callData.GetParamId(nextParamIndex));
                ++nextParamIndex;
                section.accelVect.y = (float)System.Convert.ToDouble(callData.GetParamId(nextParamIndex));
                ++nextParamIndex;
                section.accelVect.z = (float)System.Convert.ToDouble(callData.GetParamId(nextParamIndex));
                ++nextParamIndex;
                m_SectionList.Add(section);
                section_num++;
            }
            if (num > nextParamIndex) {
                string moveType = callData.GetParamId(nextParamIndex);
                ++nextParamIndex;
                IList<string> list = Converter.ConvertStringList(moveType);
                m_IsFreeMove = list.IndexOf("freemove") >= 0;
            }
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (null != senderObj.TrackEffectObj)
                obj = senderObj.TrackEffectObj;
            if (curSectionTime < StartTime) {
                return true;
            }
            if (!m_IsCurveMoving) {
                return false;
            }
            if (!m_IsInited) {
                Init(senderObj, instance);
            }
            if (m_SectionListCopy.Count == 0) {
                m_IsCurveMoving = false;
                return false;
            }
            m_Now += TriggerUtil.ConvertToSecond((long)(instance.OriginalDelta * instance.MoveScale));
            MoveSectionInfo cur_section = m_SectionListCopy[0];
            if (m_Now - cur_section.startTime > cur_section.moveTime) {
                float end_time = cur_section.startTime + cur_section.moveTime;
                float used_time = end_time - cur_section.lastUpdateTime;
                cur_section.curSpeedVect = Move(obj, cur_section.curSpeedVect, cur_section.accelVect, used_time);
                m_SectionListCopy.RemoveAt(0);
                if (m_SectionListCopy.Count > 0) {
                    cur_section = m_SectionListCopy[0];
                    cur_section.startTime = end_time;
                    cur_section.lastUpdateTime = end_time;
                    cur_section.curSpeedVect = cur_section.speedVect;
                } else {
                    m_IsCurveMoving = false;
                }
            } else {
                cur_section.curSpeedVect = Move(obj, cur_section.curSpeedVect, cur_section.accelVect, m_Now - cur_section.lastUpdateTime);
                cur_section.lastUpdateTime = m_Now;
            }
            return true;
        }
        private void Init(GfxSkillSenderInfo senderObj, SkillInstance instance)
        {
            CopySectionList();
            CalNewSpeedWithTarget(senderObj, instance);
            m_Now = instance.CurTime / 1000.0f;
            m_SectionListCopy[0].startTime = m_Now;
            m_SectionListCopy[0].lastUpdateTime = m_Now;
            m_SectionListCopy[0].curSpeedVect = m_SectionListCopy[0].speedVect;
            m_StartTransform = new GameObject();
            if (null != senderObj.TrackEffectObj) {
                m_StartTransform.transform.position = senderObj.TrackEffectObj.transform.position;
                m_StartTransform.transform.rotation = senderObj.TrackEffectObj.transform.rotation;
            } else {
                m_StartTransform.transform.position = senderObj.GfxObj.transform.position;
                m_StartTransform.transform.rotation = senderObj.GfxObj.transform.rotation;
            }
            m_IsInited = true;
        }
        private void CopySectionList()
        {
            m_SectionListCopy.Clear();
            for (int i = 0; i < m_SectionList.Count; i++) {
                m_SectionListCopy.Add(m_SectionList[i].Clone());
            }
        }
        private void CalNewSpeedWithTarget(GfxSkillSenderInfo senderObj, SkillInstance instance)
        {
            GameObject obj = senderObj.GfxObj;
            GameObject target = senderObj.TargetGfxObj;
            if (null == obj)
                return;
            float cur_distance_z = 0;
            for (int i = 0; i < m_SectionListCopy.Count; i++) {
                cur_distance_z += (m_SectionListCopy[i].speedVect.z * m_SectionListCopy[i].moveTime +
                                   m_SectionListCopy[i].accelVect.z * m_SectionListCopy[i].moveTime * m_SectionListCopy[i].moveTime / 2.0f);
            }
            m_MaxDistance = Mathf.Abs(cur_distance_z) * 2;

            if (!m_IsFreeMove) {
                Vector3 targetPos = Vector3.zero;
                if (null != target) {
                    targetPos = target.transform.position;
                }
                if (targetPos.sqrMagnitude > Geometry.c_FloatPrecision) {
                    Vector3 srcPos = obj.transform.position;
                    Vector3 target_motion = targetPos - srcPos;
                    target_motion.y = 0;
                    float target_distance_z = target_motion.magnitude;
                    float speed_ratio = 1;
                    if (cur_distance_z != 0) {
                        speed_ratio = target_distance_z / cur_distance_z;
                    }
                    for (int i = 0; i < m_SectionListCopy.Count; i++) {
                        m_SectionListCopy[i].speedVect.z *= speed_ratio;
                        m_SectionListCopy[i].accelVect.z *= speed_ratio;
                    }
                    m_MaxDistance = Mathf.Abs(target_distance_z) * 2;
                }
            }
        }
        private Vector3 Move(GameObject obj, Vector3 speed_vect, Vector3 accel_vect, float time)
        {
            m_StartTransform.transform.position = obj.transform.position;
            m_StartTransform.transform.rotation = obj.transform.rotation;
            Vector3 local_motion = speed_vect * time + accel_vect * time * time / 2;
            Vector3 word_target_pos;
            while (local_motion.magnitude > m_MaxMoveStep) {
                Vector3 child = Vector3.ClampMagnitude(local_motion, m_MaxMoveStep);
                local_motion = local_motion - child;
                word_target_pos = m_StartTransform.transform.TransformPoint(child);
                if (!m_IsFreeMove) {
                    if (!TriggerUtil.RaycastNavmesh(obj.transform.position, word_target_pos)) {
                        TriggerUtil.MoveObjTo(obj, word_target_pos);
                    } else {
                        word_target_pos = TriggerUtil.GetGroundPos(word_target_pos);
                        TriggerUtil.MoveObjTo(obj, word_target_pos);
                    }
                } else {
                    TriggerUtil.MoveObjTo(obj, word_target_pos);
                }
                m_StartTransform.transform.position = obj.transform.position;
            }
            word_target_pos = m_StartTransform.transform.TransformPoint(local_motion);
            if (!m_IsFreeMove) {
                if (!TriggerUtil.RaycastNavmesh(obj.transform.position, word_target_pos)) {
                    TriggerUtil.MoveObjTo(obj, word_target_pos);
                } else {
                    word_target_pos = TriggerUtil.GetGroundPos(word_target_pos);
                    TriggerUtil.MoveObjTo(obj, word_target_pos);
                }
            } else {
                TriggerUtil.MoveObjTo(obj, word_target_pos);
            }
            return (speed_vect + accel_vect * time);
        }

        private List<MoveSectionInfo> m_SectionList = new List<MoveSectionInfo>();
        private List<MoveSectionInfo> m_SectionListCopy = new List<MoveSectionInfo>();
        private bool m_IsFreeMove = false;

        private bool m_IsCurveMoving = true;
        private bool m_IsInited = false;
        private GameObject m_StartTransform = null;
        private float m_Now;
        private float m_MaxDistance = 10.0f;

        private static float m_MaxMoveStep = 3;
    }

    /// <summary>
    /// charge(duration,velocity,stopAtTarget[,distToTarget[,start_time[, moveType]]]);
    /// </summary>
    internal class ChargeTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ChargeTriger triger = new ChargeTriger();
            triger.m_Duration = m_Duration;
            triger.m_Velocity = m_Velocity;
            triger.m_StopAtTarget = m_StopAtTarget;
            triger.m_Offset = m_Offset;
            triger.m_IsFreeMove = m_IsFreeMove;
            triger.m_RelativeToSelf = m_RelativeToSelf;
            triger.m_RandRelativeToSelf = m_RandRelativeToSelf;
            triger.m_RandRelativeToTarget = m_RandRelativeToTarget;
            triger.m_Curve = m_Curve;
            return triger;
        }
        public override void Reset()
        {
            m_TargetChecked = false;
            m_RealDuration = 0;
            m_RealVelocity = 1;
            m_Forward = Vector3.zero;            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            GameObject targetObj = senderObj.TargetGfxObj;
            if (null != senderObj.TrackEffectObj) {
                obj = senderObj.TrackEffectObj;
                targetObj = senderObj.GfxObj;
            }
            if (curSectionTime < StartTime) {
                return true;
            } else if (curSectionTime <= StartTime + m_RealDuration) {
                if (!Prepare(senderObj, instance, obj, targetObj)) {
                    return false;
                }
                if (null != m_Curve && m_Curve.keys.Length >= 2) {
                    float time = Mathf.Clamp01((curSectionTime - StartTime) * 1.0f / m_RealDuration);
                    float val = m_Curve.Evaluate(time);
                    Vector3 targetPos = Vector3.SlerpUnclamped(new Vector3(m_StartPos.x, m_StartPos.y, m_StartPos.z), new Vector3(m_TargetPos.x, m_StartPos.y, m_StartPos.z), val);
                    if (!m_IsFreeMove) {
                        if (!TriggerUtil.RaycastNavmesh(obj.transform.position, targetPos)) {
                            TriggerUtil.MoveObjTo(obj, targetPos);
                        } else {
                            targetPos = TriggerUtil.GetGroundPos(targetPos);
                            TriggerUtil.MoveObjTo(obj, targetPos);
                        }
                    } else {
                        TriggerUtil.MoveObjTo(obj, targetPos);
                    }
                } else {
                    float dist = TriggerUtil.ConvertToSecond(delta) * m_RealVelocity;
                    Vector3 targetPos = obj.transform.position + m_Forward * dist;
                    if (!m_IsFreeMove) {
                        if (!TriggerUtil.RaycastNavmesh(obj.transform.position, targetPos)) {
                            TriggerUtil.MoveObjTo(obj, targetPos);
                        } else {
                            targetPos = TriggerUtil.GetGroundPos(targetPos);
                            TriggerUtil.MoveObjTo(obj, targetPos);
                        }
                    } else {
                        TriggerUtil.MoveObjTo(obj, targetPos);
                    }
                }
                return true;
            } else {
                Vector3 diff = m_TargetPos - obj.transform.position;
                if (diff.sqrMagnitude > 0.01f) {
                    Vector3 targetPos = m_TargetPos;
                    if (!m_IsFreeMove) {
                        if (!TriggerUtil.RaycastNavmesh(obj.transform.position, targetPos)) {
                            TriggerUtil.MoveObjTo(obj, targetPos);
                        } else {
                            targetPos = TriggerUtil.GetGroundPos(targetPos);
                            TriggerUtil.MoveObjTo(obj, targetPos);
                        }
                    } else {
                        TriggerUtil.MoveObjTo(obj, targetPos);
                    }
                }
                return false;
            }
        }
        private bool Prepare(GfxSkillSenderInfo senderObj, SkillInstance instance, GameObject obj, GameObject targetObj)
        {
            if (!m_TargetChecked) {
                m_TargetChecked = true;
                m_RealDuration = m_Duration;
                m_RealVelocity = m_Velocity;
                if (m_RealDuration <= 0) {
                    m_RealDuration += instance.CurSectionDuration;
                }
                if (m_RealDuration <= 0) {
                    LogSystem.Warn("charge duration is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                    return false;
                }
                m_StartPos = obj.transform.position;
                Vector3 targetPos = Vector3.zero;
                if (null != targetObj) {
                    targetPos = targetObj.transform.position;
                }
                if (!m_IsFreeMove && targetPos.sqrMagnitude > Geometry.c_FloatPrecision) {
                    float degree = Geometry.GetYRadian(new ScriptRuntime.Vector2(m_StartPos.x, m_StartPos.z), new ScriptRuntime.Vector2(targetPos.x, targetPos.z));
                    ScriptRuntime.Vector2 newPos;
                    if (m_RandRelativeToSelf) {
                        int v = Helper.Random.Next();
                        if (v < 50) {
                            newPos = new ScriptRuntime.Vector2(m_StartPos.x, m_StartPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.x, m_Offset.z), degree);
                            targetPos = new Vector3(newPos.X, m_StartPos.y + m_Offset.y, newPos.Y);
                        } else {
                            newPos = new ScriptRuntime.Vector2(m_StartPos.x, m_StartPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(-m_Offset.x, m_Offset.z), degree);
                            targetPos = new Vector3(newPos.X, m_StartPos.y + m_Offset.y, newPos.Y);
                        }
                    } else if (m_RelativeToSelf) {
                        newPos = new ScriptRuntime.Vector2(m_StartPos.x, m_StartPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.x, m_Offset.z), degree);
                        targetPos = new Vector3(newPos.X, m_StartPos.y + m_Offset.y, newPos.Y);
                    } else if (m_RandRelativeToTarget) {
                        int v = Helper.Random.Next();
                        if (v < 50) {
                            newPos = new ScriptRuntime.Vector2(targetPos.x, targetPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.x, m_Offset.z), degree);
                            targetPos = new Vector3(newPos.X, targetPos.y + m_Offset.y, newPos.Y);
                        } else {
                            newPos = new ScriptRuntime.Vector2(targetPos.x, targetPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(-m_Offset.x, m_Offset.z), degree);
                            targetPos = new Vector3(newPos.X, targetPos.y + m_Offset.y, newPos.Y);
                        }
                    } else {
                        newPos = new ScriptRuntime.Vector2(targetPos.x, targetPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.x, m_Offset.z), degree);
                        targetPos = new Vector3(newPos.X, targetPos.y + m_Offset.y, newPos.Y);
                    }

                    m_Forward = targetPos - m_StartPos;
                    m_Forward.y = 0;
                    m_Forward.Normalize();

                    if (m_StopAtTarget == (int)StopAtTargetType.AdjustVelocity) {
                        m_RealVelocity = (long)(1000.0f * (targetPos - m_StartPos).magnitude / m_RealDuration);
                        m_TargetPos = targetPos;
                    } else if (m_StopAtTarget == (int)StopAtTargetType.AdjustTime) {
                        m_RealDuration = (long)(1000.0f * (targetPos - m_StartPos).magnitude / m_RealVelocity);
                        m_TargetPos = targetPos;
                    } else {
                        m_TargetPos = m_StartPos + m_Forward * m_RealVelocity * m_RealDuration / 1000.0f;
                    }
                } else {
                    if (m_RandRelativeToSelf) {
                        int v = Helper.Random.Next();
                        if (v < 50) {
                            m_TargetPos = obj.transform.TransformPoint(m_Offset.x, 0, m_Offset.z);
                        } else {
                            m_TargetPos = obj.transform.TransformPoint(-m_Offset.x, 0, m_Offset.z);
                        }
                    } else if (m_RelativeToSelf) {
                        m_TargetPos = obj.transform.TransformPoint(m_Offset.x, 0, m_Offset.z);
                    } else {
                        m_TargetPos = obj.transform.TransformPoint(0, 0, m_RealVelocity * m_RealDuration / 1000.0f);
                    }
                    m_Forward = obj.transform.forward;
                }
            }
            return true;
        }
        protected override void OnInitProperties()
        {
            AddProperty("Duration", () => { return m_Duration; }, (object val) => { m_Duration = (long)Convert.ChangeType(val, typeof(long)); });
            AddProperty("Velocity", () => { return m_Velocity; }, (object val) => { m_Velocity = (float)Convert.ChangeType(val, typeof(float)); });
            AddProperty("StopAtTarget", () => { return m_StopAtTarget; }, (object val) => { m_StopAtTarget = (int)Convert.ChangeType(val, typeof(int)); });
            AddProperty("Curve", () => { return m_Curve; }, (object val) => { m_Curve = val as AnimationCurve; });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Duration = long.Parse(callData.GetParamId(0));
            }
            if (num > 1) {
                m_Velocity = float.Parse(callData.GetParamId(1));
            }
            if (num > 2) {
                m_StopAtTarget = int.Parse(callData.GetParamId(2));
            }
            if (num > 3) {
                m_Offset = DslUtility.CalcVector3(callData.GetParam(3) as Dsl.CallData);
            }
            if (num > 4) {
                StartTime = long.Parse(callData.GetParamId(4));
            }
            if (num > 5) {
                string moveType = callData.GetParamId(5);
                IList<string> list = Converter.ConvertStringList(moveType);
                m_IsFreeMove = list.IndexOf("freemove") >= 0;
                m_RelativeToSelf = list.IndexOf("relativetoself") >= 0;
                m_RandRelativeToSelf = list.IndexOf("randrelativetoself") >= 0;
                m_RandRelativeToTarget = list.IndexOf("randrelativetotarget") >= 0;
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

        private AnimationCurve m_Curve = null;

        private long m_Duration = 0;
        private float m_Velocity = 1;
        private int m_StopAtTarget = 0;
        private Vector3 m_Offset = Vector3.zero;
        private bool m_IsFreeMove = false;
        private bool m_RelativeToSelf = false;
        private bool m_RandRelativeToSelf = false;
        private bool m_RandRelativeToTarget = false;
        
        private bool m_TargetChecked = false;
        private long m_RealDuration = 0;
        private float m_RealVelocity = 1;
        private Vector3 m_Forward = Vector3.zero;
        private Vector3 m_StartPos = Vector3.zero;
        private Vector3 m_TargetPos = Vector3.zero;
    }

    /// <summary>
    /// parabola(duration, G, vector3(x,y,z) or objpath[, moveType]);
    /// </summary>
    internal class ParabolaTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ParabolaTriger triger = new ParabolaTriger();
            triger.m_Duration = m_Duration;
            triger.m_G = m_G;
            triger.m_Offset = m_Offset;
            triger.m_ObjPath = m_ObjPath;
            triger.m_IsFaceToTarget = m_IsFaceToTarget;
            return triger;
        }
        public override void Reset()
        {
            m_TargetChecked = false;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            GameObject targetObj = senderObj.TargetGfxObj;
            if (null != senderObj.TrackEffectObj) {
                obj = senderObj.TrackEffectObj;
                targetObj = senderObj.GfxObj;
            }
            if (curSectionTime < StartTime) {
                return true;
            } else if (curSectionTime <= StartTime + m_Duration) {
                if (!Prepare(senderObj, obj, targetObj)) {
                    return false;
                }
                float t = (float)(int)(curSectionTime - StartTime) / 1000.0f;
                float disty = m_YVelocity * t - m_G * t * t / 2;
                float dist = TriggerUtil.ConvertToSecond(delta) * m_Velocity;
                Vector3 targetPos = obj.transform.position + m_Forward * dist;
                targetPos.y = m_InitY + disty;
                TriggerUtil.MoveObjTo(obj, targetPos);
                return true;
            } else {
                return false;
            }
        }
        private bool Prepare(GfxSkillSenderInfo senderObj, GameObject obj, GameObject targetObj)
        {
            if (!m_TargetChecked) {
                m_TargetChecked = true;
                Vector3 srcPos = obj.transform.position;
                Vector3 targetPos = Vector3.zero;
                if (null != targetObj && m_ObjPath == "vector3") {
                    targetPos = targetObj.transform.position;
                    float degree = Geometry.GetYRadian(new ScriptRuntime.Vector2(srcPos.x, srcPos.z), new ScriptRuntime.Vector2(targetPos.x, targetPos.z));
                    ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(targetPos.x, targetPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.x, m_Offset.z), degree);
                    targetPos = new Vector3(newPos.X, targetPos.y + m_Offset.y, newPos.Y);
                } else {
                    var tobj = GameObject.Find(m_ObjPath);
                    if (null != tobj) {
                        targetPos = tobj.transform.position;
                    } else {
                        return false;
                    }
                }
                m_Forward = targetPos - srcPos;
                m_Forward.y = 0;
                m_Velocity = m_Forward.magnitude * 1000 / m_Duration;
                m_Forward.Normalize();
                if (m_IsFaceToTarget) {
                    float radian = Geometry.GetYRadian(new ScriptRuntime.Vector2(srcPos.x, srcPos.z), new ScriptRuntime.Vector2(targetPos.x, targetPos.z));
                    obj.transform.rotation = Quaternion.AngleAxis(Geometry.RadianToDegree(radian), Vector3.up); 
                }
                float tan = GetTanToPoint(m_Velocity, m_G, srcPos, targetPos);
                m_InitY = srcPos.y;
                m_YVelocity = m_Velocity * tan;
            }
            return true;
        }
        private float GetTanToPoint(float speed, float g, Vector3 bp, Vector3 tp)
        {
            var dx = Geometry.Distance(new ScriptRuntime.Vector2(bp.x, bp.z), new ScriptRuntime.Vector2(tp.x, tp.z));
            var dy = tp.y - bp.y;
            var tempV = g * dx / (2 * speed * speed);

            //speed是沿切线方向的初速度时
            //var ta1 = (1 + Mathf.Sqrt(1 - 4 * tempV * (tempV + dy / dx))) / (2 * tempV);
            //var ta2 = (1 - Mathf.Sqrt(1 - 4 * tempV * (tempV + dy / dx))) / (2 * tempV);
            //return ta2;

            //speed是水平方向初速度分量时等式更简单
            if (dx > Geometry.c_FloatPrecision)
                return tempV + dy / dx;
            else
                return 0;
        }  
        protected override void OnInitProperties()
        {
            AddProperty("Velocity", () => { return m_Velocity; }, (object val) => { m_Velocity = (float)Convert.ChangeType(val, typeof(float)); });
            AddProperty("G", () => { return m_G; }, (object val) => { m_G = (float)Convert.ChangeType(val, typeof(float)); });
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Duration = long.Parse(callData.GetParamId(0));
            } else {
                m_Duration = 1000;
            }
            if (num > 1) {
                m_G = float.Parse(callData.GetParamId(1));
            }
            if (num > 2) {
                var param = callData.GetParam(2);
                m_ObjPath = param.GetId();
                var cd = param as Dsl.CallData;
                if (null != cd && m_ObjPath == "vector3") {
                    m_Offset = DslUtility.CalcVector3(cd);
                }
            }
            if (num > 3) {
                StartTime = long.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                string moveType = callData.GetParamId(4);
                IList<string> list = Converter.ConvertStringList(moveType);
                m_IsFaceToTarget = list.IndexOf("facetotarget") >= 0;
            }
        }

        private long m_Duration = 1000;
        private float m_G = 10.0f;
        private Vector3 m_Offset = Vector3.zero;
        private string m_ObjPath = string.Empty;
        private bool m_IsFaceToTarget = false;
        
        private bool m_TargetChecked = false;
        private float m_InitY = 0;
        private float m_YVelocity = 1;
        private float m_Velocity = 1;
        private Vector3 m_Forward = Vector3.zero;
    }
    /// <summary>
    /// flight(starttime, curvetime, curveheight, qtestarttime, qteduration, qteheight, qtebuttonduration, upanim, downanim, falldownanim, fadetime);
    /// </summary>
    internal class FlightTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            FlightTriger triger = new FlightTriger();
            triger.m_CurveTime = m_CurveTime;
            triger.m_CurveHeight = m_CurveHeight;
            triger.m_QteStartTime = m_QteStartTime;
            triger.m_QteDuration = m_QteDuration;
            triger.m_QteHeight = m_QteHeight;
            triger.m_QteButtonDuration = m_QteButtonDuration;
            triger.m_UpAnim = m_UpAnim;
            triger.m_DownAnim = m_DownAnim;
            triger.m_FalldownAnim = m_FalldownAnim;
            triger.m_AnimFadeTime = m_AnimFadeTime;
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
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            HitFlightManager.Instance.Trigger(senderObj, m_CurveTime / 1000.0f, m_CurveHeight, m_QteStartTime / 1000.0f, m_QteDuration / 1000.0f, m_QteHeight, m_QteButtonDuration / 1000.0f, m_UpAnim, m_DownAnim, m_FalldownAnim, m_AnimFadeTime / 1000.0f);
            return false;
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 11) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_CurveTime = float.Parse(callData.GetParamId(1));
                m_CurveHeight = float.Parse(callData.GetParamId(2));
                m_QteStartTime = float.Parse(callData.GetParamId(3));
                m_QteDuration = float.Parse(callData.GetParamId(4));
                m_QteHeight = float.Parse(callData.GetParamId(5));
                m_QteButtonDuration = float.Parse(callData.GetParamId(6));
                m_UpAnim = callData.GetParamId(7);
                m_DownAnim = callData.GetParamId(8);
                m_FalldownAnim = callData.GetParamId(9);
                m_AnimFadeTime = float.Parse(callData.GetParamId(10));
            }
        }

        private float m_CurveTime = 0;
        private float m_CurveHeight = 0;
        private float m_QteStartTime = 0;
        private float m_QteDuration = 0;
        private float m_QteHeight = 0;
        private float m_QteButtonDuration = 0;
        private string m_UpAnim = string.Empty;
        private string m_DownAnim = string.Empty;
        private string m_FalldownAnim = string.Empty;
        private float m_AnimFadeTime = 0;
    }
    /// <summary>
    /// flighthit(starttime, hitduration);
    /// </summary>
    internal class FlightHitTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            FlightHitTriger triger = new FlightHitTriger();
            triger.m_HitDuration = m_HitDuration;
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
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            HitFlightManager.Instance.Hit(senderObj, m_HitDuration / 1000.0f);
            return false;
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_HitDuration = float.Parse(callData.GetParamId(1));
            }
        }

        private float m_HitDuration = 0;
    }
}