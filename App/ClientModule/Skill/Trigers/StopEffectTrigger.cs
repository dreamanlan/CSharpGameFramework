using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// stopeffect(starttime);
    /// </summary>
    public class StopEffectTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            StopEffectTrigger copy = new StopEffectTrigger();
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
                m_RealStartTime = TriggerUtil.RefixStartTime((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            EffectManager em = instance.CustomDatas.GetData<EffectManager>();
            if (em == null) {
                return false;
            }
            em.StopEffects();
            return false;
        }

        private long m_RealStartTime = 0;
    }
}
