using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Skill_GfxSkillSystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int PreloadSkillInstance(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(TableConfig.Skill))){
				GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
				TableConfig.Skill a1;
				checkType(l,2,out a1);
				self.PreloadSkillInstance(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(int))){
				GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				self.PreloadSkillInstance(a1);
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
	static public int FindSkillInstanceForSkillViewer(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.FindSkillInstanceForSkillViewer(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FindInnerSkillInstanceForSkillViewer(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
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
			else if(argc==4){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int GetActiveSkillInfo(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				var ret=self.GetActiveSkillInfo(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FindActiveSkillInstance(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==4){
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
			else if(argc==5){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int StopAllSkill(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				GameFramework.Skill.GfxSkillSystem self=(GameFramework.Skill.GfxSkillSystem)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				self.StopAllSkill(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
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
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
			System.Collections.Generic.Dictionary<System.String,System.Object> a5;
			checkType(l,6,out a5);
			self.SendMessage(a1,a2,a3,a4,a5);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Skill.GfxSkillSystem");
		addMember(l,Init);
		addMember(l,Reset);
		addMember(l,PreloadSkillInstance);
		addMember(l,ClearSkillInstancePool);
		addMember(l,FindSkillInstanceForSkillViewer);
		addMember(l,FindInnerSkillInstanceForSkillViewer);
		addMember(l,GetActiveSkillCount);
		addMember(l,GetActiveSkillInfo);
		addMember(l,FindActiveSkillInstance);
		addMember(l,StartSkill);
		addMember(l,CancelSkill);
		addMember(l,PauseSkill);
		addMember(l,PauseAllSkill);
		addMember(l,StopSkill);
		addMember(l,StopAllSkill);
		addMember(l,SendMessage);
		addMember(l,Tick);
		addMember(l,CalcUniqueInnerSkillId_s);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,constructor, typeof(GameFramework.Skill.GfxSkillSystem));
	}
}
