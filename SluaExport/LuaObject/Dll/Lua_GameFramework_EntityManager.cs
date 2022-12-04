using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_EntityManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.EntityManager o;
			System.Int32 a1;
			checkType(l,1,out a1);
			o=new GameFramework.EntityManager(a1);
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
	static public int SetSceneContext(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			GameFramework.SceneContextInfo a1;
			checkType(l,2,out a1);
			self.SetSceneContext(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetEntityInfo(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityInfo(a1);
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
	static public int GetEntityInfoByUnitId(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetEntityInfoByUnitId(a1);
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
	static public int AddEntity__Int32__Int32__Actor__String__A_String(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			TableConfig.Actor a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.String[] a5;
			checkParams(l,6,out a5);
			var ret=self.AddEntity(a1,a2,a3,a4,a5);
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
	static public int AddEntity__Int32__Int32__Int32__Actor__String__A_String(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			TableConfig.Actor a4;
			checkType(l,5,out a4);
			System.String a5;
			checkType(l,6,out a5);
			System.String[] a6;
			checkParams(l,7,out a6);
			var ret=self.AddEntity(a1,a2,a3,a4,a5,a6);
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
	static public int DelayAddEntity__Int32__Int32__Actor__String__A_String(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			TableConfig.Actor a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.String[] a5;
			checkParams(l,6,out a5);
			var ret=self.DelayAddEntity(a1,a2,a3,a4,a5);
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
	static public int DelayAddEntity__Int32__Int32__Int32__Actor__String__A_String(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			TableConfig.Actor a4;
			checkType(l,5,out a4);
			System.String a5;
			checkType(l,6,out a5);
			System.String[] a6;
			checkParams(l,7,out a6);
			var ret=self.DelayAddEntity(a1,a2,a3,a4,a5,a6);
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
	static public int ExecuteDelayAdd(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			self.ExecuteDelayAdd();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveEntity(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.RemoveEntity(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetNearest(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			var ret=self.GetNearest(a1,ref a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int HasCombatNpc(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			var ret=self.HasCombatNpc();
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
	static public int HasCombatNpcAlive(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			var ret=self.HasCombatNpcAlive();
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
	static public int Reset(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
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
	static public int FireDamageEvent(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			System.Int32 a6;
			checkType(l,7,out a6);
			self.FireDamageEvent(a1,a2,a3,a4,a5,a6);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_OnDamage(IntPtr l) {
		try {
			GameFramework.EntityManager self=(GameFramework.EntityManager)checkSelf(l);
			GameFramework.DamageDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnDamage=v;
			else if(op==1) self.OnDamage+=v;
			else if(op==2) self.OnDamage-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.EntityManager");
		addMember(l,ctor_s);
		addMember(l,SetSceneContext);
		addMember(l,GetEntityInfo);
		addMember(l,GetEntityInfoByUnitId);
		addMember(l,AddEntity__Int32__Int32__Actor__String__A_String);
		addMember(l,AddEntity__Int32__Int32__Int32__Actor__String__A_String);
		addMember(l,DelayAddEntity__Int32__Int32__Actor__String__A_String);
		addMember(l,DelayAddEntity__Int32__Int32__Int32__Actor__String__A_String);
		addMember(l,ExecuteDelayAdd);
		addMember(l,RemoveEntity);
		addMember(l,GetNearest);
		addMember(l,HasCombatNpc);
		addMember(l,HasCombatNpcAlive);
		addMember(l,Reset);
		addMember(l,FireDamageEvent);
		addMember(l,"OnDamage",null,set_OnDamage,true);
		createTypeMetatable(l,null, typeof(GameFramework.EntityManager));
	}
}
