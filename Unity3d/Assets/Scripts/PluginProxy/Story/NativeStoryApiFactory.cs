using System;
using DotnetStoryScript.DslExpression;
using ScriptableFramework;

internal class NativeStoryApiFactory : IExpressionFactory
{
    public IExpression Create()
    {
        return new NativeStoryApi(m_ClassName);
    }

    internal NativeStoryApiFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}