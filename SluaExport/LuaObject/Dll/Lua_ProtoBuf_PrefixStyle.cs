using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_PrefixStyle : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtoBuf.PrefixStyle");
		addMember(l,0,"None");
		addMember(l,1,"Base128");
		addMember(l,2,"Fixed32");
		addMember(l,3,"Fixed32BigEndian");
		LuaDLL.lua_pop(l, 1);
	}
}
