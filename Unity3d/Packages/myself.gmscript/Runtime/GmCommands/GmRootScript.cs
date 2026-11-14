using GmCommands;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using StoryScript;

public sealed class GmRootScript : MonoBehaviour
{
    public delegate GameObject LoadResDelegation(string resPath);
    public delegate void DebugConsoleShowHideDelegation();

    void OnEnable()
    {
    }
    void OnDisable()
    {
        DeinitAndroidReceiver();
        DeinitSocketServer();
    }
    void Start()
    {
        TryInit();
    }
    void Update()
    {
        try {
            TimeUtility.UpdateGfxTime(Time.time, Time.realtimeSinceStartup, Time.timeScale);
            long curTime = TimeUtility.GetLocalRealMilliseconds();
            if (m_ClipboardInterval > 0) {
                DetectClipboardCommand();
                if (m_LastClipboardTime + m_ClipboardInterval < curTime) {
                    if (m_NeedCheckClipboard) {
                        m_NeedCheckClipboard = false;

                        string cmd = GUIUtility.systemCopyBuffer.Trim();
                        if (cmd.StartsWith(c_clipboard_cmd_tag)) {
                            DebugConsole.Execute(cmd.Substring(c_clipboard_cmd_tag.Length));
                        }
                        else if (!string.IsNullOrEmpty(s_clipboard_cmd_tag) && cmd.StartsWith(s_clipboard_cmd_tag)) {
                            DebugConsole.Execute(cmd.Substring(s_clipboard_cmd_tag.Length));
                        }
                    }
                    m_LastClipboardTime = curTime;
                }
                else if (m_LastClipboardTime > curTime) {
                    m_LastClipboardTime = 0;
                }
            }
            if (null != m_AndroidReceiver || m_SocketServerInited) {
                HandleCommand();
            }
            // Execute requests coming from the socket / editor channel on the main thread.
            GmCommandDispatcher.Pump();
            ClientGmStorySystem.Instance.Tick();
            if (GmCommands.ProfilerForGM.IsStarted()) {
                GmCommands.ProfilerForGM.Update();
            }
            HandleLog();
            if (m_LastTaskCleanupTime + c_TaskCleanupInterval < curTime) {
                CleanupCompletedTasks();
            }
        }
        catch (Exception ex) {
            LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void DetectClipboardCommand()
    {
        bool checkCtrlV = false;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        checkCtrlV = true;
#else
        if (StoryScript.StoryConfigManager.Instance.IsDebug) {
            checkCtrlV=true;
        }
#endif
        if (checkCtrlV) {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand)) && Input.GetKey(KeyCode.V)
                && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)
                && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)) {
                m_NeedCheckClipboard = true;
            }
        }
    }
    private void TryInitGmRoot()
    {
        if (m_Inited) return;
        m_Inited = true;

        s_application_identifier = Application.identifier;
        s_clipboard_cmd_tag = string.Format("[{0}]:", s_application_identifier);

        ClientGmStorySystem.Instance.Init();
        StartupScript.TryInit();

        m_StoryDocs = DslCalculatorHost.GetSharedApiRegistry().ApiDocs;

#if UNITY_EDITOR
        InitSocketServer();
#elif UNITY_STANDALONE
#if DEVELOPMENT_BUILD
        InitSocketServer();
#endif
#elif UNITY_ANDROID
#if DEVELOPMENT_BUILD
        InitAndroidReceiver();
#endif
#else
#if DEVELOPMENT_BUILD
        InitSocketServer();
#endif
#endif
    }

    private void SetClipboardInterval(int interval)
    {
        m_ClipboardInterval = interval;
    }
    private void InitAndroidReceiver()
    {
#if UNITY_ANDROID
        if (null == m_AndroidReceiver) {
            m_AndroidReceiver = new BroadcastReceiverHandler();
        }
        m_AndroidReceiver.RegisterBroadcastReceiver("com.unity3d.command");
#endif
    }
    private void DeinitAndroidReceiver()
    {
#if UNITY_ANDROID
        if (null != m_AndroidReceiver) {
            m_AndroidReceiver.UnregisterBroadcastReceiver();
        }
#endif
    }
    private void InitSocketServer()
    {
        if (m_SocketServerInited) return;
        m_SocketServerInited = true;
        s_SocketServer = new GmSocketServer(s_SocketServerPort);
        s_SocketServer.Start();
        LogSystem.Warn("GmSocketServer started on port {0}", s_SocketServerPort);
    }
    private void DeinitSocketServer()
    {
        if (m_SocketServerInited && null != s_SocketServer) {
            s_SocketServer.Stop();
            s_SocketServer = null;
            m_SocketServerInited = false;
            LogSystem.Warn("GmSocketServer stopped.");
        }
    }

    private void OnResetDsl()
    {
        try {
            ResetGmCode();
            ClientGmStorySystem.Instance.ClearContextVariables();
            ClientGmStorySystem.Instance.Reset();
            StoryScript.StoryConfigManager.Instance.Clear();
            LogSystem.Warn("ResetDsl finish.");
        }
        catch (Exception ex) {
            LogSystem.Error("Exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void OnExecCommand(string cmd)
    {
        try {
            ClientGmStorySystem.Instance.Reset();
            ClientGmStorySystem.Instance.LoadStoryText(Encoding.UTF8.GetBytes("@@delimiter(string,\"[[\",\"]]\");script(main){onmessage(\"start\"){" + cmd + "}}"));
            ClientGmStorySystem.Instance.StartStory("main");
            LogSystem.Warn("ExecCommand {0} finish.", cmd);
        }
        catch (Exception ex) {
            LogSystem.Error("ExecCommand exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }
    private void OnExecScript(string scriptFile)
    {
        try {
            if (string.IsNullOrEmpty(scriptFile)) {
                if (string.IsNullOrEmpty(m_LocalGmFile)) {
                    if (Application.platform == RuntimePlatform.Android) {
                        scriptFile = "/data/local/tmp/gm.dsl";
                    }
                    else {
                        scriptFile = "gm.dsl";
                    }
                    m_LocalGmFile = scriptFile;
                }
            }
            else {
                m_LocalGmFile = scriptFile;
            }
            RunLocalGmFile();

            LogSystem.Warn("ExecScript {0} finish.", m_LocalGmFile);
        }
        catch (Exception ex) {
            LogSystem.Error("ExecScript exception:{0}\n{1}", ex.Message, ex.StackTrace);
        }
    }

    private void ResetGmCode()
    {
        m_LocalGmFile = "";
    }
    private void RunLocalGmFile()
    {
        if (!string.IsNullOrEmpty(m_LocalGmFile)) {

            if (File.Exists(m_LocalGmFile)) {
                ClientGmStorySystem.Instance.Reset();
                ClientGmStorySystem.Instance.LoadStory(m_LocalGmFile);
                ClientGmStorySystem.Instance.StartStory("main");
            }
            else {
                var path = Path.Combine(ScriptPath, m_LocalGmFile);
                if (File.Exists(path)) {
                    ClientGmStorySystem.Instance.Reset();
                    ClientGmStorySystem.Instance.LoadStory(path);
                    ClientGmStorySystem.Instance.StartStory("main");
                }
                else {
                    var bytes = LoadFileFromStreamingAssets(m_LocalGmFile);
                    if (null != bytes) {
                        ClientGmStorySystem.Instance.Reset();
                        ClientGmStorySystem.Instance.LoadStoryText(bytes);
                        ClientGmStorySystem.Instance.StartStory("main");
                    }
                    else {
                        m_LocalGmFile = "";
                    }
                }
            }
        }
    }

    private long m_LastTaskCleanupTime;
    private int m_ClipboardInterval;
    private long m_LastClipboardTime;
    private bool m_NeedCheckClipboard;
    private BroadcastReceiverHandler m_AndroidReceiver;
    private bool m_SocketServerInited;

    private SortedList<string, string> m_StoryDocs;
    private string m_LocalGmFile = string.Empty;
    private bool m_Inited = false;

    public static LoadResDelegation OnLoadRes;
    public static DebugConsoleShowHideDelegation OnConsoleShow;
    public static DebugConsoleShowHideDelegation OnConsoleHide;

    public static string ScriptPath
    {
        get {
            if (Application.platform == RuntimePlatform.Android) {
                return "/data/local/tmp/";
            }
            else {
                return Application.persistentDataPath + "/";
            }
        }
    }
    public static SortedList<string, string> StoryDocs
    {
        get {
            var gmScp = GetGmRootScript();
            if (null == gmScp) {
                return null;
            }
            return GetGmRootScript().m_StoryDocs;
        }
    }
    public static GameObject GameObj
    {
        get {
            return s_GameObj;
        }
    }
    public static void TryLoad()
    {
        if (null == s_GameObj) {
            var prefab = LoadRes("GmScript");
            if (prefab == null) {
                LogSystem.Error("[GmScript] GmScript.prefab load failed");
                // fallback: create a bare object; TryInit below picks it up
                // via GameObject.Find("GmScript")
                new GameObject("GmScript");
            }
            else {
                var gobj = GameObject.Instantiate<GameObject>(prefab);
                gobj.name = "GmScript";
            }
            TryInit();
        }
    }
    public static void TryInit()
    {
        if (null == s_GameObj) {
            s_GameObj = GameObject.Find("GmScript");
            if (null != s_GameObj) {
                GameObject.DontDestroyOnLoad(s_GameObj);
            }
            else {
                s_GameObj = new GameObject("GmScript");
                GameObject.DontDestroyOnLoad(s_GameObj);
            }
        }
        if (null != s_GameObj) {
            var gmRootScript = GetGmRootScript();
            if (null == gmRootScript) {
                LogSystem.Error("[GmScript] GmScript object is missing GmRootScript component");
                return;
            }
            gmRootScript.TryInitGmRoot();
        }
    }
    public static void OnDebugConsoleShow()
    {
        if (null != OnConsoleShow)
            OnConsoleShow();
    }
    public static void OnDebugConsoleHide()
    {
        if (null != OnConsoleHide)
            OnConsoleHide();
    }
    public static void ListenClipboard(int interval)
    {
        if (null != s_GameObj) {
            var gmScp = GetGmRootScript();
            if (null == gmScp) {
                return;
            }
            gmScp.SetClipboardInterval(interval);
        }
    }
    public static void ListenAndroid()
    {
        if (null != s_GameObj) {
            var gmScp = GetGmRootScript();
            if (null == gmScp) {
                return;
            }
            gmScp.InitAndroidReceiver();
        }
    }
    public static void ListenSocket(int port)
    {
        if (null != s_GameObj) {
            s_SocketServerPort = port;
            var gmScp = GetGmRootScript();
            if (null == gmScp) {
                return;
            }
            gmScp.DeinitSocketServer();
            gmScp.InitSocketServer();
        }
    }
    public static void SendCommand(string cmd)
    {
        lock (s_Lock) {
            if (s_CommandQueue.Count < c_max_command_in_queue) {
                s_CommandQueue.Enqueue(cmd);
            }
            else {
                LogSystem.Error("command queue overflow !");
            }
        }
    }
    public static void SendLog(string log)
    {
        lock (s_Lock) {
            s_LogQueue.Enqueue(log);
        }
    }

    private static GameObject LoadRes(string res)
    {
        if (null != OnLoadRes) {
            return OnLoadRes(res);
        }
        return null;
    }
    private static void HandleCommand()
    {
        lock (s_Lock) {
            if (s_CommandQueue.Count > 0) {
                string cmd = s_CommandQueue.Dequeue();
                DebugConsole.Execute(cmd);
            }
        }
    }
    private static void HandleLog()
    {
        lock (s_Lock) {
            if (s_LogQueue.Count > 0) {
                string log = s_LogQueue.Dequeue();
                if (log.Length > 1024) {
                    var lines = log.Split('\n');
                    int ct = 0;
                    var sb = new StringBuilder();
                    sb.AppendLine();
                    foreach (var line in lines) {
                        var logLine = line.TrimEnd();
                        LogSystem.Warn("{0}", logLine);
                        sb.AppendLine(logLine);
                        ++ct;
                        if (ct % 10 == 0) {
                            LogSystem.Warn("{0}", sb.ToString());
                            sb.Length = 0;
                            sb.AppendLine();
                            System.Threading.Thread.Sleep(0);
                        }
                    }
                }
                else {
                    LogSystem.Warn("{0}", log);
                }
            }
        }
    }
    private static GmRootScript GetGmRootScript()
    {
        var obj = s_GameObj;
        Debug.Assert(null != obj);
        if (null == obj) {
            return null;
        }
        var scp = obj.GetComponent<GmRootScript>();
        return scp;
    }
    private static byte[] LoadFileFromStreamingAssets(string file)
    {
        if (Application.platform == RuntimePlatform.Android) {
            using (var androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (var androidJavaActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity")) {
                    if (null != androidJavaActivity) {
                        using (var assetManager = androidJavaActivity.Call<AndroidJavaObject>("getAssets")) {
                            if (null != assetManager) {
                                using (var inputStream = assetManager.Call<AndroidJavaObject>("open", file)) {
                                    if (null != inputStream) {
                                        int available = inputStream.Call<int>("available");
                                        System.IntPtr buffer = AndroidJNI.NewSByteArray(available);
                                        System.IntPtr javaClass = AndroidJNI.FindClass("java/io/InputStream");
                                        System.IntPtr javaMethodID = AndroidJNIHelper.GetMethodID(javaClass, "read", "([B)I");
                                        int read = AndroidJNI.CallIntMethod(inputStream.GetRawObject(), javaMethodID,
                                            new[] { new jvalue() { l = buffer } });
                                        sbyte[] sbytes = AndroidJNI.FromSByteArray(buffer);
                                        AndroidJNI.DeleteLocalRef(buffer);
                                        inputStream.Call("close");
                                        inputStream.Dispose();
                                        byte[] bytes = new byte[sbytes.Length];
                                        Array.Copy(bytes, sbytes, bytes.Length);
                                        return bytes;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else {
            string assembly = Path.Combine(Application.streamingAssetsPath, file);
            if (File.Exists(assembly)) {
                var bytes = File.ReadAllBytes(assembly);
                return bytes;
            }
        }
        return null;
    }

    internal static int GetAndroidSdkInt()
    {
        using (var versionClass = new AndroidJavaClass("android.os.Build$VERSION")) {
            return versionClass.GetStatic<int>("SDK_INT");
        }
    }
    internal static void StartService(string srvClass, string extraName, BoxedValue extraValue)
    {
        using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (var unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
                if (null != unityActivity) {
                    using (var context = unityActivity.Call<AndroidJavaObject>("getApplicationContext")) {
                        if (null != context) {
                            using (var intent = new AndroidJavaObject("android.content.Intent")) {
                                using (var serviceClass = new AndroidJavaClass(srvClass)) {
                                    intent.Call<AndroidJavaObject>("setClass", context, serviceClass);
                                    PutIntentExtra(intent, extraName, extraValue);
                                    context.Call<AndroidJavaObject>("startService", intent);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    internal static void StopService(string srvClass)
    {
        using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (var unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
                if (null != unityActivity) {
                    using (var context = unityActivity.Call<AndroidJavaObject>("getApplicationContext")) {
                        if (null != context) {
                            using (var intent = new AndroidJavaObject("android.content.Intent")) {
                                using (var serviceClass = new AndroidJavaClass(srvClass)) {
                                    intent.Call<AndroidJavaObject>("setClass", context, serviceClass);
                                    context.Call<bool>("stopService", intent);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    internal static void SetPlayerPrefByJava(string key, BoxedValue val)
    {
        string prefsName = Application.identifier + ".v2.playerprefs";

        using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (var unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
                if (null != unityActivity) {
                    using (var context = unityActivity.Call<AndroidJavaObject>("getApplicationContext")) {
                        if (null != context) {
                            using (var prefs = context.Call<AndroidJavaObject>("getSharedPreferences", prefsName, 0)) {
                                if (null != prefs) {
                                    using (var wprefs = prefs.Call<AndroidJavaObject>("edit")) {
                                        if (null != wprefs) {
                                            if (val.IsInteger) {
                                                wprefs.Call<AndroidJavaObject>("putInt", key, val.GetInt());
                                            }
                                            else if (val.IsNumber) {
                                                wprefs.Call<AndroidJavaObject>("putFloat", key, val.GetFloat());
                                            }
                                            else if (val.IsString) {
                                                wprefs.Call<AndroidJavaObject>("putString", key, val.GetString());
                                            }
                                            wprefs.Call("apply");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    internal static BoxedValue GetPlayerPrefByJava(string key, BoxedValue defval)
    {
        BoxedValue ret = BoxedValue.EmptyString;
        string prefsName = Application.identifier + ".v2.playerprefs";

        using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (var unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
                if (null != unityActivity) {
                    using (var context = unityActivity.Call<AndroidJavaObject>("getApplicationContext")) {
                        if (null != context) {
                            using (var prefs = context.Call<AndroidJavaObject>("getSharedPreferences", prefsName, 0)) {
                                if (null != prefs) {
                                    if (defval.IsInteger) {
                                        ret = prefs.Call<int>("getInt", key, defval.GetInt());
                                    }
                                    else if (defval.IsNumber) {
                                        ret = prefs.Call<float>("getFloat", key, defval.GetFloat());
                                    }
                                    else if (defval.IsString) {
                                        ret = prefs.Call<string>("getString", key, defval.GetString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return ret;
    }

    internal static void PutIntentExtra(AndroidJavaObject intent, string extraName, BoxedValue extraValue)
    {
        switch (extraValue.Type) {
            case BoxedValue.c_StringType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetString());
                break;
            case BoxedValue.c_BoolType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetBool());
                break;
            case BoxedValue.c_CharType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetChar());
                break;
            case BoxedValue.c_SByteType:
            case BoxedValue.c_ByteType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetSByte());
                break;
            case BoxedValue.c_ShortType:
            case BoxedValue.c_UShortType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetShort());
                break;
            case BoxedValue.c_IntType:
            case BoxedValue.c_UIntType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetInt());
                break;
            case BoxedValue.c_LongType:
            case BoxedValue.c_ULongType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetLong());
                break;
            case BoxedValue.c_FloatType:
            case BoxedValue.c_DecimalType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetFloat());
                break;
            case BoxedValue.c_DoubleType:
                intent.Call<AndroidJavaObject>("putExtra", extraName, extraValue.GetDouble());
                break;
            case BoxedValue.c_ObjectType: {
                    var androidObj = extraValue.As<AndroidJavaObject>();
                    if (null != androidObj) {
                        intent.Call<AndroidJavaObject>("putExtra", extraName, androidObj);
                    }
                    else {
                        var list = extraValue.As<IList>();
                        if (list.Count > 0) {
                            var bv = BoxedValue.FromObject(list[0]);
                            switch (bv.Type) {
                                case BoxedValue.c_StringType: {
                                        var arr = new string[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = list[ix] as string;
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                                case BoxedValue.c_BoolType: {
                                        var arr = new bool[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = (bool)list[ix];
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                                case BoxedValue.c_CharType: {
                                        var arr = new char[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = (char)list[ix];
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                                case BoxedValue.c_SByteType:
                                case BoxedValue.c_ByteType: {
                                        var arr = new sbyte[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = (sbyte)list[ix];
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                                case BoxedValue.c_ShortType:
                                case BoxedValue.c_UShortType: {
                                        var arr = new short[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = (short)list[ix];
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                                case BoxedValue.c_IntType:
                                case BoxedValue.c_UIntType: {
                                        var arr = new int[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = (int)list[ix];
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                                case BoxedValue.c_LongType:
                                case BoxedValue.c_ULongType: {
                                        var arr = new long[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = (long)list[ix];
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                                case BoxedValue.c_FloatType:
                                case BoxedValue.c_DecimalType: {
                                        var arr = new float[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = (float)list[ix];
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                                case BoxedValue.c_DoubleType: {
                                        var arr = new double[list.Count];
                                        for (int ix = 0; ix < arr.Length; ++ix) {
                                            arr[ix] = (double)list[ix];
                                        }
                                        intent.Call<AndroidJavaObject>("putExtra", extraName, arr);
                                    }
                                    break;
                            }
                        }
                    }
                }
                break;
            default:
                // unsupported type, do nothing
                break;
        }
    }

    internal static bool UseJavaTask
    {
        get { return s_UseJavaTask; }
        set { s_UseJavaTask = value; }
    }
    internal static List<Task> Tasks
    {
        get { return s_Tasks; }
    }
    internal static List<JavaTask> JavaTasks
    {
        get { return s_JavaTasks; }
    }
    internal static void CleanupCompletedTasks()
    {
        if (s_UseJavaTask) {
            for (int ix = s_JavaTasks.Count - 1; ix >= 0; --ix) {
                var task = s_JavaTasks[ix];
                if (task.IsCompleted) {
                    s_JavaTasks.RemoveAt(ix);
                    task.Dispose();
                }
            }
        }
        else {
            for (int ix = s_Tasks.Count - 1; ix >= 0; --ix) {
                var task = s_Tasks[ix];
                if (task.IsCompleted) {
                    s_Tasks.RemoveAt(ix);
                    task.Dispose();
                }
            }
        }
    }
    internal static void ExecNoWait(string cmd, int timeout)
    {
        var state = (cmd, timeout);
        if (s_UseJavaTask) {
            var task = JavaTask.Run(ExecAndSendLogCallback, state);
            s_JavaTasks.Add(task);
        }
        else {
            // Use ThreadPool to avoid GC from Task.Run lambda
            System.Threading.ThreadPool.QueueUserWorkItem(ExecAndSendLogCallback, state);
        }
    }

    private static void ExecAndSendLogCallback(object state)
    {
        var (cmd, timeout) = ((string, int))state;
        string txt = Exec(cmd, timeout);
        SendLog(txt);
    }
    internal static string Exec(string cmd, int timeout)
    {
        var sb = new StringBuilder();
        int exitCode = -1;
        if (Application.platform == RuntimePlatform.Android) {
            AndroidJNI.AttachCurrentThread();
            using (var javaRuntime = new AndroidJavaClass("java.lang.Runtime")) {
                using (var runtime = javaRuntime.CallStatic<AndroidJavaObject>("getRuntime")) {
                    if (null != runtime) {
                        using (var process = runtime.Call<AndroidJavaObject>("exec", cmd)) {
                            if (null != process) {
                                int rowCount = 0;
                                float elapsedTime = 0;
                                float startTime = TimeUtility.GetElapsedTimeUs() / 1000.0f;
                                using (var inputStream = process.Call<AndroidJavaObject>("getInputStream")) {
                                    using (var inputStreamReader = new AndroidJavaObject("java.io.InputStreamReader", inputStream)) {
                                        if (timeout > 0) {
                                            while (elapsedTime <= timeout) {
                                                if (inputStreamReader.Call<bool>("ready")) {
                                                    int c = inputStreamReader.Call<int>("read");
                                                    if (c >= 0) {
                                                        sb.Append((char)c);
                                                        if (c == '\n') {
                                                            ++rowCount;
                                                            if (rowCount % c_ElapsedTimeLineCount == 0) {
                                                                SendLog(string.Format("command elapsed time:{0}ms", elapsedTime));
                                                            }
                                                        }
                                                    }
                                                    else {
                                                        break;
                                                    }
                                                }
                                                float curTime = TimeUtility.GetElapsedTimeUs() / 1000.0f;
                                                elapsedTime = curTime - startTime;
                                            }
                                        }
                                        else {
                                            using (var bufferedReader = new AndroidJavaObject("java.io.BufferedReader", inputStreamReader)) {
                                                string line;
                                                while ((line = bufferedReader.Call<string>("readLine")) != null) {
                                                    sb.AppendLine(line);
                                                    ++rowCount;
                                                    if (rowCount % c_ElapsedTimeLineCount == 0) {
                                                        SendLog(string.Format("command elapsed time:{0}ms", elapsedTime));
                                                    }
                                                    float curTime = TimeUtility.GetElapsedTimeUs() / 1000.0f;
                                                    elapsedTime = curTime - startTime;
                                                }
                                            }
                                        }
                                    }
                                }
                                SendLog(string.Format("command elapsed time:{0}ms", elapsedTime));

                                if (timeout <= 0) {
                                    exitCode = process.Call<int>("waitFor");
                                }
                                else {
                                    SendLog(string.Format("command timeout:{0}ms elapsed:{1}ms", timeout, elapsedTime));

                                    timeout -= (int)elapsedTime;
                                    using (var javaTimeUnit = new AndroidJavaClass("java.util.concurrent.TimeUnit")) {
                                        using (var timeUnitSeconds = javaTimeUnit.GetStatic<AndroidJavaObject>("MILLISECONDS")) {
                                            bool result = process.Call<bool>("waitFor", (long)timeout, timeUnitSeconds);
                                            if (result) {
                                                exitCode = process.Call<int>("exitValue");
                                            }
                                            else {
                                                process.Call<AndroidJavaObject>("destroyForcibly");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /*
            using (var processBuilder = new AndroidJavaObject("java.lang.ProcessBuilder", new[] { fileName, args })) {
                using (var process = processBuilder.Call<AndroidJavaObject>("start")) {
                    
                }
            }
            */
        }
        else {
            string fileName, args;
            if (cmd.StartsWith('"')) {
                int endExe = cmd.IndexOf('"', 1);
                fileName = cmd.Substring(0, endExe + 1);
                args = cmd.Substring(endExe + 1).Trim();
            }
            else {
                int splitPos = cmd.IndexOf(' ');
                while (splitPos > 0) {
                    if (cmd[splitPos - 1] == '\\') {
                        splitPos = cmd.IndexOf(' ', splitPos + 1);
                    }
                    else {
                        break;
                    }
                }
                if (splitPos > 0) {
                    fileName = cmd.Substring(0, splitPos).Trim();
                    args = cmd.Substring(splitPos + 1).Trim();
                }
                else {
                    fileName = cmd.Trim();
                    args = string.Empty;
                }
            }
            var outEncoding = Encoding.GetEncoding(GetACP());
            exitCode = RunCommand(fileName, args, sb, sb, outEncoding, timeout);
        }
        SendLog(string.Format("Command:{0} exit code:{1}", cmd, exitCode));
        return sb.ToString();
    }

    private static int RunCommand(string fileName, string args, StringBuilder output, StringBuilder error, Encoding outEncoding, int timeout)
    {
        //Considering cross-platform compatibility, do not use specific process environment variables.
        try {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            var psi = p.StartInfo;
            psi.FileName = fileName;
            psi.Arguments = args;
            psi.UseShellExecute = false;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            psi.ErrorDialog = false;
            psi.WorkingDirectory = Environment.CurrentDirectory;
            psi.RedirectStandardInput = false;
            if (null != output) {
                psi.RedirectStandardOutput = true;
                psi.StandardOutputEncoding = outEncoding;
                // Store buffers in static dictionary to avoid GC from lambda closure
                s_ProcessOutputContexts[p] = new ProcessOutputContext { Output = output, Error = error, Encoding = outEncoding };
                p.OutputDataReceived += OnOutputDataReceivedStatic;
            }
            if (null != error) {
                psi.RedirectStandardError = true;
                psi.StandardErrorEncoding = outEncoding;
                if (null == output) {
                    s_ProcessOutputContexts[p] = new ProcessOutputContext { Output = output, Error = error, Encoding = outEncoding };
                }
                p.ErrorDataReceived += OnErrorDataReceivedStatic;
            }
            if (p.Start()) {
                if (psi.RedirectStandardOutput)
                    p.BeginOutputReadLine();
                if (psi.RedirectStandardError)
                    p.BeginErrorReadLine();
                if (timeout <= 0) {
                    p.WaitForExit();
                }
                else {
                    p.WaitForExit(timeout);
                }
                if (psi.RedirectStandardOutput) {
                    p.CancelOutputRead();
                }
                if (psi.RedirectStandardError) {
                    p.CancelErrorRead();
                }
                // Clean up dictionary entry
                s_ProcessOutputContexts.Remove(p);
                int r = p.ExitCode;
                p.Close();
                return r;
            }
            else {
                Debug.LogErrorFormat("process({0} {1}) failed.", fileName, args);
                // Clean up dictionary entry on failure
                s_ProcessOutputContexts.Remove(p);
                return -1;
            }
        }
        catch (Exception ex) {
            Debug.LogErrorFormat("process({0} {1}) exception:{2} stack:{3}", fileName, args, ex.Message, ex.StackTrace);
            while (null != ex.InnerException) {
                ex = ex.InnerException;
                Debug.LogErrorFormat("\t=> exception:{0} stack:{1}", ex.Message, ex.StackTrace);
            }
            return -1;
        }
    }

    private static void OnOutputDataReceivedStatic(object sender, System.Diagnostics.DataReceivedEventArgs e)
    {
        var p = sender as System.Diagnostics.Process;
        if (p.StartInfo.RedirectStandardOutput && null != e.Data) {
            if (s_ProcessOutputContexts.TryGetValue(p, out var ctx) && null != ctx.Output) {
                AppendDataToBuffer(ctx.Output, e.Data, ctx.Encoding);
            }
        }
    }

    private static void OnErrorDataReceivedStatic(object sender, System.Diagnostics.DataReceivedEventArgs e)
    {
        var p = sender as System.Diagnostics.Process;
        if (p.StartInfo.RedirectStandardError && null != e.Data) {
            if (s_ProcessOutputContexts.TryGetValue(p, out var ctx) && null != ctx.Error) {
                AppendDataToBuffer(ctx.Error, e.Data, ctx.Encoding);
            }
        }
    }

    private static void AppendDataToBuffer(StringBuilder buffer, string str, Encoding srcEncoding)
    {
        if (srcEncoding != Encoding.UTF8) {
            var bytes = srcEncoding.GetBytes(str);
            bytes = Encoding.Convert(srcEncoding, Encoding.UTF8, bytes);
            str = Encoding.UTF8.GetString(bytes);
        }
        buffer.AppendLine(str);
    }

    private sealed class ProcessOutputContext
    {
        public StringBuilder Output;
        public StringBuilder Error;
        public Encoding Encoding;
    }

    private static Dictionary<System.Diagnostics.Process, ProcessOutputContext> s_ProcessOutputContexts = new Dictionary<System.Diagnostics.Process, ProcessOutputContext>();

    private static GameObject s_GameObj = null;
    private static Queue<string> s_CommandQueue = new Queue<string>();
    private static Queue<string> s_LogQueue = new Queue<string>();
    private static object s_Lock = new object();
    private const int c_max_command_in_queue = 1024;
    private const string c_clipboard_cmd_tag = "[cmd]:";
    private static string s_clipboard_cmd_tag = string.Empty;
    internal static string s_application_identifier = string.Empty;

    private static bool s_UseJavaTask = false;
    private static List<Task> s_Tasks = new List<Task>();
    private static List<JavaTask> s_JavaTasks = new List<JavaTask>();
    private const int c_CheckStartInterval = 500;
    private const int c_ElapsedTimeLineCount = 100;
    private const int c_TaskCleanupInterval = 1000;
    private static int s_SocketServerPort = 39527;
    private static GmSocketServer s_SocketServer;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    static extern int GetACP();
#else
    static int GetACP()
    {
        return Encoding.Default.CodePage;
    }
#endif
}

public sealed class JavaTask : IDisposable
{
    public bool IsCompleted { get; private set; } = false;

    public static JavaTask Run(Action action)
    {
        JavaTask task = new JavaTask(action);
        task.Start();
        return task;
    }
    public static JavaTask Run(Action<object> action, object state)
    {
        JavaTask task = new JavaTask(action, state);
        task.Start();
        return task;
    }
    public void Dispose()
    {
        if (null != m_Thread) {
            m_Thread.Dispose();
        }
    }

    private JavaTask(Action action)
    {
        m_Action = action;
    }
    private JavaTask(Action<object> actionWithState, object state)
    {
        m_ActionWithState = actionWithState;
        m_State = state;
    }
    private void Start()
    {
        AndroidJavaRunnable runnable;
        if (null != m_Action) {
            runnable = new AndroidJavaRunnable(Working);
        }
        else {
            runnable = new AndroidJavaRunnable(WorkingWithState);
        }
        m_Thread = new AndroidJavaObject("java.lang.Thread", runnable);
        m_Thread.Call("start");
    }
    private void Working()
    {
        try {
            m_Action();
        }
        catch {
        }
        finally {
            IsCompleted = true;
        }
    }
    private void WorkingWithState()
    {
        try {
            m_ActionWithState(m_State);
        }
        catch {
        }
        finally {
            IsCompleted = true;
        }
    }

    private Action m_Action;
    private Action<object> m_ActionWithState;
    private object m_State;
    private AndroidJavaObject m_Thread;
}

[UnityEngine.Scripting.Preserve]
internal sealed class BroadcastReceiverHandler
{
    internal void RegisterBroadcastReceiver(string actionName)
    {
        if (m_ReceiverInited) {
            return;
        }
        using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            using (var unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
                if (null != unityActivity) {
                    using (var context = unityActivity.Call<AndroidJavaObject>("getApplicationContext")) {
                        if (null != context) {
                            var callback = new BroadcastReceiverCallback();
                            m_BroadcastReceiver = new AndroidJavaObject("com.unity3d.broadcastlib.BroadcastHelper", callback);

                            using (var intentFilter = new AndroidJavaObject("android.content.IntentFilter")) {
                                intentFilter.Call("addAction", actionName);
                                int sdkInt = GmRootScript.GetAndroidSdkInt();
                                if (sdkInt >= 33) {
                                    using (var ctxClass = new AndroidJavaClass("android.content.Context")) {
                                        var flags = ctxClass.GetStatic<int>("RECEIVER_EXPORTED");
                                        context.Call<AndroidJavaObject>("registerReceiver", m_BroadcastReceiver, intentFilter, flags);
                                    }
                                }
                                else {
                                    context.Call<AndroidJavaObject>("registerReceiver", m_BroadcastReceiver, intentFilter);
                                }
                            }
                        }
                    }
                }
            }
        }
        m_ReceiverInited = true;
    }
    internal void UnregisterBroadcastReceiver()
    {
        if (m_ReceiverInited && null != m_BroadcastReceiver) {
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (var unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity")) {
                    if (null != unityActivity) {
                        using (var context = unityActivity.Call<AndroidJavaObject>("getApplicationContext")) {
                            if (null != context) {
                                context.Call("unregisterReceiver", m_BroadcastReceiver);
                            }
                        }
                    }
                }
            }
        }
    }

    private bool m_ReceiverInited;
    private AndroidJavaObject m_BroadcastReceiver;
}

[UnityEngine.Scripting.Preserve]
internal sealed class BroadcastReceiverCallback : AndroidJavaProxy
{
    public BroadcastReceiverCallback() : base("com.unity3d.broadcastlib.IBroadcastReceiver")
    { }
    void onReceive(AndroidJavaObject context, AndroidJavaObject intent)
    {
        string cmd = intent.Call<string>("getStringExtra", "cmd");
        if (!string.IsNullOrEmpty(cmd)) {
            GmRootScript.SendCommand(cmd);

            LogSystem.Warn("receive a command: {0}", cmd);
        }
        else {
            cmd = intent.Call<string>("getStringExtra", GmRootScript.s_application_identifier);
            if (!string.IsNullOrEmpty(cmd)) {
                GmRootScript.SendCommand(cmd);

                LogSystem.Warn("receive a special command: {0}", cmd);
            }
        }
    }
}

[UnityEngine.Scripting.Preserve]
internal static class WeTestAutomation
{
    internal static void InjectTouch(int action, float x, float y)
    {
        Prepare();
        if (null != s_WeTestAutomation) {
            s_WeTestAutomation.CallStatic("InjectTouchEvent", action, x, y);
        }
    }
    internal static float GetX()
    {
        float x = 0;
        Prepare();
        if (null != s_WeTestAutomation) {
            x = s_WeTestAutomation.CallStatic<float>("GetX");
        }
        return x;
    }
    internal static float GetY()
    {
        float y = 0;
        Prepare();
        if (null != s_WeTestAutomation) {
            y = s_WeTestAutomation.CallStatic<float>("GetY");
        }
        return y;
    }
    internal static int GetWidth()
    {
        int w = 0;
        Prepare();
        if (null != s_WeTestAutomation) {
            w = s_WeTestAutomation.CallStatic<int>("GetWidth");
        }
        return w;
    }
    internal static int GetHeight()
    {
        int h = 0;
        Prepare();
        if (null != s_WeTestAutomation) {
            h = s_WeTestAutomation.CallStatic<int>("GetHeight");
        }
        return h;
    }
    private static void Prepare()
    {
        if (null == s_WeTestAutomation) {
            s_WeTestAutomation = new AndroidJavaClass("com.tencent.wetest.U3DAutomation");
        }
    }

    private static AndroidJavaClass s_WeTestAutomation;
}
