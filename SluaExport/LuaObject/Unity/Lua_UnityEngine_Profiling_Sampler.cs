using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Profiling_Sampler : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRecorder(IntPtr l) {
		try {
			UnityEngine.Profiling.Sampler self=(UnityEngine.Profiling.Sampler)checkSelf(l);
			var ret=self.GetRecorder();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Get_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.Profiling.Sampler.Get(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetNames_s(IntPtr l) {
		try {
			System.Collections.Generic.List<System.String> a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.Profiling.Sampler.GetNames(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isValid(IntPtr l) {
		try {
			UnityEngine.Profiling.Sampler self=(UnityEngine.Profiling.Sampler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isValid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_name(IntPtr l) {
		try {
			UnityEngine.Profiling.Sampler self=(UnityEngine.Profiling.Sampler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.Profiling.Sampler");
		addMember(l,GetRecorder);
		addMember(l,Get_s);
		addMember(l,GetNames_s);
		addMember(l,"isValid",get_isValid,null,true);
		addMember(l,"name",get_name,null,true);
		createTypeMetatable(l,null, typeof(UnityEngine.Profiling.Sampler));
	}
}
