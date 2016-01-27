using System;
using System.Collections.Generic;

namespace StorySystem.CommonCommands
{
  /// <summary>
  /// while($val<10)
  /// {
  ///   createnpc($$);
  ///   wait(100);
  /// };
  /// </summary>
  internal class WhileCommand : AbstractStoryCommand
  {
    public override IStoryCommand Clone()
    {
      WhileCommand retCmd = new WhileCommand();
      retCmd.m_Condition = m_Condition.Clone();
      for (int i = 0; i < m_LoadedCommands.Count; i++)
      {
        retCmd.m_LoadedCommands.Add(m_LoadedCommands[i].Clone());
      }
      retCmd.IsCompositeCommand = true;
      return retCmd;
    }

    protected override void ResetState()
    {
      m_AlreadyExecute = false;
      m_CurCount = 0;
      foreach (IStoryCommand cmd in m_CommandQueue) {
        cmd.Reset();
      }
      m_CommandQueue.Clear();
    }

    protected override void Substitute(object iterator, object[] args)
    {
      m_Arguments = args;
      if (!m_AlreadyExecute) {
        m_Condition.Substitute(iterator, args);
      }
    }

    protected override void Evaluate(StoryInstance instance)
    {
      if (!m_AlreadyExecute) {
        m_Condition.Evaluate(instance);
      }
    }

    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
      bool ret = true;
      while (ret) {
        if (m_CommandQueue.Count == 0 && !m_AlreadyExecute) {
          Evaluate(instance);
          if (m_Condition.Value != 0) {
            Prepare();
            foreach (IStoryCommand cmd in m_CommandQueue) {
              cmd.Prepare(instance, m_CurCount, m_Arguments);
            }
            ++m_CurCount;
            ret = true;
            m_AlreadyExecute = true;
          } else {
            ret = false;
          }
        } else {
          while (m_CommandQueue.Count > 0) {
            IStoryCommand cmd = m_CommandQueue.Peek();
            if (cmd.Execute(instance, delta)) {
              break;
            } else {
              cmd.Reset();
              m_CommandQueue.Dequeue();
            }
          }
          ret = true;
          if (m_CommandQueue.Count > 0) {
            break;
          } else {
            m_AlreadyExecute = false;
          }
        }
      }
      return ret;
    }

    protected override void Load(Dsl.FunctionData functionData)
    {
      Dsl.CallData callData = functionData.Call;
      if (null != callData) {
        if (callData.GetParamNum() > 0) {
          Dsl.ISyntaxComponent param = callData.GetParam(0);
          m_Condition.InitFromDsl(param);
        }
        for (int i = 0; i < functionData.Statements.Count; i++)
        {
          IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(functionData.Statements[i]);
          if (null != cmd)
            m_LoadedCommands.Add(cmd);
        }
      }
    }

    private void Prepare()
    {
      foreach (IStoryCommand cmd in m_CommandQueue) {
        cmd.Reset();
      }
      m_CommandQueue.Clear();
      for (int i = 0; i < m_LoadedCommands.Count; i++)
      {
        m_CommandQueue.Enqueue(m_LoadedCommands[i]);
      }
    }

    private object[] m_Arguments = null;
    private IStoryValue<int> m_Condition = new StoryValue<int>();
    private Queue<IStoryCommand> m_CommandQueue = new Queue<IStoryCommand>();
    private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
    private int m_CurCount = 0;

    private bool m_AlreadyExecute = false;
  }
}
