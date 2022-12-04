using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichText_DrawSpriteInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			RichText.DrawSpriteInfo o;
			o=new RichText.DrawSpriteInfo();
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
	static public int get_id(IntPtr l) {
		try {
			RichText.DrawSpriteInfo self=(RichText.DrawSpriteInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.id);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_id(IntPtr l) {
		try {
			RichText.DrawSpriteInfo self=(RichText.DrawSpriteInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.id=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_name(IntPtr l) {
		try {
			RichText.DrawSpriteInfo self=(RichText.DrawSpriteInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_name(IntPtr l) {
		try {
			RichText.DrawSpriteInfo self=(RichText.DrawSpriteInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.name=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_vertices(IntPtr l) {
		try {
			RichText.DrawSpriteInfo self=(RichText.DrawSpriteInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.vertices);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_vertices(IntPtr l) {
		try {
			RichText.DrawSpriteInfo self=(RichText.DrawSpriteInfo)checkSelf(l);
			UnityEngine.Vector3[] v;
			checkArray(l,2,out v);
			self.vertices=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_alreadyDraw(IntPtr l) {
		try {
			RichText.DrawSpriteInfo self=(RichText.DrawSpriteInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.alreadyDraw);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_alreadyDraw(IntPtr l) {
		try {
			RichText.DrawSpriteInfo self=(RichText.DrawSpriteInfo)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.alreadyDraw=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichText.DrawSpriteInfo");
		addMember(l,ctor_s);
		addMember(l,"id",get_id,set_id,true);
		addMember(l,"name",get_name,set_name,true);
		addMember(l,"vertices",get_vertices,set_vertices,true);
		addMember(l,"alreadyDraw",get_alreadyDraw,set_alreadyDraw,true);
		createTypeMetatable(l,null, typeof(RichText.DrawSpriteInfo));
	}
}
