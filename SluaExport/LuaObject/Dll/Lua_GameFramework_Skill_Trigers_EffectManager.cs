using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_Trigers_EffectManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.EffectManager o;
			o=new GameFramework.Skill.Trigers.EffectManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddEffect(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.EffectManager self=(GameFramework.Skill.Trigers.EffectManager)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.AddEffect(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetParticleSpeed(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.EffectManager self=(GameFramework.Skill.Trigers.EffectManager)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			self.SetParticleSpeed(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PauseEffects(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.EffectManager self=(GameFramework.Skill.Trigers.EffectManager)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.PauseEffects(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StopEffects(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.EffectManager self=(GameFramework.Skill.Trigers.EffectManager)checkSelf(l);
			self.StopEffects();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.Trigers.EffectManager");
		addMember(l,AddEffect);
		addMember(l,SetParticleSpeed);
		addMember(l,PauseEffects);
		addMember(l,StopEffects);
		createTypeMetatable(l,constructor, typeof(GameFramework.Skill.Trigers.EffectManager));
	}
}
