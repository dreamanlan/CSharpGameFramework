using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_EntityTypeEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.EntityTypeEnum");
		addMember(l,0,"Normal");
		addMember(l,1,"Tower");
		addMember(l,2,"Hero");
		addMember(l,3,"Boss");
		addMember(l,4,"Skill");
		LuaDLL.lua_pop(l, 1);
	}
}
