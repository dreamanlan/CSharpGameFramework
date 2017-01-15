using System;
using System.Collections.Generic;
namespace StorySystem
{
    /// <summary>
    /// 简单的函数值基类，简化实现IStoryValue需要写的代码行数(当前值类只支持CallData样式)
    /// </summary>
    public abstract class SimpleStoryValueBase<SubClassType, ValueParamType> : IStoryValue<object>
        where SubClassType : SimpleStoryValueBase<SubClassType, ValueParamType>, new()
        where ValueParamType : IStoryValueParam, new()
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            m_Params.InitFromDsl(param, 0);
        }
        public IStoryValue<object> Clone()
        {
            SubClassType val = new SubClassType();
            val.m_Params = m_Params.Clone();
            val.m_Result = m_Result.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            m_Result.HaveValue = false;
            {
                m_Params.Evaluate(instance, iterator, args);
            }
        
            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get
            {
                return m_Result.HaveValue;
            }
        }
        public object Value
        {
            get
            {
                return m_Result.Value;
            }
        }
        protected abstract void UpdateValue(StoryInstance instance, ValueParamType _params, StoryValueResult result);
        private void TryUpdateValue(StoryInstance instance)
        {
            if (m_Params.HaveValue) {
                UpdateValue(instance, (ValueParamType)m_Params, m_Result);
            }
        }
        private IStoryValueParam m_Params = new ValueParamType();
        private StoryValueResult m_Result = new StoryValueResult();
    }
    /// <summary>
    /// 简单的命令基类，简化实现IStoryCommand需要写的代码行数（通常这样的命令是一个CallData样式的命令）
    /// </summary>
    public abstract class SimpleStoryCommandBase<SubClassType, ValueParamType> : IStoryCommand
        where SubClassType : SimpleStoryCommandBase<SubClassType, ValueParamType>, new()
        where ValueParamType : IStoryValueParam, new()
    {
        public void Init(Dsl.ISyntaxComponent config)
        {
            m_Params.InitFromDsl(config, 0);
        }
        public IStoryCommand Clone()
        {
            SubClassType cmd = new SubClassType();
            cmd.m_Params = m_Params.Clone();
            return cmd;
        }
        public IStoryCommand LeadCommand
        {
            get { return null; }
        }
        public void Reset()
        {
            m_LastExecResult = false;
            ResetState();
        }
        public bool Execute(StoryInstance instance, long delta, object iterator, object[] args)
        {
            if (!m_LastExecResult) {
                //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
                m_Params.Evaluate(instance, iterator, args);
            }
            m_LastExecResult = ExecCommand(instance, (ValueParamType)m_Params, delta);
            return m_LastExecResult;
        }
        public void Analyze(StoryInstance instance)
        {
            SemanticAnalyze(instance);
        }
        protected virtual void ResetState() { }
        protected virtual bool ExecCommand(StoryInstance instance, ValueParamType _params, long delta)
        {
            return false;
        }
        protected virtual void SemanticAnalyze(StoryInstance instance) { }
        private bool m_LastExecResult = false;
        private IStoryValueParam m_Params = new ValueParamType();
    }
}
