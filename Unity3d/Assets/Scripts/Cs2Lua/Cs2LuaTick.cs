using UnityEngine;
using System.IO;
using System.Collections;
using System.Reflection;
using SLua;
using GameFramework.Plugin;

public class Cs2LuaTick : MonoBehaviour
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
        csObject = PluginManager.Instance.CreateTick(className);
            if (null != csObject) {
                csObject.Init(gameObject, this);
            }
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

    internal void Update()
    {
#if !CS2LUA_DEBUG
        if (PluginType == PluginType.Lua) {
            if (luaInited && null != update) {
                update.call(self);
            }
        } else {
#endif
            if (null != csObject) {
                csObject.Update();
            }
#if !CS2LUA_DEBUG
        }
#endif
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
        svr.luaState.doFile(LuaClassFileName);
        classObj = (LuaTable)svr.luaState[className];
        self = (LuaTable)((LuaFunction)classObj["__new_object"]).call();
        init = (LuaFunction)self["Init"];
        update = (LuaFunction)self["Update"];
        call = (LuaFunction)self["Call"];
        if (null != init) {
            init.call(self, gameObject, this);
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

    private ITickPlugin csObject;
    
    private LuaSvr svr;
    private LuaTable classObj;
    private LuaTable self;
    private LuaFunction init;
    private LuaFunction update;
    private LuaFunction call;
    private bool luaInited;
    private bool startupCalled;
}
