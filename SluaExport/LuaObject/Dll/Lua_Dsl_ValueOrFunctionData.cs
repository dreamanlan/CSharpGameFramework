using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_ValueOrFunctionData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsValue(IntPtr l) {
		try {
			Dsl.ValueOrFunctionData self=(Dsl.ValueOrFunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsFunction(IntPtr l) {
		try {
			Dsl.ValueOrFunctionData self=(Dsl.ValueOrFunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFunction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AsValue(IntPtr l) {
		try {
			Dsl.ValueOrFunctionData self=(Dsl.ValueOrFunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AsValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AsFunction(IntPtr l) {
		try {
			Dsl.ValueOrFunctionData self=(Dsl.ValueOrFunctionData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AsFunction);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.ValueOrFunctionData");
		addMember(l,"IsValue",get_IsValue,null,true);
		addMember(l,"IsFunction",get_IsFunction,null,true);
		addMember(l,"AsValue",get_AsValue,null,true);
		addMember(l,"AsFunction",get_AsFunction,null,true);
		createTypeMetatable(l,null, typeof(Dsl.ValueOrFunctionData),typeof(Dsl.AbstractSyntaxComponent));
	}
}
