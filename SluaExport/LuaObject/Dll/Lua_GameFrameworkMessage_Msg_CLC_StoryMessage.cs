using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_CLC_StoryMessage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CLC_StoryMessage");
		addMember(l,ctor_s);
		addMember(l,"m_MsgId",get_m_MsgId,set_m_MsgId,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_CLC_StoryMessage));
	}
}
