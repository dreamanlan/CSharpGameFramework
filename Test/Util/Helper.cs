using System;
using System.Text;
using System.Diagnostics;

namespace DemoCommon
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

    public sealed class Random
    {
      public static int Next()
      {
        return Instance.Next(100);
      }
      public static int Next(int max)
      {
        return Instance.Next(max);
      }
      public static int Next(int min,int max)
      {
        return Instance.Next(min, max);
      }
      public static float NextFloat()
      {
        return (float)Instance.NextDouble();
      }

      private static System.Random Instance
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
      private static System.Random rand = null;
    }

    private static float c_Precision = 0.001f;
  }
}

