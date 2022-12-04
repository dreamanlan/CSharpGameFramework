using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_BclHelpers_NetObjectOptions : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtoBuf.BclHelpers.NetObjectOptions");
		addMember(l,0,"None");
		addMember(l,1,"AsReference");
		addMember(l,2,"DynamicType");
		addMember(l,4,"UseConstructor");
		addMember(l,8,"LateSet");
		LuaDLL.lua_pop(l, 1);
	}
}
