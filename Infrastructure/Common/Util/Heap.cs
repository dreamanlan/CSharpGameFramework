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
        //用传入数据构造一个最大堆(这里的最大是对IComparer接口的语义而言)
        public void Build(params ElementT[] vals)
        {
            m_Tree.Clear();
            m_Tree.AddRange(vals);
            int currentSize = Count;
            for (int i = currentSize / 2; i >= 1; --i) {
                ElementT val = m_Tree[i - 1];//子树的根
                //寻找放置i的位置
                int c = 2 * i;//c的父结点是i的目标位置
                while (c <= currentSize) {
                    //m_Tree[c]应是较大的同胞结点
                    if (c < currentSize && m_Compare.Compare(m_Tree[c - 1], m_Tree[c]) < 0)
                        ++c;
                    if (m_Compare.Compare(val, m_Tree[c - 1]) >= 0)
                        break;
                    m_Tree[c / 2 - 1] = m_Tree[c - 1];//将孩子结点上移
                    c *= 2;//下移一层
                }
                m_Tree[c / 2 - 1] = val;
            }
        }
        //往堆里压入一个元素
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
        //弹出堆顶元素
        public ElementT Pop()
        {
            ElementT root = default(ElementT);
            int currentSize = Count;
            if (currentSize > 0) {
                root = m_Tree[0];
                ElementT last = m_Tree[currentSize - 1];//最后一个元素
                int i = 1;//堆的当前结点
                int ci = 2;//i的孩子结点
                while (ci <= currentSize) {
                    //m_Tree[ci]应是i的较大的孩子
                    if (ci < currentSize && m_Compare.Compare(m_Tree[ci - 1], m_Tree[ci]) < 0)
                        ++ci;
                    if (m_Compare.Compare(last, m_Tree[ci - 1]) >= 0)
                        break;
                    m_Tree[i - 1] = m_Tree[ci - 1];//将孩子结点上移
                    i = ci;
                    ci *= 2;//下移一层
                }
                m_Tree[i - 1] = last;
                m_Tree.RemoveAt(currentSize - 1);
            }
            return root;
        }
        //查询特定数据在堆内部数据里的索引，用于Update
        public int IndexOf(ElementT val)
        {
            return m_Tree.IndexOf(val);
        }
        //更新指定索引的数据
        public void Update(int index, ElementT val)
        {
            int currentSize = Count;
            if (index >= 0 && index < currentSize) {
                //先删除
                ElementT last = m_Tree[currentSize - 1];//最后一个元素
                int i = index + 1;//要修改的堆的当前结点
                int ci = i * 2;//i的孩子结点
                while (ci <= currentSize) {
                    //m_Tree[ci]应是i的较大的孩子
                    if (ci < currentSize && m_Compare.Compare(m_Tree[ci - 1], m_Tree[ci]) < 0)
                        ++ci;
                    if (m_Compare.Compare(last, m_Tree[ci - 1]) >= 0)
                        break;
                    m_Tree[i - 1] = m_Tree[ci - 1];//将孩子结点上移
                    i = ci;
                    ci *= 2;//下移一层
                }
                m_Tree[i - 1] = last;
                //再添加
                i = currentSize;
                while (i > 1 && m_Compare.Compare(m_Tree[i / 2 - 1], val) < 0) {
                    m_Tree[i - 1] = m_Tree[i / 2 - 1];
                    i /= 2;
                }
                m_Tree[i - 1] = val;
            }
        }
        //锁定堆，返回内部数据list引用，在Unlock时将对内部数据重新进行堆整理。
        public List<ElementT> LockData()
        {
            return m_Tree;
        }
        public void SetDataDirty()
        {
            m_IsDataDirty = true;
        }
        //解除锁定，对内部数据进行重新整理（锁定后外部可以对内部数据进行增删减操作）
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
