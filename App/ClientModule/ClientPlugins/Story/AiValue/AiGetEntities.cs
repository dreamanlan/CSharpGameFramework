using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework.Plugin;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript;

public class AiGetEntities : ISimpleStoryFunctionPlugin
{
    public void SetProxy(StoryFunctionResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryFunctionPlugin Clone()
    {
        return new AiGetEntities();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryFunctionParams _params)
    {
        m_Proxy.Value = BoxedValue.FromObject(PluginFramework.Instance.EntityManager.Entities.Values);
    }

    private StoryFunctionResult m_Proxy = null;
}
