using System;
using System.Text;
using System.Diagnostics;

namespace GameFramework
{
  public sealed class Helper
  {
    public static string BinToHex(byte[] bytes)
    {
      return BinToHex(bytes, 0);
    }
    public static string BinToHex(byte[] bytes, int start)
    {
      return BinToHex(bytes, start, bytes.Length - start);
    }
    public static string BinToHex(byte[] bytes, int start, int count)
    {
      if (start < 0 || count <= 0 || start + count > bytes.Length)
        return "";
      StringBuilder sb = new StringBuilder(count * 4);
      for (int ix = 0; ix < count; ++ix) {
        sb.AppendFormat("{0,2:X2}", bytes[ix+start]);
        if ((ix + 1) % 16 == 0)
          sb.AppendLine();
        else
          sb.Append(' ');
      }
      return sb.ToString();
    }

    public static bool StringIsNullOrEmpty(string str)
    {
      if (str == null || str == "")
        return true;
      return false;
    }

    public static bool IsSameFloat(float arg0, float arg1)
    {
      return Math.Abs(arg0 - arg1) < c_Precision;
    }
    public static bool IsSameVector3(ScriptRuntime.Vector3 arg0, ScriptRuntime.Vector3 arg1)
    {
      return IsSameFloat(arg0.X, arg1.X) && IsSameFloat(arg0.Y, arg1.Y) && IsSameFloat(arg0.Z, arg1.Z);
    }
    public static bool IsDifferentMonth(DateTime last_time)
    {
      if (last_time.Year != DateTime.Now.Year || last_time.Month != DateTime.Now.Month) {
        return true;
      }
      return false;
    }
    public static bool IsDifferentDay(double last_time)
    {
      DateTime dt = (new DateTime(1970, 1, 1, 0, 0, 0)).AddHours(8).AddSeconds(last_time);
      if (dt.Year != DateTime.Now.Year || dt.Month != DateTime.Now.Month || dt.Day != DateTime.Now.Day) {
        return true;
      }
      return false;
    }
    public static bool IsDifferentDay(DateTime last_time)
    {
      if (last_time.Year != DateTime.Now.Year || last_time.Month != DateTime.Now.Month || last_time.Day != DateTime.Now.Day) {
        return true;
      }
      return false;
    }
    public static bool IsDifferentMinute(DateTime time)
    {
      if (time.Minute != DateTime.Now.Minute) {
        return true;
      }
      return false;
    }
    public static bool IsInterval24Hours(DateTime last_time)
    {
      double c_hours_per_day = 24f;
      TimeSpan ts = DateTime.Now - last_time;
      if (ts.TotalHours >= c_hours_per_day) {
        return true;
      }
      return false;
    }

    public static void LogCallStack()
    {
      LogCallStack(true);
    }
    public static void LogCallStack(bool useErrorLog)
    {
      if (useErrorLog)
        LogSystem.Error("LogCallStack:\n{0}\n", Environment.StackTrace);
      else
        LogSystem.Info("LogCallStack:\n{0}\n",Environment.StackTrace);
    }

    public static float RadianToDegree(float dir)
    {
      return (float)(dir * 180 / Math.PI);
    }
    public static float DegreeToRadian(float dir)
    {
      return (float)(dir * Math.PI / 180);
    }
    public sealed class Random
    {
      static public int Next()
      {
        return Instance.Next(100);
      }
      static public int Next(int max)
      {
        return Instance.Next(max);
      }
      static public int Next(int min,int max)
      {
        return Instance.Next(min, max);
      }
      static public float NextFloat()
      {
        return (float)Instance.NextDouble();
      }

      static private System.Random Instance
      {
        get
        {
          if (null == rand) {
            rand = new System.Random();
          }
          return rand;
        }
      }
      [ThreadStatic]
      static private System.Random rand = null;
    }

    static private float c_Precision = 0.001f;
  }
}

