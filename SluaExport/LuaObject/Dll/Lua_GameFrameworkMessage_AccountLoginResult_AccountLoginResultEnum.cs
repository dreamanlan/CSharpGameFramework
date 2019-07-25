using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_AccountLoginResult_AccountLoginResultEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFrameworkMessage.AccountLoginResult.AccountLoginResultEnum");
		addMember(l,0,"Success");
		addMember(l,1,"FirstLogin");
		addMember(l,2,"Error");
		addMember(l,3,"Wait");
		addMember(l,4,"Banned");
		addMember(l,5,"AlreadyOnline");
		addMember(l,6,"Queueing");
		addMember(l,7,"QueueFull");
		addMember(l,8,"Failed");
		addMember(l,9,"Nonactivated");
		LuaDLL.lua_pop(l, 1);
	}
}
