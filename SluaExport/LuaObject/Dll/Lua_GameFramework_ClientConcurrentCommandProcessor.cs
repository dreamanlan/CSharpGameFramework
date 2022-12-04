using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_ClientConcurrentCommandProcessor : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.ClientConcurrentCommandProcessor o;
			o=new GameFramework.ClientConcurrentCommandProcessor();
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
	static public int QueueAction(IntPtr l) {
		try {
			GameFramework.ClientConcurrentCommandProcessor self=(GameFramework.ClientConcurrentCommandProcessor)checkSelf(l);
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
			GameFramework.ClientConcurrentCommandProcessor self=(GameFramework.ClientConcurrentCommandProcessor)checkSelf(l);
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
	static public int QueueCommand(IntPtr l) {
		try {
			GameFramework.ClientConcurrentCommandProcessor self=(GameFramework.ClientConcurrentCommandProcessor)checkSelf(l);
			GameFramework.IMyCommand a1;
			checkType(l,2,out a1);
			self.QueueCommand(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DequeueCommand(IntPtr l) {
		try {
			GameFramework.ClientConcurrentCommandProcessor self=(GameFramework.ClientConcurrentCommandProcessor)checkSelf(l);
			var ret=self.DequeueCommand();
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
	static public int HandleCommands(IntPtr l) {
		try {
			GameFramework.ClientConcurrentCommandProcessor self=(GameFramework.ClientConcurrentCommandProcessor)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.HandleCommands(a1);
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
			GameFramework.ClientConcurrentCommandProcessor self=(GameFramework.ClientConcurrentCommandProcessor)checkSelf(l);
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
			GameFramework.ClientConcurrentCommandProcessor self=(GameFramework.ClientConcurrentCommandProcessor)checkSelf(l);
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
	static public int get_CurCommandNum(IntPtr l) {
		try {
			GameFramework.ClientConcurrentCommandProcessor self=(GameFramework.ClientConcurrentCommandProcessor)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurCommandNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientConcurrentCommandProcessor");
		addMember(l,ctor_s);
		addMember(l,QueueAction);
		addMember(l,QueueFunc);
		addMember(l,QueueCommand);
		addMember(l,DequeueCommand);
		addMember(l,HandleCommands);
		addMember(l,Reset);
		addMember(l,ClearPool);
		addMember(l,"CurCommandNum",get_CurCommandNum,null,true);
		createTypeMetatable(l,null, typeof(GameFramework.ClientConcurrentCommandProcessor));
	}
}
