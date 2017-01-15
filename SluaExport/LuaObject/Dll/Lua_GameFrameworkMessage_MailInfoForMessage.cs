using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_MailInfoForMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage o;
			o=new GameFrameworkMessage.MailInfoForMessage();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_AlreadyRead(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_AlreadyRead);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_AlreadyRead(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.m_AlreadyRead=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_MailGuid(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_MailGuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_MailGuid(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			System.UInt64 v;
			checkType(l,2,out v);
			self.m_MailGuid=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Module(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Module);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Module(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Module=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Title(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Title);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Title(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Title=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Sender(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Sender);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Sender(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Sender=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_SendTime(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_SendTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_SendTime(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_SendTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Text(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Text);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Text(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Text=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Items(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Items);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Money(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Money);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Money(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Money=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Gold(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Gold);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Gold(IntPtr l) {
		try {
			GameFrameworkMessage.MailInfoForMessage self=(GameFrameworkMessage.MailInfoForMessage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Gold=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.MailInfoForMessage");
		addMember(l,"m_AlreadyRead",get_m_AlreadyRead,set_m_AlreadyRead,true);
		addMember(l,"m_MailGuid",get_m_MailGuid,set_m_MailGuid,true);
		addMember(l,"m_Module",get_m_Module,set_m_Module,true);
		addMember(l,"m_Title",get_m_Title,set_m_Title,true);
		addMember(l,"m_Sender",get_m_Sender,set_m_Sender,true);
		addMember(l,"m_SendTime",get_m_SendTime,set_m_SendTime,true);
		addMember(l,"m_Text",get_m_Text,set_m_Text,true);
		addMember(l,"m_Items",get_m_Items,null,true);
		addMember(l,"m_Money",get_m_Money,set_m_Money,true);
		addMember(l,"m_Gold",get_m_Gold,set_m_Gold,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.MailInfoForMessage));
	}
}
