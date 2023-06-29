using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Plugin;

public class ScriptSimpleStoryCommandPlugin : ScriptPluginProxyBase
{
	public object Clone()
	{
		return null;
	}
	public void ResetState()
	{
	}
	public bool ExecCommand(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, StorySystem.StoryValueParams _params, System.Int64 delta)
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
