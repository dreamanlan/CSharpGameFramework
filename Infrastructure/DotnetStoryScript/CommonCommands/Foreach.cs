using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;

namespace DotnetStoryScript.CommonCommands
{
    /// <summary>
    /// foreach(v1,v2,v3)
    /// {
    ///   createnpc($$);
    ///   wait(100);
    /// };
    /// </summary>
    public sealed class ForeachCommand : AbstractStoryCommand
    {
        public override bool IsCompositeCommand { get { return true; } }
        protected override IStoryCommand CloneCommand()
        {
            ForeachCommand retCmd = new ForeachCommand();
            retCmd.m_LocalInfoIndex = m_LocalInfoIndex;
            for (int i = 0; i < m_LoadedIterators.Count; i++) {
                retCmd.m_LoadedIterators.Add(m_LoadedIterators[i].Clone());
            }
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                retCmd.m_LoadedCommands.Add(m_LoadedCommands[i].Clone());
            }
            return retCmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            var localInfos = handler.LocalInfoStack.Peek();
            var localInfo = localInfos.GetLocalInfo(m_LocalInfoIndex) as LocalInfo;
            if (localInfo.Iterators.Count <= 0 && localInfo.List.Count > 0) {
                for (int i = 0; i < localInfo.List.Count; i++) {
                    localInfo.List[i].Evaluate(instance, handler, iterator, args);
                }
                for (int i = 0; i < localInfo.List.Count; i++) {
                    localInfo.Iterators.Enqueue(localInfo.List[i].Value.GetObject());
                }
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            var runtime = handler.PeekRuntime();
            if (runtime.TryBreakLoop()) {
                return false;
            }
            var localInfos = handler.LocalInfoStack.Peek();
            var localInfo = localInfos.GetLocalInfo(m_LocalInfoIndex) as LocalInfo;
            if (null == localInfo) {
                localInfo = new LocalInfo { List = new List<IStoryFunction>() };
                for (int i = 0; i < m_LoadedIterators.Count; ++i) {
                    localInfo.List.Add(m_LoadedIterators[i].Clone());
                }
                localInfos.SetLocalInfo(m_LocalInfoIndex, localInfo);
            }
            if (!handler.PeekRuntime().CompositeReentry) {
                Evaluate(instance, handler, iterator, args);
            }
            bool ret = true;
            while (ret) {
                if (localInfo.Iterators.Count > 0) {
                    Prepare(handler);
                    runtime = handler.PeekRuntime();
                    runtime.Iterator = BoxedValue.FromObject(localInfo.Iterators.Dequeue());
                    runtime.Arguments = args;
                    ret = true;
                    //Execute directly without commands such as wait
                    runtime.Tick(instance, handler, delta);
                    if (runtime.CommandQueue.Count == 0) {
                        handler.PopRuntime(instance);
                        if (runtime.TryBreakLoop()) {
                            ret = false;
                            break;
                        }
                    } else {
                        //When encountering the wait command, jump out of execution, and then directly
                        //execute the command queue on the top of the stack in StoryMessageHandler
                        //(reducing overhead)
                        break;
                    }
                } else {
                    ret = false;
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData functionData)
        {
            if (functionData.IsHighOrder) {
                m_LocalInfoIndex = StoryCommandManager.Instance.AllocLocalInfoIndex();
                Dsl.FunctionData callData = functionData.LowerOrderFunction;
                for (int i = 0; i < callData.GetParamNum(); ++i) {
                    Dsl.ISyntaxComponent param = callData.GetParam(i);
                    StoryFunction val = new StoryFunction();
                    val.InitFromDsl(param);
                    m_LoadedIterators.Add(val);
                }
                for (int i = 0; i < functionData.GetParamNum(); i++) {
                    IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(functionData.GetParam(i));
                    if (null != cmd)
                        m_LoadedCommands.Add(cmd);
                }
            }
            return true;
        }
        private void Prepare(StoryMessageHandler handler)
        {
            var runtime = StoryRuntime.New();
            handler.PushRuntime(runtime);
            var queue = runtime.CommandQueue;
            foreach (IStoryCommand cmd in queue) {
                cmd.Reset();
            }
            queue.Clear();
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                IStoryCommand cmd = m_LoadedCommands[i];
                if (null != cmd.PrologueCommand)
                    queue.Enqueue(cmd.PrologueCommand);
                queue.Enqueue(cmd);
                if (null != cmd.EpilogueCommand)
                    queue.Enqueue(cmd.EpilogueCommand);
            }
        }

        private sealed class LocalInfo
        {
            internal Queue<object> Iterators = new Queue<object>();
            internal List<IStoryFunction> List = null;
        }

        private int m_LocalInfoIndex;
        private List<IStoryFunction> m_LoadedIterators = new List<IStoryFunction>();
        private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
    }
    /// <summary>
    /// looplist(list)
    /// {
    ///   createnpc($$);
    ///   wait(100);
    /// };
    /// </summary>
    public sealed class LoopListCommand : AbstractStoryCommand
    {
        public override bool IsCompositeCommand { get { return true; } }
        protected override IStoryCommand CloneCommand()
        {
            LoopListCommand retCmd = new LoopListCommand();
            retCmd.m_LocalInfoIndex = m_LocalInfoIndex;
            retCmd.m_LoadedList = m_LoadedList.Clone();
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                retCmd.m_LoadedCommands.Add(m_LoadedCommands[i].Clone());
            }
            return retCmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            var localInfos = handler.LocalInfoStack.Peek();
            var localInfo = localInfos.GetLocalInfo(m_LocalInfoIndex) as LocalInfo;
            if (localInfo.Iterators.Count <= 0) {
                localInfo.List.Evaluate(instance, handler, iterator, args);
                foreach (object obj in localInfo.List.Value) {
                    localInfo.Iterators.Enqueue(obj);
                }
            }
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            var runtime = handler.PeekRuntime();
            if (runtime.TryBreakLoop()) {
                return false;
            }
            var localInfos = handler.LocalInfoStack.Peek();
            var localInfo = localInfos.GetLocalInfo(m_LocalInfoIndex) as LocalInfo;
            if (null == localInfo) {
                localInfo = new LocalInfo { List = m_LoadedList.Clone() };
                localInfos.SetLocalInfo(m_LocalInfoIndex, localInfo);
            }
            if (!handler.PeekRuntime().CompositeReentry) {
                Evaluate(instance, handler, iterator, args);
            }
            bool ret = true;
            while (ret) {
                if (localInfo.Iterators.Count > 0) {
                    Prepare(handler);
                    runtime = handler.PeekRuntime();
                    runtime.Iterator = BoxedValue.FromObject(localInfo.Iterators.Dequeue());
                    runtime.Arguments = args;
                    ret = true;
                    //Execute directly without commands such as wait
                    runtime.Tick(instance, handler, delta);
                    if (runtime.CommandQueue.Count == 0) {
                        handler.PopRuntime(instance);
                        if (runtime.TryBreakLoop()) {
                            ret = false;
                            break;
                        }
                    } else {
                        //When encountering the wait command, jump out of execution, and then directly
                        //execute the command queue on the top of the stack in StoryMessageHandler
                        //(reducing overhead)
                        break;
                    }
                } else {
                    ret = false;
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData functionData)
        {
            if (functionData.IsHighOrder) {
                m_LocalInfoIndex = StoryCommandManager.Instance.AllocLocalInfoIndex();
                Dsl.FunctionData callData = functionData.LowerOrderFunction;
                if (callData.GetParamNum() > 0) {
                    m_LoadedList.InitFromDsl(callData.GetParam(0));
                }
                for (int i = 0; i < functionData.GetParamNum(); i++) {
                    IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(functionData.GetParam(i));
                    if (null != cmd)
                        m_LoadedCommands.Add(cmd);
                }
            }
            return true;
        }
        private void Prepare(StoryMessageHandler handler)
        {
            var runtime = StoryRuntime.New();
            handler.PushRuntime(runtime);
            var queue = runtime.CommandQueue;
            foreach (IStoryCommand cmd in queue) {
                cmd.Reset();
            }
            queue.Clear();
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                IStoryCommand cmd = m_LoadedCommands[i];
                if (null != cmd.PrologueCommand)
                    queue.Enqueue(cmd.PrologueCommand);
                queue.Enqueue(cmd);
                if (null != cmd.EpilogueCommand)
                    queue.Enqueue(cmd.EpilogueCommand);
            }
        }

