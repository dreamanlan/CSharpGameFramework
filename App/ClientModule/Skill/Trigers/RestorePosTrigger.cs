using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    public class StorePosTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            StorePosTrigger copy = new StorePosTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() >= 1) {
                m_StartTime = long.Parse(callData.GetParamId(0));
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
            Vector3 pos = obj.transform.position;
            instance.CustomDatas.AddData<Vector3>(pos);
            return false;
        }

        private long m_RealStartTime = 0;
    }

    public class RestorePosTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            RestorePosTrigger copy = new RestorePosTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() >= 1) {
                m_StartTime = long.Parse(callData.GetParamId(0));
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
            Vector3 old_pos = instance.CustomDatas.GetData<Vector3>();
            if (old_pos != null) {
                obj.transform.position = old_pos;
            }
            return false;
        }

        private long m_RealStartTime = 0;
    }
}
