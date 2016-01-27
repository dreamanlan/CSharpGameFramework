using System;
using System.Collections.Generic;

namespace System.Threading
{
  public static class ClientInterlocked
  {
    public static int Add(ref int location, int value)
    {
      return Interlocked.Add(ref location, value);
    }
    public static long Add(ref long location, long value)
    {
      return Interlocked.Add(ref location, value);
    }
    public static double CompareExchange(ref double location, double value, double comparand)
    {
      return Interlocked.CompareExchange(ref location, value, comparand);
    }
    public static int CompareExchange(ref int location, int value, int comparand)
    {
      return Interlocked.CompareExchange(ref location, value, comparand);
    }
    public static long CompareExchange(ref long location, long value, long comparand)
    {
      return Interlocked.CompareExchange(ref location, value, comparand);
    }
    public static IntPtr CompareExchange(ref IntPtr location, IntPtr value, IntPtr comparand)
    {
      return Interlocked.CompareExchange(ref location, value, comparand);
    }
    public static object CompareExchange(ref object location, object value, object comparand)
    {
      return Interlocked.CompareExchange(ref location, value, comparand);
    }
    public static float CompareExchange(ref float location, float value, float comparand)
    {
      return Interlocked.CompareExchange(ref location, value, comparand);
    }
    public static int Decrement(ref int location)
    {
      return Interlocked.Decrement(ref location);
    }
    public static long Decrement(ref long location)
    {
      return Interlocked.Decrement(ref location);
    }
    public static double Exchange(ref double location, double value)
    {
      return Interlocked.Exchange(ref location, value);
    }
    public static int Exchange(ref int location, int value)
    {
      return Interlocked.Exchange(ref location, value);
    }
    public static long Exchange(ref long location, long value)
    {
      return Interlocked.Exchange(ref location, value);
    }
    public static IntPtr Exchange(ref IntPtr location, IntPtr value)
    {
      return Interlocked.Exchange(ref location, value);
    }
    public static object Exchange(ref object location, object value)
    {
      return Interlocked.Exchange(ref location, value);
    }
    public static float Exchange(ref float location, float value)
    {
      return Interlocked.Exchange(ref location, value);
    }
    public static int Increment(ref int location)
    {
      return Interlocked.Increment(ref location);
    }
    public static long Increment(ref long location)
    {
      return Interlocked.Increment(ref location);
    }
    public static long Read(ref long location)
    {
      return Interlocked.Read(ref location);
    }
  }
}
