using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_BinaryTable_FileHeader : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader o;
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			o=new GameFramework.BinaryTable.FileHeader(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Identity(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_Identity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Identity(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_Identity=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_Version(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_Version);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_Version(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_Version=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_RecordNum(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_RecordNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_RecordNum(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_RecordNum=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_RecordSize(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_RecordSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_RecordSize(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_RecordSize=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_StringOffset(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_StringOffset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_StringOffset(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_StringOffset=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_IntListOffset(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_IntListOffset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_IntListOffset(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_IntListOffset=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_FloatListOffset(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_FloatListOffset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_FloatListOffset(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_FloatListOffset=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_m_StrListOffset(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.m_StrListOffset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_m_StrListOffset(IntPtr l) {
		try {
			GameFramework.BinaryTable.FileHeader self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.m_StrListOffset=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.BinaryTable.FileHeader");
		addMember(l,"m_Identity",get_m_Identity,set_m_Identity,true);
		addMember(l,"m_Version",get_m_Version,set_m_Version,true);
		addMember(l,"m_RecordNum",get_m_RecordNum,set_m_RecordNum,true);
		addMember(l,"m_RecordSize",get_m_RecordSize,set_m_RecordSize,true);
		addMember(l,"m_StringOffset",get_m_StringOffset,set_m_StringOffset,true);
		addMember(l,"m_IntListOffset",get_m_IntListOffset,set_m_IntListOffset,true);
		addMember(l,"m_FloatListOffset",get_m_FloatListOffset,set_m_FloatListOffset,true);
		addMember(l,"m_StrListOffset",get_m_StrListOffset,set_m_StrListOffset,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.BinaryTable.FileHeader),typeof(System.ValueType));
	}
}
