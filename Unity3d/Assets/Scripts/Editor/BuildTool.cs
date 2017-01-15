using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class BuildTool
{
    [MenuItem("工具/生成配表与DSL", false, 100)]
    static void BuildTable()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\dslcopy.bat");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0} Debug True", path));
        p.WaitForExit();
    }
    [MenuItem("工具/全部构建", false, 100)]
    static void BuildAll()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\buildall.bat");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0} Debug True", path));
        p.WaitForExit();
    }

    [MenuItem("工具/Build/BuildClient(mono)", false, 200)]
    static void BuildClient()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\buildclient.bat");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0} Debug True", path));
        p.WaitForExit();
    }

    [MenuItem("工具/Build/BuildServer(mono)", false, 200)]
    static void BuildServer()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\buildserver.bat");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0} Debug True", path));
        p.WaitForExit();
    }

    [MenuItem("工具/Build/定义CS2LUA_DEBUG宏", false, 400)]
    static void DefineCs2luaDebug()
    {
        string macro = "CS2LUA_DEBUG";
        string macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        if (macros.IndexOf(macro) < 0) {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, macros + ";" + macro);
        }
    }

    [MenuItem("工具/Build/取消CS2LUA_DEBUG宏", false, 400)]
    static void UndefineCs2luaDebug()
    {
        string macro = "CS2LUA_DEBUG";
        string macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        if (macros.IndexOf(macro) >=0) {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, macros.Replace(macro, string.Empty).Replace(";;", ";"));
        }
    }

    [MenuItem("工具/Build/定义CS2LUA_SERVER宏", false, 400)]
    static void DefineCs2luaServer()
    {
        string macro = "CS2LUA_SERVER";
        string macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        if (macros.IndexOf(macro) < 0) {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, macros + ";" + macro);
        }
    }

    [MenuItem("工具/Build/取消CS2LUA_SERVER宏", false, 400)]
    static void UndefineCs2luaServer()
    {
        string macro = "CS2LUA_SERVER";
        string macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        if (macros.IndexOf(macro) >= 0) {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, macros.Replace(macro, string.Empty).Replace(";;", ";"));
        }
    }

    [MenuItem("工具/Build/Cs2LuaNative", false, 500)]
    static void CallCs2LuaNative()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "CS2LUA_DEBUG");
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\cs2luanative.bat");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0} Debug", path));
        p.WaitForExit();
    }

    [MenuItem("工具/Build/Cs2Lua", false, 500)]
    static void CallCs2Lua()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "");
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\cs2lua.bat");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0} Debug", path));
        p.WaitForExit();
    }

    [MenuItem("工具/Server/启动服务器", false, 600)]
    static void RunServer()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\RunServer.cmd");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0}", path));
        p.WaitForExit();
    }

    [MenuItem("工具/Server/关闭服务器", false, 600)]
    static void StopServer()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\StopServer.cmd");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0}", path));
        p.WaitForExit();
    }
}
