using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_LC_SyncRoleInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo o;
			o=new GameFrameworkMessage.Msg_LC_SyncRoleInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HeroId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo self=(GameFrameworkMessage.Msg_LC_SyncRoleInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HeroId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_HeroId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo self=(GameFrameworkMessage.Msg_LC_SyncRoleInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.HeroId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Money(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo self=(GameFrameworkMessage.Msg_LC_SyncRoleInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Money);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Money(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo self=(GameFrameworkMessage.Msg_LC_SyncRoleInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Money=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Gold(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo self=(GameFrameworkMessage.Msg_LC_SyncRoleInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Gold);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Gold(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo self=(GameFrameworkMessage.Msg_LC_SyncRoleInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Gold=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Level(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo self=(GameFrameworkMessage.Msg_LC_SyncRoleInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Level(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_SyncRoleInfo self=(GameFrameworkMessage.Msg_LC_SyncRoleInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_SyncRoleInfo");
		addMember(l,"HeroId",get_HeroId,set_HeroId,true);
		addMember(l,"Money",get_Money,set_Money,true);
		addMember(l,"Gold",get_Gold,set_Gold,true);
		addMember(l,"Level",get_Level,set_Level,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_LC_SyncRoleInfo));
	}
}
