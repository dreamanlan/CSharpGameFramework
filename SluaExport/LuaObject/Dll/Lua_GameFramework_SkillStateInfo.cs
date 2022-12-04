using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_SkillStateInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.SkillStateInfo o;
			o=new GameFramework.SkillStateInfo();
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
	static public int Reset(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsSkillActivated(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			var ret=self.IsSkillActivated();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddSkill__SkillInfo(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			GameFramework.SkillInfo a1;
			checkType(l,2,out a1);
			self.AddSkill(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddSkill__Int32__SkillInfo(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			GameFramework.SkillInfo a2;
			checkType(l,3,out a2);
			self.AddSkill(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSkillLevel(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSkillLevel(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetTotalSkillLevel(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			var ret=self.GetTotalSkillLevel();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetCurSkillInfo(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			var ret=self.GetCurSkillInfo();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSkillInfoById(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSkillInfoById(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSkillInfoByIndex(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSkillInfoByIndex(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetCurSkillInfo(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetCurSkillInfo(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveSkill(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.RemoveSkill(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveAllSkill(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			self.RemoveAllSkill();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddImpact(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			GameFramework.ImpactInfo a1;
			checkType(l,2,out a1);
			self.AddImpact(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetImpactInfoBySeq(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetImpactInfoBySeq(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FindImpactInfoById(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.FindImpactInfoById(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveImpact(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.RemoveImpact(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveAllImpact(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			self.RemoveAllImpact();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BuffChanged(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BuffChanged);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_BuffChanged(IntPtr l) {
		try {
			GameFramework.SkillStateInfo self=(GameFramework.SkillStateInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.BuffChanged=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.SkillStateInfo");
		addMember(l,ctor_s);
		addMember(l,Reset);
		addMember(l,IsSkillActivated);
		addMember(l,AddSkill__SkillInfo);
		addMember(l,AddSkill__Int32__SkillInfo);
		addMember(l,GetSkillLevel);
		addMember(l,GetTotalSkillLevel);
		addMember(l,GetCurSkillInfo);
		addMember(l,GetSkillInfoById);
		addMember(l,GetSkillInfoByIndex);
		addMember(l,SetCurSkillInfo);
		addMember(l,RemoveSkill);
		addMember(l,RemoveAllSkill);
		addMember(l,AddImpact);
		addMember(l,GetImpactInfoBySeq);
		addMember(l,FindImpactInfoById);
		addMember(l,RemoveImpact);
		addMember(l,RemoveAllImpact);
		addMember(l,"BuffChanged",get_BuffChanged,set_BuffChanged,true);
		createTypeMetatable(l,null, typeof(GameFramework.SkillStateInfo));
	}
}
