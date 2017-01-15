using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Position : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Position o;
			o=new GameFrameworkMessage.Position();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_x(IntPtr l) {
		try {
			GameFrameworkMessage.Position self=(GameFrameworkMessage.Position)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.x);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_x(IntPtr l) {
		try {
			GameFrameworkMessage.Position self=(GameFrameworkMessage.Position)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.x=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_z(IntPtr l) {
		try {
			GameFrameworkMessage.Position self=(GameFrameworkMessage.Position)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.z);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_z(IntPtr l) {
		try {
			GameFrameworkMessage.Position self=(GameFrameworkMessage.Position)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.z=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Position");
		addMember(l,"x",get_x,set_x,true);
		addMember(l,"z",get_z,set_z,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Position));
	}
}
