using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_ImpactInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			GameFramework.ImpactInfo o;
			if(matchType(l,argc,2,typeof(int))){
				System.Int32 a1;
				checkType(l,2,out a1);
				o=new GameFramework.ImpactInfo(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(TableConfig.Skill))){
				TableConfig.Skill a1;
				checkType(l,2,out a1);
				o=new GameFramework.ImpactInfo(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RefixCharacterProperty(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			self.RefixCharacterProperty(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Seq(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Seq);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Seq(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Seq=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ImpactId(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ImpactId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ImpactId(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.ImpactId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SkillId(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkillId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SkillId(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.SkillId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ImpactSenderId(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ImpactSenderId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ImpactSenderId(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.ImpactSenderId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SenderPosition(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SenderPosition);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SenderPosition(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.SenderPosition=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_TargetType(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TargetType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_TargetType(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.TargetType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StartTime(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StartTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_StartTime(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
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
	static public int get_DurationTime(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DurationTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DurationTime(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.DurationTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_CurDamageCount(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CurDamageCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_CurDamageCount(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.CurDamageCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ImpactToTarget(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ImpactToTarget);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ImpactToTarget(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.ImpactToTarget=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ConfigData(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ConfigData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ConfigData(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			TableConfig.Skill v;
			checkType(l,2,out v);
			self.ConfigData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_DamageData(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DamageData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DamageData(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			TableConfig.SkillDamageData v;
			checkType(l,2,out v);
			self.DamageData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SenderProperty(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SenderProperty);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SenderProperty(IntPtr l) {
		try {
			GameFramework.ImpactInfo self=(GameFramework.ImpactInfo)checkSelf(l);
			GameFramework.CharacterProperty v;
			checkType(l,2,out v);
			self.SenderProperty=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.ImpactInfo");
		addMember(l,RefixCharacterProperty);
		addMember(l,"Seq",get_Seq,set_Seq,true);
		addMember(l,"ImpactId",get_ImpactId,set_ImpactId,true);
		addMember(l,"SkillId",get_SkillId,set_SkillId,true);
		addMember(l,"ImpactSenderId",get_ImpactSenderId,set_ImpactSenderId,true);
		addMember(l,"SenderPosition",get_SenderPosition,set_SenderPosition,true);
		addMember(l,"TargetType",get_TargetType,set_TargetType,true);
		addMember(l,"StartTime",get_StartTime,set_StartTime,true);
		addMember(l,"DurationTime",get_DurationTime,set_DurationTime,true);
		addMember(l,"CurDamageCount",get_CurDamageCount,set_CurDamageCount,true);
		addMember(l,"ImpactToTarget",get_ImpactToTarget,set_ImpactToTarget,true);
		addMember(l,"ConfigData",get_ConfigData,set_ConfigData,true);
		addMember(l,"DamageData",get_DamageData,set_DamageData,true);
		addMember(l,"SenderProperty",get_SenderProperty,set_SenderProperty,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.ImpactInfo));
	}
}