        private sealed class LocalInfo
        {
            internal Queue<object> Iterators = new Queue<object>();
            internal IStoryFunction<IEnumerable> List = null;
        }

        private int m_LocalInfoIndex;
        private IStoryFunction<IEnumerable> m_LoadedList = new StoryFunction<IEnumerable>();
        private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
    }
    /// <summary>
    /// loop(count)
    /// {
    ///   createnpc($$);
    ///   wait(100);
    /// };
    /// </summary>
    public sealed class LoopCommand : AbstractStoryCommand
    {
        public override bool IsCompositeCommand { get { return true; } }
        protected override IStoryCommand CloneCommand()
        {
            LoopCommand retCmd = new LoopCommand();
            retCmd.m_LocalInfoIndex = m_LocalInfoIndex;
            retCmd.m_LoadedCount = m_LoadedCount.Clone();
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                retCmd.m_LoadedCommands.Add(m_LoadedCommands[i].Clone());
            }
            return retCmd;
        }
        protected override void ResetState()
        {
        }
        protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
        {
            var localInfos = handler.LocalInfoStack.Peek();
            var localInfo = localInfos.GetLocalInfo(m_LocalInfoIndex) as LocalInfo;
            localInfo.CurCount = 0;
            localInfo.Count.Evaluate(instance, handler, iterator, args);
        }
        protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
        {
            var runtime = handler.PeekRuntime();
            if (runtime.TryBreakLoop()) {
                return false;
            }
            var localInfos = handler.LocalInfoStack.Peek();
            var localInfo = localInfos.GetLocalInfo(m_LocalInfoIndex) as LocalInfo;
            if (null == localInfo) {
                localInfo = new LocalInfo { Count = m_LoadedCount.Clone(), CurCount = 0 };
                localInfos.SetLocalInfo(m_LocalInfoIndex, localInfo);
            }
            if (!handler.PeekRuntime().CompositeReentry) {
                Evaluate(instance, handler, iterator, args);
            }
            bool ret = true;
            while (ret) {
                if (localInfo.CurCount < localInfo.Count.Value) {
                    Prepare(handler);
                    runtime = handler.PeekRuntime();
                    runtime.Iterator = localInfo.CurCount;
                    runtime.Arguments = args;
                    ++localInfo.CurCount;
                    ret = true;
                    //Execute directly without commands such as wait
                    runtime.Tick(instance, handler, delta);
                    if (runtime.CommandQueue.Count == 0) {
                        handler.PopRuntime(instance);
                        if (runtime.TryBreakLoop()) {
                            ret = false;
                            break;
                        }
                    } else {
                        //When encountering the wait command, jump out of execution, and then directly
                        //execute the command queue on the top of the stack in StoryMessageHandler
                        //(reducing overhead)
                        break;
                    }
                } else {
                    ret = false;
                }
            }
            return ret;
        }
        protected override bool Load(Dsl.FunctionData functionData)
        {
            if (functionData.IsHighOrder) {
                m_LocalInfoIndex = StoryCommandManager.Instance.AllocLocalInfoIndex();
                Dsl.FunctionData callData = functionData.LowerOrderFunction;
                if (callData.GetParamNum() > 0) {
                    Dsl.ISyntaxComponent param = callData.GetParam(0);
                    m_LoadedCount.InitFromDsl(param);
                }
                for (int i = 0; i < functionData.GetParamNum(); i++) {
                    IStoryCommand cmd = StoryCommandManager.Instance.CreateCommand(functionData.GetParam(i));
                    if (null != cmd)
                        m_LoadedCommands.Add(cmd);
                }
            }
            return true;
        }
        private void Prepare(StoryMessageHandler handler)
        {
            var runtime = StoryRuntime.New();
            handler.PushRuntime(runtime);
            var queue = runtime.CommandQueue;
            foreach (IStoryCommand cmd in queue) {
                cmd.Reset();
            }
            queue.Clear();
            for (int i = 0; i < m_LoadedCommands.Count; i++) {
                IStoryCommand cmd = m_LoadedCommands[i];
                if (null != cmd.PrologueCommand)
                    queue.Enqueue(cmd.PrologueCommand);
                queue.Enqueue(cmd);
                if (null != cmd.EpilogueCommand)
                    queue.Enqueue(cmd.EpilogueCommand);
            }
        }

        private sealed class LocalInfo
        {
            internal IStoryFunction<int> Count;
            internal int CurCount;
        }

        private int m_LocalInfoIndex;
        private IStoryFunction<int> m_LoadedCount = new StoryFunction<int>();
        private List<IStoryCommand> m_LoadedCommands = new List<IStoryCommand>();
    }
}
