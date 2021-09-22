using System;
using System.Collections;
using System.Collections.Generic;
using StorySystem;
using GameFramework;
using GameFrameworkMessage;

namespace GameFramework.Story.Commands
{
    /// <summary>
    /// startstory(story_id);
    /// </summary>
    internal class StartStoryCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            StartStoryCommand cmd = new StartStoryCommand();
            cmd.m_StoryId = m_StoryId.Clone();
            cmd.m_Multiple = m_Multiple.Clone();
            return cmd;
        }
        protected override void ResetState()
        { }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_StoryId.Evaluate(instance, handler, iterator, args);
            m_Multiple.Evaluate(instance, handler, iterator, args);
        
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {

	            var storyId = m_StoryId.Value;
	            var multiple = m_Multiple.Value;
	            userThread.QueueAction(() => {
	                if (multiple == 0)
	                    userThread.StorySystem.StartStory(storyId);
	                else
	                    userThread.StorySystem.StartStories(storyId);
	            });
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                if (!first.HaveStatement() && !second.HaveStatement()) {
                    Load(first);
                    var call = second;
                    if (call.GetId() == "multiple" && call.GetParamNum() > 0) {
                        m_Multiple.InitFromDsl(call.GetParam(0));
                    }
                }
            }
            return true;
        }
        private IStoryValue<string> m_StoryId = new StoryValue<string>();
        private IStoryValue<int> m_Multiple = new StoryValue<int>();
    }
    /// <summary>
    /// stopstory(story_id);
    /// </summary>
    internal class StopStoryCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            StopStoryCommand cmd = new StopStoryCommand();
            cmd.m_StoryId = m_StoryId.Clone();
            cmd.m_Multiple = m_Multiple.Clone();
            return cmd;
        }
        protected override void ResetState()
        { }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_StoryId.Evaluate(instance, handler, iterator, args);
            m_Multiple.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                var multiple = m_Multiple.Value;
	            if (multiple == 0)
	                userThread.StorySystem.MarkStoryTerminated(m_StoryId.Value);
	            else
	                userThread.StorySystem.MarkStoriesTerminated(m_StoryId.Value);
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                if (!first.HaveStatement() && !second.HaveStatement()) {
                    Load(first);
                    var call = second;
                    if (call.GetId() == "multiple" && call.GetParamNum() > 0) {
                        m_Multiple.InitFromDsl(call.GetParam(0));
                    }
                }
            }
            return true;
        }
        private IStoryValue<string> m_StoryId = new StoryValue<string>();
        private IStoryValue<int> m_Multiple = new StoryValue<int>();
    }
    /// <summary>
    /// waitstory(storyid1,storyid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitStoryCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            WaitStoryCommand cmd = new WaitStoryCommand();
            for (int i = 0; i < m_StoryIds.Count; i++) {
                cmd.m_StoryIds.Add(m_StoryIds[i].Clone());
            }
            cmd.m_SetVar = m_SetVar.Clone();
            cmd.m_SetVal = m_SetVal.Clone();
            cmd.m_TimeoutVal = m_TimeoutVal.Clone();
            cmd.m_TimeoutSetVar = m_TimeoutSetVar.Clone();
            cmd.m_TimeoutSetVal = m_TimeoutSetVal.Clone();
            cmd.m_Multiple = m_Multiple.Clone();
            cmd.m_HaveSet = m_HaveSet;
            cmd.m_HaveMultiple = m_HaveMultiple;
            return cmd;
        }
        protected override void ResetState()
        {
            m_CurTime = 0;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Evaluate(instance, handler, iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance, handler, iterator, args);
                m_SetVal.Evaluate(instance, handler, iterator, args);
                m_TimeoutVal.Evaluate(instance, handler, iterator, args);
                m_TimeoutSetVar.Evaluate(instance, handler, iterator, args);
                m_TimeoutSetVal.Evaluate(instance, handler, iterator, args);
            }
            if (m_HaveMultiple) {
                m_Multiple.Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            bool ret = false;
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                int ct = 0;
                for (int i = 0; i < m_StoryIds.Count; i++) {
                    ct += userThread.StorySystem.CountStory(m_StoryIds[i].Value);
                }
	            int multiple = m_Multiple.Value;
                if (ct <= 0) {
	                if (m_HaveSet) {
	                    string varName = m_SetVar.Value;
	                    var varVal = m_SetVal.Value;
	                    instance.SetVariable(varName, varVal);
	                }
	            } else {
	                int timeout = m_TimeoutVal.Value;
	                int curTime = m_CurTime;
	                m_CurTime += (int)delta;
	                if (timeout <= 0 || curTime <= timeout) {
	                    ret = true;
	                } else if (m_HaveSet) {
	                    string varName = m_TimeoutSetVar.Value;
	                    var varVal = m_TimeoutSetVal.Value;
	                    instance.SetVariable(varName, varVal);
	                }
	            }
			}
            return ret;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            int ct = statementData.Functions.Count;
            if (statementData.Functions.Count >= 2) {
                var first = statementData.First;
                var second = statementData.Second;
                if (ct == 2) {
                    m_HaveMultiple = true;
                    LoadMultiple(second);
                } else if (ct == 3) {
                    var third = statementData.Third;
                    if (null != first && null != second && null != third) {
                        m_HaveSet = true;
                        Load(first);
                        LoadSet(second);
                        LoadTimeoutSet(third);
                    }
                } else if (ct == 4) {
                    var third = statementData.Third;
                    var last = statementData.Last;
                    if (null != first && null != second && null != third && null != last) {
                        m_HaveSet = true;
                        Load(first);
                        LoadSet(second);
                        LoadTimeoutSet(third);
                        m_HaveMultiple = true;
                        LoadMultiple(last);
                    }
                }
            }
            return true;
        }
        private void LoadMultiple(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 1 && callData.GetId() == "multiple") {
                m_Multiple.InitFromDsl(callData.GetParam(0));
            }
        }
        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2 && callData.GetId() == "set") {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }
        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3 && callData.GetId() == "timeoutset") {
                m_TimeoutVal.InitFromDsl(callData.GetParam(0));
                m_TimeoutSetVar.InitFromDsl(callData.GetParam(1));
                m_TimeoutSetVal.InitFromDsl(callData.GetParam(2));
            }
        }
        private List<IStoryValue<string>> m_StoryIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue m_TimeoutSetVal = new StoryValue();
        private IStoryValue<int> m_Multiple = new StoryValue<int>();
        private bool m_HaveMultiple = false;
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
    }
    /// <summary>
    /// pausestory(storyid1,storyid2,...);
    /// </summary>
    internal class PauseStoryCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            PauseStoryCommand cmd = new PauseStoryCommand();
            for (int i = 0; i < m_StoryIds.Count; i++) {
                cmd.m_StoryIds.Add(m_StoryIds[i].Clone());
            }
            cmd.m_Multiple = m_Multiple.Clone();
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Evaluate(instance, handler, iterator, args);
            }
            m_Multiple.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                var multiple = m_Multiple.Value;
                if (multiple == 0) {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        userThread.StorySystem.PauseStory(m_StoryIds[i].Value, true);
                    }
                } else {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        userThread.StorySystem.PauseStories(m_StoryIds[i].Value, true);
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                if (!first.HaveStatement() && !second.HaveStatement()) {
                    Load(first);
                    var call = second;
                    if (call.GetId() == "multiple" && call.GetParamNum() > 0) {
                        m_Multiple.InitFromDsl(call.GetParam(0));
                    }
                }
            }
            return true;
        }
        private List<IStoryValue<string>> m_StoryIds = new List<IStoryValue<string>>();
        private IStoryValue<int> m_Multiple = new StoryValue<int>();
    }
    /// <summary>
    /// resumestory(storyid1,storyid2,...);
    /// </summary>
    internal class ResumeStoryCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ResumeStoryCommand cmd = new ResumeStoryCommand();
            for (int i = 0; i < m_StoryIds.Count; i++) {
                cmd.m_StoryIds.Add(m_StoryIds[i].Clone());
            }
            cmd.m_Multiple = m_Multiple.Clone();
            return cmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Evaluate(instance, handler, iterator, args);
            }
            m_Multiple.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                var multiple = m_Multiple.Value;
                if (multiple == 0) {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        userThread.StorySystem.PauseStory(m_StoryIds[i].Value, false);
                    }
                } else {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        userThread.StorySystem.PauseStories(m_StoryIds[i].Value, false);
                    }
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First;
                var second = statementData.Second;
                if (!first.HaveStatement() && !second.HaveStatement()) {
                    Load(first);
                    var call = second;
                    if (call.GetId() == "multiple" && call.GetParamNum() > 0) {
                        m_Multiple.InitFromDsl(call.GetParam(0));
                    }
                }
            }
            return true;
        }
        private List<IStoryValue<string>> m_StoryIds = new List<IStoryValue<string>>();
        private IStoryValue<int> m_Multiple = new StoryValue<int>();
    }
    /// <summary>
    /// firemessage(msgid,arg1,arg2,...);
    /// </summary>
    internal class FireMessageCommand : AbstractStoryCommand
    {
        public FireMessageCommand(bool isConcurrent)
        {
            m_IsConcurrent = isConcurrent;
        }
        protected override IStoryCommand CloneCommand()
        {
            FireMessageCommand cmd = new FireMessageCommand(m_IsConcurrent);
            cmd.m_MsgId = m_MsgId.Clone();
            for (int i = 0; i < m_MsgArgs.Count; ++i) {
                IStoryValue val = m_MsgArgs[i];
                cmd.m_MsgArgs.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_MsgId.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_MsgArgs.Count; ++i) {
                IStoryValue val = m_MsgArgs[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string msgId = m_MsgId.Value;
                var args = userThread.StorySystem.NewBoxedValueList();
                for (int i = 0; i < m_MsgArgs.Count; ++i) {
                    IStoryValue val = m_MsgArgs[i];
                    args.Add(val.Value);
                }
                if (m_IsConcurrent)
                    userThread.StorySystem.SendConcurrentMessage(msgId, args);
                else
                    userThread.StorySystem.SendMessage(msgId, args);
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_MsgId.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgArgs.Add(val);
            }
            return true;
        }

        private IStoryValue<string> m_MsgId = new StoryValue<string>();
        private List<IStoryValue> m_MsgArgs = new List<IStoryValue>();
        private bool m_IsConcurrent = false;
    }
    internal sealed class FireMessageCommandFactory : IStoryCommandFactory
    {
        public IStoryCommand Create()
        {
            return new FireMessageCommand(false);
        }
    }
    internal sealed class FireConcurrentMessageCommandFactory : IStoryCommandFactory
    {
        public IStoryCommand Create()
        {
            return new FireMessageCommand(true);
        }
    }
    /// <summary>
    /// waitallmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitAllMessageCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            WaitAllMessageCommand cmd = new WaitAllMessageCommand();
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

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, handler, iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance, handler, iterator, args);
                m_SetVal.Evaluate(instance, handler, iterator, args);
                m_TimeoutVal.Evaluate(instance, handler, iterator, args);
                m_TimeoutSetVar.Evaluate(instance, handler, iterator, args);
                m_TimeoutSetVal.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            bool ret = false;
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
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
                if (triggered) {
                    string varName = m_SetVar.Value;
                    var varVal = m_SetVal.Value;
                    instance.SetVariable(varName, varVal);
                } else {
                    int curTime = m_CurTime;
                    m_CurTime += (int)delta;
                    if (curTime <= m_TimeoutVal.Value) {
                        ret = true;
                    } else {
                        string varName = m_TimeoutSetVar.Value;
                        var varVal = m_TimeoutSetVal.Value;
                        instance.SetVariable(varName, varVal);
                    }
                }
            }
            return ret;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                Dsl.FunctionData third = statementData.Third;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;

                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
            return true;
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
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
        private IStoryValue m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
        private long m_StartTime = 0;
    }
    /// <summary>
    /// waitallmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitAllMessageHandlerCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            WaitAllMessageHandlerCommand cmd = new WaitAllMessageHandlerCommand();
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

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, handler, iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance, handler, iterator, args);
                m_SetVal.Evaluate(instance, handler, iterator, args);
                m_TimeoutVal.Evaluate(instance, handler, iterator, args);
                m_TimeoutSetVar.Evaluate(instance, handler, iterator, args);
                m_TimeoutSetVal.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            bool ret = false;
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                int ct = 0;
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    ct += userThread.StorySystem.CountMessage(m_MsgIds[i].Value);
                }
                if (ct <= 0) {
                    string varName = m_SetVar.Value;
                    var varVal = m_SetVal.Value;
                    instance.SetVariable(varName, varVal);
                } else {
                    int curTime = m_CurTime;
                    m_CurTime += (int)delta;
                    if (curTime <= m_TimeoutVal.Value) {
                        ret = true;
                    } else {
                        string varName = m_TimeoutSetVar.Value;
                        var varVal = m_TimeoutSetVal.Value;
                        instance.SetVariable(varName, varVal);
                    }
                }
            }
            return ret;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                Dsl.FunctionData third = statementData.Third;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;

                    Load(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
            return true;
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2) {
                m_SetVar.InitFromDsl(callData.GetParam(0));
                m_SetVal.InitFromDsl(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
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
        private IStoryValue m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
    }
    /// <summary>
    /// pauseallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    public class SuspendAllMessageHandlerCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SuspendAllMessageHandlerCommand cmd = new SuspendAllMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    userThread.StorySystem.SuspendMessageHandler(m_MsgIds[i].Value, true);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }

        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// resumeallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    public class ResumeAllMessageHandlerCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ResumeAllMessageHandlerCommand cmd = new ResumeAllMessageHandlerCommand();
            for (int i = 0; i < m_MsgIds.Count; i++) {
                cmd.m_MsgIds.Add(m_MsgIds[i].Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    userThread.StorySystem.SuspendMessageHandler(m_MsgIds[i].Value, false);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }

        private List<IStoryValue<string>> m_MsgIds = new List<IStoryValue<string>>();
    }
    /// <summary>
    /// sendserverstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendServerStoryMessageCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendServerStoryMessageCommand cmd = new SendServerStoryMessageCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Msg.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string _msg = m_Msg.Value;

                Msg_LRL_StoryMessage msg = new Msg_LRL_StoryMessage();
                msg.MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.NULL;
                        arg.str_val = "";
                        msg.Args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.INT;
                        arg.str_val = v.Get<int>().ToString();
                        msg.Args.Add(arg);
                    } else if (v.IsFloat) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.FLOAT;
                        arg.str_val = v.Get<float>().ToString();
                        msg.Args.Add(arg);
                    } else {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.Args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Value;
                    msg.UserGuid = userGuid;
                    userThread.SendServerMessage(msg);
                } else {
                    userThread.SendServerMessage(msg);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Msg.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// sendclientstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendClientStoryMessageCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendClientStoryMessageCommand cmd = new SendClientStoryMessageCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_Msg.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string _msg = m_Msg.Value;

                Msg_CLC_StoryMessage msg = new Msg_CLC_StoryMessage();
                msg.m_MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.m_Args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = v.Get<int>().ToString();
                        msg.m_Args.Add(arg);
                    } else if (v.IsFloat) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = v.Get<float>().ToString();
                        msg.m_Args.Add(arg);
                    } else {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.m_Args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Value;
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_CLC_StoryMessage, msg);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_CLC_StoryMessage, msg);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Msg.InitFromDsl(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// publishgfxevent(ev_name,group,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class PublishGfxEventCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            PublishGfxEventCommand cmd = new PublishGfxEventCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_EventName = m_EventName.Clone();
            cmd.m_Group = m_Group.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_EventName.Evaluate(instance, handler, iterator, args);
            m_Group.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string evname = m_EventName.Value;
                string group = m_Group.Value;

                Msg_LC_PublishEvent msg = new Msg_LC_PublishEvent();
                msg.group = group;
                msg.ev_name = evname;
                msg.is_logic_event = false;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = v.Get<int>().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsFloat) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = v.Get<float>().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Value;
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_LC_PublishEvent, msg);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_LC_PublishEvent, msg);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_EventName.InitFromDsl(callData.GetParam(0));
                m_Group.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_EventName = new StoryValue<string>();
        private IStoryValue<string> m_Group = new StoryValue<string>();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// sendgfxmessage(objname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendGfxMessageCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendGfxMessageCommand cmd = new SendGfxMessageCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_ObjName = m_ObjName.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_ObjName.Evaluate(instance, handler, iterator, args);
            m_Msg.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string objname = m_ObjName.Value;
                string _msg = m_Msg.Value;

                Msg_LC_SendGfxMessage msg = new Msg_LC_SendGfxMessage();
                msg.is_with_tag = false;
                msg.name = objname;
                msg.msg = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = v.Get<int>().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsFloat) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = v.Get<float>().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Value;
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_LC_SendGfxMessage, msg);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_LC_SendGfxMessage, msg);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjName.InitFromDsl(callData.GetParam(0));
                m_Msg.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_ObjName = new StoryValue<string>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// sendgfxmessagewithtag(tagname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendGfxMessageWithTagCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendGfxMessageWithTagCommand cmd = new SendGfxMessageWithTagCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_ObjTag = m_ObjTag.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_ObjTag.Evaluate(instance, handler, iterator, args);
            m_Msg.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string objname = m_ObjTag.Value;
                string _msg = m_Msg.Value;

                Msg_LC_SendGfxMessage msg = new Msg_LC_SendGfxMessage();
                msg.is_with_tag = true;
                msg.name = objname;
                msg.msg = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = v.Get<int>().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsFloat) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = v.Get<float>().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Value;
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_LC_SendGfxMessage, msg);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_LC_SendGfxMessage, msg);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjTag.InitFromDsl(callData.GetParam(0));
                m_Msg.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_Args.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != second) {
                    Load(first);
                    LoadUserId(second);
                }
            }
            return true;
        }

        private void LoadUserId(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_ObjTag = new StoryValue<string>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue> m_Args = new List<IStoryValue>();
    }
    /// <summary>
    /// highlightprompt(userguid,dictid,arg1,arg2,...);
    /// </summary>
    internal class HighlightPromptCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            HighlightPromptCommand cmd = new HighlightPromptCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_DictId = m_DictId.Clone();
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryValue val = m_DictArgs[i];
                cmd.m_DictArgs.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_UserGuid.Evaluate(instance, handler, iterator, args);
            m_DictId.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryValue val = m_DictArgs[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = m_UserGuid.Value;
                string dictId = m_DictId.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_DictArgs.Count; ++i) {
                    IStoryValue val = m_DictArgs[i];
                    arglist.Add(val.Value.Get<object>());
                }
                object[] args = arglist.ToArray();

                Msg_LC_HighlightPrompt builder = new Msg_LC_HighlightPrompt();
                builder.dict_id = dictId;
                foreach (object arg in args) {
                    builder.argument.Add(arg.ToString());
                }
                if (userGuid > 0) {
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_LC_HighlightPrompt, builder);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_LC_HighlightPrompt, builder);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_DictId.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_DictArgs.Add(val);
            }
            return true;
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_DictId = new StoryValue<string>();
        private List<IStoryValue> m_DictArgs = new List<IStoryValue>();
    }
}
