using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_StoryMessageHandler : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public int Load(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(Dsl.StatementData))){
				StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
				Dsl.StatementData a1;
				checkType(l,2,out a1);
				self.Load(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(Dsl.FunctionData))){
				StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
				Dsl.FunctionData a1;
				checkType(l,2,out a1);
				self.Load(a1);
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
	static public int Reset(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Prepare(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			self.Prepare();
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
	static public int Trigger(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			System.Object[] a2;
			checkArray(l,3,out a2);
			self.Trigger(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int get_IsPaused(IntPtr l) {
		try {
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
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
			StorySystem.StoryMessageHandler self=(StorySystem.StoryMessageHandler)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryMessageHandler");
		addMember(l,Load);
		addMember(l,Reset);
		addMember(l,Prepare);
		addMember(l,Tick);
		addMember(l,Trigger);
		addMember(l,"MessageId",get_MessageId,set_MessageId,true);
		addMember(l,"IsTriggered",get_IsTriggered,set_IsTriggered,true);
		addMember(l,"IsPaused",get_IsPaused,set_IsPaused,true);
		addMember(l,"IsInTick",get_IsInTick,null,true);
		addMember(l,"StackVariables",get_StackVariables,null,true);
		createTypeMetatable(l,constructor, typeof(StorySystem.StoryMessageHandler));
	}
}
