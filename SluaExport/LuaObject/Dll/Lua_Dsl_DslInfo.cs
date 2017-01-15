using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Dsl_DslInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Dsl.DslInfo o;
			o=new Dsl.DslInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToScriptString(IntPtr l) {
		try {
			Dsl.DslInfo self=(Dsl.DslInfo)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			var ret=self.ToScriptString(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetLoaded(IntPtr l) {
		try {
			Dsl.DslInfo self=(Dsl.DslInfo)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.SetLoaded(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsLoaded(IntPtr l) {
		try {
			Dsl.DslInfo self=(Dsl.DslInfo)checkSelf(l);
			var ret=self.IsLoaded();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetResourceName(IntPtr l) {
		try {
			Dsl.DslInfo self=(Dsl.DslInfo)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetResourceName(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetResourceName(IntPtr l) {
		try {
			Dsl.DslInfo self=(Dsl.DslInfo)checkSelf(l);
			var ret=self.GetResourceName();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			Dsl.DslInfo self=(Dsl.DslInfo)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyFrom(IntPtr l) {
		try {
			Dsl.DslInfo self=(Dsl.DslInfo)checkSelf(l);
			Dsl.DslInfo a1;
			checkType(l,2,out a1);
			self.CopyFrom(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.DslInfo");
		addMember(l,ToScriptString);
		addMember(l,SetLoaded);
		addMember(l,IsLoaded);
		addMember(l,SetResourceName);
		addMember(l,GetResourceName);
		addMember(l,Clear);
		addMember(l,CopyFrom);
		createTypeMetatable(l,constructor, typeof(Dsl.DslInfo),typeof(Dsl.StatementData));
	}
}
