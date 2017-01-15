using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using GameFramework.Story;
using StorySystem;
using SLua;

internal class NativeSimpleStoryCommandFactory : IStoryCommandFactory
{
    public IStoryCommand Create()
    {
        return new NativeSimpleStoryCommand(m_ClassName);
    }
    public NativeSimpleStoryCommandFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}
internal class LuaSimpleStoryCommandFactory : IStoryCommandFactory
{
    public IStoryCommand Create()
    {
        return new LuaSimpleStoryCommand(m_ClassName);
    }
    public LuaSimpleStoryCommandFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}
internal class NativeSimpleStoryCommand : IStoryCommand
{
    public NativeSimpleStoryCommand(string name)
        : this(name, true)
    { }
    public NativeSimpleStoryCommand(string name, bool create)
    {
        m_ClassName = name;
        if (create) {
            var module = PluginManager.Instance.CreateObject(m_ClassName);
            m_Plugin = module as ISimpleStoryCommandPlugin;
        }
    }
    public void Init(Dsl.ISyntaxComponent config)
    {
        m_Params.InitFromDsl(config, 0);
    }
    public IStoryCommand Clone()
    {
        NativeSimpleStoryCommand newObj = new NativeSimpleStoryCommand(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryValueParams;
        if (null != m_Plugin) {
            newObj.m_Plugin = m_Plugin.Clone();
        }
        return newObj;
    }
    public IStoryCommand LeadCommand
    {
        get { return null; }
    }
    public void Reset()
    {
        m_LastExecResult = false;
        ResetState();
    }
    public bool Execute(StoryInstance instance, long delta, object iterator, object[] args)
    {
        if (!m_LastExecResult) {
            //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
            m_Params.Evaluate(instance, iterator, args);
        }
        m_LastExecResult = ExecCommand(instance, m_Params, delta);
        return m_LastExecResult;
    }
    public void Analyze(StoryInstance instance)
    {
    }

    private void ResetState()
    {
        if (null != m_Plugin) {
            m_Plugin.ResetState();
        }
    }
    private bool ExecCommand(StoryInstance instance, StoryValueParams _params, long delta)
    {
        if (null != m_Plugin) {
            return m_Plugin.ExecCommand(instance, _params, delta);
        }
        return false;
    }

    private bool m_LastExecResult = false;
    private StoryValueParams m_Params = new StoryValueParams();
    
    private string m_ClassName;
    private ISimpleStoryCommandPlugin m_Plugin;
}
internal class LuaSimpleStoryCommand : IStoryCommand
{
    public LuaSimpleStoryCommand(string name)
        : this(name, true)
    {
    }
    public LuaSimpleStoryCommand(string name, bool callLua)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callLua) {
            m_Svr = Cs2LuaAssembly.Instance.LuaSvr;
            m_Svr.luaState.doFile(m_FileName);
            m_ClassObj = (LuaTable)m_Svr.luaState[m_ClassName];
            m_Self = (LuaTable)((LuaFunction)m_ClassObj["__new_object"]).call();
            BindLuaInterface();
        }
    }

    public void Init(Dsl.ISyntaxComponent config)
    {
        m_Params.InitFromDsl(config, 0);
    }
    public IStoryCommand Clone()
    {
        LuaSimpleStoryCommand newObj = new LuaSimpleStoryCommand(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryValueParams;
        if (null != m_Clone) {
            var ret = m_Clone.call(m_Self);
            newObj.m_Svr = m_Svr;
            newObj.m_ClassObj = m_ClassObj;
            newObj.m_Self = (LuaTable)ret;
            newObj.BindLuaInterface();
        }
        return newObj;
    }
    public IStoryCommand LeadCommand
    {
        get { return null; }
    }
    public void Reset()
    {
        m_LastExecResult = false;
        ResetState();
    }
    public bool Execute(StoryInstance instance, long delta, object iterator, object[] args)
    {
        if (!m_LastExecResult) {
            //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
            m_Params.Evaluate(instance, iterator, args);
        }
        m_LastExecResult = ExecCommand(instance, m_Params, delta);
        return m_LastExecResult;
    }
    public void Analyze(StoryInstance instance)
    {
    }

    private void ResetState()
    {
        if (null != m_ResetState) {
            m_ResetState.call(m_Self);
        }
    }
    private bool ExecCommand(StoryInstance instance, StoryValueParams _params, long delta)
    {
        if (null != m_ExecCommand) {
            var ret = m_ExecCommand.call(m_Self, instance, _params, delta);
            if (null != ret) {
                return (bool)ret;
            }
        }
        return false;
    }
    
    private void BindLuaInterface()
    {
        if (null != m_Self) {
            m_Clone = (LuaFunction)m_Self["Clone"];
            m_ResetState = (LuaFunction)m_Self["ResetState"];
            m_ExecCommand = (LuaFunction)m_Self["ExecCommand"];
        }
    }
    
    private bool m_LastExecResult = false;
    private StoryValueParams m_Params = new StoryValueParams();

    private string m_FileName;
    private string m_ClassName;

    private LuaSvr m_Svr;
    private LuaTable m_ClassObj;
    private LuaTable m_Self;
    private LuaFunction m_Clone;
    private LuaFunction m_ResetState;
    private LuaFunction m_ExecCommand;
}
