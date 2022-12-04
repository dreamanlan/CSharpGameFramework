using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_ChangeNameResult_ChangeNameResultEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.ChangeNameResult.ChangeNameResultEnum");
		addMember(l,0,"Success");
		addMember(l,1,"NicknameError");
		addMember(l,2,"UnknownError");
		LuaDLL.lua_pop(l, 1);
	}
}
