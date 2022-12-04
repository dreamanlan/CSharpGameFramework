﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_EventSystems_EventTrigger_Entry : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UnityEngine.EventSystems.EventTrigger.Entry o;
			o=new UnityEngine.EventSystems.EventTrigger.Entry();
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
	static public int get_eventID(IntPtr l) {
		try {
			UnityEngine.EventSystems.EventTrigger.Entry self=(UnityEngine.EventSystems.EventTrigger.Entry)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.eventID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_eventID(IntPtr l) {
		try {
			UnityEngine.EventSystems.EventTrigger.Entry self=(UnityEngine.EventSystems.EventTrigger.Entry)checkSelf(l);
			UnityEngine.EventSystems.EventTriggerType v;
			checkEnum(l,2,out v);
			self.eventID=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_callback(IntPtr l) {
		try {
			UnityEngine.EventSystems.EventTrigger.Entry self=(UnityEngine.EventSystems.EventTrigger.Entry)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.callback);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_callback(IntPtr l) {
		try {
			UnityEngine.EventSystems.EventTrigger.Entry self=(UnityEngine.EventSystems.EventTrigger.Entry)checkSelf(l);
			UnityEngine.EventSystems.EventTrigger.TriggerEvent v;
			checkType(l,2,out v);
			self.callback=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.EventSystems.EventTrigger.Entry");
		addMember(l,ctor_s);
		addMember(l,"eventID",get_eventID,set_eventID,true);
		addMember(l,"callback",get_callback,set_callback,true);
		createTypeMetatable(l,null, typeof(UnityEngine.EventSystems.EventTrigger.Entry));
	}
}
