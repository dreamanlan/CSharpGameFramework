﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework.Plugin;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript;

public class AiSelectSkill : ISimpleStoryFunctionPlugin
{
    public void SetProxy(StoryFunctionResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryFunctionPlugin Clone()
    {
        return new AiSelectSkill();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryFunctionParams _params)
    {
        var args = _params.Values;
        int objId = args[0];
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            SkillInfo skillInfo = AiLogicUtility.NpcFindCanUseSkill(npc);
            m_Proxy.Value = BoxedValue.FromObject(skillInfo);
        }
    }

    private StoryFunctionResult m_Proxy = null;
}
