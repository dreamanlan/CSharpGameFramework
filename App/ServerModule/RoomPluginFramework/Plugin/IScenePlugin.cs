using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

public interface IScenePlugin
{
    void Init(Scene scene);
    void ChangeScene(Scene scene);
    void Tick(Scene scene);
    void Call(string name, params object[] args);
}
public interface IScenePluginFactory
{
    IScenePlugin CreateInstance();
}
public sealed class ScenePluginFactory<T> : IScenePluginFactory where T : IScenePlugin, new()
{
    public IScenePlugin CreateInstance()
    {
        return new T();
    }
}
