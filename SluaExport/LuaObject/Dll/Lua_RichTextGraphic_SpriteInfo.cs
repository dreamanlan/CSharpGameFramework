using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichTextGraphic_SpriteInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			RichTextGraphic.SpriteInfo o;
			o=new RichTextGraphic.SpriteInfo();
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
	static public int get_text(IntPtr l) {
		try {
			RichTextGraphic.SpriteInfo self=(RichTextGraphic.SpriteInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.text);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_text(IntPtr l) {
		try {
			RichTextGraphic.SpriteInfo self=(RichTextGraphic.SpriteInfo)checkSelf(l);
			RichText v;
			checkType(l,2,out v);
			self.text=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichTextGraphic.SpriteInfo");
		addMember(l,ctor_s);
		addMember(l,"text",get_text,set_text,true);
		createTypeMetatable(l,null, typeof(RichTextGraphic.SpriteInfo));
	}
}
