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

public class AiGetSkills : ISimpleStoryFunctionPlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryFunctionPlugin Clone()
    {
        return new AiGetSkills();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params)
    {
        var args = _params.Values;
        int objId = args[0].GetInt();
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            m_Proxy.Value = BoxedValue.FromObject(npc.GetSkillStateInfo().GetAllSkill());
        }
    }

    private StoryValueResult m_Proxy = null;
}
