using System;

using SLua;
using System.Collections.Generic;
public class Lua_GameFramework_SceneOperationResultEnum : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.SceneOperationResultEnum");
		addMember(l,0,"Success");
		addMember(l,1,"Cant_Find_Room");
		addMember(l,2,"Cant_Find_User");
		addMember(l,3,"User_Key_Exist");
		addMember(l,4,"Not_Field_Room");
		LuaDLL.lua_pop(l, 1);
	}
}
