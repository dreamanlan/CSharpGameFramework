using System;
using System.Collections.Generic;
using System.Text;
using ScriptableFramework.Plugin;
using ScriptableFramework.Story;
using DotnetStoryScript;

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
internal class ScriptStoryCommandFactory : IStoryCommandFactory
{
    public IStoryCommand Create()
    {
        return new ScriptStoryCommand(m_ClassName);
    }
    public ScriptStoryCommandFactory(string name)
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
    protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, ScriptableFramework.BoxedValue iterator, ScriptableFramework.BoxedValueList args)
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
    protected override bool Load(Dsl.FunctionData funcData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadFuncData(funcData);
        }
        return false;
    }
    protected override bool Load(Dsl.StatementData statementData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadStatementData(statementData);
        }
        return false;
    }

    private string m_ClassName;
    private IStoryCommandPlugin m_Plugin;
}
internal class ScriptStoryCommand : AbstractStoryCommand
{
    public ScriptStoryCommand(string name)
        : this(name, true)
    {
    }
    public ScriptStoryCommand(string name, bool callScript)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callScript) {
            m_Plugin = new ScriptStoryCommandPlugin();
            m_Plugin.LoadScript(m_FileName);
            m_IsCompositeCommand = m_Plugin.IsCompositeCommand();
        }
    }
    public override bool IsCompositeCommand => m_IsCompositeCommand;
    protected override IStoryCommand CloneCommand()
    {
        var newObj = new ScriptStoryCommand(m_ClassName, false);
        if (null != m_Plugin) {
            var ret = m_Plugin.Clone();
            newObj.m_Plugin = new ScriptStoryCommandPlugin();
            newObj.m_IsCompositeCommand = newObj.m_Plugin.IsCompositeCommand();
        }
        return newObj;
    }
    protected override void ResetState()
    {
        if (null != m_Plugin) {
            m_Plugin.ResetState();
        }
    }
    protected override void Evaluate(StoryInstance instance, StoryMessageHandler handler, ScriptableFramework.BoxedValue iterator, ScriptableFramework.BoxedValueList args)
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
    protected override bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, long delta, ScriptableFramework.BoxedValue iterator, ScriptableFramework.BoxedValueList args)
    {
        if (null != m_Plugin) {
            return m_Plugin.ExecCommandWithArgs(instance, handler, delta, iterator, args);
        }
        return false;
    }
    protected override bool Load(Dsl.FunctionData funcData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadFuncData(funcData);
        }
        return false;
    }
    protected override bool Load(Dsl.StatementData statementData)
    {
        if (null != m_Plugin) {
            return m_Plugin.LoadStatementData(statementData);
        }
        return false;
    }

    private bool m_IsCompositeCommand;
    private string m_FileName;
    private string m_ClassName;
    private ScriptStoryCommandPlugin m_Plugin;
}
