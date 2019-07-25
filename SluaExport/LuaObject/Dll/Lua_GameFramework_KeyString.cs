using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_KeyString : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.KeyString o;
			o=new GameFramework.KeyString();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Equality(IntPtr l) {
		try {
			GameFramework.KeyString a1;
			checkType(l,1,out a1);
			GameFramework.KeyString a2;
			checkType(l,2,out a2);
			var ret=(a1==a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int op_Inequality(IntPtr l) {
		try {
			GameFramework.KeyString a1;
			checkType(l,1,out a1);
			GameFramework.KeyString a2;
			checkType(l,2,out a2);
			var ret=(a1!=a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsNullOrEmpty_s(IntPtr l) {
		try {
			GameFramework.KeyString a1;
			checkType(l,1,out a1);
			var ret=GameFramework.KeyString.IsNullOrEmpty(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Wrap_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.String[]))){
				System.String[] a1;
				checkParams(l,1,out a1);
				var ret=GameFramework.KeyString.Wrap(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(List<System.String>))){
				System.Collections.Generic.List<System.String> a1;
				checkType(l,1,out a1);
				var ret=GameFramework.KeyString.Wrap(a1);
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
	static public int get_Keys(IntPtr l) {
		try {
			GameFramework.KeyString self=(GameFramework.KeyString)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Keys);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Keys(IntPtr l) {
		try {
			GameFramework.KeyString self=(GameFramework.KeyString)checkSelf(l);
			List<System.String> v;
			checkType(l,2,out v);
			self.Keys=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.KeyString");
		addMember(l,op_Equality);
		addMember(l,op_Inequality);
		addMember(l,IsNullOrEmpty_s);
		addMember(l,Wrap_s);
		addMember(l,"Keys",get_Keys,set_Keys,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.KeyString));
	}
}
