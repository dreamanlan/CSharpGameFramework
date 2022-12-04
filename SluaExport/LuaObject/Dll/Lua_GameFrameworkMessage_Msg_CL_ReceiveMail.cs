using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_CL_ReceiveMail : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_ReceiveMail o;
			o=new GameFrameworkMessage.Msg_CL_ReceiveMail();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_MailGuid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_ReceiveMail self=(GameFrameworkMessage.Msg_CL_ReceiveMail)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_MailGuid);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_MailGuid(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CL_ReceiveMail self=(GameFrameworkMessage.Msg_CL_ReceiveMail)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CL_ReceiveMail");
		addMember(l,ctor_s);
		addMember(l,"m_MailGuid",get_m_MailGuid,set_m_MailGuid,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_CL_ReceiveMail));
	}
}
