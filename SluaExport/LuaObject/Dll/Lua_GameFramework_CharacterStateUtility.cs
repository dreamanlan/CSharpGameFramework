using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_CharacterStateUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NameToState_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.CharacterStateUtility.NameToState(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StateToName_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=GameFramework.CharacterStateUtility.StateToName(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.CharacterStateUtility");
		addMember(l,NameToState_s);
		addMember(l,StateToName_s);
		createTypeMetatable(l,null, typeof(GameFramework.CharacterStateUtility));
	}
}
