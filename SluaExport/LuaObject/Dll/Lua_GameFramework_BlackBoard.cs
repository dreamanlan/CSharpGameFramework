using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_BlackBoard : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			GameFramework.BlackBoard o;
			o=new GameFramework.BlackBoard();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearVariables(IntPtr l) {
		try {
			GameFramework.BlackBoard self=(GameFramework.BlackBoard)checkSelf(l);
			self.ClearVariables();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetVariable(IntPtr l) {
		try {
			GameFramework.BlackBoard self=(GameFramework.BlackBoard)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.SetVariable(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryGetVariable(IntPtr l) {
		try {
			GameFramework.BlackBoard self=(GameFramework.BlackBoard)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			var ret=self.TryGetVariable(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.BlackBoard self=(GameFramework.BlackBoard)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_BlackBoardDatas(IntPtr l) {
		try {
			GameFramework.BlackBoard self=(GameFramework.BlackBoard)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.BlackBoardDatas);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_IsGameOver(IntPtr l) {
		try {
			GameFramework.BlackBoard self=(GameFramework.BlackBoard)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsGameOver);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_IsGameOver(IntPtr l) {
		try {
			GameFramework.BlackBoard self=(GameFramework.BlackBoard)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsGameOver=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.BlackBoard");
		addMember(l,ClearVariables);
		addMember(l,SetVariable);
		addMember(l,TryGetVariable);
		addMember(l,Reset);
		addMember(l,"BlackBoardDatas",get_BlackBoardDatas,null,true);
		addMember(l,"IsGameOver",get_IsGameOver,set_IsGameOver,true);
		createTypeMetatable(l,constructor, typeof(GameFramework.BlackBoard));
	}
}
