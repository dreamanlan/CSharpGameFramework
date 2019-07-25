using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_MemberSerializationOptions : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"ProtoBuf.MemberSerializationOptions");
		addMember(l,0,"None");
		addMember(l,1,"Packed");
		addMember(l,2,"Required");
		addMember(l,4,"AsReference");
		addMember(l,8,"DynamicType");
		addMember(l,16,"OverwriteList");
		addMember(l,32,"AsReferenceHasValue");
		LuaDLL.lua_pop(l, 1);
	}
}
