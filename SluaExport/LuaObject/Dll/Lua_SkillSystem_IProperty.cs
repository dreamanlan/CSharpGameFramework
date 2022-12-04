using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_IProperty : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Value(IntPtr l) {
		try {
			SkillSystem.IProperty self=(SkillSystem.IProperty)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Value);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Value(IntPtr l) {
		try {
			SkillSystem.IProperty self=(SkillSystem.IProperty)checkSelf(l);
			System.Object v;
			checkType(l,2,out v);
			self.Value=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.IProperty");
		addMember(l,"Value",get_Value,set_Value,true);
		createTypeMetatable(l,null, typeof(SkillSystem.IProperty));
	}
}
