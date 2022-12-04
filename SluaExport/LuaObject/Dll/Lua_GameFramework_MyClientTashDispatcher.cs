using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_MyClientTashDispatcher : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher o;
			o=new GameFramework.MyClientTashDispatcher();
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
	static public int ctor__Int32_s(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new GameFramework.MyClientTashDispatcher(a1);
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
	static public int ctor__Int32__Boolean_s(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			o=new GameFramework.MyClientTashDispatcher(a1,a2);
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
	static public int ctor__Int32__Boolean__Int32_s(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			o=new GameFramework.MyClientTashDispatcher(a1,a2,a3);
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
	static public int ctor__Int32__Boolean__Int32__Int32_s(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			o=new GameFramework.MyClientTashDispatcher(a1,a2,a3,a4);
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
	static public int DispatchActionWithDelegation(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			System.Delegate a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			self.DispatchActionWithDelegation(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DispatchAction(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			GameFramework.MyAction a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.DispatchAction(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DispatchFunc(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			GameFramework.MyFunc a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.DispatchFunc(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopTaskThreads(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			self.StopTaskThreads();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ClearPool(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.ClearPool(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsPassive(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsPassive);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TickSleepTime(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TickSleepTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TickSleepTime(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.TickSleepTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ActionNumPerTick(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ActionNumPerTick);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ActionNumPerTick(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.ActionNumPerTick=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.MyClientTashDispatcher");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32_s);
		addMember(l,ctor__Int32__Boolean_s);
		addMember(l,ctor__Int32__Boolean__Int32_s);
		addMember(l,ctor__Int32__Boolean__Int32__Int32_s);
		addMember(l,DispatchActionWithDelegation);
		addMember(l,DispatchAction);
		addMember(l,DispatchFunc);
		addMember(l,StopTaskThreads);
		addMember(l,ClearPool);
		addMember(l,"IsPassive",get_IsPassive,null,true);
		addMember(l,"TickSleepTime",get_TickSleepTime,set_TickSleepTime,true);
		addMember(l,"ActionNumPerTick",get_ActionNumPerTick,set_ActionNumPerTick,true);
		createTypeMetatable(l,null, typeof(GameFramework.MyClientTashDispatcher));
	}
}
