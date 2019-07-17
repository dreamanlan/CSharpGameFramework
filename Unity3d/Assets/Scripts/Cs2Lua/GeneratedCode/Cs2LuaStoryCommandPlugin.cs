using System;
using System.Collections;
using System.Collections.Generic;
using SLua;
using GameFramework;
using GameFramework.Plugin;

public class Cs2LuaStoryCommandPlugin : LuaClassProxyBase
{
	public bool IsCompositeCommand()
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_IsCompositeCommand);
		LuaFunctionHelper.PushValue(Self);
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
	public void Evaluate(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, System.Object iterator, params object[] args)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_Evaluate);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(instance);
		LuaFunctionHelper.PushValue(handler);
		LuaFunctionHelper.PushValue(iterator);
		LuaFunctionHelper.PushParams(args);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			LuaFunctionHelper.EndGetResult();
		} else {
		}
	}
	public bool ExecCommand(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, System.Int64 delta)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_ExecCommand);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(instance);
		LuaFunctionHelper.PushValue(handler);
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
	public bool ExecCommandWithArgs(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, System.Int64 delta, System.Object iterator, params object[] args)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_ExecCommandWithArgs);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(instance);
		LuaFunctionHelper.PushValue(handler);
		LuaFunctionHelper.PushValue(delta);
		LuaFunctionHelper.PushValue(iterator);
		LuaFunctionHelper.PushParams(args);
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
	public void LoadCallData(Dsl.CallData callData)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_LoadCallData);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(callData);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			LuaFunctionHelper.EndGetResult();
		} else {
		}
	}
	public void LoadFuncData(Dsl.FunctionData funcData)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_LoadFuncData);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(funcData);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			LuaFunctionHelper.EndGetResult();
		} else {
		}
	}
	public void LoadStatementData(Dsl.StatementData statementData)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_LoadStatementData);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(statementData);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			LuaFunctionHelper.EndGetResult();
		} else {
		}
	}

	protected override void PrepareMembers()
	{
		m_Cs2Lua_IsCompositeCommand = (LuaFunction)Self["IsCompositeCommand"];
		m_Cs2Lua_Clone = (LuaFunction)Self["Clone"];
		m_Cs2Lua_ResetState = (LuaFunction)Self["ResetState"];
		m_Cs2Lua_Evaluate = (LuaFunction)Self["Evaluate"];
		m_Cs2Lua_ExecCommand = (LuaFunction)Self["ExecCommand"];
		m_Cs2Lua_ExecCommandWithArgs = (LuaFunction)Self["ExecCommandWithArgs"];
		m_Cs2Lua_LoadCallData = (LuaFunction)Self["LoadCallData"];
		m_Cs2Lua_LoadFuncData = (LuaFunction)Self["LoadFuncData"];
		m_Cs2Lua_LoadStatementData = (LuaFunction)Self["LoadStatementData"];
	}

	private LuaFunction m_Cs2Lua_IsCompositeCommand;
	private LuaFunction m_Cs2Lua_Clone;
	private LuaFunction m_Cs2Lua_ResetState;
	private LuaFunction m_Cs2Lua_Evaluate;
	private LuaFunction m_Cs2Lua_ExecCommand;
	private LuaFunction m_Cs2Lua_ExecCommandWithArgs;
	private LuaFunction m_Cs2Lua_LoadCallData;
	private LuaFunction m_Cs2Lua_LoadFuncData;
	private LuaFunction m_Cs2Lua_LoadStatementData;
}
