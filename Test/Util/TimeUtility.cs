using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DemoCommon
{
  public sealed class TimeUtility
  {
    public static long GetLocalMilliseconds()
    {
      return (GetElapsedTimeUs() - s_StartTimeUs) / 1000;
    }
    public static long GetElapsedTimeUs()
    {
      //return DateTime.Now.Ticks / 10;
      return (long)(Stopwatch.GetTimestamp() / s_TickPerUs);
    }

    static TimeUtility()
    {
      s_StartTimeUs = GetElapsedTimeUs();
    }

    private static long s_StartTimeUs = 0;
    private static double s_TickPerUs = Stopwatch.Frequency / 1000000.0;
  }
}
