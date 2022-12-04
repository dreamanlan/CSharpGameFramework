using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_AttrCalc_AbstractAttrExpression : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Calc(IntPtr l) {
		try {
			GameFramework.AttrCalc.AbstractAttrExpression self=(GameFramework.AttrCalc.AbstractAttrExpression)checkSelf(l);
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
	static public int Load(IntPtr l) {
		try {
			GameFramework.AttrCalc.AbstractAttrExpression self=(GameFramework.AttrCalc.AbstractAttrExpression)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			GameFramework.AttrCalc.DslCalculator a2;
			checkType(l,3,out a2);
			var ret=self.Load(a1,a2);
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
		getTypeTable(l,"GameFramework.AttrCalc.AbstractAttrExpression");
		addMember(l,Calc);
		addMember(l,Load);
		createTypeMetatable(l,null, typeof(GameFramework.AttrCalc.AbstractAttrExpression));
	}
}
