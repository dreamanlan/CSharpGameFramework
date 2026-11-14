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

public class AiGetTarget : ISimpleStoryApiPlugin
{
    public void Init(DslCalculatorCodeStructure code)
    {
        m_CodeStructure = code;
    }

    public bool OnCalc(DslCalculator calculator, IList<BoxedValue> operands, AsyncCalcResult result)
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

    private DslCalculatorCodeStructure m_CodeStructure = null;
}
