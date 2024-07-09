using System;
using System.Collections.Generic;
using System.Text;
using ScriptableFramework.Plugin;
using ScriptableFramework.Story;
using DotnetStoryScript;

internal class NativeSimpleStoryValueFactory : IStoryFunctionFactory
{
    public IStoryFunction Build()
    {
        return new NativeSimpleStoryValue(m_ClassName);
    }
    public NativeSimpleStoryValueFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class ScriptSimpleStoryValueFactory : IStoryFunctionFactory
{
    public IStoryFunction Build()
    {
        return new ScriptSimpleStoryValue(m_ClassName);
    }
    public ScriptSimpleStoryValueFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class NativeSimpleStoryValue : IStoryFunction
{
    public NativeSimpleStoryValue(string name)
        : this(name, true)
    { }
    public NativeSimpleStoryValue(string name, bool create)
    {
        m_ClassName = name;
        if (create) {
            var module = PluginManager.Instance.CreateObject(m_ClassName);
            m_Plugin = module as ISimpleStoryFunctionPlugin;
            if (null != m_Plugin) {
                m_Plugin.SetProxy(m_Proxy);
            }
        }
    }

    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
        m_Params.InitFromDsl(param, 0, false);
    }
    public IStoryFunction Clone()
    {
        var newObj = new NativeSimpleStoryValue(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryValueParams;
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
            m_Proxy.HaveValue = false;
            m_Params.Evaluate(instance, handler, iterator, args);
            if (m_Params.HaveValue) {
                m_Plugin.Evaluate(instance, handler, m_Params);
            }
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

    private StoryValueParams m_Params = new StoryValueParams();
    private StoryValueResult m_Proxy = new StoryValueResult();
    private string m_ClassName;

    private ISimpleStoryFunctionPlugin m_Plugin;
}
internal class ScriptSimpleStoryValue : IStoryFunction
{
    public ScriptSimpleStoryValue(string name)
        : this(name, true)
    {
    }
    public ScriptSimpleStoryValue(string name, bool callScript)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callScript) {
            m_Plugin = new ScriptSimpleStoryValuePlugin();
            m_Plugin.LoadScript(m_FileName);
            if (null != m_Plugin) {
                m_Plugin.SetProxy(m_Proxy);
            }
        }
    }

    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
        m_Params.InitFromDsl(param, 0, false);
    }
    public IStoryFunction Clone()
    {
        var newObj = new ScriptSimpleStoryValue(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryValueParams;
        newObj.m_Proxy = m_Proxy.Clone();
        if (null != m_Plugin) {
            var ret = m_Plugin.Clone();
            newObj.m_Plugin = new ScriptSimpleStoryValuePlugin();
            if (null != newObj.m_Plugin) {
                newObj.m_Plugin.SetProxy(newObj.m_Proxy);
            }
        }
        return newObj;
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, ScriptableFramework.BoxedValue iterator, ScriptableFramework.BoxedValueList args)
    {
        if (null != m_Plugin) {
            m_Proxy.HaveValue = false;
            m_Params.Evaluate(instance, handler, iterator, args);
            if (m_Params.HaveValue) {
                m_Plugin.Evaluate(instance, handler, m_Params);
            }
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

    private StoryValueParams m_Params = new StoryValueParams();
    private StoryValueResult m_Proxy = new StoryValueResult();

    private string m_FileName;
    private string m_ClassName;
    private ScriptSimpleStoryValuePlugin m_Plugin;
}
