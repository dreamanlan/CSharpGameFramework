using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_SkillDamageData : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.SkillDamageData o;
			o=new TableConfig.SkillDamageData();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetMultiple(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetMultiple(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetDamage(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetDamage(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetVampire(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetVampire(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			self.Init();
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
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			TableConfig.SkillDamageData a1;
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
	static public int Merge(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			TableConfig.SkillDamageData a1;
			checkType(l,2,out a1);
			self.Merge(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Multiples(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Multiples);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Multiples(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Collections.Generic.List<System.Int32> v;
			checkType(l,2,out v);
			self.Multiples=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Damages(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Damages);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Damages(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Collections.Generic.List<System.Int32> v;
			checkType(l,2,out v);
			self.Damages=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Vampires(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Vampires);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Vampires(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Collections.Generic.List<System.Int32> v;
			checkType(l,2,out v);
			self.Vampires=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_AddSc(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AddSc);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_AddSc(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.AddSc=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_AddUc(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AddUc);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_AddUc(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.AddUc=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsCritical(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsCritical);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsCritical(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.IsCritical=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsBlock(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsBlock);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsBlock(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.IsBlock=v;
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
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
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
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.IsFinal=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ComboSource(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ComboSource);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ComboSource(IntPtr l) {
		try {
			TableConfig.SkillDamageData self=(TableConfig.SkillDamageData)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.ComboSource=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.SkillDamageData");
		addMember(l,GetMultiple);
		addMember(l,GetDamage);
		addMember(l,GetVampire);
		addMember(l,Init);
		addMember(l,CopyFrom);
		addMember(l,Merge);
		addMember(l,"Multiples",get_Multiples,set_Multiples,true);
		addMember(l,"Damages",get_Damages,set_Damages,true);
		addMember(l,"Vampires",get_Vampires,set_Vampires,true);
		addMember(l,"AddSc",get_AddSc,set_AddSc,true);
		addMember(l,"AddUc",get_AddUc,set_AddUc,true);
		addMember(l,"IsCritical",get_IsCritical,set_IsCritical,true);
		addMember(l,"IsBlock",get_IsBlock,set_IsBlock,true);
		addMember(l,"IsFinal",get_IsFinal,set_IsFinal,true);
		addMember(l,"ComboSource",get_ComboSource,set_ComboSource,true);
		createTypeMetatable(l,constructor, typeof(TableConfig.SkillDamageData));
	}
}
