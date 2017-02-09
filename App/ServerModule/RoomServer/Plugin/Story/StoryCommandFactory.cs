using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Story;
using StorySystem;

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
    public override IStoryCommand Clone()
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
    protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
    {
        if (null != m_Plugin) {
            m_Plugin.Evaluate(instance, iterator, args);
        }
    }
    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
        if (null != m_Plugin) {
            return m_Plugin.ExecCommand(instance, delta);
        }
        return false;
    }
    protected override void Load(Dsl.CallData callData)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadCallData(callData);
        }
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
