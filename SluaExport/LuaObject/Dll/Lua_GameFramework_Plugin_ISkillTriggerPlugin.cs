using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Plugin_ISkillTriggerPlugin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetProxy(IntPtr l) {
		try {
			GameFramework.Plugin.ISkillTriggerPlugin self=(GameFramework.Plugin.ISkillTriggerPlugin)checkSelf(l);
			SkillSystem.SkillTriggerProxy a1;
			checkType(l,2,out a1);
			self.SetProxy(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Clone(IntPtr l) {
		try {
			GameFramework.Plugin.ISkillTriggerPlugin self=(GameFramework.Plugin.ISkillTriggerPlugin)checkSelf(l);
			var ret=self.Clone();
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
	static public int Reset(IntPtr l) {
		try {
			GameFramework.Plugin.ISkillTriggerPlugin self=(GameFramework.Plugin.ISkillTriggerPlugin)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Execute(IntPtr l) {
		try {
			GameFramework.Plugin.ISkillTriggerPlugin self=(GameFramework.Plugin.ISkillTriggerPlugin)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			System.Int64 a4;
			checkType(l,5,out a4);
			var ret=self.Execute(a1,a2,a3,a4);
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
	static public int LoadCallData(IntPtr l) {
		try {
			GameFramework.Plugin.ISkillTriggerPlugin self=(GameFramework.Plugin.ISkillTriggerPlugin)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			self.LoadCallData(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadFuncData(IntPtr l) {
		try {
			GameFramework.Plugin.ISkillTriggerPlugin self=(GameFramework.Plugin.ISkillTriggerPlugin)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			self.LoadFuncData(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadStatementData(IntPtr l) {
		try {
			GameFramework.Plugin.ISkillTriggerPlugin self=(GameFramework.Plugin.ISkillTriggerPlugin)checkSelf(l);
			Dsl.StatementData a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			self.LoadStatementData(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnInitProperties(IntPtr l) {
		try {
			GameFramework.Plugin.ISkillTriggerPlugin self=(GameFramework.Plugin.ISkillTriggerPlugin)checkSelf(l);
			self.OnInitProperties();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.ISkillTriggerPlugin");
		addMember(l,SetProxy);
		addMember(l,Clone);
		addMember(l,Reset);
		addMember(l,Execute);
		addMember(l,LoadCallData);
		addMember(l,LoadFuncData);
		addMember(l,LoadStatementData);
		addMember(l,OnInitProperties);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.ISkillTriggerPlugin));
	}
}
