using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SceneLogicConfig : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.SceneLogicConfig o;
			o=new GameFramework.SceneLogicConfig();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_ConfigId(IntPtr l) {
		try {
			GameFramework.SceneLogicConfig self=(GameFramework.SceneLogicConfig)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_ConfigId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_ConfigId(IntPtr l) {
		try {
			GameFramework.SceneLogicConfig self=(GameFramework.SceneLogicConfig)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_ConfigId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_LogicId(IntPtr l) {
		try {
			GameFramework.SceneLogicConfig self=(GameFramework.SceneLogicConfig)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_LogicId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_LogicId(IntPtr l) {
		try {
			GameFramework.SceneLogicConfig self=(GameFramework.SceneLogicConfig)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_LogicId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Params(IntPtr l) {
		try {
			GameFramework.SceneLogicConfig self=(GameFramework.SceneLogicConfig)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_Params);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Params(IntPtr l) {
		try {
			GameFramework.SceneLogicConfig self=(GameFramework.SceneLogicConfig)checkSelf(l);
			System.String[] v;
			checkArray(l,2,out v);
			self.m_Params=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SceneLogicConfig");
		addMember(l,"m_ConfigId",get_m_ConfigId,set_m_ConfigId,true);
		addMember(l,"m_LogicId",get_m_LogicId,set_m_LogicId,true);
		addMember(l,"m_Params",get_m_Params,set_m_Params,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.SceneLogicConfig));
	}
}
