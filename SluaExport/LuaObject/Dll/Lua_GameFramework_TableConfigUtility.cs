using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_TableConfigUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GenStoryDlgItemId_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=GameFramework.TableConfigUtility.GenStoryDlgItemId(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GenLevelMonstersId_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=GameFramework.TableConfigUtility.GenLevelMonstersId(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.TableConfigUtility");
		addMember(l,GenStoryDlgItemId_s);
		addMember(l,GenLevelMonstersId_s);
		createTypeMetatable(l,null, typeof(GameFramework.TableConfigUtility));
	}
}
