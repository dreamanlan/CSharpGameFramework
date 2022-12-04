﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_CL_GmForbidChat : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmForbidChat o;
			o=new GameFrameworkMessage.Msg_CL_GmForbidChat();
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
	static public int get_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmForbidChat self=(GameFrameworkMessage.Msg_CL_GmForbidChat)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Nickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmForbidChat self=(GameFrameworkMessage.Msg_CL_GmForbidChat)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Nickname=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_IsForbid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmForbidChat self=(GameFrameworkMessage.Msg_CL_GmForbidChat)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_IsForbid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_IsForbid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmForbidChat self=(GameFrameworkMessage.Msg_CL_GmForbidChat)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.m_IsForbid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_GmForbidChat");
		addMember(l,ctor_s);
		addMember(l,"m_Nickname",get_m_Nickname,set_m_Nickname,true);
		addMember(l,"m_IsForbid",get_m_IsForbid,set_m_IsForbid,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_CL_GmForbidChat));
	}
}
