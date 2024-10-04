using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Plugin;

public class ScriptSimpleStoryCommandPlugin : ScriptPluginProxyBase
{
	public object Clone()
	{
		return null;
	}
	public void ResetState()
	{
	}
	public bool ExecCommand(DotnetStoryScript.StoryInstance instance, DotnetStoryScript.StoryMessageHandler handler, DotnetStoryScript.StoryFunctionParams _params, System.Int64 delta)
	{
		return false;
	}

	protected override void PrepareMembers()
	{
	}

	//private ScriptFunction m_Clone;
	//private ScriptFunction m_ResetState;
	//private ScriptFunction m_ExecCommand;
}
