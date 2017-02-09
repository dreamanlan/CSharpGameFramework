using System;
using System.IO;
using System.Collections;
using System.Reflection;
using GameFramework;
using GameFramework.Plugin;

internal class Cs2LuaScene
{
    internal string ClassName;
    internal void Init(Scene scene)
    {
        string className = ClassName;
        csObject = PluginManager.Instance.CreateScene(className);
        if (null != csObject) {
            csObject.Init(scene);
        }
    }

    internal void ChangeScene(Scene scene)
    {
        if (null != csObject) {
            csObject.ChangeScene(scene);
        }
    }

    internal void Tick(Scene scene)
    {
        if (null != csObject) {
            csObject.Tick(scene);
        }
    }

    private void CallScript(object[] args)
    {
        if (args.Length > 0) {
            if (null != csObject) {
                string name = args[0] as string;
                ArrayList arr = new ArrayList(args);
                arr.RemoveAt(0);
                if (args.Length == 1)
                    csObject.Call(name);
                else if (args.Length == 2)
                    csObject.Call(name, args[1]);
                else
                    csObject.Call(name, arr.ToArray());
            }
        }
    }

    private IScenePlugin csObject;
}
