using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Plugin;
using SkillSystem;
using StorySystem;

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