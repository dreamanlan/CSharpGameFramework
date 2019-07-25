using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_CampIdEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.CampIdEnum");
		addMember(l,0,"Unkown");
		addMember(l,1,"Friendly");
		addMember(l,2,"Hostile");
		addMember(l,3,"Blue");
		addMember(l,4,"Red");
		addMember(l,5,"FreedomCamp_Begin");
		addMember(l,2147483647,"FreedomCamp_End");
		LuaDLL.lua_pop(l, 1);
	}
}
