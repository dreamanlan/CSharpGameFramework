using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// transform(startime, bone, vector3(position) or objpath, eular(rotate) or objpath, relaitve_type, is_attach[, is_use_terrain_height=false][,randomrotate = Vector3.zero]);
    /// </summary>
    internal class TransformTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            TransformTrigger copy = new TransformTrigger();
            
            copy.m_BoneName = m_BoneName;
            copy.m_Postion = m_Postion;
            copy.m_PosObjPath = m_PosObjPath;
            copy.m_Rotate = m_Rotate;
            copy.m_RotateObjPath = m_RotateObjPath;
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
            if (callData.GetParamNum() >= 5) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_BoneName = callData.GetParamId(1);
                if (m_BoneName == " ") {
                    m_BoneName = "";
                }
                var param2 = callData.GetParam(2);
                m_PosObjPath = param2.GetId();
                var cd2 = param2 as Dsl.CallData;
                if (null != cd2 && m_PosObjPath == "vector3") {
                    m_Postion = DslUtility.CalcVector3(cd2);
                }
                var param3 = callData.GetParam(3);
                m_RotateObjPath = param3.GetId();
                var cd3 = param3 as Dsl.CallData;
                if (null != cd3 && m_RotateObjPath == "eular") {
                    m_Rotate = DslUtility.CalcEularAngles(cd3);
                }
                m_RelativeType = callData.GetParamId(4);
            }
            if (callData.GetParamNum() >= 6) {
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
                    if (m_PosObjPath == "vector3") {
                        obj.transform.position = m_Postion;
                    } else {
                        var tobj = GameObject.Find(m_PosObjPath);
                        if (null != tobj) {
                            obj.transform.position = tobj.transform.position;
                        }
                    }
                    if (m_RotateObjPath == "eular") {
                        obj.transform.rotation = Quaternion.Euler(m_Rotate);
                    } else {
                        var tobj = GameObject.Find(m_RotateObjPath);
                        if (null != tobj) {
                            obj.transform.rotation = tobj.transform.rotation;
                        }
                    }
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
            obj.transform.SetParent(parent);
            Vector3 world_pos = parent.TransformPoint(m_Postion);
            TriggerUtil.MoveObjTo(obj, world_pos);
            obj.transform.localRotation = Quaternion.Euler(m_Rotate);
            if (!m_IsAttach) {
                obj.transform.SetParent(null);
            }
        }
        private void AttachToObjectForRandomRotate(GameObject obj, GameObject owner)
        {
            Transform parent = TriggerUtil.GetChildNodeByName(owner, m_BoneName);
            if (parent == null) {
                parent = owner.transform;
            }
            obj.transform.SetParent(parent);
            Vector3 world_pos = parent.TransformPoint(m_Postion);
            TriggerUtil.MoveObjTo(obj, world_pos);
            Vector3 resultrotate = new Vector3(
              m_Rotate.x + UnityEngine.Random.Range(m_RandomRotate.x / -2, m_RandomRotate.x / 2),
              m_Rotate.y + UnityEngine.Random.Range(m_RandomRotate.y / -2, m_RandomRotate.y / 2),
              m_Rotate.z + UnityEngine.Random.Range(m_RandomRotate.z / -2, m_RandomRotate.z / 2));
            obj.transform.localRotation = Quaternion.Euler(resultrotate);
            if (!m_IsAttach) {
                obj.transform.SetParent(null);
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
        private string m_PosObjPath;
        private Vector3 m_Rotate;
        private string m_RotateObjPath;
        private bool m_IsAttach;
        private bool m_IsUseTerrainHeight = false;
        private Vector3 m_RandomRotate = Vector3.zero;
        
    }
    /// <summary>
    /// teleport([starttime[, vector3(offset_x, offset_y, offset_z) or objpath]]);
    /// </summary>
    internal class TeleportTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            TeleportTrigger copy = new TeleportTrigger();            
            copy.m_RelativeOffset = m_RelativeOffset;
            copy.m_ObjPath = m_ObjPath;
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
            if (null != targetObj && m_ObjPath == "vector3") {
                Vector3 pos = targetObj.transform.TransformPoint(m_RelativeOffset);
                Vector3 srcPos = obj.transform.position;
                pos.y = srcPos.y;
                TriggerUtil.MoveObjTo(obj, pos);
            } else {
                var tobj = GameObject.Find(m_ObjPath);
                if (null != tobj) {
                    TriggerUtil.MoveObjTo(obj, tobj.transform.position);
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num >= 2) {
                var param = callData.GetParam(1);
                m_ObjPath = param.GetId();
                var cd = param as Dsl.CallData;
                if (null != cd && m_ObjPath == "vector3") {
                    m_RelativeOffset = DslUtility.CalcVector3(cd);
                }
            }
        }

        private Vector3 m_RelativeOffset = Vector3.zero;
        private string m_ObjPath = string.Empty;
    }
    /// <summary>
    /// follow(start_time, vector3(offset_x, offset_y, offset_z) or objpath, duration);
    /// </summary>
    internal class FollowTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            FollowTrigger triger = new FollowTrigger();            
            triger.m_RelativeOffset = m_RelativeOffset;
            triger.m_ObjPath = m_ObjPath;
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
            if (null != targetObj && m_ObjPath == "vector3") {
                Vector3 pos = targetObj.transform.TransformPoint(m_RelativeOffset);
                TriggerUtil.MoveObjTo(obj, pos);
            } else {
                var tobj = GameObject.Find(m_ObjPath);
                if (null != tobj) {
                    TriggerUtil.MoveObjTo(obj, tobj.transform.position);
                }
            }
            return true;
        }
        protected override void Load(Dsl.CallData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                StartTime = long.Parse(callData.GetParamId(0));
                var param = callData.GetParam(1);
                m_ObjPath = param.GetId();
                var cd = param as Dsl.CallData;
                if (null != cd && m_ObjPath == "vector3") {
                    m_RelativeOffset = DslUtility.CalcVector3(cd);
                }
                m_DurationTime = long.Parse(callData.GetParamId(2));
            }
        }

        private Vector3 m_RelativeOffset = Vector3.zero;
        private string m_ObjPath = string.Empty;
        private long m_DurationTime = 1000;
    }
}
