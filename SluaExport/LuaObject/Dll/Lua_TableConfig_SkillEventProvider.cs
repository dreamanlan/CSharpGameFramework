using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_SkillEventProvider : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.SkillEventProvider o;
			o=new TableConfig.SkillEventProvider();
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
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
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
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
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
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
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
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
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
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSkillEventCount(IntPtr l) {
		try {
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
			var ret=self.GetSkillEventCount();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_skillEventTable(IntPtr l) {
		try {
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skillEventTable);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_skillEventTable(IntPtr l) {
		try {
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
			System.Collections.Generic.Dictionary<System.Int32,System.Collections.Generic.Dictionary<System.Int32,System.Collections.Generic.Dictionary<System.Int32,TableConfig.SkillEvent>>> v;
			checkType(l,2,out v);
			self.skillEventTable=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SkillEventMgr(IntPtr l) {
		try {
			TableConfig.SkillEventProvider self=(TableConfig.SkillEventProvider)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkillEventMgr);
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
			pushValue(l,TableConfig.SkillEventProvider.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.SkillEventProvider");
		addMember(l,LoadForClient);
		addMember(l,LoadForServer);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,Clear);
		addMember(l,GetSkillEventCount);
		addMember(l,"skillEventTable",get_skillEventTable,set_skillEventTable,true);
		addMember(l,"SkillEventMgr",get_SkillEventMgr,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(TableConfig.SkillEventProvider));
	}
}
