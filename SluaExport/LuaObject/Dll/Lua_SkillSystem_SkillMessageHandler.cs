using System;

using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_SkillMessageHandler : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler o;
			o=new SkillSystem.SkillMessageHandler();
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
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
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
	static public int Load(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
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
	static public int Reset(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Prepare(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
			self.Prepare();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Tick(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
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
	static public int IsOver(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
			var ret=self.IsOver();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_MsgId(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MsgId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CurTime(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsTriggered(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsTriggered);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsTriggered(IntPtr l) {
		try {
			SkillSystem.SkillMessageHandler self=(SkillSystem.SkillMessageHandler)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsTriggered=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillMessageHandler");
		addMember(l,VisitProperties);
		addMember(l,Load);
		addMember(l,Reset);
		addMember(l,Prepare);
		addMember(l,Tick);
		addMember(l,IsOver);
		addMember(l,"MsgId",get_MsgId,null,true);
		addMember(l,"CurTime",get_CurTime,null,true);
		addMember(l,"IsTriggered",get_IsTriggered,set_IsTriggered,true);
		createTypeMetatable(l,constructor, typeof(SkillSystem.SkillMessageHandler));
	}
}
