using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichTextAsset : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			RichTextAsset o;
			o=new RichTextAsset();
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
	static public int get_texSource(IntPtr l) {
		try {
			RichTextAsset self=(RichTextAsset)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.texSource);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_texSource(IntPtr l) {
		try {
			RichTextAsset self=(RichTextAsset)checkSelf(l);
			UnityEngine.Texture v;
			checkType(l,2,out v);
			self.texSource=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichTextAsset");
		addMember(l,ctor_s);
		addMember(l,"texSource",get_texSource,set_texSource,true);
		createTypeMetatable(l,null, typeof(RichTextAsset),typeof(UnityEngine.ScriptableObject));
	}
}
