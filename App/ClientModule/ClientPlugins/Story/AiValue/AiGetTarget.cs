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

public class AiGetTarget : ISimpleStoryFunctionPlugin
{
    public void SetProxy(StoryFunctionResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryFunctionPlugin Clone()
    {
        return new AiGetTarget();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryFunctionParams _params)
    {
        var args = _params.Values;
        int objId = args[0];
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            int targetId = npc.GetAiStateInfo().Target;
            if (targetId > 0) {
                EntityInfo entity = PluginFramework.Instance.GetEntityById(targetId);
                if (null != entity && !entity.IsDead()) {
                    m_Proxy.Value = BoxedValue.FromObject(entity);
                } else {
                    m_Proxy.Value = BoxedValue.NullObject;
                }
            } else {
                m_Proxy.Value = BoxedValue.NullObject;
            }
        }
    }

    private StoryFunctionResult m_Proxy = null;
}
