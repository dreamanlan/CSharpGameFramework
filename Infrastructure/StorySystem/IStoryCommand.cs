﻿using System;
using System.Collections.Generic;
namespace StorySystem
{
    /// <summary>
    /// 剧情命令接口，剧情脚本的基本单位（有的命令是复合命令，由基本命令构成）。
    /// 命令中使用的值由IStoryValue<T>接口描述，用以支持参数、局部变量与内建函数（返回一个剧情命令用到的值）。
    /// </summary>
    public interface IStoryCommand
    {
        bool Init(Dsl.ISyntaxComponent config);//从DSL语言初始化命令实例
        string GetId();//获取命令id
        Dsl.FunctionData GetComments();//获取命令注解dsl
        Dsl.ISyntaxComponent GetConfig();//获取命令配置dsl
        void ShareConfig(IStoryCommand cloner);
        IStoryCommand Clone();//克隆一个新实例，每个命令只从DSL语言初始化一次，之后的实例由克隆产生，提升性能
        IStoryCommand PrologueCommand { get; }   //用DSL实现的支持递归的command，因为允许跨Tick运行，无法在Tick内进行出栈入栈的操作，借助入口命令与出口命令来维持栈环境。
        IStoryCommand EpilogueCommand { get; }   //用DSL实现的支持递归的command，因为允许跨Tick运行，无法在Tick内进行出栈入栈的操作，借助入口命令与出口命令来维持栈环境。
        void Reset();//复位实例，保证实例状态为初始状态。
        bool Execute(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args);//执行命令，包括处理参数、变量及命令逻辑
        bool ExecDebugger(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args);
    }
    public abstract class AbstractStoryCommand : IStoryCommand
    {
        //此命令是否复合命令。
        //注意：复合命令需要自己手动调用Evaluate方法，系统不为复合命令调用此方法
        //(也就是说此种情形Evaluate只是推荐了一个与其它命令相似的方法接口)！
        public virtual bool IsCompositeCommand
        {
            get { return false; }
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
        public bool Init(Dsl.ISyntaxComponent config)
        {
            bool ret = true;
            m_Config = config;
            m_Id = config.GetId();
            Dsl.FunctionData funcData = config as Dsl.FunctionData;
            if (null != funcData) {
                Load(funcData);
            }
            else {
                Dsl.StatementData statementData = config as Dsl.StatementData;
                if (null != statementData) {
                    int funcNum = statementData.GetFunctionNum();
                    var lastFunc = statementData.Last.AsFunction;
                    var id = lastFunc.GetId();
                    if (funcNum >= 2 && (id == "comment" || id == "comments")) {
                        m_Comments = lastFunc;
                        statementData.Functions.RemoveAt(funcNum - 1);
                        if (statementData.GetFunctionNum() == 1) {
                            funcData = statementData.GetFunction(0).AsFunction;
                            ret = Load(funcData);
                        }
                        else {
                            ret = Load(statementData);
                        }
                    }
                    else {
                        ret = Load(statementData);
                    }
                }
                else {
                    //keyword
                }
            }
            if (GameFramework.GlobalVariables.Instance.IsDevice) {
                //在设备上不保留配置信息了
                m_Comments = null;
                m_Config = null;
            }
            return ret;
        }
        public void Reset()
        {
            if (!IsCompositeCommand) {
                m_LastExecResult = false;
            }
            ResetState();
        }
        public bool Execute(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            if (IsCompositeCommand) {
                try {
                    return ExecCommand(instance, handler, delta, iterator, args);
                }
                catch (Exception ex) {
                    GameFramework.LogSystem.Error("AbstractStoryCommand Composite Command ExecCommand Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    return false;
                }
            }
            else {
                if (!m_LastExecResult) {
                    //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
                    try {
                        Evaluate(instance, handler, iterator, args);
                    }
                    catch (Exception ex) {
                        GameFramework.LogSystem.Error("AbstractStoryCommand Evaluate Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        return false;
                    }
                }
                try {
                    if (instance.IsDebug && ExecDebugger(instance, handler, delta, iterator, args))
                        return true;
                    m_LastExecResult = ExecCommand(instance, handler, delta);
                }
                catch (Exception ex) {
                    GameFramework.LogSystem.Error("AbstractStoryCommand ExecCommand Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    m_LastExecResult = false;
                }
                return m_LastExecResult;
            }
        }
        public bool ExecDebugger(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            if (null != instance.OnExecDebugger) {
                return instance.OnExecDebugger(instance, handler, this, delta, iterator, args);
            }
            return false;
        }
        public IStoryCommand Clone()
        {
            var cmd = CloneCommand();
            cmd.ShareConfig(this);
            return cmd;
        }
        public virtual IStoryCommand PrologueCommand
        {
            get { return null; }
        }
        public virtual IStoryCommand EpilogueCommand
        {
            get { return null; }
        }
        protected abstract IStoryCommand CloneCommand();
        protected virtual void ResetState() { }
        protected virtual void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args) { }
        protected virtual bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
        {
            return false;
        }
        protected virtual bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            return false;
        }
        protected virtual bool Load(Dsl.FunctionData funcData)
        {
            bool ret = true;
            if (funcData.IsHighOrder) {
                if (funcData.LowerOrderFunction.GetParamNum() > 0 || funcData.HaveStatement() || funcData.HaveExternScript()) {
                    ret = false;
                }
            }
            else if (funcData.HaveStatement() || funcData.HaveExternScript()) {
                ret = false;
            }
            else if (funcData.HaveParam()) {
                ret = true;
            }
            return ret;
        }
        protected virtual bool Load(Dsl.StatementData statementData)
        {
            bool v = true;
            foreach (var f in statementData.Functions) {
                var func = f.AsFunction;
                if (func.IsHighOrder) {
                    if (func.LowerOrderFunction.GetParamNum() > 0 || func.HaveStatement() || func.HaveExternScript()) {
                        v = false;
                    }
                }
                else if (func.HaveStatement() || func.HaveExternScript()) {
                    v = false;
                }
            }
            return v;
        }

        private Dsl.FunctionData m_Comments;
        private Dsl.ISyntaxComponent m_Config;
        private string m_Id;
        private bool m_LastExecResult = false;
    }
}
