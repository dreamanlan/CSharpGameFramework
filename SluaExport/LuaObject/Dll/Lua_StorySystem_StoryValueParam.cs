using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryValueParam : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			StorySystem.StoryValueParam o;
			o=new StorySystem.StoryValueParam();
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
	static public int InitFromDsl(IntPtr l) {
		try {
			StorySystem.StoryValueParam self=(StorySystem.StoryValueParam)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			var ret=self.InitFromDsl(a1,a2,a3);
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
	static public int Clone(IntPtr l) {
		try {
			StorySystem.StoryValueParam self=(StorySystem.StoryValueParam)checkSelf(l);
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
	static public int Evaluate(IntPtr l) {
		try {
			StorySystem.StoryValueParam self=(StorySystem.StoryValueParam)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryMessageHandler a2;
			checkType(l,3,out a2);
			BoxedValue a3;
			checkValueType(l,4,out a3);
			BoxedValueList a4;
			checkType(l,5,out a4);
			self.Evaluate(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_HaveValue(IntPtr l) {
		try {
			StorySystem.StoryValueParam self=(StorySystem.StoryValueParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HaveValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryValueParam");
		addMember(l,ctor_s);
		addMember(l,InitFromDsl);
		addMember(l,Clone);
		addMember(l,Evaluate);
		addMember(l,"HaveValue",get_HaveValue,null,true);
		createTypeMetatable(l,null, typeof(StorySystem.StoryValueParam));
	}
}
