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

public class AiGetSkills : ISimpleStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryValuePlugin Clone()
    {
        return new AiGetSkills();
    }
    public void Evaluate(StoryInstance instance, StoryValueParams _params)
    {
        Scene scene = instance.Context as Scene;
        if (null != scene) {
            ArrayList args = _params.Values;
            int objId = (int)args[0];
            EntityInfo npc = scene.GetEntityById(objId);
            if (null != npc) {
                m_Proxy.Value = npc.GetSkillStateInfo().GetAllSkill();
            }
        }
    }

    private StoryValueResult m_Proxy = null;
}
