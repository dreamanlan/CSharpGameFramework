using System;

using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_ISkillTriger : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			self.Init(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int InitProperties(IntPtr l) {
		try {
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
			self.InitProperties();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Execute(IntPtr l) {
		try {
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			System.Int64 a4;
			checkType(l,5,out a4);
			var ret=self.Execute(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StartTime(IntPtr l) {
		try {
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
	static public int get_Name(IntPtr l) {
		try {
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
	static public int get_OrderInSkill(IntPtr l) {
		try {
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
			SkillSystem.ISkillTriger self=(SkillSystem.ISkillTriger)checkSelf(l);
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
		getTypeTable(l,"SkillSystem.ISkillTriger");
		addMember(l,Init);
		addMember(l,InitProperties);
		addMember(l,Reset);
		addMember(l,Execute);
		addMember(l,"StartTime",get_StartTime,set_StartTime,true);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"OrderInSkill",get_OrderInSkill,set_OrderInSkill,true);
		addMember(l,"OrderInSection",get_OrderInSection,set_OrderInSection,true);
		addMember(l,"IsFinal",get_IsFinal,set_IsFinal,true);
		createTypeMetatable(l,null, typeof(SkillSystem.ISkillTriger));
	}
}
