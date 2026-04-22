using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

internal class ScriptSimpleStoryApi : SimpleAsyncExpressionBase
{
    protected override IEnumerator OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
    {
        while(m_Plugin.OnCalc(operands, result)) {
            yield return result.Value;
        }
    }
    protected override bool Load(IList<IExpression> exps)
    {
        InitPlugin();
        return base.Load(exps);
    }

    internal ScriptSimpleStoryApi(string className)
    {
        m_ClassName = className;
        m_FileName = m_ClassName.Replace(".", "__");
        m_Plugin = new ScriptSimpleStoryApiPlugin();
        m_Plugin.LoadScript(m_FileName);
    }

    private void InitPlugin()
    {
        if (!m_Inited) {
            m_Inited = true;
            m_Plugin.Init(Calculator);
        }
    }

    private string m_ClassName;
    private string m_FileName;
    private ScriptSimpleStoryApiPlugin m_Plugin;
    private bool m_Inited = false;
}