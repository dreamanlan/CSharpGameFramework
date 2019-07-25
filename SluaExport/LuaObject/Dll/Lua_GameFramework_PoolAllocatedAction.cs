using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_PoolAllocatedAction : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.PoolAllocatedAction o;
			o=new GameFramework.PoolAllocatedAction();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			GameFramework.PoolAllocatedAction self=(GameFramework.PoolAllocatedAction)checkSelf(l);
			System.Delegate a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			self.Init(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Run(IntPtr l) {
		try {
			GameFramework.PoolAllocatedAction self=(GameFramework.PoolAllocatedAction)checkSelf(l);
			self.Run();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitPool(IntPtr l) {
		try {
			GameFramework.PoolAllocatedAction self=(GameFramework.PoolAllocatedAction)checkSelf(l);
			GameFramework.ObjectPool<GameFramework.PoolAllocatedAction> a1;
			checkType(l,2,out a1);
			self.InitPool(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Downcast(IntPtr l) {
		try {
			GameFramework.PoolAllocatedAction self=(GameFramework.PoolAllocatedAction)checkSelf(l);
			var ret=self.Downcast();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetPoolRecycleLock(IntPtr l) {
		try {
			GameFramework.PoolAllocatedAction self=(GameFramework.PoolAllocatedAction)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.SetPoolRecycleLock(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.PoolAllocatedAction");
		addMember(l,Init);
		addMember(l,Run);
		addMember(l,InitPool);
		addMember(l,Downcast);
		addMember(l,SetPoolRecycleLock);
		createTypeMetatable(l,constructor, typeof(GameFramework.PoolAllocatedAction));
	}
}
