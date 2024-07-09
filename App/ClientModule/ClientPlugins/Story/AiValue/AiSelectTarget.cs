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

public class AiSelectTarget : ISimpleStoryFunctionPlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryFunctionPlugin Clone()
    {
        return new AiSelectTarget();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params)
    {
        var args = _params.Values;
        int objId = args[0].GetInt();
        float dist = args[1].GetFloat();
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            EntityInfo entity;
            if (dist < Geometry.c_FloatPrecision) {
                entity = AiLogicUtility.GetNearstTargetHelper(npc, CharacterRelation.RELATION_ENEMY);
                if (null != entity) {
                    npc.GetAiStateInfo().Target = entity.GetId();
                }
            } else {
                entity = AiLogicUtility.GetNearstTargetHelper(npc, dist, CharacterRelation.RELATION_ENEMY);
                if (null != entity) {
                    npc.GetAiStateInfo().Target = entity.GetId();
                }
            }
            m_Proxy.Value = BoxedValue.FromObject(entity);
        }
    }

    private StoryValueResult m_Proxy = null;
}
