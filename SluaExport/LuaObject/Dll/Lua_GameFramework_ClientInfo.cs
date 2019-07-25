using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ClientInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.ClientInfo o;
			o=new GameFramework.ClientInfo();
			pushValue(l,true);
			pushValue(l,o);
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
			pushValue(l,GameFramework.ClientInfo.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Guid(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Guid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Guid(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.Guid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_RoleData(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RoleData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_RoleData(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			GameFrameworkMessage.RoleEnterResult v;
			checkType(l,2,out v);
			self.RoleData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Mails(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Mails);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Mails(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			List<GameFrameworkMessage.MailInfoForMessage> v;
			checkType(l,2,out v);
			self.Mails=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_PropertyKey(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PropertyKey);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_PropertyKey(IntPtr l) {
		try {
			GameFramework.ClientInfo self=(GameFramework.ClientInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.PropertyKey=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ClientInfo");
		addMember(l,"Instance",get_Instance,null,false);
		addMember(l,"Guid",get_Guid,set_Guid,true);
		addMember(l,"RoleData",get_RoleData,set_RoleData,true);
		addMember(l,"Mails",get_Mails,set_Mails,true);
		addMember(l,"PropertyKey",get_PropertyKey,set_PropertyKey,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.ClientInfo));
	}
}
