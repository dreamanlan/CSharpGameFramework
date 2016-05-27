using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    public class StorePosTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            StorePosTrigger copy = new StorePosTrigger();
            
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            m_RealStartTime = StartTime;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            Vector3 pos = obj.GetMovementStateInfo().GetPosition3D();
            instance.CustomDatas.AddData<Vector3>(pos);
            return false;
        }

        private long m_RealStartTime = 0;
    }

    public class RestorePosTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            RestorePosTrigger copy = new RestorePosTrigger();
            
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            m_RealStartTime = StartTime;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            EntityInfo obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            Vector3 old_pos = instance.CustomDatas.GetData<Vector3>();
            if (old_pos != null) {
                obj.GetMovementStateInfo().SetPosition(old_pos);
            }
            return false;
        }

        private long m_RealStartTime = 0;
    }
}
