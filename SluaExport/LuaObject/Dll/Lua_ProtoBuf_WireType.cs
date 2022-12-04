using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_WireType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtoBuf.WireType");
		addMember(l,0,"Variant");
		addMember(l,1,"Fixed64");
		addMember(l,2,"String");
		addMember(l,3,"StartGroup");
		addMember(l,4,"EndGroup");
		addMember(l,5,"Fixed32");
		addMember(l,8,"SignedVariant");
		addMember(l,-1,"None");
		LuaDLL.lua_pop(l, 1);
	}
}
