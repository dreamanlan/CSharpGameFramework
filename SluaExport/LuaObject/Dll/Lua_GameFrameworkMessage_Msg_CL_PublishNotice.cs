using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CL_PublishNotice : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_PublishNotice o;
			o=new GameFrameworkMessage.Msg_CL_PublishNotice();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Content(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_PublishNotice self=(GameFrameworkMessage.Msg_CL_PublishNotice)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Content);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Content(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_PublishNotice self=(GameFrameworkMessage.Msg_CL_PublishNotice)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_Content=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_RollNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_PublishNotice self=(GameFrameworkMessage.Msg_CL_PublishNotice)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_RollNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_RollNum(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_PublishNotice self=(GameFrameworkMessage.Msg_CL_PublishNotice)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_RollNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_PublishNotice");
		addMember(l,"m_Content",get_m_Content,set_m_Content,true);
		addMember(l,"m_RollNum",get_m_RollNum,set_m_RollNum,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CL_PublishNotice));
	}
}
