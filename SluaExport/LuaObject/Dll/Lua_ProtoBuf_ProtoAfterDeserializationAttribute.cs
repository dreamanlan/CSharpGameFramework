using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoAfterDeserializationAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoAfterDeserializationAttribute o;
			o=new ProtoBuf.ProtoAfterDeserializationAttribute();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoAfterDeserializationAttribute");
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoAfterDeserializationAttribute),typeof(System.Attribute));
	}
}
