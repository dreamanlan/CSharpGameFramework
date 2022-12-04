﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_DefaultExecutionOrder : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UnityEngine.DefaultExecutionOrder o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new UnityEngine.DefaultExecutionOrder(a1);
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
	static public int get_order(IntPtr l) {
		try {
			UnityEngine.DefaultExecutionOrder self=(UnityEngine.DefaultExecutionOrder)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.order);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.DefaultExecutionOrder");
		addMember(l,ctor_s);
		addMember(l,"order",get_order,null,true);
		createTypeMetatable(l,null, typeof(UnityEngine.DefaultExecutionOrder),typeof(System.Attribute));
	}
}
