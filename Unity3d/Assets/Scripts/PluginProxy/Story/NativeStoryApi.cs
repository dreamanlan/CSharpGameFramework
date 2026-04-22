using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using DotnetStoryScript.DslExpression;

internal class NativeStoryApi : AbstractExpression
{
    public IStoryApiPlugin Plugin
    {
        get { return m_Plugin; }
    }

    public override bool IsAsync => true;

    protected override bool Load(Dsl.FunctionData funcData)
    {
        InitPlugin();
        m_Plugin.LoadFuncData(funcData);
        return true;
    }
    protected override bool Load(Dsl.StatementData statementData)
    {
        InitPlugin();
        m_Plugin.LoadStatementData(statementData);
        return true;
    }
    protected override IEnumerator DoCalc(AsyncCalcResult result)
    {
        while (m_Plugin.Calc(m_Result)) {
            yield return m_Result.Value;
        }
    }

    internal NativeStoryApi(string name)
    {
        var module = PluginManager.Instance.CreateObject(name);
        m_Plugin = module as IStoryApiPlugin;
    }

    private void InitPlugin()
    {
        if (!m_Inited) {
            m_Inited = true;
            m_Plugin.Init(Calculator);
        }
    }

    private IStoryApiPlugin m_Plugin;
    private AsyncCalcResult m_Result = new AsyncCalcResult();
    private bool m_Inited = false;
}