using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_InplaceSkillPropertyInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			SkillSystem.InplaceSkillPropertyInfo o;
			o=new SkillSystem.InplaceSkillPropertyInfo();
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
	static public int get_Group(IntPtr l) {
		try {
			SkillSystem.InplaceSkillPropertyInfo self=(SkillSystem.InplaceSkillPropertyInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Group);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Group(IntPtr l) {
		try {
			SkillSystem.InplaceSkillPropertyInfo self=(SkillSystem.InplaceSkillPropertyInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.Group=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Key(IntPtr l) {
		try {
			SkillSystem.InplaceSkillPropertyInfo self=(SkillSystem.InplaceSkillPropertyInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Key);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Key(IntPtr l) {
		try {
			SkillSystem.InplaceSkillPropertyInfo self=(SkillSystem.InplaceSkillPropertyInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.Key=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Property(IntPtr l) {
		try {
			SkillSystem.InplaceSkillPropertyInfo self=(SkillSystem.InplaceSkillPropertyInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Property);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Property(IntPtr l) {
		try {
			SkillSystem.InplaceSkillPropertyInfo self=(SkillSystem.InplaceSkillPropertyInfo)checkSelf(l);
			SkillSystem.IProperty v;
			checkType(l,2,out v);
			self.Property=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.InplaceSkillPropertyInfo");
		addMember(l,ctor_s);
		addMember(l,"Group",get_Group,set_Group,true);
		addMember(l,"Key",get_Key,set_Key,true);
		addMember(l,"Property",get_Property,set_Property,true);
		createTypeMetatable(l,null, typeof(SkillSystem.InplaceSkillPropertyInfo));
	}
}
