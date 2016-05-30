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
        protected override ISkillTriger OnClone()
        {
            StopEffectTrigger copy = new StopEffectTrigger();
            
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            
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
            EffectManager em = instance.CustomDatas.GetData<EffectManager>();
            if (em == null) {
                return false;
            }
            em.StopEffects();
            return false;
        }
        
    }
}
