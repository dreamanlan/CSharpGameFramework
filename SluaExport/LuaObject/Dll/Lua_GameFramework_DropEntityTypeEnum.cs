using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_DropEntityTypeEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.DropEntityTypeEnum");
		addMember(l,110001,"GOLD");
		addMember(l,110002,"HP");
		addMember(l,110003,"MP");
		addMember(l,110004,"MUTI_GOLD");
		addMember(l,110005,"ITEM");
		LuaDLL.lua_pop(l, 1);
	}
}
