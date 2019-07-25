using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_Trigers_KeepSectionForBuffTrigger : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.KeepSectionForBuffTrigger o;
			o=new GameFramework.Skill.Trigers.KeepSectionForBuffTrigger();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.KeepSectionForBuffTrigger self=(GameFramework.Skill.Trigers.KeepSectionForBuffTrigger)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Execute(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.KeepSectionForBuffTrigger self=(GameFramework.Skill.Trigers.KeepSectionForBuffTrigger)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.Trigers.KeepSectionForBuffTrigger");
		addMember(l,Reset);
		addMember(l,Execute);
		createTypeMetatable(l,constructor, typeof(GameFramework.Skill.Trigers.KeepSectionForBuffTrigger),typeof(SkillSystem.AbstractSkillTriger));
	}
}
