using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryValueResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			StorySystem.StoryValueResult o;
			o=new StorySystem.StoryValueResult();
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
	static public int Clone(IntPtr l) {
		try {
			StorySystem.StoryValueResult self=(StorySystem.StoryValueResult)checkSelf(l);
			var ret=self.Clone();
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
	static public int get_HaveValue(IntPtr l) {
		try {
			StorySystem.StoryValueResult self=(StorySystem.StoryValueResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HaveValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_HaveValue(IntPtr l) {
		try {
			StorySystem.StoryValueResult self=(StorySystem.StoryValueResult)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.HaveValue=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Value(IntPtr l) {
		try {
			StorySystem.StoryValueResult self=(StorySystem.StoryValueResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Value.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Value(IntPtr l) {
		try {
			StorySystem.StoryValueResult self=(StorySystem.StoryValueResult)checkSelf(l);
			BoxedValue v;
			checkValueType(l,2,out v);
			self.Value=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryValueResult");
		addMember(l,ctor_s);
		addMember(l,Clone);
		addMember(l,"HaveValue",get_HaveValue,set_HaveValue,true);
		addMember(l,"Value",get_Value,set_Value,true);
		createTypeMetatable(l,null, typeof(StorySystem.StoryValueResult));
	}
}
