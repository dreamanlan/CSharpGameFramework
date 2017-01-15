using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ProxyPublishData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.ProxyPublishData o;
			o=new GameFramework.ProxyPublishData();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_EventName(IntPtr l) {
		try {
			GameFramework.ProxyPublishData self=(GameFramework.ProxyPublishData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_EventName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_EventName(IntPtr l) {
		try {
			GameFramework.ProxyPublishData self=(GameFramework.ProxyPublishData)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.m_EventName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Group(IntPtr l) {
		try {
			GameFramework.ProxyPublishData self=(GameFramework.ProxyPublishData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Group);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Group(IntPtr l) {
		try {
			GameFramework.ProxyPublishData self=(GameFramework.ProxyPublishData)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.m_Group=v;
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
			GameFramework.ProxyPublishData self=(GameFramework.ProxyPublishData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Args);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Args(IntPtr l) {
		try {
			GameFramework.ProxyPublishData self=(GameFramework.ProxyPublishData)checkSelf(l);
			System.Object[] v;
			checkArray(l,2,out v);
			self.m_Args=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ProxyPublishData");
		addMember(l,"m_EventName",get_m_EventName,set_m_EventName,true);
		addMember(l,"m_Group",get_m_Group,set_m_Group,true);
		addMember(l,"m_Args",get_m_Args,set_m_Args,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.ProxyPublishData));
	}
}
