using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public sealed class GlobalData
    {
        public void Clear()
        {
            lock (m_Lock) {
                foreach (var pair in m_Datas) {
                    pair.Value.Deleted = true;
                }
            }
        }
        public void AddInt(string key, int val)
        {
            lock (m_Lock) {
                TableGlobalDataWrap data;
                if (!m_Datas.TryGetValue(key, out data)) {
                    data = new TableGlobalDataWrap();
                    data.Key = key;
                    m_Datas.Add(key, data);
                }
                data.IntValue = val;
                data.Deleted = false;
            }
        }
        public void RemoveInt(string key)
        {
            lock (m_Lock) {
                TableGlobalDataWrap data;
                if (m_Datas.TryGetValue(key, out data)) {
                    data.IntValue = 0;
                    if (data.IntValue == 0 && Geometry.IsSameFloat(data.FloatValue, 0.0f) && string.IsNullOrEmpty(data.StrValue)) {
                        data.Deleted = true;
                    }
                }
            }
        }
        public int GetInt(string key)
        {
            int ret = 0;
            lock (m_Lock) {
                TableGlobalDataWrap data;
                if (m_Datas.TryGetValue(key, out data)) {
                    ret = data.IntValue;
                }
            }
            return ret;
        }
        public void AddFloat(string key, float val)
        {
            lock (m_Lock) {
                TableGlobalDataWrap data;
                if (!m_Datas.TryGetValue(key, out data)) {
                    data = new TableGlobalDataWrap();
                    data.Key = key;
                    m_Datas.Add(key, data);
                }
                data.FloatValue = val;
                data.Deleted = false;
            }
        }
        public void RemoveFloat(string key)
        {
            lock (m_Lock) {
                TableGlobalDataWrap data;
                if (m_Datas.TryGetValue(key, out data)) {
                    data.FloatValue = 0;
                    if (data.IntValue == 0 && Geometry.IsSameFloat(data.FloatValue, 0.0f) && string.IsNullOrEmpty(data.StrValue)) {
                        data.Deleted = true;
                    }
                }
            }
        }
        public float GetFloat(string key)
        {
            float ret = 0;
            lock (m_Lock) {
                TableGlobalDataWrap data;
                if (m_Datas.TryGetValue(key, out data)) {
                    ret = data.FloatValue;
                }
            }
            return ret;
        }
        public void AddStr(string key, string val)
        {
            TableGlobalDataWrap data;
            lock (m_Lock) {
                if (!m_Datas.TryGetValue(key, out data)) {
                    data = new TableGlobalDataWrap();
                    data.Key = key;
                    m_Datas.Add(key, data);
                }
                data.StrValue = val;
                data.Deleted = false;
            }
        }
        public void RemoveStr(string key)
        {
            TableGlobalDataWrap data;
            lock (m_Lock) {
                if (m_Datas.TryGetValue(key, out data)) {
                    data.StrValue = string.Empty;
                    if (data.IntValue == 0 && Geometry.IsSameFloat(data.FloatValue, 0.0f) && string.IsNullOrEmpty(data.StrValue)) {
                        data.Deleted = true;
                    }
                }
            }
        }
        public string GetStr(string key)
        {
            string ret = string.Empty;
            lock (m_Lock) {
                TableGlobalDataWrap data;
                if (m_Datas.TryGetValue(key, out data)) {
                    ret = data.StrValue;
                }
            }
            return ret;
        }

        private object m_Lock = new object();
        private Dictionary<string, TableGlobalDataWrap> m_Datas = new Dictionary<string, TableGlobalDataWrap>();

        public static GlobalData Instance
        {
            get { return s_Instance; }
        }
        private static GlobalData s_Instance = new GlobalData();
    }
}
