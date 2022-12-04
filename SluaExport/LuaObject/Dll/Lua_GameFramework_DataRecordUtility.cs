using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_DataRecordUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.DataRecordUtility o;
			o=new GameFramework.DataRecordUtility();
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
	static public int ExtractInt_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractInt(a1,a2,a3);
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
	static public int ExtractLong_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int64 a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractLong(a1,a2,a3);
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
	static public int ExtractFloat_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractFloat(a1,a2,a3);
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
	static public int ExtractBool_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Boolean a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractBool(a1,a2,a3);
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
	static public int ExtractString_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractString(a1,a2,a3);
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
	static public int ExtractIntArray_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractIntArray(a1,a2,a3);
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
	static public int ExtractFloatArray_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Single[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractFloatArray(a1,a2,a3);
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
	static public int ExtractStringArray_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.String[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractStringArray(a1,a2,a3);
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
	static public int SetValue__BinaryTable__Boolean__Boolean_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			System.Boolean a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
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
	static public int SetValue__BinaryTable__Int32__Int32_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
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
	static public int SetValue__BinaryTable__Int64__Int64_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int64 a2;
			checkType(l,2,out a2);
			System.Int64 a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
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
	static public int SetValue__BinaryTable__String__String_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
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
	static public int SetValue__BinaryTable__A_Int32__A_Int32_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32[] a2;
			checkArray(l,2,out a2);
			System.Int32[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
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
	static public int SetValue__BinaryTable__A_Single__A_Single_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Single[] a2;
			checkArray(l,2,out a2);
			System.Single[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
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
	static public int SetValue__BinaryTable__A_String__A_String_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.String[] a2;
			checkArray(l,2,out a2);
			System.String[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
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
	static public int SetValue__BinaryTable__Single__Single_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.DataRecordUtility");
		addMember(l,ctor_s);
		addMember(l,ExtractInt_s);
		addMember(l,ExtractLong_s);
		addMember(l,ExtractFloat_s);
		addMember(l,ExtractBool_s);
		addMember(l,ExtractString_s);
		addMember(l,ExtractIntArray_s);
		addMember(l,ExtractFloatArray_s);
		addMember(l,ExtractStringArray_s);
		addMember(l,SetValue__BinaryTable__Boolean__Boolean_s);
		addMember(l,SetValue__BinaryTable__Int32__Int32_s);
		addMember(l,SetValue__BinaryTable__Int64__Int64_s);
		addMember(l,SetValue__BinaryTable__String__String_s);
		addMember(l,SetValue__BinaryTable__A_Int32__A_Int32_s);
		addMember(l,SetValue__BinaryTable__A_Single__A_Single_s);
		addMember(l,SetValue__BinaryTable__A_String__A_String_s);
		addMember(l,SetValue__BinaryTable__Single__Single_s);
		createTypeMetatable(l,null, typeof(GameFramework.DataRecordUtility));
	}
}
