using System;
using System.Collections.Generic;

namespace ScriptableFramework
{
  public class TimeoutLogicInfo
  {
    public long m_Timeout = 0;
    public long m_CurTime = 0;
    public bool m_IsTriggered = true;
  }
  public class SandClockLogicInfo
  {
    public int m_LastHour = -1;
    public int m_LastMinute = -1;
  }
}
