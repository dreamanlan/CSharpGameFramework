using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Plugin;

public class ScriptStoryCommandPlugin : ScriptPluginProxyBase
{
	public bool IsCompositeCommand()
	{
		return false;
	}
	public object Clone()
	{
		return null;
	}
	public void ResetState()
	{
	}
	public void Evaluate(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, System.Object iterator, params object[] args)
	{
	}
	public bool ExecCommand(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, System.Int64 delta)
	{
		return false;
	}
	public bool ExecCommandWithArgs(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, System.Int64 delta, System.Object iterator, params object[] args)
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

	//private ScriptFunction m_IsCompositeCommand;
	//private ScriptFunction m_Clone;
	//private ScriptFunction m_ResetState;
	//private ScriptFunction m_Evaluate;
	//private ScriptFunction m_ExecCommand;
	//private ScriptFunction m_ExecCommandWithArgs;
	//private ScriptFunction m_LoadCallData;
	//private ScriptFunction m_LoadFuncData;
	//private ScriptFunction m_LoadStatementData;
}
