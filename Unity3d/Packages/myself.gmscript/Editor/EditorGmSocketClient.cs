using System;
using UnityEditor;
using UnityEngine;
using StoryScript;
using StoryScript.DslExpression;

// GM client for edit mode: connects out to the MCP socket server, no listener in editor.
// The editor channel and the runtime channel (Play mode) each keep their own connection;
// MCP distinguishes them by the channel field of the hello message.
[InitializeOnLoad]
internal static class EditorGmSocketClient
{
    private const string c_AutoStartKey = "UCRayTracing.GmSocket.EditorAutoStart";

    static EditorGmSocketClient()
    {
        // The editor connection serves editor DSL only. Runtime GM commands live on the
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
        GmRootScript.StopSocketClient();
        if (null == s_Client) {
            return;
        }
        EditorApplication.update -= Pump;
        s_Client.Stop();
        s_Client = null;
    }

    internal static bool IsRunning
    {
        get { return null != s_Client; }
    }
    internal static void Start()
    {
        if (null != s_Client) {
            return;
        }
        var client = new GmSocketClient("editor");
        client.Start();
        s_Client = client;
        EditorApplication.update -= Pump;
        EditorApplication.update += Pump;
        Debug.Log(string.Format("[GmSocket] editor connection started (MCP {0})", client.Endpoint));
    }
    internal static void Stop()
    {
        if (null == s_Client) {
            return;
        }
        EditorApplication.update -= Pump;
        s_Client.Stop();
        s_Client = null;
        Debug.Log("[GmSocket] editor connection stopped.");
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

    [MenuItem("Tools/GM Socket/Start Editor Connection", false, 100)]
    private static void MenuStart()
    {
        EditorPrefs.SetBool(c_AutoStartKey, true);
        Start();
    }
    [MenuItem("Tools/GM Socket/Start Editor Connection", true)]
    private static bool MenuStartValidate()
    {
        return null == s_Client;
    }
    [MenuItem("Tools/GM Socket/Stop Editor Connection", false, 101)]
    private static void MenuStop()
    {
        EditorPrefs.SetBool(c_AutoStartKey, false);
        Stop();
    }
    [MenuItem("Tools/GM Socket/Stop Editor Connection", true)]
    private static bool MenuStopValidate()
    {
        return null != s_Client;
    }

    private static GmSocketClient s_Client;
}
