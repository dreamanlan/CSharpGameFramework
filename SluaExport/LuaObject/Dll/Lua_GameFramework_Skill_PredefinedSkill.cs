using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_PredefinedSkill : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReBuild(IntPtr l) {
		try {
			GameFramework.Skill.PredefinedSkill self=(GameFramework.Skill.PredefinedSkill)checkSelf(l);
			self.ReBuild();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Preload(IntPtr l) {
		try {
			GameFramework.Skill.PredefinedSkill self=(GameFramework.Skill.PredefinedSkill)checkSelf(l);
			self.Preload();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_EmitSkillId(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Skill.PredefinedSkill.c_EmitSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_HitSkillId(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Skill.PredefinedSkill.c_HitSkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_EmitSkillCfg(IntPtr l) {
		try {
			GameFramework.Skill.PredefinedSkill self=(GameFramework.Skill.PredefinedSkill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EmitSkillCfg);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HitSkillCfg(IntPtr l) {
		try {
			GameFramework.Skill.PredefinedSkill self=(GameFramework.Skill.PredefinedSkill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HitSkillCfg);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Skill.PredefinedSkill.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.PredefinedSkill");
		addMember(l,ReBuild);
		addMember(l,Preload);
		addMember(l,"c_EmitSkillId",get_c_EmitSkillId,null,false);
		addMember(l,"c_HitSkillId",get_c_HitSkillId,null,false);
		addMember(l,"EmitSkillCfg",get_EmitSkillCfg,null,true);
		addMember(l,"HitSkillCfg",get_HitSkillCfg,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.Skill.PredefinedSkill));
	}
}
