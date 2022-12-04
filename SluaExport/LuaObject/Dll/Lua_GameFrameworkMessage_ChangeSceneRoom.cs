﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_ChangeSceneRoom : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.ChangeSceneRoom o;
			o=new GameFrameworkMessage.ChangeSceneRoom();
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
			GameFrameworkMessage.ChangeSceneRoom self=(GameFrameworkMessage.ChangeSceneRoom)checkSelf(l);
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
			GameFrameworkMessage.ChangeSceneRoom self=(GameFrameworkMessage.ChangeSceneRoom)checkSelf(l);
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
			GameFrameworkMessage.ChangeSceneRoom self=(GameFrameworkMessage.ChangeSceneRoom)checkSelf(l);
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
			GameFrameworkMessage.ChangeSceneRoom self=(GameFrameworkMessage.ChangeSceneRoom)checkSelf(l);
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
		getTypeTable(l,"GameFrameworkMessage.ChangeSceneRoom");
		addMember(l,ctor_s);
		addMember(l,"m_SceneId",get_m_SceneId,set_m_SceneId,true);
		addMember(l,"m_RoomId",get_m_RoomId,set_m_RoomId,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.ChangeSceneRoom));
	}
}
