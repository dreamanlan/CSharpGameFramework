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
    /// startstory(story_id);
    /// </summary>
    public class StartStoryCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                var storyId = m_StoryId.Calc();
                var multiple = m_Multiple.Calc().GetInt();
                scene.DelayActionProcessor.QueueAction(() => {
                    if (multiple == 0)
                        scene.StorySystem.StartStory(storyId);
                    else
                        scene.StorySystem.StartStories(storyId);
                });
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryId = Calculator.Load(callData.GetParam(0));
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
                        m_Multiple = Calculator.Load(call.GetParam(0));
                    }
                }
            }
            return true;
        }
        private IExpression m_StoryId;
        private IExpression m_Multiple;
    }
    /// <summary>
    /// stopstory(story_id);
    /// </summary>
    public class StopStoryCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                var multiple = m_Multiple.Calc().GetInt();
                if (multiple == 0)
                    scene.StorySystem.MarkStoryTerminated(m_StoryId.Calc());
                else
                    scene.StorySystem.MarkStoriesTerminated(m_StoryId.Calc());
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryId = Calculator.Load(callData.GetParam(0));
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
                        m_Multiple = Calculator.Load(call.GetParam(0));
                    }
                }
            }
            return true;
        }
        private IExpression m_StoryId;
        private IExpression m_Multiple;
    }
    /// <summary>
    /// waitstory(storyid1,storyid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    public class WaitStoryCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }

        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            List<string> storyIds = new List<string>();
            for (int i = 0; i < m_StoryIds.Count; ++i) {
                storyIds.Add(m_StoryIds[i].Calc().ToString());
            }
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null == instance || null == scene) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
            int curTime = 0;
            while (true) {
                int ct = 0;
                for (int i = 0; i < storyIds.Count; i++) {
                    ct += scene.StorySystem.CountStory(storyIds[i]);
                }
                int multiple = m_HaveMultiple ? m_Multiple.Calc().GetInt() : 0;
                if (ct <= 0) {
                    if (m_HaveSet) {
                        string varName = m_SetVar.Calc().ToString();
                        var varVal = m_SetVal.Calc();
                        instance.SetVariable(varName, varVal);
                    }
                    break;
                } else {
                    if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || curTime <= timeout)) {
                        yield return null;
                        curTime += 1;
                    } else if (m_HaveSet) {
                        string varName = m_TimeoutSetVar.Calc().ToString();
                        var varVal = m_TimeoutSetVal.Calc();
                        instance.SetVariable(varName, varVal);
                        break;
                    }
                }
            }
            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_StoryIds.Add(Calculator.Load(callData.GetParam(i)));
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
                m_Multiple = Calculator.Load(callData.GetParam(0));
            }
        }
        private void LoadSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 2 && callData.GetId() == "set") {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
            }
        }
        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3 && callData.GetId() == "timeoutset") {
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
    /// pausestory(storyid1,storyid2,...);
    /// </summary>
    public class PauseStoryCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                var multiple = m_Multiple.Calc().GetInt();
                if (multiple == 0) {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        scene.StorySystem.PauseStory(m_StoryIds[i].Calc().ToString(), true);
                    }
                } else {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        scene.StorySystem.PauseStories(m_StoryIds[i].Calc().ToString(), true);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_StoryIds.Add(Calculator.Load(callData.GetParam(i)));
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
                        m_Multiple = Calculator.Load(call.GetParam(0));
                    }
                }
            }
            return true;
        }
        private List<IExpression> m_StoryIds = new List<IExpression>();
        private IExpression m_Multiple;
    }
    /// <summary>
    /// resumestory(storyid1,storyid2,...);
    /// </summary>
    public class ResumeStoryCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                var multiple = m_Multiple.Calc().GetInt();
                if (multiple == 0) {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        scene.StorySystem.PauseStory(m_StoryIds[i].Calc().ToString(), false);
                    }
                } else {
                    for (int i = 0; i < m_StoryIds.Count; i++) {
                        scene.StorySystem.PauseStories(m_StoryIds[i].Calc().ToString(), false);
                    }
                }
            }
            return BoxedValue.NullObject;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_StoryIds.Add(Calculator.Load(callData.GetParam(i)));
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
                        m_Multiple = Calculator.Load(call.GetParam(0));
                    }
                }
            }
            return true;
        }
        private List<IExpression> m_StoryIds = new List<IExpression>();
        private IExpression m_Multiple;
    }
    /// <summary>
    /// firemessage(msgid,arg1,arg2,...);
    /// </summary>
    public class FireMessageCommand : SimpleExpressionBase
    {
        public FireMessageCommand()
        {
        }
        public FireMessageCommand(bool isConcurrent)
        {
            m_IsConcurrent = isConcurrent;
        }
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                if (operands.Count > 0) {
                    string msgId = operands[0].ToString();
                    var args = scene.StorySystem.NewBoxedValueList();
                    for (int i = 1; i < operands.Count; ++i) {
                        args.Add(operands[i]);
                    }
                    if (m_IsConcurrent)
                        scene.StorySystem.SendConcurrentMessage(msgId, args);
                    else
                        scene.StorySystem.SendMessage(msgId, args);
                }
            }
            return BoxedValue.NullObject;
        }

        private bool m_IsConcurrent = false;
    }
    internal sealed class FireConcurrentMessageCommand : FireMessageCommand
    {
        public FireConcurrentMessageCommand()
            : base(true)
        {
        }
    }
    /// <summary>
    /// waitallmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    public class WaitAllMessageCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }

        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            List<string> msgIds = new List<string>();
            for (int i = 0; i < m_MsgIds.Count; ++i) {
                msgIds.Add(m_MsgIds[i].Calc().ToString());
            }
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null == instance || null == scene) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            long startTime = 0;
            if (m_StartTime <= 0) {
                long st = ScriptableFramework.TimeUtility.GetLocalMilliseconds();
                m_StartTime = st;
            }
            startTime = m_StartTime;
            while (true) {
                bool triggered = false;
                for (int i = 0; i < msgIds.Count; i++) {
                    long time = instance.GetMessageTriggerTime(msgIds[i]);
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
                } else {
                    int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
                    if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || m_CurTime <= timeout)) {
                        yield return null;
                        m_CurTime += 1;
                    } else {
                        if (m_HaveSet) {
                            string varName = m_TimeoutSetVar.Calc().ToString();
                            var varVal = m_TimeoutSetVal.Calc();
                            instance.SetVariable(varName, varVal);
                        }
                        break;
                    }
                }
            }
            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_MsgIds.Add(Calculator.Load(callData.GetParam(i)));
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
            if (num >= 2 && callData.GetId() == "set") {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3 && callData.GetId() == "timeoutset") {
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
        private int m_CurTime = 0;
        private long m_StartTime = 0;
    }
    /// <summary>
    /// waitallmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    public class WaitAllMessageHandlerCommand : AbstractExpression
    {
        public override bool IsAsync { get { return true; } }

        protected override IEnumerator DoCalc(AsyncCalcResult result)
        {
            if (Calculator.IsInSyncCalculation) {
                yield break;
            }
            List<string> msgIds = new List<string>();
            for (int i = 0; i < m_MsgIds.Count; ++i) {
                msgIds.Add(m_MsgIds[i].Calc().ToString());
            }
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null == instance || null == scene) {
                result.Value = BoxedValue.NullObject;
                yield break;
            }
            while (true) {
                int ct = 0;
                for (int i = 0; i < msgIds.Count; i++) {
                    ct += scene.StorySystem.CountMessage(msgIds[i]);
                }
                if (ct <= 0) {
                    if (m_HaveSet) {
                        string varName = m_SetVar.Calc().ToString();
                        var varVal = m_SetVal.Calc();
                        instance.SetVariable(varName, varVal);
                    }
                    break;
                } else {
                    int timeout = m_HaveSet ? m_TimeoutVal.Calc().GetInt() : 0;
                    if (!StoryConfigManager.Instance.IsStorySkipped && (timeout <= 0 || m_CurTime <= timeout)) {
                        yield return null;
                        m_CurTime += 1;
                    } else {
                        if (m_HaveSet) {
                            string varName = m_TimeoutSetVar.Calc().ToString();
                            var varVal = m_TimeoutSetVal.Calc();
                            instance.SetVariable(varName, varVal);
                        }
                        break;
                    }
                }
            }
            result.Value = BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                m_MsgIds.Add(Calculator.Load(callData.GetParam(i)));
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
            if (num >= 2 && callData.GetId() == "set") {
                m_SetVar = Calculator.Load(callData.GetParam(0));
                m_SetVal = Calculator.Load(callData.GetParam(1));
            }
        }

        private void LoadTimeoutSet(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num >= 3 && callData.GetId() == "timeoutset") {
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
        private int m_CurTime = 0;
    }
    /// <summary>
    /// pauseallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    public class SuspendAllMessageHandlerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < operands.Count; i++) {
                    scene.StorySystem.SuspendMessageHandler(operands[i].ToString(), true);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// resumeallmessagehandler(msgid1,msgid2,...);
    /// </summary>
    public class ResumeAllMessageHandlerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                for (int i = 0; i < operands.Count; i++) {
                    scene.StorySystem.SuspendMessageHandler(operands[i].ToString(), false);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// sendserverstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class SendServerStoryMessageCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
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
                if (m_HaveUserId) {
                    int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Msg = Calculator.Load(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                m_Args.Add(Calculator.Load(callData.GetParam(i)));
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
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendclientstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class SendClientStoryMessageCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                string _msg = m_Msg.Calc().ToString();
                Msg_CRC_StoryMessage msg = new Msg_CRC_StoryMessage();
                msg.m_MsgId = _msg;
                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
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
                    int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_Msg = Calculator.Load(callData.GetParam(0));
            }
            for (int i = 1; i < callData.GetParamNum(); ++i) {
                m_Args.Add(Calculator.Load(callData.GetParam(i)));
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
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// publishgfxevent(ev_name,group,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class PublishGfxEventCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                string evname = m_EventName.Calc().ToString();
                string group = m_Group.Calc().ToString();
                Msg_RC_PublishEvent msg = new Msg_RC_PublishEvent();
                msg.group = group;
                msg.ev_name = evname;
                msg.is_logic_event = false;
                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
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
                    int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_EventName = Calculator.Load(callData.GetParam(0));
                m_Group = Calculator.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                m_Args.Add(Calculator.Load(callData.GetParam(i)));
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
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_EventName;
        private IExpression m_Group;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendgfxmessage(objname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class SendGfxMessageCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                string objname = m_ObjName.Calc().ToString();
                string _msg = m_Msg.Calc().ToString();
                Msg_RC_SendGfxMessage msg = new Msg_RC_SendGfxMessage();
                msg.is_with_tag = false;
                msg.name = objname;
                msg.msg = _msg;
                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
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
                    int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjName = Calculator.Load(callData.GetParam(0));
                m_Msg = Calculator.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                m_Args.Add(Calculator.Load(callData.GetParam(i)));
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
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_ObjName;
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// sendgfxmessagewithtag(tagname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    public class SendGfxMessageWithTagCommand : AbstractExpression
    {
        protected override BoxedValue DoCalc()
        {
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                string objname = m_ObjTag.Calc().ToString();
                string _msg = m_Msg.Calc().ToString();
                Msg_RC_SendGfxMessage msg = new Msg_RC_SendGfxMessage();
                msg.is_with_tag = true;
                msg.name = objname;
                msg.msg = _msg;
                for (int i = 0; i < m_Args.Count; ++i) {
                    var v = m_Args[i].Calc();
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
                    int userId = m_UserId.Calc().GetInt();
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
            return BoxedValue.NullObject;
        }

        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_ObjTag = Calculator.Load(callData.GetParam(0));
                m_Msg = Calculator.Load(callData.GetParam(1));
            }
            for (int i = 2; i < callData.GetParamNum(); ++i) {
                m_Args.Add(Calculator.Load(callData.GetParam(i)));
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
                m_UserId = Calculator.Load(callData.GetParam(0));
                m_HaveUserId = true;
            }
        }

        private bool m_HaveUserId = false;
        private IExpression m_UserId;
        private IExpression m_ObjTag;
        private IExpression m_Msg;
        private List<IExpression> m_Args = new List<IExpression>();
    }
    /// <summary>
    /// activescene(target_scene_id, obj_id);
    /// </summary>
    public class ActiveSceneCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int sceneId = operands[0].GetInt();
                var idObj = operands[1];
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// changescene(target_scene_id, obj_id);
    /// </summary>
    public class ChangeSceneCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int sceneId = operands[0].GetInt();
                var idObj = operands[1];
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
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// changeroomscene(target_scene_id);
    /// </summary>
    public class ChangeRoomSceneCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int targetSceneId = operands[0].GetInt();
                //RoomManager.Instance.ChangeRoomScene(scene.GetRoomSceneInfo().RoomId, targetSceneId);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// createscenelogic(config_id,logic_id,stringlist("param1 param2 param3 ..."));
    /// </summary>
    public class CreateSceneLogicCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 2)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int configId = operands[0].GetInt();
                int logicId = operands[1].GetInt();
                IEnumerable args = operands[2].GetObject() as IEnumerable;
                List<string> list = new List<string>();
                foreach (string arg in args) {
                    list.Add(arg);
                }
                int id = scene.CreateSceneLogic(configId, logicId, list.ToArray());
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// destroyscenelogic(config_id);
    /// </summary>
    public class DestroySceneLogicCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int configId = operands[0].GetInt();
                scene.DestroySceneLogicByConfigId(configId);
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// pausescenelogic(scene_logic_config_id,true_or_false);
    /// </summary>
    public class PauseSceneLogicCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int cfgId = operands[0].GetInt();
                string enabled = operands[1].ToString();
                SceneLogicInfo info = scene.GetSceneLogicInfoByConfigId(cfgId);
                if (null != info) {
                    info.IsLogicPaused = (0 == string.Compare(enabled, "true"));
                } else {
                    LogSystem.Error("pausescenelogic can't find scenelogic {0}", cfgId);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// restarttimeout(scene_logic_config_id[,timeout]);
    /// </summary>
    public class RestartTimeoutCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 0)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int cfgId = operands[0].GetInt();
                SceneLogicInfo info = scene.GetSceneLogicInfoByConfigId(cfgId);
                if (null != info) {
                    TimeoutLogicInfo data = info.LogicDatas.GetData<TimeoutLogicInfo>();
                    if (null != data) {
                        data.m_IsTriggered = false;
                        data.m_CurTime = 0;
                        if (operands.Count > 1) {
                            data.m_Timeout = operands[1].GetInt();
                        }
                    } else {
                        LogSystem.Warn("restarttimeout scenelogic {0} dosen't start, add wait command !", cfgId);
                    }
                } else {
                    LogSystem.Error("restarttimeout can't find scenelogic {0}", cfgId);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    /// <summary>
    /// highlightprompt(objid,dictid,arg1,arg2,...);
    /// </summary>
    public class HighlightPromptCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count <= 1)
                return BoxedValue.NullObject;
            var instance = Calculator.GetFuncContext<StoryInstance>();
            Scene scene = instance?.Context as Scene;
            if (null != scene) {
                int objId = operands[0].GetInt();
                string dictId = operands[1].ToString();
                ArrayList arglist = new ArrayList();
                for (int i = 2; i < operands.Count; ++i) {
                    arglist.Add(operands[i].GetObject());
                }
                object[] args = arglist.ToArray();
                scene.SceneContext.HighlightPrompt(objId, dictId, args);
            }
            return BoxedValue.NullObject;
        }
    }
}
