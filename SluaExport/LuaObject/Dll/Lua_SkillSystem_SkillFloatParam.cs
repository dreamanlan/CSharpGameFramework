using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_SkillFloatParam : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			SkillSystem.SkillFloatParam o;
			o=new SkillSystem.SkillFloatParam();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyFrom(IntPtr l) {
		try {
			SkillSystem.SkillFloatParam self=(SkillSystem.SkillFloatParam)checkSelf(l);
			SkillSystem.SkillNonStringParam<System.Single> a1;
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
	static public int Set(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(Dsl.ISyntaxComponent))){
				SkillSystem.SkillFloatParam self=(SkillSystem.SkillFloatParam)checkSelf(l);
				Dsl.ISyntaxComponent a1;
				checkType(l,2,out a1);
				self.Set(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(float))){
				SkillSystem.SkillFloatParam self=(SkillSystem.SkillFloatParam)checkSelf(l);
				System.Single a1;
				checkType(l,2,out a1);
				self.Set(a1);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Get(IntPtr l) {
		try {
			SkillSystem.SkillFloatParam self=(SkillSystem.SkillFloatParam)checkSelf(l);
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
	static public int get_EditableValue(IntPtr l) {
		try {
			SkillSystem.SkillFloatParam self=(SkillSystem.SkillFloatParam)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EditableValue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_EditableValue(IntPtr l) {
		try {
			SkillSystem.SkillFloatParam self=(SkillSystem.SkillFloatParam)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.SkillFloatParam");
		addMember(l,CopyFrom);
		addMember(l,Set);
		addMember(l,Get);
		addMember(l,"EditableValue",get_EditableValue,set_EditableValue,true);
		createTypeMetatable(l,constructor, typeof(SkillSystem.SkillFloatParam),typeof(SkillSystem.SkillNonStringParam<System.Single>));
	}
}
