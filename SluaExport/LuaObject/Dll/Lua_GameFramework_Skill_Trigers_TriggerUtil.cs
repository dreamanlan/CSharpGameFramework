using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_Trigers_TriggerUtil : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.Skill.Trigers.TriggerUtil o;
			o=new GameFramework.Skill.Trigers.TriggerUtil();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Lookat_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				UnityEngine.Vector3 a2;
				checkType(l,2,out a2);
				GameFramework.Skill.Trigers.TriggerUtil.Lookat(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				UnityEngine.Vector3 a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				GameFramework.Skill.Trigers.TriggerUtil.Lookat(a1,a2,a3);
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
	static public int GetChildNodeByName_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetChildNodeByName(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AttachNodeToNode_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			UnityEngine.GameObject a3;
			checkType(l,3,out a3);
			System.String a4;
			checkType(l,4,out a4);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.AttachNodeToNode(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MoveChildToNode_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			GameFramework.Skill.Trigers.TriggerUtil.MoveChildToNode(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DrawCircle_s(IntPtr l) {
		try {
			UnityEngine.Vector3 a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			UnityEngine.Color a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.DrawCircle(a1,a2,a3,a4);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FindTargetInSector_s(IntPtr l) {
		try {
			UnityEngine.Vector3 a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,3,out a3);
			UnityEngine.Vector3 a4;
			checkType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.FindTargetInSector(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetObjectByPriority_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Collections.Generic.List<UnityEngine.GameObject> a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			System.Single a6;
			checkType(l,6,out a6);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetObjectByPriority(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FiltByRelation_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Collections.Generic.List<UnityEngine.GameObject> a2;
			checkType(l,2,out a2);
			GameFramework.CharacterRelation a3;
			checkEnum(l,3,out a3);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.FiltByRelation(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ConvertToSecond_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.ConvertToSecond(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetObjVisible_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			GameFramework.Skill.Trigers.TriggerUtil.SetObjVisible(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MoveObjTo_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,2,out a2);
			GameFramework.Skill.Trigers.TriggerUtil.MoveObjTo(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetObjFaceDir_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetObjFaceDir(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGroundPos_s(IntPtr l) {
		try {
			UnityEngine.Vector3 a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetGroundPos(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FloatEqual_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.FloatEqual(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetHeightWithGround_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(UnityEngine.Vector3))){
				UnityEngine.Vector3 a1;
				checkType(l,1,out a1);
				var ret=GameFramework.Skill.Trigers.TriggerUtil.GetHeightWithGround(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GameObject))){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				var ret=GameFramework.Skill.Trigers.TriggerUtil.GetHeightWithGround(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int GetSkillStartPosition_s(IntPtr l) {
		try {
			UnityEngine.Vector3 a1;
			checkType(l,1,out a1);
			TableConfig.Skill a2;
			checkType(l,2,out a2);
			SkillSystem.SkillInstance a3;
			checkType(l,3,out a3);
			System.Int32 a4;
			checkType(l,4,out a4);
			System.Int32 a5;
			checkType(l,5,out a5);
			UnityEngine.Vector3 a6;
			checkType(l,6,out a6);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetSkillStartPosition(a1,a2,a3,a4,a5,ref a6);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a6);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcImpactConfig_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			SkillSystem.SkillInstance a3;
			checkType(l,3,out a3);
			TableConfig.Skill a4;
			checkType(l,4,out a4);
			Dictionary<System.String,System.Object> a5;
			GameFramework.Skill.Trigers.TriggerUtil.CalcImpactConfig(a1,a2,a3,a4,out a5);
			pushValue(l,true);
			pushValue(l,a5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetSkillImpactId_s(IntPtr l) {
		try {
			System.Collections.Generic.Dictionary<System.String,System.Object> a1;
			checkType(l,1,out a1);
			TableConfig.Skill a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetSkillImpactId(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AoeQuery_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==7){
				GameFramework.Skill.GfxSkillSenderInfo a1;
				checkType(l,1,out a1);
				SkillSystem.SkillInstance a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				UnityEngine.Vector3 a5;
				checkType(l,5,out a5);
				System.Boolean a6;
				checkType(l,6,out a6);
				GameFramework.MyFunc<System.Single,System.Int32,System.Boolean> a7;
				LuaDelegation.checkDelegate(l,7,out a7);
				GameFramework.Skill.Trigers.TriggerUtil.AoeQuery(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				return 1;
			}
			else if(argc==11){
				UnityEngine.GameObject a1;
				checkType(l,1,out a1);
				UnityEngine.GameObject a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				SkillSystem.SkillInstance a6;
				checkType(l,6,out a6);
				System.Int32 a7;
				checkType(l,7,out a7);
				System.Int32 a8;
				checkType(l,8,out a8);
				UnityEngine.Vector3 a9;
				checkType(l,9,out a9);
				System.Boolean a10;
				checkType(l,10,out a10);
				GameFramework.MyFunc<System.Single,System.Int32,System.Boolean> a11;
				LuaDelegation.checkDelegate(l,11,out a11);
				GameFramework.Skill.Trigers.TriggerUtil.AoeQuery(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10,a11);
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
		getTypeTable(l,"GameFramework.Skill.Trigers.TriggerUtil");
		addMember(l,Lookat_s);
		addMember(l,GetChildNodeByName_s);
		addMember(l,AttachNodeToNode_s);
		addMember(l,MoveChildToNode_s);
		addMember(l,DrawCircle_s);
		addMember(l,FindTargetInSector_s);
		addMember(l,GetObjectByPriority_s);
		addMember(l,FiltByRelation_s);
		addMember(l,ConvertToSecond_s);
		addMember(l,SetObjVisible_s);
		addMember(l,MoveObjTo_s);
		addMember(l,GetObjFaceDir_s);
		addMember(l,GetGroundPos_s);
		addMember(l,FloatEqual_s);
		addMember(l,GetHeightWithGround_s);
		addMember(l,GetSkillStartPosition_s);
		addMember(l,CalcImpactConfig_s);
		addMember(l,GetSkillImpactId_s);
		addMember(l,AoeQuery_s);
		createTypeMetatable(l,constructor, typeof(GameFramework.Skill.Trigers.TriggerUtil));
	}
}
