using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_SkillTrigerManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterTrigerFactory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				SkillSystem.SkillTrigerManager self=(SkillSystem.SkillTrigerManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				SkillSystem.ISkillTrigerFactory a2;
				checkType(l,3,out a2);
				self.RegisterTrigerFactory(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				SkillSystem.SkillTrigerManager self=(SkillSystem.SkillTrigerManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				SkillSystem.ISkillTrigerFactory a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				self.RegisterTrigerFactory(a1,a2,a3);
				pushValue(l,true);
				return 1;
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
	static public int CreateTriger(IntPtr l) {
		try {
			SkillSystem.SkillTrigerManager self=(SkillSystem.SkillTrigerManager)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			var ret=self.CreateTriger(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
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
			pushValue(l,SkillSystem.SkillTrigerManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillTrigerManager");
		addMember(l,RegisterTrigerFactory);
		addMember(l,CreateTriger);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(SkillSystem.SkillTrigerManager));
	}
}
