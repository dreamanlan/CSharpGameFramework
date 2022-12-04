using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_SkillTargetType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.SkillTargetType");
		addMember(l,0,"Self");
		addMember(l,1,"Enemy");
		addMember(l,2,"Friend");
		addMember(l,3,"RandEnemy");
		addMember(l,4,"RandFriend");
		LuaDLL.lua_pop(l, 1);
	}
}
