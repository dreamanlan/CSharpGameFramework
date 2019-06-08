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

public class AiSelectTarget : ISimpleStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryValuePlugin Clone()
    {
        return new AiSelectTarget();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params)
    {
        ArrayList args = _params.Values;
        int objId = (int)args[0];
        float dist = (float)System.Convert.ChangeType(args[1], typeof(float));
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
            m_Proxy.Value = entity;
        }
    }

    private StoryValueResult m_Proxy = null;
}
