using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_AI_NavMeshCollectGeometry : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.AI.NavMeshCollectGeometry");
		addMember(l,0,"RenderMeshes");
		addMember(l,1,"PhysicsColliders");
		LuaDLL.lua_pop(l, 1);
	}
}
