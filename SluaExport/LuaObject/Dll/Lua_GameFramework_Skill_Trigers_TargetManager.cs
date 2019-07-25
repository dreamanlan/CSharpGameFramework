using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_Trigers_TargetManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.TargetManager o;
			o=new GameFramework.Skill.Trigers.TargetManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Add(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.TargetManager self=(GameFramework.Skill.Trigers.TargetManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.Add(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Exist(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.TargetManager self=(GameFramework.Skill.Trigers.TargetManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.Exist(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.TargetManager self=(GameFramework.Skill.Trigers.TargetManager)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Targets(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.TargetManager self=(GameFramework.Skill.Trigers.TargetManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Targets);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.Trigers.TargetManager");
		addMember(l,Add);
		addMember(l,Exist);
		addMember(l,Clear);
		addMember(l,"Targets",get_Targets,null,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.Skill.Trigers.TargetManager));
	}
}
