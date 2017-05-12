using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_VR_TrackingSpaceType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.VR.TrackingSpaceType");
		addMember(l,0,"Stationary");
		addMember(l,1,"RoomScale");
		LuaDLL.lua_pop(l, 1);
	}
}
