using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_SkillSystem_IPropertyAccessor : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryGetProperty(IntPtr l) {
		try {
			SkillSystem.IPropertyAccessor self=(SkillSystem.IPropertyAccessor)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			var ret=self.TryGetProperty(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetProperty(IntPtr l) {
		try {
			SkillSystem.IPropertyAccessor self=(SkillSystem.IPropertyAccessor)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.SetProperty(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"SkillSystem.IPropertyAccessor");
		addMember(l,TryGetProperty);
		addMember(l,SetProperty);
		createTypeMetatable(l,null, typeof(SkillSystem.IPropertyAccessor));
	}
}
