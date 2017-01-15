using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;
using GameFramework;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// damage(start_time);
    /// </summary>
    public class DamageTriger : AbstractSkillTriger
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) {
                return false;
            }
            if (curSectionTime >= StartTime) {
                if (senderObj.ConfigData.type != (int)SkillOrImpactType.Skill) {
                    scene.EntityController.ImpactDamage(senderObj.TargetActorId, senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
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
    /// <summary>
    /// addstate(state[,start_time]);
    /// </summary>
    public class AddStateTriger : AbstractSkillTriger
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) {
                scene.EntityController.RemoveState(senderObj.ActorId, m_State);
                return false;
            }
            if (curSectionTime >= StartTime) {
                scene.EntityController.AddState(senderObj.ActorId, m_State);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
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
    public class RemoveStateTriger : AbstractSkillTriger
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) {
                scene.EntityController.RemoveState(senderObj.ActorId, m_State);
                return false;
            }
            if (curSectionTime >= StartTime) {
                scene.EntityController.RemoveState(senderObj.ActorId, m_State);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, SkillInstance instance)
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
    public class AddShieldTriger : AbstractSkillTriger
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) {
                scene.EntityController.RemoveShield(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                return false;
            }
            if (curSectionTime >= StartTime) {
                scene.EntityController.AddShield(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
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
    /// <summary>
    /// removeshield([start_time]);
    /// </summary>
    public class RemoveShieldTriger : AbstractSkillTriger
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
            Scene scene = senderObj.Scene;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) {
                scene.EntityController.RemoveShield(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                return false;
            }
            if (curSectionTime >= StartTime) {
                scene.EntityController.RemoveShield(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
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
