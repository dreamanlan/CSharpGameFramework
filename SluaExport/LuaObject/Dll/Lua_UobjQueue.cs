using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UobjQueue : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UobjQueue o;
			o=new UobjQueue();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			UobjQueue self=(UobjQueue)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Contains(IntPtr l) {
		try {
			UobjQueue self=(UobjQueue)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			var ret=self.Contains(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Dequeue(IntPtr l) {
		try {
			UobjQueue self=(UobjQueue)checkSelf(l);
			var ret=self.Dequeue();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Peek(IntPtr l) {
		try {
			UobjQueue self=(UobjQueue)checkSelf(l);
			var ret=self.Peek();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Enqueue(IntPtr l) {
		try {
			UobjQueue self=(UobjQueue)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			self.Enqueue(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToArray(IntPtr l) {
		try {
			UobjQueue self=(UobjQueue)checkSelf(l);
			var ret=self.ToArray();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TrimExcess(IntPtr l) {
		try {
			UobjQueue self=(UobjQueue)checkSelf(l);
			self.TrimExcess();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Count(IntPtr l) {
		try {
			UobjQueue self=(UobjQueue)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UobjQueue");
		addMember(l,Clear);
		addMember(l,Contains);
		addMember(l,Dequeue);
		addMember(l,Peek);
		addMember(l,Enqueue);
		addMember(l,ToArray);
		addMember(l,TrimExcess);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,constructor, typeof(UobjQueue),typeof(System.Collections.Generic.Queue<UnityEngine.Object>));
	}
}
