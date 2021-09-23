using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using Dsl;

namespace StorySystem.CommonCommands
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
    /// 这里的Name、ArgNames与InitialCommands为同一command定义的各个调用共享。
    /// 由于解析时需要处理交叉引用，先克隆后Load。
    /// 这里的自定义命令支持挂起-恢复执行或递归(性能较低，仅处理小规模问题)，但
    /// 二者不能同时使用。
    /// 注意：
    /// 1、所有依赖InitialCommands等共享数据的其它成员，初始化需要写成lazy的样式，不要在Clone与Load里初始化，因为
    /// 此时共享数据可能还不完整！
    /// 2、因为自定义的命令与值在使用时有函数调用语义，需要可以访问传递的参数。而Evaluate接口只有一组参数，这限制了自定义
    /// 命令与值的形式至多是Function样式而不应支持Statement样式。
    /// </remarks>
    internal sealed class CompositeCommand : AbstractStoryCommand
    {
        public override bool IsCompositeCommand { get { return true; } }
        internal string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        internal IList<string> ArgNames
        {
            get { return m_ArgNames; }
        }
        internal IDictionary<string, Dsl.ISyntaxComponent> OptArgs
        {
            get { return m_OptArgs; }
        }
        internal IList<StorySystem.IStoryCommand> InitialCommands
        {
            get { return m_InitialCommands; }
        }
        internal void InitSharedData()
        {
            m_ArgNames = new List<string>();
            m_OptArgs = new Dictionary<string, ISyntaxComponent>();
            m_InitialCommands = new List<IStoryCommand>();
        }
        internal void PreCall(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            //command的执行不像函数，它支持类似协程的机制，允许暂时挂起，稍后继续，这意味着并非每次调用ExecCommand都对应语义上的一次过程调用
            //，因此栈的创建不能在ExecCommand里进行（事实上，在ExecCommand里无法区分本次执行是一次新的过程调用还是一次挂起后的继续执行）。

            var stackInfo = StoryLocalInfo.New();
            //调用实参部分需要在栈建立之前运算，结果需要记录在栈上
            for (int i = 0; i < m_LoadedArgs.Count; ++i) {
                stackInfo.Args.Add(m_LoadedArgs[i].Clone());
            }
            foreach (var pair in m_LoadedOptArgs) {
                stackInfo.OptArgs.Add(pair.Key, pair.Value.Clone());
            }
            for (int i = 0; i < stackInfo.Args.Count; i++) {
                stackInfo.Args[i].Evaluate(instance, handler, iterator, args);
            }
            foreach (var pair in stackInfo.OptArgs) {
                pair.Value.Evaluate(instance, handler, iterator, args);
            }
            //实参处理完，进入函数体执行，创建新的栈
            PushStack(instance, handler, stackInfo);
        }
        internal void PostCall(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            PopStack(instance, handler);
        }
        public override IStoryCommand PrologueCommand
        {
            get {
                return m_PrologueCommand;
            }
        }
        public override IStoryCommand EpilogueCommand
        {
            get {
                return m_EpilogueCommand;
            }
        }
        protected override IStoryCommand CloneCommand()
        {
            CompositeCommand cmd = new CompositeCommand();
            cmd.m_LoadedArgs = m_LoadedArgs;
            cmd.m_LoadedOptArgs = m_LoadedOptArgs;
            cmd.m_Name = m_Name;
            cmd.m_ArgNames = m_ArgNames;
            cmd.m_OptArgs = m_OptArgs;
            cmd.m_InitialCommands = m_InitialCommands;
            if (null == cmd.m_PrologueCommand) {
                cmd.m_PrologueCommand = new CompositePrologueCommandHelper(cmd);
            }
            if (null == cmd.m_EpilogueCommand) {
                cmd.m_EpilogueCommand = new CompositeEpilogueCommandHelper(cmd);
            }
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            //PreCall do all things, so do nothing here.
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            var runtime = handler.PeekRuntime();
            if (runtime.CompositeReentry) {
                return false;
            }
            bool ret = false;
            var stackInfo = handler.PeekLocalInfo();
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
            Prepare(handler);
            runtime = handler.PeekRuntime();
            runtime.Arguments = instance.NewBoxedValueList();
            runtime.Arguments.Capacity = stackInfo.Args.Count;
            for (int i = 0; i < stackInfo.Args.Count; i++) {
                runtime.Arguments.Add(stackInfo.Args[i].Value);
            }
            runtime.Iterator = stackInfo.Args.Count;
            //没有wait之类命令直接执行
            runtime.Tick(instance, handler, delta);
            instance.RecycleBoxedValueList(runtime.Arguments);
            if (runtime.CommandQueue.Count == 0) {
                handler.PopRuntime(instance);
            } else {
                //遇到wait命令，跳出执行，之后直接在StoryMessageHandler里执行栈顶的命令队列（降低开销）
                ret = true;
            }
            return ret;
        }
        protected override bool Load(FunctionData funcData)
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
            return true;
        }
        private bool LoadCall(Dsl.FunctionData callData)
        {
            m_LoadedOptArgs = new Dictionary<string, IStoryValue>();
            foreach (var pair in m_OptArgs) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(pair.Value);
                m_LoadedOptArgs.Add(pair.Key, val);
            }
            m_LoadedArgs = new List<IStoryValue>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_LoadedArgs.Add(val);
            }
            if (null == m_PrologueCommand) {
                m_PrologueCommand = new CompositePrologueCommandHelper(this);
            }
            if (null == m_EpilogueCommand) {
                m_EpilogueCommand = new CompositeEpilogueCommandHelper(this);
            }
            return true;
        }

        private void Prepare(StoryMessageHandler handler)
        {
            var runtime = StoryRuntime.New();
            handler.PushRuntime(runtime);
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
        private void PushStack(StoryInstance instance, StoryMessageHandler handler, StoryLocalInfo info)
        {
            handler.PushLocalInfo(info);
            instance.StackVariables = info.StackVariables;
        }
        private void PopStack(StoryInstance instance, StoryMessageHandler handler)
        {
            handler.PopLocalInfo();
            if (handler.LocalInfoStack.Count > 0) {
                instance.StackVariables = handler.PeekLocalInfo().StackVariables;
            } else {
                instance.StackVariables = handler.StackVariables;
            }
        }

        private List<IStoryValue> m_LoadedArgs = null;
        private Dictionary<string, IStoryValue> m_LoadedOptArgs = null;
        private CompositePrologueCommandHelper m_PrologueCommand = null;
        private CompositeEpilogueCommandHelper m_EpilogueCommand = null;

        private string m_Name = string.Empty;
        private List<string> m_ArgNames = null;
        private Dictionary<string, Dsl.ISyntaxComponent> m_OptArgs = null;
        private List<IStoryCommand> m_InitialCommands = null;
    }
    internal sealed class CompositeCommandFactory : IStoryCommandFactory
    {
        public IStoryCommand Create()
        {
            return m_Cmd.Clone();
        }
        internal CompositeCommandFactory(CompositeCommand cmd)
        {
            m_Cmd = cmd;
        }
        private CompositeCommand m_Cmd = null;
    }
    internal sealed class CompositePrologueCommandHelper : AbstractStoryCommand
    {
        public CompositePrologueCommandHelper(CompositeCommand cmd)
        {
            m_Cmd = cmd;
        }
        protected override IStoryCommand CloneCommand()
        {
            return null;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Cmd.PreCall(instance, handler, iterator, args);
        }
        private CompositeCommand m_Cmd = null;
    }
    internal sealed class CompositeEpilogueCommandHelper : AbstractStoryCommand
    {
        public CompositeEpilogueCommandHelper(CompositeCommand cmd)
        {
            m_Cmd = cmd;
        }
        protected override IStoryCommand CloneCommand()
        {
            return null;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Cmd.PostCall(instance, handler, iterator, args);
        }
        private CompositeCommand m_Cmd = null;
    }
    /// <summary>
    /// substcmd(id, substId);
    /// </summary>
    internal sealed class SubstCmdCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SubstCmdCommand cmd = new SubstCmdCommand();
            cmd.m_Id = m_Id.Clone();
            cmd.m_SubstId = m_SubstId.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Id.Evaluate(instance, handler, iterator, args);
            m_SubstId.Evaluate(instance, handler, iterator, args);

        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_Id.HaveValue && m_SubstId.HaveValue) {
                string id = m_Id.Value;
                string substId = m_SubstId.Value;
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(substId)) {
                    StoryCommandManager.Instance.Substitute(id, substId);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Id.InitFromDsl(callData.GetParam(0));
                m_SubstId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryValue<string> m_Id = new StoryValue<string>();
        private IStoryValue<string> m_SubstId = new StoryValue<string>();
    }
    /// <summary>
    /// clearcmdsubsts();
    /// </summary>
    internal sealed class ClearCmdSubstsCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ClearCmdSubstsCommand cmd = new ClearCmdSubstsCommand();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            StoryCommandManager.Instance.ClearSubstitutes();
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            return true;
        }
    }
    /// <summary>
    /// substval(id, substId);
    /// </summary>
    internal sealed class SubstValCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SubstValCommand cmd = new SubstValCommand();
            cmd.m_Id = m_Id.Clone();
            cmd.m_SubstId = m_SubstId.Clone();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Id.Evaluate(instance, handler, iterator, args);
            m_SubstId.Evaluate(instance, handler, iterator, args);

        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            if (m_Id.HaveValue && m_SubstId.HaveValue) {
                string id = m_Id.Value;
                string substId = m_SubstId.Value;
                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(substId)) {
                    StoryValueManager.Instance.Substitute(id, substId);
                }
            }
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            if (num > 1) {
                m_Id.InitFromDsl(callData.GetParam(0));
                m_SubstId.InitFromDsl(callData.GetParam(1));
            }
            return true;
        }
        private IStoryValue<string> m_Id = new StoryValue<string>();
        private IStoryValue<string> m_SubstId = new StoryValue<string>();
    }
    /// <summary>
    /// clearvalsubsts();
    /// </summary>
    internal sealed class ClearValSubstsCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ClearValSubstsCommand cmd = new ClearValSubstsCommand();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            StoryValueManager.Instance.ClearSubstitutes();
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            return true;
        }
    }
}
