using System;
using System.Collections.Generic;
using ScriptRuntime;
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
        public Vector3 curSpeedVect = Vector3.Zero;
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
            EntityInfo obj = senderObj.GfxObj;
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

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_IsEnable = callData.GetParamId(0)=="true";
            } else {
                m_IsEnable = true;
            }
            if (num > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            } else {
                StartTime = 0;
            }
            
        }

        private void EnableMoveAgent(EntityInfo obj, bool isEnable)
        {
        }

        private bool m_IsEnable = true;
        
    }
    /// <summary>
    /// curvemove(triggertime, is_lock_rotate, [movetime, speedx, speedy, speedz, accelx, accely, accelz]+)
    /// </summary>
    public class CurveMovementTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            CurveMovementTrigger copy = new CurveMovementTrigger();
            
            copy.m_IsLockRotate = m_IsLockRotate;
            copy.m_SectionList.AddRange(m_SectionList);
            copy.m_IsCurveMoving = true;
                        return copy;
        }

        public override void Reset()
        {
            m_IsCurveMoving = true;
            m_IsInited = false;
            
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() > 1) {
                StartTime = int.Parse(callData.GetParamId(0));
                m_IsLockRotate = bool.Parse(callData.GetParamId(1));
            }
            
            m_SectionList.Clear();
            int section_num = 0;
            while (callData.GetParamNum() >= 7 * (section_num + 1) + 2) {
                MoveSectionInfo section = new MoveSectionInfo();
                section.moveTime = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 2));
                section.speedVect.X = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 3));
                section.speedVect.Y = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 4));
                section.speedVect.Z = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 5));
                section.accelVect.X = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 6));
                section.accelVect.Y = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 7));
                section.accelVect.Z = (float)System.Convert.ToDouble(callData.GetParamId((section_num * 7) + 8));
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            bool isTower = scene.EntityController.GetEntityType(senderObj.GfxObj) == (int)EntityTypeEnum.Tower;
            if (isTower)
                return false;
            if (curSectionTime < StartTime){
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

        private void Init(EntityInfo obj, EntityInfo target, SkillInstance instance)
        {
            CopySectionList();
            CalNewSpeedWithTarget(obj, target, instance);
            m_Now = instance.CurTime / 1000.0f;
            m_SectionListCopy[0].startTime = m_Now;
            m_SectionListCopy[0].lastUpdateTime = m_Now;
            m_SectionListCopy[0].curSpeedVect = m_SectionListCopy[0].speedVect;
            m_StartPos = obj.GetMovementStateInfo().GetPosition3D();
            m_StartDir = obj.GetMovementStateInfo().GetFaceDir();
            m_IsInited = true;
        }

        private void CopySectionList()
        {
            m_SectionListCopy.Clear();
            for (int i = 0; i < m_SectionList.Count; i++) {
                m_SectionListCopy.Add(m_SectionList[i].Clone());
            }
        }

        private void CalNewSpeedWithTarget(EntityInfo obj, EntityInfo target, SkillInstance instance)
        {
            if (null == obj || null == target)
                return;
            Vector3 srcPos = obj.GetMovementStateInfo().GetPosition3D();
            Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
            obj.GetMovementStateInfo().SetFaceDir(Geometry.GetYRadian(new Vector2(srcPos.X, srcPos.Z), new Vector2(targetPos.X, targetPos.Z)));
            float cur_distance_z = 0;
            for (int i = 0; i < m_SectionListCopy.Count; i++) {
                cur_distance_z += (m_SectionListCopy[i].speedVect.Z * m_SectionListCopy[i].moveTime +
                                   m_SectionListCopy[i].accelVect.Z * m_SectionListCopy[i].moveTime * m_SectionListCopy[i].moveTime / 2.0f);
            }
            Vector3 target_motion = (targetPos - srcPos);
            target_motion.Y = 0;
            float target_distance_z = target_motion.Length();
            float speed_ratio = 1;
            if (cur_distance_z != 0) {
                speed_ratio = target_distance_z / cur_distance_z;
            }
            for (int i = 0; i < m_SectionListCopy.Count; i++) {
                m_SectionListCopy[i].speedVect.Z *= speed_ratio;
                m_SectionListCopy[i].accelVect.Z *= speed_ratio;
            }
        }

        private Vector3 Move(EntityInfo obj, Vector3 speed_vect, Vector3 accel_vect, float time)
        {
            Vector3 local_motion = speed_vect * time + accel_vect * time * time / 2;
            Vector3 word_target_pos;
            while (local_motion.Length() > m_MaxMoveStep) {
                Vector3 child = local_motion;
                child.Normalize();
                child *= m_MaxMoveStep;
                local_motion = local_motion - child;
                word_target_pos = Geometry.TransformPoint(m_StartPos, child, m_StartDir);
                TriggerUtil.MoveObjTo(obj, word_target_pos);
                m_StartPos = word_target_pos;
            }
            word_target_pos = Geometry.TransformPoint(m_StartPos, local_motion, m_StartDir);
            TriggerUtil.MoveObjTo(obj, word_target_pos);
            return (speed_vect + accel_vect * time);
        }

        private bool m_IsLockRotate = true;
        private List<MoveSectionInfo> m_SectionList = new List<MoveSectionInfo>();
        private List<MoveSectionInfo> m_SectionListCopy = new List<MoveSectionInfo>();
        private bool m_IsCurveMoving = false;

        

        private Vector3 m_StartPos = Vector3.Zero;
        private float m_StartDir = 0;
        private bool m_IsInited = false;
        private float m_Now;
        private static float m_MaxMoveStep = 3;
    }

    /// <summary>
    /// charge(duration,velocity,stopAtTarget[,distToTarget[,start_time]]);
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
            
                        return triger;
        }
        public override void Reset()
        {
            m_TargetChecked = false;
            m_RealDuration = 0;
            m_RealVelocity = 1;
            m_Forward = Vector3.Zero;
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            bool isTower = scene.EntityController.GetEntityType(senderObj.GfxObj) == (int)EntityTypeEnum.Tower;
            if (isTower) return false;
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
                EntityInfo targetObj = senderObj.TargetGfxObj;
                if (null != targetObj) {
                    Vector3 srcPos = obj.GetMovementStateInfo().GetPosition3D();
                    Vector3 targetPos = targetObj.GetMovementStateInfo().GetPosition3D();
                    float degree = Geometry.GetYRadian(new ScriptRuntime.Vector2(srcPos.X, srcPos.Z), new ScriptRuntime.Vector2(targetPos.X, targetPos.Z));
                    ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(targetPos.X, targetPos.Z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.X, m_Offset.Z), degree);
                    targetPos = new Vector3(newPos.X, targetPos.Y + m_Offset.Y, newPos.Y);
                    if (m_StopAtTarget == (int)StopAtTargetType.AdjustVelocity) {
                        m_RealVelocity = (long)(1000.0f * (targetPos - srcPos).Length() / m_RealDuration);
                    } else if (m_StopAtTarget == (int)StopAtTargetType.AdjustTime) {
                        m_RealDuration = (long)(1000.0f * (targetPos - targetPos).Length() / m_RealVelocity);
                    }
                    m_Forward = targetPos - srcPos;
                    m_Forward.Normalize();
                } else {
                    m_Forward = Geometry.GetRotate(new Vector3(0, 0, 1), obj.GetMovementStateInfo().GetFaceDir());
                }
            }
            if (curSectionTime < StartTime) {
                return true;
            } else if (curSectionTime <= StartTime + m_RealDuration) {
                Vector3 srcPos = obj.GetMovementStateInfo().GetPosition3D();
                float dist = TriggerUtil.ConvertToSecond(delta) * m_RealVelocity;
                Vector3 targetPos = srcPos + m_Forward * dist;
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
                StartTime = long.Parse(callData.GetParamId(4));
            }
            
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
        }

        private long m_Duration = 0;
        private float m_Velocity = 1;
        private int m_StopAtTarget = 0;
        private Vector3 m_Offset = Vector3.Zero;

        

        private bool m_TargetChecked = false;
        private long m_RealDuration = 0;
        private float m_RealVelocity = 1;
        private Vector3 m_Forward = Vector3.Zero;
    }

    /// <summary>
    /// jump(duration,height,velocity,stopAtTarget[,distToTarget[,start_time]]);
    /// </summary>
    internal class JumpTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            JumpTriger triger = new JumpTriger();
            triger.m_Duration = m_Duration;
            triger.m_Height = m_Height;
            triger.m_Velocity = m_Velocity;
            triger.m_StopAtTarget = m_StopAtTarget;
            triger.m_Offset = m_Offset;
            
            triger.m_YVelocity = m_YVelocity;
            triger.m_G = m_G;
                        return triger;
        }
        public override void Reset()
        {
            m_TargetChecked = false;
            m_RealDuration = 0;
            m_RealVelocity = 1;
            m_InitY = -1;
            m_Forward = Vector3.Zero;
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            bool isTower = scene.EntityController.GetEntityType(senderObj.GfxObj) == (int)EntityTypeEnum.Tower;
            if (isTower)
                return false;
            if (!m_TargetChecked) {
                m_TargetChecked = true;

                m_RealDuration = m_Duration;
                m_RealVelocity = m_Velocity;
                if (m_RealDuration <= 0) {
                    m_RealDuration += instance.CurSectionDuration;
                }
                if (m_RealDuration <= 0) {
                    LogSystem.Warn("jump duration is 0, skill id:{0} dsl skill id:{1}", senderObj.SkillId, instance.DslSkillId);
                    return false;
                }
                EntityInfo targetObj = senderObj.TargetGfxObj;
                if (null != targetObj) {
                    Vector3 srcPos = obj.GetMovementStateInfo().GetPosition3D();
                    Vector3 targetPos = targetObj.GetMovementStateInfo().GetPosition3D();
                    float degree = Geometry.GetYRadian(new ScriptRuntime.Vector2(srcPos.X, srcPos.Z), new ScriptRuntime.Vector2(targetPos.X, targetPos.Z));
                    ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(targetPos.X, targetPos.Z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_Offset.X, m_Offset.Z), degree);
                    targetPos = new Vector3(newPos.X, targetPos.Y + m_Offset.Y, newPos.Y);
                    if (m_StopAtTarget == (int)StopAtTargetType.AdjustVelocity) {
                        m_RealVelocity = (long)(1000.0f * (targetPos - srcPos).Length() / m_RealDuration);
                    } else if (m_StopAtTarget == (int)StopAtTargetType.AdjustTime) {
                        m_RealDuration = (long)(1000.0f * (targetPos - targetPos).Length() / m_RealVelocity);
                        CalcYVelocityAndG();
                    }
                    m_Forward = targetPos - srcPos;
                    m_Forward.Normalize();
                } else {
                    m_Forward = Geometry.GetRotate(new Vector3(0, 0, 1), obj.GetMovementStateInfo().GetFaceDir());
                }
                m_InitY = obj.GetMovementStateInfo().PositionY;
            }
            if (curSectionTime < StartTime) {
                return true;
            } else if (curSectionTime <= m_RealDuration) {
                float t = (float)(int)(curSectionTime - StartTime) / 1000.0f;
                float disty = m_YVelocity * t - m_G * t * t / 2;
                float dist = TriggerUtil.ConvertToSecond(delta) * m_RealVelocity;
                Vector3 targetPos = obj.GetMovementStateInfo().GetPosition3D() + m_Forward * dist;
                targetPos.Y = m_InitY + disty;

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
                StartTime = long.Parse(callData.GetParamId(5));
            }
            
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
        private Vector3 m_Offset = Vector3.Zero;

        

        private bool m_TargetChecked = false;
        private long m_RealDuration = 0;
        private float m_RealVelocity = 1;
        private float m_InitY = -1;
        private float m_YVelocity = 1;
        private float m_G = 10;
        private Vector3 m_Forward = Vector3.Zero;
    }
}