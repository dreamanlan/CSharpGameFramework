using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ClientConcurrentTypedDataCollection : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.ClientConcurrentTypedDataCollection o;
			o=new GameFramework.ClientConcurrentTypedDataCollection();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			GameFramework.ClientConcurrentTypedDataCollection self=(GameFramework.ClientConcurrentTypedDataCollection)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Visit(IntPtr l) {
		try {
			GameFramework.ClientConcurrentTypedDataCollection self=(GameFramework.ClientConcurrentTypedDataCollection)checkSelf(l);
			GameFramework.MyAction<System.Object,System.Object> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.Visit(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientConcurrentTypedDataCollection");
		addMember(l,Clear);
		addMember(l,Visit);
		createTypeMetatable(l,constructor, typeof(GameFramework.ClientConcurrentTypedDataCollection));
	}
}
