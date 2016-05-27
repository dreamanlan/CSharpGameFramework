using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using GameFramework;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// damage(start_time);
    /// </summary>
    internal class DamageTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            DamageTriger triger = new DamageTriger();
            
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) {
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                if (senderObj.ConfigData.type != (int)SkillOrImpactType.Skill) {
                    EntityController.Instance.ImpactDamage(senderObj.TargetActorId, senderObj.ActorId, senderObj.SkillId, senderObj.Seq);
                }
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            m_RealStartTime = StartTime;
        }

        private long m_RealStartTime = 0;
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
            
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) {
                EntityController.Instance.RemoveState(senderObj.ActorId, m_State);
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                EntityController.Instance.AddState(senderObj.ActorId, m_State);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
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
            m_RealStartTime = StartTime;
        }

        private string m_State = string.Empty;
        private long m_RealStartTime = 0;
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
            
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) {
                EntityController.Instance.RemoveState(senderObj.ActorId, m_State);
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                EntityController.Instance.RemoveState(senderObj.ActorId, m_State);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
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
            m_RealStartTime = StartTime;
        }

        private string m_State = string.Empty;
        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// addshield([start_time]);
    /// </summary>
    internal class AddShieldTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            AddShieldTriger triger = new AddShieldTriger();
            
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) {
                EntityController.Instance.RemoveShield(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                EntityController.Instance.AddShield(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            m_RealStartTime = StartTime;
        }

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// removeshield([start_time]);
    /// </summary>
    internal class RemoveShieldTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            RemoveShieldTriger triger = new RemoveShieldTriger();
            
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) {
                EntityController.Instance.RemoveShield(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                return false;
            }
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                EntityController.Instance.RemoveShield(senderObj.ActorId, senderObj.ConfigData, senderObj.Seq);
                return false;
            } else {
                return true;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            m_RealStartTime = StartTime;
        }

        private long m_RealStartTime = 0;
    }
}
