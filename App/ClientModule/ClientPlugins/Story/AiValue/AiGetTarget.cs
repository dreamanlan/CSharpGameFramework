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

public class AiGetTarget : ISimpleStoryApiPlugin
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
                EntityInfo entity = PluginFramework.Instance.GetEntityById(targetId);
                if (null != entity && !entity.IsDead()) {
                    result.Value = BoxedValue.FromObject(entity);
                } else {
                    result.Value = BoxedValue.NullObject;
                }
            } else {
                result.Value = BoxedValue.NullObject;
            }
        }
        return false;
    }

    private DslCalculator m_Calculator = null;
}
