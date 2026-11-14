using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StoryScript;
using StoryScript.DslExpression;

// DSL evaluation: wrap into script{ ... }, run synchronously and return the last statement value.
public static class GmDslEval
{
    // scriptName lets callers that share a Calculator avoid overwriting each other's
    // loaded "main" script (e.g. the GM socket edit channel vs the resource command window).
    public static string Eval(DslCalculator calc, string code, BoxedValue obj, BoxedValue item, string scriptName = "main")
    {
        if (null == calc) {
            throw new ArgumentNullException("calc");
        }
        if (string.IsNullOrEmpty(code)) {
            throw new ArgumentException("code is empty");
        }
        string text = "script{ " + code + "; };";
        string loadError = null;
        var file = new Dsl.DslFile();
        if (!file.LoadFromString(text, msg => { if (null == loadError) loadError = msg; })) {
            throw new Exception("dsl load failed: " + (null != loadError ? loadError : text));
        }
        if (file.DslInfos.Count <= 0) {
            throw new Exception("dsl has no info");
        }
        var info = file.DslInfos[0];
        var func = info as Dsl.FunctionData;
        if (null == func) {
            var statement = info as Dsl.StatementData;
            if (null != statement) {
                func = statement.First.AsFunction;
            }
        }
        if (null == func) {
            throw new Exception("dsl info is not a function call");
        }
        calc.LoadDsl(scriptName, new string[] { "$obj", "$item" }, func);
        var r = calc.Calc(scriptName, obj, item);
        return ToText(r);
    }
    public static string ToText(BoxedValue val)
    {
        if (val.IsNullObject) {
            return "null";
        }
        var s = val.ToString();
        return null == s ? "null" : s;
    }
}

// Command entry points shared by runtime and editor (always run on the main thread).
public static class GmCommandApi
{
    // Needed by every channel, including the editor listener (ping / logs / clearlogs).
    // Deliberately does NOT touch the GM command registry or the story runtime.
    public static void EnsureCommonInit()
    {
        if (s_CommonInited) {
            return;
        }
        s_CommonInited = true;

        GmLogBuffer.Install();
        GmCommandDispatcher.InfoProvider = BuildInfo;
    }

    // Full init for the runtime channel only (exec / console). The editor channel must
    // not pull in the GM command set or the story runtime just to answer an edit request.
    public static void EnsureInit()
    {
        EnsureCommonInit();
        if (s_RuntimeInited) {
            return;
        }
        s_RuntimeInited = true;

        try {
            GmCommands.ClientGmStorySystem.Instance.Init();
        }
        catch (Exception ex) {
            LogSystem.Error("GmCommandApi init gm commands exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }

        GmCommandDispatcher.DslExecutor = ExecDsl;
        GmCommandDispatcher.ConsoleExecutor = ExecConsole;
    }

    // The GM socket "exec" channel keeps one Calculator across requests so global
    // variables/functions defined by an earlier call survive into the next one.
    // It deliberately does NOT reuse the story-based GM script runtime: stories run
    // across ticks and have no synchronous return value, which is exactly what exec
    // needs. Both still share the same API registry, so every GM command/function is
    // available on both sides.
    private static DslCalculator s_ExecCalculator;
    public static DslCalculator ExecCalculator
    {
        get
        {
            if (null == s_ExecCalculator) {
                var calc = new DslCalculator();
                calc.ApiRegistry = DslCalculatorHost.GetSharedApiRegistry();
                calc.OnLog = msg => { Debug.LogWarning("[GmSocket] " + msg); };
                s_ExecCalculator = calc;
            }
            return s_ExecCalculator;
        }
    }

    // GM DSL evaluation against the shared registry (all GM commands/functions live there).
    public static string ExecDsl(string code)
    {
        return GmDslEval.Eval(ExecCalculator, code, BoxedValue.NullObject, BoxedValue.NullObject);
    }

    // DebugConsole channel. This is the one MCP should use to reach the GM story script
    // runtime: commands like "gm <dsl>" / "command <dsl>" are forwarded by DebugConsole
    // to GmRootScript.OnExecCommand -> ClientGmStorySystem.StartStory. Those run
    // asynchronously across ticks and report through logs rather than a return value.
    // In edit mode there is no DebugConsole instance, so fall back to synchronous DSL eval.
    public static string ExecConsole(string cmd)
    {
        if (DebugConsole.HasInstance) {
            var r = DebugConsole.ExecuteForResult(cmd);
            return null == r ? "null" : r.ToString();
        }
        return ExecDsl(cmd);
    }

    public static Dictionary<string, object> BuildInfo()
    {
        var d = new Dictionary<string, object>();
        d["mode"] = Application.isEditor ? (Application.isPlaying ? "editor-play" : "editor") : "player";
        d["unity"] = Application.unityVersion;
        d["platform"] = Application.platform.ToString();
        d["isEditor"] = Application.isEditor;
        d["isPlaying"] = Application.isPlaying;
        d["productName"] = Application.productName;
        d["frame"] = Time.frameCount;
        d["time"] = Time.realtimeSinceStartup;
        d["pendingRequests"] = GmCommandDispatcher.PendingCount;
        try {
            var scene = SceneManager.GetActiveScene();
            d["scene"] = scene.IsValid() ? scene.name : string.Empty;
        }
        catch {
            d["scene"] = string.Empty;
        }
        try {
            using (var p = System.Diagnostics.Process.GetCurrentProcess()) {
                d["pid"] = p.Id;
            }
        }
        catch {
        }
        return d;
    }

    private static bool s_CommonInited;
    private static bool s_RuntimeInited;
}
