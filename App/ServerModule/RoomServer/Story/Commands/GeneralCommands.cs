﻿using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using ScriptableFramework;
using GameFrameworkMessage;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// startstory(story_id);
    /// </summary>
    public class StartStoryCommand : AbstractStoryCommand
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
	            var storyId = m_StoryId.Value;
	            var multiple = m_Multiple.Value;
	            scene.DelayActionProcessor.QueueAction(() => {
	                if (multiple == 0)
	                    scene.StorySystem.StartStory(storyId);
	                else
	                    scene.StorySystem.StartStories(storyId);
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
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
        private IStoryFunction<string> m_StoryId = new StoryValue<string>();
        private IStoryFunction<int> m_Multiple = new StoryValue<int>();
    }
    /// <summary>
    /// stopstory(story_id);
    /// </summary>
    public class StopStoryCommand : AbstractStoryCommand
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
	            var multiple = m_Multiple.Value;
	            if (multiple == 0)
	                scene.StorySystem.MarkStoryTerminated(m_StoryId.Value);
	            else
	                scene.StorySystem.MarkStoriesTerminated(m_StoryId.Value);
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
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
        private IStoryFunction<string> m_StoryId = new StoryValue<string>();
        private IStoryFunction<int> m_Multiple = new StoryValue<int>();
    }
    /// <summary>
    /// waitstory(storyid1,storyid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    public class WaitStoryCommand : AbstractStoryCommand
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int ct = 0;
                for (int i = 0; i < m_StoryIds.Count; i++) {
                    ct += scene.StorySystem.CountStory(m_StoryIds[i].Value);
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
                IStoryFunction<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            int ct = statementData.Functions.Count;
            if (statementData.Functions.Count >= 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                if (ct == 2) {
                    m_HaveMultiple = true;
                    LoadMultiple(second);
                } else if (ct == 3) {
                    var third = statementData.Third.AsFunction;
                    if (null != first && null != second && null != third) {
                        m_HaveSet = true;
                        Load(first);
                        LoadSet(second);
                        LoadTimeoutSet(third);
                    }
                } else if (ct == 4) {
                    var third = statementData.Third.AsFunction;
                    var last = statementData.Last.AsFunction;
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
        private List<IStoryFunction<string>> m_StoryIds = new List<IStoryFunction<string>>();
        private IStoryFunction<string> m_SetVar = new StoryValue<string>();
        private IStoryFunction m_SetVal = new StoryValue();
        private IStoryFunction<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryFunction<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryFunction m_TimeoutSetVal = new StoryValue();
        private IStoryFunction<int> m_Multiple = new StoryValue<int>();
        private bool m_HaveMultiple = false;
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
    }
    /// <summary>
    /// pausestory(storyid1,storyid2,...);
    /// </summary>
    public class PauseStoryCommand : AbstractStoryCommand
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                var multiple = m_Multiple.Value;
	            if (multiple == 0) {
	                for (int i = 0; i < m_StoryIds.Count; i++) {
	                    scene.StorySystem.PauseStory(m_StoryIds[i].Value, true);
	                }
	            } else {
	                for (int i = 0; i < m_StoryIds.Count; i++) {
	                    scene.StorySystem.PauseStories(m_StoryIds[i].Value, true);
	                }
	            }
			}
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryFunction<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
        private List<IStoryFunction<string>> m_StoryIds = new List<IStoryFunction<string>>();
        private IStoryFunction<int> m_Multiple = new StoryValue<int>();
    }
    /// <summary>
    /// resumestory(storyid1,storyid2,...);
    /// </summary>
    public class ResumeStoryCommand : AbstractStoryCommand
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                var multiple = m_Multiple.Value;
	            if (multiple == 0) {
	                for (int i = 0; i < m_StoryIds.Count; i++) {
	                    scene.StorySystem.PauseStory(m_StoryIds[i].Value, false);
	                }
	            } else {
	                for (int i = 0; i < m_StoryIds.Count; i++) {
	                    scene.StorySystem.PauseStories(m_StoryIds[i].Value, false);
	                }
	            }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryFunction<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
        private List<IStoryFunction<string>> m_StoryIds = new List<IStoryFunction<string>>();
        private IStoryFunction<int> m_Multiple = new StoryValue<int>();
    }
    /// <summary>
    /// firemessage(msgid,arg1,arg2,...);
    /// </summary>
    public class FireMessageCommand : AbstractStoryCommand
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
                IStoryFunction val = m_MsgArgs[i];
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
                IStoryFunction val = m_MsgArgs[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string msgId = m_MsgId.Value;
                var args = scene.StorySystem.NewBoxedValueList();
                for (int i = 0; i < m_MsgArgs.Count; ++i) {
                    IStoryFunction val = m_MsgArgs[i];
                    args.Add(val.Value);
                }
                if (m_IsConcurrent)
                    scene.StorySystem.SendConcurrentMessage(msgId, args);
                else
                    scene.StorySystem.SendMessage(msgId, args);
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

        private IStoryFunction<string> m_MsgId = new StoryValue<string>();
        private List<IStoryFunction> m_MsgArgs = new List<IStoryFunction>();
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
    public class WaitAllMessageCommand : AbstractStoryCommand
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                if (m_StartTime <= 0) {
                    long startTime = ScriptableFramework.TimeUtility.GetLocalMilliseconds();
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
                    int timeout = m_TimeoutVal.Value;
                    int curTime = m_CurTime;
                    m_CurTime += (int)delta;
                    if (timeout <= 0 || curTime <= timeout) {
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
                IStoryFunction<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                Dsl.FunctionData third = statementData.Third.AsFunction;
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

        private List<IStoryFunction<string>> m_MsgIds = new List<IStoryFunction<string>>();
        private IStoryFunction<string> m_SetVar = new StoryValue<string>();
        private IStoryFunction m_SetVal = new StoryValue();
        private IStoryFunction<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryFunction<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryFunction m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
        private long m_StartTime = 0;
    }
    /// <summary>
    /// waitallmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    public class WaitAllMessageHandlerCommand : AbstractStoryCommand
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int ct = 0;
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    ct += scene.StorySystem.CountMessage(m_MsgIds[i].Value);
                }
                if (ct <= 0) {
                    string varName = m_SetVar.Value;
                    var varVal = m_SetVal.Value;
                    instance.SetVariable(varName, varVal);
                } else {
                    int timeout = m_TimeoutVal.Value;
                    int curTime = m_CurTime;
                    m_CurTime += (int)delta;
                    if (timeout <= 0 || curTime <= timeout) {
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
                IStoryFunction<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
                Dsl.FunctionData third = statementData.Third.AsFunction;
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

        private List<IStoryFunction<string>> m_MsgIds = new List<IStoryFunction<string>>();
        private IStoryFunction<string> m_SetVar = new StoryValue<string>();
        private IStoryFunction m_SetVal = new StoryValue();
        private IStoryFunction<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryFunction<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryFunction m_TimeoutSetVal = new StoryValue();
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    scene.StorySystem.SuspendMessageHandler(m_MsgIds[i].Value, true);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryFunction<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }

        private List<IStoryFunction<string>> m_MsgIds = new List<IStoryFunction<string>>();
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
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    scene.StorySystem.SuspendMessageHandler(m_MsgIds[i].Value, false);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryFunction<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_MsgIds.Add(val);
            }
            return true;
        }

        private List<IStoryFunction<string>> m_MsgIds = new List<IStoryFunction<string>>();
    }
    /// <summary>
    /// sendserverstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class SendServerStoryMessageCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendServerStoryMessageCommand cmd = new SendServerStoryMessageCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, handler, iterator, args);
            m_Msg.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string _msg = m_Msg.Value;

                Msg_LRL_StoryMessage msg = new Msg_LRL_StoryMessage();
                msg.MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryFunction val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.NULL;
                        arg.str_val = "";
                        msg.Args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.Args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.Args.Add(arg);
                    } else {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.Args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            msg.UserGuid = us.Guid;
                            scene.GetRoomUserManager().SendServerMessage(msg);
                        }
                    }
                } else {
                    scene.GetRoomUserManager().SendServerMessage(msg);
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
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
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
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryFunction<int> m_UserId = new StoryValue<int>();
        private IStoryFunction<string> m_Msg = new StoryValue<string>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
    }
    /// <summary>
    /// sendclientstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class SendClientStoryMessageCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendClientStoryMessageCommand cmd = new SendClientStoryMessageCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, handler, iterator, args);
            m_Msg.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string _msg = m_Msg.Value;

                Msg_CRC_StoryMessage msg = new Msg_CRC_StoryMessage();
                msg.m_MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryFunction val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_CRC_StoryMessage.MessageArg arg = new Msg_CRC_StoryMessage.MessageArg();
                        arg.val_type = ArgType.NULL;
                        arg.str_val = "";
                        msg.m_Args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_CRC_StoryMessage.MessageArg arg = new Msg_CRC_StoryMessage.MessageArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.m_Args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_CRC_StoryMessage.MessageArg arg = new Msg_CRC_StoryMessage.MessageArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.m_Args.Add(arg);
                    } else {
                        Msg_CRC_StoryMessage.MessageArg arg = new Msg_CRC_StoryMessage.MessageArg();
                        arg.val_type = ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.m_Args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_CRC_StoryMessage, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_CRC_StoryMessage, msg);
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
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
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
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryFunction<int> m_UserId = new StoryValue<int>();
        private IStoryFunction<string> m_Msg = new StoryValue<string>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
    }
    /// <summary>
    /// publishgfxevent(ev_name,group,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class PublishGfxEventCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            PublishGfxEventCommand cmd = new PublishGfxEventCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_EventName = m_EventName.Clone();
            cmd.m_Group = m_Group.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, handler, iterator, args);
            m_EventName.Evaluate(instance, handler, iterator, args);
            m_Group.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string evname = m_EventName.Value;
                string group = m_Group.Value;

                Msg_RC_PublishEvent msg = new Msg_RC_PublishEvent();
                msg.group = group;
                msg.ev_name = evname;
                msg.is_logic_event = false;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryFunction val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_RC_PublishEvent.EventArg arg = new Msg_RC_PublishEvent.EventArg();
                        arg.val_type = ArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_RC_PublishEvent.EventArg arg = new Msg_RC_PublishEvent.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_RC_PublishEvent.EventArg arg = new Msg_RC_PublishEvent.EventArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_RC_PublishEvent.EventArg arg = new Msg_RC_PublishEvent.EventArg();
                        arg.val_type = ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_PublishEvent, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_PublishEvent, msg);
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
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
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
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryFunction<int> m_UserId = new StoryValue<int>();
        private IStoryFunction<string> m_EventName = new StoryValue<string>();
        private IStoryFunction<string> m_Group = new StoryValue<string>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
    }
    /// <summary>
    /// sendgfxmessage(objname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class SendGfxMessageCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendGfxMessageCommand cmd = new SendGfxMessageCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_ObjName = m_ObjName.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, handler, iterator, args);
            m_ObjName.Evaluate(instance, handler, iterator, args);
            m_Msg.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string objname = m_ObjName.Value;
                string _msg = m_Msg.Value;

                Msg_RC_SendGfxMessage msg = new Msg_RC_SendGfxMessage();
                msg.is_with_tag = false;
                msg.name = objname;
                msg.msg = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryFunction val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
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
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
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
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryFunction<int> m_UserId = new StoryValue<int>();
        private IStoryFunction<string> m_ObjName = new StoryValue<string>();
        private IStoryFunction<string> m_Msg = new StoryValue<string>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
    }
    /// <summary>
    /// sendgfxmessagewithtag(tagname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class SendGfxMessageWithTagCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SendGfxMessageWithTagCommand cmd = new SendGfxMessageWithTagCommand();
            cmd.m_HaveUserId = m_HaveUserId;
            cmd.m_UserId = m_UserId.Clone();
            cmd.m_ObjTag = m_ObjTag.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            if (m_HaveUserId)
                m_UserId.Evaluate(instance, handler, iterator, args);
            m_ObjTag.Evaluate(instance, handler, iterator, args);
            m_Msg.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryFunction val = m_Args[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                string objname = m_ObjTag.Value;
                string _msg = m_Msg.Value;

                Msg_RC_SendGfxMessage msg = new Msg_RC_SendGfxMessage();
                msg.is_with_tag = true;
                msg.name = objname;
                msg.msg = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryFunction val = m_Args[i];
                    var v = val.Value;
                    if (v.IsNullObject) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_RC_SendGfxMessage.EventArg arg = new Msg_RC_SendGfxMessage.EventArg();
                        arg.val_type = ArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserId) {
                    int userId = m_UserId.Value;
                    EntityInfo user = scene.GetEntityById(userId);
                    if (null != user) {
                        User us = user.CustomData as User;
                        if (null != us) {
                            us.SendMessage(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
                        }
                    }
                } else {
                    scene.NotifyAllUser(RoomMessageDefine.Msg_RC_SendGfxMessage, msg);
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
                Dsl.FunctionData first = statementData.First.AsFunction;
                Dsl.FunctionData second = statementData.Second.AsFunction;
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
                m_UserId.InitFromDsl(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IStoryFunction<int> m_UserId = new StoryValue<int>();
        private IStoryFunction<string> m_ObjTag = new StoryValue<string>();
        private IStoryFunction<string> m_Msg = new StoryValue<string>();
        private List<IStoryFunction> m_Args = new List<IStoryFunction>();
    }
    /// <summary>
    /// activescene(target_scene_id, obj_id);
    /// </summary>
    public class ActiveSceneCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ActiveSceneCommand cmd = new ActiveSceneCommand();
            cmd.m_TargetSceneId = m_TargetSceneId.Clone();
            cmd.m_ObjId = m_ObjId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_TargetSceneId.Evaluate(instance, handler, iterator, args);
            m_ObjId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int sceneId = m_TargetSceneId.Value;
                var idObj = m_ObjId.Value;
                if (idObj.IsInteger) {
                    int objId = idObj.GetInt();
                    EntityInfo entity = scene.GetEntityById(objId);
                    if (null != entity) {
                        User us = entity.CustomData as User;
                        if (null != us) {
                            us.OwnRoomUserManager.PlayerRequestActiveRoom(sceneId, us.Guid);
                        }
                    }
                } else {
                    IList<int> list = idObj.ObjectVal as IList<int>;
                    if (null != list) {
                        List<ulong> guids = new List<ulong>();
                        for (int i = 0; i < list.Count; ++i) {
                            EntityInfo entity = scene.GetEntityById(list[i]);
                            if (null != entity) {
                                User us = entity.CustomData as User;
                                if (null != us) {
                                    guids.Add(us.Guid);
                                }
                            }
                        }
                        if (guids.Count > 0) {
                            scene.GetRoomUserManager().PlayerRequestActiveRoom(sceneId, guids.ToArray());
                        }
                    }
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_TargetSceneId.InitFromDsl(callData.GetParam(0));
                m_ObjId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_TargetSceneId = new StoryValue<int>();
        private IStoryFunction m_ObjId = new StoryValue();
    }
    /// <summary>
    /// changescene(target_scene_id, obj_id);
    /// </summary>
    public class ChangeSceneCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ChangeSceneCommand cmd = new ChangeSceneCommand();
            cmd.m_TargetSceneId = m_TargetSceneId.Clone();
            cmd.m_ObjId = m_ObjId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_TargetSceneId.Evaluate(instance, handler, iterator, args);
            m_ObjId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int sceneId = m_TargetSceneId.Value;
                var idObj = m_ObjId.Value;
                if (idObj.IsInteger) {
                    int objId = idObj.GetInt();
                    EntityInfo entity = scene.GetEntityById(objId);
                    if (null != entity) {
                        User us = entity.CustomData as User;
                        if (null != us) {
                            us.OwnRoomUserManager.PlayerRequestChangeRoom(sceneId, us.Guid);
                        }
                    }
                } else {
                    IList<int> list = idObj.ObjectVal as IList<int>;
                    if (null != list) {
                        List<ulong> guids = new List<ulong>();
                        for (int i = 0; i < list.Count; ++i) {
                            EntityInfo entity = scene.GetEntityById(list[i]);
                            if (null != entity) {
                                User us = entity.CustomData as User;
                                if (null != us) {
                                    guids.Add(us.Guid);
                                }
                            }
                        }
                        if (guids.Count > 0) {
                            scene.GetRoomUserManager().PlayerRequestChangeRoom(sceneId, guids.ToArray());
                        }
                    }
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_TargetSceneId.InitFromDsl(callData.GetParam(0));
                m_ObjId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_TargetSceneId = new StoryValue<int>();
        private IStoryFunction m_ObjId = new StoryValue();
    }
    /// <summary>
    /// changeroomscene(target_scene_id);
    /// </summary>
    public class ChangeRoomSceneCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ChangeRoomSceneCommand cmd = new ChangeRoomSceneCommand();
            cmd.m_TargetSceneId = m_TargetSceneId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_TargetSceneId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int targetSceneId = m_TargetSceneId.Value;
                //RoomManager.Instance.ChangeRoomScene(scene.GetRoomSceneInfo().RoomId, targetSceneId);
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_TargetSceneId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryFunction<int> m_TargetSceneId = new StoryValue<int>();
    }
    /// <summary>
    /// createscenelogic(config_id,logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    public class CreateSceneLogicCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            CreateSceneLogicCommand cmd = new CreateSceneLogicCommand();
            cmd.m_ConfigId = m_ConfigId.Clone();
            cmd.m_Logic = m_Logic.Clone();
            cmd.m_Params = m_Params.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ConfigId.Evaluate(instance, handler, iterator, args);
            m_Logic.Evaluate(instance, handler, iterator, args);
            m_Params.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int configId = m_ConfigId.Value;
                int logicId = m_Logic.Value;
                IEnumerable args = m_Params.Value;
                List<string> list = new List<string>();
                foreach (string arg in args) {
                    list.Add(arg);
                }
                int id = scene.CreateSceneLogic(configId, logicId, list.ToArray());
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 2) {
                m_ConfigId.InitFromDsl(callData.GetParam(0));
                m_Logic.InitFromDsl(callData.GetParam(1));
                m_Params.InitFromDsl(callData.GetParam(2));
            }
            return true;
        }

        private IStoryFunction<int> m_ConfigId = new StoryValue<int>();
        private IStoryFunction<int> m_Logic = new StoryValue<int>();
        private IStoryFunction<IEnumerable> m_Params = new StoryValue<IEnumerable>();
    }
    /// <summary>
    /// destroyscenelogic(config_id);
    /// </summary>
    public class DestroySceneLogicCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            DestroySceneLogicCommand cmd = new DestroySceneLogicCommand();
            cmd.m_ConfigId = m_ConfigId.Clone();
            return cmd;
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ConfigId.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int configId = m_ConfigId.Value;
                scene.DestroySceneLogicByConfigId(configId);
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_ConfigId.InitFromDsl(callData.GetParam(0));
            }
            return true;
        }

        private IStoryFunction<int> m_ConfigId = new StoryValue<int>();
    }
    /// <summary>
    /// pausescenelogic(scene_logic_config_id,true_or_false);
    /// </summary>
    public class PauseSceneLogicCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            PauseSceneLogicCommand cmd = new PauseSceneLogicCommand();
            cmd.m_SceneLogicConfigId = m_SceneLogicConfigId.Clone();
            cmd.m_Enabled = m_Enabled.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_SceneLogicConfigId.Evaluate(instance, handler, iterator, args);
            m_Enabled.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int cfgId = m_SceneLogicConfigId.Value;
                string enabled = m_Enabled.Value;
                SceneLogicInfo info = scene.GetSceneLogicInfoByConfigId(cfgId);
                if (null != info) {
                    info.IsLogicPaused = (0 == string.Compare(enabled, "true"));
                } else {
                    LogSystem.Error("pausescenelogic can't find scenelogic {0}", cfgId);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_SceneLogicConfigId.InitFromDsl(callData.GetParam(0));
                m_Enabled.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private IStoryFunction<int> m_SceneLogicConfigId = new StoryValue<int>();
        private IStoryFunction<string> m_Enabled = new StoryValue<string>();
    }
    /// <summary>
    /// restarttimeout(scene_logic_config_id[,timeout]);
    /// </summary>
    public class RestartTimeoutCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            RestartTimeoutCommand cmd = new RestartTimeoutCommand();
            cmd.m_ParamNum = m_ParamNum;
            cmd.m_SceneLogicConfigId = m_SceneLogicConfigId.Clone();
            cmd.m_Timeout = m_Timeout.Clone();
            return cmd;
        }

        protected override void ResetState()
        {
        }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_SceneLogicConfigId.Evaluate(instance, handler, iterator, args);
            if (m_ParamNum > 1)
                m_Timeout.Evaluate(instance, handler, iterator, args);
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int cfgId = m_SceneLogicConfigId.Value;
                SceneLogicInfo info = scene.GetSceneLogicInfoByConfigId(cfgId);
                if (null != info) {
                    TimeoutLogicInfo data = info.LogicDatas.GetData<TimeoutLogicInfo>();
                    if (null != data) {
                        data.m_IsTriggered = false;
                        data.m_CurTime = 0;
                        if (m_ParamNum > 1) {
                            data.m_Timeout = m_Timeout.Value;
                        }
                    } else {
                        LogSystem.Warn("restarttimeout scenelogic {0} dosen't start, add wait command !", cfgId);
                    }
                } else {
                    LogSystem.Error("restarttimeout can't find scenelogic {0}", cfgId);
                }
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            m_ParamNum = callData.GetParamNum();
            if (m_ParamNum > 0) {
                m_SceneLogicConfigId.InitFromDsl(callData.GetParam(0));
            }
            if (m_ParamNum > 1) {
                m_Timeout.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }

        private int m_ParamNum = 0;
        private IStoryFunction<int> m_SceneLogicConfigId = new StoryValue<int>();
        private IStoryFunction<int> m_Timeout = new StoryValue<int>();
    }
    /// <summary>
    /// highlightprompt(objid,dictid,arg1,arg2,...);
    /// </summary>
    public class HighlightPromptCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            HighlightPromptCommand cmd = new HighlightPromptCommand();
            cmd.m_ObjId = m_ObjId.Clone();
            cmd.m_DictId = m_DictId.Clone();
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryFunction val = m_DictArgs[i];
                cmd.m_DictArgs.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_ObjId.Evaluate(instance, handler, iterator, args);
            m_DictId.Evaluate(instance, handler, iterator, args);
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryFunction val = m_DictArgs[i];
                val.Evaluate(instance, handler, iterator, args);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            Scene scene = instance.Context as Scene;
            if (null != scene) {
                int objId = m_ObjId.Value;
                string dictId = m_DictId.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_DictArgs.Count; ++i) {
                    IStoryFunction val = m_DictArgs[i];
                    arglist.Add(val.Value.GetObject());
                }
                object[] args = arglist.ToArray();
                scene.SceneContext.HighlightPrompt(objId, dictId, args);
            }
            return false;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjId.InitFromDsl(callData.GetParam(0));
                m_DictId.InitFromDsl(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_DictArgs.Add(val);
            }
            return true;
        }

        private IStoryFunction<int> m_ObjId = new StoryValue<int>();
        private IStoryFunction<string> m_DictId = new StoryValue<string>();
        private List<IStoryFunction> m_DictArgs = new List<IStoryFunction>();
    }
}
