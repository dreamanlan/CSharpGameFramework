using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_DataRecordUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int ExtractIntList_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractIntList(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int ExtractFloatList_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Single[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractFloatList(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int ExtractStringList_s(IntPtr l) {
		try {
			GameFramework.BinaryTable a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.String[] a3;
			checkArray(l,3,out a3);
			var ret=GameFramework.DataRecordUtility.ExtractStringList(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int SetValue_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(System.Single[]),typeof(System.Single[]))){
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
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(List<System.Int32>),typeof(System.Int32[]))){
				GameFramework.BinaryTable a1;
				checkType(l,1,out a1);
				System.Collections.Generic.List<System.Int32> a2;
				checkType(l,2,out a2);
				System.Int32[] a3;
				checkArray(l,3,out a3);
				var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(List<System.Single>),typeof(System.Single[]))){
				GameFramework.BinaryTable a1;
				checkType(l,1,out a1);
				System.Collections.Generic.List<System.Single> a2;
				checkType(l,2,out a2);
				System.Single[] a3;
				checkArray(l,3,out a3);
				var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(List<System.String>),typeof(System.String[]))){
				GameFramework.BinaryTable a1;
				checkType(l,1,out a1);
				System.Collections.Generic.List<System.String> a2;
				checkType(l,2,out a2);
				System.String[] a3;
				checkArray(l,3,out a3);
				var ret=GameFramework.DataRecordUtility.SetValue(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(System.String[]),typeof(System.String[]))){
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
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(System.Int32[]),typeof(System.Int32[]))){
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
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(int),typeof(int))){
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
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(bool),typeof(bool))){
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
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(System.Int64),typeof(System.Int64))){
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
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(string),typeof(string))){
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
			else if(matchType(l,argc,1,typeof(GameFramework.BinaryTable),typeof(float),typeof(float))){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.DataRecordUtility");
		addMember(l,ExtractInt_s);
		addMember(l,ExtractLong_s);
		addMember(l,ExtractFloat_s);
		addMember(l,ExtractBool_s);
		addMember(l,ExtractString_s);
		addMember(l,ExtractIntList_s);
		addMember(l,ExtractIntArray_s);
		addMember(l,ExtractFloatList_s);
		addMember(l,ExtractFloatArray_s);
		addMember(l,ExtractStringList_s);
		addMember(l,ExtractStringArray_s);
		addMember(l,SetValue_s);
		createTypeMetatable(l,constructor, typeof(GameFramework.DataRecordUtility));
	}
}
