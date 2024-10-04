using System;
using System.Collections.Generic;
using System.Text;
using ScriptableFramework.Plugin;
using ScriptableFramework.Story;
using DotnetStoryScript;

internal class NativeSimpleStoryFunctionFactory : IStoryFunctionFactory
{
    public IStoryFunction Build()
    {
        return new NativeSimpleStoryFunction(m_ClassName);
    }
    public NativeSimpleStoryFunctionFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class ScriptSimpleStoryFunctionFactory : IStoryFunctionFactory
{
    public IStoryFunction Build()
    {
        return new ScriptSimpleStoryFunction(m_ClassName);
    }
    public ScriptSimpleStoryFunctionFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class NativeSimpleStoryFunction : IStoryFunction
{
    public NativeSimpleStoryFunction(string name)
        : this(name, true)
    { }
    public NativeSimpleStoryFunction(string name, bool create)
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
        var newObj = new NativeSimpleStoryFunction(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryFunctionParams;
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

    private StoryFunctionParams m_Params = new StoryFunctionParams();
    private StoryFunctionResult m_Proxy = new StoryFunctionResult();
    private string m_ClassName;

    private ISimpleStoryFunctionPlugin m_Plugin;
}
internal class ScriptSimpleStoryFunction : IStoryFunction
{
    public ScriptSimpleStoryFunction(string name)
        : this(name, true)
    {
    }
    public ScriptSimpleStoryFunction(string name, bool callScript)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callScript) {
            m_Plugin = new ScriptSimpleStoryFunctionPlugin();
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
        var newObj = new ScriptSimpleStoryFunction(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryFunctionParams;
        newObj.m_Proxy = m_Proxy.Clone();
        if (null != m_Plugin) {
            var ret = m_Plugin.Clone();
            newObj.m_Plugin = new ScriptSimpleStoryFunctionPlugin();
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

    private StoryFunctionParams m_Params = new StoryFunctionParams();
    private StoryFunctionResult m_Proxy = new StoryFunctionResult();

    private string m_FileName;
    private string m_ClassName;
    private ScriptSimpleStoryFunctionPlugin m_Plugin;
}
