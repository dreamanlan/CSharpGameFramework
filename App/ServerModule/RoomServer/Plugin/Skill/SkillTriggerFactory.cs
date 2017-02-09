using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using SkillSystem;

internal class NativeSkillTriggerFactory : ISkillTrigerFactory
{
    public ISkillTriger Create()
    {
        return new NativeSkillTrigger(m_ClassName);
    }
    public NativeSkillTriggerFactory(string name)
    {
        m_ClassName = name;
    }

    private string m_ClassName;
}
internal class NativeSkillTrigger : CsSkillTrigger<NativeSkillTrigger>
{
    public NativeSkillTrigger(string name) :
        this(name, true)
    { }
    public NativeSkillTrigger(string name, bool create)
        : base(name)
    {
        if (create) {
            var module = PluginManager.Instance.CreateObject(m_ClassName);
            m_Plugin = module as ISkillTriggerPlugin;
            if (null != m_Plugin) {
                m_Plugin.SetProxy(m_Proxy);
            }
        }
    }
    protected override NativeSkillTrigger Create()
    {
        return new NativeSkillTrigger(m_ClassName, false);
    }
}
internal abstract class CsSkillTrigger<T> : AbstractSkillTriger where T : CsSkillTrigger<T>
{
    public CsSkillTrigger(string name)
    {
        m_ClassName = name;
        m_Proxy = new SkillTriggerProxy(this);
    }

    protected abstract T Create();

    protected override ISkillTriger OnClone()
    {
        var newObj = Create();
        if (null != m_Plugin) {
            newObj.m_Plugin = m_Plugin.Clone();
            if (null != newObj.m_Plugin) {
                newObj.m_Plugin.SetProxy(newObj.m_Proxy);
            }
        }
        return newObj;
    }

    public override void Reset()
    {
        if (null != m_Plugin) {
            m_Plugin.Reset();
        }
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
        if (null != m_Plugin) {
            return m_Plugin.Execute(sender, instance, delta, curSectionTime);
        }
        return false;
    }

    protected override void Load(Dsl.CallData callData, SkillInstance instance)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadCallData(callData, instance);
        }
    }

    protected override void Load(Dsl.FunctionData funcData, SkillInstance instance)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadFuncData(funcData, instance);
        }
    }

    protected override void Load(Dsl.StatementData statementData, SkillInstance instance)
    {
        if (null != m_Plugin) {
            m_Plugin.LoadStatementData(statementData, instance);
        }
    }

    protected override void OnInitProperties()
    {
        if (null != m_Plugin) {
            m_Plugin.OnInitProperties();
        }
    }

    protected string m_ClassName;
    protected SkillTriggerProxy m_Proxy;
    protected ISkillTriggerPlugin m_Plugin;
}