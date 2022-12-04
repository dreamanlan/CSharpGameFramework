using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFrameworkMessage_GmUserBasic : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic o;
			o=new GameFrameworkMessage.GmUserBasic();
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
	static public int get_m_HeroId(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_HeroId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_HeroId(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_HeroId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_Level(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Level);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Level(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Level=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_Vip(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Vip);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Vip(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_Vip=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_Money(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Money);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Money(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_m_Gold(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Gold);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_Gold(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_m_CreateTime(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_CreateTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_CreateTime(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_CreateTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_LastLogoutTime(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_LastLogoutTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_LastLogoutTime(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.m_LastLogoutTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_GoldCash(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_GoldCash);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_GoldCash(IntPtr l) {
		try {
			GameFrameworkMessage.GmUserBasic self=(GameFrameworkMessage.GmUserBasic)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.m_GoldCash=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.GmUserBasic");
		addMember(l,ctor_s);
		addMember(l,"m_HeroId",get_m_HeroId,set_m_HeroId,true);
		addMember(l,"m_Level",get_m_Level,set_m_Level,true);
		addMember(l,"m_Vip",get_m_Vip,set_m_Vip,true);
		addMember(l,"m_Money",get_m_Money,set_m_Money,true);
		addMember(l,"m_Gold",get_m_Gold,set_m_Gold,true);
		addMember(l,"m_CreateTime",get_m_CreateTime,set_m_CreateTime,true);
		addMember(l,"m_LastLogoutTime",get_m_LastLogoutTime,set_m_LastLogoutTime,true);
		addMember(l,"m_GoldCash",get_m_GoldCash,set_m_GoldCash,true);
		createTypeMetatable(l,null, typeof(GameFrameworkMessage.GmUserBasic));
	}
}
