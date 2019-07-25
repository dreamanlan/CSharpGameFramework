using System;

using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_Formation_PosDir : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.Formation.PosDir o;
			System.Collections.Generic.List<System.Single> a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			o=new TableConfig.Formation.PosDir(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcPosDir(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			var ret=self.CalcPosDir(a1,a2,out a3);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_X(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.X);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_X(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.X=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Y(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Y);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Y(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Y=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Z(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Z);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Z(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Z=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Dir(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Dir);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Dir(IntPtr l) {
		try {
			TableConfig.Formation.PosDir self;
			checkValueType(l,1,out self);
			System.Single v;
			checkType(l,2,out v);
			self.Dir=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Zero(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,TableConfig.Formation.PosDir.Zero);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Zero(IntPtr l) {
		try {
			TableConfig.Formation.PosDir v;
			checkValueType(l,2,out v);
			TableConfig.Formation.PosDir.Zero=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.Formation.PosDir");
		addMember(l,CalcPosDir);
		addMember(l,"X",get_X,set_X,true);
		addMember(l,"Y",get_Y,set_Y,true);
		addMember(l,"Z",get_Z,set_Z,true);
		addMember(l,"Dir",get_Dir,set_Dir,true);
		addMember(l,"Zero",get_Zero,set_Zero,false);
		createTypeMetatable(l,constructor, typeof(TableConfig.Formation.PosDir),typeof(System.ValueType));
	}
}
