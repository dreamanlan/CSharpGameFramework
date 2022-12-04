using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_SkillParamUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			SkillSystem.SkillParamUtility o;
			o=new SkillSystem.SkillParamUtility();
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
	static public int RefixStringVariable_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,2,out a2);
			var ret=SkillSystem.SkillParamUtility.RefixStringVariable(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RefixObjectVariable_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,2,out a2);
			var ret=SkillSystem.SkillParamUtility.RefixObjectVariable(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillParamUtility");
		addMember(l,ctor_s);
		addMember(l,RefixStringVariable_s);
		addMember(l,RefixObjectVariable_s);
		createTypeMetatable(l,null, typeof(SkillSystem.SkillParamUtility));
	}
}
