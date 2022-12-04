using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_FilePathDefine_Client : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			FilePathDefine_Client o;
			o=new FilePathDefine_Client();
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
	static public int get_C_RootPath(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_RootPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_DslPath(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_DslPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_Actor(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_Actor);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_AttrDefine(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_AttrDefine);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_Const(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_Const);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_Formation(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_Formation);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_ImpactData(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_ImpactData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_LevelMonster(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_LevelMonster);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_Skill(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_Skill);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_SkillData(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_SkillData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_SkillEvent(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_SkillEvent);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_SkillResources(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_SkillResources);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_Level(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_StoryDlg(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_StoryDlg);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_StrDictionary(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_StrDictionary);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_C_UI(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,FilePathDefine_Client.C_UI);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"FilePathDefine_Client");
		addMember(l,ctor_s);
		addMember(l,"C_RootPath",get_C_RootPath,null,false);
		addMember(l,"C_DslPath",get_C_DslPath,null,false);
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
		addMember(l,"C_StoryDlg",get_C_StoryDlg,null,false);
		addMember(l,"C_StrDictionary",get_C_StrDictionary,null,false);
		addMember(l,"C_UI",get_C_UI,null,false);
		createTypeMetatable(l,null, typeof(FilePathDefine_Client));
	}
}
