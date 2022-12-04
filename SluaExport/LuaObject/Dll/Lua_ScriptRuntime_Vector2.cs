using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_Vector2 : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 o;
			o=new ScriptRuntime.Vector2();
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
	static public int ctor__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 o;
			System.Single a1;
			checkType(l,1,out a1);
			o=new ScriptRuntime.Vector2(a1);
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
	static public int ctor__Single__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 o;
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			o=new ScriptRuntime.Vector2(a1,a2);
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
	static new public int ToString(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			var ret=self.ToString();
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
	static public int Equals__Vector2(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector2 a1;
			checkValueType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static public int Equals__Object(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static public int Length(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			var ret=self.Length();
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
	static public int LengthSquared(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			var ret=self.LengthSquared();
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
	static public int Normalize(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			self.Normalize();
			pushValue(l,true);
			setBack(l,(object)self);
			return 1;
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
			var ret=ScriptRuntime.Vector2.Distance(a1,a2);
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
	static public int Distance__R_Vector2__R_Vector2__O_Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			ScriptRuntime.Vector2.Distance(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DistanceSquared__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.DistanceSquared(a1,a2);
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
	static public int DistanceSquared__R_Vector2__R_Vector2__O_Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			ScriptRuntime.Vector2.DistanceSquared(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Normalize__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Vector2.Normalize(a1);
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
	static public int Normalize__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			ScriptRuntime.Vector2.Normalize(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reflect__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Reflect(a1,a2);
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
	static public int Reflect__R_Vector2__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Reflect(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Min__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Min(a1,a2);
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
	static public int Min__R_Vector2__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Min(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Max__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Max(a1,a2);
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
	static public int Max__R_Vector2__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Max(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clamp__Vector2__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			var ret=ScriptRuntime.Vector2.Clamp(a1,a2,a3);
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
	static public int Clamp__R_Vector2__R_Vector2__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			ScriptRuntime.Vector2.Clamp(ref a1,ref a2,ref a3,out a4);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			return 5;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Lerp__Vector2__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Vector2.Lerp(a1,a2,a3);
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
	static public int Lerp__R_Vector2__R_Vector2__Single__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			ScriptRuntime.Vector2.Lerp(ref a1,ref a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a4);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SmoothStep__Vector2__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Vector2.SmoothStep(a1,a2,a3);
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
	static public int SmoothStep__R_Vector2__R_Vector2__Single__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Vector2 a4;
			ScriptRuntime.Vector2.SmoothStep(ref a1,ref a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a4);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Negate__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Vector2.Negate(a1);
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
	static public int Negate__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			ScriptRuntime.Vector2.Negate(ref a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Dot__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Dot(a1,a2);
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
	static public int Dot__R_Vector2__R_Vector2__O_Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			ScriptRuntime.Vector2.Dot(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Angle__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Angle(a1,a2);
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
	static public int Angle__R_Vector2__R_Vector2__O_Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			ScriptRuntime.Vector2.Angle(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Add__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Add(a1,a2);
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
	static public int Add__R_Vector2__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Add(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sub__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Sub(a1,a2);
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
	static public int Sub__R_Vector2__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Sub(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Multiply__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Multiply(a1,a2);
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
	static public int Multiply__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Multiply(a1,a2);
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
	static public int Multiply__R_Vector2__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Multiply(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Multiply__R_Vector2__Single__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Multiply(ref a1,a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Divide__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Divide(a1,a2);
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
	static public int Divide__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.Vector2.Divide(a1,a2);
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
	static public int Divide__R_Vector2__R_Vector2__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Divide(ref a1,ref a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Divide__R_Vector2__Single__O_Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			ScriptRuntime.Vector2 a3;
			ScriptRuntime.Vector2.Divide(ref a1,a2,out a3);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_UnaryNegation_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			var ret=-a1;
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
	static public int op_Equality_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=(a1==a2);
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
	static public int op_Inequality_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=(a1!=a2);
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
	static public int op_Addition_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=a1+a2;
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
	static public int op_Subtraction_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=a1-a2;
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
	static public int op_Multiply__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=a1*a2;
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
	static public int op_Multiply__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=a1*a2;
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
	static public int op_Multiply__Single__Vector2_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=a1*a2;
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
	static public int op_Division__Vector2__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 a2;
			checkValueType(l,2,out a2);
			var ret=a1/a2;
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
	static public int op_Division__Vector2__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=a1/a2;
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
	static public int get_X(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.X);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_X(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.X=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Y(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Y);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Y(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Y=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Zero(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector2.Zero);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_One(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector2.One);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UnitX(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector2.UnitX);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UnitY(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector2.UnitY);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getItem(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			int v;
			checkType(l,2,out v);
			var ret = self[v];
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
	static public int setItem(IntPtr l) {
		try {
			ScriptRuntime.Vector2 self;
			checkValueType(l,1,out self);
			int v;
			checkType(l,2,out v);
			float c;
			checkType(l,3,out c);
			self[v]=c;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ScriptRuntime.Vector2");
		addMember(l,ctor_s);
		addMember(l,ctor__Single_s);
		addMember(l,ctor__Single__Single_s);
		addMember(l,ToString);
		addMember(l,Equals__Vector2);
		addMember(l,Equals__Object);
		addMember(l,Length);
		addMember(l,LengthSquared);
		addMember(l,Normalize);
		addMember(l,Distance__Vector2__Vector2_s);
		addMember(l,Distance__R_Vector2__R_Vector2__O_Single_s);
		addMember(l,DistanceSquared__Vector2__Vector2_s);
		addMember(l,DistanceSquared__R_Vector2__R_Vector2__O_Single_s);
		addMember(l,Normalize__Vector2_s);
		addMember(l,Normalize__R_Vector2__O_Vector2_s);
		addMember(l,Reflect__Vector2__Vector2_s);
		addMember(l,Reflect__R_Vector2__R_Vector2__O_Vector2_s);
		addMember(l,Min__Vector2__Vector2_s);
		addMember(l,Min__R_Vector2__R_Vector2__O_Vector2_s);
		addMember(l,Max__Vector2__Vector2_s);
		addMember(l,Max__R_Vector2__R_Vector2__O_Vector2_s);
		addMember(l,Clamp__Vector2__Vector2__Vector2_s);
		addMember(l,Clamp__R_Vector2__R_Vector2__R_Vector2__O_Vector2_s);
		addMember(l,Lerp__Vector2__Vector2__Single_s);
		addMember(l,Lerp__R_Vector2__R_Vector2__Single__O_Vector2_s);
		addMember(l,SmoothStep__Vector2__Vector2__Single_s);
		addMember(l,SmoothStep__R_Vector2__R_Vector2__Single__O_Vector2_s);
		addMember(l,Negate__Vector2_s);
		addMember(l,Negate__R_Vector2__O_Vector2_s);
		addMember(l,Dot__Vector2__Vector2_s);
		addMember(l,Dot__R_Vector2__R_Vector2__O_Single_s);
		addMember(l,Angle__Vector2__Vector2_s);
		addMember(l,Angle__R_Vector2__R_Vector2__O_Single_s);
		addMember(l,Add__Vector2__Vector2_s);
		addMember(l,Add__R_Vector2__R_Vector2__O_Vector2_s);
		addMember(l,Sub__Vector2__Vector2_s);
		addMember(l,Sub__R_Vector2__R_Vector2__O_Vector2_s);
		addMember(l,Multiply__Vector2__Vector2_s);
		addMember(l,Multiply__Vector2__Single_s);
		addMember(l,Multiply__R_Vector2__R_Vector2__O_Vector2_s);
		addMember(l,Multiply__R_Vector2__Single__O_Vector2_s);
		addMember(l,Divide__Vector2__Vector2_s);
		addMember(l,Divide__Vector2__Single_s);
		addMember(l,Divide__R_Vector2__R_Vector2__O_Vector2_s);
		addMember(l,Divide__R_Vector2__Single__O_Vector2_s);
		addMember(l,op_UnaryNegation_s);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,op_Addition_s);
		addMember(l,op_Subtraction_s);
		addMember(l,op_Multiply__Vector2__Vector2_s);
		addMember(l,op_Multiply__Vector2__Single_s);
		addMember(l,op_Multiply__Single__Vector2_s);
		addMember(l,op_Division__Vector2__Vector2_s);
		addMember(l,op_Division__Vector2__Single_s);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"X",get_X,set_X,true);
		addMember(l,"Y",get_Y,set_Y,true);
		addMember(l,"Zero",get_Zero,null,false);
		addMember(l,"One",get_One,null,false);
		addMember(l,"UnitX",get_UnitX,null,false);
		addMember(l,"UnitY",get_UnitY,null,false);
		createTypeMetatable(l,null, typeof(ScriptRuntime.Vector2),typeof(System.ValueType));
	}
}
