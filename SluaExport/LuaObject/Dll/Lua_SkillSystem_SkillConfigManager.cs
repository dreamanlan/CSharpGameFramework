using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_SkillConfigManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadSkillIfNotExist(IntPtr l) {
		try {
			SkillSystem.SkillConfigManager self=(SkillSystem.SkillConfigManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.LoadSkillIfNotExist(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExistSkill(IntPtr l) {
		try {
			SkillSystem.SkillConfigManager self=(SkillSystem.SkillConfigManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.ExistSkill(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadSkill(IntPtr l) {
		try {
			SkillSystem.SkillConfigManager self=(SkillSystem.SkillConfigManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.LoadSkill(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadSkillText(IntPtr l) {
		try {
			SkillSystem.SkillConfigManager self=(SkillSystem.SkillConfigManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.LoadSkillText(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadSkillDsl(IntPtr l) {
		try {
			SkillSystem.SkillConfigManager self=(SkillSystem.SkillConfigManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			Dsl.FunctionData a2;
			checkType(l,3,out a2);
			self.LoadSkillDsl(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NewSkillInstance(IntPtr l) {
		try {
			SkillSystem.SkillConfigManager self=(SkillSystem.SkillConfigManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.NewSkillInstance(a1);
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
			SkillSystem.SkillConfigManager self=(SkillSystem.SkillConfigManager)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,SkillSystem.SkillConfigManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillConfigManager");
		addMember(l,LoadSkillIfNotExist);
		addMember(l,ExistSkill);
		addMember(l,LoadSkill);
		addMember(l,LoadSkillText);
		addMember(l,LoadSkillDsl);
		addMember(l,NewSkillInstance);
		addMember(l,Clear);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(SkillSystem.SkillConfigManager));
	}
}
