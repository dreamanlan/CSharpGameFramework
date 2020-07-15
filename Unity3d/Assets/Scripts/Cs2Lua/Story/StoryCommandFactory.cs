using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using GameFramework.Story;
using StorySystem;
using SLua;

internal class NativeStoryCommandFactory : IStoryCommandFactory
{
    public IStoryCommand Create()
    {
        return new NativeStoryCommand(m_ClassName);
    }
    public NativeStoryCommandFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}
internal class LuaStoryCommandFactory : IStoryCommandFactory
{
    public IStoryCommand Create()
    {
        return new LuaStoryCommand(m_ClassName);
    }
    public LuaStoryCommandFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}
internal class NativeStoryCommand : AbstractStoryCommand
{
    public NativeStoryCommand(string name) : this(name, true)
    { }
    public NativeStoryCommand(string name, bool create)
    {
        m_ClassName = name;
        if (create) {
            var module = PluginManager.Instance.CreateObject(m_ClassName);
            m_Plugin = module as IStoryCommandPlugin;
        }
    }
    protected override IStoryCommand CloneCommand()
    {
        var newObj = new NativeStoryCommand(m_ClassName, false);
        if (null != m_Plugin) {
            newObj.m_Plugin = m_Plugin.Clone();
        }
        return newObj;
    }
    protected override void ResetState()
    {
        if (null != m_Plugin) {
            m_Plugin.ResetState();
        }
    }
    protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
    {
        if (null != m_Plugin) {
            m_Plugin.Evaluate(instance, handler, iterator, args);
        }
    }
    protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
    {
        if (null != m_Plugin) {
            return m_Plugin.ExecCommand(instance, handler, delta);
        }
        return false;
    }
    protected override void Load(Dsl.FunctionData funcData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadFuncData(funcData);
        }
    }
    protected override void Load(Dsl.StatementData statementData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadStatementData(statementData);
        }
    }

    private string m_ClassName;
    private IStoryCommandPlugin m_Plugin;
}
internal class LuaStoryCommand : AbstractStoryCommand
{
    public LuaStoryCommand(string name)
        : this(name, true)
    {
    }
    public LuaStoryCommand(string name, bool callLua)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callLua) {
            m_Plugin = new Cs2LuaStoryCommandPlugin();
            m_Plugin.LoadLua(m_FileName);
            IsCompositeCommand = m_Plugin.IsCompositeCommand();
        }
    }

    protected override IStoryCommand CloneCommand()
    {
        var newObj = new LuaStoryCommand(m_ClassName, false);
        if (null != m_Plugin) {
            var ret = m_Plugin.Clone();
            newObj.m_Plugin = new Cs2LuaStoryCommandPlugin();
            newObj.m_Plugin.InitLua((LuaTable)ret, m_FileName);
            newObj.IsCompositeCommand = newObj.m_Plugin.IsCompositeCommand();
        }
        return newObj;
    }
    protected override void ResetState()
    {
        if (null != m_Plugin) {
            m_Plugin.ResetState();
        }
    }
    protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, object iterator, object[] args)
    {
        if (null != m_Plugin) {
            m_Plugin.Evaluate(instance, handler, iterator, args);
        }
    }
    protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta)
    {
        if (null != m_Plugin) {
            return m_Plugin.ExecCommand(instance, handler, delta);
        }
        return false;
    }
    protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta, object iterator, object[] args)
    {
        if (null != m_Plugin) {
            return m_Plugin.ExecCommandWithArgs(instance, handler, delta, iterator, args);
        }
        return false;
    }
    protected override void Load(Dsl.FunctionData funcData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadFuncData(funcData);
        }
    }
    protected override void Load(Dsl.StatementData statementData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadStatementData(statementData);
        }
    }

    private string m_FileName;
    private string m_ClassName;
    private Cs2LuaStoryCommandPlugin m_Plugin;
}
