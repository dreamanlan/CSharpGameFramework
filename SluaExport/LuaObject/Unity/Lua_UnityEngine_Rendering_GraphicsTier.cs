using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Rendering_GraphicsTier : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.Rendering.GraphicsTier");
		addMember(l,0,"Tier1");
		addMember(l,1,"Tier2");
		addMember(l,2,"Tier3");
		LuaDLL.lua_pop(l, 1);
	}
}
