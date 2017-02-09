using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_Serializer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FlushPool_s(IntPtr l) {
		try {
			ProtoBuf.Serializer.FlushPool();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ListItemTag(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,ProtoBuf.Serializer.ListItemTag);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.Serializer");
		addMember(l,FlushPool_s);
		addMember(l,"ListItemTag",get_ListItemTag,null,false);
		createTypeMetatable(l,null, typeof(ProtoBuf.Serializer));
	}
}
