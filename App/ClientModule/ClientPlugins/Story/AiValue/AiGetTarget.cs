﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using SkillSystem;
using StorySystem;

public class AiGetTarget : ISimpleStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryValuePlugin Clone()
    {
        return new AiGetTarget();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params)
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

    private StoryValueResult m_Proxy = null;
}