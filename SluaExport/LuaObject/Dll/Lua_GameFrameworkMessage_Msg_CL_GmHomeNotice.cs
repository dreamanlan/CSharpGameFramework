﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_CL_GmHomeNotice : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmHomeNotice o;
			o=new GameFrameworkMessage.Msg_CL_GmHomeNotice();
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
	static public int get_m_Content(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmHomeNotice self=(GameFrameworkMessage.Msg_CL_GmHomeNotice)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Content);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Content(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_GmHomeNotice self=(GameFrameworkMessage.Msg_CL_GmHomeNotice)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Content=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_GmHomeNotice");
		addMember(l,ctor_s);
		addMember(l,"m_Content",get_m_Content,set_m_Content,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_CL_GmHomeNotice));
	}
}
