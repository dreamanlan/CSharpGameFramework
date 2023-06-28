
using System;
//using System.Diagnostics;

namespace GameFramework
{
    public enum GameLogType
    {
        Debug,
        Info,
        Warn,
        Error,
        Assert,
        GM,
    }
    public delegate void LogSystemOutputDelegation(GameLogType type, string msg);

    public class LogSystem
    {
        public static LogSystemOutputDelegation OnOutput;
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Debug(string format, params object[] args)
        {
#if DEBUG
            if (!GlobalVariables.Instance.IsDevice || GlobalVariables.Instance.IsDebug) {
                format = "Time:" + DateTime.Now.ToString("HH-mm-ss-fff:") + format;
                string str = format;
                if (args.Length > 0)
                    str = string.Format("[Debug]:" + format, args);
                Output(GameLogType.Debug, str);
            }
#endif
        }
        public static void Info(string format, params object[] args)
        {
            if (!GlobalVariables.Instance.IsDevice || GlobalVariables.Instance.IsDebug) {
                format = "Time:" + DateTime.Now.ToString("HH-mm-ss-fff:") + format;
                string str = format;
                if (args.Length > 0)
                    str = string.Format("[Info]:" + format, args);
                Output(GameLogType.Info, str);
            }
        }
        public static void Warn(string format, params object[] args)
        {
            if (!GlobalVariables.Instance.IsDevice || GlobalVariables.Instance.IsDebug) {
                format = "Time:" + DateTime.Now.ToString("HH-mm-ss-fff:") + format;
                string str = format;
                if (args.Length > 0)
                    str = string.Format("[Warn]:" + format, args);
                Output(GameLogType.Warn, str);
            }
        }
        public static void Error(string format, params object[] args)
        {
            format = "Time:" + DateTime.Now.ToString("HH-mm-ss-fff:") + format;
            string str = format;
            if (args.Length > 0)
                str = string.Format("[Error]:" + format, args);
            Output(GameLogType.Error, str);
        }
        public static void Assert(bool check, string format, params object[] args)
        {
            if (!GlobalVariables.Instance.IsDevice || GlobalVariables.Instance.IsDebug) {
                if (!check) {
                    format = "Time:" + DateTime.Now.ToString("HH-mm-ss-fff:") + format;
                    string str = format;
                    if (args.Length > 0)
                        str = string.Format("[Assert]:" + format, args);
                    Output(GameLogType.Assert, str);
                }
            }
        }

        public static void Log(string msg)
        {
            if (!GlobalVariables.Instance.IsDevice || GlobalVariables.Instance.IsDebug) {
                Output(GameLogType.Info, msg);
            }
        }
        public static void GmLog(string format, params object[] args)
        {
            format = "Time:" + DateTime.Now.ToString("HH-mm-ss-fff:") + format;
            string str = format;
            if (args.Length > 0)
                str = string.Format("[GmLog]:" + format, args);
            Output(GameLogType.GM, str);
        }

        private static void Output(GameLogType type, string msg)
        {
            if (null != OnOutput) {
                OnOutput(type, msg);
            }
        }
    }
}
