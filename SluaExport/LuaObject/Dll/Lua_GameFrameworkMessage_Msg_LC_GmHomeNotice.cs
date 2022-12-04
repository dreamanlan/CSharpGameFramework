using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_Msg_LC_GmHomeNotice : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmHomeNotice o;
			o=new GameFrameworkMessage.Msg_LC_GmHomeNotice();
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
	static public int get_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmHomeNotice self=(GameFrameworkMessage.Msg_LC_GmHomeNotice)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.m_Result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Result(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_LC_GmHomeNotice self=(GameFrameworkMessage.Msg_LC_GmHomeNotice)checkSelf(l);
			GameFrameworkMessage.GmResultEnum v;
			checkEnum(l,2,out v);
			self.m_Result=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_LC_GmHomeNotice");
		addMember(l,ctor_s);
		addMember(l,"m_Result",get_m_Result,set_m_Result,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.Msg_LC_GmHomeNotice));
	}
}
