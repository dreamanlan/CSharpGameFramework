using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_CommonValues_DummyValue : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			StorySystem.CommonValues.DummyValue o;
			o=new StorySystem.CommonValues.DummyValue();
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
			StorySystem.CommonValues.DummyValue self=(StorySystem.CommonValues.DummyValue)checkSelf(l);
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
			StorySystem.CommonValues.DummyValue self=(StorySystem.CommonValues.DummyValue)checkSelf(l);
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
	static public int Analyze(IntPtr l) {
		try {
			StorySystem.CommonValues.DummyValue self=(StorySystem.CommonValues.DummyValue)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			self.Analyze(a1);
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
			StorySystem.CommonValues.DummyValue self=(StorySystem.CommonValues.DummyValue)checkSelf(l);
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
			StorySystem.CommonValues.DummyValue self=(StorySystem.CommonValues.DummyValue)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Value);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.CommonValues.DummyValue");
		addMember(l,InitFromDsl);
		addMember(l,Evaluate);
		addMember(l,Analyze);
		addMember(l,"HaveValue",get_HaveValue,null,true);
		addMember(l,"Value",get_Value,null,true);
		createTypeMetatable(l,constructor, typeof(StorySystem.CommonValues.DummyValue));
	}
}
