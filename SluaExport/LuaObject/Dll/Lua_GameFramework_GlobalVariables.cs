using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_GlobalVariables : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_TotalPreservedRoomCount(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.GlobalVariables.c_TotalPreservedRoomCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_PreservedRoomCountPerThread(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.GlobalVariables.c_PreservedRoomCountPerThread);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_s_EnableCalculatorLog(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.GlobalVariables.s_EnableCalculatorLog);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_s_EnableCalculatorLog(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			GameFramework.GlobalVariables.s_EnableCalculatorLog=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_s_EnableCalculatorDetailLog(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.GlobalVariables.s_EnableCalculatorDetailLog);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_s_EnableCalculatorDetailLog(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			GameFramework.GlobalVariables.s_EnableCalculatorDetailLog=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_s_EnableCalculatorOperatorLog(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.GlobalVariables.s_EnableCalculatorOperatorLog);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_s_EnableCalculatorOperatorLog(IntPtr l) {
		try {
			System.Boolean v;
			checkType(l,2,out v);
			GameFramework.GlobalVariables.s_EnableCalculatorOperatorLog=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsClient(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsClient);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsClient(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsClient=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsDebug(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsDebug);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsDebug(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsDebug=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsDevice(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsDevice);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsDevice(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsDevice=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsPublish(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsPublish);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsPublish(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsPublish=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsIphone4S(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsIphone4S);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsIphone4S(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsIphone4S=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StoryEditorOpen(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StoryEditorOpen);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StoryEditorOpen(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.StoryEditorOpen=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StoryEditorContinue(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StoryEditorContinue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StoryEditorContinue(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.StoryEditorContinue=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsStorySkipped(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsStorySkipped);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsStorySkipped(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsStorySkipped=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsStorySpeedup(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsStorySpeedup);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsStorySpeedup(IntPtr l) {
		try {
			GameFramework.GlobalVariables self=(GameFramework.GlobalVariables)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsStorySpeedup=v;
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
			pushValue(l,GameFramework.GlobalVariables.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.GlobalVariables");
		addMember(l,"c_TotalPreservedRoomCount",get_c_TotalPreservedRoomCount,null,false);
		addMember(l,"c_PreservedRoomCountPerThread",get_c_PreservedRoomCountPerThread,null,false);
		addMember(l,"s_EnableCalculatorLog",get_s_EnableCalculatorLog,set_s_EnableCalculatorLog,false);
		addMember(l,"s_EnableCalculatorDetailLog",get_s_EnableCalculatorDetailLog,set_s_EnableCalculatorDetailLog,false);
		addMember(l,"s_EnableCalculatorOperatorLog",get_s_EnableCalculatorOperatorLog,set_s_EnableCalculatorOperatorLog,false);
		addMember(l,"IsClient",get_IsClient,set_IsClient,true);
		addMember(l,"IsDebug",get_IsDebug,set_IsDebug,true);
		addMember(l,"IsDevice",get_IsDevice,set_IsDevice,true);
		addMember(l,"IsPublish",get_IsPublish,set_IsPublish,true);
		addMember(l,"IsIphone4S",get_IsIphone4S,set_IsIphone4S,true);
		addMember(l,"StoryEditorOpen",get_StoryEditorOpen,set_StoryEditorOpen,true);
		addMember(l,"StoryEditorContinue",get_StoryEditorContinue,set_StoryEditorContinue,true);
		addMember(l,"IsStorySkipped",get_IsStorySkipped,set_IsStorySkipped,true);
		addMember(l,"IsStorySpeedup",get_IsStorySpeedup,set_IsStorySpeedup,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.GlobalVariables));
	}
}
