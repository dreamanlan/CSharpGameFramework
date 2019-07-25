using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SkillOrImpactType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.SkillOrImpactType");
		addMember(l,0,"Skill");
		addMember(l,1,"Impact");
		addMember(l,2,"Buff");
		LuaDLL.lua_pop(l, 1);
	}
}
