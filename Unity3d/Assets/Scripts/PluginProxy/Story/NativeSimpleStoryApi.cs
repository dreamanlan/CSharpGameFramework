using System;
using System.Collections;
using System.Collections.Generic;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

internal class NativeSimpleStoryApi : SimpleAsyncExpressionBase
{
    public ISimpleStoryApiPlugin Plugin
    {
        get { return m_Plugin; }
    }

    protected override IEnumerator OnCalc(IList<BoxedValue> operands, AsyncCalcResult result)
    {
        while (m_Plugin.OnCalc(operands, result)) {
            yield return result.Value;
        }
    }
    protected override bool Load(IList<IExpression> exps)
    {
        InitPlugin();
        return base.Load(exps);
    }

    internal NativeSimpleStoryApi(string name)
    {
        var module = PluginManager.Instance.CreateObject(name);
        m_Plugin = module as ISimpleStoryApiPlugin;
    }

    private void InitPlugin()
    {
        if (!m_Inited) {
            m_Inited = true;
            m_Plugin.Init(Calculator);
        }
    }

    private ISimpleStoryApiPlugin m_Plugin;
    private bool m_Inited = false;
}