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
        protected override ISkillTriger OnClone()
        {
            TransformTrigger copy = new TransformTrigger();
            
            copy.m_BoneName = m_BoneName;
            copy.m_Postion = m_Postion;
            copy.m_Rotate = m_Rotate;
            copy.m_RelativeType = m_RelativeType;
            copy.m_IsAttach = m_IsAttach;
            copy.m_IsUseTerrainHeight = m_IsUseTerrainHeight;
            copy.m_RandomRotate = m_RandomRotate;
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            if (callData.GetParamNum() >= 6) {
                StartTime = long.Parse(callData.GetParamId(0));
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
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            GameObject target = senderObj.TargetGfxObj;
            if (null != senderObj.TrackEffectObj) {
                obj = senderObj.TrackEffectObj;
                target = senderObj.GfxObj;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            switch (m_RelativeType) {
                case "RelativeSelf":
                    SetTransformRelativeSelf(obj);
                    break;
                case "RelativeTarget":
                    SetTransformRelativeTarget(obj, target);
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
        
    }
    /// <summary>
    /// teleport(starttime, offset_x, offset_y, offset_z[, isForRoundMove]);
    /// </summary>
    public class TeleportTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            TeleportTrigger copy = new TeleportTrigger();
            
            copy.m_RelativeOffset = m_RelativeOffset;
            
            copy.m_IsForRoundMove = m_IsForRoundMove;
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
            GameObject targetObj = senderObj.TargetGfxObj;
            if (null != senderObj.TrackEffectObj) {
                obj = senderObj.TrackEffectObj;
                targetObj = senderObj.GfxObj;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            if (m_IsForRoundMove) {
                Vector3 srcPos = obj.transform.position;
                Vector3 targetPos = Vector3.zero;
                if (null != targetObj) {
                    targetPos = targetObj.transform.position;
                }
                TriggerUtil.GetSkillStartPosition(srcPos, senderObj.ConfigData, instance, senderObj.ActorId, senderObj.TargetActorId, ref targetPos);
                if (targetPos.sqrMagnitude > Geometry.c_FloatPrecision) {
                    float angle = Geometry.GetYRadian(new ScriptRuntime.Vector2(srcPos.x, srcPos.z), new ScriptRuntime.Vector2(targetPos.x, targetPos.z));
                    ScriptRuntime.Vector2 newPos = new ScriptRuntime.Vector2(targetPos.x, targetPos.z) + Geometry.GetRotate(new ScriptRuntime.Vector2(m_RelativeOffset.x, m_RelativeOffset.z), angle);
                    targetPos = new Vector3(newPos.X, srcPos.y + m_RelativeOffset.y, newPos.Y);
                    TriggerUtil.MoveObjTo(obj, targetPos);
                }
            } else if (null != targetObj) {
                //Vector3 pos = targetObj.transform.TransformPoint(m_RelativeOffset);
                Vector3 srcPos = obj.transform.position;
                Vector3 pos = Vector3.zero;
                if (null != targetObj)
                {
                    pos = targetObj.transform.position;
                }
                TriggerUtil.GetSkillStartPosition(srcPos, senderObj.ConfigData, instance, senderObj.ActorId, senderObj.TargetActorId, ref pos);
                pos.y = srcPos.y;
                TriggerUtil.MoveObjTo(obj, pos);
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 4) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeOffset.x = float.Parse(callData.GetParamId(1));
                m_RelativeOffset.y = float.Parse(callData.GetParamId(2));
                m_RelativeOffset.z = float.Parse(callData.GetParamId(3));
            }
            if (num >= 5) {
                m_IsForRoundMove = callData.GetParamId(4) == "true";
            }
            
        }
        private Vector3 m_RelativeOffset = Vector3.zero;
        
        private bool m_IsForRoundMove = false;
    }
    /// <summary>
    /// follow(start_time, offset_x, offset_y, offset_z, duration);
    /// </summary>
    internal class FollowTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            FollowTrigger triger = new FollowTrigger();
            
            triger.m_RelativeOffset = m_RelativeOffset;
            triger.m_DurationTime = m_DurationTime;
            
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
            if (obj == null) {
                return false;
            }
            GameObject targetObj = senderObj.TargetGfxObj;
            if (null != senderObj.TrackEffectObj) {
                obj = senderObj.TrackEffectObj;
                targetObj = senderObj.GfxObj;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            if (StartTime + m_DurationTime < curSectionTime) {
                return false;
            }
            if (null != targetObj) {
                Vector3 pos = targetObj.transform.TransformPoint(m_RelativeOffset);
                TriggerUtil.MoveObjTo(obj, pos);
            }
            return true;
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 5) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RelativeOffset.x = float.Parse(callData.GetParamId(1));
                m_RelativeOffset.y = float.Parse(callData.GetParamId(2));
                m_RelativeOffset.z = float.Parse(callData.GetParamId(3));
                m_DurationTime = long.Parse(callData.GetParamId(4));
            }
            
        }
        private Vector3 m_RelativeOffset = Vector3.zero;
        private long m_DurationTime = 1000;
        
    }
}
