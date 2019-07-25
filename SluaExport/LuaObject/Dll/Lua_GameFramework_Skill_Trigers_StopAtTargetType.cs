using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_Trigers_StopAtTargetType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.Skill.Trigers.StopAtTargetType");
		addMember(l,0,"NoStop");
		addMember(l,1,"AdjustVelocity");
		addMember(l,2,"AdjustTime");
		LuaDLL.lua_pop(l, 1);
	}
}
