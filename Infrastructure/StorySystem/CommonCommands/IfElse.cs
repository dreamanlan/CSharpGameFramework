using System;
using System.Collections.Generic;
namespace StorySystem.CommonCommands
{
    /// <summary>
    /// if(@val>0)
    /// {
    ///   createnpc(123);
    /// };
    /// 
    /// or
    /// 
    /// if(@val>0)
    /// {
    ///   createnpc(123);
    /// }
    /// else
    /// {
    ///   missioncomplete();
    /// };
    /// </summary>
    internal sealed class IfElseCommand : AbstractStoryCommand
    {
        public override IStoryCommand Clone()
        {
            IfElseCommand retCmd = new IfElseCommand();
            retCmd.m_Conditions = new List<IStoryValue<int>>();
            for (int i = 0; i < m_Conditions.Count; ++i) {
                retCmd.m_Conditions.Add(m_Conditions[i].Clone());
            }
            for (int i = 0; i < m_LoadedIfCommands.Count; i++) {
                List<IStoryCommand> cmds = new List<IStoryCommand>();
                for (int j = 0; j < m_LoadedIfCommands[i].Count; ++j) {
                    cmds.Add(m_LoadedIfCommands[i][j].Clone());
                }
                retCmd.m_LoadedIfCommands.Add(cmds);
            }
            for (int i = 0; i < m_LoadedElseCommands.Count; i++) {
                retCmd.m_LoadedElseCommands.Add(m_LoadedElseCommands[i].Clone());
            }
            retCmd.IsCompositeCommand = true;
            return retCmd;
        }
        protected override void ResetState()
        {
            m_AlreadyExecute = false;
            foreach (IStoryCommand cmd in m_IfCommandQueue) {
                cmd.Reset();
            }
            m_IfCommandQueue.Clear();
            foreach (IStoryCommand cmd in m_ElseCommandQueue) {
                cmd.Reset();
            }
            m_ElseCommandQueue.Clear();
        }
        protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
        {
            if (!m_AlreadyExecute) {
                for (int i = 0; i < m_Conditions.Count; ++i) {
                    m_Conditions[i].Evaluate(instance, iterator, args);
                }
            }
        }
        protected override bool ExecCommand(StoryInstance instance, long delta, object iterator, object[] args)
        {
            bool ret = false;
            if (m_IfCommandQueue.Count == 0 && m_ElseCommandQueue.Count == 0 && !m_AlreadyExecute) {
                Evaluate(instance, iterator, args);
                bool isElse = true;
                for (int i = 0; i < m_Conditions.Count; ++i) {
                    if (m_Conditions[i].Value != 0) {
                        PrepareIf(i);
                        isElse = false;
                    }
                }
                if (isElse) {
                    PrepareElse();
                }
                m_AlreadyExecute = true;
            }
            if (m_IfCommandQueue.Count > 0) {
                while (m_IfCommandQueue.Count > 0) {
                    IStoryCommand cmd = m_IfCommandQueue.Peek();
                    if (cmd.Execute(instance, delta, iterator, args)) {
                        ret = true;
                        break;
                    } else {
                        cmd.Reset();
                        m_IfCommandQueue.Dequeue();
                    }
                }
            }
            if (m_ElseCommandQueue.Count > 0) {
                while (m_ElseCommandQueue.Count > 0) {
                    IStoryCommand cmd = m_ElseCommandQueue.Peek();
                    if (cmd.Execute(instance, delta, iterator, args)) {
                        ret = true;
                        break;
                    } else {
                        cmd.Reset();
                        m_ElseCommandQueue.Dequeue();
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
                    StoryValue<int> cond = new StoryValue<int>();
                    cond.InitFromDsl(param);
                    m_Conditions.Add(cond);
                }
                List<IStoryCommand> cmds = new List<IStoryCommand>();
                for (int i = 0; i < functionData.Statements.Count; i++) {
                    IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(functionData.Statements[i]);
                    if (null != cmd)
                        cmds.Add(cmd);
                }
                m_LoadedIfCommands.Add(cmds);
            }
            IsCompositeCommand = true;
        }
        protected override void Load(Dsl.StatementData statementData)
        {
            Load(statementData.First);
            int ct = statementData.Functions.Count;
            for (int stIx = 1; stIx < ct; ++stIx) {
                Dsl.FunctionData functionData = statementData.Functions[stIx];
                if (null != functionData) {
                    string funcId = functionData.GetId();
                    if (funcId == "elseif") {
                        Load(functionData);
                    } else if (funcId == "else") {
                        if (stIx == ct - 1) {
                            for (int i = 0; i < functionData.Statements.Count; i++) {
                                IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(functionData.Statements[i]);
                                if (null != cmd)
                                    m_LoadedElseCommands.Add(cmd);
                            }
                        } else {
#if DEBUG
                            string err = string.Format("[StoryDsl] else must be the last function !!! line:{0}", functionData.GetLine());
                            throw new Exception(err);
#else
              GameFramework.LogSystem.Error("[StoryDsl] else must be the last function !!!");
#endif
                        }
                    }
                }
            }
        }
        private void PrepareIf(int ix)
        {
            Queue<IStoryCommand> queue = m_IfCommandQueue;
            foreach (IStoryCommand cmd in queue) {
                cmd.Reset();
            }
            queue.Clear();
            List<IStoryCommand> cmds = m_LoadedIfCommands[ix];
            for (int i = 0; i < cmds.Count; ++i) {
                IStoryCommand cmd = cmds[i];
                if (null != cmd.LeadCommand)
                    queue.Enqueue(cmd.LeadCommand);
                queue.Enqueue(cmd);
            }
        }
        private void PrepareElse()
        {
            foreach (IStoryCommand cmd in m_ElseCommandQueue) {
                cmd.Reset();
            }
            m_ElseCommandQueue.Clear();
            for (int i = 0; i < m_LoadedElseCommands.Count; ++i) {
                IStoryCommand cmd = m_LoadedElseCommands[i];
                if (null != cmd.LeadCommand)
                    m_ElseCommandQueue.Enqueue(cmd.LeadCommand);
                m_ElseCommandQueue.Enqueue(cmd);
            }
        }

        private List<IStoryValue<int>> m_Conditions = new List<IStoryValue<int>>();
        private Queue<IStoryCommand> m_IfCommandQueue = new Queue<IStoryCommand>();
        private Queue<IStoryCommand> m_ElseCommandQueue = new Queue<IStoryCommand>();
        private List<List<IStoryCommand>> m_LoadedIfCommands = new List<List<IStoryCommand>>();
        private List<IStoryCommand> m_LoadedElseCommands = new List<IStoryCommand>();
        private bool m_AlreadyExecute = false;
    }
}
