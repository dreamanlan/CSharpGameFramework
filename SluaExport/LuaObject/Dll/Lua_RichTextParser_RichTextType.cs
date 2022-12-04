using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichTextParser_RichTextType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"RichTextParser.RichTextType");
		addMember(l,0,"Normal");
		addMember(l,1,"Hyper");
		LuaDLL.lua_pop(l, 1);
	}
}
