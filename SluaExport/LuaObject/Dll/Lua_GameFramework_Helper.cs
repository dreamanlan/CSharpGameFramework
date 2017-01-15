using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Helper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int CalcCrc8WithCrcPoly_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				var ret=GameFramework.Helper.CalcCrc8WithCrcPoly(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.Byte a1;
				checkType(l,1,out a1);
				System.Byte[] a2;
				checkArray(l,2,out a2);
				var ret=GameFramework.Helper.CalcCrc8WithCrcPoly(a1,a2);
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
	static public int BinToHex_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				var ret=GameFramework.Helper.BinToHex(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.Byte[] a1;
				checkArray(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=GameFramework.Helper.BinToHex(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int IsDifferentDay_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.DateTime))){
				System.DateTime a1;
				checkValueType(l,1,out a1);
				var ret=GameFramework.Helper.IsDifferentDay(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(double))){
				System.Double a1;
				checkType(l,1,out a1);
				var ret=GameFramework.Helper.IsDifferentDay(a1);
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
	static public int LogCallStack_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==0){
				GameFramework.Helper.LogCallStack();
				pushValue(l,true);
				return 1;
			}
			else if(argc==1){
				System.Boolean a1;
				checkType(l,1,out a1);
				GameFramework.Helper.LogCallStack(a1);
				pushValue(l,true);
				return 1;
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
		getTypeTable(l,"GameFramework.Helper");
		addMember(l,CalcCrc8_s);
		addMember(l,CalcCrc8WithCrcPoly_s);
		addMember(l,BinToHex_s);
		addMember(l,StringIsNullOrEmpty_s);
		addMember(l,IsSameFloat_s);
		addMember(l,IsSameVector3_s);
		addMember(l,IsDifferentMonth_s);
		addMember(l,IsDifferentDay_s);
		addMember(l,IsDifferentMinute_s);
		addMember(l,IsInterval24Hours_s);
		addMember(l,LogCallStack_s);
		createTypeMetatable(l,constructor, typeof(GameFramework.Helper));
	}
}
