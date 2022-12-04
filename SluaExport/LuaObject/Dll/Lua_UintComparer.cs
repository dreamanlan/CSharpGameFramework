using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UintComparer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UintComparer o;
			o=new UintComparer();
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
			UintComparer self=(UintComparer)checkSelf(l);
			System.UInt32 a1;
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
			UintComparer self=(UintComparer)checkSelf(l);
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
	static public int Equals__UInt32__UInt32(IntPtr l) {
		try {
			UintComparer self=(UintComparer)checkSelf(l);
			System.UInt32 a1;
			checkType(l,2,out a1);
			System.UInt32 a2;
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
			pushValue(l,UintComparer.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UintComparer");
		addMember(l,ctor_s);
		addMember(l,GetHashCode);
		addMember(l,Equals__Object);
		addMember(l,Equals__UInt32__UInt32);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(UintComparer));
	}
}
