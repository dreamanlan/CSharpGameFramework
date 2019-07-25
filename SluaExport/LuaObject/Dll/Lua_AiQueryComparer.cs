using System;

using SLua;
using System.Collections.Generic;
public class Lua_AiQueryComparer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			AiQueryComparer o;
			System.Boolean a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			o=new AiQueryComparer(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Compare(IntPtr l) {
		try {
			AiQueryComparer self=(AiQueryComparer)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			var ret=self.Compare(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"AiQueryComparer");
		addMember(l,Compare);
		createTypeMetatable(l,constructor, typeof(AiQueryComparer));
	}
}
