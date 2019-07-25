using System;

using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_StoryInstance : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			StorySystem.StoryInstance o;
			o=new StorySystem.StoryInstance();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetVariable(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.SetVariable(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryGetVariable(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			var ret=self.TryGetVariable(a1,out a2);
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
	static public int Init(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			Dsl.DslInfo a1;
			checkType(l,2,out a1);
			var ret=self.Init(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			self.Start();
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
	static public int ClearMessage(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String[] a1;
			checkParams(l,2,out a1);
			self.ClearMessage(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PauseMessageHandler(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
	static public int Tick(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.Int64 a1;
			checkType(l,2,out a1);
			self.Tick(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetMessageTriggerTime(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetMessageTriggerTime(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StoryId(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StoryId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_StoryId(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.StoryId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Namespace(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Namespace);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Namespace(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.Namespace=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsTerminated(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsTerminated);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsTerminated(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsTerminated=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsPaused(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsPaused);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsPaused(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsPaused=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsInTick(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsInTick);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Context(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Context);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Context(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.Context=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LocalVariables(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LocalVariables);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_GlobalVariables(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.GlobalVariables);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_GlobalVariables(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			Dictionary<System.String,object> v;
			checkType(l,2,out v);
			self.GlobalVariables=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StackVariables(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StackVariables);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_StackVariables(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			Dictionary<System.String,object> v;
			checkType(l,2,out v);
			self.StackVariables=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryInstance");
		addMember(l,SetVariable);
		addMember(l,TryGetVariable);
		addMember(l,Init);
		addMember(l,Reset);
		addMember(l,Start);
		addMember(l,SendMessage);
		addMember(l,SendConcurrentMessage);
		addMember(l,CountMessage);
		addMember(l,ClearMessage);
		addMember(l,PauseMessageHandler);
		addMember(l,Tick);
		addMember(l,GetMessageTriggerTime);
		addMember(l,"StoryId",get_StoryId,set_StoryId,true);
		addMember(l,"Namespace",get_Namespace,set_Namespace,true);
		addMember(l,"IsTerminated",get_IsTerminated,set_IsTerminated,true);
		addMember(l,"IsPaused",get_IsPaused,set_IsPaused,true);
		addMember(l,"IsInTick",get_IsInTick,null,true);
		addMember(l,"Context",get_Context,set_Context,true);
		addMember(l,"LocalVariables",get_LocalVariables,null,true);
		addMember(l,"GlobalVariables",get_GlobalVariables,set_GlobalVariables,true);
		addMember(l,"StackVariables",get_StackVariables,set_StackVariables,true);
		createTypeMetatable(l,constructor, typeof(StorySystem.StoryInstance));
	}
}
