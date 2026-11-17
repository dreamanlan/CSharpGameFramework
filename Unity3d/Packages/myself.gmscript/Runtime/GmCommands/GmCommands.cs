using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Assertions.Must;
using StoryScript;
using StoryScript.DslExpression;
using UnityEngine.Profiling;
using UnityEngine.UI;
using Unity.Profiling;

namespace GmCommands
{
    public static class AndroidNativeUtil
    {
        private static AndroidJavaClass s_debugJavaClass;
        private static AndroidJavaObject s_memInfoObj;

        static AndroidNativeUtil()
        {
            s_debugJavaClass = new AndroidJavaClass("android.os.Debug");
            s_memInfoObj = new AndroidJavaObject("android.os.Debug$MemoryInfo");
        }

        public static void RefreshMeminfo()
        {
            s_debugJavaClass.CallStatic("getMemoryInfo", s_memInfoObj);
        }

        [UnityEngine.Scripting.Preserve]
        public static int GetTotalPss()
        {
            RefreshMeminfo();
            var mem = s_memInfoObj.Call<int>("getTotalPss");

            return mem;
        }

        /// <summary>
        /// https://developer.android.com/reference/android/os/Debug.MemoryInfo
        /// getTotalPrivateClean getTotalPrivateDirty getTotalSharedClean getTotalSharedDirty getTotalSwappablePss
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public static int GetMemInfo(string methodName)
        {
            var mem = s_memInfoObj.Call<int>(methodName);

            return mem;
        }

        /// <summary>
        /// https://developer.android.com/reference/android/os/Debug.MemoryInfo#getMemoryStat(java.lang.String)
        /// summary.java-heap summary.native-heap summary.code summary.stack summary.graphics summary.private-other summary.system summary.total-pss summary.total-swap
        /// </summary>
        /// <param name="statName"></param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        public static int GetMemoryStat(string statName)
        {
            var str = s_memInfoObj.Call<string>("getMemoryStat", statName);

            int.TryParse(str, out var value);

            return value;
        }
    }
    public static class IOSNativeUtil
    {
#if UNITY_IPHONE && !UNITY_EDITOR
	    [DllImport("__Internal")]
	    private static extern float ios_GetTotalPhysicalMemory();
        [DllImport("__Internal")]
        private static extern float ios_GetTotalPhysicalMemoryV2();
        [DllImport("__Internal")]
        private static extern long ios_GetOsProcAvailableMemory();
#endif

        public static float GetTotalPhysicalMemory()
        {
            float result = 0;

#if UNITY_IPHONE && !UNITY_EDITOR
            result = ios_GetTotalPhysicalMemory();
#endif

            return result;
        }

        public static float GetTotalPhysicalMemoryV2()
        {
            float result = 0;

#if UNITY_IPHONE && !UNITY_EDITOR
            result = ios_GetTotalPhysicalMemoryV2();
#endif

            return result;
        }

        //com.apple.developer.kernel.increased-memory-limit
        public static long GetOsProcAvailableMemory()
        {
            long result = 0;

#if UNITY_IPHONE && !UNITY_EDITOR
            result = ios_GetOsProcAvailableMemory();
#endif

            return result;
        }
    }
    public static class WinNativeUtil
    {
        private static System.Diagnostics.Process s_process = null;

        public static System.Diagnostics.Process CurProcess
        {
            get {
                if (s_process == null) {
                    s_process = System.Diagnostics.Process.GetCurrentProcess();
                }
                return s_process;
            }
        }

        public static int CurProcessId => CurProcess?.Id ?? 0;

#if UNITY_STANDALONE_WIN
        public static MEMORY_STATUS_EX globalMemoryStatus = new MEMORY_STATUS_EX();
        public static PROCESS_MEMORY_COUNTERS_EX2 processMemoryStatus = new PROCESS_MEMORY_COUNTERS_EX2();

        [StructLayout(LayoutKind.Sequential, Size = 40)]
        public struct PROCESS_MEMORY_COUNTERS
        {
            public uint cb;
            public uint PageFaultCount;
            public UIntPtr PeakWorkingSetSize;
            public UIntPtr WorkingSetSize;
            public UIntPtr QuotaPeakPagedPoolUsage;
            public UIntPtr QuotaPagedPoolUsage;
            public UIntPtr QuotaPeakNonPagedPoolUsage;
            public UIntPtr QuotaNonPagedPoolUsage;
            public UIntPtr PagefileUsage;
            public UIntPtr PeakPagefileUsage;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORY_STATUS_EX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORY_STATUS_EX()
            {
                dwLength = (uint)Marshal.SizeOf(typeof(MEMORY_STATUS_EX));
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class PROCESS_MEMORY_COUNTERS_EX2
        {
            public uint cb;
            public uint PageFaultCount;
            public ulong PeakWorkingSetSize;
            public ulong WorkingSetSize;
            public ulong QuotaPeakPagedPoolUsage;
            public ulong QuotaPagedPoolUsage;
            public ulong QuotaPeakNonPagedPoolUsage;
            public ulong QuotaNonPagedPoolUsage;
            public ulong PagefileUsage;
            public ulong PeakPagefileUsage;
            public ulong PrivateUsage;
            public ulong PrivateWorkingSetSize;
            public UInt64 SharedCommitUsage;

            public PROCESS_MEMORY_COUNTERS_EX2()
            {
                cb = (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS_EX2));
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern System.IntPtr GetActiveWindow();

        [DllImport("psapi.dll", SetLastError = true)]
        public static extern bool GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS counters, uint size);

        [DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetProcessMemoryInfo([In] IntPtr processHandle, [In, Out] PROCESS_MEMORY_COUNTERS_EX2 ppsmemCounters, [In] uint cb);

        [DllImport("Kernel32.dll")]
        private static extern void ExitProcess(uint uExitCode);

        [DllImport("Kernel32.dll")]
        private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        private static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORY_STATUS_EX lpBuffer);

        public static ulong GetProcessMemoryInfoWorkingSetSize()
        {
            var counters = new PROCESS_MEMORY_COUNTERS();
            counters.cb = (uint)Marshal.SizeOf(typeof(PROCESS_MEMORY_COUNTERS));
            if (GetProcessMemoryInfo(CurProcess.Handle, out counters, counters.cb)) {
                return counters.WorkingSetSize.ToUInt64();
            }
            else {
                return 0;
            }
        }

#endif

        public static void SetWindowsTitle(string title)
        {
#if UNITY_STANDALONE_WIN
            const uint WM_SETTEXT = 0x000C;
            byte[] encodedBytes = Encoding.Unicode.GetBytes(title);
            Array.Resize(ref encodedBytes, encodedBytes.Length + 1);
            encodedBytes[encodedBytes.Length - 1] = 0x00;
            string encodedString = Encoding.Unicode.GetString(encodedBytes);
            GCHandle handle = GCHandle.Alloc(encodedBytes, GCHandleType.Pinned);
            IntPtr titlePtr = handle.AddrOfPinnedObject();

            SendMessage(GetActiveWindow(), WM_SETTEXT, IntPtr.Zero, titlePtr);

            handle.Free();
#endif
        }

        public static void ProcessExit(int code)
        {
#if UNITY_STANDALONE_WIN
            ExitProcess((uint)code);
#endif
        }

        public static void ProcessTerminateHandle(int code)
        {
#if UNITY_STANDALONE_WIN
            TerminateProcess(CurProcess.Handle, (uint)code);
#endif
        }

        public static void ProcessTerminateNativeHandle(int code)
        {
#if UNITY_STANDALONE_WIN
            TerminateProcess(GetCurrentProcess(), (uint)code);
#endif
        }

        public static void ProcessTerminateNativeExitCode()
        {
#if UNITY_STANDALONE_WIN
            var succ = GetExitCodeProcess(GetCurrentProcess(), out var exitCode);
            TerminateProcess(GetCurrentProcess(), exitCode);
#endif
        }

        public static bool RefreshGlobalMemoryStatus()
        {
#if UNITY_STANDALONE_WIN
            if (GlobalMemoryStatusEx(globalMemoryStatus)) {
                return true;
            }
#endif
            return false;
        }

        public static bool RefreshProcessMemoryStatusEx2()
        {
#if UNITY_STANDALONE_WIN
            if (GetProcessMemoryInfo(CurProcess.Handle, processMemoryStatus, processMemoryStatus.cb)) {
                return true;
            }
#endif
            return false;
        }
    }
    public static class ProfilerForGM
    {
        public static long GetAllocatorCount()
        {
            return 0;
        }
        public static long GetTotalAllocationCount()
        {
            return 0;
        }
        public static long GetMallocLLAllocBytes()
        {
            return 0;
        }
        public static long GetMallocOverrideBytes()
        {
            return 0;
        }
        public static long GetTotalProfilerMemoryLong()
        {
            return 0;
        }
        public static int GetFPS()
        {
            float time = GetMainThreadFrameTime();
            if (time > float.Epsilon) {
                return (int)Math.Floor(1000.0f / time);
            }
            return 60;
        }
        public static float GetMainThreadFrameTime()
        {
            int ix = (int)StatIndexEnum.GameTime;
            if (ix < s_Datas.Count) {
                return s_Datas[ix].lastValue / 1000000.0f;
            }
            return 0.0f;
        }
        public static float GetRenderThreadFrameTime()
        {
            int ix = (int)StatIndexEnum.RenderTime;
            if (ix < s_Datas.Count) {
                return s_Datas[ix].lastValue / 1000000.0f;
            }
            return 0.0f;
        }
        public static int GetVertsCount()
        {
#if UNITY_EDITOR
            return 0;
#else
            int ix = (int)StatIndexEnum.Verts;
            if (ix < s_Datas.Count) {
                return (int)s_Datas[ix].lastValue;
            }
            return 0;
#endif
        }
        public static int GetTriangleCount()
        {
#if UNITY_EDITOR
            return 0;
#else
            int ix = (int)StatIndexEnum.Tris;
            if (ix < s_Datas.Count) {
                return (int)s_Datas[ix].lastValue;
            }
            return 0;
#endif
        }
        public static int GetDrawCallCount()
        {
#if UNITY_EDITOR
            return 0;
#else
            int ix = (int)StatIndexEnum.DrawCall;
            if (ix < s_Datas.Count) {
                return (int)s_Datas[ix].lastValue;
            }
            return 0;
#endif
        }
        public static int GetSetPassCalls()
        {
#if UNITY_EDITOR
            return 0;
#else
            int ix = (int)StatIndexEnum.SetPass;
            if (ix < s_Datas.Count) {
                return (int)s_Datas[ix].lastValue;
            }
            return 0;
#endif
        }
        public static long GetUsedTextureBytes()
        {
            return 0;
        }
        public static int GetUsedTextureCount()
        {
            return 0;
        }

