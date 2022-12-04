using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Geometry : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.Geometry o;
			o=new GameFramework.Geometry();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RadianToDegree_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Geometry.RadianToDegree(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DegreeToRadian_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Geometry.DegreeToRadian(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsInvalid__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			var ret=GameFramework.Geometry.IsInvalid(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsInvalid__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			var ret=GameFramework.Geometry.IsInvalid(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsInvalid__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Geometry.IsInvalid(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Max_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Geometry.Max(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Min_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Geometry.Min(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DistanceSquare__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.DistanceSquare(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DistanceSquare__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.DistanceSquare(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DistanceSquare__Single__Single__Single__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=GameFramework.Geometry.DistanceSquare(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Distance__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.Distance(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Distance__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.Distance(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsSameFloat_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Geometry.IsSameFloat(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsSameDouble_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			System.Double a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Geometry.IsSameDouble(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsSamePoint__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.IsSamePoint(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsSamePoint__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.IsSamePoint(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetBezierPoint__Vector2__Vector2__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=GameFramework.Geometry.GetBezierPoint(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetBezierPoint__Vector3__Vector3__Vector3__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=GameFramework.Geometry.GetBezierPoint(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetBezierPoint__Vector2__Vector2__Vector2__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			checkValueType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			var ret=GameFramework.Geometry.GetBezierPoint(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetBezierPoint__Vector3__Vector3__Vector3__Vector3__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector3 a4;
			checkValueType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			var ret=GameFramework.Geometry.GetBezierPoint(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FrontOfTarget__Vector2__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=GameFramework.Geometry.FrontOfTarget(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FrontOfTarget__Vector3__Vector3__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=GameFramework.Geometry.FrontOfTarget(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SegmentPointSqrDistance_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.SegmentPointSqrDistance(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsCapsuleDiskIntersect_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			checkValueType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			var ret=GameFramework.Geometry.IsCapsuleDiskIntersect(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsAabbDiskIntersect_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=GameFramework.Geometry.IsAabbDiskIntersect(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsObbDiskIntersect_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			checkValueType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			var ret=GameFramework.Geometry.IsObbDiskIntersect(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsSectorDiskIntersect_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			ScriptRuntime.Vector2 a5;
			checkValueType(l,5,out a5);
			System.Single a6;
			checkType(l,6,out a6);
			var ret=GameFramework.Geometry.IsSectorDiskIntersect(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetYRadian__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.GetYRadian(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetYRadian__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.GetYRadian(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetRotate__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Geometry.GetRotate(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetRotate__Vector3__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Geometry.GetRotate(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TransformPoint_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=GameFramework.Geometry.TransformPoint(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Multiply__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.Multiply(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Multiply__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.Multiply(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DotMultiply__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.DotMultiply(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DotMultiply__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.DotMultiply(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Relation__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.Relation(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Relation__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.Relation(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Perpendicular__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.Perpendicular(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Perpendicular__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.Perpendicular(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointToLineSegmentDistance__Vector2__Vector2__Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			var ret=GameFramework.Geometry.PointToLineSegmentDistance(a1,a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointToLineSegmentDistance__Vector3__Vector3__Vector3__O_Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector3 a4;
			var ret=GameFramework.Geometry.PointToLineSegmentDistance(a1,a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointToLineSegmentDistanceSquare__Vector2__Vector2__Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			var ret=GameFramework.Geometry.PointToLineSegmentDistanceSquare(a1,a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointToLineSegmentDistanceSquare__Vector3__Vector3__Vector3__O_Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector3 a4;
			var ret=GameFramework.Geometry.PointToLineSegmentDistanceSquare(a1,a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointToLineDistance__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointToLineDistance(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointToLineDistance__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointToLineDistance(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointToLineDistanceInverse__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointToLineDistanceInverse(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointToLineDistanceInverse__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointToLineDistanceInverse(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersect__Vector2__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			checkValueType(l,4,out a4);
			var ret=GameFramework.Geometry.Intersect(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Intersect__Vector3__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector3 a4;
			checkValueType(l,4,out a4);
			var ret=GameFramework.Geometry.Intersect(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LineIntersectRectangle__Vector2__Vector2__Single__Single__Single__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			System.Single a6;
			checkType(l,6,out a6);
			var ret=GameFramework.Geometry.LineIntersectRectangle(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LineIntersectRectangle__Vector3__Vector3__Single__Single__Single__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			System.Single a6;
			checkType(l,6,out a6);
			var ret=GameFramework.Geometry.LineIntersectRectangle(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointOnLine__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointOnLine(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointOnLine__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointOnLine(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointOverlapPoint__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.PointOverlapPoint(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointOverlapPoint__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Geometry.PointOverlapPoint(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RectangleOverlapRectangle_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			System.Single a6;
			checkType(l,6,out a6);
			System.Single a7;
			checkType(l,7,out a7);
			System.Single a8;
			checkType(l,8,out a8);
			var ret=GameFramework.Geometry.RectangleOverlapRectangle(a1,a2,a3,a4,a5,a6,a7,a8);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointIsLeft__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointIsLeft(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointIsLeft__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointIsLeft(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointIsLeftOn__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointIsLeftOn(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointIsLeftOn__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointIsLeftOn(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointIsCollinear__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointIsCollinear(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointIsCollinear__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.PointIsCollinear(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointInTriangle__Vector2__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			checkValueType(l,4,out a4);
			var ret=GameFramework.Geometry.PointInTriangle(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PointInTriangle__Vector3__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector3 a4;
			checkValueType(l,4,out a4);
			var ret=GameFramework.Geometry.PointInTriangle(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SignArea__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.SignArea(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SignArea__Vector3__Vector3__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector3 a3;
			checkValueType(l,3,out a3);
			var ret=GameFramework.Geometry.SignArea(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_MinDistance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Geometry.c_MinDistance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_FloatPrecision(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Geometry.c_FloatPrecision);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_DoublePrecision(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Geometry.c_DoublePrecision);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Geometry");
		addMember(l,ctor_s);
		addMember(l,RadianToDegree_s);
		addMember(l,DegreeToRadian_s);
		addMember(l,IsInvalid__Vector2_s);
		addMember(l,IsInvalid__Vector3_s);
		addMember(l,IsInvalid__Single_s);
		addMember(l,Max_s);
		addMember(l,Min_s);
		addMember(l,DistanceSquare__Vector2__Vector2_s);
		addMember(l,DistanceSquare__Vector3__Vector3_s);
		addMember(l,DistanceSquare__Single__Single__Single__Single_s);
		addMember(l,Distance__Vector2__Vector2_s);
		addMember(l,Distance__Vector3__Vector3_s);
		addMember(l,IsSameFloat_s);
		addMember(l,IsSameDouble_s);
		addMember(l,IsSamePoint__Vector2__Vector2_s);
		addMember(l,IsSamePoint__Vector3__Vector3_s);
		addMember(l,GetBezierPoint__Vector2__Vector2__Vector2__Single_s);
		addMember(l,GetBezierPoint__Vector3__Vector3__Vector3__Single_s);
		addMember(l,GetBezierPoint__Vector2__Vector2__Vector2__Vector2__Single_s);
		addMember(l,GetBezierPoint__Vector3__Vector3__Vector3__Vector3__Single_s);
		addMember(l,FrontOfTarget__Vector2__Vector2__Single_s);
		addMember(l,FrontOfTarget__Vector3__Vector3__Single_s);
		addMember(l,SegmentPointSqrDistance_s);
		addMember(l,IsCapsuleDiskIntersect_s);
		addMember(l,IsAabbDiskIntersect_s);
		addMember(l,IsObbDiskIntersect_s);
		addMember(l,IsSectorDiskIntersect_s);
		addMember(l,GetYRadian__Vector2__Vector2_s);
		addMember(l,GetYRadian__Vector3__Vector3_s);
		addMember(l,GetRotate__Vector2__Single_s);
		addMember(l,GetRotate__Vector3__Single_s);
		addMember(l,TransformPoint_s);
		addMember(l,Multiply__Vector2__Vector2__Vector2_s);
		addMember(l,Multiply__Vector3__Vector3__Vector3_s);
		addMember(l,DotMultiply__Vector2__Vector2__Vector2_s);
		addMember(l,DotMultiply__Vector3__Vector3__Vector3_s);
		addMember(l,Relation__Vector2__Vector2__Vector2_s);
		addMember(l,Relation__Vector3__Vector3__Vector3_s);
		addMember(l,Perpendicular__Vector2__Vector2__Vector2_s);
		addMember(l,Perpendicular__Vector3__Vector3__Vector3_s);
		addMember(l,PointToLineSegmentDistance__Vector2__Vector2__Vector2__O_Vector2_s);
		addMember(l,PointToLineSegmentDistance__Vector3__Vector3__Vector3__O_Vector3_s);
		addMember(l,PointToLineSegmentDistanceSquare__Vector2__Vector2__Vector2__O_Vector2_s);
		addMember(l,PointToLineSegmentDistanceSquare__Vector3__Vector3__Vector3__O_Vector3_s);
		addMember(l,PointToLineDistance__Vector2__Vector2__Vector2_s);
		addMember(l,PointToLineDistance__Vector3__Vector3__Vector3_s);
		addMember(l,PointToLineDistanceInverse__Vector2__Vector2__Vector2_s);
		addMember(l,PointToLineDistanceInverse__Vector3__Vector3__Vector3_s);
		addMember(l,Intersect__Vector2__Vector2__Vector2__Vector2_s);
		addMember(l,Intersect__Vector3__Vector3__Vector3__Vector3_s);
		addMember(l,LineIntersectRectangle__Vector2__Vector2__Single__Single__Single__Single_s);
		addMember(l,LineIntersectRectangle__Vector3__Vector3__Single__Single__Single__Single_s);
		addMember(l,PointOnLine__Vector2__Vector2__Vector2_s);
		addMember(l,PointOnLine__Vector3__Vector3__Vector3_s);
		addMember(l,PointOverlapPoint__Vector2__Vector2_s);
		addMember(l,PointOverlapPoint__Vector3__Vector3_s);
		addMember(l,RectangleOverlapRectangle_s);
		addMember(l,PointIsLeft__Vector2__Vector2__Vector2_s);
		addMember(l,PointIsLeft__Vector3__Vector3__Vector3_s);
		addMember(l,PointIsLeftOn__Vector2__Vector2__Vector2_s);
		addMember(l,PointIsLeftOn__Vector3__Vector3__Vector3_s);
		addMember(l,PointIsCollinear__Vector2__Vector2__Vector2_s);
		addMember(l,PointIsCollinear__Vector3__Vector3__Vector3_s);
		addMember(l,PointInTriangle__Vector2__Vector2__Vector2__Vector2_s);
		addMember(l,PointInTriangle__Vector3__Vector3__Vector3__Vector3_s);
		addMember(l,SignArea__Vector2__Vector2__Vector2_s);
		addMember(l,SignArea__Vector3__Vector3__Vector3_s);
		addMember(l,"c_MinDistance",get_c_MinDistance,null,false);
		addMember(l,"c_FloatPrecision",get_c_FloatPrecision,null,false);
		addMember(l,"c_DoublePrecision",get_c_DoublePrecision,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.Geometry));
	}
}
