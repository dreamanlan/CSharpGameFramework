using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using SkillSystem;
using StorySystem;

public class AiGetEntities : ISimpleStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryValuePlugin Clone()
    {
        return new AiGetEntities();
    }
    public void Evaluate(StoryInstance instance, StoryValueParams _params)
    {
        m_Proxy.Value = PluginFramework.Instance.EntityManager.Entities.Values;
    }

    private StoryValueResult m_Proxy = null;
}
