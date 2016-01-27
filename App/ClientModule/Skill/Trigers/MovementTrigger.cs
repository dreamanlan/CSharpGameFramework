using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    public enum StopAtTargetType
    {
        NoStop = 0,
        AdjustVelocity,
        AdjustTime,
    }
    public class MoveSectionInfo
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
        public override ISkillTriger Clone()
        {
            EnableMoveAgentTriger triger = new EnableMoveAgentTriger();
            triger.m_IsEnable = m_IsEnable;
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
            if (null == obj) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                EnableMoveAgent(obj, m_IsEnable);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_IsEnable = callData.GetParamId(0)=="true";
            } else {
                m_IsEnable = true;
            }
            if (num > 1) {
                m_StartTime = long.Parse(callData.GetParamId(1));
            } else {
                m_StartTime = 0;
            }
            m_RealStartTime = m_StartTime;
        }

        private void EnableMoveAgent(GameObject obj, bool isEnable)
        {
            EntityViewModel npcView = (EntityViewModel)EntityController.Instance.GetEntityView(obj);
            if (npcView != null)
                npcView.SetMoveAgentEnable(isEnable);
        }

        private bool m_IsEnable = true;
        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// curvemove(triggertime, is_lock_rotate, [movetime, speedx, speedy, speedz, accelx, accely, accelz]+)
    /// </summary>
    public class CurveMovementTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            CurveMovementTrigger copy = new CurveMovementTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_IsLockRotate = m_IsLockRotate;
            copy.m_SectionList.AddRange(m_SectionList);
            copy.m_IsCurveMoving = true;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_IsCurveMoving = true;
            m_IsInited = false;
            GameObject.Destroy(m_StartTransform);
            m_RealStartTime = m_StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() > 1) {
                m_StartTime = int.Parse(callData.GetParamId(0));
                m_IsLockRotate = bool.Parse(callData.GetParamId(1));
            }
            m_RealStartTime = m_StartTime;
            m_SectionList.Clear();
            int section_num = 0;
            while (callData.GetParamNum() >= 7 * (section_num + 1) + 2) {
                MoveSectionInfo section = new MoveSectionInfo();
                section.moveTime = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 2));
                section.speedVect.x = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 3));
                section.speedVect.y = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 4));
                section.speedVect.z = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 5));
                section.accelVect.x = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 6));
                section.accelVect.y = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 7));
                section.accelVect.z = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 8));
                m_SectionList.Add(section);
                section_num++;
            }
            if (m_SectionList.Count == 0) {
                return;
            }
            m_IsCurveMoving = true;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            bool isTower = EntityController.Instance.GetEntityType(senderObj.GfxObj) == (int)EntityTypeEnum.Tower;
            if (isTower)
                return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime){
                return true;
            }
            if (!m_IsCurveMoving){
                return false;
            }
            if (!m_IsInited){
                Init(obj, senderObj.TargetGfxObj, instance);
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

        private void Init(GameObject obj, GameObject target, SkillInstance instance)
        {
            CopySectionList();
            CalNewSpeedWithTarget(obj, target, instance);
            m_Now = instance.CurTime / 1000.0f;
            m_SectionListCopy[0].startTime = m_Now;
            m_SectionListCopy[0].lastUpdateTime = m_Now;
            m_SectionListCopy[0].curSpeedVect = m_SectionListCopy[0].speedVect;
            m_StartTransform = new GameObject();
            m_StartTransform.transform.position = obj.transform.position;
            m_StartTransform.transform.rotation = obj.transform.rotation;
            m_IsInited = true;
        }

        private void CopySectionList()
        {
            m_SectionListCopy.Clear();
            for (int i = 0; i < m_SectionList.Count; i++) {
                m_SectionListCopy.Add(m_SectionList[i].Clone());
            }
        }

        private void CalNewSpeedWithTarget(GameObject obj, GameObject target, SkillInstance instance)
        {
            if (null == obj || null == target)
                return;
            obj.transform.localRotation = Quaternion.FromToRotation(obj.transform.position, target.transform.position);
            float cur_distance_z = 0;
            for (int i = 0; i < m_SectionListCopy.Count; i++) {
                cur_distance_z += (m_SectionListCopy[i].speedVect.z * m_SectionListCopy[i].moveTime +
                                   m_SectionListCopy[i].accelVect.z * m_SectionListCopy[i].moveTime * m_SectionListCopy[i].moveTime / 2.0f);
            }
            Vector3 target_motion = (target.transform.position - obj.transform.position);
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
        }

        private Vector3 Move(GameObject obj, Vector3 speed_vect, Vector3 accel_vect, float time)
        {
            m_StartTransform.transform.position = obj.transform.position;
            if (!m_IsLockRotate) {
                m_StartTransform.transform.rotation = obj.transform.rotation;
            }
            Vector3 local_motion = speed_vect * time + accel_vect * time * time / 2;
            Vector3 word_target_pos;
            while (local_motion.magnitude > m_MaxMoveStep) {
                Vector3 child = Vector3.ClampMagnitude(local_motion, m_MaxMoveStep);
                local_motion = local_motion - child;
                word_target_pos = m_StartTransform.transform.TransformPoint(child);
                TriggerUtil.MoveObjTo(obj, word_target_pos);
                m_StartTransform.transform.position = obj.transform.position;
            }
            word_target_pos = m_StartTransform.transform.TransformPoint(local_motion);
            TriggerUtil.MoveObjTo(obj, word_target_pos);
            return (speed_vect + accel_vect * time);
        }

        private bool m_IsLockRotate = true;
        private List<MoveSectionInfo> m_SectionList = new List<MoveSectionInfo>();
        private List<MoveSectionInfo> m_SectionListCopy = new List<MoveSectionInfo>();
        private bool m_IsCurveMoving = false;

        private long m_RealStartTime = 0;

        private GameObject m_StartTransform = null;
        private bool m_IsInited = false;
        private float m_Now;
        private static float m_MaxMoveStep = 3;
    }

    /// <summary>
    /// charge(duration,velocity,stopAtTarget[,distToTarget[,start_time]]);
    /// </summary>
    internal class ChargeTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            ChargeTriger triger = new ChargeTriger();
            triger.m_Duration = m_Duration;
            triger.m_Velocity = m_Velocity;
            triger.m_StopAtTarget = m_StopAtTarget;
            triger.m_Offset = m_Offset;
            triger.m_StartTime = m_StartTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_TargetChecked = false;
            m_RealDuration = 0;
            m_RealVelocity = 1;
            m_Forward = Vector3.zero;
            m_RealStartTime = m_StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            bool isTower = EntityController.Instance.GetEntityType(senderObj.GfxObj) == (int)EntityTypeEnum.Tower;
            if (isTower) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (!m_TargetChecked) {
                m_TargetChecked = true;

                m_RealDuration = m_Duration;
                m_RealVelocity = m_Velocity;
                GameObject targetObj = senderObj.TargetGfxObj;
                if (null != targetObj) {
                    Vector3 srcPos = obj.transform.position;
                    Vector3 targetPos = targetObj.transform.position;
                    float degree = Geometry.GetYAngle(new ScriptRuntime.Vector2(srcPos.x, srcPos.z), new ScriptRuntime.Vector2(targetPos.x, targetPos.z));
                    ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(targetPos.x, targetPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.x, m_Offset.z), degree);
                    targetPos = new Vector3(newPos.X, targetPos.y + m_Offset.y, newPos.Y);
                    if (m_StopAtTarget == (int)StopAtTargetType.AdjustVelocity) {
                        m_RealVelocity = (long)(1000.0f * (targetPos - obj.transform.position).magnitude / m_RealDuration);
                    } else if (m_StopAtTarget == (int)StopAtTargetType.AdjustTime) {
                        m_RealDuration = (long)(1000.0f * (targetPos - obj.transform.position).magnitude / m_RealVelocity);
                    }
                    m_Forward = targetPos - srcPos;
                    m_Forward.Normalize();
                } else {
                    m_Forward = obj.transform.forward;
                }
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            } else if (curSectionTime <= m_RealStartTime + m_RealDuration) {
                float dist = TriggerUtil.ConvertToSecond(delta) * m_RealVelocity;
                Vector3 targetPos = obj.transform.position + m_Forward * dist;
                TriggerUtil.MoveObjTo(obj, targetPos);
                return true;
            } else {
                //
                return false;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
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
                m_StartTime = long.Parse(callData.GetParamId(4));
            }
            m_RealStartTime = m_StartTime;
        }

        private long m_Duration = 0;
        private float m_Velocity = 1;
        private int m_StopAtTarget = 0;
        private Vector3 m_Offset = Vector3.zero;

        private long m_RealStartTime = 0;

        private bool m_TargetChecked = false;
        private long m_RealDuration = 0;
        private float m_RealVelocity = 1;
        private Vector3 m_Forward = Vector3.zero;
    }

    /// <summary>
    /// jump(duration,height,velocity,stopAtTarget[,distToTarget[,start_time]]);
    /// </summary>
    internal class JumpTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            JumpTriger triger = new JumpTriger();
            triger.m_Duration = m_Duration;
            triger.m_Height = m_Height;
            triger.m_Velocity = m_Velocity;
            triger.m_StopAtTarget = m_StopAtTarget;
            triger.m_Offset = m_Offset;
            triger.m_StartTime = m_StartTime;
            triger.m_YVelocity = m_YVelocity;
            triger.m_G = m_G;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_TargetChecked = false;
            m_RealDuration = 0;
            m_RealVelocity = 1;
            m_InitY = -1;
            m_Forward = Vector3.zero;
            m_RealStartTime = m_StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            bool isTower = EntityController.Instance.GetEntityType(senderObj.GfxObj) == (int)EntityTypeEnum.Tower;
            if (isTower)
                return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (!m_TargetChecked) {
                m_TargetChecked = true;

                m_RealDuration = m_Duration;
                m_RealVelocity = m_Velocity;
                GameObject targetObj = senderObj.TargetGfxObj;
                if (null != targetObj) {
                    Vector3 srcPos = obj.transform.position;
                    Vector3 targetPos = targetObj.transform.position;
                    float degree = Geometry.GetYAngle(new ScriptRuntime.Vector2(srcPos.x, srcPos.z), new ScriptRuntime.Vector2(targetPos.x, targetPos.z));
                    ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(targetPos.x, targetPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.x, m_Offset.z), degree);
                    targetPos = new Vector3(newPos.X, targetPos.y + m_Offset.y, newPos.Y);
                    if (m_StopAtTarget == (int)StopAtTargetType.AdjustVelocity) {
                        m_RealVelocity = (long)(1000.0f * (targetPos - obj.transform.position).magnitude / m_RealDuration);
                    } else if (m_StopAtTarget == (int)StopAtTargetType.AdjustTime) {
                        m_RealDuration = (long)(1000.0f * (targetPos - obj.transform.position).magnitude / m_RealVelocity);
                        CalcYVelocityAndG();
                    }
                    m_Forward = targetPos - srcPos;
                    m_Forward.Normalize();
                } else {
                    m_Forward = obj.transform.forward;
                }
                m_InitY = obj.transform.position.y;
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            } else if (curSectionTime <= m_RealDuration) {
                float t = (float)(int)(curSectionTime - m_RealStartTime) / 1000.0f;
                float disty = m_YVelocity * t - m_G * t * t / 2;
                float dist = TriggerUtil.ConvertToSecond(delta) * m_RealVelocity;
                Vector3 targetPos = obj.transform.position + m_Forward * dist;
                targetPos.y = m_InitY + disty;

                TriggerUtil.MoveObjTo(obj, targetPos);
                return true;
            } else {
                return false;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Duration = long.Parse(callData.GetParamId(0));
            } else {
                m_Duration = 0;
            }
            if (num > 1) {
                m_Height = float.Parse(callData.GetParamId(1));
            }
            if (num > 2) {
                m_Velocity = float.Parse(callData.GetParamId(2));
            }
            if (num > 3) {
                m_StopAtTarget = int.Parse(callData.GetParamId(3));
            }
            if (num > 4) {
                m_Offset = DslUtility.CalcVector3(callData.GetParam(4) as Dsl.CallData);
            }
            if (num > 5) {
                m_StartTime = long.Parse(callData.GetParamId(5));
            }
            m_RealStartTime = m_StartTime;
            CalcYVelocityAndG();
        }

        private void CalcYVelocityAndG()
        {
            float time_div = (float)(int)m_Duration / 1000.0f;
            m_YVelocity = m_Height * 2.0f / time_div;
            m_G = m_Height * 4.0f / (time_div * time_div);
        }

        private long m_Duration = 0;
        private float m_Height = 1;
        private float m_Velocity = 1;
        private int m_StopAtTarget = 0;
        private Vector3 m_Offset = Vector3.zero;

        private long m_RealStartTime = 0;

        private bool m_TargetChecked = false;
        private long m_RealDuration = 0;
        private float m_RealVelocity = 1;
        private float m_InitY = -1;
        private float m_YVelocity = 1;
        private float m_G = 10;
        private Vector3 m_Forward = Vector3.zero;
    }
}