        public static long GetRuntimeMemorySizeLong(UnityEngine.Object o)
        {
            return Profiler.GetRuntimeMemorySizeLong(o);
        }
        public static long GetMonoHeapSizeLong()
        {
            return Profiler.GetMonoHeapSizeLong();
        }
        public static long GetMonoUsedSizeLong()
        {
            return Profiler.GetMonoHeapSizeLong();
        }
        public static long GetUsedHeapSize()
        {
            return Profiler.usedHeapSizeLong;
        }
        public static uint GetTempAllocatorSize()
        {
            return Profiler.GetTempAllocatorSize();
        }
        public static long GetTotalAllocatedMemoryLong()
        {
            return Profiler.GetTotalAllocatedMemoryLong();
        }
        public static long GetTotalUnusedReservedMemoryLong()
        {
            return Profiler.GetTotalUnusedReservedMemoryLong();
        }
        public static long GetTotalReservedMemoryLong()
        {
            return Profiler.GetTotalReservedMemoryLong();
        }
        public static long GetAllocatedMemoryForGraphicsDriver()
        {
            return Profiler.GetAllocatedMemoryForGraphicsDriver();
        }

        internal static bool IsStarted()
        {
            return s_Started;
        }
        internal static void Start()
        {
            if (s_Started)
                return;

            s_Datas.Clear();
            s_Datas.Add(new Data { category = ProfilerCategory.Render, statName = "CPU Main Thread Frame Time", displayName = "Game" });
            s_Datas.Add(new Data { category = ProfilerCategory.Render, statName = "CPU Render Thread Frame Time", displayName = "Render" });

            s_Datas.Add(new Data { category = ProfilerCategory.Render, statName = "Vertices Count", displayName = "Verts" });
            s_Datas.Add(new Data { category = ProfilerCategory.Render, statName = "Triangles Count", displayName = "Tris" });
            s_Datas.Add(new Data { category = ProfilerCategory.Render, statName = "Draw Calls Count", displayName = "Draw" });
            s_Datas.Add(new Data { category = ProfilerCategory.Render, statName = "SetPass Calls Count", displayName = "SetPass" });

            foreach (var data in s_Datas) {
                data.profilerRecorder = ProfilerRecorder.StartNew(data.category, data.statName);
            }
            Debug.Assert(s_Datas.Count == (int)StatIndexEnum.MaxNum);

            s_Started = true;
        }
        internal static void Stop()
        {
            s_Started = false;

            foreach (var data in s_Datas) {
                data.profilerRecorder.Stop();
                data.profilerRecorder.Dispose();
            }
            s_Datas.Clear();
        }
        internal static void Update()
        {
            foreach (var data in s_Datas) {
                long v = data.profilerRecorder.LastValue;
                if (v > 0) {
                    data.lastValue = v;
                }
            }
        }

        private enum StatIndexEnum
        {
            GameTime = 0,
            RenderTime,
            Verts,
            Tris,
            DrawCall,
            SetPass,
            MaxNum
        }
        private class Data
        {
            public ProfilerRecorder profilerRecorder;
            public ProfilerCategory category;
            public string statName;
            public string displayName;
            public long lastValue = 0;
        }

