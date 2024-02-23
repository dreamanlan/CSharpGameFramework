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
    Script,
}

public class PluginProxyHelper : MonoBehaviour
{
	internal void Awake () 
    {
        PluginProxy.NativeProxy = m_NativePluginProxy;
        PluginProxy.ScriptProxy = m_ScriptPluginProxy;
	}

    private NativePluginProxy m_NativePluginProxy = new NativePluginProxy();
    private ScriptPluginProxy m_ScriptPluginProxy = new ScriptPluginProxy();
}

internal static class CommonPluginProxy
{
    public static void InstallStartupPlugin(string name, string implClass, bool runAsScript)
    {
        var obj = GameObject.Find(name);
        if (null != obj) {
            var startup = obj.AddComponent<PluginProxyStartup>();
            if (null != startup) {
                startup.PluginType = runAsScript ? PluginType.Script : PluginType.Native;
                startup.ScriptClassFileName = implClass.Replace(".","__");
                startup.Start();
            }
        }
    }
    public static void RemoveStartupPlugin(string name, string implClass)
    {
        var obj = GameObject.Find(name);
        if (null != obj) {
            var startups = obj.GetComponents<PluginProxyStartup>();
            for (int i = 0; i < startups.Length; ++i) {
                if (startups[i].ScriptClassFileName == implClass.Replace(".", "__")) {
                    GameObject.Destroy(startups[i]);
                    break;
                }
            }
        }
    }
    public static void InstallTickPlugin(string name, string implClass, bool runAsScript)
    {
        var obj = GameObject.Find(name);
        if (null != obj) {
            var tick = obj.AddComponent<PluginProxyTick>();
            if (null != tick) {
                tick.PluginType = runAsScript ? PluginType.Script : PluginType.Native;
                tick.ScriptClassFileName = implClass.Replace(".", "__");
                tick.Start();
            }
        }
    }
    public static void RemoveTickPlugin(string name, string implClass)
    {
        var obj = GameObject.Find(name);
        if (null != obj) {
            var ticks = obj.GetComponents<PluginProxyTick>();
            for (int i = 0; i < ticks.Length; ++i) {
                if (ticks[i].ScriptClassFileName == implClass.Replace(".", "__")) {
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

    public void RegisterStoryCommand(string name, string doc, string implClass)
    {
        StoryCommandManager.Instance.RegisterCommandFactory(name, doc, new NativeStoryCommandFactory(implClass), true);
    }

    public void RegisterStoryFunction(string name, string doc, string implClass)
    {
        StoryFunctionManager.Instance.RegisterFunctionFactory(name, doc, new NativeStoryFunctionFactory(implClass), true);
    }

    public void RegisterSimpleStoryCommand(string name, string doc, string implClass)
    {
        StoryCommandManager.Instance.RegisterCommandFactory(name, doc, new NativeSimpleStoryCommandFactory(implClass), true);
    }

    public void RegisterSimpleStoryFunction(string name, string doc, string implClass)
    {
        StoryFunctionManager.Instance.RegisterFunctionFactory(name, doc, new NativeSimpleStoryValueFactory(implClass), true);
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

internal class ScriptPluginProxy : IPluginProxy
{
    public void RegisterAttrExpression(string name, string implClass)
    {
        GameFramework.AttrCalc.DslCalculator.Register(name, new ScriptAttrPluginFactory(implClass));
    }

    public void RegisterSkillTrigger(string name, string implClass)
    {
        SkillTrigerManager.Instance.RegisterTrigerFactory(name, new ScriptSkillTriggerFactory(implClass), true);
    }

    public void RegisterStoryCommand(string name, string doc, string implClass)
    {
        StoryCommandManager.Instance.RegisterCommandFactory(name, doc, new ScriptStoryCommandFactory(implClass), true);
    }

    public void RegisterStoryFunction(string name, string doc, string implClass)
    {
        StoryFunctionManager.Instance.RegisterFunctionFactory(name, doc, new ScriptSimpleStoryValueFactory(implClass), true);
    }

    public void RegisterSimpleStoryCommand(string name, string doc, string implClass)
    {
        StoryCommandManager.Instance.RegisterCommandFactory(name, doc, new ScriptSimpleStoryCommandFactory(implClass), true);
    }

    public void RegisterSimpleStoryFunction(string name, string doc, string implClass)
    {
        StoryFunctionManager.Instance.RegisterFunctionFactory(name, doc, new ScriptSimpleStoryValueFactory(implClass), true);
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