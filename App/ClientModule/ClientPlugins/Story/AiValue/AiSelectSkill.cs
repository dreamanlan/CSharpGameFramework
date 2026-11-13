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

public class AiSelectSkill : ISimpleStoryApiPlugin
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
            SkillInfo skillInfo = AiLogicUtility.NpcFindCanUseSkill(npc);
            result.Value = BoxedValue.FromObject(skillInfo);
        }
        return false;
    }

    private DslCalculatorCodeStructure m_CodeStructure = null;
}
