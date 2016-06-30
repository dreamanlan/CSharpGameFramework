using System;
using System.Collections.Generic;
using GameFramework;
namespace StorySystem
{
    /// <summary>
    /// story(1)
    /// {
    ///   onmessage(start)
    ///   {
    ///     dialog(1);
    ///   };
    ///   onmessage(enterarea, 1)
    ///   {
    ///     dialog(2);
    ///   };
    ///   onmessage(enddialog, 2)
    ///   {
    ///     createnpc(10,11,12);
    ///     movenpc(10,vector2(10,20));
    ///     aienable(10,11,12);
    ///   };
    ///   onmessage(killnpc,12)
    ///   {
    ///     missioncomplete();
    ///   };
    /// };
    /// </summary>
    public sealed class StoryMessageHandler
    {
        public string MessageId
        {
            get { return m_MessageId; }
            set { m_MessageId = value; }
        }
        public bool IsTriggered
        {
            get { return m_IsTriggered; }
            set { m_IsTriggered = value; }
        }
        public bool IsPaused
        {
            get { return m_IsPaused; }
            set { m_IsPaused = value; }
        }
        public bool IsInTick
        {
            get { return m_IsInTick; }
        }
        public Dictionary<string, object> StackVariables
        {
            get { return m_StackVariables; }
        }
        public StoryMessageHandler Clone()
        {
            StoryMessageHandler handler = new StoryMessageHandler();
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                handler.m_LoadedCommands.Add(m_LoadedCommands[i].Clone());
            }
            handler.m_MessageId = m_MessageId;
            handler.m_ArgumentNames = m_ArgumentNames;
            return handler;
        }
        public void Load(Dsl.FunctionData messageHandlerData)
        {
            Dsl.CallData callData = messageHandlerData.Call;
            if (null != callData && callData.HaveParam()) {
                int paramNum = callData.GetParamNum();
                string[] args = new string[paramNum];
                for (int i = 0; i < paramNum; ++i) {
                    args[i] = callData.GetParamId(i);
                }
                m_MessageId = string.Join(":", args);
            }
            RefreshCommands(messageHandlerData);
        }
        public void Load(Dsl.StatementData messageHandlerData)
        {
            Dsl.CallData first = messageHandlerData.First.Call;
            Dsl.FunctionData func = messageHandlerData.Second;
            Dsl.CallData second = func.Call;
            if (null != first && first.HaveParam()) {
                int paramNum = first.GetParamNum();
                string[] args = new string[paramNum];
                for (int i = 0; i < paramNum; ++i) {
                    args[i] = first.GetParamId(i);
                }
                m_MessageId = string.Join(":", args);
            }
            if (null != second && second.GetId() == "args" && second.HaveParam()) {
                int paramNum = second.GetParamNum();
                if (paramNum > 0) {
                    m_ArgumentNames = new string[paramNum];
                    for (int i = 0; i < paramNum; ++i) {
                        m_ArgumentNames[i] = second.GetParamId(i);
                    }
                }
            }
            RefreshCommands(func);
        }
        public void Reset()
        {
            m_IsTriggered = false;
            m_IsPaused = false;
            foreach (IStoryCommand cmd in m_CommandQueue) {
                cmd.Reset();
            }
            m_CommandQueue.Clear();
            m_StackVariables.Clear();
        }
        public void Prepare()
        {
            Reset();
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                IStoryCommand cmd = m_LoadedCommands[i];
                if (null != cmd.LeadCommand)
                    m_CommandQueue.Enqueue(cmd.LeadCommand);
                m_CommandQueue.Enqueue(cmd);
            }
        }
        public void Tick(StoryInstance instance, long delta)
        {
            if (m_IsPaused) {
                return;
            }
            try {
                instance.StackVariables = StackVariables;
                m_IsInTick = true;
                while (m_CommandQueue.Count > 0) {
                    IStoryCommand cmd = m_CommandQueue.Peek();
                    if (cmd.Execute(instance, delta, null, m_Arguments)) {
                        break;
                    } else {
                        cmd.Reset();
                        m_CommandQueue.Dequeue();
                    }
                }
                if (m_CommandQueue.Count == 0) {
                    m_IsTriggered = false;
                }
            } finally {
                m_IsInTick = false;
            }
        }
        public void Trigger(StoryInstance instance, object[] args)
        {
            Prepare();
            instance.StackVariables = StackVariables;
            m_IsTriggered = true;
            m_Arguments = args;
            if (null != m_ArgumentNames) {
                for(int i=0;i<m_ArgumentNames.Length && i<args.Length;++i) {
                    instance.SetVariable(m_ArgumentNames[i], args[i]);
                };
            }
        }
        private void RefreshCommands(Dsl.FunctionData handlerData)
        {
            m_LoadedCommands.Clear();
            for (int i = 0; i < handlerData.Statements.Count; i++) {
                IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(handlerData.Statements[i]);
                if (null != cmd) {
                    m_LoadedCommands.Add(cmd);
                }
            }
        }
        private string m_MessageId = "";
        private bool m_IsTriggered = false;
        private bool m_IsPaused = false;
        private bool m_IsInTick = false;
        private Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();
        private string[] m_ArgumentNames = null;
        private object[] m_Arguments = null;
        private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
        private Dictionary<string, object> m_StackVariables = new Dictionary<string, object>();
    }
    public sealed class StoryInstance
    {
        public string StoryId
        {
            get { return m_StoryId; }
            set { m_StoryId = value; }
        }
        public string Namespace
        {
            get { return m_Namespace; }
            set { m_Namespace = value; }
        }
        public bool IsTerminated
        {
            get { return m_IsTerminated; }
            set { m_IsTerminated = value; }
        }
        public bool IsPaused
        {
            get { return m_IsPaused; }
            set { m_IsPaused = value; }
        }
        public bool IsInTick
        {
            get { return m_IsInTick; }
        }
        public object Context
        {
            get { return m_Context; }
            set { m_Context = value; }
        }
        public Dictionary<string, object> LocalVariables
        {
            get { return m_LocalVariables; }
        }
        public Dictionary<string, object> GlobalVariables
        {
            get { return m_GlobalVariables; }
            set { m_GlobalVariables = value; }
        }
        public Dictionary<string, object> StackVariables
        {
            get { return m_StackVariables; }
            set { m_StackVariables = value; }
        }
        public TypedDataCollection CustomDatas
        {
            get { return m_CustomDatas; }
        }
        public void SetVariable(string varName, object varValue)
        {
            if (varName.StartsWith("$")) {
                if (null != m_StackVariables) {
                    if (m_StackVariables.ContainsKey(varName)) {
                        m_StackVariables[varName] = varValue;
                    } else {
                        m_StackVariables.Add(varName, varValue);
                    }
                }
            } else if (varName.StartsWith("@") && !varName.StartsWith("@@")) {
                if (m_LocalVariables.ContainsKey(varName)) {
                    m_LocalVariables[varName] = varValue;
                } else {
                    m_LocalVariables.Add(varName, varValue);
                }
            } else {
                if (null != m_GlobalVariables) {
                    if (m_GlobalVariables.ContainsKey(varName)) {
                        m_GlobalVariables[varName] = varValue;
                    } else {
                        m_GlobalVariables.Add(varName, varValue);
                    }
                }
            }
        }
        public bool TryGetVariable(string varName, out object val)
        {
            bool ret = false;
            val = null;
            if (varName.StartsWith("$")) {
                if(null!=m_StackVariables){
                    ret = m_StackVariables.TryGetValue(varName, out val);
                }
            } else if (varName.StartsWith("@") && !varName.StartsWith("@@")) {
                ret = m_LocalVariables.TryGetValue(varName, out val);
            } else {
                if (null != m_GlobalVariables) {
                    ret = m_GlobalVariables.TryGetValue(varName, out val);
                }
            }
            return ret;
        }
        public StoryInstance Clone()
        {
            StoryInstance instance = new StoryInstance();
            foreach (KeyValuePair<string, object> pair in m_PreInitedLocalVariables) {
                instance.m_PreInitedLocalVariables.Add(pair.Key, pair.Value);
            }
            for (int i = 0; i < m_MessageHandlers.Count; i++) {
                instance.m_MessageHandlers.Add(m_MessageHandlers[i].Clone());
                string msgId = m_MessageHandlers[i].MessageId;
                if (!instance.m_MessageQueues.ContainsKey(msgId)) {
                    instance.m_MessageQueues.Add(msgId, new Queue<MessageInfo>());
                }
            }
            instance.m_StoryId = m_StoryId;
            instance.m_Namespace = m_Namespace;
            return instance;
        }
        public bool Init(Dsl.DslInfo config)
        {
            bool ret = false;
            Dsl.FunctionData story = config.First;
            if (null != story && (story.GetId() == "story" || story.GetId() == "script")) {
                ret = true;
                Dsl.CallData callData = story.Call;
                if (null != callData && callData.HaveParam()) {
                    m_StoryId = callData.GetParamId(0);
                }
                for (int i = 0; i < story.Statements.Count; i++) {
                    if (story.Statements[i].GetId() == "local") {
                        Dsl.FunctionData sectionData = story.Statements[i] as Dsl.FunctionData;
                        if (null != sectionData) {
                            for (int j = 0; j < sectionData.Statements.Count; j++) {
                                Dsl.CallData defData = sectionData.Statements[j] as Dsl.CallData;
                                if (null != defData && defData.HaveId() && defData.HaveParam()) {
                                    string id = defData.GetId();
                                    if (id.StartsWith("@") && !id.StartsWith("@@")) {
                                        StoryValue val = new StoryValue();
                                        val.InitFromDsl(defData.GetParam(0));
                                        if (!m_PreInitedLocalVariables.ContainsKey(id)) {
                                            m_PreInitedLocalVariables.Add(id, val.Value);
                                        } else {
                                            m_PreInitedLocalVariables[id] = val.Value;
                                        }
                                    }
                                }
                            }
                        } else {
#if DEBUG
                            string err = string.Format("Story {0} DSL, local must be a function ! line:{1} local:{2}", m_StoryId, story.Statements[i].GetLine(), story.Statements[i].ToScriptString());
                            throw new Exception(err);
#else
                            LogSystem.Error("Story {0} DSL, local must be a function !", m_StoryId);
#endif
                        }
                    } else if (story.Statements[i].GetId() == "onmessage" || story.Statements[i].GetId() == "onnamespacedmessage") {
                        StoryMessageHandler handler = null;
                        Dsl.StatementData msgData = story.Statements[i] as Dsl.StatementData;
                        if (null != msgData) {
                            handler = new StoryMessageHandler();
                            handler.Load(msgData);
                        } else {
                            Dsl.FunctionData sectionData = story.Statements[i] as Dsl.FunctionData;
                            if (null != sectionData) {
                                handler = new StoryMessageHandler();
                                handler.Load(sectionData);
                            }
                        }
                        if (null != handler) {
                            string msgId;
                            if (!string.IsNullOrEmpty(m_Namespace) && story.Statements[i].GetId() == "onnamespacedmessage") {
                                msgId = string.Format("{0}:{1}", m_Namespace, handler.MessageId);
                                handler.MessageId = msgId;
                            } else {
                                msgId = handler.MessageId;
                            }
                            if (!m_MessageQueues.ContainsKey(msgId)) {
                                m_MessageHandlers.Add(handler);
                                m_MessageQueues.Add(msgId, new Queue<MessageInfo>());
                            } else {
#if DEBUG
                                string err = string.Format("Story {0} DSL, onmessage or onnamespacedmessage {1} duplicate, discard it ! line:{2}", m_StoryId, msgId, story.Statements[i].GetLine());
                                throw new Exception(err);
#else
                                LogSystem.Error("Story {0} DSL, onmessage {1} duplicate, discard it !", m_StoryId, msgId);
#endif
                            }
                        } else {
#if DEBUG
                            string err = string.Format("Story {0} DSL, onmessage must be a function or statement ! line:{1} onmessage:{2}", m_StoryId, story.Statements[i].GetLine(), story.Statements[i].ToScriptString());
                            throw new Exception(err);
#else
                            LogSystem.Error("Story {0} DSL, onmessage must be a function !", m_StoryId);
#endif
                        }
                    } else {
#if DEBUG
                        string err = string.Format("StoryInstance::Init, Story {0} unknown part {1}, line:{2} section:{3}", m_StoryId, story.Statements[i].GetId(), story.Statements[i].GetLine(), story.Statements[i].ToScriptString());
                        throw new Exception(err);
#else
                        LogSystem.Error("StoryInstance::Init, Story {0} unknown part {1}", m_StoryId, story.Statements[i].GetId());
#endif
                    }
                }
            } else {
#if DEBUG
                string err = string.Format("StoryInstance::Init, isn't story DSL, line:{0} story:{1}", story.GetLine(), story.ToScriptString());
                throw new Exception(err);
#else
                LogSystem.Error("StoryInstance::Init, isn't story DSL");
#endif
            }
            LogSystem.Debug("StoryInstance.Init message handler num:{0} {1}", m_MessageHandlers.Count, ret);
            return ret;
        }
        public void Reset()
        {
            m_IsTerminated = false;
            m_IsPaused = false;
            int ct = m_MessageHandlers.Count;
            for (int i = ct - 1; i >= 0; --i) {
                StoryMessageHandler handler = m_MessageHandlers[i];
                handler.Reset();
                Queue<MessageInfo> queue;
                if (m_MessageQueues.TryGetValue(handler.MessageId, out queue)) {
                    queue.Clear();
                }
            }
            m_LocalVariables.Clear();
            m_CustomDatas.Clear();
            m_Message2TriggerTimes.Clear();
        }
        public void Start()
        {
            m_LastTickTime = 0;
            m_CurTime = 0;
            foreach (KeyValuePair<string, object> pair in m_PreInitedLocalVariables) {
                m_LocalVariables.Add(pair.Key, pair.Value);
            }
            SendMessage("start");
        }
        public void SendMessage(string msgId, params object[] args)
        {
            MessageInfo msgInfo = new MessageInfo();
            msgInfo.m_MsgId = msgId;
            msgInfo.m_Args = args;
            Queue<MessageInfo> queue;
            if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                LogSystem.Info("StoryInstance queue message {0}", msgId);
                queue.Enqueue(msgInfo);
            } else {
                //忽略没有处理的消息
                LogSystem.Info("StoryInstance ignore message {0}", msgId);
            }
        }
        public int CountMessage(string msgId)
        {
            int ct = 0;
            Queue<MessageInfo> queue;
            if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                ct = queue.Count;
                for (int i = 0; i < m_MessageHandlers.Count; ++i) {
                    StoryMessageHandler handler = m_MessageHandlers[i];
                    if (handler.IsTriggered && !handler.IsInTick && handler.MessageId == msgId) {
                        ++ct;
                        break;
                    }
                }
            }
            return ct;
        }
        public void ClearMessage(params string[] msgIds)
        {
            int len = msgIds.Length;
            if (len <= 0) {
                foreach (KeyValuePair<string, Queue<MessageInfo>> pair in m_MessageQueues) {
                    Queue<MessageInfo> queue = pair.Value;
                    if (null != queue)
                        queue.Clear();
                }
            } else {
                for (int i = 0; i < len; ++i) {
                    string msgId = msgIds[i];
                    Queue<MessageInfo> queue;
                    if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                        queue.Clear();
                    }
                }
            }
        }
        public void PauseMessageHandler(string msgId, bool pause)
        {
            for (int i = 0; i < m_MessageHandlers.Count; ++i) {
                StoryMessageHandler handler = m_MessageHandlers[i];
                if (handler.IsTriggered && !handler.IsInTick && handler.MessageId == msgId) {
                    handler.IsPaused = pause;
                    break;
                }
            }
        }
        public void Tick(long curTime)
        {
            if (m_IsPaused) {
                m_LastTickTime = curTime;
                return;
            }
            try {
                m_IsInTick = true;
                const int c_MaxMsgCountPerTick = 256;
                long delta = 0;
                if (m_LastTickTime == 0) {
                    m_LastTickTime = curTime;
                } else {
                    delta = curTime - m_LastTickTime;
                    m_LastTickTime = curTime;
                    m_CurTime += delta;
                }
                int ct = m_MessageHandlers.Count;
                for (int ix = ct - 1; ix >= 0; --ix) {
                    long dt = delta;
                    StoryMessageHandler handler = m_MessageHandlers[ix];
                    if (handler.IsTriggered) {
                        handler.Tick(this, dt);
                        dt = 0;
                    }
                    if (!handler.IsTriggered) {
                        string msgId = handler.MessageId;
                        Queue<MessageInfo> queue;
                        if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                            for (int msgCt = 0; msgCt < c_MaxMsgCountPerTick && queue.Count > 0 && !handler.IsTriggered; ++msgCt) {
                                MessageInfo info = queue.Dequeue();
                                UpdateMessageTriggerTime(info.m_MsgId, curTime);
                                handler.Trigger(this, info.m_Args);
                                handler.Tick(this, dt);
                                dt = 0;
                            }
                        }
                    }
                }
            } finally {
                m_IsInTick = false;
            }
        }
        public long GetMessageTriggerTime(string msgId)
        {
            long time;
            m_Message2TriggerTimes.TryGetValue(msgId, out time);
            return time;
        }
        private void UpdateMessageTriggerTime(string msgId, long time)
        {
            if (m_Message2TriggerTimes.ContainsKey(msgId)) {
                m_Message2TriggerTimes[msgId] = time;
            } else {
                m_Message2TriggerTimes.Add(msgId, time);
            }
        }
        private class MessageInfo
        {
            public string m_MsgId = null;
            public object[] m_Args = null;
        }
        private long m_CurTime = 0;
        private long m_LastTickTime = 0;
        private Dictionary<string, object> m_LocalVariables = new Dictionary<string, object>();
        private Dictionary<string, object> m_GlobalVariables = null;
        private Dictionary<string, object> m_StackVariables = null;
        private string m_StoryId = string.Empty;
        private string m_Namespace = string.Empty;
        private bool m_IsTerminated = false;
        private bool m_IsPaused = false;
        private bool m_IsInTick = false;
        private object m_Context = null;
        private Dictionary<string, Queue<MessageInfo>> m_MessageQueues = new Dictionary<string, Queue<MessageInfo>>();
        private List<StoryMessageHandler> m_MessageHandlers = new List<StoryMessageHandler>();
        private Dictionary<string, object> m_PreInitedLocalVariables = new Dictionary<string, object>();
        private Dictionary<string, long> m_Message2TriggerTimes = new Dictionary<string, long>();
        private TypedDataCollection m_CustomDatas = new TypedDataCollection();
    }
}
