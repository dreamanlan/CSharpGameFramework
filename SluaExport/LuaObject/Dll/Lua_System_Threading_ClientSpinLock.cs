using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_System_Threading_ClientSpinLock : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			System.Threading.ClientSpinLock o;
			o=new System.Threading.ClientSpinLock();
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
	static public int ctor__Boolean_s(IntPtr l) {
		try {
			System.Threading.ClientSpinLock o;
			System.Boolean a1;
			checkType(l,1,out a1);
			o=new System.Threading.ClientSpinLock(a1);
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
	static public int Enter(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.Enter(ref a1);
			pushValue(l,true);
			pushValue(l,a1);
			setBack(l,(object)self);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryEnter__R_Boolean(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.TryEnter(ref a1);
			pushValue(l,true);
			pushValue(l,a1);
			setBack(l,(object)self);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryEnter__TimeSpan__R_Boolean(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			System.TimeSpan a1;
			checkValueType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.TryEnter(a1,ref a2);
			pushValue(l,true);
			pushValue(l,a2);
			setBack(l,(object)self);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryEnter__Int32__R_Boolean(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.TryEnter(a1,ref a2);
			pushValue(l,true);
			pushValue(l,a2);
			setBack(l,(object)self);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Exit(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			self.Exit();
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
	static public int Exit__Boolean(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.Exit(a1);
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
	static public int get_IsThreadOwnerTrackingEnabled(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsThreadOwnerTrackingEnabled);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsHeld(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsHeld);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsHeldByCurrentThread(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsHeldByCurrentThread);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Threading.ClientSpinLock");
		addMember(l,ctor_s);
		addMember(l,ctor__Boolean_s);
		addMember(l,Enter);
		addMember(l,TryEnter__R_Boolean);
		addMember(l,TryEnter__TimeSpan__R_Boolean);
		addMember(l,TryEnter__Int32__R_Boolean);
		addMember(l,Exit);
		addMember(l,Exit__Boolean);
		addMember(l,"IsThreadOwnerTrackingEnabled",get_IsThreadOwnerTrackingEnabled,null,true);
		addMember(l,"IsHeld",get_IsHeld,null,true);
		addMember(l,"IsHeldByCurrentThread",get_IsHeldByCurrentThread,null,true);
		createTypeMetatable(l,null, typeof(System.Threading.ClientSpinLock),typeof(System.ValueType));
	}
}
