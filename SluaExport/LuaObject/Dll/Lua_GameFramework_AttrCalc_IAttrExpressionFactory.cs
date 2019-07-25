using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_AttrCalc_IAttrExpressionFactory : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Create(IntPtr l) {
		try {
			GameFramework.AttrCalc.IAttrExpressionFactory self=(GameFramework.AttrCalc.IAttrExpressionFactory)checkSelf(l);
			var ret=self.Create();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AttrCalc.IAttrExpressionFactory");
		addMember(l,Create);
		createTypeMetatable(l,null, typeof(GameFramework.AttrCalc.IAttrExpressionFactory));
	}
}
