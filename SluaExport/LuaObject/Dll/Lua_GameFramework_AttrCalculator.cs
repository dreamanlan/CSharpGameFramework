using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_AttrCalculator : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int Calc__EntityInfo_s(IntPtr l) {
		try {
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			GameFramework.AttrCalculator.Calc(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Calc__EntityInfo__Skill_s(IntPtr l) {
		try {
			GameFramework.EntityInfo a1;
			checkType(l,1,out a1);
			TableConfig.Skill a2;
			checkType(l,2,out a2);
			GameFramework.AttrCalculator.Calc(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Calc__SceneContextInfo__CharacterProperty__CharacterProperty__String__A_Int64_s(IntPtr l) {
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
			var ret=GameFramework.AttrCalculator.Calc(a1,a2,a3,a4,a5);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AttrCalculator");
		addMember(l,ctor_s);
		addMember(l,LoadConfig_s);
		addMember(l,CopyBaseProperty_s);
		addMember(l,RefixAttrByImpact_s);
		addMember(l,RefixAttrBySkill_s);
		addMember(l,Calc__EntityInfo_s);
		addMember(l,Calc__EntityInfo__Skill_s);
		addMember(l,Calc__SceneContextInfo__CharacterProperty__CharacterProperty__String__A_Int64_s);
		addMember(l,SkillCalc_s);
		createTypeMetatable(l,null, typeof(GameFramework.AttrCalculator));
	}
}
