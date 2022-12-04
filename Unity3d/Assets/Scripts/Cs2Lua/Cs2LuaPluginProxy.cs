using UnityEngine;
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

public class Cs2LuaPluginProxy : MonoBehaviour
{
	internal void Awake () 
    {
        PluginProxy.NativeProxy = m_NativePluginProxy;
        PluginProxy.LuaProxy = m_LuaPluginProxy;
	}

    private NativePluginProxy m_NativePluginProxy = new NativePluginProxy();
    private LuaPluginProxy m_LuaPluginProxy = new LuaPluginProxy();
}

internal static class CommonPluginProxy
{
    public static void InstallStartupPlugin(string name, string implClass, bool runAsLua)
    {
        var obj = GameObject.Find(name);
        if (null != obj) {
            var startup = obj.AddComponent<Cs2LuaStartup>();
            if (null != startup) {
                startup.PluginType = runAsLua ? PluginType.Lua : PluginType.Native;
                startup.LuaClassFileName = implClass.Replace(".","__");
                startup.Start();
            }
        }
    }
    public static void RemoveStartupPlugin(string name, string implClass)
    {
        var obj = GameObject.Find(name);
        if (null != obj) {
            var startups = obj.GetComponents<Cs2LuaStartup>();
            for (int i = 0; i < startups.Length; ++i) {
                if (startups[i].LuaClassFileName == implClass.Replace(".", "__")) {
                    GameObject.Destroy(startups[i]);
                    break;
                }
            }
        }
    }
    public static void InstallTickPlugin(string name, string implClass, bool runAsLua)
    {
        var obj = GameObject.Find(name);
        if (null != obj) {
            var tick = obj.AddComponent<Cs2LuaTick>();
            if (null != tick) {
                tick.PluginType = runAsLua ? PluginType.Lua : PluginType.Native;
                tick.LuaClassFileName = implClass.Replace(".", "__");
                tick.Start();
            }
        }
    }
    public static void RemoveTickPlugin(string name, string implClass)
    {
        var obj = GameObject.Find(name);
        if (null != obj) {
            var ticks = obj.GetComponents<Cs2LuaTick>();
            for (int i = 0; i < ticks.Length; ++i) {
                if (ticks[i].LuaClassFileName == implClass.Replace(".", "__")) {
                    GameObject.Destroy(ticks[i]);
                    break;
                }
            }
        }
    }
}

internal class NativePluginProxy : IPluginProxy
{
    public void RegisterAttrExpression(string name, string implClass)
    {
        GameFramework.AttrCalc.DslCalculator.Register(name, new NativeAttrPluginFactory(implClass));
    }

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

    public void InstallStartupPlugin(string name, string implClass)
    {
        CommonPluginProxy.InstallStartupPlugin(name, implClass, false);
    }

    public void RemoveStartupPlugin(string name, string implClass)
    {
        CommonPluginProxy.RemoveStartupPlugin(name, implClass);
    }

    public void InstallTickPlugin(string name, string implClass)
    {
        CommonPluginProxy.InstallTickPlugin(name, implClass, false);
    }

    public void RemoveTickPlugin(string name, string implClass)
    {
        CommonPluginProxy.RemoveTickPlugin(name, implClass);
    }
}

internal class LuaPluginProxy : IPluginProxy
{
    public void RegisterAttrExpression(string name, string implClass)
    {
        GameFramework.AttrCalc.DslCalculator.Register(name, new LuaAttrPluginFactory(implClass));
    }

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

    public void InstallStartupPlugin(string name, string implClass)
    {
        CommonPluginProxy.InstallStartupPlugin(name, implClass, true);
    }

    public void RemoveStartupPlugin(string name, string implClass)
    {
        CommonPluginProxy.RemoveStartupPlugin(name, implClass);
    }

    public void InstallTickPlugin(string name, string implClass)
    {
        CommonPluginProxy.InstallTickPlugin(name, implClass, true);
    }

    public void RemoveTickPlugin(string name, string implClass)
    {
        CommonPluginProxy.RemoveTickPlugin(name, implClass);
    }
}