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
		base.CallFunction(m_Cs2Lua_SetCalculator, false, Self, calc);
	}
    public System.Int64 Calc(GameFramework.SceneContextInfo context, GameFramework.CharacterProperty source, GameFramework.CharacterProperty target, params System.Int64[] args)
	{
		var __cs2lua_ret = base.CallFunction(m_Cs2Lua_Calc, true, Self, context, source, target, args);
		return base.CastTo<System.Int64>(__cs2lua_ret);
	}
	public bool LoadValue(Dsl.ValueData valData)
	{
		var __cs2lua_ret = base.CallFunction(m_Cs2Lua_LoadValue, false, Self, valData);
		return base.CastTo<bool>(__cs2lua_ret);
	}
	public bool LoadCallData(Dsl.CallData callData)
	{
		var __cs2lua_ret = base.CallFunction(m_Cs2Lua_LoadCallData, false, Self, callData);
		return base.CastTo<bool>(__cs2lua_ret);
	}
    public bool LoadExpressions(GameFramework.AttrCalc.AttrExpressionList exps)
	{
		var __cs2lua_ret = base.CallFunction(m_Cs2Lua_LoadExpressions, false, Self, exps);
		return base.CastTo<bool>(__cs2lua_ret);
	}
	public bool LoadFuncData(Dsl.FunctionData funcData)
	{
		var __cs2lua_ret = base.CallFunction(m_Cs2Lua_LoadFuncData, false, Self, funcData);
		return base.CastTo<bool>(__cs2lua_ret);
	}
	public bool LoadStatementData(Dsl.StatementData statementData)
	{
		var __cs2lua_ret = base.CallFunction(m_Cs2Lua_LoadStatementData, false, Self, statementData);
		return base.CastTo<bool>(__cs2lua_ret);
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
