using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_SkillDataProvider : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.SkillDataProvider o;
			o=new TableConfig.SkillDataProvider();
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
			TableConfig.SkillDataProvider self=(TableConfig.SkillDataProvider)checkSelf(l);
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
			TableConfig.SkillDataProvider self=(TableConfig.SkillDataProvider)checkSelf(l);
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
			TableConfig.SkillDataProvider self=(TableConfig.SkillDataProvider)checkSelf(l);
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
			TableConfig.SkillDataProvider self=(TableConfig.SkillDataProvider)checkSelf(l);
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
			TableConfig.SkillDataProvider self=(TableConfig.SkillDataProvider)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSkillDataCount(IntPtr l) {
		try {
			TableConfig.SkillDataProvider self=(TableConfig.SkillDataProvider)checkSelf(l);
			var ret=self.GetSkillDataCount();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSkillData(IntPtr l) {
		try {
			TableConfig.SkillDataProvider self=(TableConfig.SkillDataProvider)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSkillData(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SkillDataMgr(IntPtr l) {
		try {
			TableConfig.SkillDataProvider self=(TableConfig.SkillDataProvider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkillDataMgr);
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
			pushValue(l,TableConfig.SkillDataProvider.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.SkillDataProvider");
		addMember(l,LoadForClient);
		addMember(l,LoadForServer);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,Clear);
		addMember(l,GetSkillDataCount);
		addMember(l,GetSkillData);
		addMember(l,"SkillDataMgr",get_SkillDataMgr,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(TableConfig.SkillDataProvider));
	}
}
