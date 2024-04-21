using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public sealed class Heap<ElementT>
    {
        public ElementT Root
        {
            get
            {
                ElementT t = default(ElementT);
                if (m_Tree.Count > 0)
                    t = m_Tree[0];
                return t;
            }
        }
        public int Count
        {
            get { return m_Tree.Count; }
        }
        public ElementT this[int index]
        {
            get
            {
                ElementT val = default(ElementT);
                if (index >= 0 && index < m_Tree.Count)
                    val = m_Tree[index];
                return val;
            }
        }
        public ElementT[] ToArray()
        {
            return m_Tree.ToArray();
        }
        public void Clear()
        {
            m_Tree.Clear();
        }
        //Construct a maximum heap with the incoming data (the maximum here is for the semantics of the IComparer interface)
        public void Build(params ElementT[] vals)
        {
            m_Tree.Clear();
            m_Tree.AddRange(vals);
            int currentSize = Count;
            for (int i = currentSize / 2; i >= 1; --i) {
                ElementT val = m_Tree[i - 1];//the root of the sub tree
                //Find where to place i
                int c = 2 * i;//The parent node of c is the target position of i
                while (c <= currentSize) {
                    //m_Tree[c] Should be the max sibling node
                    if (c < currentSize && m_Compare.Compare(m_Tree[c - 1], m_Tree[c]) < 0)
                        ++c;
                    if (m_Compare.Compare(val, m_Tree[c - 1]) >= 0)
                        break;
                    m_Tree[c / 2 - 1] = m_Tree[c - 1];//Move child node up
                    c *= 2;//Move down one level
                }
                m_Tree[c / 2 - 1] = val;
            }
        }
        //Push an element into the heap
        public void Push(ElementT val)
        {
            m_Tree.Add(default(ElementT));
            int i = Count;
            while (i > 1 && m_Compare.Compare(m_Tree[i / 2 - 1], val) < 0) {
                m_Tree[i - 1] = m_Tree[i / 2 - 1];
                i /= 2;
            }
            m_Tree[i - 1] = val;
        }
        //Pop the top element of the heap
        public ElementT Pop()
        {
            ElementT root = default(ElementT);
            int currentSize = Count;
            if (currentSize > 0) {
                root = m_Tree[0];
                ElementT last = m_Tree[currentSize - 1];//last element
                int i = 1;//The current node of the heap
                int ci = 2;//child node of i
                while (ci <= currentSize) {
                    //m_Tree[ci] should be the max child of i
                    if (ci < currentSize && m_Compare.Compare(m_Tree[ci - 1], m_Tree[ci]) < 0)
                        ++ci;
                    if (m_Compare.Compare(last, m_Tree[ci - 1]) >= 0)
                        break;
                    m_Tree[i - 1] = m_Tree[ci - 1];//Move child node up
                    i = ci;
                    ci *= 2;//Move down one level
                }
                m_Tree[i - 1] = last;
                m_Tree.RemoveAt(currentSize - 1);
            }
            return root;
        }
        //Query the index of specific data in the internal data of the heap for Update
        public int IndexOf(ElementT val)
        {
            return m_Tree.IndexOf(val);
        }
        //Update the data of the specified index
        public void Update(int index, ElementT val)
        {
            int currentSize = Count;
            if (index >= 0 && index < currentSize) {
                //Delete first
                ElementT last = m_Tree[currentSize - 1];//last element
                int i = index + 1;//The current node of the heap to be modified
                int ci = i * 2;//child node of i
                while (ci <= currentSize) {
                    //m_Tree[ci] should be the max child of i
                    if (ci < currentSize && m_Compare.Compare(m_Tree[ci - 1], m_Tree[ci]) < 0)
                        ++ci;
                    if (m_Compare.Compare(last, m_Tree[ci - 1]) >= 0)
                        break;
                    m_Tree[i - 1] = m_Tree[ci - 1];//Move child node up
                    i = ci;
                    ci *= 2;//Move down one level
                }
                m_Tree[i - 1] = last;
                //Add it later
                i = currentSize;
                while (i > 1 && m_Compare.Compare(m_Tree[i / 2 - 1], val) < 0) {
                    m_Tree[i - 1] = m_Tree[i / 2 - 1];
                    i /= 2;
                }
                m_Tree[i - 1] = val;
            }
        }
        //Lock the heap and return the internal data list reference. When Unlocking, the internal data will be reorganized in the heap.
        public List<ElementT> LockData()
        {
            return m_Tree;
        }
        public void SetDataDirty()
        {
            m_IsDataDirty = true;
        }
        //Unlock and reorganize internal data (after locking, external data can be added, deleted, or deleted)
        public void UnlockData()
        {
            if (m_IsDataDirty) {
                m_IsDataDirty = false;

                var arr = m_Tree.ToArray();
                Build(arr);
            }
        }
        public Heap()
        {
            Init(null);
        }
        public Heap(IComparer<ElementT> comparer)
        {
            Init(comparer);
        }
        private void Init(IComparer<ElementT> comparer)
        {
            if (null == comparer) {
                m_Compare = Comparer<ElementT>.Default;
            } else {
                m_Compare = comparer;
            }
        }

        private List<ElementT> m_Tree = new List<ElementT>();
        private IComparer<ElementT> m_Compare = null;
        private bool m_IsDataDirty = false;
    }
    public sealed class DefaultReverseComparer<T> : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            if (x == null) {
                return (y != null) ? 1 : 0;
            }
            if (y == null) {
                return -1;
            }
            if (x is IComparable<T>) {
                return -((IComparable<T>)((object)x)).CompareTo(y);
            }
            if (x is IComparable) {
                return -((IComparable)((object)x)).CompareTo(y);
            }
            throw new ArgumentException("does not implement right interface");
        }
    }
}
