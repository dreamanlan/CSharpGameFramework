using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Plugin_IStoryValuePlugin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetProxy(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryValuePlugin self=(GameFramework.Plugin.IStoryValuePlugin)checkSelf(l);
			StorySystem.StoryValueResult a1;
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
			GameFramework.Plugin.IStoryValuePlugin self=(GameFramework.Plugin.IStoryValuePlugin)checkSelf(l);
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
	static public int Evaluate(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryValuePlugin self=(GameFramework.Plugin.IStoryValuePlugin)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryMessageHandler a2;
			checkType(l,3,out a2);
			BoxedValue a3;
			checkValueType(l,4,out a3);
			BoxedValueList a4;
			checkType(l,5,out a4);
			self.Evaluate(a1,a2,a3,a4);
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
			GameFramework.Plugin.IStoryValuePlugin self=(GameFramework.Plugin.IStoryValuePlugin)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			self.LoadFuncData(a1);
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
			GameFramework.Plugin.IStoryValuePlugin self=(GameFramework.Plugin.IStoryValuePlugin)checkSelf(l);
			Dsl.StatementData a1;
			checkType(l,2,out a1);
			self.LoadStatementData(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.IStoryValuePlugin");
		addMember(l,SetProxy);
		addMember(l,Clone);
		addMember(l,Evaluate);
		addMember(l,LoadFuncData);
		addMember(l,LoadStatementData);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.IStoryValuePlugin));
	}
}
