using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Plugin_IStoryCommandPlugin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ResetState(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryCommandPlugin self=(GameFramework.Plugin.IStoryCommandPlugin)checkSelf(l);
			self.ResetState();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Evaluate(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryCommandPlugin self=(GameFramework.Plugin.IStoryCommandPlugin)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			System.Object[] a3;
			checkArray(l,4,out a3);
			self.Evaluate(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExecCommand(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryCommandPlugin self=(GameFramework.Plugin.IStoryCommandPlugin)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			var ret=self.ExecCommand(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExecCommandWithArgs(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryCommandPlugin self=(GameFramework.Plugin.IStoryCommandPlugin)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			System.Object a3;
			checkType(l,4,out a3);
			System.Object[] a4;
			checkArray(l,5,out a4);
			var ret=self.ExecCommandWithArgs(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadCallData(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryCommandPlugin self=(GameFramework.Plugin.IStoryCommandPlugin)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			self.LoadCallData(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadFuncData(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryCommandPlugin self=(GameFramework.Plugin.IStoryCommandPlugin)checkSelf(l);
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
	static public int LoadStatementData(IntPtr l) {
		try {
			GameFramework.Plugin.IStoryCommandPlugin self=(GameFramework.Plugin.IStoryCommandPlugin)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Plugin.IStoryCommandPlugin");
		addMember(l,ResetState);
		addMember(l,Evaluate);
		addMember(l,ExecCommand);
		addMember(l,ExecCommandWithArgs);
		addMember(l,LoadCallData);
		addMember(l,LoadFuncData);
		addMember(l,LoadStatementData);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.IStoryCommandPlugin));
	}
}
