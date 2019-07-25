using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_ShakeHands_Ret_RetType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.Msg_RC_ShakeHands_Ret.RetType");
		addMember(l,0,"SUCCESS");
		addMember(l,1,"ERROR");
		LuaDLL.lua_pop(l, 1);
	}
}
