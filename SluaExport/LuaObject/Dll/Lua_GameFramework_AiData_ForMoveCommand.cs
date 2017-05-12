using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_AiData_ForMoveCommand : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand o;
			System.Collections.Generic.List<ScriptRuntime.Vector3> a1;
			checkType(l,2,out a1);
			o=new GameFramework.AiData_ForMoveCommand(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_WayPoints(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand self=(GameFramework.AiData_ForMoveCommand)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.WayPoints);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_WayPoints(IntPtr l) {
		try {
			GameFramework.AiData_ForMoveCommand self=(GameFramework.AiData_ForMoveCommand)checkSelf(l);
			List<ScriptRuntime.Vector3> v;
			checkType(l,2,out v);
			self.WayPoints=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AiData_ForMoveCommand");
		addMember(l,"WayPoints",get_WayPoints,set_WayPoints,true);
		addMember(l,"Index",get_Index,set_Index,true);
		addMember(l,"IsFinish",get_IsFinish,set_IsFinish,true);
		addMember(l,"Event",get_Event,set_Event,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.AiData_ForMoveCommand));
	}
}
