using System;
using System.Collections.Generic;
namespace StorySystem
{
    /// <summary>
    /// 剧情命令接口，剧情脚本的基本单位（有的命令是复合命令，由基本命令构成）。
    /// 命令中使用的值由IStoryValue<T>接口描述，用以支持参数、局部变量与内建函数（返回一个剧情命令用到的值）。
    /// </summary>
    public interface IStoryCommand
    {
        void Init(Dsl.ISyntaxComponent config);//从DSL语言初始化命令实例
        IStoryCommand Clone();//克隆一个新实例，每个命令只从DSL语言初始化一次，之后的实例由克隆产生，提升性能
        IStoryCommand LeadCommand { get; }   //用DSL实现的支持递归的command，自身不知道何时是新调用开始，此时借助一个引导命令来发起新调用。
        void Reset();//复位实例，保证实例状态为初始状态。
        bool Execute(StoryInstance instance, long delta, object iterator, object[] args);//执行命令，包括处理参数、变量及命令逻辑
    }
    public abstract class AbstractStoryCommand : IStoryCommand
    {
        //此命令是否复合命令。
        //注意：复合命令需要自己手动调用Evaluate方法，系统不为复合命令调用此方法
        //(也就是说此种情形Evaluate只是推荐了一个与其它命令相似的方法接口)！
        public bool IsCompositeCommand
        {
            get { return m_IsCompositeCommand; }
            protected set { m_IsCompositeCommand = value; }
        }
        public void Init(Dsl.ISyntaxComponent config)
        {
            Dsl.CallData callData = config as Dsl.CallData;
            if (null != callData) {
                Load(callData);
            } else {
                Dsl.FunctionData funcData = config as Dsl.FunctionData;
                if (null != funcData) {
                    Load(funcData);
                } else {
                    Dsl.StatementData statementData = config as Dsl.StatementData;
                    if (null != statementData) {
                        //是否支持语句类型的命令语法？
                        Load(statementData);
                    } else {
                        //error
                    }
                }
            }
        }
        public void Reset()
        {
            if (!IsCompositeCommand) {
                m_LastExecResult = false;
            }
            ResetState();
        }
        public bool Execute(StoryInstance instance, long delta, object iterator, object[] args)
        {
            if (IsCompositeCommand) {
                try {
                    return ExecCommand(instance, delta, iterator, args);
                } catch(Exception ex) {
                    GameFramework.LogSystem.Error("AbstractStoryCommand Composite Command ExecCommand Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    return false;
                }
            } else {
                if (!m_LastExecResult) {
                    //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
                    try {
                        Evaluate(instance, iterator, args);
                    } catch (Exception ex) {
                        GameFramework.LogSystem.Error("AbstractStoryCommand Evaluate Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                        return false;
                    }
                }
                try {
                    m_LastExecResult = ExecCommand(instance, delta);
                } catch (Exception ex) {
                    GameFramework.LogSystem.Error("AbstractStoryCommand ExecCommand Exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    m_LastExecResult = false;
                }
                return m_LastExecResult;
            }
        }
        public abstract IStoryCommand Clone();
        public virtual IStoryCommand LeadCommand
        {
            get { return null; }
        }
        protected virtual void ResetState() { }
        protected virtual void Evaluate(StoryInstance instance, object iterator, object[] args) { }
        protected virtual bool ExecCommand(StoryInstance instance, long delta)
        {
            return false;
        }
        protected virtual bool ExecCommand(StoryInstance instance, long delta, object iterator, object[] args)
        {
            return false;
        }
        protected virtual void Load(Dsl.CallData callData) { }
        protected virtual void Load(Dsl.FunctionData funcData) { }
        protected virtual void Load(Dsl.StatementData statementData) { }

        protected bool m_LastExecResult = false;
        protected bool m_IsCompositeCommand = false;
    }
}
