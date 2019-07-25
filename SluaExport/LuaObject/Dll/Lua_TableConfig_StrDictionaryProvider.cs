using System;

using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_StrDictionaryProvider : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.StrDictionaryProvider o;
			o=new TableConfig.StrDictionaryProvider();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadForClient(IntPtr l) {
		try {
			TableConfig.StrDictionaryProvider self=(TableConfig.StrDictionaryProvider)checkSelf(l);
			self.LoadForClient();
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
			TableConfig.StrDictionaryProvider self=(TableConfig.StrDictionaryProvider)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Load(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Save(IntPtr l) {
		try {
			TableConfig.StrDictionaryProvider self=(TableConfig.StrDictionaryProvider)checkSelf(l);
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
	static public int Clear(IntPtr l) {
		try {
			TableConfig.StrDictionaryProvider self=(TableConfig.StrDictionaryProvider)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStrDictionaryCount(IntPtr l) {
		try {
			TableConfig.StrDictionaryProvider self=(TableConfig.StrDictionaryProvider)checkSelf(l);
			var ret=self.GetStrDictionaryCount();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStrDictionary(IntPtr l) {
		try {
			TableConfig.StrDictionaryProvider self=(TableConfig.StrDictionaryProvider)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetStrDictionary(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StrDictionaryMgr(IntPtr l) {
		try {
			TableConfig.StrDictionaryProvider self=(TableConfig.StrDictionaryProvider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StrDictionaryMgr);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,TableConfig.StrDictionaryProvider.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.StrDictionaryProvider");
		addMember(l,LoadForClient);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,Clear);
		addMember(l,GetStrDictionaryCount);
		addMember(l,GetStrDictionary);
		addMember(l,"StrDictionaryMgr",get_StrDictionaryMgr,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(TableConfig.StrDictionaryProvider));
	}
}
