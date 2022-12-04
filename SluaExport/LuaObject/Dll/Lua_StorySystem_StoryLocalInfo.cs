using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryLocalInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo o;
			o=new StorySystem.StoryLocalInfo();
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
	static public int Reset(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetLocalInfo(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetLocalInfo(a1);
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
	static public int SetLocalInfo(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.SetLocalInfo(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int New_s(IntPtr l) {
		try {
			var ret=StorySystem.StoryLocalInfo.New();
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
	static public int Recycle_s(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo a1;
			checkType(l,1,out a1);
			StorySystem.StoryLocalInfo.Recycle(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LocalInfos(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LocalInfos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LocalInfos(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			IntObjDict v;
			checkType(l,2,out v);
			self.LocalInfos=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StackVariables(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StackVariables);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StackVariables(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			StrBoxedValueDict v;
			checkType(l,2,out v);
			self.StackVariables=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Args(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Args);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Args(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			StorySystem.IStoryValueList v;
			checkType(l,2,out v);
			self.Args=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_OptArgs(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OptArgs);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OptArgs(IntPtr l) {
		try {
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
			StorySystem.StrIStoryValueDict v;
			checkType(l,2,out v);
			self.OptArgs=v;
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
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
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
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
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
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
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
			StorySystem.StoryLocalInfo self=(StorySystem.StoryLocalInfo)checkSelf(l);
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
		getTypeTable(l,"StorySystem.StoryLocalInfo");
		addMember(l,ctor_s);
		addMember(l,Reset);
		addMember(l,GetLocalInfo);
		addMember(l,SetLocalInfo);
		addMember(l,New_s);
		addMember(l,Recycle_s);
		addMember(l,"LocalInfos",get_LocalInfos,set_LocalInfos,true);
		addMember(l,"StackVariables",get_StackVariables,set_StackVariables,true);
		addMember(l,"Args",get_Args,set_Args,true);
		addMember(l,"OptArgs",get_OptArgs,set_OptArgs,true);
		addMember(l,"HaveValue",get_HaveValue,set_HaveValue,true);
		addMember(l,"Value",get_Value,set_Value,true);
		createTypeMetatable(l,null, typeof(StorySystem.StoryLocalInfo));
	}
}
