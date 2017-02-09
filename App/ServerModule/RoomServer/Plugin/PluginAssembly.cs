using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

internal class PluginAssembly
{
    internal Assembly Assembly
    {
        get { return m_Assembly; }
    }
    internal void Init()
    {
        GameFramework.Plugin.PluginProxy.NativeProxy = new NativePluginProxy();
#if CS2LUA_DEBUG
        Cs2LuaScript.Program.Init(); 
#else
        Load("RoomCs2LuaScript.dll");
        if (null != m_Assembly) {
            var type = m_Assembly.GetType("Cs2LuaScript.Program");
            type.InvokeMember("Init", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, null);
        }
#endif
    }
    internal void Load(string file)
    {
        string path = GameFramework.HomePath.GetAbsolutePath(file);
        m_Assembly = Assembly.LoadFile(path);
    }

    private Assembly m_Assembly = null;

    internal static PluginAssembly Instance
    {
        get { return s_Instance; }
    }
    private static PluginAssembly s_Instance = new PluginAssembly();
}
