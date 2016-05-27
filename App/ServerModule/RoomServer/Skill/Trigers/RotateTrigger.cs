using System;
using System.Collections.Generic;
using ScriptRuntime;
using SkillSystem;
using GameFramework;

namespace GameFramework.Skill.Trigers
{
    public class RotateTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            RotateTrigger copy = new RotateTrigger();
            
            copy.m_RemainTime = m_RemainTime;
            copy.m_RotateSpeed = m_RotateSpeed;
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
        }

        public override void Reset()
        {
            m_RealStartTime = StartTime;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() >= 3) {
                StartTime = long.Parse(callData.GetParamId(0));
                m_RemainTime = long.Parse(callData.GetParamId(1));
                m_RotateSpeed = DslUtility.CalcVector3(callData.GetParam(2) as Dsl.CallData);
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
            if (curSectionTime > m_RealStartTime + m_RemainTime) {
                return false;
            }
            float dir = obj.GetMovementStateInfo().GetFaceDir();
            dir = (dir + Helper.DegreeToRadian(m_RotateSpeed.Y) * TriggerUtil.ConvertToSecond(delta)) % (float)(2 * Math.PI);
            obj.GetMovementStateInfo().SetFaceDir(dir);
            return true;
        }

        private long m_RemainTime;
        private Vector3 m_RotateSpeed;

        private long m_RealStartTime = 0;
    }
}
