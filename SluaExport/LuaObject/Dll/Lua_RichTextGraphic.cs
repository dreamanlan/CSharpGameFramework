using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichTextGraphic : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RmoveSpriteInfo(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			self.RmoveSpriteInfo();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CalulateUVforRichText(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			RichText a1;
			checkType(l,2,out a1);
			self.CalulateUVforRichText(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_SpriteAsset(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.SpriteAsset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_SpriteAsset(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			RichTextAsset v;
			checkType(l,2,out v);
			self.SpriteAsset=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AnimSpriteNum(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.AnimSpriteNum);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_AnimSpriteNum(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.AnimSpriteNum=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_mainTexture(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.mainTexture);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_material(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.material);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_material(IntPtr l) {
		try {
			RichTextGraphic self=(RichTextGraphic)checkSelf(l);
			UnityEngine.Material v;
			checkType(l,2,out v);
			self.material=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichTextGraphic");
		addMember(l,RmoveSpriteInfo);
		addMember(l,CalulateUVforRichText);
		addMember(l,"SpriteAsset",get_SpriteAsset,set_SpriteAsset,true);
		addMember(l,"AnimSpriteNum",get_AnimSpriteNum,set_AnimSpriteNum,true);
		addMember(l,"mainTexture",get_mainTexture,null,true);
		addMember(l,"material",get_material,set_material,true);
		createTypeMetatable(l,null, typeof(RichTextGraphic),typeof(UnityEngine.UI.MaskableGraphic));
	}
}
