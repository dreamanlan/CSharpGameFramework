using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_ProtoSyntax : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtoBuf.Meta.ProtoSyntax");
		addMember(l,0,"Proto2");
		addMember(l,1,"Proto3");
		LuaDLL.lua_pop(l, 1);
	}
}
