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

public class AiSelectSkillByDistance : ISimpleStoryApiPlugin
{
    public void Init(DslCalculator calculator)
    {
        m_Calculator = calculator;
    }

    public bool OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
        int objId = args[0];
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            int targetId = npc.GetAiStateInfo().Target;
            if (targetId > 0) {
                EntityInfo target = PluginFramework.Instance.GetEntityById(targetId);
                if (null != target) {
                    float distToTarget = Geometry.Distance(npc.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D());
                    SkillInfo maxSkillInfo = null;
                    float diffDist = float.MaxValue;
                    SkillInfo targetSkillInfo = null;
                    for (int i = 0; i < npc.AutoSkillIds.Count; ++i) {
                        int skillId = npc.AutoSkillIds[i];
                        SkillInfo temp = npc.GetSkillStateInfo().GetSkillInfoById(skillId);
                        if (null != temp) {
                            float dist = temp.Distance;
                            float absDist = Mathf.Abs(distToTarget - dist);
                            if (diffDist > absDist) {
                                diffDist = absDist;
                                targetSkillInfo = temp;
                            }
                        }
                    }
                    if (null != targetSkillInfo)
                        result.Value = BoxedValue.FromObject(targetSkillInfo);
                    else
                        result.Value = BoxedValue.FromObject(maxSkillInfo);
                    return false;
                }
            }
        }
        result.Value = BoxedValue.NullObject;
        return false;
    }

    private DslCalculator m_Calculator = null;
}
