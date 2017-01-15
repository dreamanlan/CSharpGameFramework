using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_PointController : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			PointController o;
			o=new PointController();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PointList_s(IntPtr l) {
		try {
			UnityEngine.Vector3[] a1;
			checkArray(l,1,out a1);
			System.Int32 a2;
			checkType(l,2,out a2);
			var ret=PointController.PointList(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"PointController");
		addMember(l,PointList_s);
		createTypeMetatable(l,constructor, typeof(PointController));
	}
}
