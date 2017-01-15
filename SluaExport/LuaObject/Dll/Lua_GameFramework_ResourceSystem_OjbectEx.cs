using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ResourceSystem_OjbectEx : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.ResourceSystem.OjbectEx o;
			o=new GameFramework.ResourceSystem.OjbectEx();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_obj(IntPtr l) {
		try {
			GameFramework.ResourceSystem.OjbectEx self=(GameFramework.ResourceSystem.OjbectEx)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.obj);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_obj(IntPtr l) {
		try {
			GameFramework.ResourceSystem.OjbectEx self=(GameFramework.ResourceSystem.OjbectEx)checkSelf(l);
			UnityEngine.Object v;
			checkType(l,2,out v);
			self.obj=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_notdestroyed(IntPtr l) {
		try {
			GameFramework.ResourceSystem.OjbectEx self=(GameFramework.ResourceSystem.OjbectEx)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.notdestroyed);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_notdestroyed(IntPtr l) {
		try {
			GameFramework.ResourceSystem.OjbectEx self=(GameFramework.ResourceSystem.OjbectEx)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.notdestroyed=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ResourceSystem.OjbectEx");
		addMember(l,"obj",get_obj,set_obj,true);
		addMember(l,"notdestroyed",get_notdestroyed,set_notdestroyed,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.ResourceSystem.OjbectEx));
	}
}
