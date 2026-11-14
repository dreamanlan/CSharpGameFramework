using System;
using UnityEditor;
using UnityEngine;
using StoryScript;
using StoryScript.DslExpression;

// GM listener for edit mode: same protocol as runtime, commands executed from EditorApplication.update.
// Default port 39528 (runtime uses 39527, so both can be used at the same time in Play mode).
[InitializeOnLoad]
internal static class EditorGmSocketServer
{
    internal const int c_DefaultPort = 39528;
    private const string c_AutoStartKey = "UCRayTracing.GmSocket.EditorAutoStart";
    private const string c_PortKey = "UCRayTracing.GmSocket.EditorPort";

    static EditorGmSocketServer()
    {
        // The editor listener serves editor DSL only. Runtime GM commands live on the
        // runtime channel and are deliberately not merged into the editor Calculator.
        GmCommandDispatcher.EditorExecutor = ExecEditor;
        EditorApplication.delayCall -= OnEditorReady;
        EditorApplication.delayCall += OnEditorReady;
    }

    private static void OnEditorReady()
    {
        AssemblyReloadEvents.beforeAssemblyReload -= StopSilently;
        AssemblyReloadEvents.beforeAssemblyReload += StopSilently;
        EditorApplication.quitting -= StopSilently;
        EditorApplication.quitting += StopSilently;

        if (EditorPrefs.GetBool(c_AutoStartKey, true)) {
            Start();
        }
    }
    private static void StopSilently()
    {
        if (null == s_Server) {
            return;
        }
        EditorApplication.update -= Pump;
        s_Server.Stop();
        s_Server = null;
    }

    internal static bool IsRunning
    {
        get { return null != s_Server; }
    }
    internal static int Port
    {
        get {
            int port = EditorPrefs.GetInt(c_PortKey, c_DefaultPort);
            return port > 0 ? port : c_DefaultPort;
        }
    }
    internal static void Start()
    {
        if (null != s_Server) {
            return;
        }
        var server = new GmSocketServer(Port);
        if (!server.Start()) {
            return;
        }
        s_Server = server;
        EditorApplication.update -= Pump;
        EditorApplication.update += Pump;
        Debug.Log(string.Format("[GmSocket] editor listener started on 127.0.0.1:{0}", server.Port));
    }
    internal static void Stop()
    {
        if (null == s_Server) {
            return;
        }
        EditorApplication.update -= Pump;
        s_Server.Stop();
        s_Server = null;
        Debug.Log("[GmSocket] editor listener stopped.");
    }

    // Editor DSL evaluation: editor/resource APIs only. Runtime GM commands are NOT
    // registered here -- the editor channel must not be able to run runtime APIs, and
    // merging the shared registry just duplicated factories into an editor-only table.
    // A dedicated script name is used so a socket call never overwrites the 
    // script loaded by editor_startup.dsl / the resource command window.
    internal static string ExecEditor(string code)
    {
        return GmDslEval.Eval(EditorCalculator, code, BoxedValue.NullObject, BoxedValue.NullObject, c_EditorScriptName);
    }

    private const string c_EditorScriptName = "__gm_socket_edit__";

    private static DslCalculator EditorCalculator
    {
        get {
            // Share the very same Calculator used by the editor command window and by
            // editor_startup.dsl (EditorGmCommandWindow.RunOnEditorStartup), so global
            // variables/functions are visible to both. GetCommandCalculator() already
            // calls InitCalculator (editor APIs + EditorDslApi + project OnRegisterApi)
            // and is a lazy singleton that EditorGmCommandWindow may reset, so always fetch
            // it fresh instead of caching the instance.
            return EditorDslProcessorUtility.GetCommandCalculator();
        }
    }
    private static void Pump()
    {
        GmCommandDispatcher.Pump();
    }

    [MenuItem("Tools/GM Socket/Start Editor Listener", false, 100)]
    private static void MenuStart()
    {
        EditorPrefs.SetBool(c_AutoStartKey, true);
        Start();
    }
    [MenuItem("Tools/GM Socket/Start Editor Listener", true)]
    private static bool MenuStartValidate()
    {
        return null == s_Server;
    }
    [MenuItem("Tools/GM Socket/Stop Editor Listener", false, 101)]
    private static void MenuStop()
    {
        EditorPrefs.SetBool(c_AutoStartKey, false);
        Stop();
    }
    [MenuItem("Tools/GM Socket/Stop Editor Listener", true)]
    private static bool MenuStopValidate()
    {
        return null != s_Server;
    }

    private static GmSocketServer s_Server;
}
