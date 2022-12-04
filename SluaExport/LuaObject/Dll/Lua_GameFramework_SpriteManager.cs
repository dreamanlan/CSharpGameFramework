﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_SpriteManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.SpriteManager o;
			o=new GameFramework.SpriteManager();
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
	static public int Init_s(IntPtr l) {
		try {
			GameFramework.SpriteManager.Init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetActorIcon_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=GameFramework.SpriteManager.GetActorIcon(a1);
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
	static public int GetActorBigIcon_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=GameFramework.SpriteManager.GetActorBigIcon(a1);
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
	static public int GetSkillIcon_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=GameFramework.SpriteManager.GetSkillIcon(a1);
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
	static public int GetActorIconIndex_s(IntPtr l) {
		try {
			UnityEngine.Sprite a1;
			checkType(l,1,out a1);
			var ret=GameFramework.SpriteManager.GetActorIconIndex(a1);
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
	static public int GetActorBigIconIndex_s(IntPtr l) {
		try {
			UnityEngine.Sprite a1;
			checkType(l,1,out a1);
			var ret=GameFramework.SpriteManager.GetActorBigIconIndex(a1);
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
	static public int GetSkillIconIndex_s(IntPtr l) {
		try {
			UnityEngine.Sprite a1;
			checkType(l,1,out a1);
			var ret=GameFramework.SpriteManager.GetSkillIconIndex(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SpriteManager");
		addMember(l,ctor_s);
		addMember(l,Init_s);
		addMember(l,GetActorIcon_s);
		addMember(l,GetActorBigIcon_s);
		addMember(l,GetSkillIcon_s);
		addMember(l,GetActorIconIndex_s);
		addMember(l,GetActorBigIconIndex_s);
		addMember(l,GetSkillIconIndex_s);
		createTypeMetatable(l,null, typeof(GameFramework.SpriteManager));
	}
}
