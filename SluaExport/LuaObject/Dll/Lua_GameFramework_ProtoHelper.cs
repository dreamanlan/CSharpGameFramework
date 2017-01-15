using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ProtoHelper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.ProtoHelper o;
			o=new GameFramework.ProtoHelper();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EncodeFloat_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=GameFramework.ProtoHelper.EncodeFloat(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DecodeFloat_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=GameFramework.ProtoHelper.DecodeFloat(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EncodePosition2D_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=GameFramework.ProtoHelper.EncodePosition2D(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DecodePosition2D_s(IntPtr l) {
		try {
			System.UInt64 a1;
			checkType(l,1,out a1);
			System.Single a2;
			System.Single a3;
			GameFramework.ProtoHelper.DecodePosition2D(a1,out a2,out a3);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EncodePosition3D_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=GameFramework.ProtoHelper.EncodePosition3D(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DecodePosition3D_s(IntPtr l) {
		try {
			System.UInt64 a1;
			checkType(l,1,out a1);
			System.Single a2;
			System.Single a3;
			System.Single a4;
			GameFramework.ProtoHelper.DecodePosition3D(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ProtoHelper");
		addMember(l,EncodeFloat_s);
		addMember(l,DecodeFloat_s);
		addMember(l,EncodePosition2D_s);
		addMember(l,DecodePosition2D_s);
		addMember(l,EncodePosition3D_s);
		addMember(l,DecodePosition3D_s);
		createTypeMetatable(l,constructor, typeof(GameFramework.ProtoHelper));
	}
}
