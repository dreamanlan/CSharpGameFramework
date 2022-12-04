using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Meta_RuntimeTypeModel_Accessibility : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtoBuf.Meta.RuntimeTypeModel.Accessibility");
		addMember(l,0,"Public");
		addMember(l,1,"Internal");
		LuaDLL.lua_pop(l, 1);
	}
}
