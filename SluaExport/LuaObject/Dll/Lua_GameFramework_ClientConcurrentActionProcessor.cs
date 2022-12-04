using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_ClientConcurrentActionProcessor : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.ClientConcurrentActionProcessor o;
			o=new GameFramework.ClientConcurrentActionProcessor();
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
	static public int QueueActionWithDelegation(IntPtr l) {
		try {
			GameFramework.ClientConcurrentActionProcessor self=(GameFramework.ClientConcurrentActionProcessor)checkSelf(l);
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
			GameFramework.ClientConcurrentActionProcessor self=(GameFramework.ClientConcurrentActionProcessor)checkSelf(l);
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
			GameFramework.ClientConcurrentActionProcessor self=(GameFramework.ClientConcurrentActionProcessor)checkSelf(l);
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
	static public int DequeueAction(IntPtr l) {
		try {
			GameFramework.ClientConcurrentActionProcessor self=(GameFramework.ClientConcurrentActionProcessor)checkSelf(l);
			var ret=self.DequeueAction();
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
	static public int HandleActions(IntPtr l) {
		try {
			GameFramework.ClientConcurrentActionProcessor self=(GameFramework.ClientConcurrentActionProcessor)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.HandleActions(a1);
			pushValue(l,true);
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
			GameFramework.ClientConcurrentActionProcessor self=(GameFramework.ClientConcurrentActionProcessor)checkSelf(l);
			self.Reset();
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
			GameFramework.ClientConcurrentActionProcessor self=(GameFramework.ClientConcurrentActionProcessor)checkSelf(l);
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
	static public int get_CurActionNum(IntPtr l) {
		try {
			GameFramework.ClientConcurrentActionProcessor self=(GameFramework.ClientConcurrentActionProcessor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurActionNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientConcurrentActionProcessor");
		addMember(l,ctor_s);
		addMember(l,QueueActionWithDelegation);
		addMember(l,QueueAction);
		addMember(l,QueueFunc);
		addMember(l,DequeueAction);
		addMember(l,HandleActions);
		addMember(l,Reset);
		addMember(l,ClearPool);
		addMember(l,"CurActionNum",get_CurActionNum,null,true);
		createTypeMetatable(l,null, typeof(GameFramework.ClientConcurrentActionProcessor));
	}
}
