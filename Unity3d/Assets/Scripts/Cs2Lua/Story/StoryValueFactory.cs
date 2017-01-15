using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using GameFramework.Story;
using StorySystem;
using SLua;

internal class NativeStoryValueFactory : IStoryValueFactory
{
    public IStoryValue<object> Build()
    {
        return new NativeStoryValue(m_ClassName);
    }
    public NativeStoryValueFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}

internal class LuaStoryValueFactory : IStoryValueFactory
{
    public IStoryValue<object> Build()
    {
        return new LuaStoryValue(m_ClassName);
    }
    public LuaStoryValueFactory(string name)
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
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData) {
            Load(callData);
        } else {
            Dsl.FunctionData funcData = param as Dsl.FunctionData;
            if (null != funcData) {
                Load(funcData);
            } else {
                Dsl.StatementData statementData = param as Dsl.StatementData;
                if (null != statementData) {
                    //是否支持语句类型的命令语法？
                    Load(statementData);
                } else {
                    //error
                }
            }
        }
    }
    public IStoryValue<object> Clone()
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
    public void Evaluate(StoryInstance instance, object iterator, object[] args)
    {
        if (null != m_Plugin) {
            m_Plugin.Evaluate(instance, iterator, args);
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

    private void Load(Dsl.CallData callData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadCallData(callData);
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
internal class LuaStoryValue : IStoryValue
{
    public LuaStoryValue(string name)
        : this(name, true)
    {
    }
    public LuaStoryValue(string name, bool callLua)
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
        Dsl.CallData callData = param as Dsl.CallData;
        if (null != callData) {
            Load(callData);
        } else {
            Dsl.FunctionData funcData = param as Dsl.FunctionData;
            if (null != funcData) {
                Load(funcData);
            } else {
                Dsl.StatementData statementData = param as Dsl.StatementData;
                if (null != statementData) {
                    //是否支持语句类型的命令语法？
                    Load(statementData);
                } else {
                    //error
                }
            }
        }
    }
    public IStoryValue<object> Clone()
    {
        var newObj = new LuaStoryValue(m_ClassName, false);
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
    public void Evaluate(StoryInstance instance, object iterator, object[] args)
    {
        if (null != m_Evaluate) {
            m_Evaluate.call(m_Self, instance, iterator, args);
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

    private void Load(Dsl.CallData callData)
    {
        if (null != m_Load1) {
            m_Load1.call(m_Self, callData);
        }
    }
    private void Load(Dsl.FunctionData funcData)
    {
        if (null != m_Load2) {
            m_Load2.call(m_Self, funcData);
        }
    }
    private void Load(Dsl.StatementData statementData)
    {
        if (null != m_Load3) {
            m_Load3.call(m_Self, statementData);
        }
    }

    private void BindLuaInterface()
    {
        if (null != m_Self) {
            m_SetProxy = (LuaFunction)m_Self["SetProxy"];
            m_Clone = (LuaFunction)m_Self["Clone"];
            m_Evaluate = (LuaFunction)m_Self["Evaluate"];
            m_Load1 = (LuaFunction)m_Self["LoadCallData"];
            m_Load2 = (LuaFunction)m_Self["LoadFuncData"];
            m_Load3 = (LuaFunction)m_Self["LoadStatementData"];
        }
    }

    private string m_FileName;
    private string m_ClassName;
    private StoryValueResult m_Proxy = new StoryValueResult();

    private LuaSvr m_Svr;
    private LuaTable m_ClassObj;
    private LuaTable m_Self;
    private LuaFunction m_SetProxy;
    private LuaFunction m_Clone;
    private LuaFunction m_Evaluate;
    private LuaFunction m_Load1;
    private LuaFunction m_Load2;
    private LuaFunction m_Load3;
}
