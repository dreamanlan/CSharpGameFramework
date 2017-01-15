using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ScriptRuntime_PlaneIntersectionStatus : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ScriptRuntime.PlaneIntersectionStatus");
		addMember(l,0,"Front");
		addMember(l,1,"Back");
		addMember(l,2,"Intersecting");
		LuaDLL.lua_pop(l, 1);
	}
}
