﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_NodeRegisterResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.NodeRegisterResult o;
			o=new GameFrameworkMessage.NodeRegisterResult();
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
	static public int get_m_IsOk(IntPtr l) {
		try {
			GameFrameworkMessage.NodeRegisterResult self=(GameFrameworkMessage.NodeRegisterResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_IsOk);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_IsOk(IntPtr l) {
		try {
			GameFrameworkMessage.NodeRegisterResult self=(GameFrameworkMessage.NodeRegisterResult)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.m_IsOk=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.NodeRegisterResult");
		addMember(l,ctor_s);
		addMember(l,"m_IsOk",get_m_IsOk,set_m_IsOk,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.NodeRegisterResult));
	}
}
