using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_Trigers_RestorePosTrigger : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.RestorePosTrigger o;
			o=new GameFramework.Skill.Trigers.RestorePosTrigger();
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
			GameFramework.Skill.Trigers.RestorePosTrigger self=(GameFramework.Skill.Trigers.RestorePosTrigger)checkSelf(l);
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
			GameFramework.Skill.Trigers.RestorePosTrigger self=(GameFramework.Skill.Trigers.RestorePosTrigger)checkSelf(l);
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
		getTypeTable(l,"GameFramework.Skill.Trigers.RestorePosTrigger");
		addMember(l,Reset);
		addMember(l,Execute);
		createTypeMetatable(l,constructor, typeof(GameFramework.Skill.Trigers.RestorePosTrigger),typeof(SkillSystem.AbstractSkillTriger));
	}
}
