using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Skill_GfxSkillSenderInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor__Skill__Int32__Int32__GameObject_s(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo o;
			TableConfig.Skill a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			UnityEngine.GameObject a4;
			checkType(l,4,out a4);
			o=new GameFramework.Skill.GfxSkillSenderInfo(a1,a2,a3,a4);
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
	static public int ctor__Skill__Int32__Int32__GameObject__Int32__GameObject_s(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo o;
			TableConfig.Skill a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			System.Int32 a3;
			checkType(l,3,out a3);
			UnityEngine.GameObject a4;
			checkType(l,4,out a4);
			System.Int32 a5;
			checkType(l,5,out a5);
			UnityEngine.GameObject a6;
			checkType(l,6,out a6);
			o=new GameFramework.Skill.GfxSkillSenderInfo(a1,a2,a3,a4,a5,a6);
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
	static public int get_SkillId(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ConfigData(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ConfigData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Seq(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Seq);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ObjId(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ObjId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_GfxObj(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.GfxObj);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_GfxObj(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.GfxObj=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TargetObjId(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TargetObjId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TargetObjId(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.TargetObjId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TargetGfxObj(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TargetGfxObj);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TargetGfxObj(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.TargetGfxObj=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TrackEffectObj(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TrackEffectObj);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TrackEffectObj(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSenderInfo self=(GameFramework.Skill.GfxSkillSenderInfo)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.TrackEffectObj=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.GfxSkillSenderInfo");
		addMember(l,ctor__Skill__Int32__Int32__GameObject_s);
		addMember(l,ctor__Skill__Int32__Int32__GameObject__Int32__GameObject_s);
		addMember(l,"SkillId",get_SkillId,null,true);
		addMember(l,"ConfigData",get_ConfigData,null,true);
		addMember(l,"Seq",get_Seq,null,true);
		addMember(l,"ObjId",get_ObjId,null,true);
		addMember(l,"GfxObj",get_GfxObj,set_GfxObj,true);
		addMember(l,"TargetObjId",get_TargetObjId,set_TargetObjId,true);
		addMember(l,"TargetGfxObj",get_TargetGfxObj,set_TargetGfxObj,true);
		addMember(l,"TrackEffectObj",get_TrackEffectObj,set_TrackEffectObj,true);
		createTypeMetatable(l,null, typeof(GameFramework.Skill.GfxSkillSenderInfo));
	}
}
