﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryValueHelper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CastTo_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=StorySystem.StoryValueHelper.CastTo(a1,a2);
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
	static public int get_c_MaxWaitCommandTime(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValueHelper.c_MaxWaitCommandTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryValueHelper");
		addMember(l,CastTo_s);
		addMember(l,"c_MaxWaitCommandTime",get_c_MaxWaitCommandTime,null,false);
		createTypeMetatable(l,null, typeof(StorySystem.StoryValueHelper));
	}
}
