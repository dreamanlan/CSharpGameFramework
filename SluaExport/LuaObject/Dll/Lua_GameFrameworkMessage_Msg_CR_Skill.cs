using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_CR_Skill : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill o;
			o=new GameFrameworkMessage.Msg_CR_Skill();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_role_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill self=(GameFrameworkMessage.Msg_CR_Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.role_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_role_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill self=(GameFrameworkMessage.Msg_CR_Skill)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.role_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_skill_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill self=(GameFrameworkMessage.Msg_CR_Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skill_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_skill_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill self=(GameFrameworkMessage.Msg_CR_Skill)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.skill_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_target_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill self=(GameFrameworkMessage.Msg_CR_Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.target_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_target_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill self=(GameFrameworkMessage.Msg_CR_Skill)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.target_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_target_dir(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill self=(GameFrameworkMessage.Msg_CR_Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.target_dir);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_target_dir(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_CR_Skill self=(GameFrameworkMessage.Msg_CR_Skill)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.target_dir=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_CR_Skill");
		addMember(l,"role_id",get_role_id,set_role_id,true);
		addMember(l,"skill_id",get_skill_id,set_skill_id,true);
		addMember(l,"target_id",get_target_id,set_target_id,true);
		addMember(l,"target_dir",get_target_dir,set_target_dir,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_CR_Skill));
	}
}
