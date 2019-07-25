using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_EntityController : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			self.Init();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Release(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			self.Release();
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
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			self.Tick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetEntityViewById(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityViewById(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetEntityViewByUnitId(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityViewByUnitId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsVisible(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.IsVisible(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGameObject(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObject(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGameObjectByUnitId(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectByUnitId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetEntityView(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityView(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGameObjectUnitId(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectUnitId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGameObjectId(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExistGameObject(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(int))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.ExistGameObject(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.GameObject))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				var ret=self.ExistGameObject(a1);
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
	static public int GetEntityType(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.GameObject))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				var ret=self.GetEntityType(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetEntityType(a1);
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
	static public int GetCampId(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.GameObject))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				var ret=self.GetCampId(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetCampId(a1);
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
	static public int IsMovableEntity(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.GameObject))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				var ret=self.IsMovableEntity(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.IsMovableEntity(a1);
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
	static public int IsRotatableEntity(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.GameObject))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				var ret=self.IsRotatableEntity(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.IsRotatableEntity(a1);
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
	static public int SyncFaceDir(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.SyncFaceDir(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcPosAndDir(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			ScriptRuntime.Vector3 a2;
			System.Single a3;
			var ret=self.CalcPosAndDir(a1,out a2,out a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			pushValue(l,a3);
			return 4;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcSkillDistance(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.CalcSkillDistance(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CanCastSkill(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			TableConfig.Skill a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.CanCastSkill(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CancelCastSkill(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.CancelCastSkill(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int BuildSkillInfo(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			TableConfig.Skill a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.BuildSkillInfo(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ActivateSkill(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.ActivateSkill(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DeactivateSkill(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.DeactivateSkill(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CancelIfImpact(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			TableConfig.Skill a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.CancelIfImpact(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StopSkillAnimation(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.StopSkillAnimation(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PauseSkillAnimation(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.PauseSkillAnimation(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SelectTargetForSkill(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			TableConfig.Skill a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			System.Collections.Generic.HashSet<System.Int32> a5;
			checkType(l,6,out a5);
			var ret=self.SelectTargetForSkill(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRandEnemyId(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetRandEnemyId(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Collections.Generic.HashSet<System.Int32> a2;
				checkType(l,3,out a2);
				var ret=self.GetRandEnemyId(a1,a2);
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
	static public int GetRandFriendId(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetRandFriendId(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Collections.Generic.HashSet<System.Int32> a2;
				checkType(l,3,out a2);
				var ret=self.GetRandFriendId(a1,a2);
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
	static public int HaveShield(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.HaveShield(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetTargetType(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			TableConfig.Skill a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.GetTargetType(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetImpactDuration(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.GetImpactDuration(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetImpactSenderPosition(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.GetImpactSenderPosition(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetImpactSkillId(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.GetImpactSkillId(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcSenderAndTarget(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			GameFramework.Skill.GfxSkillSenderInfo a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			System.Int32 a3;
			self.CalcSenderAndTarget(a1,out a2,out a3);
			pushValue(l,true);
			pushValue(l,a2);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRelation(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(UnityEngine.GameObject),typeof(UnityEngine.GameObject))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				UnityEngine.GameObject a2;
				checkType(l,3,out a2);
				var ret=self.GetRelation(a1,a2);
				pushValue(l,true);
				pushEnum(l,(int)ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(int),typeof(int))){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				var ret=self.GetRelation(a1,a2);
				pushValue(l,true);
				pushEnum(l,(int)ret);
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
	static public int HaveState(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			var ret=self.HaveState(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DisableState(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.DisableState(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddState(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.AddState(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveState(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.RemoveState(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddShield(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			TableConfig.Skill a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.AddShield(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveShield(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			TableConfig.Skill a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.RemoveShield(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SendImpact(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==5){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				var ret=self.SendImpact(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==9){
				GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
				TableConfig.Skill a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				System.Int32 a4;
				checkType(l,5,out a4);
				System.Int32 a5;
				checkType(l,6,out a5);
				System.Int32 a6;
				checkType(l,7,out a6);
				System.Boolean a7;
				checkType(l,8,out a7);
				System.Collections.Generic.Dictionary<System.String,System.Object> a8;
				checkType(l,9,out a8);
				var ret=self.SendImpact(a1,a2,a3,a4,a5,a6,a7,a8);
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
	static public int TrackImpact(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			TableConfig.Skill a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			System.String a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			UnityEngine.Vector3 a8;
			checkType(l,9,out a8);
			System.Boolean a9;
			checkType(l,10,out a9);
			System.Collections.Generic.Dictionary<System.String,System.Object> a10;
			checkType(l,11,out a10);
			var ret=self.TrackImpact(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetTrackSendImpact(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Collections.Generic.Dictionary<System.String,System.Object> a3;
			checkType(l,4,out a3);
			var ret=self.GetTrackSendImpact(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TrackSendImpact(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			System.Collections.Generic.Dictionary<System.String,System.Object> a5;
			checkType(l,6,out a5);
			var ret=self.TrackSendImpact(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ImpactDamage(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			System.Boolean a5;
			checkType(l,6,out a5);
			self.ImpactDamage(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int KeepTarget(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.KeepTarget(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int BornFinish(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.BornFinish(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DeadFinish(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.DeadFinish(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRootSummoner(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			var ret=self.GetRootSummoner(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRootSummonerId(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			var ret=self.GetRootSummonerId(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRootSummonerInfo(IntPtr l) {
		try {
			GameFramework.EntityController self=(GameFramework.EntityController)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			var ret=self.GetRootSummonerInfo(a1,a2,out a3);
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
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.EntityController.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.EntityController");
		addMember(l,Init);
		addMember(l,Release);
		addMember(l,Tick);
		addMember(l,GetEntityViewById);
		addMember(l,GetEntityViewByUnitId);
		addMember(l,IsVisible);
		addMember(l,GetGameObject);
		addMember(l,GetGameObjectByUnitId);
		addMember(l,GetEntityView);
		addMember(l,GetGameObjectUnitId);
		addMember(l,GetGameObjectId);
		addMember(l,ExistGameObject);
		addMember(l,GetEntityType);
		addMember(l,GetCampId);
		addMember(l,IsMovableEntity);
		addMember(l,IsRotatableEntity);
		addMember(l,SyncFaceDir);
		addMember(l,CalcPosAndDir);
		addMember(l,CalcSkillDistance);
		addMember(l,CanCastSkill);
		addMember(l,CancelCastSkill);
		addMember(l,BuildSkillInfo);
		addMember(l,ActivateSkill);
		addMember(l,DeactivateSkill);
		addMember(l,CancelIfImpact);
		addMember(l,StopSkillAnimation);
		addMember(l,PauseSkillAnimation);
		addMember(l,SelectTargetForSkill);
		addMember(l,GetRandEnemyId);
		addMember(l,GetRandFriendId);
		addMember(l,HaveShield);
		addMember(l,GetTargetType);
		addMember(l,GetImpactDuration);
		addMember(l,GetImpactSenderPosition);
		addMember(l,GetImpactSkillId);
		addMember(l,CalcSenderAndTarget);
		addMember(l,GetRelation);
		addMember(l,HaveState);
		addMember(l,DisableState);
		addMember(l,AddState);
		addMember(l,RemoveState);
		addMember(l,AddShield);
		addMember(l,RemoveShield);
		addMember(l,SendImpact);
		addMember(l,TrackImpact);
		addMember(l,GetTrackSendImpact);
		addMember(l,TrackSendImpact);
		addMember(l,ImpactDamage);
		addMember(l,KeepTarget);
		addMember(l,BornFinish);
		addMember(l,DeadFinish);
		addMember(l,GetRootSummoner);
		addMember(l,GetRootSummonerId);
		addMember(l,GetRootSummonerInfo);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.EntityController));
	}
}
