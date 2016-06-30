using System;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
using GameFramework;
using GameFramework.Story;

namespace GameFramework.Skill.Trigers
{
    /// <summary>
    /// timescale(start_time, scale[, duration]);
    /// </summary>
    internal class TimeScaleTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            TimeScaleTriger triger = new TimeScaleTriger();
            triger.m_TimeScale = m_TimeScale;
            
            triger.m_Duration = m_Duration;
            triger.m_FixedDeltaTime = m_FixedDeltaTime;
            
            triger.m_RealTimeScale = m_RealTimeScale;
            triger.m_RealDuration = m_RealDuration;
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
            
            m_RealTimeScale = m_TimeScale;
            m_RealDuration = m_Duration;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (!m_IsSet) {
                if (curSectionTime >= StartTime) {
                    m_IsSet = true;
                    float rate = m_RealTimeScale;
                    if (rate <= Geometry.c_FloatPrecision)
                        rate = 0.01f;
                    m_UnityRealStartTime = Time.realtimeSinceStartup;
                    m_UnityRealTimeToEnd = Time.realtimeSinceStartup + m_RealDuration / rate / 1000.0f;
                    Time.timeScale = m_RealTimeScale;
                    Time.fixedDeltaTime = m_FixedDeltaTime * m_RealTimeScale;
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
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (callData.GetParamNum() > 1) {
                m_TimeScale = float.Parse(callData.GetParamId(1));
            }
            if (callData.GetParamNum() > 2) {
                m_Duration = long.Parse(callData.GetParamId(2));
            }
            try {
                m_FixedDeltaTime = GetFixedDeltaTime();
            } catch {
                m_FixedDeltaTime = 0.1f;
            }
            
            m_RealDuration = m_Duration;
            m_RealTimeScale = m_TimeScale;
        }

        protected override void OnInitProperties()
        {
            AddProperty("TimeScale", () => { return m_RealTimeScale; }, (object val) => { m_RealTimeScale = (float)Convert.ChangeType(val,typeof(float)); });
            AddProperty("Duration", () => { return m_RealDuration; }, (object val) => { m_RealDuration = (float)Convert.ChangeType(val, typeof(float)); });
        }

        private float GetFixedDeltaTime()
        {
            //下面函数是引用C++库里的函数，不能直接捕获异常，必须封装一层C#函数再捕获。
            return Time.fixedDeltaTime;
        }
        private float m_TimeScale = 1.0f;
        private float m_Duration = 0;
        private float m_FixedDeltaTime = 0.1f;
        
        private float m_RealDuration = 0;
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
        protected override ISkillTriger OnClone()
        {
            BornFinishTriger triger = new BornFinishTriger();
            
            
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
            if (null == obj) return false;
            if (curSectionTime >= StartTime) {
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
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            
        }
        
    }
    /// <summary>
    /// deadfinish(start_time);
    /// </summary>
    internal class DeadFinishTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            DeadFinishTriger triger = new DeadFinishTriger();
            
            
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
            if (null == obj) return false;
            if (curSectionTime >= StartTime) {
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
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
            
        }
        
    }
    /// <summary>
    /// sendstorymessage(start_time,msg,arg1,arg2,arg3,...);
    /// </summary>
    public class SendStoryMessageTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            SendStoryMessageTrigger copy = new SendStoryMessageTrigger();
            
