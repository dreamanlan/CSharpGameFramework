using System;
using System.Collections;
using System.Collections.Generic;
using SLua;
using GameFramework;
using GameFramework.Plugin;

public class Cs2LuaStoryValuePlugin : LuaClassProxyBase
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
	public void LoadCallData(Dsl.FunctionData callData)
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
		m_Cs2Lua_SetProxy = (LuaFunction)Self["SetProxy"];
		m_Cs2Lua_Clone = (LuaFunction)Self["Clone"];
		m_Cs2Lua_Evaluate = (LuaFunction)Self["Evaluate"];
		m_Cs2Lua_LoadCallData = (LuaFunction)Self["LoadCallData"];
		m_Cs2Lua_LoadFuncData = (LuaFunction)Self["LoadFuncData"];
		m_Cs2Lua_LoadStatementData = (LuaFunction)Self["LoadStatementData"];
	}

	private LuaFunction m_Cs2Lua_SetProxy;
	private LuaFunction m_Cs2Lua_Clone;
	private LuaFunction m_Cs2Lua_Evaluate;
	private LuaFunction m_Cs2Lua_LoadCallData;
	private LuaFunction m_Cs2Lua_LoadFuncData;
	private LuaFunction m_Cs2Lua_LoadStatementData;
}
