using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryLocalInfoStack : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack o;
			o=new StorySystem.StoryLocalInfoStack();
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
			StorySystem.StoryLocalInfoStack o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new StorySystem.StoryLocalInfoStack(a1);
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
	static public int Clear(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Contains(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
			StorySystem.StoryLocalInfo a1;
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
	[UnityEngine.Scripting.Preserve]
	static public int CopyTo(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
			StorySystem.StoryLocalInfo[] a1;
			checkArray(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.CopyTo(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TrimExcess(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
			self.TrimExcess();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Peek(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int Pop(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int Push(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
			StorySystem.StoryLocalInfo a1;
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
	[UnityEngine.Scripting.Preserve]
	static public int ToArray(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static new public int Equals(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static new public int ToString(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
			var ret=self.ToString();
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
	static public int get_Count(IntPtr l) {
		try {
			StorySystem.StoryLocalInfoStack self=(StorySystem.StoryLocalInfoStack)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryLocalInfoStack");
		addMember(l,ctor_s);
		addMember(l,ctor__Int32_s);
		addMember(l,Clear);
		addMember(l,Contains);
		addMember(l,CopyTo);
		addMember(l,TrimExcess);
		addMember(l,Peek);
		addMember(l,Pop);
		addMember(l,Push);
		addMember(l,ToArray);
		addMember(l,Equals);
		addMember(l,ToString);
		addMember(l,"Count",get_Count,null,true);
		createTypeMetatable(l,null, typeof(StorySystem.StoryLocalInfoStack),typeof(System.Collections.Generic.Stack<StorySystem.StoryLocalInfo>));
	}
}
