using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace GameFramework
{
    public interface IMyCommand
    {
        bool Execute();
    }
    public sealed class MyActionCommandHelper : IMyCommand, IClientConcurrentPoolAllocatedObject<MyActionCommandHelper>
    {
        public void Init(MyAction action)
        {
            m_OnCommand = action;
        }
        public bool Execute()
        {
            bool ret = false;
            if (null != m_OnCommand) {
                m_OnCommand();
            }
            m_OnCommand = null;
            if (null != m_Pool) {
                m_Pool.Recycle(this);
            }
            return ret;
        }
        public void InitPool(ClientConcurrentObjectPool<MyActionCommandHelper> pool)
        {
            m_Pool = pool;
        }
        public MyActionCommandHelper Downcast()
        {
            return this;
        }

        private MyAction m_OnCommand = null;
        private ClientConcurrentObjectPool<MyActionCommandHelper> m_Pool = null;
    }
    public sealed class MyFuncCommandHelper : IMyCommand, IClientConcurrentPoolAllocatedObject<MyFuncCommandHelper>
    {
        public void Init(MyFunc action)
        {
            m_OnCommand = action;
        }
        public bool Execute()
        {
            bool ret = false;
            if (null != m_OnCommand) {
                ret = m_OnCommand();
            }
            if (!ret) {
                m_OnCommand = null;
                if (null != m_Pool) {
                    m_Pool.Recycle(this);
                }
            }
            return ret;
        }
        public void InitPool(ClientConcurrentObjectPool<MyFuncCommandHelper> pool)
        {
            m_Pool = pool;
        }
        public MyFuncCommandHelper Downcast()
        {
            return this;
        }

        private MyFunc m_OnCommand = null;
        private ClientConcurrentObjectPool<MyFuncCommandHelper> m_Pool = null;
    }
    public class ClientConcurrentCommandProcessor
    {
        public int CurCommandNum
        {
            get
            {
                return m_Commands.Count;
            }
        }

        public void QueueAction(MyAction cmd)
        {
            bool needNew = true;
            ClientConcurrentObjectPool<MyActionCommandHelper> pool;
            m_CommandPools.GetOrNewData(out pool);
            if (null != pool) {
                MyActionCommandHelper helper = pool.Alloc();
                if (null != helper) {
                    helper.Init(cmd);
                    m_Commands.Enqueue(helper);
                    needNew = false;
                }
            }
            if (needNew) {
                var helper = new MyActionCommandHelper();
                helper.Init(cmd);
                m_Commands.Enqueue(helper);
                LogSystem.Warn("QueueAction {0}() use new expression, maybe out of memory.", cmd.Method.ToString());
            }
        }

        public void QueueFunc(MyFunc cmd)
        {
            bool needNew = true;
            ClientConcurrentObjectPool<MyFuncCommandHelper> pool;
            m_CommandPools.GetOrNewData(out pool);
            if (null != pool) {
                MyFuncCommandHelper helper = pool.Alloc();
                if (null != helper) {
                    helper.Init(cmd);
                    m_Commands.Enqueue(helper);
                    needNew = false;
                }
            }
            if (needNew) {
                var helper = new MyFuncCommandHelper();
                helper.Init(cmd);
                m_Commands.Enqueue(helper);
                LogSystem.Warn("QueueAction {0}() use new expression, maybe out of memory.", cmd.Method.ToString());
            }
        }

        public void QueueCommand(IMyCommand cmd)
        {
            m_Commands.Enqueue(cmd);
        }

        public IMyCommand DequeueCommand()
        {
            IMyCommand cmd = null;
            m_Commands.TryDequeue(out cmd);
            return cmd;
        }

        public void HandleCommands(int maxCount)
        {
            try {
                for (int i = 0; i < maxCount; ++i) {
                    if (m_Commands.Count > 0) {
                        IMyCommand cmd = null;
                        if (m_Commands.TryPeek(out cmd)) {
                            if (null != cmd) {
                                try {
                                    if (!cmd.Execute()) {
                                        m_Commands.TryDequeue(out cmd);
                                    } else {
                                        break;
                                    }
                                } catch (Exception ex) {
                                    m_Commands.TryDequeue(out cmd);
                                    LogSystem.Error("ClientConcurrentCommandProcessor command() throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                                }
                            } else {
                                m_Commands.TryDequeue(out cmd);
                            }
                        }
                    } else {
                        break;
                    }
                }
            } catch (Exception ex) {
                LogSystem.Error("ClientConcurrentCommandProcessor.HandleCommands throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public void Reset()
        {
            m_Commands.Clear();
            m_CommandPools.Clear();
        }

        public void ClearPool(int clearOnSize)
        {
            m_CommandPools.Visit((object type, object pool) => {
                Type t = type as Type;
                if (null != t) {
                    object ret = t.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, pool, null);
                    if (null != ret && ret is int) {
                        int ct = (int)ret;
                        if (ct > clearOnSize) {
                            t.InvokeMember("Clear", System.Reflection.BindingFlags.InvokeMethod, null, pool, null);
                        }
                    }
                }
            });
        }

        public void DebugPoolCount(MyAction<string> output)
        {
            m_CommandPools.Visit((object type, object pool) => {
                Type t = type as Type;
                if (null != t) {
                    object ret = t.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, pool, null);
                    if (null != ret) {
                        output(string.Format("CommandPool {0} buffered {1} objects", pool.GetType().ToString(), (int)ret));
                    }
                } else {
                    output(string.Format("CommandPool contain a null pool ({0}) ..", pool.GetType().ToString()));
                }
            });
        }

        public ClientConcurrentCommandProcessor()
        {
        }

        private ClientConcurrentQueue<IMyCommand> m_Commands = new ClientConcurrentQueue<IMyCommand>();
        private ClientConcurrentTypedDataCollection m_CommandPools = new ClientConcurrentTypedDataCollection();
    }
}
