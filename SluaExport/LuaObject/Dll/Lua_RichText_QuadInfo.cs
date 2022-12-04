using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichText_QuadInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			RichText.QuadInfo o;
			o=new RichText.QuadInfo();
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
	static public int get_Name(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
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
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Size(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Size);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Size(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Size=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Count(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Count(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.Count=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Sprite(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Sprite);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Sprite(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			UnityEngine.Sprite v;
			checkType(l,2,out v);
			self.Sprite=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SpriteResource(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SpriteResource);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SpriteResource(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.SpriteResource=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_InstantiatedPrefab(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.InstantiatedPrefab);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_InstantiatedPrefab(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			UnityEngine.GameObject v;
			checkType(l,2,out v);
			self.InstantiatedPrefab=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_PrefabResource(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PrefabResource);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_PrefabResource(IntPtr l) {
		try {
			RichText.QuadInfo self=(RichText.QuadInfo)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.PrefabResource=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichText.QuadInfo");
		addMember(l,ctor_s);
		addMember(l,"Name",get_Name,set_Name,true);
		addMember(l,"Size",get_Size,set_Size,true);
		addMember(l,"Count",get_Count,set_Count,true);
		addMember(l,"Sprite",get_Sprite,set_Sprite,true);
		addMember(l,"SpriteResource",get_SpriteResource,set_SpriteResource,true);
		addMember(l,"InstantiatedPrefab",get_InstantiatedPrefab,set_InstantiatedPrefab,true);
		addMember(l,"PrefabResource",get_PrefabResource,set_PrefabResource,true);
		createTypeMetatable(l,null, typeof(RichText.QuadInfo));
	}
}
