using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_EnterSceneResult : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult o;
			o=new GameFrameworkMessage.EnterSceneResult();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_result(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_result(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.result=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_server_ip(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.server_ip);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_server_ip(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.server_ip=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_server_port(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.server_port);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_server_port(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			System.UInt32 v;
			checkType(l,2,out v);
			self.server_port=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_key(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.key);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_key(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			System.UInt32 v;
			checkType(l,2,out v);
			self.key=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_camp_id(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.camp_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_camp_id(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.camp_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_scene_type(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.scene_type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_scene_type(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.scene_type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_prime(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.prime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_prime(IntPtr l) {
		try {
			GameFrameworkMessage.EnterSceneResult self=(GameFrameworkMessage.EnterSceneResult)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.prime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.EnterSceneResult");
		addMember(l,"result",get_result,set_result,true);
		addMember(l,"server_ip",get_server_ip,set_server_ip,true);
		addMember(l,"server_port",get_server_port,set_server_port,true);
		addMember(l,"key",get_key,set_key,true);
		addMember(l,"camp_id",get_camp_id,set_camp_id,true);
		addMember(l,"scene_type",get_scene_type,set_scene_type,true);
		addMember(l,"prime",get_prime,set_prime,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.EnterSceneResult));
	}
}
