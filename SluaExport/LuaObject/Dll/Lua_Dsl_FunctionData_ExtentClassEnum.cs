using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Dsl_FunctionData_ExtentClassEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Dsl.FunctionData.ExtentClassEnum");
		addMember(l,0,"EXTENT_CLASS_NOTHING");
		addMember(l,0,"EXTENT_CLASS_MIN");
		addMember(l,1,"EXTENT_CLASS_STATEMENT");
		addMember(l,2,"EXTENT_CLASS_EXTERN_SCRIPT");
		addMember(l,3,"EXTENT_CLASS_MAX");
		LuaDLL.lua_pop(l, 1);
	}
}
