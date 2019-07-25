using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_SubItemToken : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.SubItemToken o;
			o=new ProtoBuf.SubItemToken();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.SubItemToken");
		createTypeMetatable(l,constructor, typeof(ProtoBuf.SubItemToken),typeof(System.ValueType));
	}
}
