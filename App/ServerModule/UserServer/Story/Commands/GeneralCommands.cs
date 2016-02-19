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
        public override IStoryCommand Clone()
        {
            StartStoryCommand cmd = new StartStoryCommand();
            cmd.m_StoryId = m_StoryId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_StoryId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_StoryId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                userThread.QueueAction(userThread.StorySystem.StartStory, m_StoryId.Value);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<string> m_StoryId = new StoryValue<string>();
    }
    /// <summary>
    /// stopstory(story_id);
    /// </summary>
    internal class StopStoryCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            StopStoryCommand cmd = new StopStoryCommand();
            cmd.m_StoryId = m_StoryId.Clone();
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_StoryId.Substitute(iterator, args);
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_StoryId.Evaluate(instance);
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                userThread.StorySystem.MarkStoryTerminated(m_StoryId.Value);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            if (num > 0) {
                m_StoryId.InitFromDsl(callData.GetParam(0));
            }
        }

        private IStoryValue<string> m_StoryId = new StoryValue<string>();
    }
    /// <summary>
    /// waitstory(storyid1,storyid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitStoryCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
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
            cmd.m_HaveSet = m_HaveSet;
            return cmd;
        }

        protected override void ResetState()
        {
            m_CurTime = 0;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Substitute(iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Substitute(iterator, args);
                m_SetVal.Substitute(iterator, args);
                m_TimeoutVal.Substitute(iterator, args);
                m_TimeoutSetVar.Substitute(iterator, args);
                m_TimeoutSetVal.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_StoryIds.Count; i++) {
                m_StoryIds[i].Evaluate(instance);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance);
                m_SetVal.Evaluate(instance);
                m_TimeoutVal.Evaluate(instance);
                m_TimeoutSetVar.Evaluate(instance);
                m_TimeoutSetVal.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            bool ret = false;
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                int ct = 0;
                for (int i = 0; i < m_StoryIds.Count; i++) {
                    ct += userThread.StorySystem.CountStory(m_StoryIds[i].Value);
                }
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
            }
            return ret;
        }

        protected override void Load(Dsl.CallData callData)
        {
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                IStoryValue<string> val = new StoryValue<string>();
                val.InitFromDsl(callData.GetParam(i));
                m_StoryIds.Add(val);
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

        private List<IStoryValue<string>> m_StoryIds = new List<IStoryValue<string>>();
        private IStoryValue<string> m_SetVar = new StoryValue<string>();
        private IStoryValue<object> m_SetVal = new StoryValue();
        private IStoryValue<int> m_TimeoutVal = new StoryValue<int>();
        private IStoryValue<string> m_TimeoutSetVar = new StoryValue<string>();
        private IStoryValue<object> m_TimeoutSetVal = new StoryValue();
        private bool m_HaveSet = false;
        private int m_CurTime = 0;
    }
    /// <summary>
    /// firemessage(msgid,arg1,arg2,...);
    /// </summary>
    internal class FireMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            FireMessageCommand cmd = new FireMessageCommand();
            cmd.m_MsgId = m_MsgId.Clone();
            for (int i = 0; i < m_MsgArgs.Count; ++i) {
                IStoryValue<object> val = m_MsgArgs[i];
                cmd.m_MsgArgs.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_MsgId.Substitute(iterator, args);
            for (int i = 0; i < m_MsgArgs.Count; ++i) {
                IStoryValue<object> val = m_MsgArgs[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_MsgId.Evaluate(instance);
            for (int i = 0; i < m_MsgArgs.Count; ++i) {
                IStoryValue<object> val = m_MsgArgs[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string msgId = m_MsgId.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_MsgArgs.Count; ++i) {
                    IStoryValue<object> val = m_MsgArgs[i];
                    arglist.Add(val.Value);
                }
                object[] args = arglist.ToArray();
                userThread.StorySystem.SendMessage(msgId, args);
            }
            return false;
        }

        protected override void Load(Dsl.CallData callData)
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
        }

        private IStoryValue<string> m_MsgId = new StoryValue<string>();
        private List<IStoryValue<object>> m_MsgArgs = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// waitallmessage(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitAllMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
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

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Substitute(iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Substitute(iterator, args);
                m_SetVal.Substitute(iterator, args);
                m_TimeoutVal.Substitute(iterator, args);
                m_TimeoutSetVar.Substitute(iterator, args);
                m_TimeoutSetVal.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance);
                m_SetVal.Evaluate(instance);
                m_TimeoutVal.Evaluate(instance);
                m_TimeoutSetVar.Evaluate(instance);
                m_TimeoutSetVal.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
    /// waitallmessagehandler(msgid1,msgid2,...)[set(var,val)timeoutset(timeout,var,val)];
    /// </summary>
    internal class WaitAllMessageHandlerCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
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

        protected override void Substitute(object iterator, object[] args)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Substitute(iterator, args);
            }
            if (m_HaveSet) {
                m_SetVar.Substitute(iterator, args);
                m_SetVal.Substitute(iterator, args);
                m_TimeoutVal.Substitute(iterator, args);
                m_TimeoutSetVar.Substitute(iterator, args);
                m_TimeoutSetVal.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            for (int i = 0; i < m_MsgIds.Count; i++) {
                m_MsgIds[i].Evaluate(instance);
            }
            if (m_HaveSet) {
                m_SetVar.Evaluate(instance);
                m_SetVal.Evaluate(instance);
                m_TimeoutVal.Evaluate(instance);
                m_TimeoutSetVar.Evaluate(instance);
                m_TimeoutSetVal.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
    /// sendserverstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendServerStoryMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendServerStoryMessageCommand cmd = new SendServerStoryMessageCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string _msg = m_Msg.Value;

                Msg_LRL_StoryMessage msg = new Msg_LRL_StoryMessage();
                msg.MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.NULL;
                        arg.str_val = "";
                        msg.Args.Add(arg);
                    } else if (v is int) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.Args.Add(arg);
                    } else if (v is float) {
                        Msg_LRL_StoryMessage.MessageArg arg = new Msg_LRL_StoryMessage.MessageArg();
                        arg.val_type = Msg_LRL_StoryMessage.ArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
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

        protected override void Load(Dsl.CallData callData)
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
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// sendclientstorymessage(msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendClientStoryMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendClientStoryMessageCommand cmd = new SendClientStoryMessageCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                string _msg = m_Msg.Value;

                Msg_CLC_StoryMessage msg = new Msg_CLC_StoryMessage();
                msg.m_MsgId = _msg;

                for (int i = 0; i < m_Args.Count; ++i) {
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.m_Args.Add(arg);
                    } else if (v is int) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.m_Args.Add(arg);
                    } else if (v is float) {
                        Msg_CLC_StoryMessage.MessageArg arg = new Msg_CLC_StoryMessage.MessageArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
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

        protected override void Load(Dsl.CallData callData)
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
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
        {
            if (callData.GetId() == "touser" && callData.GetParamNum() == 1) {
                m_UserGuid.InitFromDsl(callData.GetParam(0));
                m_HaveUserGuid = true;
            }
        }

        private bool m_HaveUserGuid = false;
        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_Msg = new StoryValue<string>();
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// publishgfxevent(ev_name,group,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class PublishGfxEventCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            PublishGfxEventCommand cmd = new PublishGfxEventCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_EventName = m_EventName.Clone();
            cmd.m_Group = m_Group.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Substitute(iterator, args);
            m_EventName.Substitute(iterator, args);
            m_Group.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance);
            m_EventName.Evaluate(instance);
            m_Group.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v is int) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.args.Add(arg);
                    } else if (v is float) {
                        Msg_LC_PublishEvent.EventArg arg = new Msg_LC_PublishEvent.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
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

        protected override void Load(Dsl.CallData callData)
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
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
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
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// sendgfxmessage(objname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendGfxMessageCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendGfxMessageCommand cmd = new SendGfxMessageCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_ObjName = m_ObjName.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Substitute(iterator, args);
            m_ObjName.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance);
            m_ObjName.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v is int) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.args.Add(arg);
                    } else if (v is float) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
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

        protected override void Load(Dsl.CallData callData)
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
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
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
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// sendgfxmessagewithtag(tagname,msg,arg1,arg2,...)[touser(userid)];
    /// </summary>
    internal class SendGfxMessageWithTagCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            SendGfxMessageWithTagCommand cmd = new SendGfxMessageWithTagCommand();
            cmd.m_HaveUserGuid = m_HaveUserGuid;
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_ObjTag = m_ObjTag.Clone();
            cmd.m_Msg = m_Msg.Clone();
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                cmd.m_Args.Add(val.Clone());
            }
            return cmd;
        }

        protected override void Substitute(object iterator, object[] args)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Substitute(iterator, args);
            m_ObjTag.Substitute(iterator, args);
            m_Msg.Substitute(iterator, args);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            if (m_HaveUserGuid)
                m_UserGuid.Evaluate(instance);
            m_ObjTag.Evaluate(instance);
            m_Msg.Evaluate(instance);
            for (int i = 0; i < m_Args.Count; ++i) {
                IStoryValue<object> val = m_Args[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
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
                    IStoryValue<object> val = m_Args[i];
                    object v = val.Value;
                    if (null == v) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.NULL;
                        arg.str_val = "";
                        msg.args.Add(arg);
                    } else if (v is int) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.INT;
                        arg.str_val = ((int)v).ToString();
                        msg.args.Add(arg);
                    } else if (v is float) {
                        Msg_LC_SendGfxMessage.EventArg arg = new Msg_LC_SendGfxMessage.EventArg();
                        arg.val_type = LobbyArgType.FLOAT;
                        arg.str_val = ((float)v).ToString();
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

        protected override void Load(Dsl.CallData callData)
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
        }

        protected override void Load(Dsl.StatementData statementData)
        {
            if (statementData.Functions.Count == 2) {
                Dsl.FunctionData first = statementData.First;
                Dsl.FunctionData second = statementData.Second;
                if (null != first && null != first.Call && null != second && null != second.Call) {
                    Load(first.Call);
                    LoadUserId(second.Call);
                }
            }
        }

        private void LoadUserId(Dsl.CallData callData)
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
        private List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
    }
    /// <summary>
    /// highlightprompt(userguid,dictid,arg1,arg2,...);
    /// </summary>
    internal class HighlightPromptCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            HighlightPromptCommand cmd = new HighlightPromptCommand();
            cmd.m_UserGuid = m_UserGuid.Clone();
            cmd.m_DictId = m_DictId.Clone();
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryValue<object> val = m_DictArgs[i];
                cmd.m_DictArgs.Add(val.Clone());
            }
            return cmd;
        }

        protected override void ResetState()
        { }

        protected override void Substitute(object iterator, object[] args)
        {
            m_UserGuid.Substitute(iterator, args);
            m_DictId.Substitute(iterator, args);
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryValue<object> val = m_DictArgs[i];
                val.Substitute(iterator, args);
            }
        }

        protected override void Evaluate(StoryInstance instance)
        {
            m_UserGuid.Evaluate(instance);
            m_DictId.Evaluate(instance);
            for (int i = 0; i < m_DictArgs.Count; ++i) {
                IStoryValue<object> val = m_DictArgs[i];
                val.Evaluate(instance);
            }
        }

        protected override bool ExecCommand(StoryInstance instance, long delta)
        {
            UserThread userThread = instance.Context as UserThread;
            if (null != userThread) {
                ulong userGuid = m_UserGuid.Value;
                string dictId = m_DictId.Value;
                ArrayList arglist = new ArrayList();
                for (int i = 0; i < m_DictArgs.Count; ++i) {
                    IStoryValue<object> val = m_DictArgs[i];
                    arglist.Add(val.Value);
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

        protected override void Load(Dsl.CallData callData)
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
        }

        private IStoryValue<ulong> m_UserGuid = new StoryValue<ulong>();
        private IStoryValue<string> m_DictId = new StoryValue<string>();
        private List<IStoryValue<object>> m_DictArgs = new List<IStoryValue<object>>();
    }
}
