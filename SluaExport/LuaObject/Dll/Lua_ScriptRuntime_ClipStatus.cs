using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_ClipStatus : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ScriptRuntime.ClipStatus");
		addMember(l,0,"Outside");
		addMember(l,1,"Inside");
		addMember(l,2,"Intersecting");
		LuaDLL.lua_pop(l, 1);
	}
}
