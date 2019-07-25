using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_Trigers_AudioManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.AudioManager o;
			o=new GameFramework.Skill.Trigers.AudioManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsContainAudioSource(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.AudioManager self=(GameFramework.Skill.Trigers.AudioManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.IsContainAudioSource(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAudioSource(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.AudioManager self=(GameFramework.Skill.Trigers.AudioManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetAudioSource(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddAudioSource(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.AudioManager self=(GameFramework.Skill.Trigers.AudioManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			UnityEngine.AudioSource a2;
			checkType(l,3,out a2);
			self.AddAudioSource(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.Trigers.AudioManager");
		addMember(l,IsContainAudioSource);
		addMember(l,GetAudioSource);
		addMember(l,AddAudioSource);
		createTypeMetatable(l,constructor, typeof(GameFramework.Skill.Trigers.AudioManager));
	}
}
