using System;
using System.Collections.Generic;
//using System.Diagnostics;

namespace GameFramework
{
    public class DataDictionaryMgr<TData, KeyType> where TData : IDataRecord<KeyType>, new()
    {
        MyDictionary<KeyType, object> m_DataContainer;

        public DataDictionaryMgr()
        {
            m_DataContainer = new MyDictionary<KeyType, object>();
        }
        public bool LoadFromBinary(string file)
        {
            long t1 = TimeUtility.GetElapsedTimeUs();
            bool result = true;
            BinaryTable table = new BinaryTable();
            table.Load(HomePath.GetAbsolutePath(file));
            long t2 = TimeUtility.GetElapsedTimeUs();

            long t3 = TimeUtility.GetElapsedTimeUs();
            for (int index = 0; index < table.Records.Count; ++index) {
                try {
                    TData data = new TData();
                    bool ret = data.ReadFromBinary(table, index);
                    if (ret && !m_DataContainer.ContainsKey(data.GetId())) {
                        m_DataContainer.Add(data.GetId(), data);
                    } else {
                        string info = string.Format("DataTempalteMgr.CollectDataFromBinary collectData Row:{0} failed in {1}!", index, file);
                        LogSystem.Error(info);
                        Helper.LogCallStack(true);
                        result = false;
                    }
                } catch (System.Exception ex) {
                    LogSystem.Error("CollectData failed. file:{0} rowIndex:{1}\nException:{2}\n{3}", file, index, ex.Message, ex.StackTrace);
                }
            }
            long t4 = TimeUtility.GetElapsedTimeUs();
            LogSystem.Info("binary load {0} parse {1}, file {2}", t2 - t1, t4 - t3, file);
            return result;
        }
        public void SaveToBinary(string file)
        {
            BinaryTable table = new BinaryTable();
            foreach (KeyValuePair<KeyType, object> pair in m_DataContainer) {
                TData data = (TData)pair.Value;
                data.WriteToBinary(table);
            }
            table.Save(file);
        }

        public TData GetDataById(KeyType id)
        {
            object ret;
            if (m_DataContainer.TryGetValue(id, out ret))
                return (TData)ret;
            else
                return default(TData);
        }

        public int GetDataCount()
        {
            return m_DataContainer.Count;
        }

        public MyDictionary<KeyType, object> GetData()
        {
            return m_DataContainer;
        }

        public void Clear()
        {
            m_DataContainer.Clear();
        }
    }
}
