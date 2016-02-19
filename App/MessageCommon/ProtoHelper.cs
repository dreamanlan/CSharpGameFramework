using System;
using System.Collections.Generic;

namespace GameFramework
{
  public sealed class ProtoHelper
  {
    public static int EncodeFloat(float value)
    {
      int v = (int)(value * 100.0f);
      return v;
    }
    public static float DecodeFloat(int value)
    {
      float v = (float)value / 100.0f;
      return v;
    }
    public static ulong EncodePosition2D(float x,float y)
    {
      ulong vx = (ulong)EncodeFloat(x);
      ulong vy = (ulong)EncodeFloat(y);
      return (vy << 17) + vx;
    }
    public static void DecodePosition2D(ulong v, out float x, out float y)
    {
      ulong vx = (v & 0x1ffff);
      ulong vy = ((v & 0x3fffe0000)>>17);
      x = DecodeFloat((int)vx);
      y = DecodeFloat((int)vy);
    }
    public static ulong EncodePosition3D(float x, float y, float z)
    {
      ulong vx = (ulong)EncodeFloat(x);
      ulong vy = (ulong)EncodeFloat(y);
      ulong vz = (ulong)EncodeFloat(z);
      return (vz << 34) + (vy << 17) + vx;
    }
    public static void DecodePosition3D(ulong v, out float x, out float y, out float z)
    {
      ulong vx = (v & 0x1ffff);
      ulong vy = ((v & 0x3fffe0000) >> 17);
      ulong vz = ((v & 0x7fffc00000000) >> 34);
      x = DecodeFloat((int)vx);
      y = DecodeFloat((int)vy);
      z = DecodeFloat((int)vz);
    }
  }
}
