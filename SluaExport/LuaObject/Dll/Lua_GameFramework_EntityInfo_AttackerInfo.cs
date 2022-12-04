﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_EntityInfo_AttackerInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.EntityInfo.AttackerInfo o;
			o=new GameFramework.EntityInfo.AttackerInfo();
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
	static public int get_m_AttackTime(IntPtr l) {
		try {
			GameFramework.EntityInfo.AttackerInfo self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_AttackTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_AttackTime(IntPtr l) {
		try {
			GameFramework.EntityInfo.AttackerInfo self;
			checkValueType(l,1,out self);
			System.Int64 v;
			checkType(l,2,out v);
			self.m_AttackTime=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_HpDamage(IntPtr l) {
		try {
			GameFramework.EntityInfo.AttackerInfo self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_HpDamage);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_HpDamage(IntPtr l) {
		try {
			GameFramework.EntityInfo.AttackerInfo self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_HpDamage=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_NpDamage(IntPtr l) {
		try {
			GameFramework.EntityInfo.AttackerInfo self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_NpDamage);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_NpDamage(IntPtr l) {
		try {
			GameFramework.EntityInfo.AttackerInfo self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_NpDamage=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.EntityInfo.AttackerInfo");
		addMember(l,ctor_s);
		addMember(l,"m_AttackTime",get_m_AttackTime,set_m_AttackTime,true);
		addMember(l,"m_HpDamage",get_m_HpDamage,set_m_HpDamage,true);
		addMember(l,"m_NpDamage",get_m_NpDamage,set_m_NpDamage,true);
		createTypeMetatable(l,null, typeof(GameFramework.EntityInfo.AttackerInfo),typeof(System.ValueType));
	}
}
