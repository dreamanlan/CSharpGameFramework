using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_System_Threading_ClientSpinWait : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			System.Threading.ClientSpinWait o;
			o=new System.Threading.ClientSpinWait();
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
	static public int SpinOnce(IntPtr l) {
		try {
			System.Threading.ClientSpinWait self;
			checkValueType(l,1,out self);
			self.SpinOnce();
			pushValue(l,true);
			setBack(l,(object)self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reset(IntPtr l) {
		try {
			System.Threading.ClientSpinWait self;
			checkValueType(l,1,out self);
			self.Reset();
			pushValue(l,true);
			setBack(l,(object)self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NextSpinWillYield(IntPtr l) {
		try {
			System.Threading.ClientSpinWait self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.NextSpinWillYield);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Count(IntPtr l) {
		try {
			System.Threading.ClientSpinWait self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Threading.ClientSpinWait");
		addMember(l,ctor_s);
		addMember(l,SpinOnce);
		addMember(l,Reset);
		addMember(l,"NextSpinWillYield",get_NextSpinWillYield,null,true);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,null, typeof(System.Threading.ClientSpinWait),typeof(System.ValueType));
	}
}
