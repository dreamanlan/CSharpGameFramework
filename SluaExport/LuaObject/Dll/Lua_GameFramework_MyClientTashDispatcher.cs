using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_MyClientTashDispatcher : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			GameFramework.MyClientTashDispatcher o;
			if(argc==1){
				o=new GameFramework.MyClientTashDispatcher();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==2){
				System.Int32 a1;
				checkType(l,2,out a1);
				o=new GameFramework.MyClientTashDispatcher(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				o=new GameFramework.MyClientTashDispatcher(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==4){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				o=new GameFramework.MyClientTashDispatcher(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==5){
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				o=new GameFramework.MyClientTashDispatcher(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int DebugPoolCount(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			GameFramework.MyAction<System.String> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.DebugPoolCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DebugThreadActionCount(IntPtr l) {
		try {
			GameFramework.MyClientTashDispatcher self=(GameFramework.MyClientTashDispatcher)checkSelf(l);
			GameFramework.MyAction<System.String> a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.DebugThreadActionCount(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.MyClientTashDispatcher");
		addMember(l,DispatchActionWithDelegation);
		addMember(l,DispatchAction);
		addMember(l,DebugPoolCount);
		addMember(l,DebugThreadActionCount);
		addMember(l,StopTaskThreads);
		addMember(l,ClearPool);
		addMember(l,"IsPassive",get_IsPassive,null,true);
		addMember(l,"TickSleepTime",get_TickSleepTime,set_TickSleepTime,true);
		addMember(l,"ActionNumPerTick",get_ActionNumPerTick,set_ActionNumPerTick,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.MyClientTashDispatcher));
	}
}
