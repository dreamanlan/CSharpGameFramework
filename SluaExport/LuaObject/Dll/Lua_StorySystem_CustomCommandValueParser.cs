using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_StorySystem_CustomCommandValueParser : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadStory_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=StorySystem.CustomCommandValueParser.LoadStory(a1);
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
	static public int LoadStoryText_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Byte[] a2;
			checkArray(l,2,out a2);
			var ret=StorySystem.CustomCommandValueParser.LoadStoryText(a1,a2);
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
	static public int FirstParse__A_DslFile_s(IntPtr l) {
		try {
			Dsl.DslFile[] a1;
			checkParams(l,1,out a1);
			StorySystem.CustomCommandValueParser.FirstParse(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FirstParse__ISyntaxComponent_s(IntPtr l) {
		try {
			Dsl.ISyntaxComponent a1;
			checkType(l,1,out a1);
			StorySystem.CustomCommandValueParser.FirstParse(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FinalParse__A_DslFile_s(IntPtr l) {
		try {
			Dsl.DslFile[] a1;
			checkParams(l,1,out a1);
			StorySystem.CustomCommandValueParser.FinalParse(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FinalParse__ISyntaxComponent_s(IntPtr l) {
		try {
			Dsl.ISyntaxComponent a1;
			checkType(l,1,out a1);
			StorySystem.CustomCommandValueParser.FinalParse(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.CustomCommandValueParser");
		addMember(l,LoadStory_s);
		addMember(l,LoadStoryText_s);
		addMember(l,FirstParse__A_DslFile_s);
		addMember(l,FirstParse__ISyntaxComponent_s);
		addMember(l,FinalParse__A_DslFile_s);
		addMember(l,FinalParse__ISyntaxComponent_s);
		createTypeMetatable(l,null, typeof(StorySystem.CustomCommandValueParser));
	}
}
