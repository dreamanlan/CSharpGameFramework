using System;
using System.Collections.Generic;

namespace GameFramework
{
	public class SimpleObjectListPool<T>
	{
		private int listSize;
		public SimpleObjectListPool()
		{
			m_UnusedObjectLists = new Queue<List<T>>();
		}
		public SimpleObjectListPool(int initPoolSize, int listSize)
		{
			this.listSize = listSize;
			m_UnusedObjectLists = new Queue<List<T>>(initPoolSize);
			Init(initPoolSize);
		}
		public void Init(int initPoolSize)
		{
			for (int i = 0; i < initPoolSize; ++i)
			{
				List<T> t = new List<T>(listSize);
				m_UnusedObjectLists.Enqueue(t);
			}
		}
		public List<T> Alloc()
		{
			if (m_UnusedObjectLists.Count > 0)
			{
				List<T> t = m_UnusedObjectLists.Dequeue();
				return t;
			}
			else
			{
				List<T> t = new List<T>(listSize);
				return t;
			}
		}
		public void Recycle(List<T> t)
		{
			if (null != t)
			{
				m_UnusedObjectLists.Enqueue(t);
				t.Clear();
			}
		}
		public void Clear()
		{
			m_UnusedObjectLists.Clear();
		}
		public int Count
		{
			get
			{
				return m_UnusedObjectLists.Count;
			}
		}
		private Queue<List<T>> m_UnusedObjectLists = null;
	}
    public class SimpleIntListPool
    {
        private int listSize;
        public SimpleIntListPool(int initPoolSize, int listSize)
        {
            this.listSize = listSize;
            m_UnusedObjectLists = new Queue<IntList>(initPoolSize);
            for (int i = 0; i < initPoolSize; ++i) {
                IntList t = new IntList(listSize);
                m_UnusedObjectLists.Enqueue(t);
            }
        }
        public IntList Alloc()
        {
            if (m_UnusedObjectLists.Count > 0) {
                IntList t = m_UnusedObjectLists.Dequeue();
                return t;
            }
            else {
                IntList t = new IntList(listSize);
                return t;
            }
        }
        public void Recycle(IntList t)
        {
            if (null != t) {
                m_UnusedObjectLists.Enqueue(t);
                t.Clear();
            }
        }
        private Queue<IntList> m_UnusedObjectLists = null;
    }
    public class SimpleObjectPool<T> where T : new()
    {
        public SimpleObjectPool()
        {
            m_UnusedObjects = new Queue<T>();
        }
        public SimpleObjectPool(int initPoolSize)
        {
            m_UnusedObjects = new Queue<T>(initPoolSize);
            Init(initPoolSize);
        }
        public void Init(int initPoolSize)
        {
            for (int i = 0; i < initPoolSize; ++i) {
                T t = new T();
                m_UnusedObjects.Enqueue(t);
            }
        }
        public T Alloc()
        {
            if (m_UnusedObjects.Count > 0)
                return m_UnusedObjects.Dequeue();
            else {
                T t = new T();
                return t;
            }
        }
        public void Recycle(T t)
        {
            if (null != t) {
                m_UnusedObjects.Enqueue(t);
            }
        }
        public void Clear()
        {
            m_UnusedObjects.Clear();
        }
        public int Count
        {
            get {
                return m_UnusedObjects.Count;
            }
        }
        private Queue<T> m_UnusedObjects = new Queue<T>();
    }
    public class SimpleObjectPoolEx<T>
    {
        public SimpleObjectPoolEx(Func<T> creater, Action<T> destroyer)
        {
            m_UnusedObjects = new Queue<T>();
            m_Creater = creater;
            m_Destroyer = destroyer;
        }
        public SimpleObjectPoolEx(int initPoolSize, Func<T> creater, Action<T> destroyer)
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
                m_UnusedObjects.Enqueue(t);
            }
        }
        public T Alloc()
        {
            if (m_UnusedObjects.Count > 0)
                return m_UnusedObjects.Dequeue();
            else {
                T t = m_Creater();
                return t;
            }
        }
        public void Recycle(T t)
        {
            if (null != t) {
                m_UnusedObjects.Enqueue(t);
            }
        }
        public void Clear()
        {
            if (null != m_Destroyer) {
                foreach (var item in m_UnusedObjects) {
                    m_Destroyer(item);
                }
            }
            m_UnusedObjects.Clear();
        }
        public int Count
        {
            get {
                return m_UnusedObjects.Count;
            }
        }

        private Queue<T> m_UnusedObjects = new Queue<T>();
        private Func<T> m_Creater = null;
        private Action<T> m_Destroyer = null;
    }
}
