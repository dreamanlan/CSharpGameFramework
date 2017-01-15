using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_ArgType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.ArgType");
		addMember(l,0,"NULL");
		addMember(l,1,"INT");
		addMember(l,2,"FLOAT");
		addMember(l,3,"STRING");
		LuaDLL.lua_pop(l, 1);
	}
}
