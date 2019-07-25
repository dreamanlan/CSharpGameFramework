using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_SerializationContext : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			ProtoBuf.SerializationContext o;
			o=new ProtoBuf.SerializationContext();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Context(IntPtr l) {
		try {
			ProtoBuf.SerializationContext self=(ProtoBuf.SerializationContext)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Context);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Context(IntPtr l) {
		try {
			ProtoBuf.SerializationContext self=(ProtoBuf.SerializationContext)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.Context=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.SerializationContext");
		addMember(l,"Context",get_Context,set_Context,true);
		createTypeMetatable(l,constructor, typeof(ProtoBuf.SerializationContext));
	}
}