        private static List<Data> s_Datas = new List<Data>();
        private static bool s_Started = false;
    }
    //---------------------------------------------------------------------------------------------------------------
    internal sealed class LogCodeNumCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            LogSystem.Warn("app code num:{0}", 116);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class HelpCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            Application.OpenURL("https://github.com/dreamanlan/BatchCommandApp/blob/master/help.md");
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetDebugCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int val = operands[0].GetInt();
            StoryConfigManager.Instance.IsDebug = val != 0;
            return BoxedValue.NullObject;
        }
    }
    internal sealed class StartActivityCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var args = operands;
            if (Application.platform == RuntimePlatform.Android) {
                if (args.Count >= 1 && args[0].IsString) {
                    string packageName = args[0].GetString();
                    string className = string.Empty;
                    int flags = 0;
                    IList list = null;
                    IDictionary dict = null;
                    bool noError = true;
                    for (int i = 1; i < args.Count; ++i) {
                        var arg = args[i];
                        if (arg.IsString && i == 1) {
                            className = arg.GetString();
                        }
                        else if (arg.IsInteger && i <= 2) {
                            flags = arg.GetInt();
                        }
                        else if (arg.IsObject && i == args.Count - 1) {
                            list = arg.As<IList>();
                            dict = arg.As<IDictionary>();
                        }
                        else {
                            //error arg
                            LogSystem.Error("start_activity, illegal argument {0}:{1}", i, arg);
                            noError = false;
                        }
                    }
                    if (noError) {
                        StartActivity(packageName, className, flags, list, dict);
                    }
                }
                else {
                    LogSystem.Error("expect start_activity(package_name, ...)");
                }
            }
            return BoxedValue.NullObject;
        }
        private static void StartActivity(string packageName, string className, int flags, IList list, IDictionary dict)
        {
            if (string.IsNullOrEmpty(packageName)) {
                packageName = Application.identifier;
            }
            if (string.IsNullOrEmpty(className)) {
                //className = "com.unity3d.player.UnityPlayerActivity";
                className = packageName + ".MainActivity";
            }

            using (var up = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (var ca = up.GetStatic<AndroidJavaObject>("currentActivity")) {
                    using (var packageManager = ca.Call<AndroidJavaObject>("getPackageManager")) {
                        using (var intent = new AndroidJavaObject("android.content.Intent")) {
                            intent.Call<AndroidJavaObject>("setClassName", packageName, className);
                            if (flags != 0) {
                                intent.Call<AndroidJavaObject>("addFlags", flags);
                            }
                            if (null != list) {
                                for (int ix = 0; ix < list.Count - 1; ix += 2) {
                                    var key = list[ix] as string;
                                    var val = null == list[ix + 1] ? BoxedValue.NullObject : BoxedValue.FromObject(list[ix + 1]);
                                    if (!string.IsNullOrEmpty(key)) {
                                        GmRootScript.PutIntentExtra(intent, key, val);
                                    }
                                }
                            }
                            else if (null != dict) {
                                foreach (var elem in dict) {
                                    var pair = (DictionaryEntry)elem;
                                    var key = pair.Key as string;
                                    var val = null == pair.Value ? BoxedValue.NullObject : BoxedValue.FromObject(pair.Value);
                                    if (!string.IsNullOrEmpty(key)) {
                                        GmRootScript.PutIntentExtra(intent, key, val);
                                    }
                                }
                            }
                            ca.Call("startActivity", intent);

                            //intent.Call<AndroidJavaObject>("addFlags", 0x00008000); // Intent.FLAG_ACTIVITY_CLEAR_TASK
                            //intent.Call<AndroidJavaObject>("addFlags", 0x04000000); // Intent.FLAG_ACTIVITY_CLEAR_TOP
                            //intent.Call<AndroidJavaObject>("addFlags", 0x08000000); // Intent.FLAG_ACTIVITY_MULTIPLE_TASK
                            //intent.Call<AndroidJavaObject>("addFlags", 0x10000000); // Intent.FLAG_ACTIVITY_NEW_TASK
                            //intent.Call<AndroidJavaObject>("addFlags", 0x20000000); // Intent.FLAG_ACTIVITY_SINGLE_TOP
                            //ca.Call("finish");
                        }
                    }
                }
            }
        }
    }
    internal sealed class FinishActivityCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (Application.platform == RuntimePlatform.Android) {
                FinishActivity();
            }
            return BoxedValue.NullObject;
        }
        private static void FinishActivity()
        {
            using (var up = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                using (var ca = up.GetStatic<AndroidJavaObject>("currentActivity")) {
                    ca.Call("finish");
                }
            }
        }
    }
    internal sealed class ListenClipboardCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int val = operands[0].GetInt();
            GmRootScript.ListenClipboard(val);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ListenAndroidCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            GmRootScript.ListenAndroid();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ListenSocketCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int port = operands[0].GetInt();
            GmRootScript.ListenSocket(port);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class StartServiceCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string srvClass = operands[0].GetString();
            string extraName = operands[1].GetString();
            BoxedValue extraValue = operands[2];
            GmRootScript.StartService(srvClass, extraName, extraValue);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class StopServiceCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string srvClass = operands[0].GetString();
            GmRootScript.StopService(srvClass);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ShellCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string cmd = operands[0].GetString();
            GmRootScript.ExecNoWait(cmd, 0);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ShellTimeoutCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string cmd = operands[0].GetString();
            int timeout = operands[1].GetInt();
            GmRootScript.ExecNoWait(cmd, timeout);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class CleanupCompletedTasksCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            GmRootScript.CleanupCompletedTasks();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class UseJavaTaskCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            GmRootScript.UseJavaTask = operands[0].GetBool();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetClipboardCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string text = operands[0].GetString();
            GUIUtility.systemCopyBuffer = text;
            return BoxedValue.NullObject;
        }
    }
    internal sealed class EditorBreakCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            UnityEngine.Debug.Break();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class DebugBreakCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            UnityEngine.Debug.DebugBreak();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetTimeScaleCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            Time.timeScale = operands[0].GetFloat();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ClearGlobalsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            ClientGmStorySystem.Instance.ClearContextVariables();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SupportsGfxFormatCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var vs = Enum.GetValues(typeof(GraphicsFormat));
            var us = Enum.GetValues(typeof(FormatUsage));
            int ct = 0;
            foreach (var e in vs) {
                var gf = (GraphicsFormat)e;
                try {
                    var rtf = GraphicsFormatUtility.GetRenderTextureFormat(gf);
                    var tf = GraphicsFormatUtility.GetTextureFormat(gf);
                    LogSystem.Warn("[check graphics format {0} <=> rt:{1} tex:{2}]", gf, rtf, tf);
                    foreach (var u in us) {
                        var fu = (FormatUsage)u;
                        if (!SystemInfo.IsFormatSupported(gf, fu)) {
                            LogSystem.Error("can't support graphics format {0} usage {1} <=> rt:{2} tex:{3}", gf, fu, rtf, tf);
                        }
                    }
                    if (!SystemInfo.IsFormatSupported(gf, FormatUsage.MSAA8x + 1)) {
                        LogSystem.Error("can't support graphics format {0} usage MSAA16x <=> rt:{1} tex:{2}", gf, rtf, tf);
                    }
                    if (!SystemInfo.IsFormatSupported(gf, FormatUsage.MSAA8x + 2)) {
                        LogSystem.Error("can't support graphics format {0} usage MSAA32x <=> rt:{1} tex:{2}", gf, rtf, tf);
                    }
                    ++ct;
                    if (ct % 20 == 0) {
                        Thread.Sleep(0);
                    }
                }
                catch (Exception ex) {
                    LogSystem.Error("invalid graphics format {0}, exception:{1}", gf, ex.Message);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SupportsTexCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var vs = Enum.GetValues(typeof(TextureFormat));
            int ct = 0;
            foreach (var e in vs) {
                var tf = (TextureFormat)e;
                try {
                    var gfSrgb = GraphicsFormatUtility.GetGraphicsFormat(tf, true);
                    var gfLinear = GraphicsFormatUtility.GetGraphicsFormat(tf, false);
                    LogSystem.Warn("[check tex {0} <=> srgb:{1} linear:{2}]", tf, gfSrgb, gfLinear);
                    if (!SystemInfo.SupportsTextureFormat(tf)) {
                        LogSystem.Error("can't support tex {0}, srgb:{1} linear:{2}", tf, gfSrgb, gfLinear);
                    }
                    ++ct;
                    if (ct % 300 == 0) {
                        Thread.Sleep(0);
                    }
                }
                catch (Exception ex) {
                    LogSystem.Error("invalid tex format {0}, exception:{1}", tf, ex.Message);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SupportsRTCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var vs = Enum.GetValues(typeof(RenderTextureFormat));
            int ct = 0;
            foreach (var e in vs) {
                var rtf = (RenderTextureFormat)e;
                try {
                    var gfSrgb = GraphicsFormatUtility.GetGraphicsFormat(rtf, true);
                    var gfLinear = GraphicsFormatUtility.GetGraphicsFormat(rtf, false);
                    LogSystem.Warn("[check RT {0} <=> srgb:{1} linear:{2}]", rtf, gfSrgb, gfLinear);
                    if (!SystemInfo.SupportsRenderTextureFormat(rtf)) {
                        LogSystem.Error("can't support RT {0}, srgb:{1} linear:{2}", rtf, gfSrgb, gfLinear);
                    }
                    if (!SystemInfo.SupportsBlendingOnRenderTextureFormat(rtf)) {
                        LogSystem.Error("can't support blending on RT {0}, srgb:{1} linear:{2}", rtf, gfSrgb, gfLinear);
                    }
                    if (!SystemInfo.SupportsRandomWriteOnRenderTextureFormat(rtf)) {
                        LogSystem.Error("can't support random write on RT {0}, srgb:{1} linear:{2}", rtf, gfSrgb, gfLinear);
                    }
                    ++ct;
                    if (ct % 100 == 0) {
                        Thread.Sleep(0);
                    }
                }
                catch (Exception ex) {
                    LogSystem.Error("invalid rt format {0}, exception:{1}", rtf, ex.Message);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SupportsVertexAttributeFormatCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var vs = Enum.GetValues(typeof(VertexAttributeFormat));
            int ct = 0;
            foreach (var e in vs) {
                var vaf = (VertexAttributeFormat)e;
                try {
                    for (int dim = 1; dim <= 4; ++dim) {
                        LogSystem.Warn("[check vertex attribute format {0} dim {1}]", vaf, dim);
                        if (!SystemInfo.SupportsVertexAttributeFormat(vaf, dim)) {
                            LogSystem.Error("can't support vertex attribute format {0} dim {1}", vaf, dim);
                        }
                    }
                    ++ct;
                    if (ct % 100 == 0) {
                        Thread.Sleep(0);
                    }
                }
                catch (Exception ex) {
                    LogSystem.Error("invalid vertex attribute format {0}, exception:{1}", vaf, ex.Message);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class DeviceSupportsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var log1 = new StringBuilder();
            var log2 = new StringBuilder();
            int ct = 0;
            var t = typeof(SystemInfo);
            var pis = t.GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var pi in pis) {
                var v = pi.GetValue(null);
                log1.AppendFormat("{0} = {1}", pi.Name, v);
                log1.AppendLine();
                ++ct;
                if (ct % 10 == 0) {
                    LogSystem.Warn(log1.ToString());
                    log1.Length = 0;
                }

                if (v is bool) {
                    if (!(bool)v) {
                        log2.AppendFormat("{0} = false", pi.Name);
                        log2.AppendLine();
                    }
                }
            }
            LogSystem.Warn(log2.ToString());
            return BoxedValue.NullObject;
        }
    }
    internal sealed class LogResolutionsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            foreach (var reso in Screen.resolutions) {
                LogSystem.Warn("resolution:{0}x{1} refresh rate ratio:{2}", reso.width, reso.height, reso.refreshRateRatio);
            }
            return BoxedValue.NullObject;
        }
    }
    //---------------------------------------------------------------------------------------------------------------
    internal sealed class CreateDirectoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string dir = operands[0].GetString();
            dir = Environment.ExpandEnvironmentVariables(dir);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
                LogSystem.Warn("create directory {0}", dir);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class DeleteDirectoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string dir = operands[0].GetString();
            dir = Environment.ExpandEnvironmentVariables(dir);
            if (Directory.Exists(dir)) {
                Directory.Delete(dir, recursive: true);
                LogSystem.Warn("delete directory {0}", dir);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class CopyDirectoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string dir1 = operands[0].AsString;
            string dir2 = operands[1].AsString;
            dir1 = Environment.ExpandEnvironmentVariables(dir1);
            dir2 = Environment.ExpandEnvironmentVariables(dir2);
            List<string> filterAndNewExts = new List<string>();
            for (int i = 2; i < operands.Count; i++) {
                string str = operands[i].AsString;
                if (str != null) {
                    filterAndNewExts.Add(str);
                    continue;
                }
                IList strList = operands[i].As<IList>();
                if (strList == null) {
                    continue;
                }
                foreach (object strObj in strList) {
                    if (strObj is string tempStr) {
                        filterAndNewExts.Add(tempStr);
                    }
                }
            }
            if (filterAndNewExts.Count <= 0) {
                filterAndNewExts.Add("*");
            }
            string targetRoot = Path.GetFullPath(dir2);
            if (Directory.Exists(dir1)) {
                int ct = 0;
                CopyFolder(targetRoot, dir1, dir2, filterAndNewExts, ref ct);
                LogSystem.Warn("copy {0} files", ct);
            }
            return BoxedValue.NullObject;
        }
        private static void CopyFolder(string targetRoot, string from, string to, IList<string> filterAndNewExts, ref int ct)
        {
            if (!string.IsNullOrEmpty(to) && !Directory.Exists(to)) {
                Directory.CreateDirectory(to);
            }
            string[] directories = Directory.GetDirectories(from);
            foreach (string sub in directories) {
                string srcPath = Path.GetFullPath(sub);
                if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX) {
                    if (srcPath.IndexOf(targetRoot) == 0) {
                        continue;
                    }
                }
                else if (srcPath.IndexOf(targetRoot, StringComparison.CurrentCultureIgnoreCase) == 0) {
                    continue;
                }
                string sName = Path.GetFileName(sub);
                CopyFolder(targetRoot, sub, Path.Combine(to, sName), filterAndNewExts, ref ct);
            }
            for (int j = 0; j < filterAndNewExts.Count; j += 2) {
                string filter = filterAndNewExts[j];
                string newExt = string.Empty;
                if (j + 1 < filterAndNewExts.Count) {
                    newExt = filterAndNewExts[j + 1];
                }
                string[] files = Directory.GetFiles(from, filter, SearchOption.TopDirectoryOnly);
                foreach (string file in files) {
                    string targetFile = ((!string.IsNullOrEmpty(newExt)) ? Path.Combine(to, Path.ChangeExtension(Path.GetFileName(file), newExt)) : Path.Combine(to, Path.GetFileName(file)));
                    File.Copy(file, targetFile, overwrite: true);
                    ct++;
                    LogSystem.Warn("copy file {0} => {1}", file, targetFile);
                }
            }
        }
    }
    internal sealed class CopyFileCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string file1 = operands[0].GetString();
            string file2 = operands[1].GetString();
            file1 = Environment.ExpandEnvironmentVariables(file1);
            file2 = Environment.ExpandEnvironmentVariables(file2);
            if (File.Exists(file1)) {
                string dir = Path.GetDirectoryName(file2);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                File.Copy(file1, file2, overwrite: true);
                LogSystem.Warn("copy file {0} => {1}", file1, file2);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class DeleteFileCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string file = operands[0].GetString();
            file = Environment.ExpandEnvironmentVariables(file);
            if (File.Exists(file)) {
                File.Delete(file);
                LogSystem.Warn("delete file {0}", file);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class CopyFilesCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string dir1 = operands[0].AsString;
            string dir2 = operands[1].AsString;
            dir1 = Environment.ExpandEnvironmentVariables(dir1);
            dir2 = Environment.ExpandEnvironmentVariables(dir2);
            List<string> filterAndNewExts = new List<string>();
            for (int i = 2; i < operands.Count; i++) {
                string str = operands[i].AsString;
                if (str != null) {
                    filterAndNewExts.Add(str);
                    continue;
                }
                IList strList = operands[i].As<IList>();
                if (strList == null) {
                    continue;
                }
                foreach (object strObj in strList) {
                    if (strObj is string tempStr) {
                        filterAndNewExts.Add(tempStr);
                    }
                }
            }
            if (filterAndNewExts.Count <= 0) {
                filterAndNewExts.Add("*");
            }
            if (Directory.Exists(dir1)) {
                int ct = 0;
                CopyFolder(dir1, dir2, filterAndNewExts, ref ct);
                LogSystem.Warn("copy {0} files", ct);
            }
            return BoxedValue.NullObject;
        }
        private static void CopyFolder(string from, string to, IList<string> filterAndNewExts, ref int ct)
        {
            if (!string.IsNullOrEmpty(to) && !Directory.Exists(to)) {
                Directory.CreateDirectory(to);
            }
            for (int i = 0; i < filterAndNewExts.Count; i += 2) {
                string filter = filterAndNewExts[i];
                string newExt = string.Empty;
                if (i + 1 < filterAndNewExts.Count) {
                    newExt = filterAndNewExts[i + 1];
                }
                string[] files = Directory.GetFiles(from, filter, SearchOption.TopDirectoryOnly);
                foreach (string file in files) {
                    string targetFile = ((!string.IsNullOrEmpty(newExt)) ? Path.Combine(to, Path.ChangeExtension(Path.GetFileName(file), newExt)) : Path.Combine(to, Path.GetFileName(file)));
                    File.Copy(file, targetFile, overwrite: true);
                    ct++;
                    Debug.LogFormat("copy file {0} => {1}", file, targetFile);
                }
            }
        }
    }
    internal sealed class DeleteFilesCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string dir = operands[0].AsString;
            List<string> filters = new List<string>();
            for (int i = 1; i < operands.Count; i++) {
                string str = operands[i].AsString;
                if (str != null) {
                    filters.Add(str);
                    continue;
                }
                IList strList = operands[i].As<IList>();
                if (strList == null) {
                    continue;
                }
                foreach (object strObj in strList) {
                    if (strObj is string tempStr) {
                        filters.Add(tempStr);
                    }
                }
            }
            if (filters.Count <= 0) {
                filters.Add("*");
            }
            dir = Environment.ExpandEnvironmentVariables(dir);
            if (Directory.Exists(dir)) {
                int ct = 0;
                foreach (string filter in filters) {
                    string[] files = Directory.GetFiles(dir, filter, SearchOption.TopDirectoryOnly);
                    foreach (string file in files) {
                        File.Delete(file);
                        ct++;
                        LogSystem.Warn("delete file {0}", file);
                    }
                }
                LogSystem.Warn("delete {0} files", ct);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class DeleteAllFilesCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string dir = operands[0].AsString;
            List<string> filters = new List<string>();
            for (int i = 1; i < operands.Count; i++) {
                string str = operands[i].AsString;
                if (str != null) {
                    filters.Add(str);
                    continue;
                }
                IList strList = operands[i].As<IList>();
                if (strList == null) {
                    continue;
                }
                foreach (object strObj in strList) {
                    if (strObj is string tempStr) {
                        filters.Add(tempStr);
                    }
                }
            }
            if (filters.Count <= 0) {
                filters.Add("*");
            }
            dir = Environment.ExpandEnvironmentVariables(dir);
            if (Directory.Exists(dir)) {
                int ct = 0;
                foreach (string filter in filters) {
                    string[] files = Directory.GetFiles(dir, filter, SearchOption.AllDirectories);
                    foreach (string file in files) {
                        File.Delete(file);
                        ct++;
                        LogSystem.Warn("delete file {0}", file);
                    }
                }
                LogSystem.Warn("delete {0} files", ct);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetCurrentDirectoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string dir = operands[0].GetString();
            string path = Environment.ExpandEnvironmentVariables(dir);
            if (Path.IsPathRooted(path)) {
                Environment.CurrentDirectory = path;
            }
            else {
                Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, path);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class AllocMemoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            int size = operands[1].GetInt();
            BoxedValue m = BoxedValue.FromObject(new byte[size]);
            var instance = calculator.GetFuncContext<StoryInstance>();
            if (instance.ContextVariables.ContainsKey(key)) {
                instance.ContextVariables[key] = m;
            }
            else {
                instance.ContextVariables.Add(key, m);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FreeMemoryCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            var instance = calculator.GetFuncContext<StoryInstance>();
            if (instance.ContextVariables.ContainsKey(key)) {
                instance.ContextVariables.Remove(key);
                GC.Collect();
            }
            else {
                GC.Collect();
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ConsumeCpuCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int time = operands[0].GetInt();
            long startTime = TimeUtility.GetElapsedTimeUs();
            while (startTime + time > TimeUtility.GetElapsedTimeUs()) {
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GcCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            GC.Collect();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class StartProfilerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            ProfilerForGM.Start();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class StopProfilerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            ProfilerForGM.Stop();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class LogProfilerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            LogSystem.Warn("[memory] used heap:{0:f3} profiler:{1:f3} gfx:{2:f3} alloc:{3:f3} temp:{4:f3} unused reserved:{5:f3} reserved:{6:f3} alloc count:{7} total alloc count:{8} malloc LL:{9:f3} malloc override:{10:f3}", ProfilerForGM.GetUsedHeapSize() / 1024.0f / 1024.0f, ProfilerForGM.GetTotalProfilerMemoryLong() / 1024.0f / 1024.0f,
                ProfilerForGM.GetAllocatedMemoryForGraphicsDriver() / 1024.0f / 1024.0f, ProfilerForGM.GetTotalAllocatedMemoryLong() / 1024.0f / 1024.0f, ProfilerForGM.GetTempAllocatorSize() / 1024.0f / 1024.0f, ProfilerForGM.GetTotalUnusedReservedMemoryLong() / 1024.0f / 1024.0f,
                ProfilerForGM.GetTotalReservedMemoryLong() / 1024.0f / 1024.0f,
                ProfilerForGM.GetAllocatorCount(), ProfilerForGM.GetTotalAllocationCount(),
                ProfilerForGM.GetMallocLLAllocBytes() / 1024.0f / 1024.0f, ProfilerForGM.GetMallocOverrideBytes() / 1024.0f / 1024.0f);
            LogSystem.Warn("[mono] used:{0} heap:{1}", ProfilerForGM.GetMonoUsedSizeLong() / 1024.0f / 1024.0f, ProfilerForGM.GetMonoHeapSizeLong() / 1024.0f / 1024.0f);
            LogSystem.Warn("[time] main:{0} rendering:{1} fps:{2}", ProfilerForGM.GetMainThreadFrameTime(), ProfilerForGM.GetRenderThreadFrameTime(), ProfilerForGM.GetFPS());
            LogSystem.Warn("[rendering] vertex:{0} triangle:{1} dc:{2} set pass:{3} tex:{4:f3} tex count:{5}", ProfilerForGM.GetVertsCount(), ProfilerForGM.GetTriangleCount(),
                ProfilerForGM.GetDrawCallCount(), ProfilerForGM.GetSetPassCalls(),
                ProfilerForGM.GetUsedTextureBytes() / 1024.0f / 1024.0f, ProfilerForGM.GetUsedTextureCount());
            return BoxedValue.NullObject;
        }
    }
    internal sealed class TakeSnapshotCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string path = GetMemSnapPath();
            bool autoClearMemory = false;
            Unity.Profiling.Memory.CaptureFlags captureFlags = Unity.Profiling.Memory.CaptureFlags.ManagedObjects | Unity.Profiling.Memory.CaptureFlags.NativeObjects |
                                        Unity.Profiling.Memory.CaptureFlags.NativeAllocations | Unity.Profiling.Memory.CaptureFlags.NativeAllocationSites |
                                        Unity.Profiling.Memory.CaptureFlags.NativeStackTraces;
            var vals = operands;
            if (vals.Count >= 1) {
                string p = vals[0].AsString;
                if (!string.IsNullOrEmpty(p) && File.Exists(p)) {
                    path = p;
                }
            }
            if (vals.Count >= 2) {
                autoClearMemory = vals[1].GetBool();
            }
            if (vals.Count >= 3) {
                captureFlags = (Unity.Profiling.Memory.CaptureFlags)vals[2].GetUInt();
            }
            TakeSnapshot(path, autoClearMemory, captureFlags);
            LogSystem.Warn("snapshot path:{0}", path);
            return BoxedValue.NullObject;
        }
        private static string GetMemSnapPath(string ext = "snap")
        {
            var dirPath = Path.Combine(Application.persistentDataPath, "MemorySnapshot");
            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }

            var pid = System.Diagnostics.Process.GetCurrentProcess()?.Id ?? 0;
            var mem = GetAppMemory();
            var reserved = (ProfilerForGM.GetTotalReservedMemoryLong() + ProfilerForGM.GetMonoHeapSizeLong()) / (1024.0 * 1024);
            var allocated = (ProfilerForGM.GetTotalAllocatedMemoryLong() + ProfilerForGM.GetMonoUsedSizeLong()) / (1024.0 * 1024);

            var path = string.Empty;
            if (Application.platform == RuntimePlatform.Android) {
                var heap = AndroidNativeUtil.GetMemoryStat("summary.native-heap") / 1024.0f;
                var code = AndroidNativeUtil.GetMemoryStat("summary.code") / 1024.0f;
                var graphics = AndroidNativeUtil.GetMemoryStat("summary.graphics") / 1024.0f;
                var other = AndroidNativeUtil.GetMemoryStat("summary.private-other") / 1024.0f;
                var system = AndroidNativeUtil.GetMemoryStat("summary.system") / 1024.0f;
                path = Path.Combine(dirPath, $"app_{pid}_{DateTime.Now:yyyyMMdd_HHmmss}_Mem[{mem:F2}]_Reserved[{reserved:F2}]_Allocated[{allocated:F2}]_Heap[{heap:F2}]_Code[{code:F2}]_Graphics[{graphics:F2}]_Other[{other:F2}]_System[{system:F2}].{ext}");
            }
            else {
                path = Path.Combine(dirPath, $"app_{pid}_{DateTime.Now:yyyyMMdd_HHmmss}_Mem[{mem:F2}]_Reserved[{reserved:F2}]_Allocated[{allocated:F2}].{ext}");
            }

            return path;
        }
        private static float GetAppMemory()
        {
            float mem = 0;
            try {
#if UNITY_EDITOR
                mem = ((float)Profiler.GetTotalAllocatedMemoryLong()) / (1024 * 1024);
#elif UNITY_ANDROID
                mem = AndroidNativeUtil.GetTotalPss() / 1024.0f;
#elif UNITY_IOS
                mem = IOSNativeUtil.GetTotalPhysicalMemory();
#elif UNITY_STANDALONE_WIN
                mem = (float) ((double) WinNativeUtil.GetProcessMemoryInfoWorkingSetSize() / (1024 * 1024));
#endif
            }
            catch (Exception e) {
                LogSystem.Error(e.Message);
            }

            return mem;
        }
        private static void TakeSnapshot(string path, bool autoClearMemory = false,
            Unity.Profiling.Memory.CaptureFlags captureFlags = Unity.Profiling.Memory.CaptureFlags.ManagedObjects | Unity.Profiling.Memory.CaptureFlags.NativeObjects |
                                        Unity.Profiling.Memory.CaptureFlags.NativeAllocations | Unity.Profiling.Memory.CaptureFlags.NativeAllocationSites |
                                        Unity.Profiling.Memory.CaptureFlags.NativeStackTraces,
            Action<string, bool> finishCallback = null)
        {
            if (autoClearMemory) {
                GC.Collect();
                Resources.UnloadUnusedAssets();
            }

            Unity.Profiling.Memory.MemoryProfiler.TakeSnapshot(path, finishCallback, captureFlags);
        }
    }
    internal sealed class CmdCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string cmd = operands[0].GetString();
            DebugConsole.Execute(cmd);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GmCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string gmToServer = operands[0].GetString();
            LogSystem.Warn("todo: gm {0}", gmToServer);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PlayerPrefIntCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            int val = operands[1].GetInt();
            PlayerPrefs.SetInt(key, val);
            PlayerPrefs.Save();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PlayerPrefFloatCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            float val = operands[1].GetFloat();
            PlayerPrefs.SetFloat(key, val);
            PlayerPrefs.Save();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PlayerPrefStringCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            string val = operands[1].GetString();
            PlayerPrefs.SetString(key, val);
            PlayerPrefs.Save();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PlayerPrefDeleteCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PlayerPrefDeleteAllCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PrefByJavaCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            BoxedValue val = operands[1];
            GmRootScript.SetPlayerPrefByJava(key, val);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class WeTestTouchCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int action = operands[0].GetInt();
            float x = operands[1].GetFloat();
            float y = operands[2].GetFloat();
            WeTestAutomation.InjectTouch(action, x, y);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class LogMeshCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var meshObj = operands[0];
            Mesh mesh = StoryScriptUtility.GetMeshArg(ref meshObj);
            if (null != mesh) {
                var sb = new StringBuilder();
                sb.AppendFormat("mesh:{0} vertex count:{1} submesh count:{2} vertex attribute count:{3} vertex buffer count:{4}", mesh, mesh.vertexCount, mesh.subMeshCount, mesh.vertexAttributeCount, mesh.vertexBufferCount);
                sb.AppendLine();
                for (int ix = 0; ix < mesh.vertexAttributeCount; ++ix) {
                    var vattr = mesh.GetVertexAttribute(ix);
                    sb.AppendFormat("\tvattr:{0} attribute:{1} format:{2} dim:{3} stream:{4}", ix, vattr.attribute, vattr.format, vattr.dimension, vattr.stream);
                    sb.AppendLine();
                }
                for (int ix = 0; ix < mesh.vertexBufferCount; ++ix) {
                    var gf = mesh.GetVertexBuffer(ix);
                    sb.AppendFormat("\tvbuffer:{0} handle:0x{1:X} target:{2} stride:{3} count:{4}", ix, gf.bufferHandle.value, gf.target, gf.stride, gf.count);
                    sb.AppendLine();
                }
                for (int ix = 0; ix < mesh.subMeshCount; ++ix) {
                    var submesh = mesh.GetSubMesh(ix);
                    switch (submesh.topology) {
                        case MeshTopology.Triangles:
                            sb.AppendFormat("\tsubmesh:{0} vertex count:{1} index count:{2} topology:{3} triangle count:{4}", ix, submesh.vertexCount, submesh.indexCount, submesh.topology, submesh.indexCount / 3);
                            sb.AppendLine();
                            break;
                        default:
                            sb.AppendFormat("\tsubmesh:{0} vertex count:{1} index count:{2} topology:{3}", ix, submesh.vertexCount, submesh.indexCount, submesh.topology);
                            sb.AppendLine();
                            break;
                    }
                }
                LogSystem.Warn("{0}", sb.ToString());
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetMaterialCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var args = operands;
            if (args.Count == 2) {
                var dest = args[0];
                var src = args[1];
                var rd = StoryScriptUtility.GetRendererArg(ref dest);
                var m = StoryScriptUtility.GetMaterialArg(ref src);
                if (null != rd && null != m) {
                    var oldMat = rd.material;
                    rd.material = m;
                    StoryScriptUtility.DestroyObject(oldMat);
                }
            }
            else if (args.Count == 3) {
                var dest = args[0];
                int ix = args[1].GetInt();
                var src = args[2];
                var rd = StoryScriptUtility.GetRendererArg(ref dest);
                var m = StoryScriptUtility.GetMaterialArg(ref src);
                if (null != rd && ix >= 0 && ix < rd.materials.Length && null != m) {
                    var mats = rd.materials;
                    var newMats = new Material[mats.Length];
                    Array.Copy(mats, newMats, mats.Length);
                    var oldMat = mats[ix];
                    newMats[ix] = m;
                    rd.materials = newMats;
                    StoryScriptUtility.DestroyObject(oldMat);
                }
            }
            else if (args.Count == 4) {
                var dest = args[0];
                int ix = args[1].GetInt();
                var src = args[2];
                int six = args[3].GetInt();
                var rd = StoryScriptUtility.GetRendererArg(ref dest);
                var ms = StoryScriptUtility.GetMaterialsArg(ref src);
                if (null != rd && ix >= 0 && ix < rd.materials.Length && null != ms && six >= 0 && six < ms.Length) {
                    var mats = rd.materials;
                    var newMats = new Material[mats.Length];
                    Array.Copy(mats, newMats, mats.Length);
                    var oldMat = mats[ix];
                    newMats[ix] = ms[six];
                    rd.materials = newMats;
                    StoryScriptUtility.DestroyObject(oldMat);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetSharedMaterialCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var args = operands;
            if (args.Count == 2) {
                var dest = args[0];
                var src = args[1];
                var rd = StoryScriptUtility.GetRendererArg(ref dest);
                var m = StoryScriptUtility.GetMaterialArg(ref src);
                if (null != rd && null != m) {
                    rd.sharedMaterial = m;
                }
            }
            else if (args.Count == 3) {
                var dest = args[0];
                int ix = args[1].GetInt();
                var src = args[2];
                var rd = StoryScriptUtility.GetRendererArg(ref dest);
                var m = StoryScriptUtility.GetMaterialArg(ref src);
                if (null != rd && ix >= 0 && ix < rd.sharedMaterials.Length && null != m) {
                    var mats = rd.materials;
                    var newMats = new Material[mats.Length];
                    Array.Copy(mats, newMats, mats.Length);
                    newMats[ix] = m;
                    rd.sharedMaterials = newMats;
                }
            }
            else if (args.Count == 4) {
                var dest = args[0];
                int ix = args[1].GetInt();
                var src = args[2];
                int six = args[3].GetInt();
                var rd = StoryScriptUtility.GetRendererArg(ref dest);
                var ms = StoryScriptUtility.GetMaterialsArg(ref src);
                if (null != rd && ix >= 0 && ix < rd.sharedMaterials.Length && null != ms && six >= 0 && six < ms.Length) {
                    var mats = rd.materials;
                    var newMats = new Material[mats.Length];
                    Array.Copy(mats, newMats, mats.Length);
                    newMats[ix] = ms[six];
                    rd.sharedMaterials = newMats;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetMaterialsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var args = operands;
            if (args.Count == 2) {
                var dest = args[0];
                var src = args[1];
                var rd = StoryScriptUtility.GetRendererArg(ref dest);
                var ms = StoryScriptUtility.GetMaterialsArg(ref src);
                if (null != rd && null != ms) {
                    rd.materials = ms;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SetSharedMaterialsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var args = operands;
            if (args.Count == 2) {
                var dest = args[0];
                var src = args[1];
                var rd = StoryScriptUtility.GetRendererArg(ref dest);
                var ms = StoryScriptUtility.GetMaterialsArg(ref src);
                if (null != rd && null != ms) {
                    rd.sharedMaterials = ms;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialSetFloatCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            var val = operands[2].GetFloat();
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                if (key.IsString) {
                    mat.SetFloat(key.AsString, val);
                }
                else if (key.IsInteger) {
                    mat.SetFloat(key.GetInt(), val);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialSetIntCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            var val = operands[2].GetInt();
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                if (key.IsString) {
                    mat.SetInt(key.AsString, val);
                }
                else if (key.IsInteger) {
                    mat.SetInt(key.GetInt(), val);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialSetIntegerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            var val = operands[2].GetInt();
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                if (key.IsString) {
                    mat.SetInteger(key.AsString, val);
                }
                else if (key.IsInteger) {
                    mat.SetInteger(key.GetInt(), val);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialSetVectorCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            var val = operands[2];
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                var v = val.As<Vector4Obj>();
                if (key.IsString) {
                    mat.SetVector(key.AsString, v);
                }
                else if (key.IsInteger) {
                    mat.SetVector(key.GetInt(), v);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialSetColorCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            var val = operands[2];
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                Color v;
                if (val.IsString) {
                    v = StoryScriptUtility.GetColor(val.AsString);
                }
                else {
                    v = val.As<ColorObj>();
                }
                if (key.IsString) {
                    mat.SetColor(key.AsString, v);
                }
                else if (key.IsInteger) {
                    mat.SetColor(key.GetInt(), v);
                }
            }
            return BoxedValue.NullObject;
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------
    internal sealed class ExistsDirectoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string dir = operands[0].GetString();
            dir = Environment.ExpandEnvironmentVariables(dir);
            return BoxedValue.FromBool(Directory.Exists(dir));
        }
    }
    internal sealed class ExistsFileFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string file = operands[0].GetString();
            file = Environment.ExpandEnvironmentVariables(file);
            return BoxedValue.FromBool(File.Exists(file));
        }
    }
    internal sealed class ExpandEnvironmentsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string file = operands[0].GetString();
            return Environment.ExpandEnvironmentVariables(file);
        }
    }
    internal sealed class EnvironmentsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return BoxedValue.FromObject(Environment.GetEnvironmentVariables());
        }
    }
    internal sealed class GetCurrentDirectoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Environment.CurrentDirectory;
        }
    }
    internal sealed class IsDebugFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return StoryConfigManager.Instance.IsDebug;
        }
    }
    internal sealed class IsDevelopmentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return StoryConfigManager.Instance.IsDevelopment;
        }
    }
    internal sealed class TypeOfFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var args = operands;
            string type = string.Empty;
            if (args.Count == 1) {
                type = args[0].AsString;
            }
            else if (args.Count == 2) {
                type = args[0].AsString + ", " + args[1].AsString;
            }
            if (string.IsNullOrEmpty(type))
                return BoxedValue.NullObject;
            else
                return StoryScriptUtility.GetType(type);
        }
    }
    internal sealed class GetMonoMemoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return GC.GetTotalMemory(false) / 1024.0f / 1024.0f;
        }
    }
    internal sealed class GetNativeMemoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Profiler.GetTotalAllocatedMemoryLong() / 1024.0f / 1024.0f;
        }
    }
    internal sealed class GetGfxMemoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Profiler.GetAllocatedMemoryForGraphicsDriver() / 1024.0f / 1024.0f;
        }
    }
    internal sealed class GetUnusedMemoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Profiler.GetTotalUnusedReservedMemoryLong() / 1024.0f / 1024.0f;
        }
    }
    internal sealed class GetTotalMemoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Profiler.GetTotalReservedMemoryLong() / 1024.0f / 1024.0f;
        }
    }
    internal sealed class GetObjectMemoryFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var obj = (operands[0].GetObject() as UnityEngine.Object);
            return Profiler.GetRuntimeMemorySizeLong(obj);
        }
    }
    internal sealed class DeviceInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return SystemInfo.deviceName + " | " + SystemInfo.deviceModel + " | "
                + SystemInfo.graphicsDeviceName + " | " + SystemInfo.graphicsDeviceVersion + " | "
                + SystemInfo.operatingSystem + " | " + SystemInfo.processorType;
        }
    }
    internal sealed class GetClipboardFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return GUIUtility.systemCopyBuffer;
        }
    }
    internal sealed class GetObjectByIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var instId = operands[0].GetInt();
            if (instId != 0) {
                var o = typeof(UnityEngine.Object).InvokeMember("FindObjectFromInstanceID", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, new object[] { instId });
                return o as UnityEngine.Object;
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetObjectIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var obj = (operands[0].GetObject() as UnityEngine.Object);
            if (null != obj) {
                return obj.GetInstanceID();
            }
            return 0;
        }
    }
    internal sealed class GetAndroidSdkIntFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            return GmRootScript.GetAndroidSdkInt();
