using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SceneLogicId : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.SceneLogicId");
		addMember(l,10001,"TIME_OUT");
		addMember(l,10002,"SAND_CLOCK");
		LuaDLL.lua_pop(l, 1);
	}
}
