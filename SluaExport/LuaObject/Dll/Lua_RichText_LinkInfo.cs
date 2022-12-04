using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichText_LinkInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			RichText.LinkInfo o;
			o=new RichText.LinkInfo();
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
	static public int get_Color(IntPtr l) {
		try {
			RichText.LinkInfo self=(RichText.LinkInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Color);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Color(IntPtr l) {
		try {
			RichText.LinkInfo self=(RichText.LinkInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.Color=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Name(IntPtr l) {
		try {
			RichText.LinkInfo self=(RichText.LinkInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Name(IntPtr l) {
		try {
			RichText.LinkInfo self=(RichText.LinkInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.Name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichText.LinkInfo");
		addMember(l,ctor_s);
		addMember(l,"Color",get_Color,set_Color,true);
		addMember(l,"Name",get_Name,set_Name,true);
		createTypeMetatable(l,null, typeof(RichText.LinkInfo));
	}
}
