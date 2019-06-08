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
        m_Params.InitFromDsl(param, 0);
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
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
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
    public object Value
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
            m_Svr = Cs2LuaAssembly.Instance.LuaSvr;
            m_Svr.luaState.doFile(m_FileName);
            m_ClassObj = (LuaTable)m_Svr.luaState[m_ClassName];
            m_Self = (LuaTable)((LuaFunction)m_ClassObj["__new_object"]).call();
            BindLuaInterface();
            if (null != m_SetProxy) {
                m_SetProxy.call(m_Self, m_Proxy);
            }
        }
    }

    public void InitFromDsl(Dsl.ISyntaxComponent param)
    {
        m_Params.InitFromDsl(param, 0);
    }
    public IStoryValue Clone()
    {
        var newObj = new LuaSimpleStoryValue(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryValueParams;
        newObj.m_Proxy = m_Proxy.Clone();
        if (null != m_Clone) {
            var ret = m_Clone.call(m_Self);
            newObj.m_Svr = m_Svr;
            newObj.m_ClassObj = m_ClassObj;
            newObj.m_Self = (LuaTable)ret;
            newObj.BindLuaInterface();
            if (null != newObj.m_SetProxy) {
                newObj.m_SetProxy.call(newObj.m_Self, newObj.m_Proxy);
            }
        }
        return newObj;
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
    {
        if (null != m_Evaluate) {
            m_Proxy.HaveValue = false;
            m_Params.Evaluate(instance, handler, iterator, args);
            if (m_Params.HaveValue) {
                m_Evaluate.call(m_Self, instance, m_Params);
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
    public object Value
    {
        get
        {
            return m_Proxy.Value;
        }
    }

    private void BindLuaInterface()
    {
        if (null != m_Self) {
            m_SetProxy = (LuaFunction)m_Self["SetProxy"];
            m_Clone = (LuaFunction)m_Self["Clone"];
            m_Evaluate = (LuaFunction)m_Self["Evaluate"];
        }
    }

    private StoryValueParams m_Params = new StoryValueParams();
    private StoryValueResult m_Proxy = new StoryValueResult();

    private string m_FileName;
    private string m_ClassName;

    private LuaSvr m_Svr;
    private LuaTable m_ClassObj;
    private LuaTable m_Self;
    private LuaFunction m_SetProxy;
    private LuaFunction m_Clone;
    private LuaFunction m_Evaluate;
}
