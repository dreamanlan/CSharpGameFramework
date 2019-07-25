using System;

using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_PropertyAccessorHelper : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int AddProperty(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				SkillSystem.PropertyAccessorHelper self=(SkillSystem.PropertyAccessorHelper)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				SkillSystem.PropertyAccessorHelper.GetDelegation a2;
				LuaDelegation.checkDelegate(l,3,out a2);
				self.AddProperty(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.PropertyAccessorHelper");
		addMember(l,VisitProperties);
		addMember(l,TryGetProperty);
		addMember(l,SetProperty);
		addMember(l,SetGroup);
		addMember(l,AddProperty);
		createTypeMetatable(l,constructor, typeof(SkillSystem.PropertyAccessorHelper));
	}
}
