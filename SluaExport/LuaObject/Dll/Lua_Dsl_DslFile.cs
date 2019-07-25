using System;

using SLua;
using System.Collections.Generic;
public class Lua_Dsl_DslFile : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			Dsl.DslFile o;
			o=new Dsl.DslFile();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddDslInfo(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			Dsl.DslInfo a1;
			checkType(l,2,out a1);
			self.AddDslInfo(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Load(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			Dsl.DslLogDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.Load(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadFromString(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			Dsl.DslLogDelegation a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			var ret=self.LoadFromString(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Save(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Save(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GenerateBinaryCode(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Collections.Generic.Dictionary<System.String,System.String> a2;
			checkType(l,3,out a2);
			Dsl.DslLogDelegation a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			var ret=self.GenerateBinaryCode(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadBinaryFile(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Collections.Generic.Dictionary<System.String,System.String> a2;
			checkType(l,3,out a2);
			self.LoadBinaryFile(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadBinaryCode(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Collections.Generic.Dictionary<System.String,System.String> a2;
			checkType(l,3,out a2);
			self.LoadBinaryCode(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DslInfos(IntPtr l) {
		try {
			Dsl.DslFile self=(Dsl.DslFile)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DslInfos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.DslFile");
		addMember(l,AddDslInfo);
		addMember(l,Load);
		addMember(l,LoadFromString);
		addMember(l,Save);
		addMember(l,GenerateBinaryCode);
		addMember(l,LoadBinaryFile);
		addMember(l,LoadBinaryCode);
		addMember(l,"DslInfos",get_DslInfos,null,true);
		createTypeMetatable(l,constructor, typeof(Dsl.DslFile));
	}
}
