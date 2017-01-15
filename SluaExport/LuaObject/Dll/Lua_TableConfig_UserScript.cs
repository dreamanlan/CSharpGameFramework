using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_TableConfig_UserScript : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			TableConfig.UserScript o;
			o=new TableConfig.UserScript();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ReadFromBinary(IntPtr l) {
		try {
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.ReadFromBinary(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WriteToBinary(IntPtr l) {
		try {
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			GameFramework.BinaryTable a1;
			checkType(l,2,out a1);
			self.WriteToBinary(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetId(IntPtr l) {
		try {
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			var ret=self.GetId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_id(IntPtr l) {
		try {
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_id(IntPtr l) {
		try {
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_StoryId(IntPtr l) {
		try {
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
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
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			System.String v;
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
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
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
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			System.String v;
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
	static public int get_DslFile(IntPtr l) {
		try {
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.DslFile);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_DslFile(IntPtr l) {
		try {
			TableConfig.UserScript self=(TableConfig.UserScript)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.DslFile=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"TableConfig.UserScript");
		addMember(l,ReadFromBinary);
		addMember(l,WriteToBinary);
		addMember(l,GetId);
		addMember(l,"id",get_id,set_id,true);
		addMember(l,"StoryId",get_StoryId,set_StoryId,true);
		addMember(l,"Namespace",get_Namespace,set_Namespace,true);
		addMember(l,"DslFile",get_DslFile,set_DslFile,true);
		createTypeMetatable(l,constructor, typeof(TableConfig.UserScript));
	}
}
