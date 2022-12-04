using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryMessageHandler : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler o;
			o=new StorySystem.StoryMessageHandler();
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
	static public int PeekLocalInfo(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			var ret=self.PeekLocalInfo();
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
	static public int PushLocalInfo(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryLocalInfo a1;
			checkType(l,2,out a1);
			self.PushLocalInfo(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PopLocalInfo(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			self.PopLocalInfo();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PeekRuntime(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			var ret=self.PeekRuntime();
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
	static public int PushRuntime(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryRuntime a1;
			checkType(l,2,out a1);
			self.PushRuntime(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PopRuntime(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			self.PopRuntime(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CountCommand(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			var ret=self.CountCommand();
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
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
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
	static public int Load__FunctionData__String(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			Dsl.FunctionData a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.Load(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Load__StatementData__String(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			Dsl.StatementData a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.Load(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reset__StoryInstance(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryInstance a1;
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
	static public int Reset__StoryInstance__Boolean(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.Reset(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Prepare(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			self.Prepare(a1);
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
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			System.Int64 a2;
			checkType(l,3,out a2);
			self.Tick(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Trigger(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			BoxedValueList a2;
			checkType(l,3,out a2);
			self.Trigger(a1,a2);
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
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
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
	static public int get_MessageId(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.MessageId);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_MessageId(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.MessageId=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Comments(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Comments);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CanSkip(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CanSkip);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_CanSkip(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.CanSkip=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsTriggered(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsTriggered);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsTriggered(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsTriggered=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsSuspended(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsSuspended);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsSuspended(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsSuspended=v;
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
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
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
	static public int get_StackVariables(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
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
	static public int get_LocalInfoStack(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LocalInfoStack);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_RuntimeStack(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.RuntimeStack);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryMessageHandler");
		addMember(l,ctor_s);
		addMember(l,PeekLocalInfo);
		addMember(l,PushLocalInfo);
		addMember(l,PopLocalInfo);
		addMember(l,PeekRuntime);
		addMember(l,PushRuntime);
		addMember(l,PopRuntime);
		addMember(l,CountCommand);
		addMember(l,Clone);
		addMember(l,Load__FunctionData__String);
		addMember(l,Load__StatementData__String);
		addMember(l,Reset__StoryInstance);
		addMember(l,Reset__StoryInstance__Boolean);
		addMember(l,Prepare);
		addMember(l,Tick);
		addMember(l,Trigger);
		addMember(l,"StoryId",get_StoryId,null,true);
		addMember(l,"MessageId",get_MessageId,set_MessageId,true);
		addMember(l,"Comments",get_Comments,null,true);
		addMember(l,"CanSkip",get_CanSkip,set_CanSkip,true);
		addMember(l,"IsTriggered",get_IsTriggered,set_IsTriggered,true);
		addMember(l,"IsSuspended",get_IsSuspended,set_IsSuspended,true);
		addMember(l,"IsInTick",get_IsInTick,null,true);
		addMember(l,"StackVariables",get_StackVariables,null,true);
		addMember(l,"LocalInfoStack",get_LocalInfoStack,null,true);
		addMember(l,"RuntimeStack",get_RuntimeStack,null,true);
		createTypeMetatable(l,null, typeof(StorySystem.StoryMessageHandler));
	}
}
