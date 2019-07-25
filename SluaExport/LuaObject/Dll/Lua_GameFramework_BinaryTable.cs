using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_BinaryTable : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.BinaryTable o;
			o=new GameFramework.BinaryTable();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Update(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			self.Update();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddString(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.AddString(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddIntList(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.Int32[] a1;
			checkArray(l,2,out a1);
			var ret=self.AddIntList(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddFloatList(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.Single[] a1;
			checkArray(l,2,out a1);
			var ret=self.AddFloatList(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddStrList(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.String[] a1;
			checkArray(l,2,out a1);
			var ret=self.AddStrList(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetString(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetString(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetIntList(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetIntList(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetFloatList(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetFloatList(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetStrList(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self.GetStrList(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Load(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Load(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Save(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Save(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsValid_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.BinaryTable.IsValid(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_Identity(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.BinaryTable.c_Identity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_c_Version(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.BinaryTable.c_Version);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Records(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Records);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StringList(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StringList);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IntLists(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IntLists);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_FloatLists(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FloatLists);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StrLists(IntPtr l) {
		try {
			GameFramework.BinaryTable self=(GameFramework.BinaryTable)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.StrLists);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.BinaryTable");
		addMember(l,Reset);
		addMember(l,Update);
		addMember(l,AddString);
		addMember(l,AddIntList);
		addMember(l,AddFloatList);
		addMember(l,AddStrList);
		addMember(l,GetString);
		addMember(l,GetIntList);
		addMember(l,GetFloatList);
		addMember(l,GetStrList);
		addMember(l,Load);
		addMember(l,Save);
		addMember(l,IsValid_s);
		addMember(l,"c_Identity",get_c_Identity,null,false);
		addMember(l,"c_Version",get_c_Version,null,false);
		addMember(l,"Records",get_Records,null,true);
		addMember(l,"StringList",get_StringList,null,true);
		addMember(l,"IntLists",get_IntLists,null,true);
		addMember(l,"FloatLists",get_FloatLists,null,true);
		addMember(l,"StrLists",get_StrLists,null,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.BinaryTable));
	}
}
