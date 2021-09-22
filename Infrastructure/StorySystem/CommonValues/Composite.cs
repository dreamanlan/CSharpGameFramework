using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace StorySystem.CommonValues
{
    /// <summary>
    /// name(arg1,arg2,...)
    /// [{
    ///     name1(val1);
    ///     name2(val2);
    ///     ...
    /// }];
    /// </summary>
    /// <remarks>
    /// 这里的Name、ArgNames、OptArgs、ReturnName与InitialCommands为同一value定义的各个调用共享。
    /// 由于解析时需要处理交叉引用，先克隆后InitFromDsl。
    /// 这里的自定义函数支持递归(性能较低，仅处理小规模问题)，不支持基于时间的wait命令，亦即不支持挂起-恢复执行。
    /// 注意：
    /// 1、所有依赖InitialCommands等共享数据的其它成员，初始化需要写成lazy的样式，不要在Clone与InitFromDsl里初始化，因为
    /// 此时共享数据可能还不完整！
    /// 2、因为自定义的命令与值在使用时有函数调用语义，需要可以访问传递的参数。而Evaluate接口只有一组参数，这限制了自定义
    /// 命令与值的形式至多是Function样式而不应支持Statement样式。
    /// </remarks>
    internal sealed class CompositeValue : IStoryValue
    {
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public IList<string> ArgNames
        {
            get { return m_ArgNames; }
        }
        public string ReturnName
        {
            get { return m_ReturnName; }
            set { m_ReturnName = value; }
        }
        public IDictionary<string, Dsl.ISyntaxComponent> OptArgs
        {
            get { return m_OptArgs; }
        }
        public IList<StorySystem.IStoryCommand> InitialCommands
        {
            get { return m_InitialCommands; }
        }
        public void InitSharedData()
        {
            m_ArgNames = new List<string>();
            m_OptArgs = new Dictionary<string, Dsl.ISyntaxComponent>();
            m_InitialCommands = new List<IStoryCommand>();
        }
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            m_LoadedArgs = new List<IStoryValue>();
            Dsl.FunctionData funcData = param as Dsl.FunctionData;
            if (null != funcData) {
                Load(funcData);
            }
        }
        public IStoryValue Clone()
        {
            CompositeValue val = new CompositeValue();
            val.m_LoadedArgs = m_LoadedArgs;
            val.m_LoadedOptArgs = m_LoadedOptArgs;
            val.m_Name = m_Name;
            val.m_ArgNames = m_ArgNames;
            val.m_ReturnName = m_ReturnName;
            val.m_OptArgs = m_OptArgs;
            val.m_InitialCommands = m_InitialCommands;
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            var stackInfo = StoryLocalInfo.New();
            var runtime = StoryRuntime.New();
            //调用实参部分需要在栈建立之前运算，结果需要记录在栈上
            for (int i = 0; i < m_LoadedArgs.Count; ++i) {
                stackInfo.Args.Add(m_LoadedArgs[i].Clone());
            }
            foreach (var pair in m_LoadedOptArgs) {
                stackInfo.OptArgs.Add(pair.Key, pair.Value.Clone());
            }
            runtime.Arguments = instance.NewBoxedValueList();
            runtime.Arguments.Capacity = stackInfo.Args.Count;
            for (int i = 0; i < stackInfo.Args.Count; i++) {
                stackInfo.Args[i].Evaluate(instance, handler, iterator, args);
                runtime.Arguments.Add(stackInfo.Args[i].Value);
            }
            runtime.Iterator = stackInfo.Args.Count;
            foreach (var pair in stackInfo.OptArgs) {
                pair.Value.Evaluate(instance, handler, iterator, args);
            }
            //实参处理完，进入函数体执行，创建新的栈
            PushStack(instance, handler, stackInfo, runtime);
            try {
                for (int i = 0; i < m_ArgNames.Count; ++i) {
                    if (i < stackInfo.Args.Count) {
                        instance.SetVariable(m_ArgNames[i], stackInfo.Args[i].Value);
                    } else {
                        instance.SetVariable(m_ArgNames[i], BoxedValue.NullObject);
                    }
                }
                foreach (var pair in stackInfo.OptArgs) {
                    instance.SetVariable(pair.Key, pair.Value.Value);
                }
                Prepare(runtime);
                stackInfo.HaveValue = true;
                runtime.Tick(instance, handler, long.MaxValue);
                BoxedValue val;
                instance.TryGetVariable(m_ReturnName, out val);
                stackInfo.Value = val;
            } finally {
                instance.RecycleBoxedValueList(runtime.Arguments);
                PopStack(instance, handler);
            }
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }

        private void Load(Dsl.FunctionData funcData)
        {
            if (funcData.IsHighOrder) {
                var cd = funcData.LowerOrderFunction;
                LoadCall(cd);
            }
            else if (funcData.HaveParam()) {
                LoadCall(funcData);
            }
            if (funcData.HaveStatement()) {
                foreach (var comp in funcData.Params) {
                    var fcd = comp as Dsl.FunctionData;
                    if (null != fcd) {
                        var key = fcd.GetId();
                        StoryValue val = new StoryValue();
                        val.InitFromDsl(fcd.GetParam(0));
                        m_LoadedOptArgs[key] = val;
                    }
                }
            }
        }
        private void LoadCall(Dsl.FunctionData callData)
        {
            m_LoadedOptArgs = new Dictionary<string, IStoryValue>();
            foreach (var pair in m_OptArgs) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(pair.Value);
                m_LoadedOptArgs.Add(pair.Key, val);
            }
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_LoadedArgs.Add(val);
            }
        }
        private void Prepare(StoryRuntime runtime)
        {
            if (null != m_InitialCommands) {
                for (int i = 0; i < m_InitialCommands.Count; ++i) {
                    IStoryCommand cmd = m_InitialCommands[i].Clone();
                    if (null != cmd.PrologueCommand)
                        runtime.CommandQueue.Enqueue(cmd.PrologueCommand);
                    runtime.CommandQueue.Enqueue(cmd);
                    if (null != cmd.EpilogueCommand)
                        runtime.CommandQueue.Enqueue(cmd.EpilogueCommand);
                }
            }
        }
        private void PushStack(StoryInstance instance, StoryMessageHandler handler, StoryLocalInfo info, StoryRuntime runtime)
        {
            handler.PushLocalInfo(info);
            handler.PushRuntime(runtime);
            instance.StackVariables = info.StackVariables;
        }
        private void PopStack(StoryInstance instance, StoryMessageHandler handler)
        {
            handler.PopRuntime(instance);
            var old = handler.PeekLocalInfo();
            bool haveVal = old.HaveValue;
            var val = old.Value;
            handler.PopLocalInfo();
            if (handler.LocalInfoStack.Count > 0) {
                var info = handler.PeekLocalInfo();
                instance.StackVariables = info.StackVariables;
                m_HaveValue = haveVal;
                m_Value = val;
            } else {
                instance.StackVariables = handler.StackVariables;
                m_HaveValue = haveVal;
                m_Value = val;
            }
        }

        private bool m_HaveValue;
        private BoxedValue m_Value;
        private List<IStoryValue> m_LoadedArgs = null;
        private Dictionary<string, IStoryValue> m_LoadedOptArgs = null;

        private string m_Name = string.Empty;
        private List<string> m_ArgNames = null;
        private string m_ReturnName = string.Empty;
        private Dictionary<string, Dsl.ISyntaxComponent> m_OptArgs = null;
        private List<IStoryCommand> m_InitialCommands = null;
    }
    internal sealed class CompositeValueFactory : IStoryValueFactory
    {
        public IStoryValue Build()
        {
            return m_Val.Clone();
        }
        internal CompositeValueFactory(CompositeValue val)
        {
            m_Val = val;
        }
        private CompositeValue m_Val;
    }
    internal sealed class GetCmdSubstValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() > 0) {
                m_Id.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            GetCmdSubstValue val = new GetCmdSubstValue();
            val.m_Id = m_Id.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Id.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }
        private void TryUpdateValue()
        {
            if (m_Id.HaveValue) {
                string id = m_Id.Value;
                m_HaveValue = true;
                string substId;
                if(StoryCommandManager.Instance.TryGetSubstitute(id, out substId)) {
                    m_Value = substId;
                }
                else {
                    m_Value = BoxedValue.NullObject;
                }
            }
        }

        private IStoryValue<string> m_Id = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
    internal sealed class GetValSubstValue : IStoryValue
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            Dsl.FunctionData callData = param as Dsl.FunctionData;
            if (null != callData && callData.GetParamNum() > 0) {
                m_Id.InitFromDsl(callData.GetParam(0));
                TryUpdateValue();
            }
        }
        public IStoryValue Clone()
        {
            GetValSubstValue val = new GetValSubstValue();
            val.m_Id = m_Id.Clone();
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_HaveValue = false;
            m_Id.Evaluate(instance, handler, iterator, args);
            TryUpdateValue();
        }
        public bool HaveValue
        {
            get {
                return m_HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
                return m_Value;
            }
        }
        private void TryUpdateValue()
        {
            if (m_Id.HaveValue) {
                string id = m_Id.Value;
                m_HaveValue = true;
                string substId;
                if (StoryValueManager.Instance.TryGetSubstitute(id, out substId)) {
                    m_Value = substId;
                }
                else {
                    m_Value = BoxedValue.NullObject;
                }
            }
        }

        private IStoryValue<string> m_Id = new StoryValue<string>();
        private bool m_HaveValue;
        private BoxedValue m_Value;
    }
}
