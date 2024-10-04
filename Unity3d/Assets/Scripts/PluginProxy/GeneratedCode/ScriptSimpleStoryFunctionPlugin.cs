using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Plugin;

public class ScriptSimpleStoryFunctionPlugin : ScriptPluginProxyBase
{
	public void SetProxy(DotnetStoryScript.StoryFunctionResult result)
	{
	}
	public object Clone()
	{
		return null;
	}
	public void Evaluate(DotnetStoryScript.StoryInstance instance, DotnetStoryScript.StoryMessageHandler handler, DotnetStoryScript.StoryFunctionParams _params)
	{
	}

	protected override void PrepareMembers()
	{
	}

	//private ScriptFunction m_SetProxy;
	//private ScriptFunction m_Clone;
	//private ScriptFunction m_Evaluate;
}
