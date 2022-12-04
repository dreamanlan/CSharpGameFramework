using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UlongComparer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UlongComparer o;
			o=new UlongComparer();
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
	static new public int GetHashCode(IntPtr l) {
		try {
			UlongComparer self=(UlongComparer)checkSelf(l);
			System.UInt64 a1;
			checkType(l,2,out a1);
			var ret=self.GetHashCode(a1);
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
	static public int Equals__Object(IntPtr l) {
		try {
			UlongComparer self=(UlongComparer)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static public int Equals__UInt64__UInt64(IntPtr l) {
		try {
			UlongComparer self=(UlongComparer)checkSelf(l);
			System.UInt64 a1;
			checkType(l,2,out a1);
			System.UInt64 a2;
			checkType(l,3,out a2);
			var ret=self.Equals(a1,a2);
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
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,UlongComparer.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UlongComparer");
		addMember(l,ctor_s);
		addMember(l,GetHashCode);
		addMember(l,Equals__Object);
		addMember(l,Equals__UInt64__UInt64);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(UlongComparer));
	}
}
