﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_UI_LayoutRebuilder : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UnityEngine.UI.LayoutRebuilder o;
			o=new UnityEngine.UI.LayoutRebuilder();
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
	static public int IsDestroyed(IntPtr l) {
		try {
			UnityEngine.UI.LayoutRebuilder self=(UnityEngine.UI.LayoutRebuilder)checkSelf(l);
			var ret=self.IsDestroyed();
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
	static public int Rebuild(IntPtr l) {
		try {
			UnityEngine.UI.LayoutRebuilder self=(UnityEngine.UI.LayoutRebuilder)checkSelf(l);
			UnityEngine.UI.CanvasUpdate a1;
			checkEnum(l,2,out a1);
			self.Rebuild(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LayoutComplete(IntPtr l) {
		try {
			UnityEngine.UI.LayoutRebuilder self=(UnityEngine.UI.LayoutRebuilder)checkSelf(l);
			self.LayoutComplete();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GraphicUpdateComplete(IntPtr l) {
		try {
			UnityEngine.UI.LayoutRebuilder self=(UnityEngine.UI.LayoutRebuilder)checkSelf(l);
			self.GraphicUpdateComplete();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static new public int Equals(IntPtr l) {
		try {
			UnityEngine.UI.LayoutRebuilder self=(UnityEngine.UI.LayoutRebuilder)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static new public int ToString(IntPtr l) {
		try {
			UnityEngine.UI.LayoutRebuilder self=(UnityEngine.UI.LayoutRebuilder)checkSelf(l);
			var ret=self.ToString();
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
	static public int ForceRebuildLayoutImmediate_s(IntPtr l) {
		try {
			UnityEngine.RectTransform a1;
			checkType(l,1,out a1);
			UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MarkLayoutForRebuild_s(IntPtr l) {
		try {
			UnityEngine.RectTransform a1;
			checkType(l,1,out a1);
			UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_transform(IntPtr l) {
		try {
			UnityEngine.UI.LayoutRebuilder self=(UnityEngine.UI.LayoutRebuilder)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.transform);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.UI.LayoutRebuilder");
		addMember(l,ctor_s);
		addMember(l,IsDestroyed);
		addMember(l,Rebuild);
		addMember(l,LayoutComplete);
		addMember(l,GraphicUpdateComplete);
		addMember(l,Equals);
		addMember(l,ToString);
		addMember(l,ForceRebuildLayoutImmediate_s);
		addMember(l,MarkLayoutForRebuild_s);
		addMember(l,"transform",get_transform,null,true);
		createTypeMetatable(l,null, typeof(UnityEngine.UI.LayoutRebuilder));
	}
}
