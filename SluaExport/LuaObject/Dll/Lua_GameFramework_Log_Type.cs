using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Log_Type : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.Log_Type");
		addMember(l,0,"LT_Debug");
		addMember(l,1,"LT_Info");
		addMember(l,2,"LT_Warn");
		addMember(l,3,"LT_Error");
		addMember(l,4,"LT_Assert");
		addMember(l,5,"LT_GM");
		LuaDLL.lua_pop(l, 1);
	}
}
