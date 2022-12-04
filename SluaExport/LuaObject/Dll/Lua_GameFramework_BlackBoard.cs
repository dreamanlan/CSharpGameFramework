using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_BlackBoard : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.BlackBoard");
		addMember(l,ctor_s);
		addMember(l,ClearVariables);
		addMember(l,SetVariable);
		addMember(l,TryGetVariable);
		createTypeMetatable(l,null, typeof(GameFramework.BlackBoard));
	}
}
