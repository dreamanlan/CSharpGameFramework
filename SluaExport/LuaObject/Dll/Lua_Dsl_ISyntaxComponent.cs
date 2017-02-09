using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_Dsl_ISyntaxComponent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsValid(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			var ret=self.IsValid();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetId(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			var ret=self.GetId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetIdType(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			var ret=self.GetIdType();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLine(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			var ret=self.GetLine();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ToScriptString(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			System.Boolean a1;
			checkType(l,2,out a1);
			var ret=self.ToScriptString(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyComments(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			self.CopyComments(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyFirstComments(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			self.CopyFirstComments(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CopyLastComments(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			Dsl.ISyntaxComponent a1;
			checkType(l,2,out a1);
			self.CopyLastComments(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcFirstComment(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			var ret=self.CalcFirstComment();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CalcLastComment(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			var ret=self.CalcLastComment();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_FirstComments(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FirstComments);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_FirstCommentOnNewLine(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FirstCommentOnNewLine);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_FirstCommentOnNewLine(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.FirstCommentOnNewLine=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LastComments(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LastComments);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_LastCommentOnNewLine(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LastCommentOnNewLine);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_LastCommentOnNewLine(IntPtr l) {
		try {
			Dsl.ISyntaxComponent self=(Dsl.ISyntaxComponent)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.LastCommentOnNewLine=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.ISyntaxComponent");
		addMember(l,IsValid);
		addMember(l,GetId);
		addMember(l,GetIdType);
		addMember(l,GetLine);
		addMember(l,ToScriptString);
		addMember(l,CopyComments);
		addMember(l,CopyFirstComments);
		addMember(l,CopyLastComments);
		addMember(l,CalcFirstComment);
		addMember(l,CalcLastComment);
		addMember(l,"FirstComments",get_FirstComments,null,true);
		addMember(l,"FirstCommentOnNewLine",get_FirstCommentOnNewLine,set_FirstCommentOnNewLine,true);
		addMember(l,"LastComments",get_LastComments,null,true);
		addMember(l,"LastCommentOnNewLine",get_LastCommentOnNewLine,set_LastCommentOnNewLine,true);
		createTypeMetatable(l,null, typeof(Dsl.ISyntaxComponent));
	}
}
