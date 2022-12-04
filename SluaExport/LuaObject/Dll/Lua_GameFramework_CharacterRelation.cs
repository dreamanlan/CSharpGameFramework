using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_CharacterRelation : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.CharacterRelation");
		addMember(l,0,"RELATION_ENEMY");
		addMember(l,1,"RELATION_FRIEND");
		addMember(l,2,"RELATION_NUMBERS");
		addMember(l,-1,"RELATION_INVALID");
		LuaDLL.lua_pop(l, 1);
	}
}
