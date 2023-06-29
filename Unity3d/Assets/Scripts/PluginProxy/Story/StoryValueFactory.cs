using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using GameFramework.Story;
using StorySystem;

internal class NativeStoryValueFactory : IStoryValueFactory
{
    public IStoryValue Build()
    {
        return new NativeStoryValue(m_ClassName);
    }
    public NativeStoryValueFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class ScriptStoryValueFactory : IStoryValueFactory
{
    public IStoryValue Build()
    {
        return new ScriptStoryValue(m_ClassName);
    }
    public ScriptStoryValueFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class NativeStoryValue : IStoryValue
{
    public NativeStoryValue(string name) : this(name, true)
    { }
    public NativeStoryValue(string name, bool create)
    {
        m_ClassName = name;
        if (create) {
            var module = PluginManager.Instance.CreateObject(m_ClassName);
            m_Plugin = module as IStoryValuePlugin;
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
                //是否支持语句类型的命令语法？
                Load(statementData);
            }
            else {
                //error
            }
        }
    }
    public IStoryValue Clone()
    {
        var newObj = new NativeStoryValue(m_ClassName, false);
        newObj.m_Proxy = m_Proxy.Clone();
        if (null != m_Plugin) {
            newObj.m_Plugin = m_Plugin.Clone();
            if (null != newObj.m_Plugin) {
                newObj.m_Plugin.SetProxy(newObj.m_Proxy);
            }
        }
        return newObj;
    }    
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
    public BoxedValue Value
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
    private StoryValueResult m_Proxy = new StoryValueResult();
    private IStoryValuePlugin m_Plugin;
}
internal class ScriptStoryValue : IStoryValue
{
    public ScriptStoryValue(string name)
        : this(name, true)
    {
    }
    public ScriptStoryValue(string name, bool callScript)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callScript) {
            m_Plugin = new ScriptStoryValuePlugin();
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
                //是否支持语句类型的命令语法？
                Load(statementData);
            }
            else {
                //error
            }
        }
    }
    public IStoryValue Clone()
    {
        var newObj = new ScriptStoryValue(m_ClassName, false);
        newObj.m_Proxy = m_Proxy.Clone();
        if (null != m_Plugin) {
            var ret = m_Plugin.Clone();
            newObj.m_Plugin = new ScriptStoryValuePlugin();
            if (null != newObj.m_Plugin) {
                newObj.m_Plugin.SetProxy(newObj.m_Proxy);
            }
        }
        return newObj;
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
    public BoxedValue Value
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
    private StoryValueResult m_Proxy = new StoryValueResult();
    private ScriptStoryValuePlugin m_Plugin;
}
