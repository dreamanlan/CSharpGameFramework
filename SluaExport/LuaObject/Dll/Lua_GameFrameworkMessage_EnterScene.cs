﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_EnterScene : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.EnterScene o;
			o=new GameFrameworkMessage.EnterScene();
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
	static public int get_m_SceneId(IntPtr l) {
		try {
			GameFrameworkMessage.EnterScene self=(GameFrameworkMessage.EnterScene)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_SceneId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_SceneId(IntPtr l) {
		try {
			GameFrameworkMessage.EnterScene self=(GameFrameworkMessage.EnterScene)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_SceneId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_RoomId(IntPtr l) {
		try {
			GameFrameworkMessage.EnterScene self=(GameFrameworkMessage.EnterScene)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_RoomId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_RoomId(IntPtr l) {
		try {
			GameFrameworkMessage.EnterScene self=(GameFrameworkMessage.EnterScene)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_RoomId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.EnterScene");
		addMember(l,ctor_s);
		addMember(l,"m_SceneId",get_m_SceneId,set_m_SceneId,true);
		addMember(l,"m_RoomId",get_m_RoomId,set_m_RoomId,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.EnterScene));
	}
}
