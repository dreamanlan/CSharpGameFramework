using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Helper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.Helper o;
			o=new GameFramework.Helper();
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
	static public int ConvertTo_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			System.Type a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Helper.ConvertTo(a1,a2);
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
	static public int CalcCrc8_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			var ret=GameFramework.Helper.CalcCrc8(a1);
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
	static public int CalcCrc8WithCrcPoly__A_Byte_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			var ret=GameFramework.Helper.CalcCrc8WithCrcPoly(a1);
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
	static public int CalcCrc8WithCrcPoly__Byte__A_Byte_s(IntPtr l) {
		try {
			System.Byte a1;
			checkType(l,1,out a1);
			System.Byte[] a2;
			checkArray(l,2,out a2);
			var ret=GameFramework.Helper.CalcCrc8WithCrcPoly(a1,a2);
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
	static public int BinToHex__A_Byte_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			var ret=GameFramework.Helper.BinToHex(a1);
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
	static public int BinToHex__A_Byte__Int32_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Helper.BinToHex(a1,a2);
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
	static public int BinToHex__A_Byte__Int32__Int32_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			var ret=GameFramework.Helper.BinToHex(a1,a2,a3);
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
	static public int StringIsNullOrEmpty_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Helper.StringIsNullOrEmpty(a1);
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
			var ret=GameFramework.Helper.IsSameFloat(a1,a2);
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
	static public int IsSameVector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 a2;
			checkValueType(l,2,out a2);
			var ret=GameFramework.Helper.IsSameVector3(a1,a2);
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
	static public int IsDifferentMonth_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			var ret=GameFramework.Helper.IsDifferentMonth(a1);
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
	static public int IsDifferentDay__DateTime_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			var ret=GameFramework.Helper.IsDifferentDay(a1);
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
	static public int IsDifferentDay__Double_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Helper.IsDifferentDay(a1);
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
	static public int IsDifferentMinute_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			var ret=GameFramework.Helper.IsDifferentMinute(a1);
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
	static public int IsInterval24Hours_s(IntPtr l) {
		try {
			System.DateTime a1;
			checkValueType(l,1,out a1);
			var ret=GameFramework.Helper.IsInterval24Hours(a1);
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
	static public int LogCallStack_s(IntPtr l) {
		try {
			GameFramework.Helper.LogCallStack();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LogCallStack__Boolean_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			GameFramework.Helper.LogCallStack(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LogInnerException_s(IntPtr l) {
		try {
			System.Exception a1;
			checkType(l,1,out a1);
			System.Text.StringBuilder a2;
			checkType(l,2,out a2);
			GameFramework.Helper.LogInnerException(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Helper");
		addMember(l,ctor_s);
		addMember(l,ConvertTo_s);
		addMember(l,CalcCrc8_s);
		addMember(l,CalcCrc8WithCrcPoly__A_Byte_s);
		addMember(l,CalcCrc8WithCrcPoly__Byte__A_Byte_s);
		addMember(l,BinToHex__A_Byte_s);
		addMember(l,BinToHex__A_Byte__Int32_s);
		addMember(l,BinToHex__A_Byte__Int32__Int32_s);
		addMember(l,StringIsNullOrEmpty_s);
		addMember(l,IsSameFloat_s);
		addMember(l,IsSameVector3_s);
		addMember(l,IsDifferentMonth_s);
		addMember(l,IsDifferentDay__DateTime_s);
		addMember(l,IsDifferentDay__Double_s);
		addMember(l,IsDifferentMinute_s);
		addMember(l,IsInterval24Hours_s);
		addMember(l,LogCallStack_s);
		addMember(l,LogCallStack__Boolean_s);
		addMember(l,LogInnerException_s);
		createTypeMetatable(l,null, typeof(GameFramework.Helper));
	}
}
