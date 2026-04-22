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

public class AiNeedKeepAway : ISimpleStoryApiPlugin
{
    public void Init(DslCalculator calculator)
    {
        m_Calculator = calculator;
    }

    public bool OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
        int objId = args[0].GetInt();
        SkillInfo skillInfo = args[1].ObjectVal as SkillInfo;
        float ratio = args[2].GetFloat();
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc && null != skillInfo) {
            int targetId = npc.GetAiStateInfo().Target;
            if (targetId > 0) {
                EntityInfo target = PluginFramework.Instance.GetEntityById(targetId);
                if (null != target) {
                    float distSqr = Geometry.DistanceSquare(npc.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D());
                    if (distSqr < ratio * ratio * skillInfo.Distance * skillInfo.Distance) {
                        result.Value = 1;
                        return false;
                    }
                }
            }
        }
        result.Value = 0;
        return false;
    }

    private DslCalculator m_Calculator = null;
}
