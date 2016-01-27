using System;
using System.Collections.Generic;

namespace GameFramework
{
  public sealed class DataListMgr<TData> where TData : IDataRecord, new()
  {
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
          string info = string.Format("DataTableMgr.CollectDataFromBinary collectData Row:{0} failed!", index);
          LogSystem.Assert(ret, info);
          if (ret) {
            m_DataContainer.Add(data);
          } else {
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
      foreach (TData data in m_DataContainer) {
        data.WriteToBinary(table);
      }
      table.Save(file);
    }

    public int GetDataCount()
    {
      return m_DataContainer.Count;
    }

    public List<TData> GetData()
    {
      return m_DataContainer;
    }

    public void Clear()
    {
      m_DataContainer.Clear();
    }

    private List<TData> m_DataContainer = new List<TData>();
  }
}
