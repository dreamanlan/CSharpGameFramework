using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Plugin;

public class ScriptStoryValuePlugin : ScriptPluginProxyBase
{
	public void SetProxy(DotnetStoryScript.StoryValueResult result)
	{
	}
	public object Clone()
	{
		return null;
	}
	public void Evaluate(DotnetStoryScript.StoryInstance instance, DotnetStoryScript.StoryMessageHandler handler, System.Object iterator, params object[] args)
	{
	}
	public void LoadCallData(Dsl.FunctionData callData)
	{
	}
	public void LoadFuncData(Dsl.FunctionData funcData)
	{
	}
	public void LoadStatementData(Dsl.StatementData statementData)
	{
	}

	protected override void PrepareMembers()
	{
	}

	//private ScriptFunction m_SetProxy;
	//private ScriptFunction m_Clone;
	//private ScriptFunction m_Evaluate;
	//private ScriptFunction m_LoadCallData;
	//private ScriptFunction m_LoadFuncData;
	//private ScriptFunction m_LoadStatementData;
}
