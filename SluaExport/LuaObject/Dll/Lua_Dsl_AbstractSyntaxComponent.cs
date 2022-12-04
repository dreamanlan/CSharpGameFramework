using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Dsl_AbstractSyntaxComponent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsValid(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetId(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetIdType(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int GetLine(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int ToScriptString(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int HaveId(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
			var ret=self.HaveId();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CalcFirstComment(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int CalcLastComment(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int CopyComments(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int CopyFirstComments(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int CopyLastComments(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_ID_TOKEN(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.AbstractSyntaxComponent.ID_TOKEN);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NUM_TOKEN(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.AbstractSyntaxComponent.NUM_TOKEN);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_STRING_TOKEN(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.AbstractSyntaxComponent.STRING_TOKEN);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_MAX_TYPE(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,Dsl.AbstractSyntaxComponent.MAX_TYPE);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_FirstCommentOnNewLine(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FirstCommentOnNewLine);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_FirstCommentOnNewLine(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public int get_LastCommentOnNewLine(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LastCommentOnNewLine);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LastCommentOnNewLine(IntPtr l) {
		try {
			Dsl.AbstractSyntaxComponent self=(Dsl.AbstractSyntaxComponent)checkSelf(l);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Dsl.AbstractSyntaxComponent");
		addMember(l,IsValid);
		addMember(l,GetId);
		addMember(l,GetIdType);
		addMember(l,GetLine);
		addMember(l,ToScriptString);
		addMember(l,HaveId);
		addMember(l,CalcFirstComment);
		addMember(l,CalcLastComment);
		addMember(l,CopyComments);
		addMember(l,CopyFirstComments);
		addMember(l,CopyLastComments);
		addMember(l,"ID_TOKEN",get_ID_TOKEN,null,false);
		addMember(l,"NUM_TOKEN",get_NUM_TOKEN,null,false);
		addMember(l,"STRING_TOKEN",get_STRING_TOKEN,null,false);
		addMember(l,"MAX_TYPE",get_MAX_TYPE,null,false);
		addMember(l,"FirstCommentOnNewLine",get_FirstCommentOnNewLine,set_FirstCommentOnNewLine,true);
		addMember(l,"LastCommentOnNewLine",get_LastCommentOnNewLine,set_LastCommentOnNewLine,true);
		createTypeMetatable(l,null, typeof(Dsl.AbstractSyntaxComponent));
	}
}
