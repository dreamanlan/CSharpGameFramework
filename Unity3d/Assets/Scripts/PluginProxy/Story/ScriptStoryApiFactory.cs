using System;
using DotnetStoryScript.DslExpression;

internal class ScriptStoryApiFactory : IExpressionFactory
{
    public IExpression Create()
    {
        return new ScriptStoryApi(m_ClassName);
    }

    internal ScriptStoryApiFactory(string className)
    {
        m_ClassName = className;
    }

    private string m_ClassName;
}
