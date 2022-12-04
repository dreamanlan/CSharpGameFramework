using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichTextParser_HyperTextAttr : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			RichTextParser.HyperTextAttr o;
			o=new RichTextParser.HyperTextAttr();
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
	static public int get_Key(IntPtr l) {
		try {
			RichTextParser.HyperTextAttr self=(RichTextParser.HyperTextAttr)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Key);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Key(IntPtr l) {
		try {
			RichTextParser.HyperTextAttr self=(RichTextParser.HyperTextAttr)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.Key=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Value(IntPtr l) {
		try {
			RichTextParser.HyperTextAttr self=(RichTextParser.HyperTextAttr)checkSelf(l);
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
			RichTextParser.HyperTextAttr self=(RichTextParser.HyperTextAttr)checkSelf(l);
			string v;
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
		getTypeTable(l,"RichTextParser.HyperTextAttr");
		addMember(l,ctor_s);
		addMember(l,"Key",get_Key,set_Key,true);
		addMember(l,"Value",get_Value,set_Value,true);
		createTypeMetatable(l,null, typeof(RichTextParser.HyperTextAttr));
	}
}
