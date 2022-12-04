using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_ISkillTrigerFactory : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Create(IntPtr l) {
		try {
			SkillSystem.ISkillTrigerFactory self=(SkillSystem.ISkillTrigerFactory)checkSelf(l);
			var ret=self.Create();
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
		getTypeTable(l,"SkillSystem.ISkillTrigerFactory");
		addMember(l,Create);
		createTypeMetatable(l,null, typeof(SkillSystem.ISkillTrigerFactory));
	}
}
