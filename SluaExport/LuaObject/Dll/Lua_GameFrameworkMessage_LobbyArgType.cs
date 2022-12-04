using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_LobbyArgType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.LobbyArgType");
		addMember(l,0,"NULL");
		addMember(l,1,"INT");
		addMember(l,2,"FLOAT");
		addMember(l,3,"STRING");
		LuaDLL.lua_pop(l, 1);
	}
}
