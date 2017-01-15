using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CLC_StoryMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CLC_StoryMessage o;
			o=new GameFrameworkMessage.Msg_CLC_StoryMessage();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_MsgId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CLC_StoryMessage self=(GameFrameworkMessage.Msg_CLC_StoryMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_MsgId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_MsgId(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CLC_StoryMessage self=(GameFrameworkMessage.Msg_CLC_StoryMessage)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_MsgId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Args(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CLC_StoryMessage self=(GameFrameworkMessage.Msg_CLC_StoryMessage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Args);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CLC_StoryMessage");
		addMember(l,"m_MsgId",get_m_MsgId,set_m_MsgId,true);
		addMember(l,"m_Args",get_m_Args,null,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CLC_StoryMessage));
	}
}
