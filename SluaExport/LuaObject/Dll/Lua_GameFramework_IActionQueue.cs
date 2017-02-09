using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_IActionQueue : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int QueueActionWithDelegation(IntPtr l) {
		try {
			GameFramework.IActionQueue self=(GameFramework.IActionQueue)checkSelf(l);
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
	static public int QueueAction(IntPtr l) {
		try {
			GameFramework.IActionQueue self=(GameFramework.IActionQueue)checkSelf(l);
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
	static public int get_CurActionNum(IntPtr l) {
		try {
			GameFramework.IActionQueue self=(GameFramework.IActionQueue)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurActionNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.IActionQueue");
		addMember(l,QueueActionWithDelegation);
		addMember(l,QueueAction);
		addMember(l,"CurActionNum",get_CurActionNum,null,true);
		createTypeMetatable(l,null, typeof(GameFramework.IActionQueue));
	}
}
