﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_TimeSnapshot : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.TimeSnapshot");
		addMember(l,ctor_s);
		addMember(l,Start_s);
		addMember(l,End_s);
		addMember(l,DoCheckPoint_s);
		createTypeMetatable(l,null, typeof(GameFramework.TimeSnapshot));
	}
}
