using System;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript
{
    /// <summary>
    /// A simple function value base class that simplifies the number of lines of code needed to implement IStoryValue
    /// (current value classes only support the FunctionData style)
    /// </summary>
    public abstract class SimpleStoryFunctionBase<SubClassType, ValueParamType> : IStoryFunction
        where SubClassType : SimpleStoryFunctionBase<SubClassType, ValueParamType>, new()
        where ValueParamType : IStoryValueParam, new()
    {
        public void InitFromDsl(Dsl.ISyntaxComponent param)
        {
            m_Params.InitFromDsl(param, 0, false);
        }
        public IStoryFunction Clone()
        {
            SubClassType val = new SubClassType();
            val.m_Params = m_Params.Clone();
            val.m_Result = m_Result.Clone();
            val.CopyFields((SubClassType)this);
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
        protected virtual void CopyFields(SubClassType other) { }
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
    /// A simple command base class that simplifies the number of lines of code needed
    /// to implement IStoryCommand (usually such a command is a FunctionData style command)
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
            if (ScriptableFramework.GlobalVariables.Instance.IsDevice) {
                //Configuration information is no longer retained on the device
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
            cmd.CopyFields((SubClassType)this);
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
                //When executing repeatedly, the variable value does not need to be updated every tick.
                //Each command is executed and the variable value is only read once.
                try {
                    m_Params.Evaluate(instance, handler, iterator, args);
                }
                catch (Exception ex) {
                    LogSystem.Error("SimpleStoryCommand Evaluate Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    return false;
                }
            }
            try {
                if (instance.IsDebug && ExecDebugger(instance, handler, delta, iterator, args))
                    return true;
                m_LastExecResult = ExecCommand(instance, (ValueParamType)m_Params, delta);
            }
            catch (Exception ex) {
                LogSystem.Error("SimpleStoryCommand ExecCommand Exception:{0}\n{1}", ex.Message, ex.StackTrace);
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
        protected virtual void CopyFields(SubClassType other) { }
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
