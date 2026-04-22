using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;
using ScriptableFrameworkMessage;

namespace ScriptableFramework.Story.Commands
{
    /// <summary>
    /// startstory(story_id)[multiple(n)];
    /// </summary>
    internal sealed class StartStoryCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                var storyId = m_StoryId.Calc().ToString();
                var multiple = m_HaveMultiple ? m_Multiple.Calc().GetInt() : 0;
                userThread.QueueAction(() => {
                    if (multiple == 0)
                        userThread.StorySystem.StartStory(storyId);
                    else
                        userThread.StorySystem.StartStories(storyId);
                });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.GetParamNum() > 0) {
                m_StoryId = Calculator.Load(funcData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    if (second.GetId() == "multiple" && second.GetParamNum() > 0) {
                        m_HaveMultiple = true;
                        m_Multiple = Calculator.Load(second.GetParam(0));
                    }
                }
            }
            return true;
        }

        private IExpression m_StoryId;
        private IExpression m_Multiple;
        private bool m_HaveMultiple = false;
    }
    /// <summary>
    /// stopstory(story_id)[multiple(n)];
    /// </summary>
    internal sealed class StopStoryCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                var storyId = m_StoryId.Calc().ToString();
                var multiple = m_HaveMultiple ? m_Multiple.Calc().GetInt() : 0;
                if (multiple == 0)
                    userThread.StorySystem.MarkStoryTerminated(storyId);
                else
                    userThread.StorySystem.MarkStoriesTerminated(storyId);
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData funcData)
        {
            if (funcData.GetParamNum() > 0) {
                m_StoryId = Calculator.Load(funcData.GetParam(0));
            }
            return true;
        }
        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    if (second.GetId() == "multiple" && second.GetParamNum() > 0) {
                        m_HaveMultiple = true;
                        m_Multiple = Calculator.Load(second.GetParam(0));
                    }
                }
            }
            return true;
        }

        private IExpression m_StoryId;
        private IExpression m_Multiple;
        private bool m_HaveMultiple = false;
    }
    /// <summary>
    /// waitstory(storyid1,storyid2,...)[set(var,val)timeoutset(timeout,var,val)][multiple(n)];
    /// </summary>
    internal sealed class WaitStoryCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }

        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (userThread == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            int multiple = m_HaveMultiple ? m_Multiple.Calc().GetInt() : 0;
            int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
            long startTime = instance.CurrentTime;

            while (true) {
                int ct = 0;
                for (int i = 0; i < m_StoryIds.Count; i++) {
                    string sid = m_StoryIds[i].Calc().ToString();
                    if (multiple == 0)
                        ct += userThread.StorySystem.CountStory(sid);
                    else
                        ct += userThread.StorySystem.CountStory(sid);
                }
                if (ct <= 0) {
                    if (m_HaveSet) {
                        string varName = m_SetVar.Calc().ToString();
                        var varVal = m_SetVal.Calc();
                        instance.SetVariable(varName, varVal);
                    }
                    break;
                }
                long curTime = instance.CurrentTime - startTime;
                if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || curTime <= timeout)) {
                    yield return null;
                } else {
                    if (m_HaveSet) {
                        string varName = m_TimeoutSetVar.Calc().ToString();
                        var varVal = m_TimeoutSetVal.Calc();
                        instance.SetVariable(varName, varVal);
                    }
                    break;
                }
            }
            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadStoryIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            int ct = statementData.Functions.Count;
            if (ct >= 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                if (ct == 2) {
                    Load(first);
                    LoadMultiple(second);
                } else if (ct == 3) {
                    var third = statementData.Third.AsFunction;
                    if (null != first && null != second && null != third) {
                        m_HaveSet = true;
                        LoadStoryIds(first);
                        LoadSet(second);
                        LoadTimeoutSet(third);
                    }
                } else if (ct == 4) {
                    var third = statementData.Third.AsFunction;
                    var last = statementData.Last.AsFunction;
                    if (null != first && null != second && null != third && null != last) {
                        m_HaveSet = true;
                        LoadStoryIds(first);
                        LoadSet(second);
                        LoadTimeoutSet(third);
                        LoadMultiple(last);
                    }
                }
            }
            return true;
        }

        private void LoadStoryIds(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_StoryIds.Add(Calculator.Load(callData.GetParam(i)));
            }
        }

        private void LoadMultiple(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "multiple" && callData.GetParamNum() >= 1) {
                m_HaveMultiple = true;
                m_Multiple = Calculator.Load(callData.GetParam(0));
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "set" && callData.GetParamNum() >= 2) {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            if (callData.GetId() == "timeoutset" && callData.GetParamNum() >= 3) {
                m_TimeoutVal = Calculator.Load(callData.GetParam(0));
                m_TimeoutSetVar = Calculator.Load(callData.GetParam(1));
                m_TimeoutSetVal = Calculator.Load(callData.GetParam(2));
            }
        }

        private List<IExpression> m_StoryIds = new List<IExpression>();
        private IExpression m_SetVar;
        private IExpression m_SetVal;
        private IExpression m_TimeoutVal;
        private IExpression m_TimeoutSetVar;
        private IExpression m_TimeoutSetVal;
        private IExpression m_Multiple;
        private bool m_HaveMultiple = false;
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// pausestory(storyid1,storyid2,...)[multiple(n)];
    /// </summary>
    internal sealed class PauseStoryCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                var multiple = m_HaveMultiple ? m_Multiple.Calc().GetInt() : 0;
                if (multiple == 0) {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        userThread.StorySystem.PauseStory(m_StoryIds[i].Calc().ToString(), true);
                    }
                } else {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        userThread.StorySystem.PauseStories(m_StoryIds[i].Calc().ToString(), true);
                    }
                }
            }
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            int num = funcData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_StoryIds.Add(Calculator.Load(funcData.GetParam(i)));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    if (second.GetId() == "multiple" && second.GetParamNum() > 0) {
                        m_HaveMultiple = true;
                        m_Multiple = Calculator.Load(second.GetParam(0));
                    }
                }
            }
            return true;
        }

        private List<IExpression> m_StoryIds = new List<IExpression>();
        private IExpression m_Multiple;
        private bool m_HaveMultiple = false;
    }
    /// <summary>
    /// resumestory(storyid1,storyid2,...)[multiple(n)];
    /// </summary>
    internal sealed class ResumeStoryCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                var multiple = m_HaveMultiple ? m_Multiple.Calc().GetInt() : 0;
                if (multiple == 0) {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        userThread.StorySystem.PauseStory(m_StoryIds[i].Calc().ToString(), false);
                    }
                } else {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        userThread.StorySystem.PauseStories(m_StoryIds[i].Calc().ToString(), false);
                    }
                }
            }
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            int num = funcData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_StoryIds.Add(Calculator.Load(funcData.GetParam(i)));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.GetFunctionNum() == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                if (null != first && null != second) {
                    Load(first);
                    if (second.GetId() == "multiple" && second.GetParamNum() > 0) {
                        m_HaveMultiple = true;
                        m_Multiple = Calculator.Load(second.GetParam(0));
                    }
                }
            }
            return true;
        }

        private List<IExpression> m_StoryIds = new List<IExpression>();
        private IExpression m_Multiple;
        private bool m_HaveMultiple = false;
    }
    /// <summary>
    /// firemessage(msgid,arg1,arg2,...);
    /// </summary>
    internal sealed class FireMessageCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                string msgId = operands[0].ToString();
                var args = userThread.StorySystem.NewBoxedValueList();
                for (int i = 1; i < operands.Count; ++i) {
                    args.Add(operands[i]);
                }
                userThread.StorySystem.SendMessage(msgId, args);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// fireconcurrentmessage(msgid,arg1,arg2,...);
    /// </summary>
    internal sealed class FireConcurrentMessageCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                string msgId = operands[0].ToString();
                var args = userThread.StorySystem.NewBoxedValueList();
                for (int i = 1; i < operands.Count; ++i) {
                    args.Add(operands[i]);
                }
                userThread.StorySystem.SendConcurrentMessage(msgId, args);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// waitallmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal sealed class WaitAllMessageCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }

        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (userThread == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
            long startTime = ScriptableFramework.TimeUtility.GetLocalMilliseconds();
            long storyStartTime = instance.CurrentTime;

            while (true) {
                bool triggered = false;
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    long time = instance.GetMessageTriggerTime(m_MsgIds[i].Calc().ToString());
                    if (time > startTime) {
                        triggered = true;
                        break;
                    }
                }
                if (triggered) {
                    if (m_HaveSet) {
                        string varName = m_SetVar.Calc().ToString();
                        var varVal = m_SetVal.Calc();
                        instance.SetVariable(varName, varVal);
                    }
                    break;
                }
                long curTime = instance.CurrentTime - storyStartTime;
                if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || curTime <= timeout)) {
                    yield return null;
                } else {
                    if (m_HaveSet) {
                        string varName = m_TimeoutSetVar.Calc().ToString();
                        var varVal = m_TimeoutSetVal.Calc();
                        instance.SetVariable(varName, varVal);
                    }
                    break;
                }
            }
            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                var third = statementData.Third.AsFunction;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;
                    LoadMsgIds(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
            return true;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_MsgIds.Add(Calculator.Load(callData.GetParam(i)));
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            if (callData.GetParamNum() >= 2) {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            if (callData.GetParamNum() >= 3) {
                m_TimeoutVal = Calculator.Load(callData.GetParam(0));
                m_TimeoutSetVar = Calculator.Load(callData.GetParam(1));
                m_TimeoutSetVal = Calculator.Load(callData.GetParam(2));
            }
        }

        private List<IExpression> m_MsgIds = new List<IExpression>();
        private IExpression m_SetVar;
        private IExpression m_SetVal;
        private IExpression m_TimeoutVal;
        private IExpression m_TimeoutSetVar;
        private IExpression m_TimeoutSetVal;
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// waitallmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal sealed class WaitAllMessageHandlerCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }

        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (userThread == null) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }

            int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
            long startTime = instance.CurrentTime;

            while (true) {
                int ct = 0;
                for (int i = 0; i < m_MsgIds.Count; i++) {
                    ct += userThread.StorySystem.CountMessage(m_MsgIds[i].Calc().ToString());
                }
                if (ct <= 0) {
                    if (m_HaveSet) {
                        string varName = m_SetVar.Calc().ToString();
                        var varVal = m_SetVal.Calc();
                        instance.SetVariable(varName, varVal);
                    }
                    break;
                }
                long curTime = instance.CurrentTime - startTime;
                if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || curTime <= timeout)) {
                    yield return null;
                } else {
                    if (m_HaveSet) {
                        string varName = m_TimeoutSetVar.Calc().ToString();
                        var varVal = m_TimeoutSetVal.Calc();
                        instance.SetVariable(varName, varVal);
                    }
                    break;
                }
            }
            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            LoadMsgIds(funcData);
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count >= 3) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
                var third = statementData.Third.AsFunction;
                if (null != first && null != second && null != third) {
                    m_HaveSet = true;
                    LoadMsgIds(first);
                    LoadSet(second);
                    LoadTimeoutSet(third);
                }
            }
            return true;
        }

        private void LoadMsgIds(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_MsgIds.Add(Calculator.Load(callData.GetParam(i)));
            }
        }

        private void LoadSet(Dsl.FunctionData callData)
        {
            if (callData.GetParamNum() >= 2) {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            if (callData.GetParamNum() >= 3) {
                m_TimeoutVal = Calculator.Load(callData.GetParam(0));
                m_TimeoutSetVar = Calculator.Load(callData.GetParam(1));
                m_TimeoutSetVal = Calculator.Load(callData.GetParam(2));
            }
        }

        private List<IExpression> m_MsgIds = new List<IExpression>();
        private IExpression m_SetVar;
        private IExpression m_SetVal;
        private IExpression m_TimeoutVal;
        private IExpression m_TimeoutSetVar;
        private IExpression m_TimeoutSetVal;
        private bool m_HaveSet = false;
    }
    /// <summary>
    /// pauseallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    public sealed class SuspendAllMessageHandlerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                for (int i = 0; i < operands.Count; i++) {
                    userThread.StorySystem.SuspendMessageHandler(operands[i].ToString(), true);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// resumeallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    public sealed class ResumeAllMessageHandlerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                for (int i = 0; i < operands.Count; i++) {
                    userThread.StorySystem.SuspendMessageHandler(operands[i].ToString(), false);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// sendserverstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal sealed class SendServerStoryMessageCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                string _msg = m_Msg.Calc().ToString();

                Msg_LRL_StoryMessage msg = new Msg_LRL_StoryMessage();
                msg.MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
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
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Calc().GetULong();
                    msg.UserGuid = userGuid;
                    userThread.SendServerMessage(msg);
                } else {
                    userThread.SendServerMessage(msg);
                }
            }
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            int num = funcData.GetParamNum();
            if (num > 0) {
                m_Msg = Calculator.Load(funcData.GetParam(0));
            }
            for (int i = 1; i < num; ++i) {
                m_Args.Add(Calculator.Load(funcData.GetParam(i)));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
                m_UserGuid = Calculator.Load(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IExpression m_UserGuid;
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendclientstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal sealed class SendClientStoryMessageCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                string _msg = m_Msg.Calc().ToString();

                Msg_CLC_StoryMessage msg = new Msg_CLC_StoryMessage();
                msg.m_MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
                    if (v.IsNullObject) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.m_Args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.m_Args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.m_Args.Add(arg);
                    } else {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.m_Args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Calc().GetULong();
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_CLC_StoryMessage, msg);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_CLC_StoryMessage, msg);
                }
            }
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            int num = funcData.GetParamNum();
            if (num > 0) {
                m_Msg = Calculator.Load(funcData.GetParam(0));
            }
            for (int i = 1; i < num; ++i) {
                m_Args.Add(Calculator.Load(funcData.GetParam(i)));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
                m_UserGuid = Calculator.Load(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IExpression m_UserGuid;
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// publishgfxevent(ev_name,group,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal sealed class PublishGfxEventCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                string evname = m_EventName.Calc().ToString();
                string group = m_Group.Calc().ToString();

                Msg_LC_PublishEvent msg = new Msg_LC_PublishEvent();
                msg.group = group;
                msg.ev_name = evname;
                msg.is_logic_event = false;

                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
                    if (v.IsNullObject) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Calc().GetULong();
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_LC_PublishEvent, msg);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_LC_PublishEvent, msg);
                }
            }
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            int num = funcData.GetParamNum();
            if (num > 1) {
                m_EventName = Calculator.Load(funcData.GetParam(0));
                m_Group = Calculator.Load(funcData.GetParam(1));
            }
            for (int i = 2; i < num; ++i) {
                m_Args.Add(Calculator.Load(funcData.GetParam(i)));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
                m_UserGuid = Calculator.Load(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IExpression m_UserGuid;
        private IExpression m_EventName;
        private IExpression m_Group;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendgfxmessage(objname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal sealed class SendGfxMessageCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                string objname = m_ObjName.Calc().ToString();
                string _msg = m_Msg.Calc().ToString();

                Msg_LC_SendGfxMessage msg = new Msg_LC_SendGfxMessage();
                msg.is_with_tag = false;
                msg.name = objname;
                msg.msg = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
                    if (v.IsNullObject) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Calc().GetULong();
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_LC_SendGfxMessage, msg);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_LC_SendGfxMessage, msg);
                }
            }
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            int num = funcData.GetParamNum();
            if (num > 1) {
                m_ObjName = Calculator.Load(funcData.GetParam(0));
                m_Msg = Calculator.Load(funcData.GetParam(1));
            }
            for (int i = 2; i < num; ++i) {
                m_Args.Add(Calculator.Load(funcData.GetParam(i)));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
                m_UserGuid = Calculator.Load(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IExpression m_UserGuid;
        private IExpression m_ObjName;
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendgfxmessagewithtag(tagname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal sealed class SendGfxMessageWithTagCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                string objname = m_ObjTag.Calc().ToString();
                string _msg = m_Msg.Calc().ToString();

                Msg_LC_SendGfxMessage msg = new Msg_LC_SendGfxMessage();
                msg.is_with_tag = true;
                msg.name = objname;
                msg.msg = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
                    if (v.IsNullObject) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v.IsInteger) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = v.GetInt().ToString();
                        msg.args.Add(arg);
                    } else if (v.IsNumber) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = v.GetFloat().ToString();
                        msg.args.Add(arg);
                    } else {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.STRING;
                        arg.str_val = v.ToString();
                        msg.args.Add(arg);
                    }
                }
                if (m_HaveUserGuid) {
                    ulong userGuid = m_UserGuid.Calc().GetULong();
                    userThread.NotifyUser(userGuid, LobbyMessageDefine.Msg_LC_SendGfxMessage, msg);
                } else {
                    userThread.NotifyAllUser(LobbyMessageDefine.Msg_LC_SendGfxMessage, msg);
                }
            }
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData funcData)
        {
            int num = funcData.GetParamNum();
            if (num > 1) {
                m_ObjTag = Calculator.Load(funcData.GetParam(0));
                m_Msg = Calculator.Load(funcData.GetParam(1));
            }
            for (int i = 2; i < num; ++i) {
                m_Args.Add(Calculator.Load(funcData.GetParam(i)));
            }
            return true;
        }

        protected override bool Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                var first = statementData.First.AsFunction;
                var second = statementData.Second.AsFunction;
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
                m_UserGuid = Calculator.Load(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IExpression m_UserGuid;
        private IExpression m_ObjTag;
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// highlightprompt(userguid,dictid,arg1,arg2,...);
    /// </summary>
    internal sealed class HighlightPromptCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            UserThread userThread = instance?.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = operands[0].GetULong();
                string dictId = operands[1].ToString();
                ArrayList arglist = new ArrayList();
                for (int i = 2; i < operands.Count; ++i) {
                    arglist.Add(operands[i].GetObject());
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
            return BoxedValue.NullObject;
        }
    }
}
