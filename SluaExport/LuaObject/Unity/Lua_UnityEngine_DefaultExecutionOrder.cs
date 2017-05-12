using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_DefaultExecutionOrder : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.DefaultExecutionOrder o;
			System.Int32 a1;
			checkType(l,2,out a1);
			o=new UnityEngine.DefaultExecutionOrder(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_order(IntPtr l) {
		try {
			UnityEngine.DefaultExecutionOrder self=(UnityEngine.DefaultExecutionOrder)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.order);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.DefaultExecutionOrder");
		addMember(l,"order",get_order,null,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.DefaultExecutionOrder),typeof(System.Attribute));
	}
}
