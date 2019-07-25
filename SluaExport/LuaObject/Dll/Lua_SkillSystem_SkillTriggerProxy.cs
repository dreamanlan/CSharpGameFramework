using System;

using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_SkillTriggerProxy : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy o;
			SkillSystem.AbstractSkillTriger a1;
			checkType(l,2,out a1);
			o=new SkillSystem.SkillTriggerProxy(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DoClone(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			SkillSystem.SkillTriggerProxy a1;
			checkType(l,2,out a1);
			self.DoClone(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddProperty(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			SkillSystem.PropertyAccessorHelper.GetDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			SkillSystem.PropertyAccessorHelper.SetDelegation a3;
			LuaDelegation.checkDelegate(l,4,out a3);
			self.AddProperty(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Name(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Name(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.Name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StartTime(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StartTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_StartTime(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.StartTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_OrderInSkill(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OrderInSkill);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OrderInSkill(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.OrderInSkill=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_OrderInSection(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OrderInSection);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_OrderInSection(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.OrderInSection=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsFinal(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFinal);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsFinal(IntPtr l) {
		try {
			SkillSystem.SkillTriggerProxy self=(SkillSystem.SkillTriggerProxy)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsFinal=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillTriggerProxy");
		addMember(l,DoClone);
		addMember(l,AddProperty);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"StartTime",get_StartTime,set_StartTime,true);
		addMember(l,"OrderInSkill",get_OrderInSkill,set_OrderInSkill,true);
		addMember(l,"OrderInSection",get_OrderInSection,set_OrderInSection,true);
		addMember(l,"IsFinal",get_IsFinal,set_IsFinal,true);
		createTypeMetatable(l,constructor, typeof(SkillSystem.SkillTriggerProxy));
	}
}
