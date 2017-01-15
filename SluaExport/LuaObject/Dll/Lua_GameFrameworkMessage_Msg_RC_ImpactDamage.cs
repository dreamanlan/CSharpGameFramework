using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFrameworkMessage_Msg_RC_ImpactDamage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage o;
			o=new GameFrameworkMessage.Msg_RC_ImpactDamage();
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
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
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
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
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
	static public int get_attacker_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.attacker_id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_attacker_id(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.attacker_id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_damage_status(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.damage_status);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_damage_status(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.damage_status=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_hp(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.hp);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_hp(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.hp=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_energy(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.energy);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_energy(IntPtr l) {
		try {
			GameFrameworkMessage.Msg_RC_ImpactDamage self=(GameFrameworkMessage.Msg_RC_ImpactDamage)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.energy=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFrameworkMessage.Msg_RC_ImpactDamage");
		addMember(l,"role_id",get_role_id,set_role_id,true);
		addMember(l,"attacker_id",get_attacker_id,set_attacker_id,true);
		addMember(l,"damage_status",get_damage_status,set_damage_status,true);
		addMember(l,"hp",get_hp,set_hp,true);
		addMember(l,"energy",get_energy,set_energy,true);
		createTypeMetatable(l,constructor, typeof(GameFrameworkMessage.Msg_RC_ImpactDamage));
	}
}
