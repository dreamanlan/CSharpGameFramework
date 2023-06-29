using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Plugin;

public class ScriptSimpleStoryValuePlugin : ScriptPluginProxyBase
{
	public void SetProxy(StorySystem.StoryValueResult result)
	{
	}
	public object Clone()
	{
		return null;
	}
	public void Evaluate(StorySystem.StoryInstance instance, StorySystem.StoryMessageHandler handler, StorySystem.StoryValueParams _params)
	{
	}

	protected override void PrepareMembers()
	{
	}

	//private ScriptFunction m_SetProxy;
	//private ScriptFunction m_Clone;
	//private ScriptFunction m_Evaluate;
}
