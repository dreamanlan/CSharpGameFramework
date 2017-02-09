using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoAfterSerializationAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoAfterSerializationAttribute o;
			o=new ProtoBuf.ProtoAfterSerializationAttribute();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoAfterSerializationAttribute");
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoAfterSerializationAttribute),typeof(System.Attribute));
	}
}
