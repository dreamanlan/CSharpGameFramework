using System;

using SLua;
using System.Collections.Generic;
public class Lua_Dsl_CallData_ParamClassEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Dsl.FunctionData.ParamClassEnum");
		addMember(l,0,"PARAM_CLASS_NOTHING");
		addMember(l,0,"PARAM_CLASS_MIN");
		addMember(l,1,"PARAM_CLASS_PARENTHESIS");
		addMember(l,2,"PARAM_CLASS_BRACKET");
		addMember(l,3,"PARAM_CLASS_PERIOD");
		addMember(l,4,"PARAM_CLASS_PERIOD_PARENTHESIS");
		addMember(l,5,"PARAM_CLASS_PERIOD_BRACKET");
		addMember(l,6,"PARAM_CLASS_PERIOD_BRACE");
		addMember(l,7,"PARAM_CLASS_OPERATOR");
		addMember(l,8,"PARAM_CLASS_TERNARY_OPERATOR");
		addMember(l,9,"PARAM_CLASS_MAX");
		addMember(l,127,"PARAM_CLASS_UNMASK");
		addMember(l,128,"PARAM_CLASS_WRAP_OBJECT_MEMBER_ASSIGN_MASK");
		LuaDLL.lua_pop(l, 1);
	}
}