#endif
            return 0;
        }
    }
    internal sealed class GetBoolFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            var def = operands[1].GetBool();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<bool>("getBooleanExtra", str, def);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetBoolArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<bool[]>("getBooleanArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetStringFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<string>("getStringExtra", str);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetStringArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<string[]>("getStringArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetCharFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            var def = operands[1].GetChar();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<char>("getCharExtra", str, def);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetCharArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<char[]>("getCharArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetByteFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            var def = operands[1].GetSByte();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<sbyte>("getByteExtra", str, def);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetByteArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<sbyte[]>("getByteArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetShortFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            var def = operands[1].GetShort();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<short>("getShortExtra", str, def);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetShortArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<short[]>("getShortArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetIntFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            var def = operands[1].GetInt();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<int>("getIntExtra", str, def);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetIntArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<int[]>("getIntArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetLongFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            var def = operands[1].GetLong();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<long>("getLongExtra", str, def);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetLongArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<long[]>("getLongArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetFloatFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            var def = operands[1].GetFloat();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<float>("getFloatExtra", str, def);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetFloatArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<float[]>("getFloatArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetDoubleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            var def = operands[1].GetDouble();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return intent.Call<double>("getDoubleExtra", str, def);
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetDoubleArrayFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var str = operands[0].GetString();
            if (!string.IsNullOrEmpty(str)) {
                var act = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                var intent = act.Call<AndroidJavaObject>("getIntent");
                return BoxedValue.FromObject(intent.Call<double[]>("getDoubleArrayExtra", str));
            }
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetActivityClassNameFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
#if UNITY_ANDROID
            var javaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            string fullClassName = javaObject.Call<AndroidJavaObject>("getClass").Call<string>("getName");
            return fullClassName;
#endif
            return BoxedValue.NullObject;
        }
    }
    internal sealed class PlayerPrefIntFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            int def = operands.Count > 1 ? operands[1].GetInt() : 0;
            int val = PlayerPrefs.GetInt(key, def);

            return val;
        }
    }
    internal sealed class PlayerPrefFloatFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            float def = operands.Count > 1 ? operands[1].GetFloat() : 0;
            float val = PlayerPrefs.GetFloat(key, def);

            return val;
        }
    }
    internal sealed class PlayerPrefStringFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            string def = operands.Count > 1 ? operands[1].GetString() : string.Empty;
            string val = PlayerPrefs.GetString(key, def);

            return val;
        }
    }
    internal sealed class PrefByJavaFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string key = operands[0].GetString();
            BoxedValue def = operands.Count > 1 ? operands[1] : BoxedValue.NullObject;
            return GmRootScript.GetPlayerPrefByJava(key, def);
        }
    }
    internal sealed class IsFormatSupportedFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int r = -1;
            string ename = operands[0].GetString();
            string uname = operands[1].GetString();
            GraphicsFormat gf;
            FormatUsage fu;
            if (Enum.TryParse(ename, out gf) && Enum.TryParse(uname, out fu)) {
                try {
                    r = SystemInfo.IsFormatSupported(gf, fu) ? 1 : 0;
                }
                catch {
                    r = -1;
                }
            }
            return r;
        }
    }
    internal sealed class GetCompatibleFormatFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string r = string.Empty;
            string ename = operands[0].GetString();
            string uname = operands[1].GetString();
            GraphicsFormat gf;
            FormatUsage fu;
            if (Enum.TryParse(ename, out gf) && Enum.TryParse(uname, out fu)) {
                try {
                    var f = SystemInfo.GetCompatibleFormat(gf, fu);
                    r = f.ToString();
                }
                catch {
                    r = string.Empty;
                }
            }
            return r;
        }
    }
    internal sealed class GetGraphicsFormatFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string r = string.Empty;
            string ename = operands[0].GetString();
            DefaultFormat df;
            if (Enum.TryParse(ename, out df)) {
                try {
                    var f = SystemInfo.GetGraphicsFormat(df);
                    r = f.ToString();
                }
                catch {
                    r = string.Empty;
                }
            }
            return r;
        }
    }
    internal sealed class GetMSAASampleCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            int r = -1;
            int w = operands[0].GetInt();
            int h = operands[1].GetInt();
            string ename = operands[2].GetString();
            int d = operands[3].GetInt();
            int mips = operands[4].GetInt();
            GraphicsFormat gfc;
            if (Enum.TryParse(ename, out gfc)) {
                try {
                    r = SystemInfo.GetRenderTextureSupportedMSAASampleCount(new RenderTextureDescriptor(w, h, gfc, d, mips));
                }
                catch {
                    r = -1;
                }
            }
            return r;
        }
    }
    internal sealed class SystemInfoFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return typeof(SystemInfo);
        }
    }
    internal sealed class ScreenFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return typeof(Screen);
        }
    }
    internal sealed class ScreenWidthFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Screen.width;
        }
    }
    internal sealed class ScreenHeightFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Screen.height;
        }
    }
    internal sealed class ScreenDPIFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Screen.dpi;
        }
    }
    internal sealed class ApplicationFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return typeof(Application);
        }
    }
    internal sealed class AppIdFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Application.identifier;
        }
    }
    internal sealed class AppNameFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Application.productName;
        }
    }
    internal sealed class PlatformFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Application.platform.ToString();
        }
    }
    internal sealed class IsEditorFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Application.isEditor;
        }
    }
    internal sealed class IsConsoleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Application.isConsolePlatform;
        }
    }
    internal sealed class IsMobileFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Application.isMobilePlatform;
        }
    }
    internal sealed class IsAndroidFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Application.platform == RuntimePlatform.Android;
        }
    }
    internal sealed class IsIPhoneFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }
    internal sealed class IsPCFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return !Application.isMobilePlatform && !Application.isConsolePlatform;
        }
    }
    internal sealed class ShellFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string cmd = operands[0].GetString();
            return GmRootScript.Exec(cmd, 0);
        }
    }
    internal sealed class ShellTimeoutFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string cmd = operands[0].GetString();
            int timeout = operands[1].GetInt();
            return GmRootScript.Exec(cmd, timeout);
        }
    }
    internal sealed class IsJavaTaskFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return GmRootScript.UseJavaTask;
        }
    }
    internal sealed class GetTaskCountFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return GmRootScript.UseJavaTask ? GmRootScript.JavaTasks.Count : GmRootScript.Tasks.Count;
        }
    }
    internal sealed class WeTestGetXFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return WeTestAutomation.GetX();
        }
    }
    internal sealed class WeTestGetYFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return WeTestAutomation.GetY();
        }
    }
    internal sealed class WeTestGetWidthFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return WeTestAutomation.GetWidth();
        }
    }
    internal sealed class WeTestGetHeightFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return WeTestAutomation.GetHeight();
        }
    }
    internal sealed class GetPointerFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return (StoryScript.Vector3Obj)Input.mousePosition;
        }
    }
    internal sealed class PointerRaycastUisFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return BoxedValue.FromObject(RaycastUisFunction.GetUiObjectsUnderMouse(Input.mousePosition));
        }
    }
    internal sealed class GetPointerComponentsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var typeObj = operands[0].GetObject();
            bool include_inactive = operands[1].GetBool();
            var list = RaycastComponentsFunction.GetUiComponentsUnderMouse(Input.mousePosition, typeObj, include_inactive);
            if (null != list) {
                return BoxedValue.FromObject(list);
            }
            return BoxedValue.NullObject;
        }
    }
    public sealed class RaycastUisFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            return BoxedValue.FromObject(GetUiObjectsUnderMouse(new Vector2(x, y)));
        }
        public static List<UnityEngine.EventSystems.RaycastResult> GetUiObjectsUnderMouse(Vector2 pos)
        {
            List<UnityEngine.EventSystems.RaycastResult> results = new List<UnityEngine.EventSystems.RaycastResult>();

            var eventSystems = GameObject.FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
            var graphicRaycasters = GameObject.FindObjectsOfType<UnityEngine.UI.GraphicRaycaster>();

            foreach (var eventSystem in eventSystems) {
                foreach (var graphicRaycaster in graphicRaycasters) {
                    var pointerEventData = new UnityEngine.EventSystems.PointerEventData(eventSystem);
                    pointerEventData.position = pos;

                    graphicRaycaster.Raycast(pointerEventData, results);
                }
            }

            return results;
        }
    }
    public sealed class RaycastComponentsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            var typeObj = operands[2].GetObject();
            bool include_inactive = operands[3].GetBool();
            var list = GetUiComponentsUnderMouse(new Vector2(x, y), typeObj, include_inactive);
            if (null != list) {
                return BoxedValue.FromObject(list);
            }
            return BoxedValue.NullObject;
        }
        public static List<Component> GetUiComponentsUnderMouse(Vector2 pos, object typeObj, bool include_inactive)
        {
            var type = typeObj as Type;
            var typeStr = typeObj as string;
            if (null != typeStr) {
                type = StoryScriptUtility.GetType(typeStr);
            }
            if (null != type) {
                var list = new List<Component>();
                var objs = RaycastUisFunction.GetUiObjectsUnderMouse(pos);
                foreach (var obj in objs) {
                    var comp = obj.gameObject.GetComponent(type);
                    if (null != comp) {
                        list.Add(comp);
                    }
                    else {
                        comp = obj.gameObject.GetComponentInParent(type, include_inactive);
                        if (null != comp) {
                            list.Add(comp);
                        }
                        else {
                            comp = obj.gameObject.GetComponentInChildren(type, include_inactive);
                            if (null != comp) {
                                list.Add(comp);
                            }
                        }
                    }
                }
                return list;
            }
            return null;
        }
    }
    internal sealed class GetScenePathFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var prefixs = (operands[0].GetObject() as System.Collections.IList);
            var obj = operands[1].GetObject();
            int up_level = operands[2].GetInt();
            return GetScenePath(prefixs, obj, up_level);
        }
        internal static string GetScenePath(System.Collections.IList prefixs, object obj, int up_level)
        {
            var tr = obj as Transform;
            if (null == tr) {
                var gobj = obj as GameObject;
                var comp = obj as Component;
                if (null != gobj) {
                    tr = gobj.transform;
                }
                else if (null != comp) {
                    tr = comp.transform;
                }
                else {
                    var str = obj as string;
                    if (!string.IsNullOrEmpty(str)) {
                        gobj = GameObject.Find(str);
                        if (null != gobj) {
                            tr = gobj.transform;
                        }
                    }
                }
            }
            var sb = new StringBuilder();
            if (tr != null) {
                foreach (var prefix in prefixs) {
                    sb.Append(prefix);
                }
                sb.Append(StoryScriptUtility.GetScenePath(tr, up_level));
            }
            return sb.ToString();
        }
    }
    internal sealed class ShaderFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            return typeof(Shader);
        }
    }
    internal sealed class GetMaterialFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count == 1) {
                var matObj = operands[0];
                var mat = StoryScriptUtility.GetMaterialArg(ref matObj);
                return mat;
            }
            else if (operands.Count == 2) {
                var matObj = operands[0];
                var matIx = operands[1].GetInt();
                var mats = StoryScriptUtility.GetMaterialsArg(ref matObj);
                if (null != mats && matIx >= 0 && matIx < mats.Length) {
                    return mats[matIx];
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class GetSharedMaterialFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            if (operands.Count == 1) {
                var matObj = operands[0];
                var mat = StoryScriptUtility.GetSharedMaterialArg(ref matObj);
                return mat;
            }
            else if (operands.Count == 2) {
                var matObj = operands[0];
                var matIx = operands[1].GetInt();
                var mats = StoryScriptUtility.GetSharedMaterialsArg(ref matObj);
                if (null != mats && matIx >= 0 && matIx < mats.Length) {
                    return mats[matIx];
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialGetFloatFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                if (key.IsString) {
                    return mat.GetFloat(key.AsString);
                }
                else if (key.IsInteger) {
                    return mat.GetFloat(key.GetInt());
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialGetIntFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                if (key.IsString) {
                    return mat.GetInt(key.AsString);
                }
                else if (key.IsInteger) {
                    return mat.GetInt(key.GetInt());
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialGetIntegerFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                if (key.IsString) {
                    return mat.GetInteger(key.AsString);
                }
                else if (key.IsInteger) {
                    return mat.GetInteger(key.GetInt());
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialGetVectorFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                if (key.IsString) {
                    return (StoryScript.Vector4Obj)mat.GetVector(key.AsString);
                }
                else if (key.IsInteger) {
                    return (StoryScript.Vector4Obj)mat.GetVector(key.GetInt());
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class MaterialGetColorFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var matObj = operands[0];
            var key = operands[1];
            Material mat = StoryScriptUtility.GetMaterialArg(ref matObj);
            if (null != mat) {
                if (key.IsString) {
                    return (StoryScript.ColorObj)mat.GetColor(key.AsString);
                }
                else if (key.IsInteger) {
                    return (StoryScript.ColorObj)mat.GetColor(key.GetInt());
                }
            }
            return BoxedValue.NullObject;
        }
    }
    //---------------------------------------------------------------------------------------------------------------
    internal sealed class LogObjectsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var name = operands[0].GetString();
            var typeObj = operands[1].GetObject();
            var type = typeObj as Type;
            var typeStr = typeObj as string;
            if (null != typeStr) {
                type = StoryScriptUtility.GetType(typeStr);
            }
            if (null != type) {
                var objs = Resources.FindObjectsOfTypeAll(type);
                foreach (var obj in objs) {
                    if (null != obj) {
                        string objName = obj.name;
                        if (objName.Contains(name)) {
                            LogSystem.Warn("{0} {1} {2}", obj.name, obj, obj.GetType());
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class LogComponentsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var root = operands[0].GetString();
            var vals = (operands[1].GetObject() as System.Collections.IList);
            var typeObj = operands[2].GetObject();
            int up_level = operands[3].GetInt();
            bool include_inactive = operands[4].GetBool();
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var type = typeObj as Type;
            var typeStr = typeObj as string;
            if (null != typeStr) {
                type = StoryScriptUtility.GetType(typeStr);
            }
            if (null != type) {
                if (string.IsNullOrEmpty(root)) {
                    var objs = GameObject.FindObjectsOfType(type, include_inactive);
                    foreach (var obj in objs) {
                        var comp = obj as Component;
                        if (null != comp) {
                            if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                                LogSystem.Warn("{0}", StoryScriptUtility.GetScenePath(comp.transform, up_level));
                            }
                        }
                    }
                }
                else {
                    var rootObj = GameObject.Find(root);
                    if (null != rootObj) {
                        var comps = rootObj.GetComponentsInChildren(type, include_inactive);
                        foreach (var comp in comps) {
                            if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                                LogSystem.Warn("{0}", StoryScriptUtility.GetScenePath(comp.transform, up_level));
                            }
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class LogScenePathCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var prefixs = (operands[0].GetObject() as System.Collections.IList);
            var obj = operands[1].GetObject();
            int up_level = operands[2].GetInt();
            LogSystem.Warn(GetScenePathFunction.GetScenePath(prefixs, obj, up_level));
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ClickUiCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var btns0 = GameObject.FindObjectsOfType<Button>(false);
            foreach (var btn in btns0) {
                if (StoryScriptUtility.IsPathMatch(btn.transform, names)) {
                    btn.onClick.Invoke();
                    break;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ToggleOnCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var toggles0 = GameObject.FindObjectsOfType<Toggle>(false);
            foreach (var toggle in toggles0) {
                if (StoryScriptUtility.IsPathMatch(toggle.transform, names)) {
                    toggle.isOn = !toggle.isOn;
                    break;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ClickOnPointerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            ClickOnPosCommand.ClickFirstOnPos(Input.mousePosition);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ToggleOnPointerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            ToggleOnPosCommand.ClickFirstOnPos(Input.mousePosition);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ClickOnPosCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            ClickFirstOnPos(new Vector2(x, y));
            return BoxedValue.NullObject;
        }
        internal static void ClickFirstOnPos(Vector2 pos)
        {
            var objs = RaycastUisFunction.GetUiObjectsUnderMouse(pos);
            Button firstButton = null;
            foreach (var obj in objs) {
                var btn0 = obj.gameObject.GetComponent<Button>();
                if (null != btn0) {
                    firstButton = btn0;
                    break;
                }
                else {
                    btn0 = obj.gameObject.GetComponentInParent<Button>();
                    if (null != btn0) {
                        firstButton = btn0;
                        break;
                    }
                    else {
                        btn0 = obj.gameObject.GetComponentInChildren<Button>();
                        if (null != btn0) {
                            firstButton = btn0;
                            break;
                        }
                    }
                }
            }
            if (null != firstButton) {
                firstButton.onClick.Invoke();
            }
        }
    }
    internal sealed class ToggleOnPosCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            ClickFirstOnPos(new Vector2(x, y));
            return BoxedValue.NullObject;
        }
        internal static void ClickFirstOnPos(Vector2 pos)
        {
            var objs = RaycastUisFunction.GetUiObjectsUnderMouse(pos);
            Toggle firstToggle = null;
            foreach (var obj in objs) {
                var toggle0 = obj.gameObject.GetComponent<Toggle>();
                if (null != toggle0) {
                    firstToggle = toggle0;
                    break;
                }
                else {
                    toggle0 = obj.gameObject.GetComponentInParent<Toggle>();
                    if (null != toggle0) {
                        firstToggle = toggle0;
                        break;
                    }
                    else {
                        toggle0 = obj.gameObject.GetComponentInChildren<Toggle>();
                        if (null != toggle0) {
                            firstToggle = toggle0;
                            break;
                        }
                    }
                }
            }
            if (null != firstToggle) {
                firstToggle.isOn = !firstToggle.isOn;
            }
        }
    }
    internal sealed class ClickCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var obj = operands[0].GetObject();
            var btn = obj as Button;
            if (null != btn) {
                btn.onClick.Invoke();
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ToggleCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var obj = operands[0].GetObject();
            var toggle = obj as Toggle;
            if (null != toggle) {
                toggle.isOn = !toggle.isOn;
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class LogCompiledStartupsCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            StartupScript.LogCompiledStartups();
            return BoxedValue.NullObject;
        }
    }
    internal sealed class RunStartupCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var startupFile = operands[0].GetString();
            bool runGm = operands[1].GetBool();
            if (!Path.IsPathRooted(startupFile)) {
                startupFile = StartupScript.ScriptPath + startupFile;
            }
            StartupScript.ClearStartups();
            StartupScript.LoadStartup(startupFile);
            StartupScript.RunStartup(out var gmtxt);
            if (runGm && !string.IsNullOrEmpty(gmtxt)) {
                DebugConsole.Execute(gmtxt);
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class CompileStartupCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var startupFile = operands[0].GetString();
            if (!Path.IsPathRooted(startupFile)) {
                startupFile = StartupScript.ScriptPath + startupFile;
            }
            StartupScript.CompileStartup(startupFile);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class ImportNamespaceCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string ns = operands[0].GetString();
            string assembly = operands[1].GetString();
            StoryScriptUtility.ImportNamespace(ns, assembly);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class UnImportNamespaceCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            string ns = operands[0].GetString();
            string assembly = operands[1].GetString();
            StoryScriptUtility.UnImportNamespace(ns, assembly);
            return BoxedValue.NullObject;
        }
    }
    internal sealed class AddOrUpdateSceneViewerCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var args = operands;
            string key = args[0].GetString();
            Vector4Obj v4obj = null;
            float interval = 0.1f;
            switch (args.Count) {
                case 1://add_or_update_scene_viewer(key)
                    break;
                case 2://add_or_update_scene_viewer(key,rect)
                    v4obj = args[1].As<Vector4Obj>();
                    break;
                case 3://add_or_update_scene_viewer(key,rect,interval)
                    v4obj = args[1].As<Vector4Obj>();
                    interval = args[2].GetFloat();
                    break;
            }
            var gobj = GameObject.Find(key);
            if (null == gobj) {
                gobj = new GameObject(key);
            }
            var viewer = gobj.GetComponent<AutoFitSceneViewer>();
            if (null == viewer) {
                viewer = gobj.AddComponent<AutoFitSceneViewer>();
                if (null != v4obj) {
                    var v4 = v4obj.Value;
                    viewer.imgLeft = (int)v4.x;
                    viewer.imgTop = (int)v4.y;
                    viewer.rtWidth = (int)v4.z;
                    viewer.rtHeight = (int)v4.w;
                }
                viewer.refreshInterval = interval;
            }
            else {
                if (null != v4obj) {
                    var v4 = v4obj.Value;
                    viewer.Setup((int)v4.x, (int)v4.y, (int)v4.z, (int)v4.w, interval);
                }
                else {
                    viewer.refreshInterval = interval;
                }
            }
            viewer.target = GetCameraTarget();
            return BoxedValue.NullObject;
        }
        internal static Transform GetCameraTarget()
        {
            var root = GameObject.Find("LookPos");
            if (null != root) {
                return root.transform;
            }
            var camera = Camera.main;
            if (null == camera)
                camera = Camera.current;
            if (null != camera) {
                return camera.transform;
            }
            return null;
        }
    }
    internal sealed class SceneViewerLookAtCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var args = operands;
            string key = args[0].GetString();
            var gobj = GameObject.Find(key);
            if (null != gobj) {
                var viewer = gobj.GetComponent<AutoFitSceneViewer>();
                if (null != viewer) {
                    Transform player = AddOrUpdateSceneViewerCommand.GetCameraTarget();
                    switch (args.Count) {
                        case 1://scene_viewer_look_at(key)
                            if (null != player) {
                                viewer.target = player;
                            }
                            break;
                        case 2://scene_viewer_look_at(key, distance)
                            if (null != player) {
                                viewer.target = player;
                            }
                            viewer.distToTarget = args[1].GetFloat();
                            break;
                        case 3://scene_viewer_look_at(key, distance, direction)
                            if (null != player) {
                                viewer.target = player;
                            }
                            viewer.distToTarget = args[1].GetFloat();
                            viewer.direction = args[2].As<Vector3Obj>().Value;
                            break;
                        case 4://scene_viewer_look_at(key, distance, direction, target)
                            viewer.distToTarget = args[1].GetFloat();
                            viewer.direction = args[2].As<Vector3Obj>().Value;
                            if (args[3].IsString) {
                                var tobj = GameObject.Find(args[3].GetString());
                                if (null != tobj) {
                                    viewer.target = tobj.transform;
                                }
                            }
                            break;
                    }
                    viewer.Refresh();
                }
            }
            return BoxedValue.NullObject;
        }
    }
    //---------------------------------------------------------------------------------------------------------------
    internal sealed class FindRawImageFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<RawImage>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindUiImageFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<Image>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindUiButtonFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {

            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<Button>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindUiToggleFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {

            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<Toggle>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindUiSliderFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {

            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<Slider>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindUiInputFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {

            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<InputField>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindTmpInputFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {

            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<TMPro.TMP_InputField>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindUiDropdownFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {

            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<Dropdown>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindTmpDropdownFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {

            var vals = operands;
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var comp0 = GameObject.FindObjectsOfType<TMPro.TMP_Dropdown>(false);
            foreach (var comp in comp0) {
                if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                    return comp;
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class FindObjectFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var name = operands[0].GetString();
            var typeObj = operands[1].GetObject();
            var type = typeObj as Type;
            var typeStr = typeObj as string;
            if (null != typeStr) {
                type = StoryScriptUtility.GetType(typeStr);
            }
            if (null != type) {
                var objs = Resources.FindObjectsOfTypeAll(type);
                foreach (var obj in objs) {
                    if (null != obj) {
                        string objName = obj.name;
                        if (objName.Contains(name)) {
                            return BoxedValue.FromObject(obj);
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SearchObjectsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var list = new List<UnityEngine.Object>();
            var name = operands[0].GetString();
            var typeObj = operands[1].GetObject();
            var type = typeObj as Type;
            var typeStr = typeObj as string;
            if (null != typeStr) {
                type = StoryScriptUtility.GetType(typeStr);
            }
            if (null != type) {
                var objs = Resources.FindObjectsOfTypeAll(type);
                foreach (var obj in objs) {
                    if (null != obj) {
                        string objName = obj.name;
                        if (objName.Contains(name)) {
                            list.Add(obj);
                        }
                    }
                }
            }
            return BoxedValue.FromObject(list);
        }
    }
    internal sealed class FindComponentFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var root = operands[0].GetString();
            var vals = (operands[1].GetObject() as System.Collections.IList);
            var typeObj = operands[2].GetObject();
            bool include_inactive = operands[3].GetBool();
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var type = typeObj as Type;
            var typeStr = typeObj as string;
            if (null != typeStr) {
                type = StoryScriptUtility.GetType(typeStr);
            }
            if (null != type) {
                if (string.IsNullOrEmpty(root)) {
                    var objs = GameObject.FindObjectsOfType(type, include_inactive);
                    foreach (var obj in objs) {
                        var comp = obj as Component;
                        if (null != comp) {
                            if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                                return BoxedValue.FromObject(comp);
                            }
                        }
                    }
                }
                else {
                    var rootObj = GameObject.Find(root);
                    if (null != rootObj) {
                        var comps = rootObj.GetComponentsInChildren(type, include_inactive);
                        foreach (var comp in comps) {
                            if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                                return BoxedValue.FromObject(comp);
                            }
                        }
                    }
                }
            }
            return BoxedValue.NullObject;
        }
    }
    internal sealed class SearchComponentsFunction : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var list = new List<Component>();
            var root = operands[0].GetString();
            var vals = (operands[1].GetObject() as System.Collections.IList);
            var typeObj = operands[2].GetObject();
            bool include_inactive = operands[3].GetBool();
            var names = new List<string>();
            foreach (var v in vals) {
                names.Add(v.ToString());
            }
            var type = typeObj as Type;
            var typeStr = typeObj as string;
            if (null != typeStr) {
                type = StoryScriptUtility.GetType(typeStr);
            }
            if (null != type) {
                if (string.IsNullOrEmpty(root)) {
                    var objs = GameObject.FindObjectsOfType(type, include_inactive);
                    foreach (var obj in objs) {
                        var comp = obj as Component;
                        if (null != comp) {
                            if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                                list.Add(comp);
                            }
                        }
                    }
                }
                else {
                    var rootObj = GameObject.Find(root);
                    if (null != rootObj) {
                        var comps = rootObj.GetComponentsInChildren(type, include_inactive);
                        foreach (var comp in comps) {
                            if (StoryScriptUtility.IsPathMatch(comp.transform, names)) {
                                list.Add(comp);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }

    // List APIs registered on the CURRENT interpreter (same pattern as BatchCommand's
    // HelpAllExp: BatchScript.ApiDocs => Calculator.ApiDocs). AbstractExpression exposes a
    // protected `Calculator`, so no reflection, no captured static registry and no
    // GetSharedApiRegistry() fallback is needed - this is exactly the Calculator running
    // the call, including APIs registered dynamically by scripts.
    // api_list(pattern, ...) - regex search over all api docs, like helpall().
    internal sealed class ApiListCommand : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(DslCalculator calculator, IList<BoxedValue> operands)
        {
            var regexes = new List<System.Text.RegularExpressions.Regex>();
            foreach (var op in operands)
            {
                string pattern = op.ToString();
                if (!string.IsNullOrEmpty(pattern))
                    regexes.Add(new System.Text.RegularExpressions.Regex(pattern,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            }
            var docs = calculator.ApiDocs;
            var sb = new StringBuilder();
            int total = docs == null ? 0 : docs.Count;
            int shown = 0;
            sb.AppendLine(string.Format("api_list: channel=runtime source=Calculator.ApiDocs total={0} patterns={1}",
                total, regexes.Count));
            if (docs != null)
            {
                foreach (var pair in docs)
                {
                    bool match = regexes.Count == 0;
                    string info = string.Format("{0}: {1}", pair.Key, pair.Value);
                    foreach (var regex in regexes)
                    {
                        if (regex.IsMatch(info)) { match = true; break; }
                    }
                    if (match)
                    {
                        sb.AppendLine(info);
                        ++shown;
                    }
                }
            }
            sb.AppendLine("shown=" + shown);
            return BoxedValue.FromString(sb.ToString());
        }
    }
}
