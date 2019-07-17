using System;
using System.Collections;
using System.Collections.Generic;
using SLua;
using GameFramework;
using GameFramework.Plugin;

public class Cs2LuaSimpleStoryCommandPlugin : LuaClassProxyBase
{
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
	public void ResetState()
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_ResetState);
		LuaFunctionHelper.PushValue(Self);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			LuaFunctionHelper.EndGetResult();
		} else {
		}
	}
	public bool ExecCommand(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, StorySystem.StoryValueParams _params, System.Int64 delta)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_ExecCommand);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(instance);
		LuaFunctionHelper.PushValue(handler);
		LuaFunctionHelper.PushValue(_params);
		LuaFunctionHelper.PushValue(delta);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			bool __cs2lua_ret;
			LuaFunctionHelper.GetResult(out __cs2lua_ret);
			LuaFunctionHelper.EndGetResult();
			return __cs2lua_ret;
		} else {
			return false;
		}
	}

	protected override void PrepareMembers()
	{
		m_Cs2Lua_Clone = (LuaFunction)Self["Clone"];
		m_Cs2Lua_ResetState = (LuaFunction)Self["ResetState"];
		m_Cs2Lua_ExecCommand = (LuaFunction)Self["ExecCommand"];
	}

	private LuaFunction m_Cs2Lua_Clone;
	private LuaFunction m_Cs2Lua_ResetState;
	private LuaFunction m_Cs2Lua_ExecCommand;
}
