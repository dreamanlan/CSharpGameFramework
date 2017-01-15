using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_GmStateEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.GmStateEnum");
		addMember(l,1,"Online");
		addMember(l,2,"Offline");
		addMember(l,3,"Banned");
		LuaDLL.lua_pop(l, 1);
	}
}
