using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_DelimiterInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			Dsl.DelimiterInfo o;
			o=new Dsl.DelimiterInfo();
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
	static public int ctor__String__String__String__String_s(IntPtr l) {
		try {
			Dsl.DelimiterInfo o;
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			System.String a4;
			checkType(l,4,out a4);
			o=new Dsl.DelimiterInfo(a1,a2,a3,a4);
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
	static public int get_StringBeginDelimiter(IntPtr l) {
		try {
			Dsl.DelimiterInfo self=(Dsl.DelimiterInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StringBeginDelimiter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StringBeginDelimiter(IntPtr l) {
		try {
			Dsl.DelimiterInfo self=(Dsl.DelimiterInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.StringBeginDelimiter=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StringEndDelimiter(IntPtr l) {
		try {
			Dsl.DelimiterInfo self=(Dsl.DelimiterInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StringEndDelimiter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StringEndDelimiter(IntPtr l) {
		try {
			Dsl.DelimiterInfo self=(Dsl.DelimiterInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.StringEndDelimiter=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ScriptBeginDelimiter(IntPtr l) {
		try {
			Dsl.DelimiterInfo self=(Dsl.DelimiterInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ScriptBeginDelimiter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ScriptBeginDelimiter(IntPtr l) {
		try {
			Dsl.DelimiterInfo self=(Dsl.DelimiterInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.ScriptBeginDelimiter=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ScriptEndDelimiter(IntPtr l) {
		try {
			Dsl.DelimiterInfo self=(Dsl.DelimiterInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ScriptEndDelimiter);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ScriptEndDelimiter(IntPtr l) {
		try {
			Dsl.DelimiterInfo self=(Dsl.DelimiterInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.ScriptEndDelimiter=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.DelimiterInfo");
		addMember(l,ctor_s);
		addMember(l,ctor__String__String__String__String_s);
		addMember(l,"StringBeginDelimiter",get_StringBeginDelimiter,set_StringBeginDelimiter,true);
		addMember(l,"StringEndDelimiter",get_StringEndDelimiter,set_StringEndDelimiter,true);
		addMember(l,"ScriptBeginDelimiter",get_ScriptBeginDelimiter,set_ScriptBeginDelimiter,true);
		addMember(l,"ScriptEndDelimiter",get_ScriptEndDelimiter,set_ScriptEndDelimiter,true);
		createTypeMetatable(l,null, typeof(Dsl.DelimiterInfo));
	}
}
