using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using ScriptableFramework;

namespace ScriptableFramework.Skill.Trigers
{
    /// <summary>
    /// damage([start_time[,isFinal]]);
    /// </summary>
    internal class DamageTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            DamageTriger triger = new DamageTriger();
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
            if (null == obj) {
                return false;
            }
            if (curSectionTime >= StartTime) {
                if (senderObj.ConfigData.type != (int)SkillOrImpactType.Skill) {
                    // Settle through the ImpactSystem (unified damage settlement). The impact was
                    // added on the target by SendImpact/TrackSendImpact and carries the skill's
                    // DamageData; here we only resolve the entities and hand over for settlement.
                    EntityViewModel view = EntityController.Instance.GetEntityViewById(senderObj.ObjId);
                    EntityViewModel srcView = EntityController.Instance.GetEntityViewById(senderObj.TargetObjId);
                    if (null != view && null != view.Entity && !view.Entity.IsDeadSkillCasting() &&
                        null != srcView && null != srcView.Entity) {
                        ImpactSystem.Instance.SettleSkillDamage(view.Entity, srcView.Entity, senderObj.SkillId, senderObj.Seq);
                    }
                }
                return false;
            } else {
                return true;
            }
        }
        protected override void OnInitProperties()
        {
            AddProperty("IsFinal", () => { return IsFinal; }, (object val) => { IsFinal = (bool)Convert.ChangeType(val, typeof(bool)); });
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
        {
            instance.AddDamageForInit(this);
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            if (num > 1) {
                IsFinal = callData.GetParamId(1) == "true";
            } else {
                IsFinal = true;
            }
        }     
    }
}
