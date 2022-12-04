using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_RoleEnter : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnter o;
			o=new GameFrameworkMessage.RoleEnter();
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
	static public int get_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnter self=(GameFrameworkMessage.RoleEnter)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Nickname);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Nickname(IntPtr l) {
		try {
			GameFrameworkMessage.RoleEnter self=(GameFrameworkMessage.RoleEnter)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.RoleEnter");
		addMember(l,ctor_s);
		addMember(l,"m_Nickname",get_m_Nickname,set_m_Nickname,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.RoleEnter));
	}
}
