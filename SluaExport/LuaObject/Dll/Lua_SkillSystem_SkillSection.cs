using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_SkillSection : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			SkillSystem.SkillSection o;
			o=new SkillSystem.SkillSection();
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
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
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
	static public int Clone(IntPtr l) {
		try {
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
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
	static public int Load(IntPtr l) {
		try {
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			self.Load(a1,a2);
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
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
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
	static public int Prepare(IntPtr l) {
		try {
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
			self.Prepare();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Tick(IntPtr l) {
		try {
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			self.Tick(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Duration(IntPtr l) {
		try {
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Duration);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Duration(IntPtr l) {
		try {
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
			System.Int64 v;
			checkType(l,2,out v);
			self.Duration=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CurTime(IntPtr l) {
		try {
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsFinished(IntPtr l) {
		try {
			SkillSystem.SkillSection self=(SkillSystem.SkillSection)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFinished);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillSection");
		addMember(l,ctor_s);
		addMember(l,VisitProperties);
		addMember(l,Clone);
		addMember(l,Load);
		addMember(l,Reset);
		addMember(l,Prepare);
		addMember(l,Tick);
		addMember(l,"Duration",get_Duration,set_Duration,true);
		addMember(l,"CurTime",get_CurTime,null,true);
		addMember(l,"IsFinished",get_IsFinished,null,true);
		createTypeMetatable(l,null, typeof(SkillSystem.SkillSection));
	}
}
