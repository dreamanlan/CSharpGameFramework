using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_StoryListenFlagUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FromString_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=GameFramework.StoryListenFlagUtility.FromString(a1);
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ToString_s(IntPtr l) {
		try {
			GameFramework.StoryListenFlagEnum a1;
			checkEnum(l,1,out a1);
			var ret=GameFramework.StoryListenFlagUtility.ToString(a1);
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
	static public int get_c_story_bit_prefix(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,GameFramework.StoryListenFlagUtility.c_story_bit_prefix);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.StoryListenFlagUtility");
		addMember(l,FromString_s);
		addMember(l,ToString_s);
		addMember(l,"c_story_bit_prefix",get_c_story_bit_prefix,null,false);
		createTypeMetatable(l,null, typeof(GameFramework.StoryListenFlagUtility));
	}
}
