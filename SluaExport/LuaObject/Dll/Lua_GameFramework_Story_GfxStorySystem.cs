using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Story_GfxStorySystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Init(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
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
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PreloadSceneStories(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			self.PreloadSceneStories();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PreloadBattleStories(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.PreloadBattleStories(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PreloadNamespacedStory(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.PreloadNamespacedStory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PreloadAiStory(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.PreloadAiStory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearStoryInstancePool(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			self.ClearStoryInstancePool();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NewAiStoryInstance(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.NewAiStoryInstance(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				System.String[] a3;
				checkParams(l,4,out a3);
				var ret=self.NewAiStoryInstance(a1,a2,a3);
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
	static public int RecycleAiStoryInstance(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			GameFramework.AiStoryInstanceInfo a1;
			checkType(l,2,out a1);
			self.RecycleAiStoryInstance(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.GetStory(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				var ret=self.GetStory(a1,a2);
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
	static public int StartStory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.StartStory(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				System.String[] a3;
				checkParams(l,4,out a3);
				self.StartStory(a1,a2,a3);
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
	static public int PauseStory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==3){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				self.PauseStory(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				self.PauseStory(a1,a2,a3);
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
	static public int StopStory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.StopStory(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				self.StopStory(a1,a2);
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
	static public int CountStory(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.CountStory(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				var ret=self.CountStory(a1,a2);
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
	static public int MarkStoryTerminated(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.MarkStoryTerminated(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				self.MarkStoryTerminated(a1,a2);
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
	static public int Tick(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			self.Tick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SendMessage(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			self.SendMessage(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SendConcurrentMessage(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkParams(l,3,out a2);
			self.SendConcurrentMessage(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CountMessage(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.CountMessage(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PauseMessageHandler(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.PauseMessageHandler(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ThreadInitMask_s(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem.ThreadInitMask();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_SceneId(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SceneId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_SceneId(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.SceneId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ActiveStoryCount(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ActiveStoryCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_GfxDatas(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.GfxDatas);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_GlobalVariables(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.GlobalVariables);
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
			pushValue(l,GameFramework.Story.GfxStorySystem.Instance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Story.GfxStorySystem");
		addMember(l,Init);
		addMember(l,Reset);
		addMember(l,PreloadSceneStories);
		addMember(l,PreloadBattleStories);
		addMember(l,PreloadNamespacedStory);
		addMember(l,PreloadAiStory);
		addMember(l,ClearStoryInstancePool);
		addMember(l,NewAiStoryInstance);
		addMember(l,RecycleAiStoryInstance);
		addMember(l,GetStory);
		addMember(l,StartStory);
		addMember(l,PauseStory);
		addMember(l,StopStory);
		addMember(l,CountStory);
		addMember(l,MarkStoryTerminated);
		addMember(l,Tick);
		addMember(l,SendMessage);
		addMember(l,SendConcurrentMessage);
		addMember(l,CountMessage);
		addMember(l,PauseMessageHandler);
		addMember(l,ThreadInitMask_s);
		addMember(l,"SceneId",get_SceneId,set_SceneId,true);
		addMember(l,"ActiveStoryCount",get_ActiveStoryCount,null,true);
		addMember(l,"GfxDatas",get_GfxDatas,null,true);
		addMember(l,"GlobalVariables",get_GlobalVariables,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.Story.GfxStorySystem));
	}
}
