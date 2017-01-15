using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace GameFramework
{
  public static class DslUtility
  {
    public static Vector2 CalcVector2(Dsl.CallData callData)
    {
      if (null == callData || callData.GetId() != "vector2")
        return Vector2.Zero;
      int num = callData.GetParamNum();
      if (2 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        return new Vector2(x, y);
      } else {
        return Vector2.Zero;
      }
    }
    public static Vector3 CalcVector3(Dsl.CallData callData)
    {
      if (null == callData || callData.GetId() != "vector3")
        return Vector3.Zero;
      int num = callData.GetParamNum();
      if (3 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        return new Vector3(x, y, z);
      } else {
        return Vector3.Zero;
      }
    }
    public static Vector4 CalcVector4(Dsl.CallData callData)
    {
      if (null == callData || callData.GetId() != "vector4")
        return Vector4.Zero;
      int num = callData.GetParamNum();
      if (4 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        float w = float.Parse(callData.GetParamId(3));
        return new Vector4(x, y, z, w);
      } else {
        return Vector4.Zero;
      }
    }
    public static Quaternion CalcQuaternion(Dsl.CallData callData)
    {
      if (null == callData || callData.GetId() != "quaternion")
        return Quaternion.Identity;
      int num = callData.GetParamNum();
      if (4 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        float w = float.Parse(callData.GetParamId(3));
        return new Quaternion(x, y, z, w);
      } else {
        return Quaternion.Identity;
      }
    }
    public static Quaternion CalcEularRotation(Dsl.CallData callData)
    {
      if (null == callData || callData.GetId() != "eular")
        return Quaternion.Identity;
      int num = callData.GetParamNum();
      if (3 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        return Quaternion.CreateFromYawPitchRoll(x, y, z);
      } else {
        return Quaternion.Identity;
      }
    }
    public static Vector3 CalcEularAngles(Dsl.CallData callData)
    {
      if (null == callData || callData.GetId() != "eular")
        return Vector3.Zero;
      int num = callData.GetParamNum();
      if (3 == num) {
        float x = float.Parse(callData.GetParamId(0));
        float y = float.Parse(callData.GetParamId(1));
        float z = float.Parse(callData.GetParamId(2));
        return new Vector3(x, y, z) * 59.29578f;
      } else {
        return Vector3.Zero;
      }
    }
  }
}
