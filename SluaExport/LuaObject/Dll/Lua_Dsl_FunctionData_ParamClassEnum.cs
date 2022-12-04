using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_FunctionData_ParamClassEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Dsl.FunctionData.ParamClassEnum");
		addMember(l,0,"PARAM_CLASS_MIN");
		addMember(l,0,"PARAM_CLASS_NOTHING");
		addMember(l,1,"PARAM_CLASS_PARENTHESIS");
		addMember(l,2,"PARAM_CLASS_BRACKET");
		addMember(l,3,"PARAM_CLASS_PERIOD");
		addMember(l,4,"PARAM_CLASS_PERIOD_PARENTHESIS");
		addMember(l,5,"PARAM_CLASS_PERIOD_BRACKET");
		addMember(l,6,"PARAM_CLASS_PERIOD_BRACE");
		addMember(l,7,"PARAM_CLASS_QUESTION_PERIOD");
		addMember(l,8,"PARAM_CLASS_QUESTION_PARENTHESIS");
		addMember(l,9,"PARAM_CLASS_QUESTION_BRACKET");
		addMember(l,10,"PARAM_CLASS_QUESTION_BRACE");
		addMember(l,11,"PARAM_CLASS_POINTER");
		addMember(l,12,"PARAM_CLASS_STATEMENT");
		addMember(l,13,"PARAM_CLASS_EXTERN_SCRIPT");
		addMember(l,14,"PARAM_CLASS_PARENTHESIS_COLON");
		addMember(l,15,"PARAM_CLASS_BRACKET_COLON");
		addMember(l,16,"PARAM_CLASS_ANGLE_BRACKET_COLON");
		addMember(l,17,"PARAM_CLASS_PARENTHESIS_PERCENT");
		addMember(l,18,"PARAM_CLASS_BRACKET_PERCENT");
		addMember(l,19,"PARAM_CLASS_BRACE_PERCENT");
		addMember(l,20,"PARAM_CLASS_ANGLE_BRACKET_PERCENT");
		addMember(l,21,"PARAM_CLASS_COLON_COLON");
		addMember(l,22,"PARAM_CLASS_COLON_COLON_PARENTHESIS");
		addMember(l,23,"PARAM_CLASS_COLON_COLON_BRACKET");
		addMember(l,24,"PARAM_CLASS_COLON_COLON_BRACE");
		addMember(l,25,"PARAM_CLASS_PERIOD_STAR");
		addMember(l,26,"PARAM_CLASS_QUESTION_PERIOD_STAR");
		addMember(l,27,"PARAM_CLASS_POINTER_STAR");
		addMember(l,28,"PARAM_CLASS_OPERATOR");
		addMember(l,29,"PARAM_CLASS_TERNARY_OPERATOR");
		addMember(l,30,"PARAM_CLASS_MAX");
		addMember(l,31,"PARAM_CLASS_UNMASK");
		addMember(l,32,"PARAM_CLASS_WRAP_INFIX_CALL_MASK");
		LuaDLL.lua_pop(l, 1);
	}
}
