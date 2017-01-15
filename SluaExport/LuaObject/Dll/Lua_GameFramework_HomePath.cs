using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_HomePath : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.HomePath o;
			o=new GameFramework.HomePath();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitHomePath_s(IntPtr l) {
		try {
			GameFramework.HomePath.InitHomePath();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAbsolutePath_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.HomePath.GetAbsolutePath(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CurHomePath(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.HomePath.CurHomePath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_CurHomePath(IntPtr l) {
		try {
			string v;
			checkType(l,2,out v);
			GameFramework.HomePath.CurHomePath=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.HomePath");
		addMember(l,InitHomePath_s);
		addMember(l,GetAbsolutePath_s);
		addMember(l,"CurHomePath",get_CurHomePath,set_CurHomePath,false);
		createTypeMetatable(l,constructor, typeof(GameFramework.HomePath));
	}
}
