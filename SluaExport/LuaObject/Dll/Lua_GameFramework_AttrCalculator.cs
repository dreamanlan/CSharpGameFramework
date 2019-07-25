using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_AttrCalculator : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.AttrCalculator o;
			o=new GameFramework.AttrCalculator();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadConfig_s(IntPtr l) {
		try {
			GameFramework.AttrCalculator.LoadConfig();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyBaseProperty_s(IntPtr l) {
		try {
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			GameFramework.AttrCalculator.CopyBaseProperty(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RefixAttrByImpact_s(IntPtr l) {
		try {
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			GameFramework.AttrCalculator.RefixAttrByImpact(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RefixAttrBySkill_s(IntPtr l) {
		try {
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			TableConfig.Skill a2;
			checkType(l,2,out a2);
			GameFramework.AttrCalculator.RefixAttrBySkill(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Calc_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				GameFramework.EntityInfo a1;
				checkType(l,1,out a1);
				GameFramework.AttrCalculator.Calc(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==2){
				GameFramework.EntityInfo a1;
				checkType(l,1,out a1);
				TableConfig.Skill a2;
				checkType(l,2,out a2);
				GameFramework.AttrCalculator.Calc(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				GameFramework.SceneContextInfo a1;
				checkType(l,1,out a1);
				GameFramework.CharacterProperty a2;
				checkType(l,2,out a2);
				GameFramework.CharacterProperty a3;
				checkType(l,3,out a3);
				System.String a4;
				checkType(l,4,out a4);
				System.Int64[] a5;
				checkParams(l,5,out a5);
				var ret=GameFramework.AttrCalculator.Calc(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SkillCalc_s(IntPtr l) {
		try {
			GameFramework.SceneContextInfo a1;
			checkType(l,1,out a1);
			GameFramework.CharacterProperty a2;
			checkType(l,2,out a2);
			GameFramework.CharacterProperty a3;
			checkType(l,3,out a3);
			System.String a4;
			checkType(l,4,out a4);
			System.Int64[] a5;
			checkParams(l,5,out a5);
			var ret=GameFramework.AttrCalculator.SkillCalc(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AttrCalculator");
		addMember(l,LoadConfig_s);
		addMember(l,CopyBaseProperty_s);
		addMember(l,RefixAttrByImpact_s);
		addMember(l,RefixAttrBySkill_s);
		addMember(l,Calc_s);
		addMember(l,SkillCalc_s);
		createTypeMetatable(l,constructor, typeof(GameFramework.AttrCalculator));
	}
}
