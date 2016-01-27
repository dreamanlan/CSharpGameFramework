using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace GameFramework
{
  public interface IClientConcurrentPoolAllocatedObject<T> where T : IClientConcurrentPoolAllocatedObject<T>, new()
  {
    void InitPool(ClientConcurrentObjectPool<T> pool);
    T Downcast();
  }
  public class ClientConcurrentObjectPool<T> where T : IClientConcurrentPoolAllocatedObject<T>, new()
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
    public void Recycle(IClientConcurrentPoolAllocatedObject<T> t)
    {
      if (null != t) {
        m_UnusedObjects.Enqueue(t.Downcast());
      }
    }
    public void Clear()
    {
      m_UnusedObjects.Clear();
    }
    public int Count
    {
      get
      {
        return m_UnusedObjects.Count;
      }
    }
    private ClientConcurrentQueue<T> m_UnusedObjects = new ClientConcurrentQueue<T>();
  }
}
