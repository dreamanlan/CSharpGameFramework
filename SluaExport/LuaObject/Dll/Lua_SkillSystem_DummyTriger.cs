using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_DummyTriger : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			SkillSystem.DummyTriger o;
			o=new SkillSystem.DummyTriger();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.DummyTriger");
		addMember(l,ctor_s);
		createTypeMetatable(l,null, typeof(SkillSystem.DummyTriger),typeof(SkillSystem.AbstractSkillTriger));
	}
}
