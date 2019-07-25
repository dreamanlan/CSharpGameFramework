using System;

using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_AttrDefineProvider : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.AttrDefineProvider o;
			o=new TableConfig.AttrDefineProvider();
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
			TableConfig.AttrDefineProvider self=(TableConfig.AttrDefineProvider)checkSelf(l);
			self.LoadForClient();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadForServer(IntPtr l) {
		try {
			TableConfig.AttrDefineProvider self=(TableConfig.AttrDefineProvider)checkSelf(l);
			self.LoadForServer();
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
			TableConfig.AttrDefineProvider self=(TableConfig.AttrDefineProvider)checkSelf(l);
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
			TableConfig.AttrDefineProvider self=(TableConfig.AttrDefineProvider)checkSelf(l);
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
			TableConfig.AttrDefineProvider self=(TableConfig.AttrDefineProvider)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAttrDefineCount(IntPtr l) {
		try {
			TableConfig.AttrDefineProvider self=(TableConfig.AttrDefineProvider)checkSelf(l);
			var ret=self.GetAttrDefineCount();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAttrDefine(IntPtr l) {
		try {
			TableConfig.AttrDefineProvider self=(TableConfig.AttrDefineProvider)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetAttrDefine(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_AttrDefineMgr(IntPtr l) {
		try {
			TableConfig.AttrDefineProvider self=(TableConfig.AttrDefineProvider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AttrDefineMgr);
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
			pushValue(l,TableConfig.AttrDefineProvider.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.AttrDefineProvider");
		addMember(l,LoadForClient);
		addMember(l,LoadForServer);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,Clear);
		addMember(l,GetAttrDefineCount);
		addMember(l,GetAttrDefine);
		addMember(l,"AttrDefineMgr",get_AttrDefineMgr,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(TableConfig.AttrDefineProvider));
	}
}
