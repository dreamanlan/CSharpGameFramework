using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_NullSyntax : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			Dsl.NullSyntax o;
			o=new Dsl.NullSyntax();
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
	static public int IsValid(IntPtr l) {
		try {
			Dsl.NullSyntax self=(Dsl.NullSyntax)checkSelf(l);
			var ret=self.IsValid();
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
	static public int GetId(IntPtr l) {
		try {
			Dsl.NullSyntax self=(Dsl.NullSyntax)checkSelf(l);
			var ret=self.GetId();
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
	static public int GetIdType(IntPtr l) {
		try {
			Dsl.NullSyntax self=(Dsl.NullSyntax)checkSelf(l);
			var ret=self.GetIdType();
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
	static public int GetLine(IntPtr l) {
		try {
			Dsl.NullSyntax self=(Dsl.NullSyntax)checkSelf(l);
			var ret=self.GetLine();
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
	static public int ToScriptString(IntPtr l) {
		try {
			Dsl.NullSyntax self=(Dsl.NullSyntax)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int HaveId(IntPtr l) {
		try {
			Dsl.NullSyntax self=(Dsl.NullSyntax)checkSelf(l);
			var ret=self.HaveId();
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
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.NullSyntax.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.NullSyntax");
		addMember(l,ctor_s);
		addMember(l,IsValid);
		addMember(l,GetId);
		addMember(l,GetIdType);
		addMember(l,GetLine);
		addMember(l,ToScriptString);
		addMember(l,HaveId);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(Dsl.NullSyntax),typeof(Dsl.AbstractSyntaxComponent));
	}
}
