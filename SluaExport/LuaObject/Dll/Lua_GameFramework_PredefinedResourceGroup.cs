using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_PredefinedResourceGroup : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"GameFramework.PredefinedResourceGroup");
		addMember(l,0,"Default");
		addMember(l,1,"PlayerSkillEffect");
		addMember(l,2,"PlayerImpactEffect");
		addMember(l,3,"PlayerBuffEffect");
		addMember(l,4,"OtherSkillEffect");
		addMember(l,5,"OtherImpactEffect");
		addMember(l,6,"OtherBuffEffect");
		addMember(l,7,"Miscellaneous");
		addMember(l,8,"Sound");
		addMember(l,9,"MaxCount");
		LuaDLL.lua_pop(l, 1);
	}
}
