﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_SleepTimeout : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UnityEngine.SleepTimeout o;
			o=new UnityEngine.SleepTimeout();
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
	static public int get_NeverSleep(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UnityEngine.SleepTimeout.NeverSleep);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SystemSetting(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UnityEngine.SleepTimeout.SystemSetting);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.SleepTimeout");
		addMember(l,ctor_s);
		addMember(l,"NeverSleep",get_NeverSleep,null,false);
		addMember(l,"SystemSetting",get_SystemSetting,null,false);
		createTypeMetatable(l,null, typeof(UnityEngine.SleepTimeout));
	}
}
