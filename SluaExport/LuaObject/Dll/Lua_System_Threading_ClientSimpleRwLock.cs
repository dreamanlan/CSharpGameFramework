using System;

using SLua;
using System.Collections.Generic;
public class Lua_System_Threading_ClientSimpleRwLock : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int EnterReadLock(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock self;
			checkValueType(l,1,out self);
			self.EnterReadLock();
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExitReadLock(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock self;
			checkValueType(l,1,out self);
			self.ExitReadLock();
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EnterWriteLock(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock self;
			checkValueType(l,1,out self);
			self.EnterWriteLock();
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExitWriteLock(IntPtr l) {
		try {
			System.Threading.ClientSimpleRwLock self;
			checkValueType(l,1,out self);
			self.ExitWriteLock();
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Threading.ClientSimpleRwLock");
		addMember(l,EnterReadLock);
		addMember(l,ExitReadLock);
		addMember(l,EnterWriteLock);
		addMember(l,ExitWriteLock);
		createTypeMetatable(l,constructor, typeof(System.Threading.ClientSimpleRwLock),typeof(System.ValueType));
	}
}
