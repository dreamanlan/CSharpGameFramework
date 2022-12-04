using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_DataFormat : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtoBuf.DataFormat");
		addMember(l,0,"Default");
		addMember(l,1,"ZigZag");
		addMember(l,2,"TwosComplement");
		addMember(l,3,"FixedSize");
		addMember(l,4,"Group");
		addMember(l,5,"WellKnown");
		LuaDLL.lua_pop(l, 1);
	}
}
