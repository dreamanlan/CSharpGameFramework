using System;
using System.IO;
using System.Collections;
using System.Reflection;
using SLua;
using GameFramework;
using GameFramework.Plugin;

internal class Cs2LuaScene
{
    internal PluginType PluginType = PluginType.Native;
    internal string LuaClassFileName;

    internal void Init(Scene scene, Cs2LuaEnvironment env)
    {
        if (startupCalled)
            return;
        startupCalled = true;

        string className = LuaClassFileName.Replace("__", ".");
#if !CS2LUA_DEBUG
        if (PluginType == PluginType.Lua) {
            luaInited = false;
            StartupLua(className, scene, env);
        } else {
#endif
            csObject = PluginManager.Instance.CreateScene(className);
            if (null != csObject) {
                csObject.Init(scene);
            }
#if !CS2LUA_DEBUG
        }
#endif
    }

    internal void ChangeScene(Scene scene)
    {
#if !CS2LUA_DEBUG
        if (PluginType == PluginType.Lua) {
            if (luaInited && null != changeScene) {
                changeScene.call(self);
            }
        } else {
#endif
            if (null != csObject) {
                csObject.ChangeScene(scene);
            }
#if !CS2LUA_DEBUG
        }
#endif
    }

    internal void Tick(Scene scene)
    {
#if !CS2LUA_DEBUG
        if (PluginType == PluginType.Lua) {
            if (luaInited && null != tick) {
                tick.call(self);
            }
        } else {
#endif
            if (null != csObject) {
                csObject.Tick(scene);
            }
#if !CS2LUA_DEBUG
        }
#endif
    }

    private void StartupLua(string className, Scene scene, Cs2LuaEnvironment env)
    {
        svr = env.LuaSvr;
        svr.luaState.doFile(LuaClassFileName);
        classObj = (LuaTable)svr.luaState[className];
        self = (LuaTable)((LuaFunction)classObj["__new_object"]).call();
        init = (LuaFunction)self["Init"];
        changeScene = (LuaFunction)self["ChangeScene"];
        tick = (LuaFunction)self["Tick"];
        call = (LuaFunction)self["Call"];
        if (null != init) {
            init.call(self, scene);
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

    private IScenePlugin csObject;
    
    private LuaSvr svr;
    private LuaTable classObj;
    private LuaTable self;
    private LuaFunction init;
    private LuaFunction changeScene;
    private LuaFunction tick;
    private LuaFunction call;
    private bool luaInited;
    private bool startupCalled;
}
