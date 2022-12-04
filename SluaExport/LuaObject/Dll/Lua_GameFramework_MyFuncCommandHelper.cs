using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_MyFuncCommandHelper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.MyFuncCommandHelper o;
			o=new GameFramework.MyFuncCommandHelper();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Init(IntPtr l) {
		try {
			GameFramework.MyFuncCommandHelper self=(GameFramework.MyFuncCommandHelper)checkSelf(l);
			GameFramework.MyFunc a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.Init(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Execute(IntPtr l) {
		try {
			GameFramework.MyFuncCommandHelper self=(GameFramework.MyFuncCommandHelper)checkSelf(l);
			var ret=self.Execute();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Downcast(IntPtr l) {
		try {
			GameFramework.MyFuncCommandHelper self=(GameFramework.MyFuncCommandHelper)checkSelf(l);
			var ret=self.Downcast();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.MyFuncCommandHelper");
		addMember(l,ctor_s);
		addMember(l,Init);
		addMember(l,Execute);
		addMember(l,Downcast);
		createTypeMetatable(l,null, typeof(GameFramework.MyFuncCommandHelper));
	}
}
