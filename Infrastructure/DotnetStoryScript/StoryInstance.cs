using System;
using System.Collections.Generic;
using System.Collections;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

namespace DotnetStoryScript
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
    /// };
    /// </summary>
    public sealed class StoryInstance : IDictionary
    {
        public IEnumerator<MessageHandlerInfo> GetMessageHandlerEnumerator()
        {
            var list = new List<MessageHandlerInfo>();
            foreach (var c in m_ActiveCoroutines) {
                list.Add(new MessageHandlerInfo(c));
            }
            return list.GetEnumerator();
        }

        public IEnumerator<MessageHandlerInfo> GetConcurrentMessageHandlerEnumerator()
        {
            var list = new List<MessageHandlerInfo>();
            foreach (var c in m_ConcurrentCoroutines) {
                list.Add(new MessageHandlerInfo(c));
            }
            return list.GetEnumerator();
        }

        public class MessageHandlerInfo
        {
            private CoroutineInfo m_Info;
            internal MessageHandlerInfo(CoroutineInfo info) { m_Info = info; }
            public string MessageId { get { return m_Info.MessageId; } }
            public bool IsTriggered { get { return m_Info.IsTriggered; } set { m_Info.IsTriggered = value; } }
            public bool CanSkip { get { return m_Info.CanSkip; } set { m_Info.CanSkip = value; } }
        }

        public delegate bool StoryCommandDebuggerDelegation(StoryInstance instance, string msgId, IExpression expression, long delta);

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

        public Dsl.ISyntaxComponent Config
        {
            get { return m_Config; }
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

        internal CoroutineInfo CurrentCoroutine
        {
            get { return m_CurrentCoroutine; }
            set { m_CurrentCoroutine = value; }
        }

        public StrBoxedValueDict ContextVariables
        {
            get { return m_ContextVariables; }
            set { m_ContextVariables = value; }
        }

        public StrBoxedValueDict InstanceVariables
        {
            get { return m_InstanceVariables; }
        }

        public void SetVariable(string varName, BoxedValue varValue)
        {
            if (m_Calculator != null) {
                m_Calculator.SetVariable(varName, varValue);
            }
        }

        public bool TryGetVariable(string varName, out BoxedValue val)
        {
            if (m_Calculator != null) {
                return m_Calculator.TryGetVariable(varName, out val);
            }
            val = BoxedValue.NullObject;
            return false;
        }

        public StoryInstance Clone()
        {
            StoryInstance instance = new StoryInstance();
            instance.Namespace = m_Namespace;
            instance.Init(m_Config);
            return instance;
        }

        public bool Init(Dsl.ISyntaxComponent config)
        {
            bool ret = false;
            m_Config = config;

            Dsl.FunctionData story = config as Dsl.FunctionData;
            if (null == story) {
#if DEBUG
                string err = string.Format("StoryInstance::Init, isn't story DSL, line:{0} story:{1}", config.GetLine(), config.ToScriptString(false, Dsl.DelimiterInfo.Default));
                throw new Exception(err);
#else
                    LogSystem.Error("StoryInstance::Init, isn't story DSL");
                    return false;
#endif
            }
            if (story.IsHighOrder && (story.GetId() == "story" || story.GetId() == "script")) {
                ret = true;
                Dsl.FunctionData callData = story.LowerOrderFunction;
                if (null != callData && callData.HaveParam()) {
                    int paramNum = callData.GetParamNum();
                    string[] args = new string[paramNum];
                    for (int i = 0; i < paramNum; ++i) {
                        args[i] = callData.GetParamId(i);
                    }
                    m_StoryId = string.Join(":", args);
                }

                m_Calculator = DslCalculatorHost.NewCalculator();
                EnsureCalculatorCallbacks(m_Calculator);
                var calculator = m_Calculator;

                for (int i = 0; i < story.GetParamNum(); i++) {
                    var part = story.GetParam(i);
                    string partId = part.GetId();
                    if (partId == "local") {
                        Dsl.FunctionData sectionData = part as Dsl.FunctionData;
                        if (null != sectionData) {
                            for (int j = 0; j < sectionData.GetParamNum(); j++) {
                                Dsl.FunctionData defData = sectionData.GetParam(j) as Dsl.FunctionData;
                                if (null != defData && defData.HaveId() && defData.HaveParam()) {
                                    string id = defData.GetId();
                                    if (id.StartsWith("@") && !id.StartsWith("@@")) {
                                        var exp = calculator.Load(defData.GetParam(0));
                                        BoxedValue val = BoxedValue.NullObject;
                                        if (null != exp) {
                                            val = exp.Calc();
                                        }
                                        if (!m_PreInitedLocalVariables.ContainsKey(id)) {
                                            m_PreInitedLocalVariables.Add(id, val);
                                        }
                                        else {
                                            m_PreInitedLocalVariables[id] = val;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (partId == "onmessage" || partId == "onnamespacedmessage") {
                        ProcessOnMessage(part, calculator);
                    }
                    else {
#if DEBUG
                        string err = string.Format("StoryInstance::Init, Story {0} unknown part {1}, line:{2} section:{3}", m_StoryId, part.GetId(), part.GetLine(), part.ToScriptString(false, Dsl.DelimiterInfo.Default));
                        throw new Exception(err);
#else
                        LogSystem.Error("StoryInstance::Init, Story {0} unknown part {1}", m_StoryId, part.GetId());
#endif
                    }
                }
            }
            else {
#if DEBUG
                string err = string.Format("StoryInstance::Init, isn't story DSL, line:{0} story:{1}", story.GetLine(), story.ToScriptString(false, Dsl.DelimiterInfo.Default));
                throw new Exception(err);
#else
                LogSystem.Error("StoryInstance::Init, isn't story DSL");
#endif
            }
            LogSystem.Debug("StoryInstance.Init message handler num:{0} {1}", m_HandlerFunctionNames.Count, ret);
            if (StoryConfigManager.Instance.IsDevice) {
                m_Config = null;
            }
            return ret;
        }

        private void ProcessOnMessage(Dsl.ISyntaxComponent part, DslCalculator calculator)
        {
            string msgId = "";
            string[] paramNames = null;
            Dsl.FunctionData bodyFunc = null;

            Dsl.StatementData msgData = part as Dsl.StatementData;
            if (null != msgData) {
                Dsl.FunctionData callData = msgData.First.AsFunction;
                if (callData.IsHighOrder) {
                    callData = callData.LowerOrderFunction;
                }
                if (null != callData && callData.HaveParam()) {
                    int paramNum = callData.GetParamNum();
                    string[] args = new string[paramNum];
                    for (int i = 0; i < paramNum; ++i) {
                        args[i] = callData.GetParamId(i);
                    }
                    msgId = string.Join(":", args);
                }

                for (int ix = 1; ix < msgData.GetFunctionNum(); ++ix) {
                    var funcData = msgData.Functions[ix].AsFunction;
                    var id = funcData.GetId();
                    if (id == "params" || id == "args") {
                        var innerCall = funcData;
                        if (funcData.IsHighOrder) {
                            innerCall = funcData.LowerOrderFunction;
                        }
                        if (null != innerCall && innerCall.HaveParam()) {
                            int paramNum = innerCall.GetParamNum();
                            if (paramNum > 0) {
                                paramNames = new string[paramNum];
                                for (int i = 0; i < paramNum; ++i) {
                                    paramNames[i] = innerCall.GetParamId(i);
                                }
                            }
                        }
                    }
                }

                for (int ix = 0; ix < msgData.GetFunctionNum(); ++ix) {
                    var funcData = msgData.Functions[ix].AsFunction;
                    if (funcData.HaveStatement() && funcData.GetId() != "comment" && funcData.GetId() != "comments") {
                        bodyFunc = funcData;
                        break;
                    }
                }
            }
            else {
                Dsl.FunctionData sectionData = part as Dsl.FunctionData;
                if (null != sectionData) {
                    Dsl.FunctionData callData = sectionData;
                    if (callData.IsHighOrder) {
                        callData = callData.LowerOrderFunction;
                    }
                    if (null != callData && callData.HaveParam()) {
                        int paramNum = callData.GetParamNum();
                        string[] args = new string[paramNum];
                        for (int i = 0; i < paramNum; ++i) {
                            args[i] = callData.GetParamId(i);
                        }
                        msgId = string.Join(":", args);
                    }
                    if (sectionData.HaveStatement()) {
                        bodyFunc = sectionData;
                    }
                }
            }

            if (!string.IsNullOrEmpty(msgId) && bodyFunc != null) {
                string msgIdNoNamespace = msgId;
                bool isNamespaceMsg = part.GetId() == "onnamespacedmessage";
                if (!string.IsNullOrEmpty(m_Namespace) && isNamespaceMsg) {
                    msgId = string.Format("{0}:{1}", m_Namespace, msgId);
                }

                string funcName = string.Format("__story_{0}_{1}", m_StoryId, msgIdNoNamespace).Replace(':', '_');

                if (!m_HandlerFunctionNames.ContainsKey(msgId)) {
                    // Use bodyFunc directly - it already has the correct structure with statements
                    // bodyFunc is a FunctionData with HaveStatement()==true, statements are in its Params
                    if (null != paramNames && paramNames.Length > 0) {
                        calculator.LoadDsl(funcName, paramNames, bodyFunc);
                    }
                    else {
                        calculator.LoadDsl(funcName, bodyFunc);
                    }

                    m_HandlerFunctionNames.Add(msgId, funcName);
                    m_MessageQueues.Add(msgId, new Queue<MessageInfo>());
                    m_ConcurrentMessageQueues.Add(msgId, new Queue<MessageInfo>());
                }
                else {
#if DEBUG
                    string err = string.Format("Story {0} DSL, onmessage {1} duplicate, discard it ! line:{2}", m_StoryId, msgId, part.GetLine());
                    throw new Exception(err);
#else
                    LogSystem.Error("Story {0} DSL, onmessage {1} duplicate, discard it !", m_StoryId, msgId);
#endif
                }
            }
        }

        private void EnsureCalculatorCallbacks(DslCalculator calculator)
        {
            calculator.OnTryGetVariable = TryGetVariableCallback;
            calculator.OnTrySetVariable = TrySetVariableCallback;
        }

        private bool TryGetVariableCallback(string name, out BoxedValue val)
        {
            if (name == "this") {
                val = BoxedValue.FromObject(this);
                return true;
            }
            if (name.StartsWith("@@")) {
                return CrossThreadVariableStore.TryGetValue(name, out val);
            }
            if (m_ContextVariables != null && !name.StartsWith("@") && m_ContextVariables.TryGetValue(name, out val)) {
                return true;
            }
            val = BoxedValue.NullObject;
            return false;
        }

        private bool TrySetVariableCallback(string name, ref BoxedValue val)
        {
            if (name.StartsWith("@@")) {
                CrossThreadVariableStore.SetValue(name, val);
                return true;
            }
            if (m_ContextVariables != null && !name.StartsWith("@")) {
                if (m_ContextVariables.ContainsKey(name)) {
                    m_ContextVariables[name] = val;
                }
                else {
                    m_ContextVariables.Add(name, val);
                }
                return true;
            }
            return false;
        }

        public void Reset()
        {
            Reset(true);
        }

        public void Reset(bool logIfTriggered)
        {
            m_IsTerminated = false;
            m_IsPaused = false;
            StopAllCoroutines();
            foreach (KeyValuePair<string, Queue<MessageInfo>> pair in m_MessageQueues) {
                Queue<MessageInfo> queue = pair.Value;
                if (null != queue)
                    ClearMessageInfoQueue(queue);
            }
            foreach (KeyValuePair<string, Queue<MessageInfo>> pair in m_ConcurrentMessageQueues) {
                Queue<MessageInfo> queue = pair.Value;
                if (null != queue)
                    ClearMessageInfoQueue(queue);
            }
            m_Message2TriggerTimes.Clear();
            m_MessageCount = 0;
            m_ConcurrentMessageCount = 0;
            m_Calculator?.Clear();
        }

        public void Start()
        {
            m_LastTickTime = 0;
            m_CurTime = 0;

            // Pre-populate calculator globals from DSL 'local' section and external staging
            if (m_Calculator != null) {
                foreach (KeyValuePair<string, BoxedValue> pair in m_PreInitedLocalVariables) {
                    m_Calculator.SetGlobalVariable(pair.Key, pair.Value);
                }
                foreach (KeyValuePair<string, BoxedValue> pair in m_InstanceVariables) {
                    m_Calculator.SetGlobalVariable(pair.Key, pair.Value);
                }
            }
            SendMessage("start");
        }

        public BoxedValueList NewBoxedValueList()
        {
            var args = m_BoxedValueListPool.Alloc();
            args.Clear();
            return args;
        }

        public void RecycleBoxedValueList(BoxedValueList list)
        {
            m_BoxedValueListPool.Recycle(list);
        }

        public void SendMessage(string msgId)
        {
            var args = NewBoxedValueList();
            SendMessage(msgId, args);
        }

        public void SendMessage(string msgId, BoxedValue arg1)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            SendMessage(msgId, args);
        }

        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            SendMessage(msgId, args);
        }

        public void SendMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            SendMessage(msgId, args);
        }

        public void SendMessage(string msgId, BoxedValueList args)
        {
            MessageInfo msgInfo = m_MessageInfoPool.Alloc();
            msgInfo.m_MsgId = msgId;
            msgInfo.m_Args = args;
            Queue<MessageInfo> queue;
            if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                LogSystem.Warn("StoryInstance queue message {0}", msgId);
                queue.Enqueue(msgInfo);
                ++m_MessageCount;
            }
            else {
                // Ignore unprocessed messages
            }
        }

        public void SendConcurrentMessage(string msgId)
        {
            var args = NewBoxedValueList();
            SendConcurrentMessage(msgId, args);
        }

        public void SendConcurrentMessage(string msgId, BoxedValue arg1)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            SendConcurrentMessage(msgId, args);
        }

        public void SendConcurrentMessage(string msgId, BoxedValue arg1, BoxedValue arg2)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            SendConcurrentMessage(msgId, args);
        }

        public void SendConcurrentMessage(string msgId, BoxedValue arg1, BoxedValue arg2, BoxedValue arg3)
        {
            var args = NewBoxedValueList();
            args.Add(arg1);
            args.Add(arg2);
            args.Add(arg3);
            SendConcurrentMessage(msgId, args);
        }

        public void SendConcurrentMessage(string msgId, BoxedValueList args)
        {
            MessageInfo msgInfo = m_MessageInfoPool.Alloc();
            msgInfo.m_MsgId = msgId;
            msgInfo.m_Args = args;
            Queue<MessageInfo> queue;
            if (m_ConcurrentMessageQueues.TryGetValue(msgId, out queue)) {
                if (msgId != "start" && msgId != "PlayerStart" && !msgId.StartsWith("common_")) {
                    LogSystem.Warn("StoryInstance queue concurrent message {0}", msgId);
                }
                queue.Enqueue(msgInfo);
                ++m_ConcurrentMessageCount;
            }
            else {
                // Ignore unprocessed messages
            }
        }

        public int CountMessage(string msgId)
        {
            int ct = 0;
            Queue<MessageInfo> queue;
            if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                ct = queue.Count;
                ct += m_ActiveCoroutines.FindAll(c => c.MessageId == msgId).Count;
            }
            if (m_ConcurrentMessageQueues.TryGetValue(msgId, out queue)) {
                ct += queue.Count;
                ct += m_ConcurrentCoroutines.FindAll(c => c.MessageId == msgId).Count;
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
                        ClearMessageInfoQueue(queue);
                }
                foreach (KeyValuePair<string, Queue<MessageInfo>> pair in m_ConcurrentMessageQueues) {
                    Queue<MessageInfo> queue = pair.Value;
                    if (null != queue)
                        ClearMessageInfoQueue(queue);
                }
                m_MessageCount = 0;
                m_ConcurrentMessageCount = 0;
            }
            else {
                for (int i = 0; i < len; ++i) {
                    string msgId = msgIds[i];
                    Queue<MessageInfo> queue;
                    if (m_MessageQueues.TryGetValue(msgId, out queue)) {
                        m_MessageCount -= queue.Count;
                        ClearMessageInfoQueue(queue);
                    }
                    if (m_ConcurrentMessageQueues.TryGetValue(msgId, out queue)) {
                        m_ConcurrentMessageCount -= queue.Count;
                        ClearMessageInfoQueue(queue);
                    }
                }
            }
        }

        public void SuspendMessageHandler(string msgId, bool suspend)
        {
            foreach (var coroutine in m_ActiveCoroutines) {
                if (coroutine.MessageId == msgId) {
                    coroutine.IsSuspended = suspend;
                }
            }
            foreach (var coroutine in m_ConcurrentCoroutines) {
                if (coroutine.MessageId == msgId) {
                    coroutine.IsSuspended = suspend;
                }
            }
        }

        public bool CanSleep()
        {
            return m_IsPaused || m_MessageCount + m_ConcurrentMessageCount <= 0 && m_ActiveCoroutines.Count + m_ConcurrentCoroutines.Count <= 0;
        }

        public void Tick(long curTime)
        {
            if (m_IsPaused) {
                m_LastTickTime = curTime;
                return;
            }

            m_IsInTick = true;
            try {
                long delta = 0;
                if (m_LastTickTime == 0) {
                    m_LastTickTime = curTime;
                }
                else {
                    delta = curTime - m_LastTickTime;
                    m_LastTickTime = curTime;
                    m_CurTime += delta;
                }

                ProcessMessageQueues(curTime);
                ProcessConcurrentMessageQueues(curTime);

                TickCoroutines(m_ActiveCoroutines, delta);
                TickCoroutines(m_ConcurrentCoroutines, delta);
            }
            finally {
                m_IsInTick = false;
            }
        }

        public long CurrentTime
        {
            get { return m_CurTime; }
        }

        private void ProcessMessageQueues(long curTime)
        {
            const int c_MaxMsgCountPerTick = 64;
            foreach (var pair in m_MessageQueues) {
                string msgId = pair.Key;
                Queue<MessageInfo> queue = pair.Value;
                for (int msgCt = 0; msgCt < c_MaxMsgCountPerTick && queue.Count > 0; ++msgCt) {
                    MessageInfo info = queue.Dequeue();
                    --m_MessageCount;
                    UpdateMessageTriggerTime(info.m_MsgId, curTime);
                    TriggerMessage(msgId, info.m_Args, false);
                    info.Clear();
                    m_MessageInfoPool.Recycle(info);
                }
            }
        }

        private void ProcessConcurrentMessageQueues(long curTime)
        {
            const int c_MaxConcurrentMsgCountPerTick = 16;
            foreach (var pair in m_ConcurrentMessageQueues) {
                Queue<MessageInfo> queue = pair.Value;
                for (int concurrentMsgCt = 0; concurrentMsgCt < c_MaxConcurrentMsgCountPerTick && queue.Count > 0; ++concurrentMsgCt) {
                    MessageInfo info = queue.Dequeue();
                    --m_ConcurrentMessageCount;
                    UpdateMessageTriggerTime(info.m_MsgId, curTime);
                    TriggerMessage(info.m_MsgId, info.m_Args, true);
                    info.Clear();
                    m_MessageInfoPool.Recycle(info);
                }
            }
        }

        private void TriggerMessage(string msgId, BoxedValueList args, bool isConcurrent)
        {
            string funcName;
            if (!m_HandlerFunctionNames.TryGetValue(msgId, out funcName)) {
                return;
            }

            var calculator = m_Calculator;
            DslCalculator.FuncInfo funcInfo;
            if (!calculator.TryGetFuncInfo(funcName, out funcInfo)) {
                return;
            }

            var calcArgs = new List<BoxedValue>();
            for (int i = 0; i < args.Count; ++i) {
                calcArgs.Add(args[i]);
            }

            var asyncResult = new AsyncCalcResult();
            var enumerator = calculator.CalcAsync<StoryInstance>(calcArgs, this, funcInfo, asyncResult);

            var coroutine = new CoroutineInfo();
            coroutine.MessageId = msgId;
            coroutine.AsyncResult = asyncResult;
            coroutine.IsConcurrent = isConcurrent;
            coroutine.AsyncContext = calculator.CreateAsyncContext();
            coroutine.EnumeratorStack.Push(enumerator);

            if (isConcurrent) {
                m_ConcurrentCoroutines.Add(coroutine);
            }
            else {
                m_ActiveCoroutines.Add(coroutine);
            }
        }

        private void TickCoroutines(List<CoroutineInfo> coroutines, long delta)
        {
            var calculator = m_Calculator;
            for (int i = coroutines.Count - 1; i >= 0; --i) {
                var coroutine = coroutines[i];
                if (coroutine.IsSuspended) {
                    continue;
                }
                if (coroutine.AsyncResult.IsCompleted) {
                    continue;
                }
                try {
                    m_CurrentCoroutine = coroutine;
                    calculator.SetAsyncContext(coroutine.AsyncContext);

                    var stack = coroutine.EnumeratorStack;
                    bool yieldNull = false;
                    while (stack.Count > 0 && !yieldNull) {
                        var top = stack.Peek();
                        bool hasMore = top.MoveNext();
                        if (hasMore) {
                            if (top.Current is IEnumerator subEnum) {
                                stack.Push(subEnum);
                                continue;
                            }
                            yieldNull = true;
                        }
                        else {
                            stack.Pop();
                        }
                    }

                    if (stack.Count == 0 || coroutine.AsyncResult.IsCompleted) {
                        coroutine.AsyncResult.IsCompleted = true;
                    }
                }
                catch (Exception ex) {
                    LogSystem.Error("Story {0} message {1} coroutine error: {2}\n{3}", m_StoryId, coroutine.MessageId, ex.Message, ex.StackTrace);
                    coroutine.AsyncResult.IsCompleted = true;
                }
                finally {
                    calculator.SaveAsyncContext(coroutine.AsyncContext);
                    m_CurrentCoroutine = null;
                }
                if (coroutine.AsyncResult.IsCompleted) {
                    DisposeCoroutineEnumerators(coroutine);
                    coroutines.RemoveAt(i);
                }
            }
        }

        private void DisposeCoroutineEnumerators(CoroutineInfo coroutine)
        {
            while (coroutine.EnumeratorStack.Count > 0) {
                var top = coroutine.EnumeratorStack.Pop();
                var disp = top as IDisposable;
                if (null != disp) {
                    try { disp.Dispose(); } catch { }
                }
            }
        }

        private void StopAllCoroutines()
        {
            foreach (var c in m_ActiveCoroutines) {
                DisposeCoroutineEnumerators(c);
            }
            foreach (var c in m_ConcurrentCoroutines) {
                DisposeCoroutineEnumerators(c);
            }
            m_ActiveCoroutines.Clear();
            m_ConcurrentCoroutines.Clear();
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
            }
            else {
                m_Message2TriggerTimes.Add(msgId, time);
            }
        }

        private void ClearMessageInfoQueue(Queue<MessageInfo> queue)
        {
            while (queue.Count > 0) {
                var mi = queue.Dequeue();
                m_BoxedValueListPool.Recycle(mi.m_Args);
                mi.Clear();
                m_MessageInfoPool.Recycle(mi);
            }
        }

        internal class CoroutineInfo
        {
            public string MessageId;
            public Stack<IEnumerator> EnumeratorStack = new Stack<IEnumerator>();
            public AsyncCalcResult AsyncResult;
            public bool IsConcurrent;
            public bool IsSuspended;
            public bool CanSkip;
            public bool IsTriggered;
            public AsyncTaskRuntimeContext AsyncContext;
        }

        private class MessageInfo
        {
            public string m_MsgId = null;
            public BoxedValueList m_Args = null;

            public void Clear()
            {
                m_MsgId = null;
                m_Args = null;
            }
        }

        private SimpleObjectPool<MessageInfo> m_MessageInfoPool = new SimpleObjectPool<MessageInfo>();
        private SimpleObjectPool<BoxedValueList> m_BoxedValueListPool = new SimpleObjectPool<BoxedValueList>();
        private long m_CurTime = 0;
        private long m_LastTickTime = 0;
        private StrBoxedValueDict m_ContextVariables = null;
        private StrBoxedValueDict m_InstanceVariables = new StrBoxedValueDict();
        private bool m_IsDebug = false;
        private string m_StoryId = string.Empty;
        private string m_Namespace = string.Empty;
        private bool m_IsTerminated = false;
        private bool m_IsPaused = false;
        private bool m_IsInTick = false;
        private object m_Context = null;
        private CoroutineInfo m_CurrentCoroutine = null;
        private int m_MessageCount = 0;
        private int m_ConcurrentMessageCount = 0;
        private Dictionary<string, Queue<MessageInfo>> m_MessageQueues = new Dictionary<string, Queue<MessageInfo>>();
        private Dictionary<string, Queue<MessageInfo>> m_ConcurrentMessageQueues = new Dictionary<string, Queue<MessageInfo>>();
        private List<CoroutineInfo> m_ActiveCoroutines = new List<CoroutineInfo>();
        private List<CoroutineInfo> m_ConcurrentCoroutines = new List<CoroutineInfo>();
        private Dictionary<string, long> m_Message2TriggerTimes = new Dictionary<string, long>();
        private StrBoxedValueDict m_PreInitedLocalVariables = new StrBoxedValueDict();
        private Dictionary<string, string> m_HandlerFunctionNames = new Dictionary<string, string>();
        private Dsl.ISyntaxComponent m_Config = null;
        private DslCalculator m_Calculator = null;

        #region IDictionary implementation for 'this.name' DSL syntax support
        private Hashtable m_PropertyDict = new Hashtable();

        /// <summary>
        /// Public indexer for C# access to story instance properties
        /// </summary>
        public BoxedValue this[string name]
        {
            get {
                if (m_PropertyDict[name] is BoxedValue val)
                    return val;
                return BoxedValue.NullObject;
            }
            set {
                m_PropertyDict[name] = value;
            }
        }

        /// <summary>
        /// Clear all dynamic properties
        /// </summary>
        public void ClearProperties()
        {
            m_PropertyDict.Clear();
        }

        // Explicit IDictionary implementation - enables 'this.name' syntax in DSL
        // Delegate to m_PropertyDict which already implements IDictionary
        bool IDictionary.Contains(object key) => m_PropertyDict.Contains(key);
        object IDictionary.this[object key]
        {
            get => m_PropertyDict[key];
            set => m_PropertyDict[key] = value;
        }
        void IDictionary.Add(object key, object value) => m_PropertyDict.Add(key, value);
        void IDictionary.Clear() => m_PropertyDict.Clear();
        void IDictionary.Remove(object key) => m_PropertyDict.Remove(key);
        ICollection IDictionary.Keys => m_PropertyDict.Keys;
        ICollection IDictionary.Values => m_PropertyDict.Values;
        int ICollection.Count => m_PropertyDict.Count;
        bool IDictionary.IsReadOnly => m_PropertyDict.IsReadOnly;
        bool IDictionary.IsFixedSize => m_PropertyDict.IsFixedSize;
        IDictionaryEnumerator IDictionary.GetEnumerator() => m_PropertyDict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => m_PropertyDict.GetEnumerator();
        void ICollection.CopyTo(Array array, int index) => m_PropertyDict.CopyTo(array, index);
        bool ICollection.IsSynchronized => m_PropertyDict.IsSynchronized;
        object ICollection.SyncRoot => m_PropertyDict.SyncRoot;
        #endregion
    }
}
