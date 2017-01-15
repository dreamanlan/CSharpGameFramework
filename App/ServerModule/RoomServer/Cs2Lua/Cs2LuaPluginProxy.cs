using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Plugin;
using SkillSystem;
using StorySystem;

public enum PluginType
{
    Native = 0,
    Lua,
}

public class Cs2LuaPluginProxy
{
	public void Init()
    {
        PluginProxy.NativeProxy = m_NativePluginProxy;
        PluginProxy.LuaProxy = m_LuaPluginProxy;
	}

    private NativePluginProxy m_NativePluginProxy = new NativePluginProxy();
    private LuaPluginProxy m_LuaPluginProxy = new LuaPluginProxy();

    public static Cs2LuaPluginProxy Instance
    {
        get { return s_Instance; }
    }
    private static Cs2LuaPluginProxy s_Instance = new Cs2LuaPluginProxy();
}

internal class NativePluginProxy : IPluginProxy
{
    public void RegisterSkillTrigger(string name, string implClass)
    {
        SkillTrigerManager.Instance.RegisterTrigerFactory(name, new NativeSkillTriggerFactory(implClass), true);
    }

    public void RegisterStoryCommand(string name, string implClass)
    {
        StoryCommandManager.Instance.RegisterCommandFactory(name, new NativeStoryCommandFactory(implClass), true);
    }

    public void RegisterStoryValue(string name, string implClass)
    {
        StoryValueManager.Instance.RegisterValueFactory(name, new NativeStoryValueFactory(implClass), true);
    }

    public void RegisterSimpleStoryCommand(string name, string implClass)
    {
        StoryCommandManager.Instance.RegisterCommandFactory(name, new NativeSimpleStoryCommandFactory(implClass), true);
    }

    public void RegisterSimpleStoryValue(string name, string implClass)
    {
        StoryValueManager.Instance.RegisterValueFactory(name, new NativeSimpleStoryValueFactory(implClass), true);
    }
}

internal class LuaPluginProxy : IPluginProxy
{
    public void RegisterSkillTrigger(string name, string implClass)
    {
        SkillTrigerManager.Instance.RegisterTrigerFactory(name, new LuaSkillTriggerFactory(implClass), true);
    }

    public void RegisterStoryCommand(string name, string implClass)
    {
        StoryCommandManager.Instance.RegisterCommandFactory(name, new LuaStoryCommandFactory(implClass), true);
    }

    public void RegisterStoryValue(string name, string implClass)
    {
        StoryValueManager.Instance.RegisterValueFactory(name, new LuaSimpleStoryValueFactory(implClass), true);
    }

    public void RegisterSimpleStoryCommand(string name, string implClass)
    {
        StoryCommandManager.Instance.RegisterCommandFactory(name, new LuaSimpleStoryCommandFactory(implClass), true);
    }

    public void RegisterSimpleStoryValue(string name, string implClass)
    {
        StoryValueManager.Instance.RegisterValueFactory(name, new LuaSimpleStoryValueFactory(implClass), true);
    }
}