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
        protected override IStoryCommand CloneCommand()
        {
            IfElseCommand retCmd = new IfElseCommand();
            retCmd.m_LocalInfoIndex = m_LocalInfoIndex;
            retCmd.m_LoadedConditions = new List<IStoryValue<int>>();
            for (int i = 0; i < m_LoadedConditions.Count; ++i) {
                retCmd.m_LoadedConditions.Add(m_LoadedConditions[i].Clone());
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
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
        {
            var localInfos = handler.LocalInfoStack.Peek();
            var conditions = localInfos.GetLocalInfo(m_LocalInfoIndex) as List<IStoryValue<int>>;
            for (int i = 0; i < conditions.Count; ++i) {
                conditions[i].Evaluate(instance, handler, iterator, args);
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta, object iterator, object[] args)
        {
            var runtime = handler.PeekRuntime();
            if (runtime.CompositeReentry)
                return false;
            bool ret = false;
            var localInfos = handler.LocalInfoStack.Peek();
            var conditions = localInfos.GetLocalInfo(m_LocalInfoIndex) as List<IStoryValue<int>>;
            if (null == conditions) {
                conditions = new List<IStoryValue<int>>();
                for (int i = 0; i < m_LoadedConditions.Count; ++i) {
                    conditions.Add(m_LoadedConditions[i].Clone());
                }
                localInfos.SetLocalInfo(m_LocalInfoIndex, conditions);
            }
            Evaluate(instance, handler, iterator, args);
            bool isElse = true;
            for (int i = 0; i < conditions.Count; ++i) {
                if (conditions[i].Value != 0) {
                    PrepareIf(i, handler);
                    runtime = handler.PeekRuntime();
                    runtime.Iterator = iterator;
                    runtime.Arguments = args;
                    isElse = false;
                    break;
                }
            }
            if (isElse) {
                PrepareElse(handler);
                runtime = handler.PeekRuntime();
                runtime.Iterator = iterator;
                runtime.Arguments = args;
            }
            //没有wait之类命令直接执行
            runtime = handler.PeekRuntime();
            runtime.Tick(instance, handler, delta);
            if (runtime.CommandQueue.Count == 0) {
                handler.PopRuntime();
                ret = false;
            } else {
                //遇到wait命令，跳出执行，之后直接在StoryMessageHandler里执行栈顶的命令队列（降低开销）
                ret = true;
            }
            return ret;
        }
        protected override void Load(Dsl.FunctionData functionData)
        {
            if (functionData.IsHighOrder) {
                m_LocalInfoIndex = StoryCommandManager.Instance.AllocLocalInfoIndex();
                Dsl.FunctionData callData = functionData.LowerOrderFunction;
                if (null != callData) {
                    if (callData.GetParamNum() > 0) {
                        Dsl.ISyntaxComponent param = callData.GetParam(0);
                        StoryValue<int> cond = new StoryValue<int>();
                        cond.InitFromDsl(param);
                        m_LoadedConditions.Add(cond);
                    }
                    List<IStoryCommand> cmds = new List<IStoryCommand>();
                    for (int i = 0; i < functionData.GetParamNum(); i++) {
                        IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(functionData.GetParam(i));
                        if (null != cmd)
                            cmds.Add(cmd);
                    }
                    m_LoadedIfCommands.Add(cmds);
                }
                IsCompositeCommand = true;
            }
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
                            for (int i = 0; i < functionData.GetParamNum(); i++) {
                                IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(functionData.GetParam(i));
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
        private void PrepareIf(int ix, StoryMessageHandler handler)
        {
            var runtime = StoryRuntime.New();
            handler.PushRuntime(runtime);
            var queue = handler.PeekRuntime().CommandQueue;
            foreach (IStoryCommand cmd in queue) {
                cmd.Reset();
            }
            queue.Clear();
            List<IStoryCommand> cmds = m_LoadedIfCommands[ix];
            for (int i = 0; i < cmds.Count; ++i) {
                IStoryCommand cmd = cmds[i];
                if (null != cmd.PrologueCommand)
                    queue.Enqueue(cmd.PrologueCommand);
                queue.Enqueue(cmd);
                if (null != cmd.EpilogueCommand)
                    queue.Enqueue(cmd.EpilogueCommand);
            }
        }
        private void PrepareElse(StoryMessageHandler handler)
        {
            var runtime = StoryRuntime.New();
            handler.PushRuntime(runtime);
            var queue = handler.PeekRuntime().CommandQueue;
            foreach (IStoryCommand cmd in queue) {
                cmd.Reset();
            }
            queue.Clear();
            for (int i = 0; i < m_LoadedElseCommands.Count; ++i) {
                IStoryCommand cmd = m_LoadedElseCommands[i];
                if (null != cmd.PrologueCommand)
                    queue.Enqueue(cmd.PrologueCommand);
                queue.Enqueue(cmd);
                if (null != cmd.EpilogueCommand)
                    queue.Enqueue(cmd.EpilogueCommand);
            }
        }

        private int m_LocalInfoIndex;
        private List<IStoryValue<int>> m_LoadedConditions = new List<IStoryValue<int>>();
        private List<List<IStoryCommand>> m_LoadedIfCommands = new List<List<IStoryCommand>>();
        private List<IStoryCommand> m_LoadedElseCommands = new List<IStoryCommand>();
    }
}
