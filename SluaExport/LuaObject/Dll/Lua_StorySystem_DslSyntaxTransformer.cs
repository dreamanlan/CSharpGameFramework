using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_DslSyntaxTransformer : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int TryTransformCommandLineLikeSyntax_s(IntPtr l) {
		try {
			Dsl.StatementData a1;
			checkType(l,1,out a1);
			Dsl.FunctionData a2;
			var ret=StorySystem.DslSyntaxTransformer.TryTransformCommandLineLikeSyntax(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.DslSyntaxTransformer");
		addMember(l,TryTransformCommandLineLikeSyntax_s);
		createTypeMetatable(l,null, typeof(StorySystem.DslSyntaxTransformer));
	}
}
