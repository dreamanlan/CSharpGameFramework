using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_SkillIntParam : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			SkillSystem.SkillIntParam o;
			o=new SkillSystem.SkillIntParam();
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
	static public int Set__T(IntPtr l) {
		try {
			SkillSystem.SkillIntParam self=(SkillSystem.SkillIntParam)checkSelf(l);
			System.Int32 a1;
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
			SkillSystem.SkillIntParam self=(SkillSystem.SkillIntParam)checkSelf(l);
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
			SkillSystem.SkillIntParam self=(SkillSystem.SkillIntParam)checkSelf(l);
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
	static new public int Equals(IntPtr l) {
		try {
			SkillSystem.SkillIntParam self=(SkillSystem.SkillIntParam)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static new public int ToString(IntPtr l) {
		try {
			SkillSystem.SkillIntParam self=(SkillSystem.SkillIntParam)checkSelf(l);
			var ret=self.ToString();
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
			SkillSystem.SkillIntParam self=(SkillSystem.SkillIntParam)checkSelf(l);
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
			SkillSystem.SkillIntParam self=(SkillSystem.SkillIntParam)checkSelf(l);
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
		getTypeTable(l,"SkillSystem.SkillIntParam");
		addMember(l,ctor_s);
		addMember(l,Set__T);
		addMember(l,Set__ISyntaxComponent);
		addMember(l,Get);
		addMember(l,Equals);
		addMember(l,ToString);
		addMember(l,"EditableValue",get_EditableValue,set_EditableValue,true);
		createTypeMetatable(l,null, typeof(SkillSystem.SkillIntParam),typeof(SkillSystem.SkillNonStringParam<System.Int32>));
	}
}
