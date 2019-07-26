using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SLua;
using GameFramework.Plugin;

public class Cs2LuaStartup : MonoBehaviour
{
    public PluginType PluginType;
    public string LuaClassFileName;

    public void Startup()
    {
        if (startupCalled)
            return;
        startupCalled = true;

        string className = LuaClassFileName.Replace("__", ".");
#if !CS2LUA_DEBUG
        if (PluginType == PluginType.Lua) {
            luaInited = false;
            if(Cs2LuaAssembly.Instance.LuaInited)
                DoStartupLua(className);
            else
                StartCoroutine(StartupLua(className));
        } else {
#endif
        csObject = PluginManager.Instance.CreateStartup(className);
        csObject.Start(gameObject, this);
#if !CS2LUA_DEBUG
        }
#endif
    }

    internal void Start()
    {
        if (!string.IsNullOrEmpty(LuaClassFileName)) {
            Startup();
        }
    }

    private IEnumerator StartupLua(string className)
    {
        while (!Cs2LuaAssembly.Instance.LuaInited)
            yield return null;
        DoStartupLua(className);
        yield return null;
    }

    private void DoStartupLua(string className)
    {
        svr = Cs2LuaAssembly.Instance.LuaSvr;
		string fileName = LuaClassFileName.ToLower();
        var sb = new System.Text.StringBuilder();
        sb.Append("require ");
        sb.Append('"');
        sb.Append(fileName);
        sb.Append('"');
        svr.luaState.doString(sb.ToString());
        classObj = (LuaTable)svr.luaState[className];
        self = (LuaTable)((LuaFunction)classObj["__new_object"]).call();
        start = (LuaFunction)self["Start"];
        call = (LuaFunction)self["Call"];
        if (null != start) {
            start.call(self, gameObject, this);
        }
        luaInited = true;
    }

    private void CallScript(object[] args)
    {
        if (args.Length > 0) {
#if !CS2LUA_DEBUG
            if (PluginType == PluginType.Lua) {
                if (luaInited && null != call) {
                    ArrayList arr = new ArrayList();
                    arr.Add(self);
                    arr.AddRange(args);
                    call.call(arr.ToArray());
                }
            } else {
#endif
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
#if !CS2LUA_DEBUG
            }
#endif
        }
    }

    private IStartupPlugin csObject;
    
    private LuaSvr svr;
    private LuaTable classObj;
    private LuaTable self;
    private LuaFunction start;
    private LuaFunction call;
    private bool luaInited;
    private bool startupCalled;
}
