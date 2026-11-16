using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework;
using ScriptableFramework.Skill;
using SkillSystem;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

public class AiSelectTarget : ISimpleStoryApiPlugin
{
    public void Init(DslCalculatorCodeStructure code)
    {
        m_CodeStructure = code;
    }

    public bool OnCalc(DslCalculator calculator, IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
        int objId = args[0].GetInt();
        float dist = args[1].GetFloat();
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            EntityInfo entity = dist < Geometry.c_FloatPrecision
                ? AiLogicUtility.GetNearstTargetHelper(npc, CharacterRelation.RELATION_ENEMY)
                : AiLogicUtility.GetNearstTargetHelper(npc, dist, CharacterRelation.RELATION_ENEMY);
            if (null != entity) {
                npc.GetAiStateInfo().Target = entity.GetId();
                // Drive the visible lock-on reticle from the party leader's
                // current attack target. Without this, a target was only ever
                // locked when the leader got hit or the player clicked manually,
                // so the player could not tell who was being fought.
                if (npc.GetId() == PluginFramework.Instance.LeaderId) {
                    AiCommand.AiTarget(npc, entity);
                    // Face the current opponent: without this the leader keeps
                    // whatever heading NavMeshAgent gave it while running.
                    float dir = Geometry.GetYRadian(
                        npc.GetMovementStateInfo().GetPosition3D(),
                        entity.GetMovementStateInfo().GetPosition3D());
                    npc.GetMovementStateInfo().SetWantedFaceDir(dir);
                }
            }
            result.Value = BoxedValue.FromObject(entity);
        }
        return false;
    }

    private DslCalculatorCodeStructure m_CodeStructure = null;
}
