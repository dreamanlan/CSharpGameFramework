using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Skill_GfxSkillSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem o;
			o=new GameFramework.Skill.GfxSkillSystem();
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
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			self.Init();
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
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
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
	static public int PreloadSkillInstance__Int32(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.PreloadSkillInstance(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PreloadSkillInstance__Skill(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			TableConfig.Skill a1;
			checkType(l,2,out a1);
			self.PreloadSkillInstance(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ClearSkillInstancePool(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			self.ClearSkillInstancePool();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FindSkillInstanceForSkillViewer__Int32(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.FindSkillInstanceForSkillViewer(a1);
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
	static public int FindSkillInstanceForSkillViewer__Int32__O_GfxSkillSenderInfo(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			GameFramework.Skill.GfxSkillSenderInfo a2;
			var ret=self.FindSkillInstanceForSkillViewer(a1,out a2);
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
	static public int FindInnerSkillInstanceForSkillViewer__Int32__SkillInstance(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			var ret=self.FindInnerSkillInstanceForSkillViewer(a1,a2);
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
	static public int FindInnerSkillInstanceForSkillViewer__Int32__SkillInstance__O_GfxSkillSenderInfo(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,3,out a2);
			GameFramework.Skill.GfxSkillSenderInfo a3;
			var ret=self.FindInnerSkillInstanceForSkillViewer(a1,a2,out a3);
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
	static public int GetActiveSkillCount(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			var ret=self.GetActiveSkillCount();
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
	static public int GetActiveSkillInfo__Int32(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetActiveSkillInfo(a1);
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
	static public int GetActiveSkillInfo__Int32__O_GfxSkillSenderInfo(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			GameFramework.Skill.GfxSkillSenderInfo a2;
			var ret=self.GetActiveSkillInfo(a1,out a2);
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
	static public int FindActiveSkillInstance__Int32__Int32__Int32(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			var ret=self.FindActiveSkillInstance(a1,a2,a3);
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
	static public int FindActiveSkillInstance__Int32__Int32__Int32__O_GfxSkillSenderInfo(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			GameFramework.Skill.GfxSkillSenderInfo a4;
			var ret=self.FindActiveSkillInstance(a1,a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StartSkill(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			TableConfig.Skill a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Collections.Generic.Dictionary<System.String,System.Object>[] a4;
			checkParams(l,5,out a4);
			var ret=self.StartSkill(a1,a2,a3,a4);
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
	static public int StartSkillWithGameObject(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			TableConfig.Skill a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Collections.Generic.Dictionary<System.String,System.Object>[] a4;
			checkParams(l,5,out a4);
			var ret=self.StartSkillWithGameObject(a1,a2,a3,a4);
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
	static public int CancelSkill(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.CancelSkill(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CancelSkillWithGameObject(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			self.CancelSkillWithGameObject(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PauseSkill(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			self.PauseSkill(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PauseSkillWithGameObject(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			self.PauseSkillWithGameObject(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PauseAllSkill(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.PauseAllSkill(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PauseAllSkillWithGameObject(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.PauseAllSkillWithGameObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopSkill(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			self.StopSkill(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopSkillWithGameObject(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			self.StopSkillWithGameObject(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopAllSkill__Int32__Boolean(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.StopAllSkill(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopAllSkill__Int32__Boolean__Boolean__Boolean(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			self.StopAllSkill(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopAllSkillWithGameObject__GameObject__Boolean(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.StopAllSkillWithGameObject(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopAllSkillWithGameObject__GameObject__Boolean__Boolean__Boolean(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			System.Boolean a4;
			checkType(l,5,out a4);
			self.StopAllSkillWithGameObject(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendMessage(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			self.SendMessage(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendMessageWithGameObject(IntPtr l) {
		try {
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			self.SendMessageWithGameObject(a1,a2,a3,a4);
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
			GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
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
	static public int CalcUniqueInnerSkillId_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			SkillSystem.SkillInstance a2;
			checkType(l,2,out a2);
			var ret=GameFramework.Skill.GfxSkillSystem.CalcUniqueInnerSkillId(a1,a2);
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
	static public int get_Instance(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.Skill.GfxSkillSystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.GfxSkillSystem");
		addMember(l,ctor_s);
		addMember(l,Init);
		addMember(l,Reset);
		addMember(l,PreloadSkillInstance__Int32);
		addMember(l,PreloadSkillInstance__Skill);
		addMember(l,ClearSkillInstancePool);
		addMember(l,FindSkillInstanceForSkillViewer__Int32);
		addMember(l,FindSkillInstanceForSkillViewer__Int32__O_GfxSkillSenderInfo);
		addMember(l,FindInnerSkillInstanceForSkillViewer__Int32__SkillInstance);
		addMember(l,FindInnerSkillInstanceForSkillViewer__Int32__SkillInstance__O_GfxSkillSenderInfo);
		addMember(l,GetActiveSkillCount);
		addMember(l,GetActiveSkillInfo__Int32);
		addMember(l,GetActiveSkillInfo__Int32__O_GfxSkillSenderInfo);
		addMember(l,FindActiveSkillInstance__Int32__Int32__Int32);
		addMember(l,FindActiveSkillInstance__Int32__Int32__Int32__O_GfxSkillSenderInfo);
		addMember(l,StartSkill);
		addMember(l,StartSkillWithGameObject);
		addMember(l,CancelSkill);
		addMember(l,CancelSkillWithGameObject);
		addMember(l,PauseSkill);
		addMember(l,PauseSkillWithGameObject);
		addMember(l,PauseAllSkill);
		addMember(l,PauseAllSkillWithGameObject);
		addMember(l,StopSkill);
		addMember(l,StopSkillWithGameObject);
		addMember(l,StopAllSkill__Int32__Boolean);
		addMember(l,StopAllSkill__Int32__Boolean__Boolean__Boolean);
		addMember(l,StopAllSkillWithGameObject__GameObject__Boolean);
		addMember(l,StopAllSkillWithGameObject__GameObject__Boolean__Boolean__Boolean);
		addMember(l,SendMessage);
		addMember(l,SendMessageWithGameObject);
		addMember(l,Tick);
		addMember(l,CalcUniqueInnerSkillId_s);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.Skill.GfxSkillSystem));
	}
}
