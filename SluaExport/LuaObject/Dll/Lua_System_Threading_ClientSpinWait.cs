using System;

using SLua;
using System.Collections.Generic;
public class Lua_System_Threading_ClientSpinWait : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int SpinOnce(IntPtr l) {
		try {
			System.Threading.ClientSpinWait self;
			checkValueType(l,1,out self);
			self.SpinOnce();
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			System.Threading.ClientSpinWait self;
			checkValueType(l,1,out self);
			self.Reset();
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SpinUntil_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				GameFramework.MyFunc<System.Boolean> a1;
				LuaDelegation.checkDelegate(l,1,out a1);
				System.Threading.ClientSpinWait.SpinUntil(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(GameFramework.MyFunc<System.Boolean>),typeof(int))){
				GameFramework.MyFunc<System.Boolean> a1;
				LuaDelegation.checkDelegate(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Threading.ClientSpinWait.SpinUntil(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(GameFramework.MyFunc<System.Boolean>),typeof(System.TimeSpan))){
				GameFramework.MyFunc<System.Boolean> a1;
				LuaDelegation.checkDelegate(l,1,out a1);
				System.TimeSpan a2;
				checkValueType(l,2,out a2);
				var ret=System.Threading.ClientSpinWait.SpinUntil(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Threading.ClientSpinWait");
		addMember(l,SpinOnce);
		addMember(l,Reset);
		addMember(l,SpinUntil_s);
		addMember(l,"NextSpinWillYield",get_NextSpinWillYield,null,true);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,constructor, typeof(System.Threading.ClientSpinWait),typeof(System.ValueType));
	}
}
