using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using GameFramework;

namespace GameFramework.Skill.Trigers
{
    public class RotateTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            RotateTrigger copy = new RotateTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_RemainTime = m_RemainTime;
            copy.m_RotateSpeed = m_RotateSpeed;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() >= 3) {
                m_StartTime = long.Parse(callData.GetParamId(0));
                m_RemainTime = long.Parse(callData.GetParamId(1));
                m_RotateSpeed = DslUtility.CalcVector3(callData.GetParam(2) as Dsl.CallData);
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
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            if (curSectionTime > m_RealStartTime + m_RemainTime) {
                return false;
            }
            obj.transform.Rotate(m_RotateSpeed * TriggerUtil.ConvertToSecond(delta));
            return true;
        }

        private long m_RemainTime;
        private Vector3 m_RotateSpeed;

        private long m_RealStartTime = 0;
    }
}
