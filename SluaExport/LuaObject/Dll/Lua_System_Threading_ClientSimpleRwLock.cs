using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_System_Threading_ClientSimpleRwLock : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock o;
			o=new System.Threading.ClientSimpleRwLock();
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
	static public int EnterReadLock(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock self;
			checkValueType(l,1,out self);
			self.EnterReadLock();
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
	static public int ExitReadLock(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock self;
			checkValueType(l,1,out self);
			self.ExitReadLock();
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
	static public int EnterWriteLock(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock self;
			checkValueType(l,1,out self);
			self.EnterWriteLock();
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
	static public int ExitWriteLock(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock self;
			checkValueType(l,1,out self);
			self.ExitWriteLock();
			pushValue(l,true);
			setBack(l,(object)self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Threading.ClientSimpleRwLock");
		addMember(l,ctor_s);
		addMember(l,EnterReadLock);
		addMember(l,ExitReadLock);
		addMember(l,EnterWriteLock);
		addMember(l,ExitWriteLock);
		createTypeMetatable(l,null, typeof(System.Threading.ClientSimpleRwLock),typeof(System.ValueType));
	}
}
