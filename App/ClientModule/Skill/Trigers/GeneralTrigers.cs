using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using GameFramework;
using GameFramework.Story;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// timescale(start_time, scale[, end_time]);
    /// </summary>
    internal class TimeScaleTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            TimeScaleTriger triger = new TimeScaleTriger();
            triger.m_TimeScale = m_TimeScale;
            triger.m_StartTime = m_StartTime;
            triger.m_EndTime = m_EndTime;
            triger.m_FixedDeltaTime = m_FixedDeltaTime;
            triger.m_RealStartTime = m_RealStartTime;
            triger.m_RealTimeScale = m_RealTimeScale;
            triger.m_RealEndTime = m_RealEndTime;
            return triger;
        }

        public override void Reset()
        {
            if (m_IsSet && !m_IsReset) {
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = m_FixedDeltaTime;
            }
            m_IsSet = false;
            m_IsReset = false;
            m_RealStartTime = m_StartTime;
            m_RealTimeScale = m_TimeScale;
            m_RealEndTime = m_EndTime;
        }

        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (!m_IsSet) {
                if (curSectionTime >= m_RealStartTime) {
                    m_IsSet = true;
                    m_UnityRealStartTime = Time.realtimeSinceStartup;
                    m_UnityRealTimeToEnd = Time.realtimeSinceStartup + (m_RealEndTime - m_RealStartTime)/1000.0f;
                    Time.timeScale = m_RealTimeScale;
                    Time.fixedDeltaTime = m_FixedDeltaTime * m_RealTimeScale;
                }
            }

            if (m_IsSet) {
                float t = (Time.realtimeSinceStartup - m_UnityRealStartTime)/(m_UnityRealTimeToEnd - m_UnityRealStartTime);
                const float m = 0.7f;
                if (t > m) {
                    t = (t - m) / (1 - m);
                    float scale = Mathf.Lerp(m_RealTimeScale, 1, t);
                    Time.timeScale = scale;
                    Time.fixedDeltaTime = m_FixedDeltaTime * scale;
                }
            }

            if (m_IsSet && Time.realtimeSinceStartup > m_UnityRealTimeToEnd) {
                if (!m_IsReset) {
                    m_IsReset = true;
                    Time.timeScale = 1.0f;
                    Time.fixedDeltaTime = m_FixedDeltaTime;
                }
                return false;
            }

            return true;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            if (callData.GetParamNum() > 0) {
                m_StartTime = long.Parse(callData.GetParamId(0));
            }
            if (callData.GetParamNum() > 1) {
                m_TimeScale = float.Parse(callData.GetParamId(1));
            }
            if (callData.GetParamNum() > 2) {
                m_EndTime = long.Parse(callData.GetParamId(2));
            }
            try {
                m_FixedDeltaTime = GetFixedDeltaTime();
            } catch {
                m_FixedDeltaTime = 0.1f;
            }
            m_RealStartTime = m_StartTime;
            m_RealEndTime = m_EndTime;
            m_RealTimeScale = m_TimeScale;
        }

        private float GetFixedDeltaTime()
        {
            //下面函数是引用C++库里的函数，不能直接捕获异常，必须封装一层C#函数再捕获。
            return Time.fixedDeltaTime;
        }

        private float m_TimeScale = 1.0f;
        private float m_EndTime = 0;
        private float m_FixedDeltaTime = 0.1f;

        private long m_RealStartTime = 0;
        private float m_RealEndTime = 0;
        private float m_RealTimeScale = 1.0f;

        private bool m_IsSet = false;
        private bool m_IsReset = false;
        private float m_UnityRealStartTime = 0;
        private float m_UnityRealTimeToEnd = 0;
    }

    /// <summary>
    /// bornfinish(start_time);
    /// </summary>
    internal class BornFinishTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            BornFinishTriger triger = new BornFinishTriger();
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
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                EntityController.Instance.BornFinish(senderObj.ActorId);
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
    /// deadfinish(start_time);
    /// </summary>
    internal class DeadFinishTriger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            DeadFinishTriger triger = new DeadFinishTriger();
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
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime >= m_RealStartTime) {
                EntityController.Instance.DeadFinish(senderObj.ActorId);
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
    /// sendstorymessage(start_time,msg,arg1,arg2,arg3,...);
    /// </summary>
    public class SendStoryMessageTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            SendStoryMessageTrigger copy = new SendStoryMessageTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_Msg = m_Msg;
            copy.m_Args.AddRange(m_Args);
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
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
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            List<object> args = new List<object>();
            args.Add(senderObj);
            for (int i = 0; i < m_Args.Count; ++i) {
                args.Add(m_Args[i]);
            }
            GfxStorySystem.Instance.SendMessage(m_Msg, args.ToArray());
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_StartTime = int.Parse(callData.GetParamId(0));
                m_Msg = callData.GetParamId(1);
            }
            for (int i = 2; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            m_RealStartTime = m_StartTime;
        }

        private string m_Msg = string.Empty;
        private List<string> m_Args = new List<string>();

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// sendgfxmessage(start_time,objname,msg,arg1,arg2,arg3,...);
    /// </summary>
    public class SendGfxMessageTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            SendGfxMessageTrigger copy = new SendGfxMessageTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_Object = m_Object;
            copy.m_Msg = m_Msg;
            copy.m_Args.AddRange(m_Args);
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
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
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            List<object> arglist = new List<object>();
            arglist.Add(senderObj);
            for (int i = 0; i < m_Args.Count; ++i) {
                arglist.Add(m_Args[i]);
            }
            object[] args = arglist.ToArray();
            if (args.Length == 0)
                Utility.SendMessage(m_Object, m_Msg, null);
            else if (args.Length == 1)
                Utility.SendMessage(m_Object, m_Msg, args[0]);
            else
                Utility.SendMessage(m_Object, m_Msg, args);
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_StartTime = int.Parse(callData.GetParamId(0));
                m_Object = callData.GetParamId(1);
                m_Msg = callData.GetParamId(2);
            }
            for (int i = 3; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            m_RealStartTime = m_StartTime;
        }

        private string m_Object = string.Empty;
        private string m_Msg = string.Empty;
        private List<string> m_Args = new List<string>();

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// sendgfxmessagewithtag(start_time,tag,msg,arg1,arg2,arg3,...);
    /// </summary>
    public class SendGfxMessageWithTagTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            SendGfxMessageWithTagTrigger copy = new SendGfxMessageWithTagTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_Tag = m_Tag;
            copy.m_Msg = m_Msg;
            copy.m_Args.AddRange(m_Args);
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
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
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            List<object> arglist = new List<object>();
            arglist.Add(senderObj);
            for (int i = 0; i < m_Args.Count; ++i) {
                arglist.Add(m_Args[i]);
            }
            object[] args = arglist.ToArray();
            if (args.Length == 0)
                Utility.SendMessageWithTag(m_Tag, m_Msg, null);
            else if (args.Length == 1)
                Utility.SendMessageWithTag(m_Tag, m_Msg, args[0]);
            else
                Utility.SendMessageWithTag(m_Tag, m_Msg, args);
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_StartTime = int.Parse(callData.GetParamId(0));
                m_Tag = callData.GetParamId(1);
                m_Msg = callData.GetParamId(2);
            }
            for (int i = 3; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            m_RealStartTime = m_StartTime;
        }

        private string m_Tag = string.Empty;
        private string m_Msg = string.Empty;
        private List<string> m_Args = new List<string>();

        private long m_RealStartTime = 0;
    }
    /// <summary>
    /// publishgfxevent(start_time,event,group,arg1,arg2,arg3,...);
    /// </summary>
    public class PublishGfxEventTrigger : AbstractSkillTriger
    {
        public override ISkillTriger Clone()
        {
            PublishGfxEventTrigger copy = new PublishGfxEventTrigger();
            copy.m_StartTime = m_StartTime;
            copy.m_Event = m_Event;
            copy.m_Group = m_Group;
            copy.m_Args.AddRange(m_Args);
            copy.m_RealStartTime = m_RealStartTime;
            return copy;
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
            if (null == obj) return false;
            if (m_RealStartTime < 0) {
                m_RealStartTime = TriggerUtil.RefixStartTimeByConfig((int)m_StartTime, instance.LocalVariables, senderObj.ConfigData);
            }
            if (curSectionTime < m_RealStartTime) {
                return true;
            }
            List<object> arglist = new List<object>();
            arglist.Add(senderObj);
            for (int i = 0; i < m_Args.Count; ++i) {
                arglist.Add(m_Args[i]);
            }
            object[] args = arglist.ToArray();
            Utility.EventSystem.Publish(m_Event, m_Group, args);
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_StartTime = int.Parse(callData.GetParamId(0));
                m_Event = callData.GetParamId(1);
                m_Group = callData.GetParamId(2);
            }
            for (int i = 3; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            m_RealStartTime = m_StartTime;
        }

        private string m_Event = string.Empty;
        private string m_Group = string.Empty;
        private List<string> m_Args = new List<string>();

        private long m_RealStartTime = 0;
    }
}
