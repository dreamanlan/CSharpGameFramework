using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichTextParser_IRichText : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Type(IntPtr l) {
		try {
			RichTextParser.IRichText self=(RichTextParser.IRichText)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichTextParser.IRichText");
		addMember(l,"Type",get_Type,null,true);
		createTypeMetatable(l,null, typeof(RichTextParser.IRichText));
	}
}
