using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SceneTypeEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.SceneTypeEnum");
		addMember(l,0,"Unclassified");
		addMember(l,1,"MainUi");
		addMember(l,2,"Story");
		addMember(l,3,"Activity");
		addMember(l,4,"Battle");
		LuaDLL.lua_pop(l, 1);
	}
}
