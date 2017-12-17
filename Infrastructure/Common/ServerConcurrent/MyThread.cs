using System;
using System.Collections.Generic;
using System.Threading;

namespace GameFramework
{
    public delegate void MyServerThreadEventDelegate();
    public class MyServerThread : IActionQueue
    {
        public MyServerThread()
        {
            InitThread(new ServerAsyncActionProcessor());
        }
        public MyServerThread(int tickSleepTime)
        {
            m_TickSleepTime = tickSleepTime;
            InitThread(new ServerAsyncActionProcessor());
        }
        public MyServerThread(int tickSleepTime, int actionNumPerTick)
        {
            m_TickSleepTime = tickSleepTime;
            m_ActionNumPerTick = actionNumPerTick;
            InitThread(new ServerAsyncActionProcessor());
        }
        public MyServerThread(ServerAsyncActionProcessor asyncActionQueue)
        {
            InitThread(asyncActionQueue);
        }
        public MyServerThread(int tickSleepTime, ServerAsyncActionProcessor asyncActionQueue)
        {
            m_TickSleepTime = tickSleepTime;
            InitThread(asyncActionQueue);
        }
        public MyServerThread(int tickSleepTime, int actionNumPerTick, ServerAsyncActionProcessor asyncActionQueue)
        {
            m_TickSleepTime = tickSleepTime;
            m_ActionNumPerTick = actionNumPerTick;
            InitThread(asyncActionQueue);
        }

        public int TickSleepTime
        {
            get { return m_TickSleepTime; }
            set { m_TickSleepTime = value; }
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
            }
        }

        public int CurActionNum
        {
            get
            {
                return m_ActionQueue.CurActionNum;
            }
        }

        public void DebugPoolCount(MyAction<string> output)
        {
            m_ActionQueue.DebugPoolCount(output);
        }

        public void ClearPool(int clearOnSize)
        {
            m_ActionQueue.ClearPool(clearOnSize);
        }

        public void Start()
        {
            m_IsRun = true;
            m_Thread.Start();
        }

        public void Stop()
        {
            m_IsRun = false;
            m_Thread.Join();
        }

        public bool IsCurrentThread
        {
            get
            {
                return m_Thread == Thread.CurrentThread;
            }
        }

        public Thread Thread
        {
            get
            {
                return m_Thread;
            }
        }

        public void QueueActionWithDelegation(Delegate action, params object[] args)
        {
            m_ActionQueue.QueueActionWithDelegation(action, args);
        }

        public void QueueAction(MyAction action)
        {
            m_ActionQueue.QueueAction(action);
        }

        public void QueueAction<T1>(MyAction<T1> action, T1 t1)
        {
            m_ActionQueue.QueueAction(action, t1);
        }

        public void QueueAction<T1, T2>(MyAction<T1, T2> action, T1 t1, T2 t2)
        {
            m_ActionQueue.QueueAction(action, t1, t2);
        }

        public void QueueAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3);
        }

        public void QueueAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4);
        }

        public void QueueAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6>(MyAction<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7>(MyAction<T1, T2, T3, T4, T5, T6, T7> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
        {
            m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public void QueueFunc(MyFunc action)
        {
            m_ActionQueue.QueueFunc(action);
        }

        public void QueueFunc<R>(MyFunc<R> action)
        {
            m_ActionQueue.QueueFunc(action);
        }

        public void QueueFunc<T1, R>(MyFunc<T1, R> action, T1 t1)
        {
            m_ActionQueue.QueueFunc(action, t1);
        }

        public void QueueFunc<T1, T2, R>(MyFunc<T1, T2, R> action, T1 t1, T2 t2)
        {
            m_ActionQueue.QueueFunc(action, t1, t2);
        }

        public void QueueFunc<T1, T2, T3, R>(MyFunc<T1, T2, T3, R> action, T1 t1, T2 t2, T3 t3)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3);
        }

        public void QueueFunc<T1, T2, T3, T4, R>(MyFunc<T1, T2, T3, T4, R> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, R>(MyFunc<T1, T2, T3, T4, T5, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, R>(MyFunc<T1, T2, T3, T4, T5, T6, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
        {
            m_ActionQueue.QueueFunc(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public MyServerThreadEventDelegate OnStartEvent;
        public MyServerThreadEventDelegate OnTickEvent;
        public MyServerThreadEventDelegate OnQuitEvent;

        protected virtual void OnStart()
        {
        }
        protected virtual void OnTick()
        {
        }
        protected virtual void OnQuit()
        {
        }

        private void InitThread(ServerAsyncActionProcessor asyncActionQueue)
        {
            m_Thread = new Thread(this.Loop);
            m_ActionQueue = asyncActionQueue;
        }

        private void Loop()
        {
            try {
                if (OnStartEvent != null)
                    OnStartEvent();
                else
                    OnStart();
                while (m_IsRun) {
                    try {
                        if (OnTickEvent != null)
                            OnTickEvent();
                        else
                            OnTick();
                    } catch (Exception ex) {
                        LogSystem.Error("MyThread.Tick throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                    m_ActionQueue.HandleActions(m_ActionNumPerTick);
                    Thread.Sleep(m_TickSleepTime);
                }
                if (OnQuitEvent != null)
                    OnQuitEvent();
                else
                    OnQuit();
            } catch (Exception ex) {
                LogSystem.Error("MyThread.Loop throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private Thread m_Thread = null;
        private bool m_IsRun = true;

        private int m_TickSleepTime = 10;
        private int m_ActionNumPerTick = 1024;

        private ServerAsyncActionProcessor m_ActionQueue = null;
    }
}
