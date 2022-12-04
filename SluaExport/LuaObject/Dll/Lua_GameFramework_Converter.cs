using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Converter : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IntList2String_s(IntPtr l) {
		try {
			System.Int32[] a1;
			checkParams(l,1,out a1);
			var ret=GameFramework.Converter.IntList2String(a1);
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
	static public int FloatList2String_s(IntPtr l) {
		try {
			System.Single[] a1;
			checkParams(l,1,out a1);
			var ret=GameFramework.Converter.FloatList2String(a1);
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
	static public int BoolList2String_s(IntPtr l) {
		try {
			System.Boolean[] a1;
			checkParams(l,1,out a1);
			var ret=GameFramework.Converter.BoolList2String(a1);
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
	static public int StringList2String_s(IntPtr l) {
		try {
			System.String[] a1;
			checkParams(l,1,out a1);
			var ret=GameFramework.Converter.StringList2String(a1);
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
	static public int ConvertIntList_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertIntList(a1);
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
	static public int ConvertUintList_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertUintList(a1);
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
	static public int ConvertFloatList_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertFloatList(a1);
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
	static public int ConvertDoubleList_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertDoubleList(a1);
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
	static public int ConvertBoolList_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertBoolList(a1);
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
	static public int ConvertStringList_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertStringList(a1);
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
	static public int ConvertVector2D_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertVector2D(a1);
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
	static public int ConvertVector3D_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertVector3D(a1);
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
	static public int ConvertVector2DList_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertVector2DList(a1);
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
	static public int ConvertVector3DList_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Converter.ConvertVector3DList(a1);
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
	static public int CastArgsForCall_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.Reflection.BindingFlags a3;
			checkEnum(l,3,out a3);
			System.Object[] a4;
			checkParams(l,4,out a4);
			GameFramework.Converter.CastArgsForCall(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CastArgsForSet_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.Reflection.BindingFlags a3;
			checkEnum(l,3,out a3);
			System.Object[] a4;
			checkParams(l,4,out a4);
			GameFramework.Converter.CastArgsForSet(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CastArgsForGet_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.Reflection.BindingFlags a3;
			checkEnum(l,3,out a3);
			System.Object[] a4;
			checkParams(l,4,out a4);
			GameFramework.Converter.CastArgsForGet(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CastTo_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Converter.CastTo(a1,a2);
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
	static public int FileContent2Utf8String_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			var ret=GameFramework.Converter.FileContent2Utf8String(a1);
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
		getTypeTable(l,"GameFramework.Converter");
		addMember(l,IntList2String_s);
		addMember(l,FloatList2String_s);
		addMember(l,BoolList2String_s);
		addMember(l,StringList2String_s);
		addMember(l,ConvertIntList_s);
		addMember(l,ConvertUintList_s);
		addMember(l,ConvertFloatList_s);
		addMember(l,ConvertDoubleList_s);
		addMember(l,ConvertBoolList_s);
		addMember(l,ConvertStringList_s);
		addMember(l,ConvertVector2D_s);
		addMember(l,ConvertVector3D_s);
		addMember(l,ConvertVector2DList_s);
		addMember(l,ConvertVector3DList_s);
		addMember(l,CastArgsForCall_s);
		addMember(l,CastArgsForSet_s);
		addMember(l,CastArgsForGet_s);
		addMember(l,CastTo_s);
		addMember(l,FileContent2Utf8String_s);
		createTypeMetatable(l,null, typeof(GameFramework.Converter));
	}
}
