using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_GameControler : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.GameControler o;
			o=new GameFramework.GameControler();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			GameFramework.GameControler.Init(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitGame_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			GameFramework.GameControler.InitGame(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PauseGame_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			GameFramework.GameControler.PauseGame(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PauseGameForeground_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			GameFramework.GameControler.PauseGameForeground(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Release_s(IntPtr l) {
		try {
			GameFramework.GameControler.Release();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TickGame_s(IntPtr l) {
		try {
			GameFramework.GameControler.TickGame();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsInited(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.GameControler.IsInited);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.GameControler");
		addMember(l,Init_s);
		addMember(l,InitGame_s);
		addMember(l,PauseGame_s);
		addMember(l,PauseGameForeground_s);
		addMember(l,Release_s);
		addMember(l,TickGame_s);
		addMember(l,"IsInited",get_IsInited,null,false);
		createTypeMetatable(l,constructor, typeof(GameFramework.GameControler));
	}
}
