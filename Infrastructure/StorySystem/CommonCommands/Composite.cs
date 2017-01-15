using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
namespace StorySystem.CommonCommands
{
    /// <summary>
    /// name(arg1,arg2,...);
    /// </summary>
    /// <remarks>
    /// 这里的Name、ArgNames与InitialCommands为同一command定义的各个调用共享。
    /// 由于解析时需要处理交叉引用，先克隆后Load。
    /// 这里的自定义命令支持挂起-恢复执行或递归(性能较低，仅处理小规模问题)，但
    /// 二者不能同时使用。
    /// 注意：所有依赖InitialCommands等共享数据的其它成员，初始化需要写成lazy的样式，不要在Clone与Load里初始化，因为
    /// 此时共享数据可能还不完整！
    /// </remarks>
    internal sealed class CompositeCommand : AbstractStoryCommand
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
        public IList<StorySystem.IStoryCommand> InitialCommands
        {
            get { return m_InitialCommands; }
        }
        public void InitSharedData()
        {
            m_ArgNames = new List<string>();
            m_InitialCommands = new List<IStoryCommand>();
        }
        public void NewCall(StoryInstance instance, object iterator, object[] args)
        {
            //command的执行不像函数，它支持类似协程的机制，允许暂时挂起，稍后继续，这意味着并非每次调用ExecCommand都对应语义上的一次过程调用
            //，因此栈的创建不能在ExecCommand里进行（事实上，在ExecCommand里无法区分本次执行是一次新的过程调用还是一次挂起后的继续执行）。

            StackElementInfo stackInfo = NewStackElementInfo();
            //调用实参部分需要在栈建立之前运算，结果需要记录在栈上
            for (int i = 0; i < m_LoadedArgs.Count; ++i) {
                stackInfo.m_Args.Add(m_LoadedArgs[i].Clone());
            }
            for (int i = 0; i < stackInfo.m_Args.Count; i++) {
                stackInfo.m_Args[i].Evaluate(instance, iterator, args);
            }
            //实参处理完，进入函数体执行，创建新的栈
            PushStack(instance, stackInfo);
        }
        public override IStoryCommand Clone()
        {
            CompositeCommand cmd = new CompositeCommand();
            cmd.m_LoadedArgs = m_LoadedArgs;
            cmd.m_Name = m_Name;
            cmd.m_ArgNames=m_ArgNames;
            cmd.m_InitialCommands=m_InitialCommands;
            cmd.IsCompositeCommand = true;
            if (null == cmd.m_LeadCommand) {
                cmd.m_LeadCommand = new CompositeCommandHelper(cmd);
            }
            return cmd;
        }
        public override IStoryCommand LeadCommand
        {
            get
            {
                return m_LeadCommand;
            }
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (m_Stack.Count > 0) {
                StackElementInfo stackInfo = m_Stack.Peek();
                for (int i = 0; i < m_ArgNames.Count; ++i) {
                    if (i < stackInfo.m_Args.Count) {
                        instance.SetVariable(m_ArgNames[i], stackInfo.m_Args[i].Value);
                    } else {
                        instance.SetVariable(m_ArgNames[i], null);
                    }
                }
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta, object iterator, object[] args)
        {
            bool ret = false;
            if (m_Stack.Count > 0) {
                StackElementInfo stackInfo = m_Stack.Peek();
                instance.StackVariables = stackInfo.m_StackVariables;
                if (stackInfo.m_CommandQueue.Count == 0 && !stackInfo.m_AlreadyExecute) {
                    Evaluate(instance, iterator, args);
                    Prepare(stackInfo);
                    stackInfo.m_AlreadyExecute = true;
                }
                if (stackInfo.m_CommandQueue.Count > 0) {
                    while (stackInfo.m_CommandQueue.Count > 0) {
                        IStoryCommand cmd = stackInfo.m_CommandQueue.Peek();
                        if (cmd.Execute(instance, delta, iterator, args)) {
                            ret = true;
                            break;
                        } else {
                            cmd.Reset();
                            stackInfo.m_CommandQueue.Dequeue();
                        }
                    }
                }
                if (!ret) {
                    PopStack(instance);
                    stackInfo.m_AlreadyExecute = false;
                }
            }
            return ret;
        }
        protected override void Load(Dsl.CallData callData)
        {
            m_LoadedArgs = new List<IStoryValue<object>>();
            int num = callData.GetParamNum();
            for (int i = 0; i < num; ++i) {
                StoryValue val = new StoryValue();
                val.InitFromDsl(callData.GetParam(i));
                m_LoadedArgs.Add(val);
            }
            IsCompositeCommand = true;
            if (null == m_LeadCommand) {
                m_LeadCommand = new CompositeCommandHelper(this);
            }
        }

        private void Prepare(StackElementInfo stackInfo)
        {
            if (null != m_InitialCommands && m_FirstStackCommands.Count <= 0) {
                for (int i = 0; i < m_InitialCommands.Count; ++i) {
                    IStoryCommand cmd = m_InitialCommands[i].Clone();
                    m_FirstStackCommands.Add(cmd);
                }
            }
            if (m_Stack.Count <= 1) {
                for (int i = 0; i < m_FirstStackCommands.Count; ++i) {
                    IStoryCommand cmd = m_FirstStackCommands[i];
                    if (null != cmd.LeadCommand)
                        stackInfo.m_CommandQueue.Enqueue(cmd.LeadCommand);
                    stackInfo.m_CommandQueue.Enqueue(cmd);
                }
            } else {
                for (int i = 0; i < m_InitialCommands.Count; ++i) {
                    IStoryCommand cmd = m_InitialCommands[i].Clone();
                    if (null != cmd.LeadCommand)
                        stackInfo.m_CommandQueue.Enqueue(cmd.LeadCommand);
                    stackInfo.m_CommandQueue.Enqueue(cmd);
                }
            }
        }
        private StackElementInfo NewStackElementInfo()
        {
            if (m_Stack.Count <= 0) {
                m_FirstStackInfo.Reset();
                return m_FirstStackInfo;
            } else {
                return new StackElementInfo();
            }
        }
        private void PushStack(StoryInstance instance, StackElementInfo info)
        {
            if (m_Stack.Count <= 0) {
                m_TopStack = instance.StackVariables;
            }
            m_Stack.Push(info);
            instance.StackVariables = info.m_StackVariables;
        }
        private void PopStack(StoryInstance instance)
        {
            if (m_Stack.Count > 0) {
                m_Stack.Pop();
                if (m_Stack.Count > 0) {
                    StackElementInfo info = m_Stack.Peek();
                    instance.StackVariables = info.m_StackVariables;
                } else {
                    instance.StackVariables = m_TopStack;
                }
            }
        }

        private class StackElementInfo
        {
            internal List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
            internal Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();
            internal bool m_AlreadyExecute = false;
            internal Dictionary<string, object> m_StackVariables = new Dictionary<string, object>();

            internal void Reset()
            {
                m_Args.Clear();
                m_CommandQueue.Clear();
                m_StackVariables.Clear();
                m_AlreadyExecute = false;
            }
        }

        private StackElementInfo m_FirstStackInfo = new StackElementInfo();
        private List<IStoryCommand> m_FirstStackCommands = new List<IStoryCommand>();

        private Dictionary<string, object> m_TopStack = null;
        private Stack<StackElementInfo> m_Stack = new Stack<StackElementInfo>();

        private List<IStoryValue<object>> m_LoadedArgs = null;
        private CompositeCommandHelper m_LeadCommand = null;

        private string m_Name = string.Empty;
        private List<string> m_ArgNames = null;
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
    internal sealed class CompositeCommandHelper : AbstractStoryCommand
    {
        public CompositeCommandHelper(CompositeCommand cmd)
        {
            m_Cmd = cmd;
        }
        public override IStoryCommand Clone()
        {
            return null;
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Cmd.NewCall(instance, iterator, args);
        }
        private CompositeCommand m_Cmd = null;
    }
}
