﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ScriptableFramework
{
  public class ClientConcurrentActionProcessor : IActionQueue
  {
    public int CurActionNum
    {
      get
      {
        return m_Actions.Count;
      }
    }

    public void QueueActionWithDelegation(Delegate action, params object[] args)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, args);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
#if DEBUG
        if (null != action.Target)
          LogSystem.Debug("QueueActionWithDelegation, {0} {1}, action count {2}", action.Method.ToString(), action.Target.ToString(), m_Actions.Count);
        else
          LogSystem.Debug("QueueActionWithDelegation, {0}, action count {1}", action.Method.ToString(), m_Actions.Count);
#endif
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action.DynamicInvoke(args); });
        LogSystem.Warn("QueueActionWithDelegation {0} use lambda expression, maybe out of memory.", action.Method.ToString());
      }
    }

    public void QueueAction(MyAction action)
    {
      m_Actions.Enqueue(action);
    }

    public void QueueAction<T1>(MyAction<T1> action, T1 t1)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1); });
        LogSystem.Warn("QueueAction {0}({1}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1);
      }
    }

    public void QueueAction<T1, T2>(MyAction<T1, T2> action, T1 t1, T2 t2)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2); });
        LogSystem.Warn("QueueAction {0}({1},{2}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2);
      }
    }

    public void QueueAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3);
      }
    }

    public void QueueAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6>(MyAction<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7>(MyAction<T1, T2, T3, T4, T5, T6, T7> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
      }
    }

    public void QueueFunc(MyFunc action)
    {
        bool needUseLambda = true;
        ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc> pool;
        m_ActionPools.GetOrNewData(out pool);
        if (null != pool) {
            ClientConcurrentPoolAllocatedFunc helper = pool.Alloc();
            if (null != helper) {
                helper.Init(action);
                m_Actions.Enqueue(helper.Run);
                needUseLambda = false;
            }
        }
        if (needUseLambda) {
            m_Actions.Enqueue(() => { action(); });
            LogSystem.Warn("QueueAction {0}() use lambda expression, maybe out of memory.", action.Method.ToString());
        }
    }

    public void QueueFunc<R>(MyFunc<R> action)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(); });
        LogSystem.Warn("QueueAction {0}() use lambda expression, maybe out of memory.", action.Method.ToString());
      }
    }

    public void QueueFunc<T1, R>(MyFunc<T1, R> action, T1 t1)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1); });
        LogSystem.Warn("QueueAction {0}({1}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1);
      }
    }

    public void QueueFunc<T1, T2, R>(MyFunc<T1, T2, R> action, T1 t1, T2 t2)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2); });
        LogSystem.Warn("QueueAction {0}({1},{2}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2);
      }
    }

    public void QueueFunc<T1, T2, T3, R>(MyFunc<T1, T2, T3, R> action, T1 t1, T2 t2, T3 t3)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, R>(MyFunc<T1, T2, T3, T4, R> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, R>(MyFunc<T1, T2, T3, T4, T5, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, R>(MyFunc<T1, T2, T3, T4, T5, T6, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
      }
    }

    public void QueueFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
    {
      bool needUseLambda = true;
      ClientConcurrentObjectPool<ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        ClientConcurrentPoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> helper = pool.Alloc();
        if (null != helper) {
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
      }
    }

    public MyAction DequeueAction()
    {
      MyAction action = null;
      m_Actions.TryDequeue(out action);
      return action;
    }

    public void HandleActions(int maxCount)
    {
      try {
        for (int i = 0; i < maxCount; ++i) {
          if (m_Actions.Count > 0) {
            MyAction action = null;
            m_Actions.TryDequeue(out action);
            if (null != action) {
              try {
                action();
              } catch (Exception ex) {
                LogSystem.Error("ClientConcurrentActionProcessor action() throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
              }
            }
          } else {
            break;
          }
        }
      } catch (Exception ex) {
        LogSystem.Error("ClientConcurrentActionProcessor.HandleActions throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
      }
    }

    public void Reset()
    {
      m_Actions.Clear();
      m_ActionPools.Clear();
    }

    public void ClearPool(int clearOnSize)
    {
      m_ActionPools.Visit((object type, object pool) => {
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
      m_ActionPools.Visit((object type, object pool) => {
        Type t = type as Type;
        if (null != t) {
          object ret = t.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, pool, null);
          if (null != ret) {
            output(string.Format("ActionPool {0} buffered {1} objects", pool.GetType().ToString(), (int)ret));
          }
        } else {
          output(string.Format("ActionPool contain a null pool ({0}) ..", pool.GetType().ToString()));
        }
      });
    }

    public ClientConcurrentActionProcessor()
    {
    }

    private ClientConcurrentQueue<MyAction> m_Actions = new ClientConcurrentQueue<MyAction>();
    private ClientConcurrentTypedDataCollection m_ActionPools = new ClientConcurrentTypedDataCollection();
  }
  public class ClientAsyncActionProcessor : ClientConcurrentActionProcessor
  {
  }
  public class ClientDelayActionProcessor : ClientConcurrentActionProcessor
  {
  }
  /*
  public sealed class DelayActionProcessor : IActionQueue
  {
    public int CurActionNum
    {
      get
      {
        return m_Actions.Count;
      }
    }

    public void QueueActionWithDelegation(Delegate action, params object[] args)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, args);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action.DynamicInvoke(args); });
        LogSystem.Warn("QueueActionWithDelegation {0} use lambda expression, maybe out of memory.", action.Method.ToString());
      }
    }

    public void QueueAction(MyAction action)
    {
      m_Actions.Enqueue(action);
    }

    public void QueueAction<T1>(MyAction<T1> action, T1 t1)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1); });
        LogSystem.Warn("QueueAction {0}({1}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1);
      }
    }

    public void QueueAction<T1, T2>(MyAction<T1, T2> action, T1 t1, T2 t2)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2); });
        LogSystem.Warn("QueueAction {0}({1},{2}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2);
      }
    }

    public void QueueAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3);
      }
    }

    public void QueueAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6>(MyAction<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7>(MyAction<T1, T2, T3, T4, T5, T6, T7> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
      }
    }

    public void QueueAction<R>(MyFunc<R> action)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(); });
        LogSystem.Warn("QueueAction {0}() use lambda expression, maybe out of memory.", action.Method.ToString());
      }
    }

    public void QueueAction<T1, R>(MyFunc<T1, R> action, T1 t1)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1); });
        LogSystem.Warn("QueueAction {0}({1}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1);
      }
    }

    public void QueueAction<T1, T2, R>(MyFunc<T1, T2, R> action, T1 t1, T2 t2)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2); });
        LogSystem.Warn("QueueAction {0}({1},{2}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2);
      }
    }

    public void QueueAction<T1, T2, T3, R>(MyFunc<T1, T2, T3, R> action, T1 t1, T2 t2, T3 t3)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3);
      }
    }

    public void QueueAction<T1, T2, T3, T4, R>(MyFunc<T1, T2, T3, T4, R> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, R>(MyFunc<T1, T2, T3, T4, T5, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, R>(MyFunc<T1, T2, T3, T4, T5, T6, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
    {
      bool needUseLambda = true;
      ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>> pool;
      m_ActionPools.GetOrNewData(out pool);
      if (null != pool) {
        PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> helper = pool.Alloc();
        if (null != helper) {
          helper.SetPoolRecycleLock(m_LockForAsyncActionRun);
          helper.Init(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
          m_Actions.Enqueue(helper.Run);
          needUseLambda = false;
        }
      }
      if (needUseLambda) {
        m_Actions.Enqueue(() => { action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16); });
        LogSystem.Warn("QueueAction {0}({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}) use lambda expression, maybe out of memory.", action.Method.ToString(), t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
      }
    }

    public MyAction DequeueAction()
    {
      MyAction action = null;
      if (m_Actions.Count > 0) {
        if (null != m_LockForAsyncActionRun) {
          lock (m_LockForAsyncActionRun) {
            if (m_Actions.Count > 0) {
              action = m_Actions.Dequeue();
            }
          }
        } else {
          action = m_Actions.Dequeue();
        }
      }
      return action;
    }

    public void HandleActions(int maxCount)
    {
      try {
        for (int i = 0; i < maxCount; ++i) {
          if (m_Actions.Count > 0) {
            MyAction action = null;
            if (null != m_LockForAsyncActionRun) {
              lock (m_LockForAsyncActionRun) {
                if (m_Actions.Count > 0) {
                  action = m_Actions.Dequeue();
                }
              }
            } else {
              action = m_Actions.Dequeue();
            }
            if (null != action) {
              try {
                action();
              } catch (Exception ex) {
                LogSystem.Error("DelayActionProcessor action() throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
              }
            }
          } else {
            break;
          }
        }
      } catch (Exception ex) {
        LogSystem.Error("DelayActionProcessor.HandleActions throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
      }
    }

    public void Reset()
    {
      m_Actions.Clear();
      m_ActionPools.Clear();
    }

    public void DebugPoolCount(MyAction<string> output)
    {
      if (null == m_LockForAsyncActionRun) {
        m_ActionPools.Visit((object type, object pool) => {
          Type t = type as Type;
          if (null != t) {
            object ret = t.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, pool, null);
            if (null != ret) {
              output(string.Format("ActionPool {0} buffered {1} objects", pool.GetType().ToString(), (int)ret));
            }
          } else {
            output(string.Format("ActionPool contain a null pool ({0}) ..", pool.GetType().ToString()));
          }
        });
      } else {
        m_ActionPools.Visit((object type, object pool) => {
          Type t = type as Type;
          if (null != t) {
            object ret = t.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, pool, null);
            if (null != ret) {
              output(string.Format("ActionPool {0} buffered {1} objects", pool.GetType().ToString(), (int)ret));
            }
          } else {
            output(string.Format("ActionPool contain a null pool ({0}) ..", pool.GetType().ToString()));
          }
        }, m_LockForAsyncActionRun);
      }
    }

    public DelayActionProcessor()
    {
    }

    internal DelayActionProcessor(object lockObj)
    {
      m_LockForAsyncActionRun = lockObj;
      //ObjectPool<PoolAllocatedAction> pool;
      //m_ActionPools.GetOrNewData(out pool);
      //pool.Init(200);
    }

    private object m_LockForAsyncActionRun = null;
    private Queue<MyAction> m_Actions = new Queue<MyAction>();
    private TypedDataCollection m_ActionPools = new TypedDataCollection();
  }
  */
  /*  
  public sealed class AsyncActionProcessor : IActionQueue
  {
    public int CurActionNum
    {
      get
      {
        return m_ActionQueue.CurActionNum;
      }
    }

    public void QueueActionWithDelegation(Delegate action, params object[] args)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueActionWithDelegation(action, args);
      }
    }

    public void QueueAction(MyAction action)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action);
      }
    }

    public void QueueAction<T1>(MyAction<T1> action, T1 t1)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1);
      }
    }

    public void QueueAction<T1, T2>(MyAction<T1, T2> action, T1 t1, T2 t2)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2);
      }
    }

    public void QueueAction<T1, T2, T3>(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3);
      }
    }

    public void QueueAction<T1, T2, T3, T4>(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5>(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6>(MyAction<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7>(MyAction<T1, T2, T3, T4, T5, T6, T7> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
      }
    }

    public void QueueAction<R>(MyFunc<R> action)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action);
      }
    }

    public void QueueAction<T1, R>(MyFunc<T1, R> action, T1 t1)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1);
      }
    }

    public void QueueAction<T1, T2, R>(MyFunc<T1, T2, R> action, T1 t1, T2 t2)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2);
      }
    }

    public void QueueAction<T1, T2, T3, R>(MyFunc<T1, T2, T3, R> action, T1 t1, T2 t2, T3 t3)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3);
      }
    }

    public void QueueAction<T1, T2, T3, T4, R>(MyFunc<T1, T2, T3, T4, R> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, R>(MyFunc<T1, T2, T3, T4, T5, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, R>(MyFunc<T1, T2, T3, T4, T5, T6, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
      }
    }

    public void QueueAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
    {
      lock (m_Lock) {
        m_ActionQueue.QueueAction(action, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
      }
    }

    public MyAction DequeueAction()
    {
      return m_ActionQueue.DequeueAction();
    }

    public void HandleActions(int maxCount)
    {
      m_ActionQueue.HandleActions(maxCount);
    }

    public void Reset()
    {
      lock (m_Lock) {
        m_ActionQueue.Reset();
      }
    }
    
    public void DebugPoolCount(MyAction<string> output)
    {
      lock (m_Lock) {
        m_ActionQueue.DebugPoolCount(output);
      }
    }

    public AsyncActionProcessor()
    {
      m_ActionQueue = new DelayActionProcessor(m_Lock);
    }

    private object m_Lock = new object();
    private DelayActionProcessor m_ActionQueue = null;
  }
  */
}