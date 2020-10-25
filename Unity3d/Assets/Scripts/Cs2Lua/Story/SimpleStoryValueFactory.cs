using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using GameFramework.Story;
using StorySystem;
using SLua;

internal class NativeSimpleStoryValueFactory : IStoryValueFactory
{
    public IStoryValue Build()
    {
        return new NativeSimpleStoryValue(m_ClassName);
    }
    public NativeSimpleStoryValueFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class LuaSimpleStoryValueFactory : IStoryValueFactory
{
    public IStoryValue Build()
    {
        return new LuaSimpleStoryValue(m_ClassName);
    }
    public LuaSimpleStoryValueFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class NativeSimpleStoryValue : IStoryValue
{
    public NativeSimpleStoryValue(string name)
        : this(name, true)
    { }
    public NativeSimpleStoryValue(string name, bool create)
    {
        m_ClassName = name;
        if (create) {
            var module = PluginManager.Instance.CreateObject(m_ClassName);
            m_Plugin = module as ISimpleStoryValuePlugin;
            if (null != m_Plugin) {
                m_Plugin.SetProxy(m_Proxy);
            }
        }
    }

    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
        m_Params.InitFromDsl(param, 0, false);
    }
    public IStoryValue Clone()
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
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
    public BoxedValue Value
    {
        get
        {
            return m_Proxy.Value;
        }
    }

    private StoryValueParams m_Params = new StoryValueParams();
    private StoryValueResult m_Proxy = new StoryValueResult();
    private string m_ClassName;

    private ISimpleStoryValuePlugin m_Plugin;
}
internal class LuaSimpleStoryValue : IStoryValue
{
    public LuaSimpleStoryValue(string name)
        : this(name, true)
    {
    }
    public LuaSimpleStoryValue(string name, bool callLua)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callLua) {
            m_Plugin = new Cs2LuaSimpleStoryValuePlugin();
            m_Plugin.LoadLua(m_FileName);
            if (null != m_Plugin) {
                m_Plugin.SetProxy(m_Proxy);
            }
        }
    }

    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
        m_Params.InitFromDsl(param, 0, false);
    }
    public IStoryValue Clone()
    {
        var newObj = new LuaSimpleStoryValue(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryValueParams;
        newObj.m_Proxy = m_Proxy.Clone();
        if (null != m_Plugin) {
            var ret = m_Plugin.Clone();
            newObj.m_Plugin = new Cs2LuaSimpleStoryValuePlugin();
            newObj.m_Plugin.InitLua((LuaTable)ret, m_FileName);
            if (null != newObj.m_Plugin) {
                newObj.m_Plugin.SetProxy(newObj.m_Proxy);
            }
        }
        return newObj;
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, BoxedValue iterator, BoxedValueList args)
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
    public BoxedValue Value
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
    private Cs2LuaSimpleStoryValuePlugin m_Plugin;
}
