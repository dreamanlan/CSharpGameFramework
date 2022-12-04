using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_PropertyAccessorHelper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			SkillSystem.PropertyAccessorHelper o;
			o=new SkillSystem.PropertyAccessorHelper();
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
	static public int VisitProperties(IntPtr l) {
		try {
			SkillSystem.PropertyAccessorHelper self=(SkillSystem.PropertyAccessorHelper)checkSelf(l);
			SkillSystem.VisitPropertyDelegation a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.VisitProperties(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryGetProperty(IntPtr l) {
		try {
			SkillSystem.PropertyAccessorHelper self=(SkillSystem.PropertyAccessorHelper)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			var ret=self.TryGetProperty(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetProperty(IntPtr l) {
		try {
			SkillSystem.PropertyAccessorHelper self=(SkillSystem.PropertyAccessorHelper)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.SetProperty(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetGroup(IntPtr l) {
		try {
			SkillSystem.PropertyAccessorHelper self=(SkillSystem.PropertyAccessorHelper)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetGroup(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddProperty__String__GetDelegation(IntPtr l) {
		try {
			SkillSystem.PropertyAccessorHelper self=(SkillSystem.PropertyAccessorHelper)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			SkillSystem.PropertyAccessorHelper.GetDelegation a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			self.AddProperty(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddProperty__String__GetDelegation__SetDelegation(IntPtr l) {
		try {
			SkillSystem.PropertyAccessorHelper self=(SkillSystem.PropertyAccessorHelper)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.PropertyAccessorHelper");
		addMember(l,ctor_s);
		addMember(l,VisitProperties);
		addMember(l,TryGetProperty);
		addMember(l,SetProperty);
		addMember(l,SetGroup);
		addMember(l,AddProperty__String__GetDelegation);
		addMember(l,AddProperty__String__GetDelegation__SetDelegation);
		createTypeMetatable(l,null, typeof(SkillSystem.PropertyAccessorHelper));
	}
}
