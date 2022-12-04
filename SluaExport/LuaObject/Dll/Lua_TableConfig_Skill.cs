using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_TableConfig_Skill : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			TableConfig.Skill o;
			o=new TableConfig.Skill();
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
	static public int ReadFromBinary(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.ReadFromBinary(a1,a2);
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
	static public int WriteToBinary(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			self.WriteToBinary(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetId(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			var ret=self.GetId();
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
	static public int get_id(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_id(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_desc(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.desc);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_desc(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.desc=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_type(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_type(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.type=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_icon(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.icon);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_icon(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.icon=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_impacttoself(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.impacttoself);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_impacttoself(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.impacttoself=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_impact(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.impact);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_impact(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.impact=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_targetType(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.targetType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_targetType(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.targetType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_aoeType(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.aoeType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_aoeType(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.aoeType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_aoeSize(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.aoeSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_aoeSize(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.aoeSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_aoeAngleOrLength(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.aoeAngleOrLength);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_aoeAngleOrLength(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.aoeAngleOrLength=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_maxAoeTargetCount(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.maxAoeTargetCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_maxAoeTargetCount(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.maxAoeTargetCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_dslFile(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.dslFile);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_dslFile(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.dslFile=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_damageData(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.damageData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_damageData(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			TableConfig.SkillDamageData v;
			checkType(l,2,out v);
			self.damageData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skillData(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.skillData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_skillData(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			TableConfig.SkillData v;
			checkType(l,2,out v);
			self.skillData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_impactData(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.impactData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_impactData(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			TableConfig.ImpactData v;
			checkType(l,2,out v);
			self.impactData=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SkillDistance(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkillDistance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SkillCooldown(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SkillCooldown);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ImpactDuration(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ImpactDuration);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ImpactCooldown(IntPtr l) {
		try {
			TableConfig.Skill self=(TableConfig.Skill)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ImpactCooldown);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.Skill");
		addMember(l,ctor_s);
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,GetId);
		addMember(l,"id",get_id,set_id,true);
		addMember(l,"desc",get_desc,set_desc,true);
		addMember(l,"type",get_type,set_type,true);
		addMember(l,"icon",get_icon,set_icon,true);
		addMember(l,"impacttoself",get_impacttoself,set_impacttoself,true);
		addMember(l,"impact",get_impact,set_impact,true);
		addMember(l,"targetType",get_targetType,set_targetType,true);
		addMember(l,"aoeType",get_aoeType,set_aoeType,true);
		addMember(l,"aoeSize",get_aoeSize,set_aoeSize,true);
		addMember(l,"aoeAngleOrLength",get_aoeAngleOrLength,set_aoeAngleOrLength,true);
		addMember(l,"maxAoeTargetCount",get_maxAoeTargetCount,set_maxAoeTargetCount,true);
		addMember(l,"dslFile",get_dslFile,set_dslFile,true);
		addMember(l,"damageData",get_damageData,set_damageData,true);
		addMember(l,"skillData",get_skillData,set_skillData,true);
		addMember(l,"impactData",get_impactData,set_impactData,true);
		addMember(l,"SkillDistance",get_SkillDistance,null,true);
		addMember(l,"SkillCooldown",get_SkillCooldown,null,true);
		addMember(l,"ImpactDuration",get_ImpactDuration,null,true);
		addMember(l,"ImpactCooldown",get_ImpactCooldown,null,true);
		createTypeMetatable(l,null, typeof(TableConfig.Skill));
	}
}
