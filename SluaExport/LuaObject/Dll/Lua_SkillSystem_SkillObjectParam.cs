using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_SkillObjectParam : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			SkillSystem.SkillObjectParam o;
			o=new SkillSystem.SkillObjectParam();
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
	static public int CopyFrom(IntPtr l) {
		try {
			SkillSystem.SkillObjectParam self=(SkillSystem.SkillObjectParam)checkSelf(l);
			SkillSystem.SkillObjectParam a1;
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
	[UnityEngine.Scripting.Preserve]
	static public int Set__Object(IntPtr l) {
		try {
			SkillSystem.SkillObjectParam self=(SkillSystem.SkillObjectParam)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			self.Set(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Set__ISyntaxComponent(IntPtr l) {
		try {
			SkillSystem.SkillObjectParam self=(SkillSystem.SkillObjectParam)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			self.Set(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Get(IntPtr l) {
		try {
			SkillSystem.SkillObjectParam self=(SkillSystem.SkillObjectParam)checkSelf(l);
			SkillSystem.SkillInstance a1;
			checkType(l,2,out a1);
			var ret=self.Get(a1);
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
	static public int get_EditableValue(IntPtr l) {
		try {
			SkillSystem.SkillObjectParam self=(SkillSystem.SkillObjectParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EditableValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_EditableValue(IntPtr l) {
		try {
			SkillSystem.SkillObjectParam self=(SkillSystem.SkillObjectParam)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.EditableValue=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillObjectParam");
		addMember(l,ctor_s);
		addMember(l,CopyFrom);
		addMember(l,Set__Object);
		addMember(l,Set__ISyntaxComponent);
		addMember(l,Get);
		addMember(l,"EditableValue",get_EditableValue,set_EditableValue,true);
		createTypeMetatable(l,null, typeof(SkillSystem.SkillObjectParam));
	}
}
