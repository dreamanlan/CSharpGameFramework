using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;
using DotnetStoryScript.DslExpression;

internal class ScriptStoryApi : AbstractExpression
{
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
    protected override BoxedValue DoCalc()
    {
        return m_Plugin.Calc();
    }

    internal ScriptStoryApi(string className)
    {
        m_ClassName = className;
        m_FileName = m_ClassName.Replace(".", "__");
        m_Plugin = new ScriptStoryApiPlugin();
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
    private ScriptStoryApiPlugin m_Plugin;
    private bool m_Inited = false;
}