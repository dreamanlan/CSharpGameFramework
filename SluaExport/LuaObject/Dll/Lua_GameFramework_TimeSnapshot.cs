using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_TimeSnapshot : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.TimeSnapshot o;
			o=new GameFramework.TimeSnapshot();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start_s(IntPtr l) {
		try {
			GameFramework.TimeSnapshot.Start();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int End_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeSnapshot.End();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DoCheckPoint_s(IntPtr l) {
		try {
			var ret=GameFramework.TimeSnapshot.DoCheckPoint();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.TimeSnapshot");
		addMember(l,Start_s);
		addMember(l,End_s);
		addMember(l,DoCheckPoint_s);
		createTypeMetatable(l,constructor, typeof(GameFramework.TimeSnapshot));
	}
}
