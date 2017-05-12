using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_FilePathDefine_Server : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			FilePathDefine_Server o;
			o=new FilePathDefine_Server();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_RootPath(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_RootPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_DslPath(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_DslPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_UserScript(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_UserScript);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_Actor(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_Actor);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_AttrDefine(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_AttrDefine);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_Const(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_Const);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_Formation(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_Formation);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_ImpactData(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_ImpactData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_LevelMonster(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_LevelMonster);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_Skill(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_Skill);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_SkillData(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_SkillData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_SkillEvent(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_SkillEvent);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_SkillResources(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_SkillResources);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_C_Level(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Server.C_Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"FilePathDefine_Server");
		addMember(l,"C_RootPath",get_C_RootPath,null,false);
		addMember(l,"C_DslPath",get_C_DslPath,null,false);
		addMember(l,"C_UserScript",get_C_UserScript,null,false);
		addMember(l,"C_Actor",get_C_Actor,null,false);
		addMember(l,"C_AttrDefine",get_C_AttrDefine,null,false);
		addMember(l,"C_Const",get_C_Const,null,false);
		addMember(l,"C_Formation",get_C_Formation,null,false);
		addMember(l,"C_ImpactData",get_C_ImpactData,null,false);
		addMember(l,"C_LevelMonster",get_C_LevelMonster,null,false);
		addMember(l,"C_Skill",get_C_Skill,null,false);
		addMember(l,"C_SkillData",get_C_SkillData,null,false);
		addMember(l,"C_SkillEvent",get_C_SkillEvent,null,false);
		addMember(l,"C_SkillResources",get_C_SkillResources,null,false);
		addMember(l,"C_Level",get_C_Level,null,false);
		createTypeMetatable(l,constructor, typeof(FilePathDefine_Server));
	}
}
