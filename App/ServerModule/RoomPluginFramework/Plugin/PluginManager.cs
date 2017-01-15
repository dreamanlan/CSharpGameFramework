using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GameFramework.Plugin;

public interface IObjectPluginFactory
{
    object CreateInstance();
}
public sealed class ObjectPluginFactory<T> : IObjectPluginFactory where T : new()
{
    public object CreateInstance()
    {
        return new T();
    }
}
public class PluginManager
{
    public object CreateObject(string name)
    {
        IObjectPluginFactory factory;
        if (m_ObjectFactories.TryGetValue(name, out factory)) {
            return factory.CreateInstance();
        }
        return null;
    }
    public IScenePlugin CreateScene(string name)
    {
        IScenePluginFactory factory;
        if (m_SceneFactories.TryGetValue(name, out factory)) {
            return factory.CreateInstance();
        }
        return null;
    }
    public void RegisterObjectFactory(string name, IObjectPluginFactory factory)
    {
        if (!m_ObjectFactories.ContainsKey(name)) {
            m_ObjectFactories.Add(name, factory);
        } else {
            m_ObjectFactories[name] = factory;
        }
    }
    public void RegisterSceneFactory(string name, IScenePluginFactory factory)
    {
        if (!m_SceneFactories.ContainsKey(name)) {
            m_SceneFactories.Add(name, factory);
        } else {
            m_SceneFactories[name] = factory;
        }
    }

    private Dictionary<string, IObjectPluginFactory> m_ObjectFactories = new Dictionary<string, IObjectPluginFactory>();
    private Dictionary<string, IScenePluginFactory> m_SceneFactories = new Dictionary<string, IScenePluginFactory>();

    public static PluginManager Instance
    {
        get { return s_Instance; }
    }
    private static PluginManager s_Instance = new PluginManager();
};
