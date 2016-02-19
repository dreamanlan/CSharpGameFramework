using System;
using System.Collections.Generic;
using GameFramework;

namespace GameFramework
{
  internal class GlobalSaveCounter
  {
    internal int NextSaveCount
    {
      get { return m_NextSaveCount; }      
    }
    internal int CurrentSaveCount
    {
      get { return m_CurrentSaveCount; }
      set { m_CurrentSaveCount = value; }
    }
    internal int SaveInterval
    {
      get { return m_SaveInterval; }
      set { m_SaveInterval = value; }
    }
    internal long LastSaveTime 
    {
      get { return m_LastSaveTime; }
      set { m_LastSaveTime = value; }
    }

    internal void IncreaseNextSaveCount()
    {
      if (m_NextSaveCount != DataCacheThread.UltimateSaveCount) {
        m_NextSaveCount++;
      }
    }
    internal void SetUltimateNextSaveCount()
    {
      m_NextSaveCount = DataCacheThread.UltimateSaveCount;        
    }
    internal bool IsTimeToSave(long curTime)
    {
      if (curTime - m_LastSaveTime > m_SaveInterval && m_NextSaveCount != DataCacheThread.UltimateSaveCount) {
        m_LastSaveTime = curTime;
        return true;
      } else {
        return false;
      }
    }

    private int m_NextSaveCount = 1;      //下一个存储计数            
    private int m_CurrentSaveCount = 0;   //当前存储计数
    private int m_SaveInterval = 600000;  //存储时间间隔
    private long m_LastSaveTime = 0;      //上次存储时刻
  }
}
