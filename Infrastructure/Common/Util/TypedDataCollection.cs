using System;
using System.Collections;
using System.Collections.Generic;

namespace ScriptableFramework
{
    /// <remarks>
    /// This method is mainly used in situations where the data level is difficult to abstract.
    /// If the data definition can be abstracted from a business perspective, try not to use
    /// this dynamic data aggregation method and use it as much as possible.
    /// Static type association.
    /// This class is only used to manage several different types of data. For example, in the
    /// previous situation where union was used to manage different data in C++, usage restrictions:
    /// 1. There can only be one instance of each type of data in this collection.
    /// 2. Due to the use of runtime type information and non-strongly typed collections, the operating efficiency is low.
    /// </remarks>
    public sealed class TypedDataCollection
    {
        public void GetOrNewData<T>(out T t) where T : new()
        {
            t = GetData<T>();
            if (null == t) {
                t = new T();
                AddData(t);
            }
        }
        public void GetOrNewData(Type type, out object t)
        {
            t = GetData(type);
            if (null == t) {
                t = type.TypeInitializer.Invoke(null);
                AddData(t);
            }
        }
        public void AddData<T>(T data)
        {
            Type t = typeof(T);
            if (null != data) {
                if (m_Datas.Contains(t)) {
                    m_Datas[t] = data;
                } else {
                    m_Datas.Add(t, data);
                }
            }
        }
        public void AddData(Type t, object data)
        {
            AddData(data);
        }
        public void AddData(object data)
        {
            if (null != data) {
                Type t = data.GetType();
                if (m_Datas.Contains(t)) {
                    m_Datas[t] = data;
                } else {
                    m_Datas.Add(t, data);
                }
            }
        }
        public void RemoveData<T>(T t)
        {
            RemoveData<T>();
        }
        public void RemoveData<T>()
        {
            Type t = typeof(T);
            if (m_Datas.Contains(t)) {
                m_Datas.Remove(t);
            }
        }
        public void RemoveData(Type t, string key)
        {
            RemoveData(t);
        }
        public void RemoveData(Type t)
        {
            if (m_Datas.Contains(t)) {
                m_Datas.Remove(t);
            }
        }
        public T GetData<T>()
        {
            T ret = default(T);
            Type t = typeof(T);
            if (m_Datas.Contains(t)) {
                ret = (T)m_Datas[t];
            }
            return ret;
        }
        public object GetData(Type t)
        {
            object ret = null;
            if (m_Datas.Contains(t)) {
                ret = m_Datas[t];
            }
            return ret;
        }
        public void Clear()
        {
            m_Datas.Clear();
        }
        public void Visit(MyAction<object, object> visitor)
        {
            foreach (DictionaryEntry dict in m_Datas) {
                visitor(dict.Key, dict.Value);
            }
        }
        public void Visit(MyAction<object, object> visitor, object lockObj)
        {
            DictionaryEntry[] dicts;
            lock (lockObj) {
                dicts = new DictionaryEntry[m_Datas.Count];
                m_Datas.CopyTo(dicts, 0);
            }
            foreach (DictionaryEntry dict in dicts) {
                visitor(dict.Key, dict.Value);
            }
        }

        private Hashtable m_Datas = new Hashtable();
    }
    public sealed class CustomDataCollection
    {
        public void GetOrNewData<T>(out T t) where T : new()
        {
            t = GetData<T>();
            if (null == t) {
                t = new T();
                AddData(t);
            }
        }
        public void GetOrNewData(Type type, out object t)
        {
            t = GetData(type);
            if (null == t) {
                t = type.TypeInitializer.Invoke(null);
                AddData(t);
            }
        }
        public void AddData<T>(T data)
        {
            AddData(typeof(T), data);
        }
        public void AddData(Type t, object data)
        {
            AddData(t.FullName, data);
        }
        public void RemoveData<T>(T t)
        {
            RemoveData<T>();
        }
        public void RemoveData<T>()
        {
            RemoveData(typeof(T));
        }
        public void RemoveData(Type t, string key)
        {
            RemoveData(t);
        }
        public void RemoveData(Type t)
        {
            RemoveData(t.FullName);
        }
        public T GetData<T>()
        {
            object o = GetData(typeof(T));
            if(null==o)
                return default(T);
            else
                return (T)o;
        }
        public object GetData(Type t)
        {
            return GetData(t.FullName);
        }
        public void AddData(string key, object data)
        {
            if (m_Datas.ContainsKey(key)) {
                m_Datas[key] = data;
            } else {
                m_Datas.Add(key, data);
            }
        }
        public void RemoveData(string key)
        {
            m_Datas.Remove(key);
        }
        public object GetData(string key)
        {
            object data;
            m_Datas.TryGetValue(key, out data);
            return data;
        }
        public void Clear()
        {
            m_Datas.Clear();
        }
        public void Visit(MyAction<string, object> visitor)
        {
            foreach (var pair in m_Datas) {
                visitor(pair.Key, pair.Value);
            }
        }
        public void Visit(MyAction<string, object> visitor, object lockObj)
        {
            List<KeyValuePair<string, object>> dicts;
            lock (lockObj) {
                dicts = new List<KeyValuePair<string, object>>();
                foreach (var pair in m_Datas) {
                    dicts.Add(new KeyValuePair<string, object>(pair.Key, pair.Value));
                }
            }
            foreach (var pair in dicts) {
                visitor(pair.Key, pair.Value);
            }
        }

        private Dictionary<string, object> m_Datas = new Dictionary<string, object>();
    }
}
