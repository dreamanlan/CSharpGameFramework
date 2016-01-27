using System;
using System.Collections.Generic;
//using System.Diagnostics;
using ScriptRuntime;

namespace GameFramework
{
  public class Geometry3D
  {
    public static ScriptRuntime.Vector3 GetCenter(ScriptRuntime.Vector3 fvPos1, ScriptRuntime.Vector3 fvPos2)
    {
      ScriptRuntime.Vector3 fvRet = new ScriptRuntime.Vector3();

      fvRet.X = (fvPos1.X + fvPos2.X) / 2.0f;
      fvRet.Y = (fvPos1.Y + fvPos2.Y) / 2.0f;
      fvRet.Z = (fvPos1.Z + fvPos2.Z) / 2.0f;

      return fvRet;
    }
  }
}

