using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryInstance : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
	static public int SetVariable(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValue a2;
			checkValueType(l,3,out a2);
			self.SetVariable(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryGetVariable(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			BoxedValue a2;
			var ret=self.TryGetVariable(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2.GetObject());
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveVariable(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.RemoveVariable(a1);
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
	static public int Clone(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			var ret=self.Clone();
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
	static public int Init(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			Dsl.ISyntaxComponent a1;
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int Reset__Boolean(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int NewBoxedValueList(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
	static public int RecycleBoxedValueList(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			BoxedValueList a1;
			checkType(l,2,out a1);
			self.RecycleBoxedValueList(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SendMessage__String(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int SuspendMessageHandler(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
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
	static public int CanSleep(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			var ret=self.CanSleep();
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
	[UnityEngine.Scripting.Preserve]
	static public int GetMessageHandlerEnumerator(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			var ret=self.GetMessageHandlerEnumerator();
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
	static public int GetConcurrentMessageHandlerEnumerator(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			var ret=self.GetConcurrentMessageHandlerEnumerator();
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
	static public int GetMessageHandler(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetMessageHandler(a1);
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
	static public int GetConcurrentMessageHandler__String(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetConcurrentMessageHandler(a1);
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
	static public int GetConcurrentMessageHandler__String__StoryMessageHandlerList(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			StorySystem.StoryMessageHandlerList a2;
			checkType(l,3,out a2);
			var ret=self.GetConcurrentMessageHandler(a1,a2);
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
	[UnityEngine.Scripting.Preserve]
	static public int set_OnExecDebugger(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			StorySystem.StoryCommandDebuggerDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.OnExecDebugger=v;
			else if(op==1) self.OnExecDebugger+=v;
			else if(op==2) self.OnExecDebugger-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int get_Config(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Config);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsDebug(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsDebug);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsDebug(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsDebug=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int set_GlobalVariables(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			StrBoxedValueDict v;
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public int set_StackVariables(IntPtr l) {
		try {
			StorySystem.StoryInstance self=(StorySystem.StoryInstance)checkSelf(l);
			StrBoxedValueDict v;
			checkType(l,2,out v);
			self.StackVariables=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryInstance");
		addMember(l,ctor_s);
		addMember(l,SetVariable);
		addMember(l,TryGetVariable);
		addMember(l,RemoveVariable);
		addMember(l,Clone);
		addMember(l,Init);
		addMember(l,Reset);
		addMember(l,Reset__Boolean);
		addMember(l,Start);
		addMember(l,NewBoxedValueList);
		addMember(l,RecycleBoxedValueList);
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
		addMember(l,ClearMessage);
		addMember(l,SuspendMessageHandler);
		addMember(l,CanSleep);
		addMember(l,Tick);
		addMember(l,GetMessageHandlerEnumerator);
		addMember(l,GetConcurrentMessageHandlerEnumerator);
		addMember(l,GetMessageHandler);
		addMember(l,GetConcurrentMessageHandler__String);
		addMember(l,GetConcurrentMessageHandler__String__StoryMessageHandlerList);
		addMember(l,GetMessageTriggerTime);
		addMember(l,"OnExecDebugger",null,set_OnExecDebugger,true);
		addMember(l,"StoryId",get_StoryId,set_StoryId,true);
		addMember(l,"Namespace",get_Namespace,set_Namespace,true);
		addMember(l,"Config",get_Config,null,true);
		addMember(l,"IsDebug",get_IsDebug,set_IsDebug,true);
		addMember(l,"IsTerminated",get_IsTerminated,set_IsTerminated,true);
		addMember(l,"IsPaused",get_IsPaused,set_IsPaused,true);
		addMember(l,"IsInTick",get_IsInTick,null,true);
		addMember(l,"Context",get_Context,set_Context,true);
		addMember(l,"LocalVariables",get_LocalVariables,null,true);
		addMember(l,"GlobalVariables",get_GlobalVariables,set_GlobalVariables,true);
		addMember(l,"StackVariables",get_StackVariables,set_StackVariables,true);
		createTypeMetatable(l,null, typeof(StorySystem.StoryInstance));
	}
}
