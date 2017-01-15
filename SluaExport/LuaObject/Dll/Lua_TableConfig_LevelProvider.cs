using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_LevelProvider : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.LevelProvider o;
			o=new TableConfig.LevelProvider();
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
			TableConfig.LevelProvider self=(TableConfig.LevelProvider)checkSelf(l);
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
			TableConfig.LevelProvider self=(TableConfig.LevelProvider)checkSelf(l);
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
			TableConfig.LevelProvider self=(TableConfig.LevelProvider)checkSelf(l);
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
			TableConfig.LevelProvider self=(TableConfig.LevelProvider)checkSelf(l);
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
			TableConfig.LevelProvider self=(TableConfig.LevelProvider)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLevelCount(IntPtr l) {
		try {
			TableConfig.LevelProvider self=(TableConfig.LevelProvider)checkSelf(l);
			var ret=self.GetLevelCount();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLevel(IntPtr l) {
		try {
			TableConfig.LevelProvider self=(TableConfig.LevelProvider)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetLevel(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LevelMgr(IntPtr l) {
		try {
			TableConfig.LevelProvider self=(TableConfig.LevelProvider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LevelMgr);
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
			pushValue(l,TableConfig.LevelProvider.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.LevelProvider");
		addMember(l,LoadForClient);
		addMember(l,LoadForServer);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,Clear);
		addMember(l,GetLevelCount);
		addMember(l,GetLevel);
		addMember(l,"LevelMgr",get_LevelMgr,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(TableConfig.LevelProvider));
	}
}
