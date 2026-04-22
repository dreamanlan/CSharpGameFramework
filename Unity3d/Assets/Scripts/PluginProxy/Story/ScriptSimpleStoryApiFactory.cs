using System;
using DotnetStoryScript.DslExpression;

internal class ScriptSimpleStoryApiFactory : IExpressionFactory
{
    public IExpression Create()
    {
        return new ScriptSimpleStoryApi(m_ClassName);
    }

    internal ScriptSimpleStoryApiFactory(string className)
    {
        m_ClassName = className;
    }

    private string m_ClassName;
}
