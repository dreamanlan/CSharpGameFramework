using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Plugin_IAttrExpressionPlugin : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetCalculator(IntPtr l) {
		try {
			GameFramework.Plugin.IAttrExpressionPlugin self=(GameFramework.Plugin.IAttrExpressionPlugin)checkSelf(l);
			GameFramework.AttrCalc.DslCalculator a1;
			checkType(l,2,out a1);
			self.SetCalculator(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Calc(IntPtr l) {
		try {
			GameFramework.Plugin.IAttrExpressionPlugin self=(GameFramework.Plugin.IAttrExpressionPlugin)checkSelf(l);
			GameFramework.SceneContextInfo a1;
			checkType(l,2,out a1);
			GameFramework.CharacterProperty a2;
			checkType(l,3,out a2);
			GameFramework.CharacterProperty a3;
			checkType(l,4,out a3);
			System.Int64[] a4;
			checkArray(l,5,out a4);
			var ret=self.Calc(a1,a2,a3,a4);
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
	static public int LoadValue(IntPtr l) {
		try {
			GameFramework.Plugin.IAttrExpressionPlugin self=(GameFramework.Plugin.IAttrExpressionPlugin)checkSelf(l);
			Dsl.ValueData a1;
			checkType(l,2,out a1);
			var ret=self.LoadValue(a1);
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
			GameFramework.Plugin.IAttrExpressionPlugin self=(GameFramework.Plugin.IAttrExpressionPlugin)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			var ret=self.LoadCallData(a1);
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
	static public int LoadExpressions(IntPtr l) {
		try {
			GameFramework.Plugin.IAttrExpressionPlugin self=(GameFramework.Plugin.IAttrExpressionPlugin)checkSelf(l);
			GameFramework.AttrCalc.AttrExpressionList a1;
			checkType(l,2,out a1);
			var ret=self.LoadExpressions(a1);
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
	static public int LoadFuncData(IntPtr l) {
		try {
			GameFramework.Plugin.IAttrExpressionPlugin self=(GameFramework.Plugin.IAttrExpressionPlugin)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			var ret=self.LoadFuncData(a1);
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
	static public int LoadStatementData(IntPtr l) {
		try {
			GameFramework.Plugin.IAttrExpressionPlugin self=(GameFramework.Plugin.IAttrExpressionPlugin)checkSelf(l);
			Dsl.StatementData a1;
			checkType(l,2,out a1);
			var ret=self.LoadStatementData(a1);
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
		getTypeTable(l,"GameFramework.Plugin.IAttrExpressionPlugin");
		addMember(l,SetCalculator);
		addMember(l,Calc);
		addMember(l,LoadValue);
		addMember(l,LoadCallData);
		addMember(l,LoadExpressions);
		addMember(l,LoadFuncData);
		addMember(l,LoadStatementData);
		createTypeMetatable(l,null, typeof(GameFramework.Plugin.IAttrExpressionPlugin));
	}
}
