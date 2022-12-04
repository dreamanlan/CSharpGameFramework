using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ScriptRuntime_Vector4 : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 o;
			o=new ScriptRuntime.Vector4();
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
			ScriptRuntime.Vector4 o;
			System.Single a1;
			checkType(l,1,out a1);
			o=new ScriptRuntime.Vector4(a1);
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
	static public int ctor__Vector3__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 o;
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			o=new ScriptRuntime.Vector4(a1,a2);
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
	static public int ctor__Vector2__Single__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 o;
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			o=new ScriptRuntime.Vector4(a1,a2,a3);
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
	static public int ctor__Single__Single__Single__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 o;
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			o=new ScriptRuntime.Vector4(a1,a2,a3,a4);
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
			ScriptRuntime.Vector4 self;
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
	static public int Equals__Vector4(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector4 a1;
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
			ScriptRuntime.Vector4 self;
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
			ScriptRuntime.Vector4 self;
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
			ScriptRuntime.Vector4 self;
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
			ScriptRuntime.Vector4 self;
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
	static public int Distance__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Distance(a1,a2);
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
	static public int Distance__R_Vector4__R_Vector4__O_Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			ScriptRuntime.Vector4.Distance(ref a1,ref a2,out a3);
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
	static public int DistanceSquared__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.DistanceSquared(a1,a2);
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
	static public int DistanceSquared__R_Vector4__R_Vector4__O_Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			ScriptRuntime.Vector4.DistanceSquared(ref a1,ref a2,out a3);
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
	static public int Dot__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Dot(a1,a2);
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
	static public int Dot__R_Vector4__R_Vector4__O_Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			ScriptRuntime.Vector4.Dot(ref a1,ref a2,out a3);
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
	static public int Normalize__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Vector4.Normalize(a1);
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
	static public int Normalize__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			ScriptRuntime.Vector4.Normalize(ref a1,out a2);
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
	static public int Min__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Min(a1,a2);
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
	static public int Min__R_Vector4__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Min(ref a1,ref a2,out a3);
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
	static public int Max__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Max(a1,a2);
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
	static public int Max__R_Vector4__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Max(ref a1,ref a2,out a3);
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
	static public int Clamp__Vector4__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			checkValueType(l,3,out a3);
			var ret=ScriptRuntime.Vector4.Clamp(a1,a2,a3);
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
	static public int Clamp__R_Vector4__R_Vector4__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector4 a4;
			ScriptRuntime.Vector4.Clamp(ref a1,ref a2,ref a3,out a4);
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
	static public int Lerp__Vector4__Vector4__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Vector4.Lerp(a1,a2,a3);
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
	static public int Lerp__R_Vector4__R_Vector4__Single__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Vector4 a4;
			ScriptRuntime.Vector4.Lerp(ref a1,ref a2,a3,out a4);
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
	static public int SmoothStep__Vector4__Vector4__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=ScriptRuntime.Vector4.SmoothStep(a1,a2,a3);
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
	static public int SmoothStep__R_Vector4__R_Vector4__Single__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			ScriptRuntime.Vector4 a4;
			ScriptRuntime.Vector4.SmoothStep(ref a1,ref a2,a3,out a4);
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
	static public int Hermite__Vector4__Vector4__Vector4__Vector4__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector4 a4;
			checkValueType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			var ret=ScriptRuntime.Vector4.Hermite(a1,a2,a3,a4,a5);
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
	static public int Hermite__R_Vector4__R_Vector4__R_Vector4__R_Vector4__Single__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			checkValueType(l,3,out a3);
			ScriptRuntime.Vector4 a4;
			checkValueType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			ScriptRuntime.Vector4 a6;
			ScriptRuntime.Vector4.Hermite(ref a1,ref a2,ref a3,ref a4,a5,out a6);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			pushValue(l,a6);
			return 6;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Project__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Project(a1,a2);
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
	static public int Project__R_Vector4__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Project(ref a1,ref a2,out a3);
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
	static public int Negate__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			var ret=ScriptRuntime.Vector4.Negate(a1);
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
	static public int Negate__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			ScriptRuntime.Vector4.Negate(ref a1,out a2);
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
	static public int Add__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Add(a1,a2);
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
	static public int Add__R_Vector4__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Add(ref a1,ref a2,out a3);
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
	static public int Sub__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Sub(a1,a2);
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
	static public int Sub__R_Vector4__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Sub(ref a1,ref a2,out a3);
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
	static public int Multiply__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Multiply(a1,a2);
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
	static public int Multiply__Vector4__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Multiply(a1,a2);
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
	static public int Multiply__R_Vector4__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Multiply(ref a1,ref a2,out a3);
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
	static public int Multiply__R_Vector4__Single__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Multiply(ref a1,a2,out a3);
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
	static public int Divide__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Divide(a1,a2);
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
	static public int Divide__Vector4__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=ScriptRuntime.Vector4.Divide(a1,a2);
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
	static public int Divide__R_Vector4__R_Vector4__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
			checkValueType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Divide(ref a1,ref a2,out a3);
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
	static public int Divide__R_Vector4__Single__O_Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			ScriptRuntime.Vector4 a3;
			ScriptRuntime.Vector4.Divide(ref a1,a2,out a3);
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
			ScriptRuntime.Vector4 a1;
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
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
	static public int op_Multiply__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
	static public int op_Multiply__Vector4__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
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
	static public int op_Multiply__Single__Vector4_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
	static public int op_Division__Vector4__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 a2;
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
	static public int op_Division__Vector4__Single_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
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
			ScriptRuntime.Vector4 self;
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
			ScriptRuntime.Vector4 self;
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
			ScriptRuntime.Vector4 self;
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
			ScriptRuntime.Vector4 self;
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
	static public int get_Z(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Z);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Z(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Z=v;
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
	static public int get_W(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.W);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_W(IntPtr l) {
		try {
			ScriptRuntime.Vector4 self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.W=v;
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
			pushValue(l,ScriptRuntime.Vector4.Zero);
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
			pushValue(l,ScriptRuntime.Vector4.One);
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
			pushValue(l,ScriptRuntime.Vector4.UnitX);
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
			pushValue(l,ScriptRuntime.Vector4.UnitY);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UnitZ(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector4.UnitZ);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_UnitW(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ScriptRuntime.Vector4.UnitW);
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
			ScriptRuntime.Vector4 self;
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
			ScriptRuntime.Vector4 self;
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
		getTypeTable(l,"ScriptRuntime.Vector4");
		addMember(l,ctor_s);
		addMember(l,ctor__Single_s);
		addMember(l,ctor__Vector3__Single_s);
		addMember(l,ctor__Vector2__Single__Single_s);
		addMember(l,ctor__Single__Single__Single__Single_s);
		addMember(l,ToString);
		addMember(l,Equals__Vector4);
		addMember(l,Equals__Object);
		addMember(l,Length);
		addMember(l,LengthSquared);
		addMember(l,Normalize);
		addMember(l,Distance__Vector4__Vector4_s);
		addMember(l,Distance__R_Vector4__R_Vector4__O_Single_s);
		addMember(l,DistanceSquared__Vector4__Vector4_s);
		addMember(l,DistanceSquared__R_Vector4__R_Vector4__O_Single_s);
		addMember(l,Dot__Vector4__Vector4_s);
		addMember(l,Dot__R_Vector4__R_Vector4__O_Single_s);
		addMember(l,Normalize__Vector4_s);
		addMember(l,Normalize__R_Vector4__O_Vector4_s);
		addMember(l,Min__Vector4__Vector4_s);
		addMember(l,Min__R_Vector4__R_Vector4__O_Vector4_s);
		addMember(l,Max__Vector4__Vector4_s);
		addMember(l,Max__R_Vector4__R_Vector4__O_Vector4_s);
		addMember(l,Clamp__Vector4__Vector4__Vector4_s);
		addMember(l,Clamp__R_Vector4__R_Vector4__R_Vector4__O_Vector4_s);
		addMember(l,Lerp__Vector4__Vector4__Single_s);
		addMember(l,Lerp__R_Vector4__R_Vector4__Single__O_Vector4_s);
		addMember(l,SmoothStep__Vector4__Vector4__Single_s);
		addMember(l,SmoothStep__R_Vector4__R_Vector4__Single__O_Vector4_s);
		addMember(l,Hermite__Vector4__Vector4__Vector4__Vector4__Single_s);
		addMember(l,Hermite__R_Vector4__R_Vector4__R_Vector4__R_Vector4__Single__O_Vector4_s);
		addMember(l,Project__Vector4__Vector4_s);
		addMember(l,Project__R_Vector4__R_Vector4__O_Vector4_s);
		addMember(l,Negate__Vector4_s);
		addMember(l,Negate__R_Vector4__O_Vector4_s);
		addMember(l,Add__Vector4__Vector4_s);
		addMember(l,Add__R_Vector4__R_Vector4__O_Vector4_s);
		addMember(l,Sub__Vector4__Vector4_s);
		addMember(l,Sub__R_Vector4__R_Vector4__O_Vector4_s);
		addMember(l,Multiply__Vector4__Vector4_s);
		addMember(l,Multiply__Vector4__Single_s);
		addMember(l,Multiply__R_Vector4__R_Vector4__O_Vector4_s);
		addMember(l,Multiply__R_Vector4__Single__O_Vector4_s);
		addMember(l,Divide__Vector4__Vector4_s);
		addMember(l,Divide__Vector4__Single_s);
		addMember(l,Divide__R_Vector4__R_Vector4__O_Vector4_s);
		addMember(l,Divide__R_Vector4__Single__O_Vector4_s);
		addMember(l,op_UnaryNegation_s);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		addMember(l,op_Addition_s);
		addMember(l,op_Subtraction_s);
		addMember(l,op_Multiply__Vector4__Vector4_s);
		addMember(l,op_Multiply__Vector4__Single_s);
		addMember(l,op_Multiply__Single__Vector4_s);
		addMember(l,op_Division__Vector4__Vector4_s);
		addMember(l,op_Division__Vector4__Single_s);
		addMember(l,getItem);
		addMember(l,setItem);
		addMember(l,"X",get_X,set_X,true);
		addMember(l,"Y",get_Y,set_Y,true);
		addMember(l,"Z",get_Z,set_Z,true);
		addMember(l,"W",get_W,set_W,true);
		addMember(l,"Zero",get_Zero,null,false);
		addMember(l,"One",get_One,null,false);
		addMember(l,"UnitX",get_UnitX,null,false);
		addMember(l,"UnitY",get_UnitY,null,false);
		addMember(l,"UnitZ",get_UnitZ,null,false);
		addMember(l,"UnitW",get_UnitW,null,false);
		createTypeMetatable(l,null, typeof(ScriptRuntime.Vector4),typeof(System.ValueType));
	}
}
