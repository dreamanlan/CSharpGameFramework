using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_AiStoryInstanceInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.AiStoryInstanceInfo o;
			o=new GameFramework.AiStoryInstanceInfo();
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
	static public int Recycle(IntPtr l) {
		try {
			GameFramework.AiStoryInstanceInfo self=(GameFramework.AiStoryInstanceInfo)checkSelf(l);
			self.Recycle();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_StoryInstance(IntPtr l) {
		try {
			GameFramework.AiStoryInstanceInfo self=(GameFramework.AiStoryInstanceInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_StoryInstance);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_StoryInstance(IntPtr l) {
		try {
			GameFramework.AiStoryInstanceInfo self=(GameFramework.AiStoryInstanceInfo)checkSelf(l);
			StorySystem.StoryInstance v;
			checkType(l,2,out v);
			self.m_StoryInstance=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_m_IsUsed(IntPtr l) {
		try {
			GameFramework.AiStoryInstanceInfo self=(GameFramework.AiStoryInstanceInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.m_IsUsed);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_m_IsUsed(IntPtr l) {
		try {
			GameFramework.AiStoryInstanceInfo self=(GameFramework.AiStoryInstanceInfo)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.m_IsUsed=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.AiStoryInstanceInfo");
		addMember(l,ctor_s);
		addMember(l,Recycle);
		addMember(l,"m_StoryInstance",get_m_StoryInstance,set_m_StoryInstance,true);
		addMember(l,"m_IsUsed",get_m_IsUsed,set_m_IsUsed,true);
		createTypeMetatable(l,null, typeof(GameFramework.AiStoryInstanceInfo));
	}
}
