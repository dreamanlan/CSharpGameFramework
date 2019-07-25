using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Geometry : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int IsInvalid_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				var ret=GameFramework.Geometry.IsInvalid(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				var ret=GameFramework.Geometry.IsInvalid(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(float))){
				System.Single a1;
				checkType(l,1,out a1);
				var ret=GameFramework.Geometry.IsInvalid(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int DistanceSquare_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.DistanceSquare(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector2 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.DistanceSquare(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Distance_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.Distance(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector2 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.Distance(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcLength_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector3>))){
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a1;
				checkType(l,1,out a1);
				var ret=GameFramework.Geometry.CalcLength(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector2>))){
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a1;
				checkType(l,1,out a1);
				var ret=GameFramework.Geometry.CalcLength(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int IsSamePoint_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.IsSamePoint(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector2 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.IsSamePoint(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetBezierPoint_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(float))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(float))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(float))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(float))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FrontOfTarget_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(float))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(float))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int GetYRadian_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.GetYRadian(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector2 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.GetYRadian(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRotate_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(float))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=GameFramework.Geometry.GetRotate(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(float))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				var ret=GameFramework.Geometry.GetRotate(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int Multiply_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DotMultiply_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Relation_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Perpendicular_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointToLineSegmentDistance_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(LuaOut))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(LuaOut))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointToLineSegmentDistanceSquare_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(LuaOut))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(LuaOut))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointToLineDistance_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointToLineDistanceInverse_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointToPolylineDistance_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(IList<ScriptRuntime.Vector3>),typeof(int),typeof(int),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				ScriptRuntime.Vector3 a5;
				checkValueType(l,5,out a5);
				var ret=GameFramework.Geometry.PointToPolylineDistance(a1,a2,a3,a4,ref a5);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a5);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(IList<ScriptRuntime.Vector2>),typeof(int),typeof(int),typeof(ScriptRuntime.Vector2))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				ScriptRuntime.Vector2 a5;
				checkValueType(l,5,out a5);
				var ret=GameFramework.Geometry.PointToPolylineDistance(a1,a2,a3,a4,ref a5);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a5);
				return 3;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointToPolylineDistanceSquare_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(IList<ScriptRuntime.Vector3>),typeof(int),typeof(int),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				ScriptRuntime.Vector3 a5;
				checkValueType(l,5,out a5);
				var ret=GameFramework.Geometry.PointToPolylineDistanceSquare(a1,a2,a3,a4,ref a5);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a5);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(IList<ScriptRuntime.Vector2>),typeof(int),typeof(int),typeof(ScriptRuntime.Vector2))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				ScriptRuntime.Vector2 a5;
				checkValueType(l,5,out a5);
				var ret=GameFramework.Geometry.PointToPolylineDistanceSquare(a1,a2,a3,a4,ref a5);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a5);
				return 3;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Intersect_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LineIntersectRectangle_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(float),typeof(float),typeof(float),typeof(float))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(float),typeof(float),typeof(float),typeof(float))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointOnLine_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointOverlapPoint_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector3 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.PointOverlapPoint(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				ScriptRuntime.Vector2 a2;
				checkValueType(l,2,out a2);
				var ret=GameFramework.Geometry.PointOverlapPoint(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int PointIsLeft_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointIsLeftOn_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointIsCollinear_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsCounterClockwise_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector3>),typeof(int),typeof(int))){
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				var ret=GameFramework.Geometry.IsCounterClockwise(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector2>),typeof(int),typeof(int))){
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				var ret=GameFramework.Geometry.IsCounterClockwise(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcPolygonCentroidAndRadius_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector3>),typeof(int),typeof(int),typeof(LuaOut))){
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				ScriptRuntime.Vector3 a4;
				var ret=GameFramework.Geometry.CalcPolygonCentroidAndRadius(a1,a2,a3,out a4);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a4);
				return 3;
			}
			else if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector2>),typeof(int),typeof(int),typeof(LuaOut))){
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				ScriptRuntime.Vector2 a4;
				var ret=GameFramework.Geometry.CalcPolygonCentroidAndRadius(a1,a2,a3,out a4);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a4);
				return 3;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcPolygonBound_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector3>),typeof(int),typeof(int),typeof(LuaOut),typeof(LuaOut),typeof(LuaOut),typeof(LuaOut))){
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Single a4;
				System.Single a5;
				System.Single a6;
				System.Single a7;
				GameFramework.Geometry.CalcPolygonBound(a1,a2,a3,out a4,out a5,out a6,out a7);
				pushValue(l,true);
				pushValue(l,a4);
				pushValue(l,a5);
				pushValue(l,a6);
				pushValue(l,a7);
				return 5;
			}
			else if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector2>),typeof(int),typeof(int),typeof(LuaOut),typeof(LuaOut),typeof(LuaOut),typeof(LuaOut))){
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Single a4;
				System.Single a5;
				System.Single a6;
				System.Single a7;
				GameFramework.Geometry.CalcPolygonBound(a1,a2,a3,out a4,out a5,out a6,out a7);
				pushValue(l,true);
				pushValue(l,a4);
				pushValue(l,a5);
				pushValue(l,a6);
				pushValue(l,a7);
				return 5;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointInPolygon_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(IList<ScriptRuntime.Vector3>),typeof(int),typeof(int))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				var ret=GameFramework.Geometry.PointInPolygon(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(IList<ScriptRuntime.Vector2>),typeof(int),typeof(int))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				var ret=GameFramework.Geometry.PointInPolygon(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(IList<ScriptRuntime.Vector3>),typeof(int),typeof(int),typeof(float),typeof(float),typeof(float),typeof(float))){
				ScriptRuntime.Vector3 a1;
				checkValueType(l,1,out a1);
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				System.Single a6;
				checkType(l,6,out a6);
				System.Single a7;
				checkType(l,7,out a7);
				System.Single a8;
				checkType(l,8,out a8);
				var ret=GameFramework.Geometry.PointInPolygon(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(IList<ScriptRuntime.Vector2>),typeof(int),typeof(int),typeof(float),typeof(float),typeof(float),typeof(float))){
				ScriptRuntime.Vector2 a1;
				checkValueType(l,1,out a1);
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				System.Single a6;
				checkType(l,6,out a6);
				System.Single a7;
				checkType(l,7,out a7);
				System.Single a8;
				checkType(l,8,out a8);
				var ret=GameFramework.Geometry.PointInPolygon(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointInTriangle_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SignArea_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3),typeof(ScriptRuntime.Vector3))){
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
			else if(matchType(l,argc,1,typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2),typeof(ScriptRuntime.Vector2))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Sparseness_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector3>),typeof(int),typeof(int),typeof(float))){
				System.Collections.Generic.IList<ScriptRuntime.Vector3> a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				var ret=GameFramework.Geometry.Sparseness(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(IList<ScriptRuntime.Vector2>),typeof(int),typeof(int),typeof(float))){
				System.Collections.Generic.IList<ScriptRuntime.Vector2> a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				var ret=GameFramework.Geometry.Sparseness(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Geometry");
		addMember(l,RadianToDegree_s);
		addMember(l,DegreeToRadian_s);
		addMember(l,IsInvalid_s);
		addMember(l,Max_s);
		addMember(l,Min_s);
		addMember(l,DistanceSquare_s);
		addMember(l,Distance_s);
		addMember(l,CalcLength_s);
		addMember(l,IsSameFloat_s);
		addMember(l,IsSameDouble_s);
		addMember(l,IsSamePoint_s);
		addMember(l,GetBezierPoint_s);
		addMember(l,FrontOfTarget_s);
		addMember(l,SegmentPointSqrDistance_s);
		addMember(l,IsCapsuleDiskIntersect_s);
		addMember(l,IsAabbDiskIntersect_s);
		addMember(l,IsObbDiskIntersect_s);
		addMember(l,IsSectorDiskIntersect_s);
		addMember(l,GetYRadian_s);
		addMember(l,GetRotate_s);
		addMember(l,TransformPoint_s);
		addMember(l,Multiply_s);
		addMember(l,DotMultiply_s);
		addMember(l,Relation_s);
		addMember(l,Perpendicular_s);
		addMember(l,PointToLineSegmentDistance_s);
		addMember(l,PointToLineSegmentDistanceSquare_s);
		addMember(l,PointToLineDistance_s);
		addMember(l,PointToLineDistanceInverse_s);
		addMember(l,PointToPolylineDistance_s);
		addMember(l,PointToPolylineDistanceSquare_s);
		addMember(l,Intersect_s);
		addMember(l,LineIntersectRectangle_s);
		addMember(l,PointOnLine_s);
		addMember(l,PointOverlapPoint_s);
		addMember(l,RectangleOverlapRectangle_s);
		addMember(l,PointIsLeft_s);
		addMember(l,PointIsLeftOn_s);
		addMember(l,PointIsCollinear_s);
		addMember(l,IsCounterClockwise_s);
		addMember(l,CalcPolygonCentroidAndRadius_s);
		addMember(l,CalcPolygonBound_s);
		addMember(l,PointInPolygon_s);
		addMember(l,PointInTriangle_s);
		addMember(l,SignArea_s);
		addMember(l,Sparseness_s);
		addMember(l,"c_MinDistance",get_c_MinDistance,null,false);
		addMember(l,"c_FloatPrecision",get_c_FloatPrecision,null,false);
		addMember(l,"c_DoublePrecision",get_c_DoublePrecision,null,false);
		createTypeMetatable(l,constructor, typeof(GameFramework.Geometry));
	}
}
