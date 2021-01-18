using System;
using System.Collections.Generic;

namespace GameFramework
{
    public interface IPoolAllocatedObject<T> where T : IPoolAllocatedObject<T>, new()
    {
        void InitPool(ObjectPool<T> pool);
        T Downcast();
    }

	public class ObjectPool<T> where T : IPoolAllocatedObject<T>, new()
	{
		private int initPoolSize;
		public ObjectPool()
		{
			m_UnusedObjects = new Queue<T>();
		}
		public ObjectPool(int initPoolSize)
		{
			m_UnusedObjects = new Queue<T>(initPoolSize);
			Init(initPoolSize);
		}
		public void Init(int initPoolSize)
		{
			this.initPoolSize = initPoolSize;
			for (int i = 0; i < initPoolSize; ++i) {
				T t = new T();
				t.InitPool(this);
				Recycle(t);
			}
		}
		public T Alloc()
		{
            if (m_UnusedObjects.Count > 0) {
                var t = m_UnusedObjects.Dequeue();
                m_HashCodes.Remove(t.GetHashCode());
                return t;
            }
            else {
                T t = new T();
                if (null != t) {
                    t.InitPool(this);
                }
                return t;
            }
		}
		public void Recycle(IPoolAllocatedObject<T> t)
		{
            if (null != t && m_UnusedObjects.Count < m_PoolSize) {
                int hashCode = t.GetHashCode();
                if (!m_HashCodes.Contains(hashCode)) {
                    m_HashCodes.Add(hashCode);
                    m_UnusedObjects.Enqueue(t.Downcast());
                }
            }
        }
        public void Clear()
		{
            m_HashCodes.Clear();
			m_UnusedObjects.Clear();
		}
		public void ReInit()
		{
			while (m_UnusedObjects.Count < initPoolSize)
			{
				T t = new T();
				t.InitPool(this);
				m_UnusedObjects.Enqueue(t);
			}
			while (m_UnusedObjects.Count > initPoolSize)
			{
				m_UnusedObjects.Dequeue();
			}
		}
        public int Count
        {
            get
            {
                return m_UnusedObjects.Count;
            }
        }

        private HashSet<int> m_HashCodes = new HashSet<int>();
        private Queue<T> m_UnusedObjects = null;
        private int m_PoolSize = 4096;
    }

    public interface IPoolAllocatedObjectEx<T> where T : IPoolAllocatedObjectEx<T>
    {
        void InitPool(ObjectPoolEx<T> pool);
        T Downcast();
    }

    public class ObjectPoolEx<T> where T : IPoolAllocatedObjectEx<T>
    {
        public ObjectPoolEx(Func<T> creater, Action<T> destroyer)
        {
            m_UnusedObjects = new Queue<T>();
            m_Creater = creater;
            m_Destroyer = destroyer;
        }
        public ObjectPoolEx(int initPoolSize, Func<T> creater, Action<T> destroyer)
        {
            m_UnusedObjects = new Queue<T>(initPoolSize);
            Init(initPoolSize, creater, destroyer);
        }
        public void Init(int initPoolSize, Func<T> creater, Action<T> destroyer)
        {
            m_Creater = creater;
            m_Destroyer = destroyer;
            for (int i = 0; i < initPoolSize; ++i) {
                T t = creater();
                t.InitPool(this);
                Recycle(t);
            }
        }
        public T Alloc()
        {
            if (m_UnusedObjects.Count > 0) {
                var t = m_UnusedObjects.Dequeue();
                m_HashCodes.Remove(t.GetHashCode());
                return t;
            }
            else {
                T t = m_Creater();
                if (null != t) {
                    t.InitPool(this);
                }
                return t;
            }
        }
        public void Recycle(IPoolAllocatedObjectEx<T> t)
        {
            if (null != t && m_UnusedObjects.Count < m_PoolSize) {
                int hashCode = t.GetHashCode();
                if (!m_HashCodes.Contains(hashCode)) {
                    m_HashCodes.Add(hashCode);
                    m_UnusedObjects.Enqueue(t.Downcast());
                }
            }
        }
        public void Clear()
        {
            if (null != m_Destroyer) {
                foreach (var item in m_UnusedObjects) {
                    m_Destroyer(item);
                }
            }
            m_HashCodes.Clear();
            m_UnusedObjects.Clear();
        }
        public int Count
        {
            get
            {
                return m_UnusedObjects.Count;
            }
        }

        private HashSet<int> m_HashCodes = new HashSet<int>();
        private Queue<T> m_UnusedObjects = null;
        private Func<T> m_Creater = null;
        private Action<T> m_Destroyer = null;
        private int m_PoolSize = 4096;
    }
}
