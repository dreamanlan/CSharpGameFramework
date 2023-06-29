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

    [MenuItem("工具/Build/定义CS2DSL_DEBUG宏", false, 300)]
    static void DefineCs2DslDebug()
    {
        string macro = "CS2DSL_DEBUG";
        string macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        if (macros.IndexOf(macro) < 0) {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, macros + ";" + macro);
        }
    }

    [MenuItem("工具/Build/取消CS2DSL_DEBUG宏", false, 300)]
    static void UndefineCs2DslDebug()
    {
        string macro = "CS2DSL_DEBUG";
        string macros = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
        if (macros.IndexOf(macro) >=0) {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, macros.Replace(macro, string.Empty).Replace(";;", ";"));
        }
    }

    [MenuItem("工具/Server/启动服务器", false, 500)]
    static void RunServer()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\RunServer.cmd");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0}", path));
        p.WaitForExit();
    }

    [MenuItem("工具/Server/关闭服务器", false, 500)]
    static void StopServer()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "..\\..\\..\\StopServer.cmd");
        System.Diagnostics.Process p = System.Diagnostics.Process.Start("cmd", string.Format("/c call {0}", path));
        p.WaitForExit();
    }
}
