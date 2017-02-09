using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoIgnoreAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoIgnoreAttribute o;
			o=new ProtoBuf.ProtoIgnoreAttribute();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoIgnoreAttribute");
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoIgnoreAttribute),typeof(System.Attribute));
	}
}
