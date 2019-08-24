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
    public sealed class StoryLocalInfo
    {
        public IntObjDict LocalInfos
        {
            get { return m_LocalInfos; }
            set { m_LocalInfos = value; }
        }
        public StrObjDict StackVariables
        {
            get { return m_StackVariables; }
            set { m_StackVariables = value; }
        }
        public IStoryValueList Args
        {
            get { return m_Args; }
            set { m_Args = value; }
        }
        public StrIStoryValueDict OptArgs
        {
            get { return m_OptArgs; }
            set { m_OptArgs = value; }
        }
        public bool HaveValue
        {
            get { return m_HaveValue; }
            set { m_HaveValue = value; }
        }
        public object Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }
        public void Reset()
        {
            m_LocalInfos.Clear();
            m_StackVariables.Clear();
            m_Args.Clear();
            m_OptArgs.Clear();
            m_HaveValue = false;
            m_Value = null;
        }
        public object GetLocalInfo(int index)
        {
            object val;
            m_LocalInfos.TryGetValue(index, out val);
            return val;
        }
        public void SetLocalInfo(int index, object val)
        {
            m_LocalInfos[index] = val;
        }

        private IntObjDict m_LocalInfos = new IntObjDict();
        private StrObjDict m_StackVariables = new StrObjDict();
        private IStoryValueList m_Args = new IStoryValueList();
        private StrIStoryValueDict m_OptArgs = new StrIStoryValueDict();
        private bool m_HaveValue;
        private object m_Value;

        public static StoryLocalInfo New()
        {
            return s_StoryLocalInfoStackPool.Alloc();
        }
        public static void Recycle(StoryLocalInfo localInfo)
        {
            localInfo.Reset();
            s_StoryLocalInfoStackPool.Recycle(localInfo);
        }
        private static SimpleObjectPool<StoryLocalInfo> s_StoryLocalInfoStackPool = new SimpleObjectPool<StoryLocalInfo>(8);
    }
    public sealed class StoryRuntime
    {
        public object Iterator
        {
            get { return m_Iterator; }
            set { m_Iterator = value; }
        }
        public object[] Arguments
        {
            get { return m_Arguments; }
            set { m_Arguments = value; }
        }
        public StoryCommandQueue CommandQueue
        {
            get { return m_CommandQueue; }
        }
        public bool CompositeReentry
        {
            get { return m_CompositeReentry; }
            set { m_CompositeReentry = value; }
        }
        public bool IsBreak
        {
            get { return m_IsBreak; }
            set { m_IsBreak = value; }
        }
        public bool IsContinue
        {
            get { return m_IsContinue; }
            set { m_IsContinue = value; }
        }
        public bool IsReturn
        {
            get { return m_IsReturn; }
            set { m_IsReturn = value; }
        }
        public void Reset()
        {
            m_Iterator = null;
            m_Arguments = null;
            ResetCommandQueue();
            m_CompositeReentry = false;
            m_IsBreak = false;
            m_IsContinue = false;
            m_IsReturn = false;
        }
        public bool TryBreakLoop()
        {
            bool needBreak = false;
            if(m_IsBreak || m_IsReturn) {
                needBreak = true;
            }
            if (m_IsBreak)
                m_IsBreak = false;
            if (m_IsContinue)
                m_IsContinue = false;
            return needBreak;
        }
        public void Tick(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            while (m_CommandQueue.Count > 0) {
                IStoryCommand cmd = m_CommandQueue.Peek();
                if (cmd.Execute(instance, handler, delta, m_Iterator, m_Arguments)) {
                    m_CompositeReentry = false;
                    if(m_IsBreak || m_IsContinue || m_IsReturn) {
                        ResetCommandQueue();
                    }
                    break;
                } else {
                    m_CompositeReentry = false;
                    cmd.Reset();
                    m_CommandQueue.Dequeue();
                    if (m_IsBreak || m_IsContinue || m_IsReturn) {
                        ResetCommandQueue();
                        break;
                    }
                }
            }
        }

        private void ResetCommandQueue()
        {
            foreach (IStoryCommand cmd in m_CommandQueue) {
                cmd.Reset();
            }
            m_CommandQueue.Clear();
        }

        private object m_Iterator;
        private object[] m_Arguments;
        private StoryCommandQueue m_CommandQueue = new StoryCommandQueue();
        private bool m_CompositeReentry;
        private bool m_IsBreak;
        private bool m_IsContinue;
        private bool m_IsReturn;

        public static StoryRuntime New()
        {
            return s_StoryRuntimePool.Alloc();
        }
        public static void Recycle(StoryRuntime runtime)
        {
            runtime.Reset();
            s_StoryRuntimePool.Recycle(runtime);
        }
        private static SimpleObjectPool<StoryRuntime> s_StoryRuntimePool = new SimpleObjectPool<StoryRuntime>(8);
    }
    public sealed class IStoryValueList : List<IStoryValue>
    {
        public IStoryValueList() { }
        public IStoryValueList(int capacity) : base(capacity) { }
        public IStoryValueList(IEnumerable<IStoryValue> coll) : base(coll) { }
    }
    public sealed class StrIStoryValueDict : Dictionary<string, IStoryValue>
    {
        public StrIStoryValueDict() { }
        public StrIStoryValueDict(int capacity) : base(capacity) { }
        public StrIStoryValueDict(IDictionary<string, IStoryValue> dict) : base(dict) { }
    }
    public sealed class StoryLocalInfoStack : Stack<StoryLocalInfo>
    {
        public StoryLocalInfoStack() { }
        public StoryLocalInfoStack(int capacity) : base(capacity) { }
        public StoryLocalInfoStack(IEnumerable<StoryLocalInfo> coll) : base(coll) { }
    }
    public sealed class StoryCommandQueue : Queue<IStoryCommand>
    {
        public StoryCommandQueue() { }
        public StoryCommandQueue(int capacity) : base(capacity) { }
        public StoryCommandQueue(IEnumerable<IStoryCommand> coll) : base(coll) { }
    }
    public sealed class StoryRuntimeStack : Stack<StoryRuntime>
    {
        public StoryRuntimeStack() { }
        public StoryRuntimeStack(int capacity) : base(capacity) { }
        public StoryRuntimeStack(IEnumerable<StoryRuntime> coll) : base(coll) { }
    }
    public sealed class StoryMessageHandler
    {
        public string StoryId
        {
            get { return m_StoryId; }
        }
        public string MessageId
        {
            get { return m_MessageId; }
            set { m_MessageId = value; }
        }
        public Dsl.FunctionData Comments
        {
            get {
                return m_Comments;
            }
        }
        public bool IsTriggered
        {
            get { return m_IsTriggered; }
            set { m_IsTriggered = value; }
        }
        public bool IsSuspended
        {
            get { return m_IsSuspended; }
            set { m_IsSuspended = value; }
        }
        public bool IsInTick
        {
            get { return m_IsInTick; }
        }
        public StrObjDict StackVariables
        {
            get { return m_LocalInfo.StackVariables; }
        }
        public StoryLocalInfoStack LocalInfoStack
        {
            get { return m_LocalInfoStack; }
        }
        public StoryRuntimeStack RuntimeStack
        {
            get { return m_RuntimeStack; }
        }
        public StoryLocalInfo PeekLocalInfo()
        {
            return m_LocalInfoStack.Peek();
        }
        public void PushLocalInfo(StoryLocalInfo info)
        {
            m_LocalInfoStack.Push(info);
        }
        public void PopLocalInfo()
        {
            var r = m_LocalInfoStack.Pop();
            if (m_LocalInfoStack.Count > 0) {
                StoryLocalInfo.Recycle(r);
            } else {
                r.Reset();
            }
        }
        public StoryRuntime PeekRuntime()
        {
            return m_RuntimeStack.Peek();
        }
        public void PushRuntime(StoryRuntime runtime)
        {
            m_RuntimeStack.Push(runtime);
        }
        public void PopRuntime()
        {
            var runtime = m_RuntimeStack.Pop();
            if (m_RuntimeStack.Count > 0) {
                var newRuntime = m_RuntimeStack.Peek();
                newRuntime.IsBreak = runtime.IsBreak;
                newRuntime.IsContinue = runtime.IsContinue;
                newRuntime.IsReturn = runtime.IsReturn;
                StoryRuntime.Recycle(runtime);
            } else {
                runtime.Reset();
            }
        }
        public StoryMessageHandler Clone()
        {
            StoryMessageHandler handler = new StoryMessageHandler();
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                handler.m_LoadedCommands.Add(m_LoadedCommands[i].Clone());
            }
            handler.m_MessageId = m_MessageId;
            handler.m_ArgumentNames = m_ArgumentNames;
            handler.m_Comments = m_Comments;
            return handler;
        }
        public void Load(Dsl.FunctionData messageHandlerData, string storyId)
        {
            m_StoryId = storyId;
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
        public void Load(Dsl.StatementData messageHandlerData, string storyId)
        {
            m_StoryId = storyId;
            Dsl.CallData msgCallData = messageHandlerData.First.Call;
            if (null != msgCallData && msgCallData.HaveParam()) {
                int paramNum = msgCallData.GetParamNum();
                string[] args = new string[paramNum];
                for (int i = 0; i < paramNum; ++i) {
                    args[i] = msgCallData.GetParamId(i);
                }
                m_MessageId = string.Join(":", args);
            }
            for(int ix = 1; ix < messageHandlerData.GetFunctionNum(); ++ix) {
                var funcData = messageHandlerData.Functions[ix];
                var id = funcData.GetId();
                if (id == "args") {
                    var callData = funcData.Call;
                    if (null != callData && callData.HaveParam()) {
                        int paramNum = callData.GetParamNum();
                        if (paramNum > 0) {
                            m_ArgumentNames = new string[paramNum];
                            for (int i = 0; i < paramNum; ++i) {
                                m_ArgumentNames[i] = callData.GetParamId(i);
                            }
                        }
                    }
                } else if (id == "comment" || id == "comments") {
                    m_Comments = funcData;
                } else if (id == "body") {
                } else {
                    LogSystem.Error("Story {0} MessageHandler {1}, part '{2}' error !", storyId, m_MessageId, id);
                }
            }
            Dsl.FunctionData bodyFunc = null;
            for (int ix = 0; ix < messageHandlerData.GetFunctionNum(); ++ix) {
                var funcData = messageHandlerData.Functions[ix];
                var id = funcData.GetId();
                if (funcData.HaveStatement() && id != "comment" && id != "comments") {
                    bodyFunc = funcData;
                }
            }
            if (null != bodyFunc) {
                RefreshCommands(bodyFunc);
            } else {
                LogSystem.Error("Story {0} MessageHandler {1}, no body !", storyId, m_MessageId);
            }
        }
        public void Reset()
        {
            Reset(true);
        }
        public void Reset(bool logIfTriggered)
        {
            if (logIfTriggered && m_IsTriggered) {
                LogSystem.Error("Reset a running message handler !");
                Helper.LogCallStack(true);
            }
            m_IsTriggered = false;
            m_IsSuspended = false;
            while (LocalInfoStack.Count > 0) {
                PopLocalInfo();
            }
            while (RuntimeStack.Count > 0) {
                PopRuntime();
            }
        }
        public void Prepare()
        {
            Reset();
            PushLocalInfo(m_LocalInfo);
            PushRuntime(m_Runtime);
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                IStoryCommand cmd = m_LoadedCommands[i];
                if (null != cmd.PrologueCommand)
                    m_Runtime.CommandQueue.Enqueue(cmd.PrologueCommand);
                m_Runtime.CommandQueue.Enqueue(cmd);
                if (null != cmd.EpilogueCommand)
                    m_Runtime.CommandQueue.Enqueue(cmd.EpilogueCommand);
            }
        }
        public void Tick(StoryInstance instance, long delta)
        {
            if (m_IsSuspended) {
                return;
            }
            try {
                m_IsInTick = true;
                instance.StackVariables = PeekLocalInfo().StackVariables;
                var runtime = RuntimeStack.Peek();
                runtime.Tick(instance, this, delta);
                bool isReturn = runtime.IsReturn;
                if (runtime.CommandQueue.Count == 0) {
                    PopRuntime();
                    if (RuntimeStack.Count > 0) {
                        RuntimeStack.Peek().CompositeReentry = true;
                    }
                }
                if (RuntimeStack.Count <= 0 || isReturn) {
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
                for (int i = 0; i < m_ArgumentNames.Length; ++i) {
                    if (i < args.Length)
                        instance.SetVariable(m_ArgumentNames[i], args[i]);
                    else
                        instance.SetVariable(m_ArgumentNames[i], null);
                }
            }
            m_Runtime.Iterator = null;
            m_Runtime.Arguments = m_Arguments;
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
        private string m_StoryId = string.Empty;
        private string m_MessageId = string.Empty;
        private bool m_IsTriggered = false;
        private bool m_IsSuspended = false;
        private bool m_IsInTick = false;
        private StoryLocalInfo m_LocalInfo = new StoryLocalInfo();
        private StoryRuntime m_Runtime = new StoryRuntime();
        private string[] m_ArgumentNames = null;
        private object[] m_Arguments = null;
        private StoryLocalInfoStack m_LocalInfoStack = new StoryLocalInfoStack();
        private StoryRuntimeStack m_RuntimeStack = new StoryRuntimeStack();
        private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
        private Dsl.FunctionData m_Comments = null;
    }
    public sealed class StoryMessageHandlerList : List<StoryMessageHandler>
    {
        public StoryMessageHandlerList() { }
        public StoryMessageHandlerList(int capacity) : base(capacity) { }
        public StoryMessageHandlerList(IEnumerable<StoryMessageHandler> coll) : base(coll) { }
    }
    public sealed class StoryMessageHandlerEnumerator
    {
        public StoryMessageHandler Current
        {
            get { return m_Enumerator.Current; }
        }
        public bool MoveNext()
        {
            return m_Enumerator.MoveNext();
        }

        internal StoryMessageHandlerEnumerator(IEnumerator<StoryMessageHandler> enumer)
        {
            m_Enumerator = enumer;
        }
        private IEnumerator<StoryMessageHandler> m_Enumerator;
    }
    public delegate bool StoryCommandDebuggerDelegation(StoryInstance instance, StoryMessageHandler handler, IStoryCommand command, long delta, object iterator, object[] args);
    public sealed class StoryInstance
    {
        public StoryCommandDebuggerDelegation OnExecDebugger;
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
        public bool IsDebug
        {
            get { return m_IsDebug; }
            set { m_IsDebug = value; }
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
        public StrObjDict LocalVariables
        {
            get { return m_LocalVariables; }
        }
        public StrObjDict GlobalVariables
        {
            get { return m_GlobalVariables; }
            set { m_GlobalVariables = value; }
        }
        public StrObjDict StackVariables
        {
            get { return m_StackVariables; }
            set { m_StackVariables = value; }
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
                if (null != m_StackVariables) {
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
        public bool RemoveVariable(string varName)
        {
            bool ret = false;
            if (varName.StartsWith("$")) {
                if (null != m_StackVariables) {
                    ret = m_StackVariables.Remove(varName);
                }
            } else if (varName.StartsWith("@") && !varName.StartsWith("@@")) {
                ret = m_LocalVariables.Remove(varName);
            } else {
                if (null != m_GlobalVariables) {
                    ret = m_GlobalVariables.Remove(varName);
                }
            }
            return ret;
        }
        public StoryInstance Clone()
        {
            StoryInstance instance = new StoryInstance();
            foreach (var pair in m_PreInitedLocalVariables) {
                instance.m_PreInitedLocalVariables.Add(pair.Key, pair.Value);
            }
            foreach (var pair in m_LoadedMessageHandlers) {
                instance.m_LoadedMessageHandlers.Add(pair.Key, pair.Value);
            }
            for (int i = 0; i < m_MessageHandlers.Count; i++) {
                instance.m_MessageHandlers.Add(m_MessageHandlers[i].Clone());
                string msgId = m_MessageHandlers[i].MessageId;
                if (!instance.m_MessageQueues.ContainsKey(msgId)) {
                    instance.m_MessageQueues.Add(msgId, new Queue<MessageInfo>());
                }
                if (!instance.m_ConcurrentMessageQueues.ContainsKey(msgId)) {
                    instance.m_ConcurrentMessageQueues.Add(msgId, new Queue<MessageInfo>());
                }
                if (!instance.m_ConcurrentMessageHandlerPool.ContainsKey(msgId)) {
                    instance.m_ConcurrentMessageHandlerPool.Add(msgId, new Queue<StoryMessageHandler>());
                }
            }
            instance.m_StoryId = m_StoryId;
            instance.m_Namespace = m_Namespace;
            return instance;
        }
        public bool Init(Dsl.DslInfo config)
        {
            if (null == config || null == config.First)
                return false;
            bool ret = false;
            Dsl.FunctionData story = config.First;
            if (story.GetId() == "story" || story.GetId() == "script") {
                ret = true;
                Dsl.CallData callData = story.Call;
                if (null != callData && callData.HaveParam()) {
                    int paramNum = callData.GetParamNum();
                    string[] args = new string[paramNum];
                    for (int i = 0; i < paramNum; ++i) {
                        args[i] = callData.GetParamId(i);
                    }
                    m_StoryId = string.Join(":", args);
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
                            string err = string.Format("Story {0} DSL, local must be a function ! line:{1} local:{2}", m_StoryId, story.Statements[i].GetLine(), story.Statements[i].ToScriptString(false));
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
                            handler.Load(msgData, m_StoryId);
                        } else {
                            Dsl.FunctionData sectionData = story.Statements[i] as Dsl.FunctionData;
                            if (null != sectionData) {
                                handler = new StoryMessageHandler();
                                handler.Load(sectionData, m_StoryId);
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
                            if (!m_LoadedMessageHandlers.ContainsKey(msgId)) {
                                m_LoadedMessageHandlers.Add(msgId, handler);
                                m_MessageHandlers.Add(handler.Clone());
                                m_MessageQueues.Add(msgId, new Queue<MessageInfo>());
                                m_ConcurrentMessageQueues.Add(msgId, new Queue<MessageInfo>());
                                m_ConcurrentMessageHandlerPool.Add(msgId, new Queue<StoryMessageHandler>());
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
                            string err = string.Format("Story {0} DSL, onmessage must be a function or statement ! line:{1} onmessage:{2}", m_StoryId, story.Statements[i].GetLine(), story.Statements[i].ToScriptString(false));
                            throw new Exception(err);
#else
                            LogSystem.Error("Story {0} DSL, onmessage must be a function !", m_StoryId);
#endif
                        }
                    } else {
#if DEBUG
                        string err = string.Format("StoryInstance::Init, Story {0} unknown part {1}, line:{2} section:{3}", m_StoryId, story.Statements[i].GetId(), story.Statements[i].GetLine(), story.Statements[i].ToScriptString(false));
                        throw new Exception(err);
#else
                        LogSystem.Error("StoryInstance::Init, Story {0} unknown part {1}", m_StoryId, story.Statements[i].GetId());
#endif
                    }
                }
            } else {
#if DEBUG
                string err = string.Format("StoryInstance::Init, isn't story DSL, line:{0} story:{1}", story.GetLine(), story.ToScriptString(false));
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
            Reset(true);
        }
        public void Reset(bool logIfTriggered)
        {
            m_IsTerminated = false;
            m_IsPaused = false;
            int ct = m_MessageHandlers.Count;
            for (int i = ct - 1; i >= 0; --i) {
                StoryMessageHandler handler = m_MessageHandlers[i];
                handler.Reset(logIfTriggered);
            }
            ct = m_ConcurrentMessageHandlers.Count;
            for (int i = ct - 1; i >= 0; --i) {
                StoryMessageHandler handler = m_ConcurrentMessageHandlers[i];
                RecycleConcurrentMessageHandler(handler, logIfTriggered);
            }
            foreach (KeyValuePair<string, Queue<MessageInfo>> pair in m_MessageQueues) {
                Queue<MessageInfo> queue = pair.Value;
                if (null != queue)
                    queue.Clear();
            }
            foreach (KeyValuePair<string, Queue<MessageInfo>> pair in m_ConcurrentMessageQueues) {
                Queue<MessageInfo> queue = pair.Value;
                if (null != queue)
                    queue.Clear();
            }
            m_ConcurrentMessageHandlers.Clear();
            m_LocalVariables.Clear();
            m_Message2TriggerTimes.Clear();
            m_MessageCount = 0;
            m_ConcurrentMessageCount = 0;
            m_TriggeredHandlerCount = 0;
        }
        public void Start()
        {
            m_LastTickTime = 0;
            m_CurTime = 0;
            try {
                foreach (KeyValuePair<string, object> pair in m_PreInitedLocalVariables) {
                    m_LocalVariables.Add(pair.Key, pair.Value);
                }
            } catch (Exception ex) {
                LogSystem.Error("Story {0} local variable duplicate! (for AI, @objid is a system predefined variable; for UI, @window is a system predefined variable), Exception:{1}\n{2}", m_StoryId, ex.Message, ex.StackTrace);
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
                if (msgId != "start" && msgId != "PlayerStart" && !msgId.StartsWith("common_")) {
                    LogSystem.Warn("StoryInstance queue message {0}", msgId);
                }
                queue.Enqueue(msgInfo);
                ++m_MessageCount;
            } else {
                //忽略没有处理的消息
                //LogSystem.Info("StoryInstance ignore message {0}", msgId);
            }
        }
        public void SendConcurrentMessage(string msgId, params object[] args)
        {
            MessageInfo msgInfo = new MessageInfo();
            msgInfo.m_MsgId = msgId;
            msgInfo.m_Args = args;
            Queue<MessageInfo> queue;
            if (m_ConcurrentMessageQueues.TryGetValue(msgId, out queue)) {
                if (msgId != "start" && msgId != "PlayerStart" && !msgId.StartsWith("common_")) {
                    LogSystem.Warn("StoryInstance queue concurrent message {0}", msgId);
                }
                queue.Enqueue(msgInfo);
                ++m_ConcurrentMessageCount;
            } else {
                //忽略没有处理的消息
                //LogSystem.Info("StoryInstance ignore concurrent message {0}", msgId);
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
            if (m_ConcurrentMessageQueues.TryGetValue(msgId, out queue)) {
                ct += queue.Count;
                for (int i = 0; i < m_ConcurrentMessageHandlers.Count; ++i) {
                    StoryMessageHandler handler = m_ConcurrentMessageHandlers[i];
                    if (handler.IsTriggered && !handler.IsInTick && handler.MessageId == msgId) {
                        ++ct;
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
                foreach (KeyValuePair<string, Queue<MessageInfo>> pair in m_ConcurrentMessageQueues) {
                    Queue<MessageInfo> queue = pair.Value;
                    if (null != queue)
                        queue.Clear();
                }
                m_MessageCount = 0;
                m_ConcurrentMessageCount = 0;
            } else {
                for (int i = 0; i < len; ++i) {
                    string msgId = msgIds[i];
                    Queue<MessageInfo> queue;
                    if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                        m_MessageCount -= queue.Count;
                        queue.Clear();
                    }
                    if (m_ConcurrentMessageQueues.TryGetValue(msgId, out queue)) {
                        m_ConcurrentMessageCount -= queue.Count;
                        queue.Clear();
                    }
                }
            }
        }
        public void SuspendMessageHandler(string msgId, bool suspend)
        {
            for (int i = 0; i < m_MessageHandlers.Count; ++i) {
                StoryMessageHandler handler = m_MessageHandlers[i];
                if (handler.IsTriggered && !handler.IsInTick && handler.MessageId == msgId) {
                    handler.IsSuspended = suspend;
                    break;
                }
            }
            for (int i = 0; i < m_ConcurrentMessageHandlers.Count; ++i) {
                StoryMessageHandler handler = m_ConcurrentMessageHandlers[i];
                if (handler.IsTriggered && !handler.IsInTick && handler.MessageId == msgId) {
                    handler.IsSuspended = suspend;
                    break;
                }
            }
        }
        public bool CanSleep()
        {
            return m_IsPaused || m_MessageCount + m_ConcurrentMessageCount + m_TriggeredHandlerCount <= 0;
        }
        public void Tick(long curTime)
        {
            if (m_IsPaused || m_MessageCount + m_ConcurrentMessageCount + m_TriggeredHandlerCount <= 0) {
                m_LastTickTime = curTime;
                return;
            }
            try {
                m_IsInTick = true;
                const int c_MaxMsgCountPerTick = 64;
                const int c_MaxConcurrentMsgCountPerTick = 16;
                long delta = 0;
                if (m_LastTickTime == 0) {
                    m_LastTickTime = curTime;
                } else {
                    delta = curTime - m_LastTickTime;
                    m_LastTickTime = curTime;
                    m_CurTime += delta;
                }
                int curTriggerdCount = 0;
                int ct = m_MessageHandlers.Count;
                for (int ix = ct - 1; ix >= 0; --ix) {
                    long dt = delta;
                    StoryMessageHandler handler = m_MessageHandlers[ix];
                    if (handler.IsTriggered) {
                        handler.Tick(this, dt);
                        dt = 0;
                    }
                    if (!handler.IsTriggered && m_MessageCount > 0) {
                        string msgId = handler.MessageId;
                        Queue<MessageInfo> queue;
                        if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                            for (int msgCt = 0; msgCt < c_MaxMsgCountPerTick && queue.Count > 0 && !handler.IsTriggered; ++msgCt) {
                                MessageInfo info = queue.Dequeue();
                                --m_MessageCount;
                                UpdateMessageTriggerTime(info.m_MsgId, curTime);
                                handler.Trigger(this, info.m_Args);
                                handler.Tick(this, 0);
                            }
                        }
                    }
                    if (handler.IsTriggered) {
                        ++curTriggerdCount;
                    }
                }
                ct = m_ConcurrentMessageHandlers.Count;
                if (m_ConcurrentMessageCount > 0) {
                    foreach (var pair in m_ConcurrentMessageQueues) {
                        Queue<MessageInfo> queue = pair.Value;
                        for (int concurrentMsgCt = 0; concurrentMsgCt < c_MaxConcurrentMsgCountPerTick && queue.Count > 0; ++concurrentMsgCt) {
                            MessageInfo info = queue.Dequeue();
                            --m_ConcurrentMessageCount;
                            StoryMessageHandler handler = NewConcurrentMessageHandler(info.m_MsgId);
                            if (null != handler) {
                                UpdateMessageTriggerTime(info.m_MsgId, curTime);
                                handler.Trigger(this, info.m_Args);
                                handler.Tick(this, 0);
                                if (handler.IsTriggered) {
                                    m_ConcurrentMessageHandlers.Add(handler);
                                } else {
                                    RecycleConcurrentMessageHandler(handler, true);
                                }
                            }
                        }
                    }
                }
                for (int ix = ct - 1; ix >= 0; --ix) {
                    long dt = delta;
                    StoryMessageHandler handler = m_ConcurrentMessageHandlers[ix];
                    if (handler.IsTriggered) {
                        handler.Tick(this, dt);
                        dt = 0;
                    }
                    if (!handler.IsTriggered) {
                        m_ConcurrentMessageHandlers.RemoveAt(ix);
                        RecycleConcurrentMessageHandler(handler, true);
                    }
                }
                m_TriggeredHandlerCount = curTriggerdCount + m_ConcurrentMessageHandlers.Count;
            } finally {
                m_IsInTick = false;
            }
        }
        public StoryMessageHandlerEnumerator GetMessageHandlerEnumerator()
        {
            var enumer = m_MessageHandlers.GetEnumerator();
            return new StoryMessageHandlerEnumerator(enumer);
        }
        public StoryMessageHandlerEnumerator GetConcurrentMessageHandlerEnumerator()
        {
            var enumer = m_ConcurrentMessageHandlers.GetEnumerator();
            return new StoryMessageHandlerEnumerator(enumer);
        }
        public StoryMessageHandler GetMessageHandler(string msgId)
        {
            StoryMessageHandler ret = null;
            for(int i=0;i<m_MessageHandlers.Count;++i) {
                var handler = m_MessageHandlers[i];
                if (handler.MessageId == msgId) {
                    ret = handler;
                    break;
                }
            }
            return ret;
        }
        public StoryMessageHandlerList GetConcurrentMessageHandler(string msgId)
        {
            var ret = new StoryMessageHandlerList();
            int ct = GetConcurrentMessageHandler(msgId, ret);
            return ret;
        }
        public int GetConcurrentMessageHandler(string msgId, StoryMessageHandlerList list)
        {
            int ct = 0;
            for (int i = 0; i < m_ConcurrentMessageHandlers.Count; ++i) {
                var handler = m_ConcurrentMessageHandlers[i];
                if (handler.MessageId == msgId) {
                    list.Add(handler);
                    ++ct;
                    break;
                }
            }
            return ct;
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

        private StoryMessageHandler NewConcurrentMessageHandler(string msgId)
        {
            Queue<StoryMessageHandler> queue;
            if (m_ConcurrentMessageHandlerPool.TryGetValue(msgId, out queue)) {
                if (queue.Count > 0) {
                    StoryMessageHandler handler = queue.Dequeue();
                    return handler;
                }
            }
            return NewMessageHandler(msgId);
        }
        private void RecycleConcurrentMessageHandler(StoryMessageHandler handler, bool logIfTriggered)
        {
            string msgId = handler.MessageId;
            Queue<StoryMessageHandler> queue;
            if (m_ConcurrentMessageHandlerPool.TryGetValue(msgId, out queue)) {
                handler.Reset(logIfTriggered);
                queue.Enqueue(handler);
            }
        }
        private StoryMessageHandler NewMessageHandler(string msgId)
        {
            StoryMessageHandler handler;
            if (m_LoadedMessageHandlers.TryGetValue(msgId, out handler)) {
                handler = handler.Clone();
            }
            return handler;
        }

        private class MessageInfo
        {
            public string m_MsgId = null;
            public object[] m_Args = null;
        }
        private long m_CurTime = 0;
        private long m_LastTickTime = 0;
        private StrObjDict m_LocalVariables = new StrObjDict();
        private StrObjDict m_GlobalVariables = null;
        private StrObjDict m_StackVariables = null;
        private bool m_IsDebug = false;
        private string m_StoryId = string.Empty;
        private string m_Namespace = string.Empty;
        private bool m_IsTerminated = false;
        private bool m_IsPaused = false;
        private bool m_IsInTick = false;
        private object m_Context = null;
        private int m_MessageCount = 0;
        private int m_ConcurrentMessageCount = 0;
        private int m_TriggeredHandlerCount = 0;
        private Dictionary<string, Queue<MessageInfo>> m_MessageQueues = new Dictionary<string, Queue<MessageInfo>>();
        private List<StoryMessageHandler> m_MessageHandlers = new List<StoryMessageHandler>();
        private Dictionary<string, Queue<MessageInfo>> m_ConcurrentMessageQueues = new Dictionary<string, Queue<MessageInfo>>();
        private List<StoryMessageHandler> m_ConcurrentMessageHandlers = new List<StoryMessageHandler>();
        private Dictionary<string, long> m_Message2TriggerTimes = new Dictionary<string, long>();

        private Dictionary<string, Queue<StoryMessageHandler>> m_ConcurrentMessageHandlerPool = new Dictionary<string, Queue<StoryMessageHandler>>();
        private StrObjDict m_PreInitedLocalVariables = new StrObjDict();
        private Dictionary<string, StoryMessageHandler> m_LoadedMessageHandlers = new Dictionary<string, StoryMessageHandler>();
    }
}
