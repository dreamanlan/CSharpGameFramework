using System;
using System.Collections;
using System.Collections.Generic;
using SLua;
using GameFramework;
using GameFramework.Plugin;

public class Cs2LuaSimpleStoryValuePlugin : LuaClassProxyBase
{
	public void SetProxy(StorySystem.StoryValueResult result)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_SetProxy);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(result);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			LuaFunctionHelper.EndGetResult();
		} else {
		}
	}
	public object Clone()
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_Clone);
		LuaFunctionHelper.PushValue(Self);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			object __cs2lua_ret;
			LuaFunctionHelper.GetResult(out __cs2lua_ret);
			LuaFunctionHelper.EndGetResult();
			return __cs2lua_ret;
		} else {
			return null;
		}
	}
	public void Evaluate(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, StorySystem.StoryValueParams _params)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_Evaluate);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(instance);
		LuaFunctionHelper.PushValue(handler);
		LuaFunctionHelper.PushValue(_params);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			LuaFunctionHelper.EndGetResult();
		} else {
		}
	}

	protected override void PrepareMembers()
	{
		m_Cs2Lua_SetProxy = (LuaFunction)Self["SetProxy"];
		m_Cs2Lua_Clone = (LuaFunction)Self["Clone"];
		m_Cs2Lua_Evaluate = (LuaFunction)Self["Evaluate"];
	}

	private LuaFunction m_Cs2Lua_SetProxy;
	private LuaFunction m_Cs2Lua_Clone;
	private LuaFunction m_Cs2Lua_Evaluate;
}
