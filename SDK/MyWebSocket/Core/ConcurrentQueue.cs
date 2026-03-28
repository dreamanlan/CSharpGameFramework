using System;
using System.Net;
using System.Collections.Generic;

namespace System.Collections.Concurrent
{
  public class SimpleConcurrentQueue<T>
  {
    private Queue<T> m_Queue;

    private object m_SyncRoot = new object();

    public SimpleConcurrentQueue()
    {
      m_Queue = new Queue<T>();
    }

    public SimpleConcurrentQueue(int capacity)
    {
      m_Queue = new Queue<T>(capacity);
    }

    public SimpleConcurrentQueue(IEnumerable<T> collection)
    {
      m_Queue = new Queue<T>(collection);
    }

    public int Count
    {
      get
      {
        return m_Queue.Count;
      }
    }

    public void Clear()
    {
      lock (m_SyncRoot) {
        m_Queue.Clear();
      }
    }

    public void Enqueue(T item)
    {
      lock (m_SyncRoot) {
        m_Queue.Enqueue(item);
      }
    }

    public bool TryDequeue(out T item)
    {
      lock (m_SyncRoot) {
        if (m_Queue.Count <= 0) {
          item = default(T);
          return false;
        }

        item = m_Queue.Dequeue();
        return true;
      }
    }
  }
}
