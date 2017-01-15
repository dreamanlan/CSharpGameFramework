using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ResourceReadProxy : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReadAsMemoryStream_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.ResourceReadProxy.ReadAsMemoryStream(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReadAsArray_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.ResourceReadProxy.ReadAsArray(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Exists_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.ResourceReadProxy.Exists(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OnReadAsArray(IntPtr l) {
		try {
			GameFramework.ReadAsArrayDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) GameFramework.ResourceReadProxy.OnReadAsArray=v;
			else if(op==1) GameFramework.ResourceReadProxy.OnReadAsArray+=v;
			else if(op==2) GameFramework.ResourceReadProxy.OnReadAsArray-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ResourceReadProxy");
		addMember(l,ReadAsMemoryStream_s);
		addMember(l,ReadAsArray_s);
		addMember(l,Exists_s);
		addMember(l,"OnReadAsArray",null,set_OnReadAsArray,false);
		createTypeMetatable(l,null, typeof(GameFramework.ResourceReadProxy));
	}
}
