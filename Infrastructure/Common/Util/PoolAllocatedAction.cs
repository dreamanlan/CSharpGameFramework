using System;
using System.Collections.Generic;

namespace GameFramework
{
  public sealed class PoolAllocatedAction : IPoolAllocatedObject<PoolAllocatedAction>
  {
    public void Init(Delegate action, params object[] args)
    {
      m_Action = action;
      m_Args = args;
    }
    public void Run()
    {
      try {
        m_Action.DynamicInvoke(m_Args);
      } catch (Exception ex) {
        if (null != m_Action.Target)
          LogSystem.Error("PoolAllocatedAction.Run throw exception {0}[{1}] {2}\n{3}", m_Action.Method.ToString(), m_Action.Target.ToString(), ex.Message, ex.StackTrace);
        else
          LogSystem.Error("PoolAllocatedAction.Run throw exception {0} {1}\n{2}", m_Action.Method.ToString(), ex.Message, ex.StackTrace);
      }
      m_Action = null;
      m_Args = null;
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction> m_Pool;

    private Delegate m_Action;
    private object[] m_Args;
    
    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1> : IPoolAllocatedObject<PoolAllocatedAction<T1>>
  {
    public void Init(MyAction<T1> action, T1 t1)
    {
      m_Action = action;
      m_T1 = t1;
    }
    public void Run()
    {
      m_Action(m_T1);
      m_Action = null;
      m_T1 = default(T1);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1>> m_Pool;

    private MyAction<T1> m_Action;
    private T1 m_T1;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2>>
  {
    public void Init(MyAction<T1, T2> action, T1 t1, T2 t2)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2>> m_Pool;

    private MyAction<T1, T2> m_Action;
    private T1 m_T1;
    private T2 m_T2;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3>>
  {
    public void Init(MyAction<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3>> m_Pool;

    private MyAction<T1, T2, T3> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4>>
  {
    public void Init(MyAction<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4>> m_Pool;

    private MyAction<T1, T2, T3, T4> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
      m_T13 = t13;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12, m_T13);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      m_T13 = default(T13);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;
    private T13 m_T13;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
      m_T13 = t13;
      m_T14 = t14;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12, m_T13, m_T14);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      m_T13 = default(T13);
      m_T14 = default(T14);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;
    private T13 m_T13;
    private T14 m_T14;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
      m_T13 = t13;
      m_T14 = t14;
      m_T15 = t15;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12, m_T13, m_T14, m_T15);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      m_T13 = default(T13);
      m_T14 = default(T14);
      m_T15 = default(T15);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;
    private T13 m_T13;
    private T14 m_T14;
    private T15 m_T15;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : IPoolAllocatedObject<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
  {
    public void Init(MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
      m_T13 = t13;
      m_T14 = t14;
      m_T15 = t15;
      m_T16 = t16;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12, m_T13, m_T14, m_T15, m_T16);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      m_T13 = default(T13);
      m_T14 = default(T14);
      m_T15 = default(T15);
      m_T16 = default(T16);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>> m_Pool;

    private MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;
    private T13 m_T13;
    private T14 m_T14;
    private T15 m_T15;
    private T16 m_T16;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }

  public sealed class PoolAllocatedFunc<R> : IPoolAllocatedObject<PoolAllocatedFunc<R>>
  {
    public void Init(MyFunc<R> action)
    {
      m_Action = action;
    }
    public void Run()
    {
      m_Action();
      m_Action = null;
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<R>> m_Pool;

    private MyFunc<R> m_Action;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, R>>
  {
    public void Init(MyFunc<T1, R> action, T1 t1)
    {
      m_Action = action;
      m_T1 = t1;
    }
    public void Run()
    {
      m_Action(m_T1);
      m_Action = null;
      m_T1 = default(T1);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, R>> m_Pool;

    private MyFunc<T1, R> m_Action;
    private T1 m_T1;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, R>>
  {
    public void Init(MyFunc<T1, T2, R> action, T1 t1, T2 t2)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, R>> m_Pool;

    private MyFunc<T1, T2, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, R>>
  {
    public void Init(MyFunc<T1, T2, T3, R> action, T1 t1, T2 t2, T3 t3)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, R>> m_Pool;

    private MyFunc<T1, T2, T3, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, R> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
      m_T13 = t13;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12, m_T13);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      m_T13 = default(T13);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;
    private T13 m_T13;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
      m_T13 = t13;
      m_T14 = t14;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12, m_T13, m_T14);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      m_T13 = default(T13);
      m_T14 = default(T14);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;
    private T13 m_T13;
    private T14 m_T14;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
      m_T13 = t13;
      m_T14 = t14;
      m_T15 = t15;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12, m_T13, m_T14, m_T15);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      m_T13 = default(T13);
      m_T14 = default(T14);
      m_T15 = default(T15);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;
    private T13 m_T13;
    private T14 m_T14;
    private T15 m_T15;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
  public sealed class PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> : IPoolAllocatedObject<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>>
  {
    public void Init(MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> action, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
    {
      m_Action = action;
      m_T1 = t1;
      m_T2 = t2;
      m_T3 = t3;
      m_T4 = t4;
      m_T5 = t5;
      m_T6 = t6;
      m_T7 = t7;
      m_T8 = t8;
      m_T9 = t9;
      m_T10 = t10;
      m_T11 = t11;
      m_T12 = t12;
      m_T13 = t13;
      m_T14 = t14;
      m_T15 = t15;
      m_T16 = t16;
    }
    public void Run()
    {
      m_Action(m_T1, m_T2, m_T3, m_T4, m_T5, m_T6, m_T7, m_T8, m_T9, m_T10, m_T11, m_T12, m_T13, m_T14, m_T15, m_T16);
      m_Action = null;
      m_T1 = default(T1);
      m_T2 = default(T2);
      m_T3 = default(T3);
      m_T4 = default(T4);
      m_T5 = default(T5);
      m_T6 = default(T6);
      m_T7 = default(T7);
      m_T8 = default(T8);
      m_T9 = default(T9);
      m_T10 = default(T10);
      m_T11 = default(T11);
      m_T12 = default(T12);
      m_T13 = default(T13);
      m_T14 = default(T14);
      m_T15 = default(T15);
      m_T16 = default(T16);
      Recycle();
    }

    public void InitPool(ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>> pool)
    {
      m_Pool = pool;
    }
    public PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> Downcast()
    {
      return this;
    }
    private ObjectPool<PoolAllocatedFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>> m_Pool;

    private MyFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> m_Action;
    private T1 m_T1;
    private T2 m_T2;
    private T3 m_T3;
    private T4 m_T4;
    private T5 m_T5;
    private T6 m_T6;
    private T7 m_T7;
    private T8 m_T8;
    private T9 m_T9;
    private T10 m_T10;
    private T11 m_T11;
    private T12 m_T12;
    private T13 m_T13;
    private T14 m_T14;
    private T15 m_T15;
    private T16 m_T16;

    public void SetPoolRecycleLock(object lockObj)
    {
      m_PoolLock = lockObj;
    }
    private void Recycle()
    {
      if (null != m_PoolLock) {
        lock (m_PoolLock) {
          m_Pool.Recycle(this);
        }
      } else {
        m_Pool.Recycle(this);
      }
    }
    private object m_PoolLock;
  }
}
