using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_StoryCommandGroupDefine : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.StoryCommandGroupDefine");
		addMember(l,0,"USER");
		addMember(l,1,"GM");
		addMember(l,2,"GFX");
		addMember(l,3,"UI");
		addMember(l,4,"NUM");
		LuaDLL.lua_pop(l, 1);
	}
}
