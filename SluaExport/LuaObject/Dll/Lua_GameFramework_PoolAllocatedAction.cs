using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_PoolAllocatedAction : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.PoolAllocatedAction");
		addMember(l,ctor_s);
		addMember(l,Init);
		addMember(l,Run);
		addMember(l,Downcast);
		addMember(l,SetPoolRecycleLock);
		createTypeMetatable(l,null, typeof(GameFramework.PoolAllocatedAction));
	}
}
