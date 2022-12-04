using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Skill_Trigers_TriggerUtil : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public int Lookat__GameObject__Vector3_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,2,out a2);
			GameFramework.Skill.Trigers.TriggerUtil.Lookat(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Lookat__GameObject__Vector3__Single_s(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int GetRayCastPosInNavMesh_s(IntPtr l) {
		try {
			UnityEngine.Vector3 a1;
			checkType(l,1,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,2,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,3,out a3);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetRayCastPosInNavMesh(a1,a2,ref a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SamplePositionInNavMesh_s(IntPtr l) {
		try {
			UnityEngine.Vector3 a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.SamplePositionInNavMesh(a1,a2);
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
	static public int RaycastNavmesh_s(IntPtr l) {
		try {
			UnityEngine.Vector3 a1;
			checkType(l,1,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.RaycastNavmesh(a1,a2);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetHeightWithGround__GameObject_s(IntPtr l) {
		try {
			UnityEngine.GameObject a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetHeightWithGround(a1);
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
	static public int GetHeightWithGround__Vector3_s(IntPtr l) {
		try {
			UnityEngine.Vector3 a1;
			checkType(l,1,out a1);
			var ret=GameFramework.Skill.Trigers.TriggerUtil.GetHeightWithGround(a1);
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.Trigers.TriggerUtil");
		addMember(l,ctor_s);
		addMember(l,Lookat__GameObject__Vector3_s);
		addMember(l,Lookat__GameObject__Vector3__Single_s);
		addMember(l,GetChildNodeByName_s);
		addMember(l,AttachNodeToNode_s);
		addMember(l,MoveChildToNode_s);
		addMember(l,DrawCircle_s);
		addMember(l,ConvertToSecond_s);
		addMember(l,SetObjVisible_s);
		addMember(l,MoveObjTo_s);
		addMember(l,GetObjFaceDir_s);
		addMember(l,GetGroundPos_s);
		addMember(l,GetRayCastPosInNavMesh_s);
		addMember(l,SamplePositionInNavMesh_s);
		addMember(l,RaycastNavmesh_s);
		addMember(l,FloatEqual_s);
		addMember(l,GetHeightWithGround__GameObject_s);
		addMember(l,GetHeightWithGround__Vector3_s);
		addMember(l,GetSkillStartPosition_s);
		addMember(l,CalcImpactConfig_s);
		createTypeMetatable(l,null, typeof(GameFramework.Skill.Trigers.TriggerUtil));
	}
}
