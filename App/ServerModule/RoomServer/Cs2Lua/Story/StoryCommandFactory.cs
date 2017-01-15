using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
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
            m_DelayCallLua = true;
        }
    }

    public override IStoryCommand Clone()
    {
        var newObj = new LuaStoryCommand(m_ClassName, false);
        if (null != m_Clone) {
            Clone(newObj);
        } else {
            AddDelayCommand(() => { Clone(newObj); });
        }
        return newObj;
    }
    protected override void ResetState()
    {
        if (null != m_ResetState) {
            m_ResetState.call(m_Self);
        } else if (!m_DelayReset) {
            m_DelayReset = true;
            AddDelayCommand(() => { ResetState(); });
        }
    }
    protected override void Evaluate(StoryInstance instance, object iterator, object[] args)
    {
        if (null != m_Evaluate) {
            m_Evaluate.call(m_Self, instance, iterator, args);
        } else if (!m_DelayEvaluate) {
            m_DelayEvaluate = true;
            AddDelayCommand(() => { Evaluate(instance, iterator, args); });
        }
    }
    protected override bool ExecCommand(StoryInstance instance, long delta)
    {
        if (!Prepare(instance.Context as Scene)) {
            return true;
        }
        if (null != m_ExecCommand) {
            var ret = m_ExecCommand.call(m_Self, instance, delta);
            if (null != ret) {
                return (bool)ret;
            }
        }
        return false;
    }
    protected override void Load(Dsl.CallData callData)
    {
        if (null != m_Load1) {
            m_Load1.call(m_Self, callData);
        } else {
            AddDelayCommand(() => { Load(callData); });
        }
    }
    protected override void Load(Dsl.FunctionData funcData)
    {
        if (null != m_Load2) {
            m_Load2.call(m_Self, funcData);
        } else {
            AddDelayCommand(() => { Load(funcData); });
        }
    }
    protected override void Load(Dsl.StatementData statementData)
    {
        if (null != m_Load3) {
            m_Load3.call(m_Self, statementData);
        } else {
            AddDelayCommand(() => { Load(statementData); });
        }
    }

    private bool Prepare(Scene scene)
    {
        if (m_DelayCallLua) {
            var rum = scene.GetRoomUserManager();
            int roomId = rum.RoomId;
            int localId = rum.LocalRoomId;
            Room room = RoomManager.Instance.GetRoomByRoomIdAndLocalId(roomId, localId);
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
    }
    private void Clone(LuaStoryCommand newObj)
    {
        var ret = m_Clone.call(m_Self);
        newObj.m_Svr = m_Svr;
        newObj.m_ClassObj = m_ClassObj;
        newObj.m_Self = (LuaTable)ret;
        newObj.BindLuaInterface();
    }
    private void BindLuaInterface()
    {
        if (null != m_Self) {
            m_Clone = (LuaFunction)m_Self["Clone"];
            m_ResetState = (LuaFunction)m_Self["ResetState"];
            m_Evaluate = (LuaFunction)m_Self["Evaluate"];
            m_ExecCommand = (LuaFunction)m_Self["ExecCommand"];
            m_Load1 = (LuaFunction)m_Self["LoadCallData"];
            m_Load2 = (LuaFunction)m_Self["LoadFuncData"];
            m_Load3 = (LuaFunction)m_Self["LoadStatementData"];
        }
    }
    private void AddDelayCommand(Action cmd)
    {
        m_DelayCommand = true;
        m_DelayedCommands.Enqueue(cmd);
    }

    private string m_FileName;
    private string m_ClassName;

    private bool m_DelayCallLua = false;
    private bool m_DelayCommand = false;
    private bool m_DelayReset = false;
    private bool m_DelayEvaluate = false;
    private Queue<Action> m_DelayedCommands = new Queue<Action>();

    private LuaSvr m_Svr;
    private LuaTable m_ClassObj;
    private LuaTable m_Self;
    private LuaFunction m_Clone;
    private LuaFunction m_ResetState;
    private LuaFunction m_Evaluate;
    private LuaFunction m_ExecCommand;
    private LuaFunction m_Load1;
    private LuaFunction m_Load2;
    private LuaFunction m_Load3;
}
