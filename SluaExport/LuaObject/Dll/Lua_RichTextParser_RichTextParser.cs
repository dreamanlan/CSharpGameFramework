using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichTextParser_RichTextParser : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			RichTextParser.RichTextParser o;
			o=new RichTextParser.RichTextParser();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Parse(IntPtr l) {
		try {
			RichTextParser.RichTextParser self=(RichTextParser.RichTextParser)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Parse(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Texts(IntPtr l) {
		try {
			RichTextParser.RichTextParser self=(RichTextParser.RichTextParser)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Texts);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichTextParser.RichTextParser");
		addMember(l,ctor_s);
		addMember(l,Parse);
		addMember(l,"Texts",get_Texts,null,true);
		createTypeMetatable(l,null, typeof(RichTextParser.RichTextParser));
	}
}
