using System;
using System.Threading;

namespace GameFramework
{
    public class MyClientTashDispatcher
    {
        public void DispatchActionWithDelegation(Delegate action, params object[] args)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueActionWithDelegation(action, args);
        }

        public void DispatchAction(MyAction action)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action);
        }

        public void DispatchAction<T1>(MyAction<T1> action, T1 t1)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1);
        }

        public void DispatchAction<T1, T2>(MyAction<T1, T2> action, T1 t1, T2 t2)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2);
        }

        public void DispatchAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3);
        }

        public void DispatchAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4);
        }

        public void DispatchAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6>(MyAction<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7>(MyAction<T1, T2, T3, T4, T5, T6, T7> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public void DispatchAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public void DispatchFunc(MyFunc action)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action);
        }

        public void DispatchFunc<R>(MyFunc<R> action)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action);
        }

        public void DispatchFunc<T1, R>(MyFunc<T1, R> action, T1 t1)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1);
        }

        public void DispatchFunc<T1, T2, R>(MyFunc<T1, T2, R> action, T1 t1, T2 t2)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2);
        }

        public void DispatchFunc<T1, T2, T3, R>(MyFunc<T1, T2, T3, R> action, T1 t1, T2 t2, T3 t3)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3);
        }

        public void DispatchFunc<T1, T2, T3, T4, R>(MyFunc<T1, T2, T3, T4, R> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, R>(MyFunc<T1, T2, T3, T4, T5, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, R>(MyFunc<T1, T2, T3, T4, T5, T6, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public void DispatchFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
        {
            IActionQueue actionQueue = GetActionQueue();
            actionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public void DebugPoolCount(MyAction<string> output)
        {
            if (m_IsPassive) {
                m_ActionQueue.DebugPoolCount((string msg) => {
                    output(string.Format("ActionQueue {0}", msg));
                });
            } else {
                for (int i = 0; i < m_ThreadNum; ++i) {
                    MyClientThread t = m_Threads[i];
                    t.DebugPoolCount((string msg) => {
                        output(string.Format("ThreadActionQueue {0} {1}", t.Thread.ManagedThreadId, msg));
                    });
                }
            }
        }

        public void DebugThreadActionCount(MyAction<string> output)
        {
            if (m_IsPassive) {
                output(string.Format("ActionCount {0}", m_ActionQueue.CurActionNum));
            } else {
                for (int i = 0; i < m_ThreadNum; ++i) {
                    MyClientThread t = m_Threads[i];
                    output(string.Format("ThreadActionCount {0} {1}", t.Thread.ManagedThreadId, t.CurActionNum));
                }
            }
        }

        public bool IsPassive
        {
            get { return m_IsPassive; }
        }
        public int TickSleepTime
        {
            get
            {
                return m_TickSleepTime;
            }
            set
            {
                m_TickSleepTime = value;
                for (int i = 0; i < m_ThreadNum; ++i) {
                    MyClientThread thread = m_Threads[i];
                    thread.TickSleepTime = m_TickSleepTime;
                }
            }
        }
        public int ActionNumPerTick
        {
            get
            {
                return m_ActionNumPerTick;
            }
            set
            {
                m_ActionNumPerTick = value;
                for (int i = 0; i < m_ThreadNum; ++i) {
                    MyClientThread thread = m_Threads[i];
                    thread.ActionNumPerTick = m_ActionNumPerTick;
                }
            }
        }

        public MyClientTashDispatcher()
        {
            InitTaskThreads(m_ThreadNum, m_IsPassive, m_TickSleepTime, m_ActionNumPerTick);
        }
        public MyClientTashDispatcher(int threadNum)
        {
            if (threadNum < 1)
                threadNum = 1;
            m_ThreadNum = threadNum;
            InitTaskThreads(m_ThreadNum, m_IsPassive, m_TickSleepTime, m_ActionNumPerTick);
        }
        public MyClientTashDispatcher(int threadNum, bool isPassive)
        {
            if (threadNum < 1)
                threadNum = 1;
            m_ThreadNum = threadNum;
            m_IsPassive = isPassive;
            InitTaskThreads(m_ThreadNum, m_IsPassive, m_TickSleepTime, m_ActionNumPerTick);
        }
        public MyClientTashDispatcher(int threadNum, bool isPassive, int tickSleepTime)
        {
            if (threadNum < 1)
                threadNum = 1;
            m_ThreadNum = threadNum;
            m_IsPassive = isPassive;
            m_TickSleepTime = tickSleepTime;
            InitTaskThreads(m_ThreadNum, m_IsPassive, m_TickSleepTime, m_ActionNumPerTick);
        }
        public MyClientTashDispatcher(int threadNum, bool isPassive, int tickSleepTime, int actionNumPerTick)
        {
            if (threadNum < 1)
                threadNum = 1;
            m_ThreadNum = threadNum;
            m_IsPassive = isPassive;
            m_TickSleepTime = tickSleepTime;
            m_ActionNumPerTick = actionNumPerTick;
            InitTaskThreads(m_ThreadNum, m_IsPassive, m_TickSleepTime, m_ActionNumPerTick);
        }
        public void StopTaskThreads()
        {
            for (int i = 0; i < m_ThreadNum; ++i) {
                m_Threads[i].Stop();
            }
        }
        public void ClearPool(int clearOnSize)
        {
            if (m_IsPassive) {
                m_ActionQueue.ClearPool(clearOnSize);
            } else {
                for (int i = 0; i < m_ThreadNum; ++i) {
                    MyClientThread thread = m_Threads[i];
                    thread.ClearPool(clearOnSize);
                }
            }
        }
        private void InitTaskThreads(int threadNum, bool isPassive, int tickSleepTime, int actionNumPerTick)
        {
            if (isPassive) {
                m_ActionQueue = new ClientAsyncActionProcessor();
            }
            m_Threads = new MyClientThread[threadNum];
            for (int i = 0; i < threadNum; ++i) {
                MyClientThread thread = null;
                if (isPassive) {
                    thread = new MyClientThread(tickSleepTime, actionNumPerTick, m_ActionQueue);//线程主动取策略
                } else {
                    thread = new MyClientThread(tickSleepTime, actionNumPerTick);//dispatcher主动推策略
                }
                m_Threads[i] = thread;
                thread.Start();
            }
        }
        private IActionQueue GetActionQueue()
        {
            //二种策略，dispatch主动推送到处理线程或者线程主动取
            if (m_IsPassive) {
                //主动取策略，所有线程共享一个ActionQueue
                return m_ActionQueue;
            } else {
                //主动推送策略，此情形每个线程各有一个ActionQueue
                int index = Interlocked.Increment(ref m_TurnIndex) % m_ThreadNum;
                return m_Threads[index];
            }
        }

        private ClientAsyncActionProcessor m_ActionQueue = null;
        private MyClientThread[] m_Threads;
        private int m_ThreadNum = c_DefaultThreadNum;
        private int m_TurnIndex = 0;
        private bool m_IsPassive = false;
        private int m_TickSleepTime = 10;
        private int m_ActionNumPerTick = 1024;

        private const int c_DefaultThreadNum = 8;
    }
}

