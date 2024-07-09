using System;
using System.Collections.Generic;

using System.Text;

namespace ScriptableFramework
{
    /// <summary>
    /// We frequently use situations where there are few additions and deletions,
    /// but many objects are queried and traversed based on ID. This class provides
    /// a container implementation with better performance for this application scenario.
    /// </summary>
    /// <typeparam name="KeyT"></typeparam>
    /// <typeparam name="ValueT"></typeparam>
    /// <remarks>
    /// When using it frequently, you need to pay attention to the overhead of the foreach statement.
    /// This class is not expected to be used for foreach statement traversal (it is retained for
    /// compatibility with existing code and will be gradually replaced).
    /// In addition, this class does not provide id traversal, so KeyT should be deduced from ValueT
    /// when used.
    /// </remarks>
    public sealed class LinkedListDictionary<TKey, TValue>
    {
        public bool Contains(TKey id)
        {
            return m_LinkNodeDictionary.ContainsKey(id);
        }
        ///Duplication is not considered here,
        ///it is guaranteed when called from the outside (performance considerations)
        public void AddFirst(TKey id, TValue obj)
        {
            LinkedListNode<TValue> linkNode = m_Objects.AddFirst(obj);
            if (null != linkNode) {
                try {
                    m_LinkNodeDictionary.Add(id, linkNode);
                } catch (Exception ex) {
                    m_Objects.RemoveFirst();
                    LogSystem.Error("LinkedListDictionary.AddFirst throw Exception:{0}\n{1}\n Add id:{2}", ex.Message, ex.StackTrace, id);
                }
            }
        }
        public void AddLast(TKey id, TValue obj)
        {
            LinkedListNode<TValue> linkNode = m_Objects.AddLast(obj);
            if (null != linkNode) {
                try {
                    m_LinkNodeDictionary.Add(id, linkNode);
                } catch (Exception ex) {
                    m_Objects.RemoveLast();
                    LogSystem.Error("LinkedListDictionary.AddLast throw Exception:{0}\n{1}\n Add id:{2}", ex.Message, ex.StackTrace, id);
                }
            }
        }
        public void Remove(TKey id)
        {
            LinkedListNode<TValue> linkNode;
            if (m_LinkNodeDictionary.TryGetValue(id, out linkNode)) {
                m_LinkNodeDictionary.Remove(id);
                try {
                    m_Objects.Remove(linkNode);
                } catch (Exception ex) {
                    LogSystem.Error("EntityInfoDictionary.Remove throw Exception:{0}\n{1}\n Remove id:{2}", ex.Message, ex.StackTrace, id);
                }
            }
        }
        public void Clear()
        {
            m_LinkNodeDictionary.Clear();
            m_Objects.Clear();
        }
        public bool TryGetValue(TKey id, out TValue value)
        {
            LinkedListNode<TValue> linkNode;
            bool ret = m_LinkNodeDictionary.TryGetValue(id, out linkNode);
            if (ret) {
                value = linkNode.Value;
            } else {
                value = default(TValue);
            }
            return ret;
        }
        public void VisitValues(MyAction<TValue> visitor)
        {
            for (LinkedListNode<TValue> linkNode = m_Objects.First; null != linkNode && m_Objects.Count > 0; linkNode = linkNode.Next) {
                visitor(linkNode.Value);
            }
        }
        public void VisitValues(MyFunc<TValue, bool> visitor)
        {
            for (LinkedListNode<TValue> linkNode = m_Objects.First; null != linkNode && m_Objects.Count > 0; linkNode = linkNode.Next) {
                if (!visitor(linkNode.Value))
                    break;
            }
        }
        public TValue FindValue(MyFunc<TValue, bool> visitor)
        {
            for (LinkedListNode<TValue> linkNode = m_Objects.First; null != linkNode && m_Objects.Count > 0; linkNode = linkNode.Next) {
                if (visitor(linkNode.Value)) {
                    return linkNode.Value;
                }
            }
            return default(TValue);
        }
        public int Count
        {
            get
            {
                return m_LinkNodeDictionary.Count;
            }
        }
        public TValue this[TKey id]
        {
            get
            {
                LinkedListNode<TValue> ret = null;
                if (m_LinkNodeDictionary.TryGetValue(id, out ret) == true) {
                    return ret.Value;
                } else {
                    return default(TValue);
                }
            }
            set
            {
                LinkedListNode<TValue> linkNode;
                if (m_LinkNodeDictionary.TryGetValue(id, out linkNode)) {
                    linkNode.Value = value;
                } else {
                    AddLast(id, value);
                }
            }
        }
        public LinkedListNode<TValue> FirstNode
        {
            get
            {
                return m_Objects.First;
            }
        }
        public LinkedListNode<TValue> LastNode
        {
            get
            {
                return m_Objects.Last;
            }
        }
        public void CopyValuesTo(TValue[] array, int index)
        {
            m_Objects.CopyTo(array, index);
        }
        public IEnumerable<TValue> Values
        {
            get
            {
                return m_Objects;
            }
        }

        private MyDictionary<TKey, LinkedListNode<TValue>> m_LinkNodeDictionary = new MyDictionary<TKey, LinkedListNode<TValue>>();
        private LinkedList<TValue> m_Objects = new LinkedList<TValue>();
    }
}
