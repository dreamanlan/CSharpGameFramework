using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_System_Threading_ClientSpinLock : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			System.Threading.ClientSpinLock o;
			System.Boolean a1;
			checkType(l,2,out a1);
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
	static public int Enter(IntPtr l) {
		try {
			System.Threading.ClientSpinLock self;
			checkValueType(l,1,out self);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.Enter(ref a1);
			pushValue(l,true);
			pushValue(l,a1);
			setBack(l,self);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryEnter(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				System.Threading.ClientSpinLock self;
				checkValueType(l,1,out self);
				System.Boolean a1;
				checkType(l,2,out a1);
				self.TryEnter(ref a1);
				pushValue(l,true);
				pushValue(l,a1);
				setBack(l,self);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(System.Boolean))){
				System.Threading.ClientSpinLock self;
				checkValueType(l,1,out self);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				self.TryEnter(a1,ref a2);
				pushValue(l,true);
				pushValue(l,a2);
				setBack(l,self);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.TimeSpan),typeof(System.Boolean))){
				System.Threading.ClientSpinLock self;
				checkValueType(l,1,out self);
				System.TimeSpan a1;
				checkValueType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				self.TryEnter(a1,ref a2);
				pushValue(l,true);
				pushValue(l,a2);
				setBack(l,self);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Exit(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.Threading.ClientSpinLock self;
				checkValueType(l,1,out self);
				self.Exit();
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			else if(argc==2){
				System.Threading.ClientSpinLock self;
				checkValueType(l,1,out self);
				System.Boolean a1;
				checkType(l,2,out a1);
				self.Exit(a1);
				pushValue(l,true);
				setBack(l,self);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Threading.ClientSpinLock");
		addMember(l,Enter);
		addMember(l,TryEnter);
		addMember(l,Exit);
		addMember(l,"IsThreadOwnerTrackingEnabled",get_IsThreadOwnerTrackingEnabled,null,true);
		addMember(l,"IsHeld",get_IsHeld,null,true);
		addMember(l,"IsHeldByCurrentThread",get_IsHeldByCurrentThread,null,true);
		createTypeMetatable(l,constructor, typeof(System.Threading.ClientSpinLock),typeof(System.ValueType));
	}
}
