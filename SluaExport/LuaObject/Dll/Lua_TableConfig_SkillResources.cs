using System;

using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_SkillResources : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.SkillResources o;
			o=new TableConfig.SkillResources();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReadFromBinary(IntPtr l) {
		try {
			TableConfig.SkillResources self=(TableConfig.SkillResources)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.ReadFromBinary(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteToBinary(IntPtr l) {
		try {
			TableConfig.SkillResources self=(TableConfig.SkillResources)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			self.WriteToBinary(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_skillId(IntPtr l) {
		try {
			TableConfig.SkillResources self=(TableConfig.SkillResources)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_skillId(IntPtr l) {
		try {
			TableConfig.SkillResources self=(TableConfig.SkillResources)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.skillId=v;
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
			TableConfig.SkillResources self=(TableConfig.SkillResources)checkSelf(l);
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
			TableConfig.SkillResources self=(TableConfig.SkillResources)checkSelf(l);
			System.String v;
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
	static public int get_resource(IntPtr l) {
		try {
			TableConfig.SkillResources self=(TableConfig.SkillResources)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.resource);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_resource(IntPtr l) {
		try {
			TableConfig.SkillResources self=(TableConfig.SkillResources)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.resource=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.SkillResources");
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,"skillId",get_skillId,set_skillId,true);
		addMember(l,"key",get_key,set_key,true);
		addMember(l,"resource",get_resource,set_resource,true);
		createTypeMetatable(l,constructor, typeof(TableConfig.SkillResources));
	}
}
