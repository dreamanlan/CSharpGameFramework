﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_RoleEnterResult_RoleEnterResultEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.RoleEnterResult.RoleEnterResultEnum");
		addMember(l,0,"Success");
		addMember(l,1,"Wait");
		addMember(l,2,"Reconnect");
		addMember(l,3,"UnknownError");
		LuaDLL.lua_pop(l, 1);
	}
}
