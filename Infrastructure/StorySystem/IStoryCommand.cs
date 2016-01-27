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
    void Reset();//复位实例，保证实例状态为初始状态。
    void Prepare(StoryInstance instance, object iterator, object[] args);//准备执行，处理参数与一些上下文相关且在执行过程中不再更新的依赖
    bool Execute(StoryInstance instance, long delta);//执行命令，包括处理变量及命令逻辑
  }
  public abstract class AbstractStoryCommand : IStoryCommand
  {
    //此命令是否复合命令。
    //注意：复合命令需要自己手动调用Evaluate方法，系统不为复合命令调用此方法！
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
      m_LastExecResult = false;
      ResetState();
    }

    public void Prepare(StoryInstance instance, object iterator, object[] args)
    {
      Substitute(iterator, args);
    }
    public bool Execute(StoryInstance instance, long delta)
    {
      if (!IsCompositeCommand && !m_LastExecResult) {
        //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
        Evaluate(instance);
      }
      m_LastExecResult = ExecCommand(instance, delta);
      return m_LastExecResult;
    }
    public abstract IStoryCommand Clone();
    protected virtual void ResetState() { }
    protected virtual void Substitute(object iterator, object[] args) { }
    protected virtual void Evaluate(StoryInstance instance) { }
    protected virtual bool ExecCommand(StoryInstance instance, long delta)
    {
      return false;
    }

    protected virtual void Load(Dsl.CallData callData) { }
    protected virtual void Load(Dsl.FunctionData funcData) { }
    protected virtual void Load(Dsl.StatementData statementData) { }

    private bool m_LastExecResult = false;
    private bool m_IsCompositeCommand = false;
  }
}
