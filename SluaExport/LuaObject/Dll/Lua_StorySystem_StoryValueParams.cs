using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_StoryValueParams : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			StorySystem.StoryValueParams o;
			o=new StorySystem.StoryValueParams();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitFromDsl(IntPtr l) {
		try {
			StorySystem.StoryValueParams self=(StorySystem.StoryValueParams)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.InitFromDsl(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Evaluate(IntPtr l) {
		try {
			StorySystem.StoryValueParams self=(StorySystem.StoryValueParams)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			System.Object[] a3;
			checkArray(l,4,out a3);
			self.Evaluate(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HaveValue(IntPtr l) {
		try {
			StorySystem.StoryValueParams self=(StorySystem.StoryValueParams)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HaveValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Values(IntPtr l) {
		try {
			StorySystem.StoryValueParams self=(StorySystem.StoryValueParams)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Values);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryValueParams");
		addMember(l,InitFromDsl);
		addMember(l,Evaluate);
		addMember(l,"HaveValue",get_HaveValue,null,true);
		addMember(l,"Values",get_Values,null,true);
		createTypeMetatable(l,constructor, typeof(StorySystem.StoryValueParams));
	}
}
