using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_ImpactDamage_Flag : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.Msg_RC_ImpactDamage.Flag");
		addMember(l,1,"IS_KILLER");
		addMember(l,2,"IS_CRITICAL");
		addMember(l,4,"IS_ORDINARY");
		LuaDLL.lua_pop(l, 1);
	}
}
