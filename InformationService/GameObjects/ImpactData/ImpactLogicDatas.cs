using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // Per-impact runtime state, keyed by state type.
    public sealed class ImpactLogicDatas
    {
        public T GetData<T>() where T : class
        {
            object val;
            m_Datas.TryGetValue(typeof(T), out val);
            return val as T;
        }
        public void AddData<T>(T data) where T : class { m_Datas[typeof(T)] = data; }
        public T GetOrNewData<T>() where T : class, new()
        {
            T data = GetData<T>();
            if (null == data) { data = new T(); AddData(data); }
            return data;
        }
        public void Clear() { m_Datas.Clear(); }
        private Dictionary<Type, object> m_Datas = new Dictionary<Type, object>();
    }
}
