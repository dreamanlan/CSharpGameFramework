using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_AiData_ForMoveCommand : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Index(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand self=(GameFramework.AiData_ForMoveCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Index);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Index(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand self=(GameFramework.AiData_ForMoveCommand)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Index=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsFinish(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand self=(GameFramework.AiData_ForMoveCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFinish);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsFinish(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand self=(GameFramework.AiData_ForMoveCommand)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsFinish=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Event(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand self=(GameFramework.AiData_ForMoveCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Event);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Event(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand self=(GameFramework.AiData_ForMoveCommand)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.Event=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AiData_ForMoveCommand");
		addMember(l,"Index",get_Index,set_Index,true);
		addMember(l,"IsFinish",get_IsFinish,set_IsFinish,true);
		addMember(l,"Event",get_Event,set_Event,true);
		createTypeMetatable(l,null, typeof(GameFramework.AiData_ForMoveCommand));
	}
}
