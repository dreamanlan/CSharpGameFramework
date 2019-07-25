using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_CustomDataCollection : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.CustomDataCollection o;
			o=new GameFramework.CustomDataCollection();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetOrNewData(IntPtr l) {
		try {
			GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
			System.Type a1;
			checkType(l,2,out a1);
			System.Object a2;
			self.GetOrNewData(a1,out a2);
			pushValue(l,true);
			pushValue(l,a2);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddData(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string),typeof(System.Object))){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				self.AddData(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(System.Type),typeof(System.Object))){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				System.Type a1;
				checkType(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				self.AddData(a1,a2);
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
	static public int RemoveData(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string))){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.RemoveData(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(System.Type))){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				System.Type a1;
				checkType(l,2,out a1);
				self.RemoveData(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				System.Type a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				self.RemoveData(a1,a2);
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
	static public int GetData(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,2,typeof(string))){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.GetData(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Type))){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				System.Type a1;
				checkType(l,2,out a1);
				var ret=self.GetData(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int Clear(IntPtr l) {
		try {
			GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Visit(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				GameFramework.MyAction<System.String,System.Object> a1;
				LuaDelegation.checkDelegate(l,2,out a1);
				self.Visit(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				GameFramework.CustomDataCollection self=(GameFramework.CustomDataCollection)checkSelf(l);
				GameFramework.MyAction<System.String,System.Object> a1;
				LuaDelegation.checkDelegate(l,2,out a1);
				System.Object a2;
				checkType(l,3,out a2);
				self.Visit(a1,a2);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.CustomDataCollection");
		addMember(l,GetOrNewData);
		addMember(l,AddData);
		addMember(l,RemoveData);
		addMember(l,GetData);
		addMember(l,Clear);
		addMember(l,Visit);
		createTypeMetatable(l,constructor, typeof(GameFramework.CustomDataCollection));
	}
}
