﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_SkillSystem_IPropertyVisitor : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int VisitProperties(IntPtr l) {
		try {
			SkillSystem.IPropertyVisitor self=(SkillSystem.IPropertyVisitor)checkSelf(l);
			SkillSystem.VisitPropertyDelegation a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.VisitProperties(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.IPropertyVisitor");
		addMember(l,VisitProperties);
		createTypeMetatable(l,null, typeof(SkillSystem.IPropertyVisitor));
	}
}
