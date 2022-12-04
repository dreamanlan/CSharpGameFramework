using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryValueManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RegisterValueFactory__String__IStoryValueFactory(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			StorySystem.IStoryValueFactory a2;
			checkType(l,3,out a2);
			self.RegisterValueFactory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RegisterValueFactory__String__IStoryValueFactory__Boolean(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			StorySystem.IStoryValueFactory a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			self.RegisterValueFactory(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RegisterValueFactory__StoryValueGroupDefine__String__IStoryValueFactory(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			GameFramework.StoryValueGroupDefine a1;
			checkEnum(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			StorySystem.IStoryValueFactory a3;
			checkType(l,4,out a3);
			self.RegisterValueFactory(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RegisterValueFactory__StoryValueGroupDefine__String__IStoryValueFactory__Boolean(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			GameFramework.StoryValueGroupDefine a1;
			checkEnum(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			StorySystem.IStoryValueFactory a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			self.RegisterValueFactory(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FindFactory__String(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.FindFactory(a1);
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
	static public int FindFactory__StoryValueGroupDefine__String(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			GameFramework.StoryValueGroupDefine a1;
			checkEnum(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			var ret=self.FindFactory(a1,a2);
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
	static public int CalcValue(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			var ret=self.CalcValue(a1);
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
	static public int Substitute(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.Substitute(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryGetSubstitute(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			var ret=self.TryGetSubstitute(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ClearSubstitutes(IntPtr l) {
		try {
			StorySystem.StoryValueManager self=(StorySystem.StoryValueManager)checkSelf(l);
			self.ClearSubstitutes();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_MaxValueGroupNum(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValueManager.c_MaxValueGroupNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ThreadValueGroupsMask(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValueManager.ThreadValueGroupsMask);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ThreadValueGroupsMask(IntPtr l) {
		try {
			System.UInt64 v;
			checkType(l,2,out v);
			StorySystem.StoryValueManager.ThreadValueGroupsMask=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,StorySystem.StoryValueManager.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryValueManager");
		addMember(l,RegisterValueFactory__String__IStoryValueFactory);
		addMember(l,RegisterValueFactory__String__IStoryValueFactory__Boolean);
		addMember(l,RegisterValueFactory__StoryValueGroupDefine__String__IStoryValueFactory);
		addMember(l,RegisterValueFactory__StoryValueGroupDefine__String__IStoryValueFactory__Boolean);
		addMember(l,FindFactory__String);
		addMember(l,FindFactory__StoryValueGroupDefine__String);
		addMember(l,CalcValue);
		addMember(l,Substitute);
		addMember(l,TryGetSubstitute);
		addMember(l,ClearSubstitutes);
		addMember(l,"c_MaxValueGroupNum",get_c_MaxValueGroupNum,null,false);
		addMember(l,"ThreadValueGroupsMask",get_ThreadValueGroupsMask,set_ThreadValueGroupsMask,false);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(StorySystem.StoryValueManager));
	}
}
