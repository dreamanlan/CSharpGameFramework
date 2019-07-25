using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_PredefinedAiStateId : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.PredefinedAiStateId");
		addMember(l,0,"Invalid");
		addMember(l,1,"Idle");
		addMember(l,2,"MoveCommand");
		addMember(l,3,"WaitCommand");
		addMember(l,100,"MaxValue");
		LuaDLL.lua_pop(l, 1);
	}
}
