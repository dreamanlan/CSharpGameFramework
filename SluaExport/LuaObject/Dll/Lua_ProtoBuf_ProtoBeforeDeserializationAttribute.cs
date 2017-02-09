using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoBeforeDeserializationAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoBeforeDeserializationAttribute o;
			o=new ProtoBuf.ProtoBeforeDeserializationAttribute();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoBeforeDeserializationAttribute");
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoBeforeDeserializationAttribute),typeof(System.Attribute));
	}
}
