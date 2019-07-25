using System;

using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_DummyTriger : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.DummyTriger");
		createTypeMetatable(l,constructor, typeof(SkillSystem.DummyTriger),typeof(SkillSystem.AbstractSkillTriger));
	}
}
