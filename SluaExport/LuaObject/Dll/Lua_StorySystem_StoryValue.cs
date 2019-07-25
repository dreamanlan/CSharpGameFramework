using System;

using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_StoryValue : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			StorySystem.StoryValue o;
			o=new StorySystem.StoryValue();
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
			StorySystem.StoryValue self=(StorySystem.StoryValue)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			self.InitFromDsl(a1);
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
			StorySystem.StoryValue self=(StorySystem.StoryValue)checkSelf(l);
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
	static public int get_c_Iterator(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValue.c_Iterator);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_NotArg(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValue.c_NotArg);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HaveValue(IntPtr l) {
		try {
			StorySystem.StoryValue self=(StorySystem.StoryValue)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HaveValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Value(IntPtr l) {
		try {
			StorySystem.StoryValue self=(StorySystem.StoryValue)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Value);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsConst(IntPtr l) {
		try {
			StorySystem.StoryValue self=(StorySystem.StoryValue)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsConst);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryValue");
		addMember(l,InitFromDsl);
		addMember(l,Evaluate);
		addMember(l,"c_Iterator",get_c_Iterator,null,false);
		addMember(l,"c_NotArg",get_c_NotArg,null,false);
		addMember(l,"HaveValue",get_HaveValue,null,true);
		addMember(l,"Value",get_Value,null,true);
		addMember(l,"IsConst",get_IsConst,null,true);
		createTypeMetatable(l,constructor, typeof(StorySystem.StoryValue));
	}
}
