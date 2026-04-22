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

public class AiGetSkills : ISimpleStoryApiPlugin
{
    public void Init(DslCalculator calculator)
    {
        m_Calculator = calculator;
    }

    public bool OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
    {
        var args = operands;
        int objId = args[0].GetInt();
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            result.Value = BoxedValue.FromObject(npc.GetSkillStateInfo().GetAllSkill());
        }
        return false;
    }

    private DslCalculator m_Calculator = null;
}
