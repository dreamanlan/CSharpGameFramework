using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_StoryValueGroupDefine : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.StoryValueGroupDefine");
		addMember(l,0,"USER");
		addMember(l,1,"GM");
		addMember(l,2,"GFX");
		addMember(l,3,"UI");
		addMember(l,4,"NUM");
		LuaDLL.lua_pop(l, 1);
	}
}
