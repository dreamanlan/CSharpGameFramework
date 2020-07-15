using System;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace GameFramework
{
  public static class DslUtility
  {
    public static Vector2 CalcVector2(Dsl.FunctionData callData)
    {
      if (null == callData || callData.GetId() != "vector2")
        return Vector2.zero;
      int num = callData.GetParamNum();
      if (2 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        return new Vector2(x, y);
      } else {
        return Vector2.zero;
      }
    }
    public static Vector3 CalcVector3(Dsl.FunctionData callData)
    {
      if (null == callData || callData.GetId() != "vector3")
        return Vector3.zero;
      int num = callData.GetParamNum();
      if (3 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        return new Vector3(x, y, z);
      } else {
        return Vector3.zero;
      }
    }
    public static Vector4 CalcVector4(Dsl.FunctionData callData)
    {
      if (null == callData || callData.GetId() != "vector4")
        return Vector4.zero;
      int num = callData.GetParamNum();
      if (4 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        float w = float.Parse(callData.GetParamId(3));
        return new Vector4(x, y, z, w);
      } else {
        return Vector4.zero;
      }
    }
    public static Color CalcColor(Dsl.FunctionData callData)
    {
      if (null == callData || callData.GetId() != "color")
        return Color.white;
      int num = callData.GetParamNum();
      if (4 == num) {
        float r = float.Parse(callData.GetParamId(0));
        float g = float.Parse(callData.GetParamId(1));
        float b = float.Parse(callData.GetParamId(2));
        float a = float.Parse(callData.GetParamId(3));
        return new Color(r, g, b, a);
      } else {
        return Color.white;
      }
    }
    public static Quaternion CalcQuaternion(Dsl.FunctionData callData)
    {
      if (null == callData || callData.GetId() != "quaternion")
        return Quaternion.identity;
      int num = callData.GetParamNum();
      if (4 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        float w = float.Parse(callData.GetParamId(3));
        return new Quaternion(x, y, z, w);
      } else {
        return Quaternion.identity;
      }
    }
    public static Quaternion CalcEularRotation(Dsl.FunctionData callData)
    {
      if (null == callData || callData.GetId() != "eular")
        return Quaternion.identity;
      int num = callData.GetParamNum();
      if (3 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        try {
          return Quaternion.Euler(x, y, z);
        } catch {
          return Quaternion.identity;
        }
      } else {
        return Quaternion.identity;
      }
    }
    public static Vector3 CalcEularAngles(Dsl.FunctionData callData)
    {
      if (null == callData || callData.GetId() != "eular")
        return Vector3.zero;
      int num = callData.GetParamNum();
      if (3 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        try {
          return Quaternion.Euler(x, y, z).eulerAngles;
        } catch {
          return new Vector3(x, y, z) * 59.29578f;
        }
      } else {
        return Vector3.zero;
      }
    }
  }
}
