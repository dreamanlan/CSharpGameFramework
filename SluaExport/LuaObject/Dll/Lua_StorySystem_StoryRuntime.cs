using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_StoryRuntime : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			StorySystem.StoryRuntime o;
			o=new StorySystem.StoryRuntime();
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
	static public int CountCommand(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
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
	static public int Reset(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
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
	static public int TryBreakLoop(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			var ret=self.TryBreakLoop();
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
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			StorySystem.StoryInstance a1;
			checkType(l,2,out a1);
			StorySystem.StoryMessageHandler a2;
			checkType(l,3,out a2);
			System.Int64 a3;
			checkType(l,4,out a3);
			self.Tick(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int New_s(IntPtr l) {
		try {
			var ret=StorySystem.StoryRuntime.New();
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
	static public int Recycle_s(IntPtr l) {
		try {
			StorySystem.StoryRuntime a1;
			checkType(l,1,out a1);
			StorySystem.StoryRuntime.Recycle(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Iterator(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Iterator.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Iterator(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			BoxedValue v;
			checkValueType(l,2,out v);
			self.Iterator=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Arguments(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Arguments);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Arguments(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			BoxedValueList v;
			checkType(l,2,out v);
			self.Arguments=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CommandQueue(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CommandQueue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_CompositeReentry(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.CompositeReentry);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_CompositeReentry(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.CompositeReentry=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsBreak(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsBreak);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsBreak(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsBreak=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsContinue(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsContinue);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsContinue(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsContinue=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsReturn(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsReturn);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsReturn(IntPtr l) {
		try {
			StorySystem.StoryRuntime self=(StorySystem.StoryRuntime)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsReturn=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.StoryRuntime");
		addMember(l,ctor_s);
		addMember(l,CountCommand);
		addMember(l,Reset);
		addMember(l,TryBreakLoop);
		addMember(l,Tick);
		addMember(l,New_s);
		addMember(l,Recycle_s);
		addMember(l,"Iterator",get_Iterator,set_Iterator,true);
		addMember(l,"Arguments",get_Arguments,set_Arguments,true);
		addMember(l,"CommandQueue",get_CommandQueue,null,true);
		addMember(l,"CompositeReentry",get_CompositeReentry,set_CompositeReentry,true);
		addMember(l,"IsBreak",get_IsBreak,set_IsBreak,true);
		addMember(l,"IsContinue",get_IsContinue,set_IsContinue,true);
		addMember(l,"IsReturn",get_IsReturn,set_IsReturn,true);
		createTypeMetatable(l,null, typeof(StorySystem.StoryRuntime));
	}
}
