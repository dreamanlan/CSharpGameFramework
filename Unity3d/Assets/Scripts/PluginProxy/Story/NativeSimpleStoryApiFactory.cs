using System;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

internal class NativeSimpleStoryApiFactory : IExpressionFactory
{
    public IExpression Create()
    {
        return new NativeSimpleStoryApi(m_ClassName);
    }

    internal NativeSimpleStoryApiFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}