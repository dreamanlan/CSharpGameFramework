using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_StorySystem_CustomCommandValueParser : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
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
	static public int LoadStoryText_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=StorySystem.CustomCommandValueParser.LoadStoryText(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadStoryCode_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=StorySystem.CustomCommandValueParser.LoadStoryCode(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FirstParse_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(Dsl.DslInfo))){
				Dsl.DslInfo a1;
				checkType(l,1,out a1);
				StorySystem.CustomCommandValueParser.FirstParse(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(Dsl.DslFile[]))){
				Dsl.DslFile[] a1;
				checkParams(l,1,out a1);
				StorySystem.CustomCommandValueParser.FirstParse(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FinalParse_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(Dsl.DslInfo))){
				Dsl.DslInfo a1;
				checkType(l,1,out a1);
				StorySystem.CustomCommandValueParser.FinalParse(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(Dsl.DslFile[]))){
				Dsl.DslFile[] a1;
				checkParams(l,1,out a1);
				StorySystem.CustomCommandValueParser.FinalParse(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"StorySystem.CustomCommandValueParser");
		addMember(l,LoadStory_s);
		addMember(l,LoadStoryText_s);
		addMember(l,LoadStoryCode_s);
		addMember(l,FirstParse_s);
		addMember(l,FinalParse_s);
		createTypeMetatable(l,null, typeof(StorySystem.CustomCommandValueParser));
	}
}
