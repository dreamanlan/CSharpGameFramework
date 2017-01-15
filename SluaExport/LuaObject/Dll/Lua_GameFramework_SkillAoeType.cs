using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SkillAoeType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.SkillAoeType");
		addMember(l,0,"Unknown");
		addMember(l,1,"Circle");
		addMember(l,2,"Sector");
		addMember(l,3,"Capsule");
		addMember(l,4,"Rectangle");
		LuaDLL.lua_pop(l, 1);
	}
}
