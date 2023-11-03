using System;
using System.Collections.Generic;
namespace StorySystem
{
    /// <summary>
    /// 简单的函数值基类，简化实现IStoryValue需要写的代码行数(当前值类只支持FunctionData样式)
    /// </summary>
    public abstract class SimpleStoryValueBase<SubClassType, ValueParamType> : IStoryValue
        where SubClassType : SimpleStoryValueBase<SubClassType, ValueParamType>, new()
        where ValueParamType : IStoryValueParam, new()
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            m_Params.InitFromDsl(param, 0, false);
        }
        public IStoryValue Clone()
        {
            SubClassType val = new SubClassType();
            val.m_Params = m_Params.Clone();
            val.m_Result = m_Result.Clone();
            return val;
        }
        public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            m_Result.HaveValue = false;
            {
                m_Params.Evaluate(instance, handler, iterator, args);
            }

            TryUpdateValue(instance);
        }
        public bool HaveValue
        {
            get {
                return m_Result.HaveValue;
            }
        }
        public BoxedValue Value
        {
            get {
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
    /// 简单的命令基类，简化实现IStoryCommand需要写的代码行数（通常这样的命令是一个FunctionData样式的命令）
    /// </summary>
    public abstract class SimpleStoryCommandBase<SubClassType, ValueParamType> : IStoryCommand
        where SubClassType : SimpleStoryCommandBase<SubClassType, ValueParamType>, new()
        where ValueParamType : IStoryValueParam, new()
    {
        public bool Init(Dsl.ISyntaxComponent config)
        {
            m_Comments = m_Params.InitFromDsl(config, 0, true);
            m_Config = config;
            m_Id = config.GetId();
            if (GameFramework.GlobalVariables.Instance.IsDevice) {
                //在设备上不保留配置信息了
                m_Comments = null;
                m_Config = null;
            }
            return config is Dsl.FunctionData || config is Dsl.ValueData;
        }
        public IStoryCommand Clone()
        {
            SubClassType cmd = new SubClassType();
            cmd.m_Params = m_Params.Clone();
            cmd.m_Comments = m_Comments;
            cmd.m_Config = m_Config;
            cmd.m_Id = m_Id;
            return cmd;
        }
        public IStoryCommand PrologueCommand
        {
            get { return null; }
        }
        public IStoryCommand EpilogueCommand
        {
            get { return null; }
        }
        public string GetId()
        {
            return m_Id;
        }
        public Dsl.FunctionData GetComments()
        {
            return m_Comments;
        }
        public Dsl.ISyntaxComponent GetConfig()
        {
            return m_Config;
        }
        public void ShareConfig(IStoryCommand cloner)
        {
            m_Comments = cloner.GetComments();
            m_Config = cloner.GetConfig();
            m_Id = cloner.GetId();
        }
        public void Reset()
        {
            m_LastExecResult = false;
            ResetState();
        }
        public bool Execute(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            if (!m_LastExecResult) {
                //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
                try {
                    m_Params.Evaluate(instance, handler, iterator, args);
                }
                catch (Exception ex) {
                    GameFramework.LogSystem.Error("SimpleStoryCommand Evaluate Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    return false;
                }
            }
            try {
                if (instance.IsDebug && ExecDebugger(instance, handler, delta, iterator, args))
                    return true;
                m_LastExecResult = ExecCommand(instance, (ValueParamType)m_Params, delta);
            }
            catch (Exception ex) {
                GameFramework.LogSystem.Error("SimpleStoryCommand ExecCommand Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                m_LastExecResult = false;
            }
            return m_LastExecResult;
        }
        public bool ExecDebugger(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            if (null != instance.OnExecDebugger) {
                return instance.OnExecDebugger(instance, handler, this, delta, iterator, args);
            }
            return false;
        }
        protected virtual void ResetState() { }
        protected virtual bool ExecCommand(StoryInstance instance, ValueParamType _params, long delta)
        {
            return false;
        }
        private bool m_LastExecResult = false;
        private IStoryValueParam m_Params = new ValueParamType();
        private Dsl.FunctionData m_Comments;
        private Dsl.ISyntaxComponent m_Config;
        private string m_Id;
    }
}
