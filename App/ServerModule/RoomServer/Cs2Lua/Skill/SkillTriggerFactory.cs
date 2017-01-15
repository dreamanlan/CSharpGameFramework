using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using SkillSystem;
using SLua;

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

internal class LuaSkillTriggerFactory : ISkillTrigerFactory
{
    public ISkillTriger Create()
    {
        return new LuaSkillTrigger(m_ClassName);
    }
    public LuaSkillTriggerFactory(string name)
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
internal class LuaSkillTrigger : AbstractSkillTriger
{
    public LuaSkillTrigger(string name)
        : this(name, true)
    {
    }
    public LuaSkillTrigger(string name, bool callLua)
    {
        m_ClassName = name;
        m_Proxy = new SkillTriggerProxy(this);
        m_FileName = m_ClassName.Replace(".", "__");

        if (callLua) {
            m_DelayCallLua = true;
        }
    }

    public override void Reset()
    {
        if (null != m_Reset) {
            m_Reset.call(m_Self);
        } else if(!m_DelayReset) {
            m_DelayReset = true;
            AddDelayCommand(Reset);
        }
    }

    public override bool Execute(object sender, SkillInstance instance, long delta, long curSectionTime)
    {
        if (!Prepare(instance.Context as Scene)) {
            return true;
        }
        if (null != m_Execute) {
            var ret = m_Execute.call(m_Self, sender, instance, delta, curSectionTime);
            if (null != ret) {
                return (bool)ret;
            }
        }
        return false;
    }

    protected override ISkillTriger OnClone()
    {
        var newObj = new LuaSkillTrigger(m_ClassName, false);
        newObj.m_Proxy.DoClone(m_Proxy);
        if (null != m_Clone) {
            Clone(newObj);
        } else {
            AddDelayCommand(() => { Clone(newObj); });
        }
        return newObj;
    }

    protected override void Load(Dsl.CallData callData, SkillInstance instance)
    {
        if (null != m_Load1) {
            m_Load1.call(m_Self, callData, instance);
        } else {
            AddDelayCommand(() => { Load(callData, instance); });
        }
    }

    protected override void Load(Dsl.FunctionData funcData, SkillInstance instance)
    {
        if (null != m_Load2) {
            m_Load2.call(m_Self, funcData, instance);
        } else {
            AddDelayCommand(() => { Load(funcData, instance); });
        }
    }

    protected override void Load(Dsl.StatementData statementData, SkillInstance instance)
    {
        if (null != m_Load3) {
            m_Load3.call(m_Self, statementData, instance);
        } else {
            AddDelayCommand(() => { Load(statementData, instance); });
        }
    }

    protected override void OnInitProperties()
    {
        if (null != m_OnInitProperties) {
            m_OnInitProperties.call(m_Self);
        } else if(!m_DeleyInitProperties) {
            m_DeleyInitProperties = true;
            AddDelayCommand(OnInitProperties);
        }
    }

    private bool Prepare(Scene scene)
    {
        if (m_DelayCallLua) {
            var rum = scene.GetRoomUserManager();
            int roomId = rum.RoomId;
            int localId = rum.LocalRoomId;
            Room room  = RoomManager.Instance.GetRoomByRoomIdAndLocalId(roomId, localId);
            if (null != room) {
                var env = room.ScriptEnvironment;
                CallLua(env);                
            }
            m_DelayCallLua = false;
        }
        if (m_DelayCommand) {
            if (null != m_Svr && null != m_ClassObj && null != m_Self) {
                while (m_DelayedCommands.Count > 0) {
                    var cmd = m_DelayedCommands.Dequeue();
                    cmd();
                }
                m_DelayCommand = false;
            }
        }
        return !(m_DelayCallLua || m_DelayCommand);
    }
    private void CallLua(Cs2LuaEnvironment env)
    {
        m_Svr = env.LuaSvr;
        m_Svr.luaState.doFile(m_FileName);
        m_ClassObj = (LuaTable)m_Svr.luaState[m_ClassName];
        m_Self = (LuaTable)((LuaFunction)m_ClassObj["__new_object"]).call();
        BindLuaInterface();
        if (null != m_SetProxy) {
            m_SetProxy.call(m_Self, m_Proxy);
        }
    }
    private void Clone(LuaSkillTrigger newObj)
    {
        var ret = m_Clone.call(m_Self);
        newObj.m_Svr = m_Svr;
        newObj.m_ClassObj = m_ClassObj;
        newObj.m_Self = (LuaTable)ret;
        newObj.BindLuaInterface();
        if (null != newObj.m_SetProxy) {
            newObj.m_SetProxy.call(newObj.m_Self, newObj.m_Proxy);
        }
    }
    private void BindLuaInterface()
    {
        if (null != m_Self) {
            m_SetProxy = (LuaFunction)m_Self["SetProxy"];
            m_Reset = (LuaFunction)m_Self["Reset"];
            m_Execute = (LuaFunction)m_Self["Execute"];
            m_Clone = (LuaFunction)m_Self["Clone"];
            m_Load1 = (LuaFunction)m_Self["LoadCallData"];
            m_Load2 = (LuaFunction)m_Self["LoadFuncData"];
            m_Load3 = (LuaFunction)m_Self["LoadStatementData"];
            m_OnInitProperties = (LuaFunction)m_Self["OnInitProperties"];
        }
    }
    private void AddDelayCommand(Action cmd)
    {
        m_DelayCommand = true;
        m_DelayedCommands.Enqueue(cmd);
    }

    private string m_FileName;
    private string m_ClassName;
    private SkillTriggerProxy m_Proxy;

    private bool m_DelayCallLua = false;
    private bool m_DelayCommand = false;
    private bool m_DelayReset = false;
    private bool m_DeleyInitProperties = false;
    private Queue<Action> m_DelayedCommands = new Queue<Action>();

    private LuaSvr m_Svr;
    private LuaTable m_ClassObj;
    private LuaTable m_Self;
    private LuaFunction m_SetProxy;
    private LuaFunction m_Reset;
    private LuaFunction m_Execute;
    private LuaFunction m_Clone;
    private LuaFunction m_Load1;
    private LuaFunction m_Load2;
    private LuaFunction m_Load3;
    private LuaFunction m_OnInitProperties;
}