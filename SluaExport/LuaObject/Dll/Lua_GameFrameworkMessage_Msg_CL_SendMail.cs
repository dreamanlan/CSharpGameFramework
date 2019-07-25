using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_SendMail : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail o;
			o=new GameFrameworkMessage.Msg_CL_SendMail();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Receiver(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Receiver);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Receiver(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Receiver=v;
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
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
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
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
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
	static public int get_m_Text(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
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
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
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
	static public int get_m_ExpiryDate(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ExpiryDate);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_ExpiryDate(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_ExpiryDate=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_LevelDemand(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_LevelDemand);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_LevelDemand(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_LevelDemand=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ItemId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_ItemId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_ItemId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_ItemNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ItemNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_ItemNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_ItemNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Money(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
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
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
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
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
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
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Stamina(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Stamina);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Stamina(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_SendMail self=(GameFrameworkMessage.Msg_CL_SendMail)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Stamina=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_SendMail");
		addMember(l,"m_Receiver",get_m_Receiver,set_m_Receiver,true);
		addMember(l,"m_Title",get_m_Title,set_m_Title,true);
		addMember(l,"m_Text",get_m_Text,set_m_Text,true);
		addMember(l,"m_ExpiryDate",get_m_ExpiryDate,set_m_ExpiryDate,true);
		addMember(l,"m_LevelDemand",get_m_LevelDemand,set_m_LevelDemand,true);
		addMember(l,"m_ItemId",get_m_ItemId,set_m_ItemId,true);
		addMember(l,"m_ItemNum",get_m_ItemNum,set_m_ItemNum,true);
		addMember(l,"m_Money",get_m_Money,set_m_Money,true);
		addMember(l,"m_Gold",get_m_Gold,set_m_Gold,true);
		addMember(l,"m_Stamina",get_m_Stamina,set_m_Stamina,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_SendMail));
	}
}
