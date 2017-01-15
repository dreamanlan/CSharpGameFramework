using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_GmResultEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.GmResultEnum");
		addMember(l,1,"Success");
		addMember(l,2,"Failed");
		LuaDLL.lua_pop(l, 1);
	}
}
