using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript;
using DotnetStoryScript.DslExpression;

public class AiNeedChase : ISimpleStoryApiPlugin
{
    public void Init(DslCalculatorCodeStructure code)
    {
        m_CodeStructure = code;
    }

    public bool OnCalc(DslCalculator calculator, IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
        int objId = args[0].GetInt();
        SkillInfo skillInfo = args[1].ObjectVal as SkillInfo;
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc && null != skillInfo) {
            int targetId = npc.GetAiStateInfo().Target;
            if (targetId > 0) {
                EntityInfo target = PluginFramework.Instance.GetEntityById(targetId);
                if (null != target) {
                    float distSqr = Geometry.DistanceSquare(npc.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D());
                    if (distSqr > skillInfo.Distance * skillInfo.Distance) {
                        result.Value = 1;
                        return false;
                    }
                }
            }
        }
        result.Value = 0;
        return false;
    }

    private DslCalculatorCodeStructure m_CodeStructure = null;
}
