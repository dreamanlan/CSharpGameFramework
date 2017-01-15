using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_LongComparer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			LongComparer o;
			o=new LongComparer();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,LongComparer.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"LongComparer");
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(LongComparer));
	}
}
