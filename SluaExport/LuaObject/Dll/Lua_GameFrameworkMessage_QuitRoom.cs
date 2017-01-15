using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_QuitRoom : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.QuitRoom o;
			o=new GameFrameworkMessage.QuitRoom();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_IsQuitRoom(IntPtr l) {
		try {
			GameFrameworkMessage.QuitRoom self=(GameFrameworkMessage.QuitRoom)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_IsQuitRoom);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_IsQuitRoom(IntPtr l) {
		try {
			GameFrameworkMessage.QuitRoom self=(GameFrameworkMessage.QuitRoom)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.m_IsQuitRoom=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.QuitRoom");
		addMember(l,"m_IsQuitRoom",get_m_IsQuitRoom,set_m_IsQuitRoom,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.QuitRoom));
	}
}
