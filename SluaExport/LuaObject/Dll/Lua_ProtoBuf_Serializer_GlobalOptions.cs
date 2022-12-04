using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_ProtoBuf_Serializer_GlobalOptions : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Serializer.GlobalOptions");
		createTypeMetatable(l,null, typeof(ProtoBuf.Serializer.GlobalOptions));
	}
}
