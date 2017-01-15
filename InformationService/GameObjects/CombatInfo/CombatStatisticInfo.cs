using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
  public class CombatStatisticInfo
  {
    public int DeadCount
    {
      get { return m_DeadCount; }
      set { m_DeadCount = value; m_DataChanged = true; }
    }
    public int KillHeroCount
    {
      get { return m_KillHeroCount; }
      set { m_KillHeroCount = value; m_DataChanged = true; }
    }
    public int KillNpcCount
    {
      get { return m_KillNpcCount; }
      set { m_KillNpcCount = value; m_DataChanged = true; }
    }
    public int ContinueKillCount
    {
      get { return m_ContinueKillCount; }
      set { m_ContinueKillCount = value; m_DataChanged = true; }
    }
    public int ContinueDeadCount
    {
      get { return m_ContinueDeadCount; }
      set { m_ContinueDeadCount = value; m_DataChanged = true; }
    }
    public int MultiKillCount
    {
      get { return m_MultiKillCount; }
      set { m_MultiKillCount = value; }
    }
    public int MaxContinueKillCount
    {
      get { return m_MaxContinueKillCount; }
      set { m_MaxContinueKillCount = value; }
    }
    public int MaxMultiKillCount
    {
      get { return m_MaxMultiKillCount; }
      set { m_MaxMultiKillCount = value; }
    }
    public int KillTowerCount
    {
      get { return m_KillTowerCount; }
      set { m_KillTowerCount = value; }
    }
    public int TotalDamageFromMyself
    {
      get { return m_TotalDamageFromMyself; }
      set { m_TotalDamageFromMyself = value; m_DataChanged = true; }
    }
    public int TotalDamageToMyself
    {
      get { return m_TotalDamageToMyself; }
      set { m_TotalDamageToMyself = value; m_DataChanged = true; }
    }
    public void AddDeadCount(int count)
    {
      m_DeadCount += count;
      m_DataChanged = true;
    }
    public void AddKillHeroCount(int count)
    {
      m_KillHeroCount += count;
      m_DataChanged = true;
    }
    public void AddKillNpcCount(int count)
    {
      m_KillNpcCount += count;
      m_DataChanged = true;
    }
    public void ClearContinueKillCount()
    {
      m_ContinueKillCount = 0;
      m_DataChanged = true;
    }
    public void AddContinueKillCount(int count)
    {
      m_ContinueKillCount += count;
      if (m_ContinueKillCount > m_MaxContinueKillCount)
        m_MaxContinueKillCount = m_ContinueKillCount;
      m_DataChanged = true;
    }
    public void ClearContinueDeadCount()
    {
      m_ContinueKillCount = 0;
      m_DataChanged = true;
    }
    public void AddContinueDeadCount(int count)
    {
      m_ContinueDeadCount += count;
      m_DataChanged = true;
    }
    public void ClearMultiKillCount()
    {
      m_MultiKillCount = 0;
      m_DataChanged = true;
    }
    public void AddMultiKillCount(int count)
    {
      m_MultiKillCount += count;
      if (m_MultiKillCount > m_MaxMultiKillCount)
        MaxMultiKillCount = m_MultiKillCount;
      m_DataChanged = true;
    }
    public void AddKillTowerCount(int count)
    {
      m_KillTowerCount += count;
      m_DataChanged = true;
    }
    public void AddTotalDamageFromMyself(int val)
    {
      m_TotalDamageFromMyself += val;
      m_DataChanged = true;
    }
    public void AddTotalDamageToMyself(int val)
    {
      m_TotalDamageToMyself += val;
      m_DataChanged = true;
    }

    public void Reset()
    {
      m_DeadCount = 0;
      m_KillHeroCount = 0;
      m_KillNpcCount = 0;
      m_ContinueKillCount = 0;
      m_ContinueDeadCount = 0;
      m_MultiKillCount = 0;
      m_MaxContinueKillCount = 0;
      m_MaxMultiKillCount = 0;
      m_KillTowerCount = 0;
      m_TotalDamageFromMyself = 0;
      m_TotalDamageToMyself = 0;
      m_DataChanged = false;
    }
    public bool DataChanged
    {
      get { return m_DataChanged; }
      set { m_DataChanged = value; }
    }

    private int m_DeadCount = 0;
    private int m_KillHeroCount = 0;
    private int m_KillNpcCount = 0;
    private int m_ContinueKillCount = 0;
    private int m_ContinueDeadCount = 0;
    private int m_MultiKillCount = 0;
    private int m_MaxContinueKillCount = 0;
    private int m_MaxMultiKillCount = 0;
    private int m_KillTowerCount = 0;
    private int m_TotalDamageFromMyself = 0;
    private int m_TotalDamageToMyself = 0;

    private bool m_DataChanged = false;
  }
}
