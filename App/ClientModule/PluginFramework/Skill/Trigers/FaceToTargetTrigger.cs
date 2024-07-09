using System;
using System.Collections.Generic;
using UnityEngine;
using DotnetSkillScript;

namespace ScriptableFramework.Skill.Trigers
{
    internal class TargetManager
    {
        public HashSet<int> Targets
        {
            get { return m_Targets; }
        }
        public void Add(int targetId)
        {
            Targets.Add(targetId);
        }
        public bool Exist(int targetId)
        {
            return Targets.Contains(targetId);
        }
        public void Clear()
        {
            Targets.Clear();
        }
        private HashSet<int> m_Targets = new HashSet<int>();
    }
    /// <summary>
    /// selecttarget(type[, start_time]);
    /// </summary>
    internal class SelectTargetTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            SelectTargetTrigger copy = new SelectTargetTrigger();
            copy.m_Type = m_Type;
            
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Type = callData.GetParamId(0);
            }
            if (num > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            } else {
                StartTime = 0;
            }
            
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
            Vector3 pos = obj.transform.position;
            TargetManager mgr = instance.CustomDatas.GetData<TargetManager>();
            if (null == mgr) {
                mgr = new TargetManager();
                instance.CustomDatas.AddData(mgr);
            }
            int targetId = EntityController.Instance.SelectTargetForSkill(m_Type, senderObj.ObjId, senderObj.ConfigData, senderObj.Seq, mgr.Targets);
            if (targetId > 0) {
                mgr.Add(targetId);
                GameObject target = EntityController.Instance.GetGameObject(targetId);
                senderObj.TargetObjId = targetId;
                senderObj.TargetGfxObj = target;
            }
            return false;
        }
        private string m_Type = "minhp";
        
    }
    /// <summary>
    /// facetotarget(starttime,remaintime[,rotate,selecttype]);
    /// </summary>
    internal class FaceToTargetTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            FaceToTargetTrigger copy = new FaceToTargetTrigger();
            
            copy.m_RemainTime = m_RemainTime;
            copy.m_IsHaveRotateSpeed = m_IsHaveRotateSpeed;
            copy.m_RotateSpeed = m_RotateSpeed;
            copy.m_SelectTargetType = m_SelectTargetType;
            return copy;
        }
        public override void Reset()
        {
            if (m_IsExecuted && m_RemainTime > 0 && null != m_Myself && null != m_Target) {
                TriggerUtil.Lookat(m_Myself, m_Target.transform.position);
            }
            m_IsExecuted = false;
            m_Myself = null;
            m_Target = null;
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num >= 2) {
                m_RemainTime = long.Parse(callData.GetParamId(1));
            }
            if (num >= 3) {
                m_IsHaveRotateSpeed = true;
                m_RotateSpeed = Vector3.zero;
                m_RotateSpeed.y = float.Parse(callData.GetParamId(2));
            }
            if (num >= 4) {
                m_SelectTargetType = callData.GetParamId(3);
            }
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            m_Myself = senderObj.GfxObj;
            if (null == m_Myself) {
                return false;
            }
            if (curSectionTime < StartTime) {
                return true;
            }
            if (m_IsExecuted && curSectionTime > (StartTime + m_RemainTime)) {
                return false;
            }
            Vector3 pos = m_Myself.transform.position;
            m_Target = senderObj.TargetGfxObj;
            if (!m_IsExecuted && (null == m_Target && !string.IsNullOrEmpty(m_SelectTargetType))) {
                TargetManager mgr = instance.CustomDatas.GetData<TargetManager>();
                if (null == mgr) {
                    mgr = new TargetManager();
                    instance.CustomDatas.AddData(mgr);
                }
                int targetId = EntityController.Instance.SelectTargetForSkill(m_SelectTargetType, senderObj.ObjId, senderObj.ConfigData, senderObj.Seq, mgr.Targets);
                if (targetId > 0) {
                    mgr.Add(targetId);
                    m_Target = EntityController.Instance.GetGameObject(targetId);
                    senderObj.TargetObjId = targetId;
                    senderObj.TargetGfxObj = m_Target;
                }
            }
            if (null != m_Target) {
                m_IsExecuted = true;
                if (!m_IsHaveRotateSpeed || m_RotateSpeed.y == 0) {
                    TriggerUtil.Lookat(m_Myself, m_Target.transform.position);
                } else {
                    float maxRotateDelta = m_RotateSpeed.y * TriggerUtil.ConvertToSecond(delta);
                    TriggerUtil.Lookat(m_Myself, m_Target.transform.position, maxRotateDelta);
                }
            }
            return true;
        }

        private long m_RemainTime = 0;
        private bool m_IsHaveRotateSpeed = false;
        private Vector3 m_RotateSpeed = Vector3.zero;
        private string m_SelectTargetType = string.Empty;
        private bool m_IsExecuted = false;

        private GameObject m_Myself = null;
        private GameObject m_Target = null;
    }
    /// <summary>
    /// cleartargets(starttime);
    /// </summary>
    internal class ClearTargetsTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ClearTargetsTrigger copy = new ClearTargetsTrigger();
            
            
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
            TargetManager mgr = instance.CustomDatas.GetData<TargetManager>();
            if (null != mgr) {
                mgr.Clear();
            }
            return false;
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            
        }
        
    }
}
