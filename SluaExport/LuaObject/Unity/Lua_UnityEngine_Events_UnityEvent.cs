﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Events_UnityEvent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UnityEngine.Events.UnityEvent o;
			o=new UnityEngine.Events.UnityEvent();
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
	static public int AddListener(IntPtr l) {
		try {
			UnityEngine.Events.UnityEvent self=(UnityEngine.Events.UnityEvent)checkSelf(l);
			UnityEngine.Events.UnityAction a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.AddListener(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveListener(IntPtr l) {
		try {
			UnityEngine.Events.UnityEvent self=(UnityEngine.Events.UnityEvent)checkSelf(l);
			UnityEngine.Events.UnityAction a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.RemoveListener(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Invoke(IntPtr l) {
		try {
			UnityEngine.Events.UnityEvent self=(UnityEngine.Events.UnityEvent)checkSelf(l);
			self.Invoke();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.Events.UnityEvent");
		addMember(l,ctor_s);
		addMember(l,AddListener);
		addMember(l,RemoveListener);
		addMember(l,Invoke);
		createTypeMetatable(l,null, typeof(UnityEngine.Events.UnityEvent),typeof(UnityEngine.Events.UnityEventBase));
	}
}
