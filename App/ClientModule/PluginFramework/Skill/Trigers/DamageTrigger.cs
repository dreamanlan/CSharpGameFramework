﻿using System;
using System.Collections.Generic;
using UnityEngine;
using DotnetSkillScript;
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
                    EntityController.Instance.ImpactDamage(senderObj.TargetObjId, senderObj.ObjId, senderObj.SkillId, senderObj.Seq, IsFinal);
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
    /// <summary>
    /// addstate(state[,start_time]);
    /// </summary>
    internal class AddStateTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AddStateTriger triger = new AddStateTriger();
            triger.m_State = m_State;
            
            
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
                EntityController.Instance.RemoveState(senderObj.ObjId, m_State);
                return false;
            }
            if (curSectionTime >= StartTime) {
                EntityController.Instance.AddState(senderObj.ObjId, m_State);
                return false;
            } else {
                return true;
            }
        }
        protected override void OnInitProperties()
        {
            AddProperty("State", () => { return m_State; }, (object val) => { m_State = val as string; });
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_State = callData.GetParamId(0);
            }
            if (num > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            } else {
                StartTime = 0;
            }
            
        }
        private string m_State = string.Empty;
        
    }
    /// <summary>
    /// removestate(state[,start_time]);
    /// </summary>
    internal class RemoveStateTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            RemoveStateTriger triger = new RemoveStateTriger();
            triger.m_State = m_State;            
            
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
                EntityController.Instance.RemoveState(senderObj.ObjId, m_State);
                return false;
            }
            if (curSectionTime >= StartTime) {
                EntityController.Instance.RemoveState(senderObj.ObjId, m_State);
                return false;
            } else {
                return true;
            }
        }
        protected override void OnInitProperties()
        {
            AddProperty("State", () => { return m_State; }, (object val) => { m_State = val as string; });
        }
        protected override void Load(Dsl.FunctionData callData, SkillInstance instance)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_State = callData.GetParamId(0);
            }
            if (num > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            } else {
                StartTime = 0;
            }
            
        }
        private string m_State = string.Empty;
        
    }
    /// <summary>
    /// addshield([start_time]);
    /// </summary>
    internal class AddShieldTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AddShieldTriger triger = new AddShieldTriger();            
            
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
                EntityController.Instance.RemoveShield(senderObj.ObjId, senderObj.ConfigData, senderObj.Seq);
                return false;
            }
            if (curSectionTime >= StartTime) {
                EntityController.Instance.AddShield(senderObj.ObjId, senderObj.ConfigData, senderObj.Seq);
                return false;
            } else {
                return true;
            }
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
    /// <summary>
    /// removeshield([start_time]);
    /// </summary>
    internal class RemoveShieldTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            RemoveShieldTriger triger = new RemoveShieldTriger();
            
            
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
                EntityController.Instance.RemoveShield(senderObj.ObjId, senderObj.ConfigData, senderObj.Seq);
                return false;
            }
            if (curSectionTime >= StartTime) {
                EntityController.Instance.RemoveShield(senderObj.ObjId, senderObj.ConfigData, senderObj.Seq);
                return false;
            } else {
                return true;
            }
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
