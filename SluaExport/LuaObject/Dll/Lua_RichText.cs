using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_RichText : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DoRichTextClick(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			self.DoRichTextClick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int OnPointerClick(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			self.OnPointerClick(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetVerticesDirty(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			self.SetVerticesDirty();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetLinkColor_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			RichText.SetLinkColor(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onLoadResource(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			RichText.LoadResourceDelegate v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onLoadResource=v;
			else if(op==1) self.onLoadResource+=v;
			else if(op==2) self.onLoadResource-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onBuildLinkOrQuadInfo(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			RichText.BuildLinkOrGuadInfoDelegation v;
			int op=LuaDelegation.checkDelegate(l,2,out v);
			if(op==0) self.onBuildLinkOrQuadInfo=v;
			else if(op==1) self.onBuildLinkOrQuadInfo+=v;
			else if(op==2) self.onBuildLinkOrQuadInfo-=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_text(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
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
			RichText self=(RichText)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.text=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_inputText(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.inputText);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_HaveError(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.HaveError);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_parsedTexts(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.parsedTexts);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_parsedTexts(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			RichTextParser.IRichTextList v;
			checkType(l,2,out v);
			self.parsedTexts=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_onHrefClick(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.onHrefClick);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_onHrefClick(IntPtr l) {
		try {
			RichText self=(RichText)checkSelf(l);
			RichText.HrefClickEvent v;
			checkType(l,2,out v);
			self.onHrefClick=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_defaultQuadSize(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,RichText.defaultQuadSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_defaultQuadSize(IntPtr l) {
		try {
			int v;
			checkType(l,2,out v);
			RichText.defaultQuadSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"RichText");
		addMember(l,DoRichTextClick);
		addMember(l,OnPointerClick);
		addMember(l,SetVerticesDirty);
		addMember(l,SetLinkColor_s);
		addMember(l,"onLoadResource",null,set_onLoadResource,true);
		addMember(l,"onBuildLinkOrQuadInfo",null,set_onBuildLinkOrQuadInfo,true);
		addMember(l,"text",get_text,set_text,true);
		addMember(l,"inputText",get_inputText,null,true);
		addMember(l,"HaveError",get_HaveError,null,true);
		addMember(l,"parsedTexts",get_parsedTexts,set_parsedTexts,true);
		addMember(l,"onHrefClick",get_onHrefClick,set_onHrefClick,true);
		addMember(l,"defaultQuadSize",get_defaultQuadSize,set_defaultQuadSize,false);
		createTypeMetatable(l,null, typeof(RichText),typeof(UnityEngine.UI.Text));
	}
}
