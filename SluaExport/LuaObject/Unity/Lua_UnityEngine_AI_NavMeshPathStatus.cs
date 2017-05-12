using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_AI_NavMeshPathStatus : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.AI.NavMeshPathStatus");
		addMember(l,0,"PathComplete");
		addMember(l,1,"PathPartial");
		addMember(l,2,"PathInvalid");
		LuaDLL.lua_pop(l, 1);
	}
}
