using System;
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
        Scene scene = instance.Context as Scene;
        if (null != scene) {
            m_Proxy.Value = scene.EntityManager.Entities.Values;
        } else {
            m_Proxy.Value = null;
        }
    }

    private StoryValueResult m_Proxy = null;
}
