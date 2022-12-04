﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_TableConfig_StoryDlgProvider : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			TableConfig.StoryDlgProvider o;
			o=new TableConfig.StoryDlgProvider();
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
	static public int LoadForClient(IntPtr l) {
		try {
			TableConfig.StoryDlgProvider self=(TableConfig.StoryDlgProvider)checkSelf(l);
			self.LoadForClient();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Load(IntPtr l) {
		try {
			TableConfig.StoryDlgProvider self=(TableConfig.StoryDlgProvider)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Load(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Save(IntPtr l) {
		try {
			TableConfig.StoryDlgProvider self=(TableConfig.StoryDlgProvider)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Save(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clear(IntPtr l) {
		try {
			TableConfig.StoryDlgProvider self=(TableConfig.StoryDlgProvider)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetStoryDlgCount(IntPtr l) {
		try {
			TableConfig.StoryDlgProvider self=(TableConfig.StoryDlgProvider)checkSelf(l);
			var ret=self.GetStoryDlgCount();
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
	static public int GetStoryDlg(IntPtr l) {
		try {
			TableConfig.StoryDlgProvider self=(TableConfig.StoryDlgProvider)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetStoryDlg(a1);
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
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,TableConfig.StoryDlgProvider.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.StoryDlgProvider");
		addMember(l,ctor_s);
		addMember(l,LoadForClient);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,Clear);
		addMember(l,GetStoryDlgCount);
		addMember(l,GetStoryDlg);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(TableConfig.StoryDlgProvider));
	}
}
