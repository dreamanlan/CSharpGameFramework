using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_PluginFramework : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.PluginFramework o;
			o=new GameFramework.PluginFramework();
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
	static public int Init(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.Init(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Release(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			self.Release();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Preload(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			self.Preload();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Tick(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			self.Tick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryEnterScene(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.UInt32 a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			self.TryEnterScene(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DelayChangeScene(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.DelayChangeScene(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ChangeScene(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.ChangeScene(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnSceneLoaded(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			TableConfig.Level a1;
			checkType(l,2,out a1);
			self.OnSceneLoaded(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadBattle(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.LoadBattle(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnBattleLoaded(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			TableConfig.Level a1;
			checkType(l,2,out a1);
			self.OnBattleLoaded(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int UnloadBattle(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.UnloadBattle(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnBattleUnloaded(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			TableConfig.Level a1;
			checkType(l,2,out a1);
			self.OnBattleUnloaded(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnRoomServerDisconnected(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			self.OnRoomServerDisconnected();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnRoomServerConnected(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			self.OnRoomServerConnected();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetBossCount(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			var ret=self.GetBossCount();
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
	static public int GetBattleNpcCount(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			var ret=self.GetBattleNpcCount();
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
	static public int GetBattleNpcCount__Int32(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetBattleNpcCount(a1);
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
	static public int GetBattleNpcCount__EntityInfo__CharacterRelation(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			GameFramework.CharacterRelation a2;
			checkEnum(l,3,out a2);
			var ret=self.GetBattleNpcCount(a1,a2);
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
	static public int GetBattleNpcCount__Int32__CharacterRelation(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			GameFramework.CharacterRelation a2;
			checkEnum(l,3,out a2);
			var ret=self.GetBattleNpcCount(a1,a2);
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
	static public int GetDyingBattleNpcCount__Int32(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetDyingBattleNpcCount(a1);
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
	static public int GetDyingBattleNpcCount__Int32__CharacterRelation(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			GameFramework.CharacterRelation a2;
			checkEnum(l,3,out a2);
			var ret=self.GetDyingBattleNpcCount(a1,a2);
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
	static public int GetNpcCount(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.GetNpcCount(a1,a2);
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
	static public int GetEntityById(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityById(a1);
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
	static public int GetEntityByUnitId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityByUnitId(a1);
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
	static public int DestroyEntityById(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.DestroyEntityById(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CreateEntity__Int32__Single__Single__Single__Single__Int32__Int32(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Int32 a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			var ret=self.CreateEntity(a1,a2,a3,a4,a5,a6,a7);
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
	static public int CreateEntity__Int32__Int32__Single__Single__Single__Single__Int32__Int32(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			System.Int32 a8;
			checkType(l,9,out a8);
			var ret=self.CreateEntity(a1,a2,a3,a4,a5,a6,a7,a8);
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
	static public int CreateEntity__Int32__Single__Single__Single__Single__Int32__Int32__String__A_String(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Int32 a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			System.String a8;
			checkType(l,9,out a8);
			System.String[] a9;
			checkParams(l,10,out a9);
			var ret=self.CreateEntity(a1,a2,a3,a4,a5,a6,a7,a8,a9);
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
	static public int CreateEntity__Int32__Int32__Single__Single__Single__Single__Int32__Int32__String__A_String(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			System.Int32 a8;
			checkType(l,9,out a8);
			System.String a9;
			checkType(l,10,out a9);
			System.String[] a10;
			checkParams(l,11,out a10);
			var ret=self.CreateEntity(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
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
	static public int CreateSceneLogic__Int32__Int32__A_String(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.String[] a3;
			checkParams(l,4,out a3);
			var ret=self.CreateSceneLogic(a1,a2,a3);
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
	static public int CreateSceneLogic__Int32__Int32__Int32__A_String(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.String[] a4;
			checkParams(l,5,out a4);
			var ret=self.CreateSceneLogic(a1,a2,a3,a4);
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
	static public int DestroySceneLogic(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.DestroySceneLogic(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DestroySceneLogicByConfigId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.DestroySceneLogicByConfigId(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSceneLogicInfo(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSceneLogicInfo(a1);
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
	static public int GetSceneLogicInfoByConfigId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetSceneLogicInfoByConfigId(a1);
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
	static public int HighlightPromptWithDict(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			self.HighlightPromptWithDict(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int HighlightPrompt(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.HighlightPrompt(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetLeaderEntityInfo(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			var ret=self.GetLeaderEntityInfo();
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
	static public int QueueAction(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			GameFramework.MyAction a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.QueueAction(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int QueueActionWithDelegation(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Delegate a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			self.QueueActionWithDelegation(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsLocalSkillEffect(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			GameFramework.Skill.GfxSkillSenderInfo a1;
			checkType(l,2,out a1);
			var ret=self.IsLocalSkillEffect(a1);
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
	static public int UnitId2ObjId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.UnitId2ObjId(a1);
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
	static public int ObjId2UnitId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.ObjId2UnitId(a1);
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
	static public int GetEntityView(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetEntityViewById(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetEntityViewByUnitId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int ExistGameObject__GameObject(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.ExistGameObject(a1);
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
	static public int ExistGameObject__Int32(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.ExistGameObject(a1);
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
	static public int GetGameObject(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetGameObjectByUnitId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetGameObjectId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetGameObjectUnitId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetEntityByGameObject(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityByGameObject(a1);
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
	static public int GetGameObjectByEntity(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			GameFramework.EntityInfo a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectByEntity(a1);
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
	static public int GetGameObjectCurSkillId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectCurSkillId(a1);
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
	static public int GetGameObjectType(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectType(a1);
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
	static public int GetGameObjectHp(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectHp(a1);
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
	static public int GetGameObjectEnergy(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectEnergy(a1);
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
	static public int GetGameObjectProperty(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetGameObjectProperty(a1);
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
	static public int GetCampId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetCampId(a1);
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
	static public int ClickNpc(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.ClickNpc(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetLockTarget(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SetLockTarget(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MoveTo(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			self.MoveTo(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SkillCanFindTarget(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.SkillCanFindTarget(a1,a2);
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
	static public int CastSkill(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.CastSkill(a1,a2);
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
	static public int GetAIEnable(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetAIEnable(a1);
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
	static public int SetAIEnable(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.SetAIEnable(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetNpcMp(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetNpcMp(a1);
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
	static public int GetNpcCooldown(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Single a2;
			System.Single a3;
			var ret=self.GetNpcCooldown(a1,out a2,out a3);
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
	[UnityEngine.Scripting.Preserve]
	static public int LoadTableConfig_s(IntPtr l) {
		try {
			GameFramework.PluginFramework.LoadTableConfig();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.PluginFramework.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SceneId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SceneInfo(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneInfo);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BattleSceneId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BattleSceneId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BattleSceneInfo(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BattleSceneInfo);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsMainUiScene(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsMainUiScene);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsBattleState(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsBattleState);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SelectedTarget(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SelectedTarget);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_RoomObjId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RoomObjId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_RoomUnitId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RoomUnitId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LeaderId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LeaderId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LeaderId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.LeaderId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CampId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CampId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_CampId(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.CampId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EntityManager(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.EntityManager);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SceneLogicInfoManager(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneLogicInfoManager);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_BlackBoard(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BlackBoard);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_KdTree(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KdTree);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SceneContext(IntPtr l) {
		try {
			GameFramework.PluginFramework self=(GameFramework.PluginFramework)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneContext);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.PluginFramework");
		addMember(l,ctor_s);
		addMember(l,Init);
		addMember(l,Release);
		addMember(l,Reset);
		addMember(l,Preload);
		addMember(l,Tick);
		addMember(l,TryEnterScene);
		addMember(l,DelayChangeScene);
		addMember(l,ChangeScene);
		addMember(l,OnSceneLoaded);
		addMember(l,LoadBattle);
		addMember(l,OnBattleLoaded);
		addMember(l,UnloadBattle);
		addMember(l,OnBattleUnloaded);
		addMember(l,OnRoomServerDisconnected);
		addMember(l,OnRoomServerConnected);
		addMember(l,GetBossCount);
		addMember(l,GetBattleNpcCount);
		addMember(l,GetBattleNpcCount__Int32);
		addMember(l,GetBattleNpcCount__EntityInfo__CharacterRelation);
		addMember(l,GetBattleNpcCount__Int32__CharacterRelation);
		addMember(l,GetDyingBattleNpcCount__Int32);
		addMember(l,GetDyingBattleNpcCount__Int32__CharacterRelation);
		addMember(l,GetNpcCount);
		addMember(l,GetEntityById);
		addMember(l,GetEntityByUnitId);
		addMember(l,DestroyEntityById);
		addMember(l,CreateEntity__Int32__Single__Single__Single__Single__Int32__Int32);
		addMember(l,CreateEntity__Int32__Int32__Single__Single__Single__Single__Int32__Int32);
		addMember(l,CreateEntity__Int32__Single__Single__Single__Single__Int32__Int32__String__A_String);
		addMember(l,CreateEntity__Int32__Int32__Single__Single__Single__Single__Int32__Int32__String__A_String);
		addMember(l,CreateSceneLogic__Int32__Int32__A_String);
		addMember(l,CreateSceneLogic__Int32__Int32__Int32__A_String);
		addMember(l,DestroySceneLogic);
		addMember(l,DestroySceneLogicByConfigId);
		addMember(l,GetSceneLogicInfo);
		addMember(l,GetSceneLogicInfoByConfigId);
		addMember(l,HighlightPromptWithDict);
		addMember(l,HighlightPrompt);
		addMember(l,GetLeaderEntityInfo);
		addMember(l,QueueAction);
		addMember(l,QueueActionWithDelegation);
		addMember(l,IsLocalSkillEffect);
		addMember(l,UnitId2ObjId);
		addMember(l,ObjId2UnitId);
		addMember(l,GetEntityView);
		addMember(l,GetEntityViewById);
		addMember(l,GetEntityViewByUnitId);
		addMember(l,ExistGameObject__GameObject);
		addMember(l,ExistGameObject__Int32);
		addMember(l,GetGameObject);
		addMember(l,GetGameObjectByUnitId);
		addMember(l,GetGameObjectId);
		addMember(l,GetGameObjectUnitId);
		addMember(l,GetEntityByGameObject);
		addMember(l,GetGameObjectByEntity);
		addMember(l,GetGameObjectCurSkillId);
		addMember(l,GetGameObjectType);
		addMember(l,GetGameObjectHp);
		addMember(l,GetGameObjectEnergy);
		addMember(l,GetGameObjectProperty);
		addMember(l,GetCampId);
		addMember(l,ClickNpc);
		addMember(l,SetLockTarget);
		addMember(l,MoveTo);
		addMember(l,SkillCanFindTarget);
		addMember(l,CastSkill);
		addMember(l,GetAIEnable);
		addMember(l,SetAIEnable);
		addMember(l,GetNpcMp);
		addMember(l,GetNpcCooldown);
		addMember(l,LoadTableConfig_s);
		addMember(l,"Instance",get_Instance,null,false);
		addMember(l,"SceneId",get_SceneId,null,true);
		addMember(l,"SceneInfo",get_SceneInfo,null,true);
		addMember(l,"BattleSceneId",get_BattleSceneId,null,true);
		addMember(l,"BattleSceneInfo",get_BattleSceneInfo,null,true);
		addMember(l,"IsMainUiScene",get_IsMainUiScene,null,true);
		addMember(l,"IsBattleState",get_IsBattleState,null,true);
		addMember(l,"SelectedTarget",get_SelectedTarget,null,true);
		addMember(l,"RoomObjId",get_RoomObjId,null,true);
		addMember(l,"RoomUnitId",get_RoomUnitId,null,true);
		addMember(l,"LeaderId",get_LeaderId,set_LeaderId,true);
		addMember(l,"CampId",get_CampId,set_CampId,true);
		addMember(l,"EntityManager",get_EntityManager,null,true);
		addMember(l,"SceneLogicInfoManager",get_SceneLogicInfoManager,null,true);
		addMember(l,"BlackBoard",get_BlackBoard,null,true);
		addMember(l,"KdTree",get_KdTree,null,true);
		addMember(l,"SceneContext",get_SceneContext,null,true);
		createTypeMetatable(l,null, typeof(GameFramework.PluginFramework));
	}
}
