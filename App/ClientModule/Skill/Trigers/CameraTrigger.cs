using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// camerafollow(starttime[,remaintime]);
    /// </summary>
    internal class CameraFollowTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            CameraFollowTriger triger = new CameraFollowTriger();
            
            triger.m_RemainTime = m_RemainTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_Triggered = false;
            m_RealStartTime = StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) {
                Utility.SendMessage("GameRoot", "CameraFollowPath", null);
                return false;
            }
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (m_RemainTime > 0 && m_RemainTime > curSectionTime) {
                    Utility.SendMessage("GameRoot", "CameraFollowPath", null);
                    return false;
                }
                if (curSectionTime >= m_RealStartTime) {
                    if (!m_Triggered) {
                        m_Triggered = true;
                        Utility.SendMessage("GameRoot", "CameraFollow", senderObj.ActorId);
                    }
                    return m_RemainTime > 0;
                } else {
                    return true;
                }
            } else {
                Utility.SendMessage("GameRoot", "CameraFollowPath", null);
                return false;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num > 1) {
                m_RemainTime = long.Parse(callData.GetParamId(1));
            }
            m_RealStartTime = StartTime;
        }

        private bool m_Triggered = false;
        private float m_RemainTime = 0;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// cameralook(starttime[,remaintime]);
    /// </summary>
    internal class CameraLookTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            CameraLookTriger triger = new CameraLookTriger();
            
            triger.m_RemainTime = m_RemainTime;
            triger.m_RealStartTime = m_RealStartTime;
            return triger;
        }
        public override void Reset()
        {
            m_Triggered = false;
            m_RealStartTime = StartTime;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) {
                Utility.SendMessage("GameRoot", "CameraFollowPath", null);
                return false;
            }
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (m_RemainTime > 0 && m_RemainTime > curSectionTime) {
                    Utility.SendMessage("GameRoot", "CameraFollowPath", null);
                    return false;
                }
                if (curSectionTime >= m_RealStartTime) {
                    if (!m_Triggered) {
                        m_Triggered = true;
                        Vector3 pos = obj.transform.position;
                        Utility.SendMessage("GameRoot", "CameraLook", new float[] { pos.x, pos.y + 1.5f, pos.z });
                    }
                    return m_RemainTime > 0;
                } else {
                    return true;
                }
            } else {
                Utility.SendMessage("GameRoot", "CameraFollowPath", null);
                return false;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num > 1) {
                m_RemainTime = long.Parse(callData.GetParamId(1));
            }
            m_RealStartTime = StartTime;
        }

        private bool m_Triggered = false;
        private float m_RemainTime = 0;

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// camerafollowpath(starttime);
    /// </summary>
    internal class CameraFollowPathTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            CameraFollowPathTriger triger = new CameraFollowPathTriger();
            
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
            if (null == senderObj) {
                Utility.SendMessage("GameRoot", "CameraFollowPath", null);
                return false;
            }
            GameObject obj = senderObj.GfxObj;
            if (null != obj) {
                if (m_RealStartTime < 0) {
                    m_RealStartTime = TriggerUtil.RefixStartTime((int)StartTime, instance.LocalVariables, senderObj.ConfigData);
                }
                if (curSectionTime >= m_RealStartTime) {
                    Utility.SendMessage("GameRoot", "CameraFollowPath", senderObj.ActorId);
                    return false;
                } else {
                    return true;
                }
            } else {
                Utility.SendMessage("GameRoot", "CameraFollowPath", null);
                return false;
            }
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            m_RealStartTime = StartTime;
        }

        private long m_RealStartTime = 0;
    }
}
