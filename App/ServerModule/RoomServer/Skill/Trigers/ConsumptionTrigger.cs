using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;
using GameFramework;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// consume(start_time);
    /// </summary>
    internal class ConsumeTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ConsumeTriger triger = new ConsumeTriger();
            
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
                if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {

                }
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
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
