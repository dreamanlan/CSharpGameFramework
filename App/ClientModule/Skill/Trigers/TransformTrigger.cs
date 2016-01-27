using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// transform(startime, bone, vector3(position), eular(rotate), relaitve_type, is_attach[, is_use_terrain_height=false][,randomrotate = Vector3.zero]);
    /// </summary>
    public class TransformTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            TransformTrigger copy = new TransformTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_BoneName = m_BoneName;
            copy.m_Postion = m_Postion;
            copy.m_Rotate = m_Rotate;
            copy.m_RelativeType = m_RelativeType;
            copy.m_IsAttach = m_IsAttach;
            copy.m_IsUseTerrainHeight = m_IsUseTerrainHeight;
            copy.m_RandomRotate = m_RandomRotate;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() >= 6) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_BoneName = callData.GetParamId(1);
                if (m_BoneName == " ") {
                    m_BoneName = "";
                }
                m_Postion = DslUtility.CalcVector3(callData.GetParam(2) as Dsl.CallData);
                m_Rotate = DslUtility.CalcEularAngles(callData.GetParam(3) as Dsl.CallData);
                m_RelativeType = callData.GetParamId(4);
                m_IsAttach = bool.Parse(callData.GetParamId(5));
            }
            if (callData.GetParamNum() >= 7) {
                m_IsUseTerrainHeight = bool.Parse(callData.GetParamId(6));
            }
            if (callData.GetParamNum() >= 8) {
                m_RandomRotate = DslUtility.CalcVector3(callData.GetParam(7) as Dsl.CallData);
            }
            m_RealStartTime = m_StartTime;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            switch (m_RelativeType) {
                case "RelativeSelf":
                    SetTransformRelativeSelf(obj);
                    break;
                case "RelativeTarget":
                    SetTransformRelativeTarget(obj, senderObj.TargetGfxObj);
                    break;
                case "RelativeWorld":
                    obj.transform.position = m_Postion;
                    obj.transform.rotation = Quaternion.Euler(m_Rotate);
                    break;
            }
            if (m_IsUseTerrainHeight) {
                Vector3 terrain_pos = TriggerUtil.GetGroundPos(obj.transform.position);
                obj.transform.position = terrain_pos;
            }
            return false;
        }

        private void SetTransformRelativeTarget(GameObject obj, GameObject target)
        {
            if (null == target) {
                return;
            }
            AttachToObject(obj, target);
        }

        private void AttachToObject(GameObject obj, GameObject owner)
        {
            Transform parent = TriggerUtil.GetChildNodeByName(owner, m_BoneName);
            if (parent == null) {
                parent = owner.transform;
            }
            obj.transform.parent = parent;
            Vector3 world_pos = parent.TransformPoint(m_Postion);
            TriggerUtil.MoveObjTo(obj, world_pos);
            obj.transform.localRotation = Quaternion.Euler(m_Rotate);
            if (!m_IsAttach) {
                obj.transform.parent = null;
            }
        }

        private void AttachToObjectForRandomRotate(GameObject obj, GameObject owner)
        {
            Transform parent = TriggerUtil.GetChildNodeByName(owner, m_BoneName);
            if (parent == null) {
                parent = owner.transform;
            }
            obj.transform.parent = parent;
            Vector3 world_pos = parent.TransformPoint(m_Postion);
            TriggerUtil.MoveObjTo(obj, world_pos);
            Vector3 resultrotate = new Vector3(
              m_Rotate.x + UnityEngine.Random.Range(m_RandomRotate.x / -2, m_RandomRotate.x / 2),
              m_Rotate.y + UnityEngine.Random.Range(m_RandomRotate.y / -2, m_RandomRotate.y / 2),
              m_Rotate.z + UnityEngine.Random.Range(m_RandomRotate.z / -2, m_RandomRotate.z / 2));
            obj.transform.localRotation = Quaternion.Euler(resultrotate);
            if (!m_IsAttach) {
                obj.transform.parent = null;
            }
        }

        private void SetTransformRelativeSelf(GameObject obj)
        {
            Vector3 new_pos = obj.transform.TransformPoint(m_Postion);
            TriggerUtil.MoveObjTo(obj, new_pos);
            obj.transform.rotation *= Quaternion.Euler(m_Rotate);
        }

        private string m_BoneName;
        private string m_RelativeType;
        private Vector3 m_Postion;
        private Vector3 m_Rotate;
        private bool m_IsAttach;
        private bool m_IsUseTerrainHeight = false;
        private Vector3 m_RandomRotate = Vector3.zero;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// teleport(starttime, offset_x, offset_y, offset_z);
    /// </summary>
    public class TeleportTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            TeleportTrigger copy = new TeleportTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_RelativeOffset = m_RelativeOffset;
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
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            GameObject targetObj = senderObj.TargetGfxObj;
            if (null != targetObj) {
                Vector3 pos = targetObj.transform.TransformPoint(m_RelativeOffset);
                TriggerUtil.MoveObjTo(obj, pos);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 4) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeOffset.x = float.Parse(callData.GetParamId(1));
                m_RelativeOffset.y = float.Parse(callData.GetParamId(2));
                m_RelativeOffset.z = float.Parse(callData.GetParamId(3));
            }
            m_RealStartTime = m_StartTime;
        }

        private Vector3 m_RelativeOffset = Vector3.zero;
        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// follow(start_time, offset_x, offset_y, offset_z, duration);
    /// </summary>
    internal class FollowTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            FollowTrigger triger = new FollowTrigger();
            triger.m_StartTime = m_StartTime;
            triger.m_RelativeOffset = m_RelativeOffset;
            triger.m_DurationTime = m_DurationTime;
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
            if (obj == null) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (m_RealStartTime + m_DurationTime < curSectionTime) {
                return false;
            }
            GameObject targetObj = senderObj.TargetGfxObj;
            if (null != targetObj) {
                Vector3 pos = targetObj.transform.TransformPoint(m_RelativeOffset);
                TriggerUtil.MoveObjTo(obj, pos);
            }
            return true;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 5) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeOffset.x = float.Parse(callData.GetParamId(1));
                m_RelativeOffset.y = float.Parse(callData.GetParamId(2));
                m_RelativeOffset.z = float.Parse(callData.GetParamId(3));
                m_DurationTime = long.Parse(callData.GetParamId(4));
            }
            m_RealStartTime = m_StartTime;
        }

        private Vector3 m_RelativeOffset = Vector3.zero;
        private long m_DurationTime = 1000;

        private long m_RealStartTime = 0;
    }
}
