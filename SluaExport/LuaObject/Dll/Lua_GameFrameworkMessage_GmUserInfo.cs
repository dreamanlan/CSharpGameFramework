using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_GmUserInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo o;
			o=new GameFrameworkMessage.GmUserInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Guid(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Guid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Guid(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.m_Guid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_AccountId(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_AccountId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_AccountId(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_AccountId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Nickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Nickname=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_UserState(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.m_UserState);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_UserState(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			GameFrameworkMessage.GmStateEnum v;
			checkEnum(l,2,out v);
			self.m_UserState=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_UserBasic(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_UserBasic);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_UserBasic(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			GameFrameworkMessage.GmUserBasic v;
			checkType(l,2,out v);
			self.m_UserBasic=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_UserEquips(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_UserEquips);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_UserBagItems(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserInfo self=(GameFrameworkMessage.GmUserInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_UserBagItems);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.GmUserInfo");
		addMember(l,"m_Guid",get_m_Guid,set_m_Guid,true);
		addMember(l,"m_AccountId",get_m_AccountId,set_m_AccountId,true);
		addMember(l,"m_Nickname",get_m_Nickname,set_m_Nickname,true);
		addMember(l,"m_UserState",get_m_UserState,set_m_UserState,true);
		addMember(l,"m_UserBasic",get_m_UserBasic,set_m_UserBasic,true);
		addMember(l,"m_UserEquips",get_m_UserEquips,null,true);
		addMember(l,"m_UserBagItems",get_m_UserBagItems,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.GmUserInfo));
	}
}
