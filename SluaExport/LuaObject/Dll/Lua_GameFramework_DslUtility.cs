using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_DslUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcVector2_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			var ret=GameFramework.DslUtility.CalcVector2(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcVector3_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			var ret=GameFramework.DslUtility.CalcVector3(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcVector4_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			var ret=GameFramework.DslUtility.CalcVector4(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcColor_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			var ret=GameFramework.DslUtility.CalcColor(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcQuaternion_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			var ret=GameFramework.DslUtility.CalcQuaternion(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcEularRotation_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			var ret=GameFramework.DslUtility.CalcEularRotation(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcEularAngles_s(IntPtr l) {
		try {
			Dsl.FunctionData a1;
			checkType(l,1,out a1);
			var ret=GameFramework.DslUtility.CalcEularAngles(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.DslUtility");
		addMember(l,CalcVector2_s);
		addMember(l,CalcVector3_s);
		addMember(l,CalcVector4_s);
		addMember(l,CalcColor_s);
		addMember(l,CalcQuaternion_s);
		addMember(l,CalcEularRotation_s);
		addMember(l,CalcEularAngles_s);
		createTypeMetatable(l,null, typeof(GameFramework.DslUtility));
	}
}
