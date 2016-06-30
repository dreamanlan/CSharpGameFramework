using System;
using System.Collections.Generic;
using System.Collections;
namespace StorySystem.CommonCommands
{
    /// <summary>
    /// dummy命令，用于注册没有对应实现的命令（为了解析需要注册）。
    /// </summary>
    public sealed class DummyCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            DummyCommand cmd = new DummyCommand();
            return cmd;
        }
    }
    /// <summary>
    /// localmessage(msgid,arg1,arg2,...);
    /// </summary>
    internal sealed class LocalMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            LocalMessageCommand cmd = new LocalMessageCommand();
            cmd.m_MsgId = m_MsgId.Clone();
            for (int i = 0; i < m_MsgArgs.Count; i++) {
                cmd.m_MsgArgs.Add(m_MsgArgs[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_MsgId.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_MsgArgs.Count; i++) {
                m_MsgArgs[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            string msgId = m_MsgId.Value;
            ArrayList arglist = new ArrayList();
            for (int i = 0; i < m_MsgArgs.Count; i++) {
                arglist.Add(m_MsgArgs[i].Value);
            }
            object[] args = arglist.ToArray();
            instance.SendMessage(msgId, args);
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_MsgId.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < num; ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgArgs.Add(val);
            }
        }
        private IStoryValue<string> m_MsgId = new StoryValue<string>();
        private List<IStoryValue<object>> m_MsgArgs = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// clearmessage(msgid1,msgid2,...);
    /// </summary>
    internal sealed class ClearMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ClearMessageCommand cmd = new ClearMessageCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            string[] arglist = new string[m_MsgIds.Count];
            for (int i = 0; i < m_MsgIds.Count; i++) {
                arglist[i] = m_MsgIds[i].Value;
            }
            instance.ClearMessage(arglist);
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// waitlocalmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal sealed class WaitLocalMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            WaitLocalMessageCommand cmd = new WaitLocalMessageCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_TimeoutVal = m_TimeoutVal.Clone();
            cmd.m_TimeoutSetVar = m_TimeoutSetVar.Clone();
            cmd.m_TimeoutSetVal = m_TimeoutSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }
        protected override void ResetState()
        {
            m_CurTime = 0;
            m_StartTime = 0;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance, iterator, args);
                m_SetVal.Evaluate(instance, iterator, args);
                m_TimeoutVal.Evaluate(instance, iterator, args);
                m_TimeoutSetVar.Evaluate(instance, iterator, args);
                m_TimeoutSetVal.Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_StartTime <= 0) {
                long startTime = GameFramework.TimeUtility.GetLocalMilliseconds();
                m_StartTime = startTime;
            }
            bool triggered = false;
            for (int i = 0; i < m_MsgIds.Count; i++) {
                long time = instance.GetMessageTriggerTime(m_MsgIds[i].Value);
                if (time > m_StartTime) {
                    triggered = true;
                    break;
                }
            }
            bool ret = false;
            if (triggered) {
                string varName = m_SetVar.Value;
                object varVal = m_SetVal.Value;
                instance.SetVariable(varName, varVal);
            } else {
                int timeout = m_TimeoutVal.Value;
                int curTime = m_CurTime;
                m_CurTime += (int)delta;
                if (timeout <= 0 || curTime <= timeout) {
                    ret = true;
                } else {
                    string varName = m_TimeoutSetVar.Value;
                    object varVal = m_TimeoutSetVal.Value;
                    instance.SetVariable(varName, varVal);
                }
            }
            return ret;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                Dsl.CallData third = statementData.Functions[2].Call;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;
                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
        }
        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }
        private void LoadTimeoutSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal.InitFromDsl(callData.GetParam(0));
                m_TimeoutSetVar.InitFromDsl(callData.GetParam(1));
                m_TimeoutSetVal.InitFromDsl(callData.GetParam(2));
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue<object> m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
        private long m_StartTime = 0;
    }
    /// <summary>
    /// waitlocalmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal sealed class WaitLocalMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            WaitLocalMessageHandlerCommand cmd = new WaitLocalMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_TimeoutVal = m_TimeoutVal.Clone();
            cmd.m_TimeoutSetVar = m_TimeoutSetVar.Clone();
            cmd.m_TimeoutSetVal = m_TimeoutSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }
        protected override void ResetState()
        {
            m_CurTime = 0;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance, iterator, args);
                m_SetVal.Evaluate(instance, iterator, args);
                m_TimeoutVal.Evaluate(instance, iterator, args);
                m_TimeoutSetVar.Evaluate(instance, iterator, args);
                m_TimeoutSetVal.Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int ct = 0;
            for (int i = 0; i < m_MsgIds.Count; i++) {
                ct += instance.CountMessage(m_MsgIds[i].Value);
            }
            bool ret = false;
            if (ct <= 0) {
                string varName = m_SetVar.Value;
                object varVal = m_SetVal.Value;
                instance.SetVariable(varName, varVal);
            } else {
                int curTime = m_CurTime;
                m_CurTime += (int)delta;
                if (curTime <= m_TimeoutVal.Value) {
                    ret = true;
                } else {
                    string varName = m_TimeoutSetVar.Value;
                    object varVal = m_TimeoutSetVal.Value;
                    instance.SetVariable(varName, varVal);
                }
            }
            return ret;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                Dsl.CallData third = statementData.Functions[2].Call;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;
                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
        }
        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }
        private void LoadTimeoutSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal.InitFromDsl(callData.GetParam(0));
                m_TimeoutSetVar.InitFromDsl(callData.GetParam(1));
                m_TimeoutSetVal.InitFromDsl(callData.GetParam(2));
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue<object> m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
    }
    /// <summary>
    /// pauselocalmessagehandler(msgid1,msgid2,...);
    /// </summary>
    internal sealed class PauseLocalMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            PauseLocalMessageHandlerCommand cmd = new PauseLocalMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                instance.PauseMessageHandler(m_MsgIds[i].Value, true);
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// resumelocalmessagehandler(msgid1,msgid2,...);
    /// </summary>
    internal sealed class ResumeLocalMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ResumeLocalMessageHandlerCommand cmd = new ResumeLocalMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                instance.PauseMessageHandler(m_MsgIds[i].Value, false);
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// localnamespacedmessage(msgid,arg1,arg2,...);
    /// </summary>
    internal sealed class LocalNamespacedMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            LocalNamespacedMessageCommand cmd = new LocalNamespacedMessageCommand();
            cmd.m_MsgId = m_MsgId.Clone();
            for (int i = 0; i < m_MsgArgs.Count; i++) {
                cmd.m_MsgArgs.Add(m_MsgArgs[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_MsgId.Evaluate(instance, iterator, args);
            for (int i = 0; i < m_MsgArgs.Count; i++) {
                m_MsgArgs[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            string msgId = m_MsgId.Value;
            ArrayList arglist = new ArrayList();
            string _namespace = instance.Namespace;
            if (!string.IsNullOrEmpty(_namespace)) {
                msgId = string.Format("{0}:{1}", _namespace, msgId);
            }
            for (int i = 0; i < m_MsgArgs.Count; i++) {
                arglist.Add(m_MsgArgs[i].Value);
            }
            object[] args = arglist.ToArray();
            instance.SendMessage(msgId, args);
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_MsgId.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < num; ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgArgs.Add(val);
            }
        }
        private IStoryValue<string> m_MsgId = new StoryValue<string>();
        private List<IStoryValue<object>> m_MsgArgs = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// clearnamespacedmessage(msgid1,msgid2,...);
    /// </summary>
    internal sealed class ClearNamespacedMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ClearNamespacedMessageCommand cmd = new ClearNamespacedMessageCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            string[] arglist = new string[m_MsgIds.Count];
            string _namespace = instance.Namespace;
            if (string.IsNullOrEmpty(_namespace)) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    arglist[i] = m_MsgIds[i].Value;
                }
            } else {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    arglist[i] = string.Format("{0}:{1}", _namespace, m_MsgIds[i].Value);
                }
            }
            instance.ClearMessage(arglist);
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// waitlocalnamespacedmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal sealed class WaitLocalNamespacedMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            WaitLocalNamespacedMessageCommand cmd = new WaitLocalNamespacedMessageCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_TimeoutVal = m_TimeoutVal.Clone();
            cmd.m_TimeoutSetVar = m_TimeoutSetVar.Clone();
            cmd.m_TimeoutSetVal = m_TimeoutSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }
        protected override void ResetState()
        {
            m_CurTime = 0;
            m_StartTime = 0;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance, iterator, args);
                m_SetVal.Evaluate(instance, iterator, args);
                m_TimeoutVal.Evaluate(instance, iterator, args);
                m_TimeoutSetVar.Evaluate(instance, iterator, args);
                m_TimeoutSetVal.Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            if (m_StartTime <= 0) {
                long startTime = GameFramework.TimeUtility.GetLocalMilliseconds();
                m_StartTime = startTime;
            }
            bool triggered = false;
            string _namespace = instance.Namespace;
            if (string.IsNullOrEmpty(_namespace)) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    long time = instance.GetMessageTriggerTime(m_MsgIds[i].Value);
                    if (time > m_StartTime) {
                        triggered = true;
                        break;
                    }
                }
            } else {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    long time = instance.GetMessageTriggerTime(string.Format("{0}:{1}", _namespace, m_MsgIds[i].Value));
                    if (time > m_StartTime) {
                        triggered = true;
                        break;
                    }
                }
            }
            bool ret = false;
            if (triggered) {
                string varName = m_SetVar.Value;
                object varVal = m_SetVal.Value;
                instance.SetVariable(varName, varVal);
            } else {
                int curTime = m_CurTime;
                m_CurTime += (int)delta;
                if (curTime <= m_TimeoutVal.Value) {
                    ret = true;
                } else {
                    string varName = m_TimeoutSetVar.Value;
                    object varVal = m_TimeoutSetVal.Value;
                    instance.SetVariable(varName, varVal);
                }
            }
            return ret;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                Dsl.CallData third = statementData.Functions[2].Call;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;
                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
        }
        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }
        private void LoadTimeoutSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal.InitFromDsl(callData.GetParam(0));
                m_TimeoutSetVar.InitFromDsl(callData.GetParam(1));
                m_TimeoutSetVal.InitFromDsl(callData.GetParam(2));
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue<object> m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
        private long m_StartTime = 0;
    }
    /// <summary>
    /// waitlocalnamespacedmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal sealed class WaitLocalNamespacedMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            WaitLocalNamespacedMessageHandlerCommand cmd = new WaitLocalNamespacedMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_TimeoutVal = m_TimeoutVal.Clone();
            cmd.m_TimeoutSetVar = m_TimeoutSetVar.Clone();
            cmd.m_TimeoutSetVal = m_TimeoutSetVal.Clone();
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }
        protected override void ResetState()
        {
            m_CurTime = 0;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance, iterator, args);
                m_SetVal.Evaluate(instance, iterator, args);
                m_TimeoutVal.Evaluate(instance, iterator, args);
                m_TimeoutSetVar.Evaluate(instance, iterator, args);
                m_TimeoutSetVal.Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            int ct = 0;
            string _namespace = instance.Namespace;
            if (string.IsNullOrEmpty(_namespace)) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    ct += instance.CountMessage(m_MsgIds[i].Value);
                }
            } else {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    ct += instance.CountMessage(string.Format("{0}:{1}", _namespace, m_MsgIds[i].Value));
                }
            }
            bool ret = false;
            if (ct <= 0) {
                string varName = m_SetVar.Value;
                object varVal = m_SetVal.Value;
                instance.SetVariable(varName, varVal);
            } else {
                int curTime = m_CurTime;
                m_CurTime += (int)delta;
                if (curTime <= m_TimeoutVal.Value) {
                    ret = true;
                } else {
                    string varName = m_TimeoutSetVar.Value;
                    object varVal = m_TimeoutSetVal.Value;
                    instance.SetVariable(varName, varVal);
                }
            }
            return ret;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.CallData first = statementData.Functions[0].Call;
                Dsl.CallData second = statementData.Functions[1].Call;
                Dsl.CallData third = statementData.Functions[2].Call;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;
                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
        }
        private void LoadSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }
        private void LoadTimeoutSet(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3) {
                m_TimeoutVal.InitFromDsl(callData.GetParam(0));
                m_TimeoutSetVar.InitFromDsl(callData.GetParam(1));
                m_TimeoutSetVal.InitFromDsl(callData.GetParam(2));
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue<object> m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
    }
    /// <summary>
    /// pauselocalnamespacedmessagehandler(msgid1,msgid2,...);
    /// </summary>
    internal sealed class PauseLocalNamespacedMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            PauseLocalNamespacedMessageHandlerCommand cmd = new PauseLocalNamespacedMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            string _namespace = instance.Namespace;
            if (string.IsNullOrEmpty(_namespace)) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    instance.PauseMessageHandler(m_MsgIds[i].Value, true);
                }
            } else {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    instance.PauseMessageHandler(string.Format("{0}:{1}", _namespace, m_MsgIds[i].Value), true);
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// resumelocalnamespacedmessagehandler(msgid1,msgid2,...);
    /// </summary>
    internal sealed class ResumeLocalNamespacedMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            ResumeLocalNamespacedMessageHandlerCommand cmd = new ResumeLocalNamespacedMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            string _namespace = instance.Namespace;
            if (string.IsNullOrEmpty(_namespace)) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    instance.PauseMessageHandler(m_MsgIds[i].Value, false);
                }
            } else {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    instance.PauseMessageHandler(string.Format("{0}:{1}", _namespace, m_MsgIds[i].Value), false);
                }
            }
            return false;
        }
        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
        }
        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
}
