using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using GameFramework.Story;
using StorySystem;

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
internal class ScriptSimpleStoryCommandFactory : IStoryCommandFactory
{
    public IStoryCommand Create()
    {
        return new ScriptSimpleStoryCommand(m_ClassName);
    }
    public ScriptSimpleStoryCommandFactory(string name)
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
    public string GetId()
    {
        return m_Config.GetId();
    }
    public Dsl.FunctionData GetComments()
    {
        return m_Comments;
    }
    public Dsl.ISyntaxComponent GetConfig()
    {
        return m_Config;
    }
    public void ShareConfig(IStoryCommand cloner)
    {
        m_Comments = cloner.GetComments();
        m_Config = cloner.GetConfig();
    }
    public bool Init(Dsl.ISyntaxComponent config)
    {
        m_Comments = m_Params.InitFromDsl(config, 0, true);
        m_Config = config;
        return true;
    }
    public IStoryCommand Clone()
    {
        NativeSimpleStoryCommand newObj = new NativeSimpleStoryCommand(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryValueParams;
        newObj.m_Comments = m_Comments;
        newObj.m_Config = m_Config;
        if (null != m_Plugin) {
            newObj.m_Plugin = m_Plugin.Clone();
        }
        return newObj;
    }
    public virtual IStoryCommand PrologueCommand
    {
        get { return null; }
    }
    public virtual IStoryCommand EpilogueCommand
    {
        get { return null; }
    }
    public void Reset()
    {
        m_LastExecResult = false;
        ResetState();
    }
    public bool Execute(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
    {
        if (!m_LastExecResult) {
            //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
            m_Params.Evaluate(instance, handler, iterator, args);
        }
        m_LastExecResult = ExecCommand(instance, handler, m_Params, delta);
        return m_LastExecResult;
    }
    public bool ExecDebugger(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
    {
        return false;
    }

    private void ResetState()
    {
        if (null != m_Plugin) {
            m_Plugin.ResetState();
        }
    }
    private bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params, long delta)
    {
        if (null != m_Plugin) {
            return m_Plugin.ExecCommand(instance, handler, _params, delta);
        }
        return false;
    }

    private bool m_LastExecResult = false;
    private StoryValueParams m_Params = new StoryValueParams();
    private Dsl.FunctionData m_Comments;
    private Dsl.ISyntaxComponent m_Config;
    
    private string m_ClassName;
    private ISimpleStoryCommandPlugin m_Plugin;
}
internal class ScriptSimpleStoryCommand : IStoryCommand
{
    public ScriptSimpleStoryCommand(string name)
        : this(name, true)
    {
    }
    public ScriptSimpleStoryCommand(string name, bool callScript)
    {
        m_ClassName = name;
        m_FileName = m_ClassName.Replace(".", "__");

        if (callScript) {
        }
    }

    public string GetId()
    {
        return m_Config.GetId();
    }
    public Dsl.FunctionData GetComments()
    {
        return m_Comments;
    }
    public Dsl.ISyntaxComponent GetConfig()
    {
        return m_Config;
    }
    public void ShareConfig(IStoryCommand cloner)
    {
        m_Comments = cloner.GetComments();
        m_Config = cloner.GetConfig();
    }
    public bool Init(Dsl.ISyntaxComponent config)
    {
        m_Comments = m_Params.InitFromDsl(config, 0, true);
        m_Config = config;
        return true;
    }
    public IStoryCommand Clone()
    {
        ScriptSimpleStoryCommand newObj = new ScriptSimpleStoryCommand(m_ClassName, false);
        newObj.m_Params = m_Params.Clone() as StoryValueParams;
        newObj.m_Comments = m_Comments;
        newObj.m_Config = m_Config;
        if (null != m_Plugin) {
            var ret = m_Plugin.Clone();
            newObj.m_Plugin = new ScriptSimpleStoryCommandPlugin();
        }
        return newObj;
    }
    public virtual IStoryCommand PrologueCommand
    {
        get { return null; }
    }
    public virtual IStoryCommand EpilogueCommand
    {
        get { return null; }
    }
    public void Reset()
    {
        m_LastExecResult = false;
        ResetState();
    }
    public bool Execute(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
    {
        if (!m_LastExecResult) {
            //重复执行时不需要每个tick都更新变量值，每个命令每次执行，变量值只读取一次。
            m_Params.Evaluate(instance, handler, iterator, args);
        }
        m_LastExecResult = ExecCommand(instance, handler, m_Params, delta);
        return m_LastExecResult;
    }
    public bool ExecDebugger(StoryInstance instance, StoryMessageHandler handler, long delta, BoxedValue iterator, BoxedValueList args)
    {
        return false;
    }

    private void ResetState()
    {
        if (null != m_Plugin) {
            m_Plugin.ResetState();
        }
    }
    private bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params, long delta)
    {
        if (null != m_Plugin) {
            var ret = m_Plugin.ExecCommand(instance, handler, _params, delta);
            if (ret) {
                return ret;
            }
        }
        return false;
    }
    
    private bool m_LastExecResult = false;
    private StoryValueParams m_Params = new StoryValueParams();
    private Dsl.FunctionData m_Comments;
    private Dsl.ISyntaxComponent m_Config;

    private string m_FileName;
    private string m_ClassName;
    private ScriptSimpleStoryCommandPlugin m_Plugin;
}
