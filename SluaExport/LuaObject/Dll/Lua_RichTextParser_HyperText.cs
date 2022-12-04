using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichTextParser_HyperText : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			RichTextParser.HyperText o;
			o=new RichTextParser.HyperText();
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
	static public int get_Type(IntPtr l) {
		try {
			RichTextParser.HyperText self=(RichTextParser.HyperText)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Attrs(IntPtr l) {
		try {
			RichTextParser.HyperText self=(RichTextParser.HyperText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Attrs);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Texts(IntPtr l) {
		try {
			RichTextParser.HyperText self=(RichTextParser.HyperText)checkSelf(l);
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
		getTypeTable(l,"RichTextParser.HyperText");
		addMember(l,ctor_s);
		addMember(l,"Type",get_Type,null,true);
		addMember(l,"Attrs",get_Attrs,null,true);
		addMember(l,"Texts",get_Texts,null,true);
		createTypeMetatable(l,null, typeof(RichTextParser.HyperText));
	}
}
