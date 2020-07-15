using System;
using System.Collections;
using System.Collections.Generic;
using SLua;
using GameFramework;
using GameFramework.Plugin;

public class Cs2LuaAttrExpressionPlugin : LuaClassProxyBase, IAttrExpressionPlugin
{
	public void SetCalculator(GameFramework.AttrCalc.DslCalculator calc)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_SetCalculator);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(calc);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			LuaFunctionHelper.EndGetResult();
		} else {
		}
	}
	public System.Int64 Calc(GameFramework.SceneContextInfo context, GameFramework.CharacterProperty source, GameFramework.CharacterProperty target, params System.Int64[] args)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_Calc);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(context);
		LuaFunctionHelper.PushValue(source);
		LuaFunctionHelper.PushValue(target);
		LuaFunctionHelper.PushParams(args);
		var end_call_res = LuaFunctionHelper.EndCall(err);
		if (end_call_res) {
			LuaFunctionHelper.BeginGetResult(err);
			System.Int64 __cs2lua_ret;
			LuaFunctionHelper.GetResult(out __cs2lua_ret);
			LuaFunctionHelper.EndGetResult();
			return __cs2lua_ret;
		} else {
			return (System.Int64)0;
		}
	}
	public bool LoadValue(Dsl.ValueData valData)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_LoadValue);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(valData);
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
	public bool LoadCallData(Dsl.FunctionData callData)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_LoadCallData);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(callData);
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
	public bool LoadExpressions(GameFramework.AttrCalc.AttrExpressionList exps)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_LoadExpressions);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(exps);
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
	public bool LoadFuncData(Dsl.FunctionData funcData)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_LoadFuncData);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(funcData);
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
	public bool LoadStatementData(Dsl.StatementData statementData)
	{
		var err = LuaFunctionHelper.BeginCall(m_Cs2Lua_LoadStatementData);
		LuaFunctionHelper.PushValue(Self);
		LuaFunctionHelper.PushValue(statementData);
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
		m_Cs2Lua_SetCalculator = (LuaFunction)Self["SetCalculator"];
		m_Cs2Lua_Calc = (LuaFunction)Self["Calc"];
		m_Cs2Lua_LoadValue = (LuaFunction)Self["LoadValue"];
		m_Cs2Lua_LoadCallData = (LuaFunction)Self["LoadCallData"];
		m_Cs2Lua_LoadExpressions = (LuaFunction)Self["LoadExpressions"];
		m_Cs2Lua_LoadFuncData = (LuaFunction)Self["LoadFuncData"];
		m_Cs2Lua_LoadStatementData = (LuaFunction)Self["LoadStatementData"];
	}

	private LuaFunction m_Cs2Lua_SetCalculator;
	private LuaFunction m_Cs2Lua_Calc;
	private LuaFunction m_Cs2Lua_LoadValue;
	private LuaFunction m_Cs2Lua_LoadCallData;
	private LuaFunction m_Cs2Lua_LoadExpressions;
	private LuaFunction m_Cs2Lua_LoadFuncData;
	private LuaFunction m_Cs2Lua_LoadStatementData;
}
