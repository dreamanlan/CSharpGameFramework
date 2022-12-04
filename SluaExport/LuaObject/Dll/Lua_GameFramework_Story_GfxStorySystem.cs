using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_Story_GfxStorySystem : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int Reset__Boolean(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.Reset(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadSceneStories(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			self.LoadSceneStories();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadBattleStories(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.LoadBattleStories(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadStory(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.LoadStory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadAiStory(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.LoadAiStory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int NewAiStoryInstance(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int GetStory__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetStory(a1);
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
	static public int GetStory__String__String(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StartStories__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.StartStories(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StartStories__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.StartStories(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PauseStories__String__Boolean(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.PauseStories(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PauseStories__String__String__Boolean(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			self.PauseStories(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopStories__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.StopStories(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopStories__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.StopStories(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CountStories__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.CountStories(a1);
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
	static public int CountStories__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			var ret=self.CountStories(a1,a2);
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
	static public int MarkStoriesTerminated__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.MarkStoriesTerminated(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MarkStoriesTerminated__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.MarkStoriesTerminated(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SkipCurMessageHandlers__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.SkipCurMessageHandlers(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SkipCurMessageHandlers__String__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			self.SkipCurMessageHandlers(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StartStory__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.StartStory(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StartStory__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.StartStory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PauseStory__String__Boolean(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.PauseStory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PauseStory__String__String__Boolean(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopStory__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.StopStory(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int StopStory__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.StopStory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CountStory__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.CountStory(a1);
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
	static public int CountStory__String__String(IntPtr l) {
		try {
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
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MarkStoryTerminated__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.MarkStoryTerminated(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int MarkStoryTerminated__String__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.MarkStoryTerminated(a1,a2);
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
	[UnityEngine.Scripting.Preserve]
	static public int AddBindedStory(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			UnityEngine.Object a1;
			checkType(l,2,out a1);
			StorySystem.StoryInstance a2;
			checkType(l,3,out a2);
			self.AddBindedStory(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int NewBoxedValueList(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			var ret=self.NewBoxedValueList();
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
	static public int SendMessage__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SendMessage(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendMessage__String__BoxedValue(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValue a2;
			checkValueType(l,3,out a2);
			self.SendMessage(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendMessage__String__BoxedValueList(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValueList a2;
			checkType(l,3,out a2);
			self.SendMessage(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendMessage__String__BoxedValue__BoxedValue(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValue a2;
			checkValueType(l,3,out a2);
			BoxedValue a3;
			checkValueType(l,4,out a3);
			self.SendMessage(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendMessage__String__BoxedValue__BoxedValue__BoxedValue(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValue a2;
			checkValueType(l,3,out a2);
			BoxedValue a3;
			checkValueType(l,4,out a3);
			BoxedValue a4;
			checkValueType(l,5,out a4);
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
	static public int SendConcurrentMessage__String(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SendConcurrentMessage(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendConcurrentMessage__String__BoxedValue(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValue a2;
			checkValueType(l,3,out a2);
			self.SendConcurrentMessage(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendConcurrentMessage__String__BoxedValueList(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValueList a2;
			checkType(l,3,out a2);
			self.SendConcurrentMessage(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendConcurrentMessage__String__BoxedValue__BoxedValue(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValue a2;
			checkValueType(l,3,out a2);
			BoxedValue a3;
			checkValueType(l,4,out a3);
			self.SendConcurrentMessage(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendConcurrentMessage__String__BoxedValue__BoxedValue__BoxedValue(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValue a2;
			checkValueType(l,3,out a2);
			BoxedValue a3;
			checkValueType(l,4,out a3);
			BoxedValue a4;
			checkValueType(l,5,out a4);
			self.SendConcurrentMessage(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int SuspendMessageHandler(IntPtr l) {
		try {
			GameFramework.Story.GfxStorySystem self=(GameFramework.Story.GfxStorySystem)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.SuspendMessageHandler(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Story.GfxStorySystem");
		addMember(l,Init);
		addMember(l,Reset);
		addMember(l,Reset__Boolean);
		addMember(l,LoadSceneStories);
		addMember(l,LoadBattleStories);
		addMember(l,LoadStory);
		addMember(l,LoadAiStory);
		addMember(l,ClearStoryInstancePool);
		addMember(l,NewAiStoryInstance);
		addMember(l,RecycleAiStoryInstance);
		addMember(l,GetStory__String);
		addMember(l,GetStory__String__String);
		addMember(l,StartStories__String);
		addMember(l,StartStories__String__String);
		addMember(l,PauseStories__String__Boolean);
		addMember(l,PauseStories__String__String__Boolean);
		addMember(l,StopStories__String);
		addMember(l,StopStories__String__String);
		addMember(l,CountStories__String);
		addMember(l,CountStories__String__String);
		addMember(l,MarkStoriesTerminated__String);
		addMember(l,MarkStoriesTerminated__String__String);
		addMember(l,SkipCurMessageHandlers__String__String);
		addMember(l,SkipCurMessageHandlers__String__String__String);
		addMember(l,StartStory__String);
		addMember(l,StartStory__String__String);
		addMember(l,PauseStory__String__Boolean);
		addMember(l,PauseStory__String__String__Boolean);
		addMember(l,StopStory__String);
		addMember(l,StopStory__String__String);
		addMember(l,CountStory__String);
		addMember(l,CountStory__String__String);
		addMember(l,MarkStoryTerminated__String);
		addMember(l,MarkStoryTerminated__String__String);
		addMember(l,Tick);
		addMember(l,AddBindedStory);
		addMember(l,NewBoxedValueList);
		addMember(l,SendMessage__String);
		addMember(l,SendMessage__String__BoxedValue);
		addMember(l,SendMessage__String__BoxedValueList);
		addMember(l,SendMessage__String__BoxedValue__BoxedValue);
		addMember(l,SendMessage__String__BoxedValue__BoxedValue__BoxedValue);
		addMember(l,SendConcurrentMessage__String);
		addMember(l,SendConcurrentMessage__String__BoxedValue);
		addMember(l,SendConcurrentMessage__String__BoxedValueList);
		addMember(l,SendConcurrentMessage__String__BoxedValue__BoxedValue);
		addMember(l,SendConcurrentMessage__String__BoxedValue__BoxedValue__BoxedValue);
		addMember(l,CountMessage);
		addMember(l,SuspendMessageHandler);
		addMember(l,ThreadInitMask_s);
		addMember(l,"SceneId",get_SceneId,set_SceneId,true);
		addMember(l,"ActiveStoryCount",get_ActiveStoryCount,null,true);
		addMember(l,"GlobalVariables",get_GlobalVariables,null,true);
		addMember(l,"Instance",get_Instance,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.Story.GfxStorySystem));
	}
}
