using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Experimental_Rendering_VisibleLightFlags : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.Experimental.Rendering.VisibleLightFlags");
		addMember(l,0,"None");
		addMember(l,1,"IntersectsNearPlane");
		addMember(l,2,"IntersectsFarPlane");
		LuaDLL.lua_pop(l, 1);
	}
}
