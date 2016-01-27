
using System;
//using System.Diagnostics;

namespace GameFramework
{
  public enum Log_Type
  {
    LT_Debug,
    LT_Info,
    LT_Warn,
    LT_Error,
    LT_Assert,
  }
  public delegate void LogSystemOutputDelegation(Log_Type type,string msg);

  public class LogSystem
  {
    public static LogSystemOutputDelegation OnOutput;
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Debug(string format, params object[] args)
    {
#if DEBUG
      string str = string.Format("[Debug]:" + format, args);
      Output(Log_Type.LT_Debug, str);
#endif
    }
    public static void Info(string format, params object[] args)
    {
      string str = string.Format("[Info]:" + format, args);
      Output(Log_Type.LT_Info, str);
    }
    public static void Warn(string format, params object[] args)
    {
      string str = string.Format("[Warn]:" + format, args);
      Output(Log_Type.LT_Warn, str);
    }
    public static void Error(string format, params object[] args)
    {
      string str = string.Format("[Error]:" + format, args);
      Output(Log_Type.LT_Error, str);
    }
    public static void Assert(bool check, string format, params object[] args)
    {
      if (!check) 
      {
        string str = string.Format("[Assert]:" + format, args);
        Output(Log_Type.LT_Assert, str);
      }
    }

    public static void Log(string msg)
    {
      Output(Log_Type.LT_Info, msg);
    }

    private static void Output(Log_Type type, string msg)
    {
      if (null != OnOutput) {
        OnOutput(type, msg);
      }
    }
  }
}
