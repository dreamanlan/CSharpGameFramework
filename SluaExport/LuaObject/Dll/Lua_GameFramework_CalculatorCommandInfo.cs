using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_CalculatorCommandInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.CalculatorCommandInfo o;
			o=new GameFramework.CalculatorCommandInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Strings(IntPtr l) {
		try {
			GameFramework.CalculatorCommandInfo self=(GameFramework.CalculatorCommandInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Strings);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Args(IntPtr l) {
		try {
			GameFramework.CalculatorCommandInfo self=(GameFramework.CalculatorCommandInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Args);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.CalculatorCommandInfo");
		addMember(l,"Strings",get_Strings,null,true);
		addMember(l,"Args",get_Args,null,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.CalculatorCommandInfo));
	}
}
