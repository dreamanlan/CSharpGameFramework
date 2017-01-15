using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_Helper_Random : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.Helper.Random o;
			o=new GameFramework.Helper.Random();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Next_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==0){
				var ret=GameFramework.Helper.Random.Next();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==1){
				System.Int32 a1;
				checkType(l,1,out a1);
				var ret=GameFramework.Helper.Random.Next(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.Int32 a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=GameFramework.Helper.Random.Next(a1,a2);
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
	static public int NextFloat_s(IntPtr l) {
		try {
			var ret=GameFramework.Helper.Random.NextFloat();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.Helper.Random");
		addMember(l,Next_s);
		addMember(l,NextFloat_s);
		createTypeMetatable(l,constructor, typeof(GameFramework.Helper.Random));
	}
}
