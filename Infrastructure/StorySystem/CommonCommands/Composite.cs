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
    /// The Name, ArgNames and InitialCommands here are shared by each call defined by the same
    /// command.
    /// Since cross-references need to be processed during parsing, clone first and then load.
    /// The custom commands here support suspend-resume execution or recursion (lower performance,
    /// only handles small-scale problems), but not both at the same time.
    /// Notice:
    /// 1. All other members that rely on shared data such as InitialCommands need to be initialized
    /// in a lazy style. Do not initialize in Clone and Load, because the shared data may not be
    /// complete at this time!
    /// 2. Because custom commands and values have function call semantics when used, the passed
    /// parameters need to be accessible. The Evaluate interface has only one set of parameters,
    /// which limits the form of custom commands and values to Function style at most and should
    /// not support Statement style.
    /// </remarks>
    public sealed class CompositeCommand : AbstractStoryCommand
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
            //The execution of command is not like a function. It supports a coroutine-like
            //mechanism, allowing temporary suspension and continuing later. This means that
            //not every call to ExecCommand corresponds to a semantic procedure call, so the
            //stack cannot be created in ExecCommand. (In fact, in ExecCommand, it is impossible
            //to distinguish whether this execution is a new procedure call or a continued
            //execution after suspension).
            var stackInfo = StoryLocalInfo.New();
            //The actual parameter part of the call needs to be calculated before the stack is established
            //, and the result needs to be recorded on the stack.
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
            //After the actual parameters are processed, enter the function body
            //for execution and create a new stack.
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
            //Execute directly without commands such as wait
            runtime.Tick(instance, handler, delta);
            instance.RecycleBoxedValueList(runtime.Arguments);
            if (runtime.CommandQueue.Count == 0) {
                handler.PopRuntime(instance);
            } else {
                //When encountering the wait command, jump out of execution, and then directly
                //execute the command queue on the top of the stack in StoryMessageHandler
                //(reducing overhead)
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
            m_LoadedOptArgs = new Dictionary<string, IStoryFunction>();
            foreach (var pair in m_OptArgs) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(pair.Value);
                m_LoadedOptArgs.Add(pair.Key, val);
            }
            m_LoadedArgs = new List<IStoryFunction>();
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

        private List<IStoryFunction> m_LoadedArgs = null;
        private Dictionary<string, IStoryFunction> m_LoadedOptArgs = null;
        private CompositePrologueCommandHelper m_PrologueCommand = null;
        private CompositeEpilogueCommandHelper m_EpilogueCommand = null;

        private string m_Name = string.Empty;
        private List<string> m_ArgNames = null;
        private Dictionary<string, Dsl.ISyntaxComponent> m_OptArgs = null;
        private List<IStoryCommand> m_InitialCommands = null;
    }
    public sealed class CompositeCommandFactory : IStoryCommandFactory
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
    public sealed class CompositePrologueCommandHelper : AbstractStoryCommand
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
    public sealed class CompositeEpilogueCommandHelper : AbstractStoryCommand
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
    public sealed class SubstCmdCommand : AbstractStoryCommand
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
        private IStoryFunction<string> m_Id = new StoryValue<string>();
        private IStoryFunction<string> m_SubstId = new StoryValue<string>();
    }
    /// <summary>
    /// clearcmdsubsts();
    /// </summary>
    public sealed class ClearCmdSubstsCommand : AbstractStoryCommand
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
    /// substfunc(id, substId);
    /// </summary>
    public sealed class SubstFuncCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            SubstFuncCommand cmd = new SubstFuncCommand();
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
                    StoryFunctionManager.Instance.Substitute(id, substId);
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
        private IStoryFunction<string> m_Id = new StoryValue<string>();
        private IStoryFunction<string> m_SubstId = new StoryValue<string>();
    }
    /// <summary>
    /// clearfuncsubsts();
    /// </summary>
    public sealed class ClearFuncSubstsCommand : AbstractStoryCommand
    {
        protected override IStoryCommand CloneCommand()
        {
            ClearFuncSubstsCommand cmd = new ClearFuncSubstsCommand();
            return cmd;
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            StoryFunctionManager.Instance.ClearSubstitutes();
            return false;
        }
        protected override bool Load(Dsl.FunctionData callData)
        {
            int num = callData.GetParamNum();
            return true;
        }
    }
}
