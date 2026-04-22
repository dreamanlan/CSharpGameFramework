using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableFramework;

using DotnetStoryScript.DslExpression;

public class ScriptStoryApiPlugin : ScriptPluginProxyBase
{
    public void Init(DslCalculator calculator)
    {
    }
    public BoxedValue Calc()
    {
        return BoxedValue.NullObject;
    }
    public void LoadFuncData(Dsl.FunctionData funcData)
    {
    }
    public void LoadStatementData(Dsl.StatementData statementData)
    {
    }

    protected override void PrepareMembers()
    {
    }
}
