﻿using System;
using System.Collections.Generic;
using System.Text;
using ScriptableFramework.Plugin;
using ScriptableFramework.Story;
using DotnetStoryScript;

internal class NativeStoryFunctionFactory : IStoryFunctionFactory
{
    public IStoryFunction Build()
    {
        return new NativeStoryFunction(m_ClassName);
    }
    public NativeStoryFunctionFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class ScriptStoryFunctionFactory : IStoryFunctionFactory
{
    public IStoryFunction Build()
    {
        return new ScriptStoryFunction(m_ClassName);
    }
    public ScriptStoryFunctionFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class NativeStoryFunction : IStoryFunction
{
    public NativeStoryFunction(string name) : this(name, true)
    { }
    public NativeStoryFunction(string name, bool create)
    {
        m_ClassName = name;
        if (create) {
            var module = PluginManager.Instance.CreateObject(m_ClassName);
            m_Plugin = module as IStoryFunctionPlugin;
            if (null != m_Plugin) {
                m_Plugin.SetProxy(m_Proxy);
            }
        }
    }

    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
        Dsl.FunctionData funcData = param as Dsl.FunctionData;
        if (null != funcData) {
            Load(funcData);
        }
        else {
            Dsl.StatementData statementData = param as Dsl.StatementData;
            if (null != statementData) {
                //Is statement type command syntax supported?
                Load(statementData);
            }
            else {
                //error
            }
        }
    }
    public IStoryFunction Clone()
    {
        var newObj = new NativeStoryFunction(m_ClassName, false);
        newObj.m_Proxy = m_Proxy.Clone();
        if (null != m_Plugin) {
            newObj.m_Plugin = m_Plugin.Clone();
            if (null != newObj.m_Plugin) {
                newObj.m_Plugin.SetProxy(newObj.m_Proxy);
            }
        }
        return newObj;
    }    
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, ScriptableFramework.BoxedValue iterator, ScriptableFramework.BoxedValueList args)
    {
        if (null != m_Plugin) {
            m_Plugin.Evaluate(instance, handler, iterator, args);
        }
    }
    public bool HaveValue
    {
        get
        {
            return m_Proxy.HaveValue;
        }
    }
    public ScriptableFramework.BoxedValue Value
    {
        get
        {
            return m_Proxy.Value;
        }
    }

    private void Load(Dsl.FunctionData funcData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadFuncData(funcData);
        }
    }
    private void Load(Dsl.StatementData statementData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadStatementData(statementData);
        }
    }

    private string m_ClassName;
    private StoryFunctionResult m_Proxy = new StoryFunctionResult();
    private IStoryFunctionPlugin m_Plugin;
}
internal class ScriptStoryFunction : IStoryFunction
{
    public ScriptStoryFunction(string name)
        : this(name, true)
    {
    }
    public ScriptStoryFunction(string name, bool callScript)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callScript) {
            m_Plugin = new ScriptStoryFunctionPlugin();
            m_Plugin.LoadScript(m_FileName);
            if (null != m_Plugin) {
                m_Plugin.SetProxy(m_Proxy);
            }
        }
    }
    
    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
        Dsl.FunctionData funcData = param as Dsl.FunctionData;
        if (null != funcData) {
            Load(funcData);
        }
        else {
            Dsl.StatementData statementData = param as Dsl.StatementData;
            if (null != statementData) {
                //Is statement type command syntax supported?
                Load(statementData);
            }
            else {
                //error
            }
        }
    }
    public IStoryFunction Clone()
    {
        var newObj = new ScriptStoryFunction(m_ClassName, false);
        newObj.m_Proxy = m_Proxy.Clone();
        if (null != m_Plugin) {
            var ret = m_Plugin.Clone();
            newObj.m_Plugin = new ScriptStoryFunctionPlugin();
            if (null != newObj.m_Plugin) {
                newObj.m_Plugin.SetProxy(newObj.m_Proxy);
            }
        }
        return newObj;
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, ScriptableFramework.BoxedValue iterator, ScriptableFramework.BoxedValueList args)
    {
        if (null != m_Plugin) {
            m_Plugin.Evaluate(instance, handler, iterator, args);
        }
    }
    public bool HaveValue
    {
        get
        {
            return m_Proxy.HaveValue;
        }
    }
    public ScriptableFramework.BoxedValue Value
    {
        get
        {
            return m_Proxy.Value;
        }
    }

    private void Load(Dsl.FunctionData funcData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadFuncData(funcData);
        }
    }
    private void Load(Dsl.StatementData statementData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadStatementData(statementData);
        }
    }

    private string m_FileName;
    private string m_ClassName;
    private StoryFunctionResult m_Proxy = new StoryFunctionResult();
    private ScriptStoryFunctionPlugin m_Plugin;
}
