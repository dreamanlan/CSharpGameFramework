using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_SimpleIntListPool : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.SimpleIntListPool o;
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			o=new GameFramework.SimpleIntListPool(a1,a2);
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
	static public int Alloc(IntPtr l) {
		try {
			GameFramework.SimpleIntListPool self=(GameFramework.SimpleIntListPool)checkSelf(l);
			var ret=self.Alloc();
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
	static public int Recycle(IntPtr l) {
		try {
			GameFramework.SimpleIntListPool self=(GameFramework.SimpleIntListPool)checkSelf(l);
			IntList a1;
			checkType(l,2,out a1);
			self.Recycle(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SimpleIntListPool");
		addMember(l,ctor_s);
		addMember(l,Alloc);
		addMember(l,Recycle);
		createTypeMetatable(l,null, typeof(GameFramework.SimpleIntListPool));
	}
}
