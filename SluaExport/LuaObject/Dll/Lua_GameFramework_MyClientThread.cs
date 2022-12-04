using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_MyClientThread : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.MyClientThread o;
			o=new GameFramework.MyClientThread();
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
			GameFramework.MyClientThread o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new GameFramework.MyClientThread(a1);
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
	static public int ctor__ClientAsyncActionProcessor_s(IntPtr l) {
		try {
			GameFramework.MyClientThread o;
			GameFramework.ClientAsyncActionProcessor a1;
			checkType(l,1,out a1);
			o=new GameFramework.MyClientThread(a1);
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
	static public int ctor__Int32__Int32_s(IntPtr l) {
		try {
			GameFramework.MyClientThread o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			o=new GameFramework.MyClientThread(a1,a2);
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
	static public int ctor__Int32__ClientAsyncActionProcessor_s(IntPtr l) {
		try {
			GameFramework.MyClientThread o;
			System.Int32 a1;
			checkType(l,1,out a1);
			GameFramework.ClientAsyncActionProcessor a2;
			checkType(l,2,out a2);
			o=new GameFramework.MyClientThread(a1,a2);
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
	static public int ctor__Int32__Int32__ClientAsyncActionProcessor_s(IntPtr l) {
		try {
			GameFramework.MyClientThread o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			GameFramework.ClientAsyncActionProcessor a3;
			checkType(l,3,out a3);
			o=new GameFramework.MyClientThread(a1,a2,a3);
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
	static public int ClearPool(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
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
	static public int Start(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			self.Start();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Stop(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			self.Stop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int QueueActionWithDelegation(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			System.Delegate a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			self.QueueActionWithDelegation(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int QueueAction(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			GameFramework.MyAction a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.QueueAction(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int QueueFunc(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			GameFramework.MyFunc a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.QueueFunc(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OnStartEvent(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			GameFramework.MyClientThreadEventDelegate v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnStartEvent=v;
			else if(op==1) self.OnStartEvent+=v;
			else if(op==2) self.OnStartEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OnTickEvent(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			GameFramework.MyClientThreadEventDelegate v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnTickEvent=v;
			else if(op==1) self.OnTickEvent+=v;
			else if(op==2) self.OnTickEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OnQuitEvent(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			GameFramework.MyClientThreadEventDelegate v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnQuitEvent=v;
			else if(op==1) self.OnQuitEvent+=v;
			else if(op==2) self.OnQuitEvent-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TickSleepTime(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
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
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
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
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
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
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CurActionNum(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurActionNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsCurrentThread(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsCurrentThread);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Thread(IntPtr l) {
		try {
			GameFramework.MyClientThread self=(GameFramework.MyClientThread)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Thread);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.MyClientThread");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32_s);
		addMember(l,ctor__ClientAsyncActionProcessor_s);
		addMember(l,ctor__Int32__Int32_s);
		addMember(l,ctor__Int32__ClientAsyncActionProcessor_s);
		addMember(l,ctor__Int32__Int32__ClientAsyncActionProcessor_s);
		addMember(l,ClearPool);
		addMember(l,Start);
		addMember(l,Stop);
		addMember(l,QueueActionWithDelegation);
		addMember(l,QueueAction);
		addMember(l,QueueFunc);
		addMember(l,"OnStartEvent",null,set_OnStartEvent,true);
		addMember(l,"OnTickEvent",null,set_OnTickEvent,true);
		addMember(l,"OnQuitEvent",null,set_OnQuitEvent,true);
		addMember(l,"TickSleepTime",get_TickSleepTime,set_TickSleepTime,true);
		addMember(l,"ActionNumPerTick",get_ActionNumPerTick,set_ActionNumPerTick,true);
		addMember(l,"CurActionNum",get_CurActionNum,null,true);
		addMember(l,"IsCurrentThread",get_IsCurrentThread,null,true);
		addMember(l,"Thread",get_Thread,null,true);
		createTypeMetatable(l,null, typeof(GameFramework.MyClientThread));
	}
}
