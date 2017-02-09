using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_ObjStack : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			ObjStack o;
			if(argc==1){
				o=new ObjStack();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==2){
				System.Collections.Generic.IEnumerable<System.Object> a1;
				checkType(l,2,out a1);
				o=new ObjStack(a1);
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
	static public int Clear(IntPtr l) {
		try {
			ObjStack self=(ObjStack)checkSelf(l);
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
			ObjStack self=(ObjStack)checkSelf(l);
			System.Object a1;
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
	static public int Peek(IntPtr l) {
		try {
			ObjStack self=(ObjStack)checkSelf(l);
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
	static public int Pop(IntPtr l) {
		try {
			ObjStack self=(ObjStack)checkSelf(l);
			var ret=self.Pop();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Push(IntPtr l) {
		try {
			ObjStack self=(ObjStack)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.Push(a1);
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
			ObjStack self=(ObjStack)checkSelf(l);
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
			ObjStack self=(ObjStack)checkSelf(l);
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
			ObjStack self=(ObjStack)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"ObjStack");
		addMember(l,Clear);
		addMember(l,Contains);
		addMember(l,Peek);
		addMember(l,Pop);
		addMember(l,Push);
		addMember(l,ToArray);
		addMember(l,TrimExcess);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,constructor, typeof(ObjStack),typeof(System.Collections.Generic.Stack<System.Object>));
	}
}
