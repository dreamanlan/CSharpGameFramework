﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_LC_PublishEvent_EventArg : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_PublishEvent.EventArg o;
			o=new GameFrameworkMessage.Msg_LC_PublishEvent.EventArg();
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
	static public int get_val_type(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_PublishEvent.EventArg self=(GameFrameworkMessage.Msg_LC_PublishEvent.EventArg)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.val_type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_val_type(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_PublishEvent.EventArg self=(GameFrameworkMessage.Msg_LC_PublishEvent.EventArg)checkSelf(l);
			GameFrameworkMessage.LobbyArgType v;
			checkEnum(l,2,out v);
			self.val_type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_str_val(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_PublishEvent.EventArg self=(GameFrameworkMessage.Msg_LC_PublishEvent.EventArg)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.str_val);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_str_val(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_PublishEvent.EventArg self=(GameFrameworkMessage.Msg_LC_PublishEvent.EventArg)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.str_val=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_PublishEvent.EventArg");
		addMember(l,ctor_s);
		addMember(l,"val_type",get_val_type,set_val_type,true);
		addMember(l,"str_val",get_str_val,set_str_val,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_LC_PublishEvent.EventArg));
	}
}
