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

public class AiGetEntities : ISimpleStoryApiPlugin
{
    public void Init(DslCalculatorCodeStructure code)
    {
        m_CodeStructure = code;
    }

    public bool OnCalc(DslCalculator calculator, IList<BoxedValue> operands, AsyncCalcResult result)
    {
        result.Value = BoxedValue.FromObject(PluginFramework.Instance.EntityManager.Entities.Values);
        return false;
    }

    private DslCalculatorCodeStructure m_CodeStructure = null;
}
