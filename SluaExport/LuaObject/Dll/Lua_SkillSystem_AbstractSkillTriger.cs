﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_AbstractSkillTriger : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int VisitProperties(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	static public int Init(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int Clone(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
			var ret=self.Clone();
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
	static public int InitProperties(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
			self.InitProperties();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reset(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	static public int Execute(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int CopyTo(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
			SkillSystem.ISkillTriger a1;
			checkType(l,2,out a1);
			self.CopyTo(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddProperty(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int SetParam__FunctionData__Int32__String_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			SkillSystem.AbstractSkillTriger.SetParam(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetParam__StatementData__Int32__Int32__String_s(IntPtr l) {
		try {
			Dsl.StatementData a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.String a4;
			checkType(l,4,out a4);
			SkillSystem.AbstractSkillTriger.SetParam(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetStatementParam__FunctionData__Int32__Int32__String_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.String a4;
			checkType(l,4,out a4);
			SkillSystem.AbstractSkillTriger.SetStatementParam(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetStatementParam__StatementData__Int32__Int32__Int32__String_s(IntPtr l) {
		try {
			Dsl.StatementData a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.String a5;
			checkType(l,5,out a5);
			SkillSystem.AbstractSkillTriger.SetStatementParam(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Name(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Name(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_StartTime(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StartTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StartTime(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_OrderInSkill(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OrderInSkill);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OrderInSkill(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_OrderInSection(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.OrderInSection);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OrderInSection(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_IsFinal(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFinal);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsFinal(IntPtr l) {
		try {
			SkillSystem.AbstractSkillTriger self=(SkillSystem.AbstractSkillTriger)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.AbstractSkillTriger");
		addMember(l,VisitProperties);
		addMember(l,TryGetProperty);
		addMember(l,SetProperty);
		addMember(l,Init);
		addMember(l,Clone);
		addMember(l,InitProperties);
		addMember(l,Reset);
		addMember(l,Execute);
		addMember(l,CopyTo);
		addMember(l,AddProperty);
		addMember(l,SetParam__FunctionData__Int32__String_s);
		addMember(l,SetParam__StatementData__Int32__Int32__String_s);
		addMember(l,SetStatementParam__FunctionData__Int32__Int32__String_s);
		addMember(l,SetStatementParam__StatementData__Int32__Int32__Int32__String_s);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"StartTime",get_StartTime,set_StartTime,true);
		addMember(l,"OrderInSkill",get_OrderInSkill,set_OrderInSkill,true);
		addMember(l,"OrderInSection",get_OrderInSection,set_OrderInSection,true);
		addMember(l,"IsFinal",get_IsFinal,set_IsFinal,true);
		createTypeMetatable(l,null, typeof(SkillSystem.AbstractSkillTriger));
	}
}