            copy.m_Msg = m_Msg;
            copy.m_Args.AddRange(m_Args);
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
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
                StartTime = int.Parse(callData.GetParamId(0));
                m_Msg = callData.GetParamId(1);
            }
            for (int i = 2; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            
        }
        private string m_Msg = string.Empty;
        private List<string> m_Args = new List<string>();
        
    }
    /// <summary>
    /// sendgfxmessage(start_time,objname,msg,arg1,arg2,arg3,...);
    /// </summary>
    public class SendGfxMessageTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            SendGfxMessageTrigger copy = new SendGfxMessageTrigger();
            
            copy.m_Object = m_Object;
            copy.m_Msg = m_Msg;
            copy.m_Args.AddRange(m_Args);
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
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
                StartTime = int.Parse(callData.GetParamId(0));
                m_Object = callData.GetParamId(1);
                m_Msg = callData.GetParamId(2);
            }
            for (int i = 3; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            
        }
        private string m_Object = string.Empty;
        private string m_Msg = string.Empty;
        private List<string> m_Args = new List<string>();
        
    }
    /// <summary>
    /// sendgfxmessagewithtag(start_time,tag,msg,arg1,arg2,arg3,...);
    /// </summary>
    public class SendGfxMessageWithTagTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            SendGfxMessageWithTagTrigger copy = new SendGfxMessageWithTagTrigger();
            
            copy.m_Tag = m_Tag;
            copy.m_Msg = m_Msg;
            copy.m_Args.AddRange(m_Args);
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
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
                StartTime = int.Parse(callData.GetParamId(0));
                m_Tag = callData.GetParamId(1);
                m_Msg = callData.GetParamId(2);
            }
            for (int i = 3; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            
        }
        private string m_Tag = string.Empty;
        private string m_Msg = string.Empty;
        private List<string> m_Args = new List<string>();
        
    }
    /// <summary>
    /// publishgfxevent(start_time,event,group,arg1,arg2,arg3,...);
    /// </summary>
    public class PublishGfxEventTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            PublishGfxEventTrigger copy = new PublishGfxEventTrigger();
            
            copy.m_Event = m_Event;
            copy.m_Group = m_Group;
            copy.m_Args.AddRange(m_Args);
            
            return copy;
        }
        public override void Reset()
        {
            
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime) {
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
                StartTime = int.Parse(callData.GetParamId(0));
                m_Event = callData.GetParamId(1);
                m_Group = callData.GetParamId(2);
            }
            for (int i = 3; i < num; ++i) {
                m_Args.Add(callData.GetParamId(i));
            }
            
        }
        private string m_Event = string.Empty;
        private string m_Group = string.Empty;
        private List<string> m_Args = new List<string>();
        
    }
    /// <summary>
    /// params([startTime])
    /// {
    ///     int(name,value);
    ///     long(name,value);
    ///     float(name,value);
    ///     double(name,value);
    ///     string(name,value);
    ///     ...
    /// };
    /// </summary>
    internal class ParamsTriger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            ParamsTriger triger = new ParamsTriger();
            triger.m_Params = new Dictionary<string, object>(m_Params);
            return triger;
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            GameObject obj = senderObj.GfxObj;
            if (null == obj) return false;
            if (curSectionTime < StartTime)
                return true;
            foreach (var pair in m_Params) {
                instance.SetVariable(pair.Key, pair.Value);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            m_Params = new Dictionary<string, object>();
            int num = callData.GetParamNum();
            if (num > 0) {
                StartTime = long.Parse(callData.GetParamId(0));
            } else {
                StartTime = 0;
            }
        }

        protected override void Load(Dsl.FunctionData funcData, int dslSkillId)
        {
            Dsl.CallData callData = funcData.Call;
            if (null != callData) {
                Load(callData, dslSkillId);

                for (int i = 0; i < funcData.Statements.Count; ++i) {
                    Dsl.ISyntaxComponent statement = funcData.Statements[i];
                    Dsl.CallData stCall = statement as Dsl.CallData;
                    if (null != stCall) {
                        string id = stCall.GetId();
                        string key = stCall.GetParamId(0);
                        object val = string.Empty;
                        if (id == "int") {
                            val = int.Parse(stCall.GetParamId(1));
                        } else if (id == "long") {
                            val = long.Parse(stCall.GetParamId(1));
                        } else if (id == "float") {
                            val = float.Parse(stCall.GetParamId(1));
                        } else if (id == "double") {
                            val = double.Parse(stCall.GetParamId(1));
                        } else if (id == "string") {
                            val = stCall.GetParamId(1);
                        }
                        m_Params.Add(key, val);
                    }
                }
            }
        }

        private Dictionary<string, object> m_Params = null;
    }
    /// <summary>
    /// keeptarget([starttime[,remaintime]]);
    /// </summary>
    public class KeepTargetTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            KeepTargetTrigger copy = new KeepTargetTrigger();
            copy.m_RemainTime = m_RemainTime;
            return copy;
        }
        public override void Reset()
        {
        }
        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num >= 1) {
                StartTime = long.Parse(callData.GetParamId(0));
            }
            if (num >= 2) {
                m_RemainTime = long.Parse(callData.GetParamId(1));
            }
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            if (m_RemainTime > 0 && curSectionTime > (StartTime + m_RemainTime)) {
                return false;
            }
            if (senderObj.ConfigData.type == (int)SkillOrImpactType.Skill) {
                EntityController.Instance.KeepTarget(senderObj.TargetActorId);
            } else {
                EntityController.Instance.KeepTarget(senderObj.ActorId);
            }
            return true;
        }

        private long m_RemainTime = 0;
    }
    /// <summary>
    /// useimpact(impactid,[starttime[,is_external_impact]])[if(type)];
    /// </summary>
    public class UseImpactTrigger : AbstractSkillTriger
    {
        protected override ISkillTriger OnClone()
        {
            UseImpactTrigger copy = new UseImpactTrigger();
            copy.m_Impact = m_Impact;
            copy.m_IsExternalImpact = m_IsExternalImpact;
            copy.m_Type = m_Type;
            return copy;
        }
        public override void Reset()
        {
        }
        public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
        {
            GfxSkillSenderInfo senderObj = sender as GfxSkillSenderInfo;
            if (null == senderObj) return false;
            if (curSectionTime < StartTime) {
                return true;
            }
            bool needSetImpact = false;
            if (string.IsNullOrEmpty(m_Type)) {
                needSetImpact = true;
            } else {
                if (m_Type == "block" && instance.Variables.ContainsKey("impact_block")) {
                    needSetImpact = true;
                }
            }
            if (needSetImpact) {
                int impact = m_Impact;
                if (!m_IsExternalImpact) {
                    impact = SkillInstance.GenInnerHitSkillId(m_Impact <= 0 ? 1 : m_Impact);
                }
                instance.SetVariable("impact", impact);
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Impact = int.Parse(callData.GetParamId(0));
            }
            if (num > 1) {
                StartTime = long.Parse(callData.GetParamId(1));
            }
            if (num > 2) {
                m_IsExternalImpact = callData.GetParamId(2) == "true";
            }
        }
        protected override void Load(Dsl.StatementData statementData, int dslSkillId)
        {
            Dsl.FunctionData func1 = statementData.First;
            Dsl.FunctionData func2 = statementData.Second;
            if (null != func1 && null != func2) {
                Load(func1.Call, dslSkillId);
                LoadIf(func2.Call, dslSkillId);
            }
        }
        private void LoadIf(Dsl.CallData callData, int dslSkillId)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Type = callData.GetParamId(0);
            }
        }

        private int m_Impact = 0;
        private bool m_IsExternalImpact = false;
        private string m_Type = string.Empty;
    }
}
