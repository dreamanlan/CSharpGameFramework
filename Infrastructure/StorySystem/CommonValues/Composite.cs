using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace StorySystem.CommonValues
{
    /// <summary>
    /// name(arg1,arg2,...);
    /// </summary>
    /// <remarks>
    /// 这里的Name、ArgNames、ReturnName与InitialCommands为同一value定义的各个调用共享。
    /// 由于解析时需要处理交叉引用，先克隆后InitFromDsl。
    /// 这里的自定义函数支持递归(性能较低，仅处理小规模问题)，不支持基于时间的wait命令，亦即不支持挂起-恢复执行。
    /// 注意：所有依赖InitialCommands等共享数据的其它成员，初始化需要写成lazy的样式，不要在Clone与InitFromDsl里初始化，因为
    /// 此时共享数据可能还不完整！
    /// </remarks>
    internal sealed class CompositeValue : IStoryValue<object>
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
        public IList<StorySystem.IStoryCommand> InitialCommands
        {
            get { return m_InitialCommands; }
        }
        public void InitSharedData()
        {
            m_ArgNames = new List<string>();
            m_InitialCommands = new List<IStoryCommand>();
        }
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            m_LoadedArgs = new List<IStoryValue<object>>();
            Dsl.CallData callData = param as Dsl.CallData;
            if (null != callData) {
                int num = callData.GetParamNum();
                for (int i = 0; i < num; ++i) {
                    StoryValue val = new StoryValue();
                    val.InitFromDsl(callData.GetParam(i));
                    m_LoadedArgs.Add(val);
                }
            }
        }
        public IStoryValue<object> Clone()
        {
            CompositeValue val = new CompositeValue();
            val.m_LoadedArgs = m_LoadedArgs;
            val.m_Name = m_Name;
            val.m_ArgNames = m_ArgNames;
            val.m_ReturnName = m_ReturnName;
            val.m_InitialCommands = m_InitialCommands;
            val.m_HaveValue = m_HaveValue;
            val.m_Value = m_Value;
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
		{
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
            try {
                for (int i = 0; i < m_ArgNames.Count && i < stackInfo.m_Args.Count; ++i) {
                    instance.SetVariable(m_ArgNames[i], stackInfo.m_Args[i].Value);
                }
                Prepare(stackInfo);
                stackInfo.m_HaveValue = true;
                for (int i = 0; i < stackInfo.m_Commands.Count; ++i) {
                    //函数调用命令需要忽略其中的wait指令（从而不会出现“挂起-恢复”行为），所以这里传的delta值是一个很大的值，目的是为了让wait直接结束
                    stackInfo.m_Commands[i].Execute(instance, StoryValueHelper.c_MaxWaitCommandTime, iterator, args);
                }
                instance.TryGetVariable(m_ReturnName, out stackInfo.m_Value);
            } finally {
                PopStack(instance);
            }
        }
        public bool HaveValue
        {
            get
            {
                if (m_Stack.Count > 0) {
                    return m_Stack.Peek().m_HaveValue;
                } else {
                    return m_HaveValue;
                }
            }
        }
        public object Value
        {
            get
            {
                if (m_Stack.Count > 0) {
                    return m_Stack.Peek().m_Value;
                } else {
                    return m_Value;
                }
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
                        stackInfo.m_Commands.Add(cmd.LeadCommand);
                    stackInfo.m_Commands.Add(cmd);
                }
            } else {
                for (int i = 0; i < m_InitialCommands.Count; ++i) {
                    IStoryCommand cmd = m_InitialCommands[i].Clone();
                    if (null != cmd.LeadCommand)
                        stackInfo.m_Commands.Add(cmd.LeadCommand);
                    stackInfo.m_Commands.Add(cmd);
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
                StackElementInfo old = m_Stack.Peek();
                bool haveVal = old.m_HaveValue;
                object val = old.m_Value;
                m_Stack.Pop();
                if (m_Stack.Count > 0) {
                    StackElementInfo info = m_Stack.Peek();
                    instance.StackVariables = info.m_StackVariables;
                } else {
                    instance.StackVariables = m_TopStack;
                    m_HaveValue = haveVal;
                    m_Value = val;
                }
            }
        }

        private class StackElementInfo
        {
            internal List<IStoryValue<object>> m_Args = new List<IStoryValue<object>>();
            internal List<IStoryCommand> m_Commands = new List<IStoryCommand>();
            internal bool m_HaveValue;
            internal object m_Value;
            internal Dictionary<string, object> m_StackVariables = new Dictionary<string, object>();

            internal void Reset()
            {
                m_Args.Clear();
                m_Commands.Clear();
                m_HaveValue = false;
                m_Value = null;
                m_StackVariables.Clear();
            }
        }

        private StackElementInfo m_FirstStackInfo = new StackElementInfo();
        private List<IStoryCommand> m_FirstStackCommands = new List<IStoryCommand>();

        private Dictionary<string, object> m_TopStack = null;
        private Stack<StackElementInfo> m_Stack = new Stack<StackElementInfo>();

        private bool m_HaveValue;
        private object m_Value;
        private List<IStoryValue<object>> m_LoadedArgs = null;

        private string m_Name = string.Empty;
        private List<string> m_ArgNames = null;
        private string m_ReturnName = string.Empty;
        private List<IStoryCommand> m_InitialCommands = null;
    }
    internal sealed class CompositeValueFactory : IStoryValueFactory
    {
        public IStoryValue<object> Build()
        {
            return m_Val.Clone();
        }
        internal CompositeValueFactory(CompositeValue val)
        {
            m_Val = val;
        }
        private CompositeValue m_Val;
    }
}
