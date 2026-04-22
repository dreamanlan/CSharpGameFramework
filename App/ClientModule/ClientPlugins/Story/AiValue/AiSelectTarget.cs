using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript.DslExpression;

public class AiSelectTarget : ISimpleStoryApiPlugin
{
    public void Init(DslCalculator calculator)
    {
        m_Calculator = calculator;
    }

    public bool OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
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
            result.Value = BoxedValue.FromObject(entity);
        }
        return false;
    }

    private DslCalculator m_Calculator = null;
}
