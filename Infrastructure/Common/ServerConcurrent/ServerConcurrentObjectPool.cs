using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ScriptableFramework
{
  public interface IServerConcurrentPoolAllocatedObject<T> where T : IServerConcurrentPoolAllocatedObject<T>, new()
  {
    void InitPool(ServerConcurrentObjectPool<T> pool);
    T Downcast();
  }
  public class ServerConcurrentObjectPool<T> where T : IServerConcurrentPoolAllocatedObject<T>, new()
  {
    public void Init(int initPoolSize)
    {
      for (int i = 0; i < initPoolSize; ++i) {
        T t = new T();
        t.InitPool(this);
        m_UnusedObjects.Enqueue(t);
      }
    }
    public T Alloc()
    {
      T t;
      if (!m_UnusedObjects.TryDequeue(out t)) {
        t = new T();
        if (null != t) {
          t.InitPool(this);
        }
      }
      return t;
    }
    public void Recycle(IServerConcurrentPoolAllocatedObject<T> t)
    {
      if (null != t) {
        m_UnusedObjects.Enqueue(t.Downcast());
      }
    }
    public int Count
    {
      get
      {
        return m_UnusedObjects.Count;
      }
    }
    private ServerConcurrentQueue<T> m_UnusedObjects = new ServerConcurrentQueue<T>();
  }
}
