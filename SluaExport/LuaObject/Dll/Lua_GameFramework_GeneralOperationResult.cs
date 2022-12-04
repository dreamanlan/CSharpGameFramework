using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_GeneralOperationResult : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.GeneralOperationResult");
		addMember(l,0,"LC_Succeed");
		addMember(l,1,"LC_Failed");
		LuaDLL.lua_pop(l, 1);
	}
}
