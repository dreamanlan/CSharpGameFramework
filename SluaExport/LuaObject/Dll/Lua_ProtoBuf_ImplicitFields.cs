using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ImplicitFields : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtoBuf.ImplicitFields");
		addMember(l,0,"None");
		addMember(l,1,"AllPublic");
		addMember(l,2,"AllFields");
		LuaDLL.lua_pop(l, 1);
	}
}
