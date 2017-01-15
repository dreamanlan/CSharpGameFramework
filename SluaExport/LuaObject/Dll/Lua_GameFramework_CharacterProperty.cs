using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_CharacterProperty : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.CharacterProperty o;
			o=new GameFramework.CharacterProperty();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetFloat(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			var ret=self.GetFloat(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetFloat(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.SetFloat(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IncreaseFloat(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.IncreaseFloat(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetInt(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			var ret=self.GetInt(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetInt(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.SetInt(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IncreaseInt(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.IncreaseInt(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLong(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			var ret=self.GetLong(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetLong(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			self.SetLong(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IncreaseLong(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterPropertyEnum a1;
			checkEnum(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			self.IncreaseLong(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyFrom(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.CharacterProperty a1;
			checkType(l,2,out a1);
			self.CopyFrom(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ConfigData(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ConfigData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ConfigData(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			TableConfig.Actor v;
			checkType(l,2,out v);
			self.ConfigData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Owner(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Owner);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Owner(IntPtr l) {
		try {
			GameFramework.CharacterProperty self=(GameFramework.CharacterProperty)checkSelf(l);
			GameFramework.EntityInfo v;
			checkType(l,2,out v);
			self.Owner=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.CharacterProperty");
		addMember(l,GetFloat);
		addMember(l,SetFloat);
		addMember(l,IncreaseFloat);
		addMember(l,GetInt);
		addMember(l,SetInt);
		addMember(l,IncreaseInt);
		addMember(l,GetLong);
		addMember(l,SetLong);
		addMember(l,IncreaseLong);
		addMember(l,CopyFrom);
		addMember(l,"ConfigData",get_ConfigData,set_ConfigData,true);
		addMember(l,"Owner",get_Owner,set_Owner,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.CharacterProperty));
	}
}
