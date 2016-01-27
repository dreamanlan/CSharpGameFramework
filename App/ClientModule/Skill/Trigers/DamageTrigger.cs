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
        public override ISkillTriger Clone()
        {
            DamageTriger triger = new DamageTriger();
            triger.m_StartTime = m_StartTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
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
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
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
                m_StartTime = long.Parse(callData.GetParamId(0));
            } else {
                m_StartTime = 0;
            }
            m_RealStartTime = m_StartTime;
        }

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// addstate(state[,start_time]);
    /// </summary>
    internal class AddStateTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            AddStateTriger triger = new AddStateTriger();
            triger.m_State = m_State;
            triger.m_StartTime = m_StartTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
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
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
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
                m_StartTime = long.Parse(callData.GetParamId(1));
            } else {
                m_StartTime = 0;
            }
            m_RealStartTime = m_StartTime;
        }

        private string m_State = string.Empty;
        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// removestate(state[,start_time]);
    /// </summary>
    internal class RemoveStateTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            RemoveStateTriger triger = new RemoveStateTriger();
            triger.m_State = m_State;
            triger.m_StartTime = m_StartTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
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
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
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
                m_StartTime = long.Parse(callData.GetParamId(1));
            } else {
                m_StartTime = 0;
            }
            m_RealStartTime = m_StartTime;
        }

        private string m_State = string.Empty;
        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// addshield([start_time]);
    /// </summary>
    internal class AddShieldTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            AddShieldTriger triger = new AddShieldTriger();
            triger.m_StartTime = m_StartTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
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
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
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
                m_StartTime = long.Parse(callData.GetParamId(0));
            } else {
                m_StartTime = 0;
            }
            m_RealStartTime = m_StartTime;
        }

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// removeshield([start_time]);
    /// </summary>
    internal class RemoveShieldTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            RemoveShieldTriger triger = new RemoveShieldTriger();
            triger.m_StartTime = m_StartTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_RealStartTime = m_StartTime;
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
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
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
                m_StartTime = long.Parse(callData.GetParamId(0));
            } else {
                m_StartTime = 0;
            }
            m_RealStartTime = m_StartTime;
        }

        private long m_RealStartTime = 0;
    }
}
