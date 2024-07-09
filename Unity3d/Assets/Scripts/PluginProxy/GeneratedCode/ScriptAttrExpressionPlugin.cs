using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Plugin;

public class ScriptAttrExpressionPlugin : ScriptPluginProxyBase, IAttrExpressionPlugin
{
	public void SetCalculator(ScriptableFramework.AttrCalc.DslCalculator calc)
	{
	}
	public System.Int64 Calc(ScriptableFramework.SceneContextInfo context, ScriptableFramework.CharacterProperty source, ScriptableFramework.CharacterProperty target, params System.Int64[] args)
	{
		return (System.Int64)0;
	}
	public bool LoadValue(Dsl.ValueData valData)
	{
		return false;
	}
	public bool LoadCallData(Dsl.FunctionData callData)
	{
		return false;
	}
	public bool LoadExpressions(ScriptableFramework.AttrCalc.AttrExpressionList exps)
	{
		return false;
	}
	public bool LoadFuncData(Dsl.FunctionData funcData)
	{
		return false;
	}
	public bool LoadStatementData(Dsl.StatementData statementData)
	{
		return false;
	}

	protected override void PrepareMembers()
	{
	}

	//private ScriptFunction m_SetCalculator;
	//private ScriptFunction m_Calc;
	//private ScriptFunction m_LoadValue;
	//private ScriptFunction m_LoadCallData;
	//private ScriptFunction m_LoadExpressions;
	//private ScriptFunction m_LoadFuncData;
	//private ScriptFunction m_LoadStatementData;
}
