﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_LensFlare : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_brightness(IntPtr l) {
		try {
			UnityEngine.LensFlare self=(UnityEngine.LensFlare)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.brightness);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_brightness(IntPtr l) {
		try {
			UnityEngine.LensFlare self=(UnityEngine.LensFlare)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.brightness=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_fadeSpeed(IntPtr l) {
		try {
			UnityEngine.LensFlare self=(UnityEngine.LensFlare)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.fadeSpeed);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_fadeSpeed(IntPtr l) {
		try {
			UnityEngine.LensFlare self=(UnityEngine.LensFlare)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.fadeSpeed=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_color(IntPtr l) {
		try {
			UnityEngine.LensFlare self=(UnityEngine.LensFlare)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.color);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_color(IntPtr l) {
		try {
			UnityEngine.LensFlare self=(UnityEngine.LensFlare)checkSelf(l);
			UnityEngine.Color v;
			checkType(l,2,out v);
			self.color=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_flare(IntPtr l) {
		try {
			UnityEngine.LensFlare self=(UnityEngine.LensFlare)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.flare);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_flare(IntPtr l) {
		try {
			UnityEngine.LensFlare self=(UnityEngine.LensFlare)checkSelf(l);
			UnityEngine.Flare v;
			checkType(l,2,out v);
			self.flare=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.LensFlare");
		addMember(l,"brightness",get_brightness,set_brightness,true);
		addMember(l,"fadeSpeed",get_fadeSpeed,set_fadeSpeed,true);
		addMember(l,"color",get_color,set_color,true);
		addMember(l,"flare",get_flare,set_flare,true);
		createTypeMetatable(l,null, typeof(UnityEngine.LensFlare),typeof(UnityEngine.Behaviour));
	}
}
