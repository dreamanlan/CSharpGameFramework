using System;

using SLua;
using System.Collections.Generic;
public class Lua_ProtoBuf_IExtensible : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetExtensionObject(IntPtr l) {
		try {
			ProtoBuf.IExtensible self=(ProtoBuf.IExtensible)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			var ret=self.GetExtensionObject(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ProtoBuf.IExtensible");
		addMember(l,GetExtensionObject);
		createTypeMetatable(l,null, typeof(ProtoBuf.IExtensible));
	}
}
