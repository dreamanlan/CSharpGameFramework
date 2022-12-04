﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_AttrCalc_DslCalculator : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.AttrCalc.DslCalculator o;
			o=new GameFramework.AttrCalc.DslCalculator();
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
	static public int Load(IntPtr l) {
		try {
			GameFramework.AttrCalc.DslCalculator self=(GameFramework.AttrCalc.DslCalculator)checkSelf(l);
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
	static public int Calc(IntPtr l) {
		try {
			GameFramework.AttrCalc.DslCalculator self=(GameFramework.AttrCalc.DslCalculator)checkSelf(l);
			GameFramework.SceneContextInfo a1;
			checkType(l,2,out a1);
			GameFramework.CharacterProperty a2;
			checkType(l,3,out a2);
			GameFramework.CharacterProperty a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.Int64[] a5;
			checkParams(l,6,out a5);
			var ret=self.Calc(a1,a2,a3,a4,a5);
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
	static public int Register_s(IntPtr l) {
		try {
			GameFramework.AttrCalc.DslCalculator.Register();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Register__String__IAttrExpressionFactory_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			GameFramework.AttrCalc.IAttrExpressionFactory a2;
			checkType(l,2,out a2);
			GameFramework.AttrCalc.DslCalculator.Register(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AttrCalc.DslCalculator");
		addMember(l,ctor_s);
		addMember(l,Load);
		addMember(l,Calc);
		addMember(l,Register_s);
		addMember(l,Register__String__IAttrExpressionFactory_s);
		createTypeMetatable(l,null, typeof(GameFramework.AttrCalc.DslCalculator));
	}
}
