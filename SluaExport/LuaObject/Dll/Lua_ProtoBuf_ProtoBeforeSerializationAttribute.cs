using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_ProtoBeforeSerializationAttribute : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.ProtoBeforeSerializationAttribute o;
			o=new ProtoBuf.ProtoBeforeSerializationAttribute();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.ProtoBeforeSerializationAttribute");
		createTypeMetatable(l,constructor, typeof(ProtoBuf.ProtoBeforeSerializationAttribute),typeof(System.Attribute));
	}
}
