using System;

using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_LevelMonsterProvider : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.LevelMonsterProvider o;
			o=new TableConfig.LevelMonsterProvider();
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
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
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
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
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
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
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
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
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
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLevelMonsterCount(IntPtr l) {
		try {
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
			var ret=self.GetLevelMonsterCount();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int BuildGroupedLevelMonsters(IntPtr l) {
		try {
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
			self.BuildGroupedLevelMonsters();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryGetValue(IntPtr l) {
		try {
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			List<TableConfig.LevelMonster> a2;
			var ret=self.TryGetValue(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LevelMonsterMgr(IntPtr l) {
		try {
			TableConfig.LevelMonsterProvider self=(TableConfig.LevelMonsterProvider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LevelMonsterMgr);
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
			pushValue(l,TableConfig.LevelMonsterProvider.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.LevelMonsterProvider");
		addMember(l,LoadForClient);
		addMember(l,LoadForServer);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,Clear);
		addMember(l,GetLevelMonsterCount);
		addMember(l,BuildGroupedLevelMonsters);
		addMember(l,TryGetValue);
		addMember(l,"LevelMonsterMgr",get_LevelMonsterMgr,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(TableConfig.LevelMonsterProvider));
	}
}
