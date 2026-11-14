using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using StoryScript;
using System.Diagnostics;
using UnityEngine;

namespace GmCommands
{
    public sealed class Logger : IDisposable
    {
        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }
        public void Log(string format, params object[] args)
        {
            try {
                if (!m_Enabled)
                    return;
                //dont log to file on PC (There may be multiple processes writing logs at the same time)
                if (!Application.isEditor && !Application.isMobilePlatform && !Application.isConsolePlatform)
                    return;
                string msg = DateTime.Now.ToString("HH-mm-ss-fff:") + string.Format(format, args);
                lock (m_Lock) {
                    m_LogBuilder.Append(msg);
                    m_LogBuilder.AppendLine();
                    Flush(false);
                }
            }
            catch (Exception ex) {
                UnityEngine.Debug.LogErrorFormat("exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        public void Init(string logPath, string suffix)
        {
            try {
#if UNITY_EDITOR
                if (!Directory.Exists("Logs")) {
                    Directory.CreateDirectory("Logs");
                }
                string logFile;
                if (Application.isPlaying) {
                    logFile = string.Format("Logs/StoryScript{0}.log", suffix);
                }
                else {
                    logFile = string.Format("Logs/EditorStoryScript{0}.log", suffix);
                }
#else
                string logFile = string.Format("{0}/StoryScript{1}.log", logPath, suffix);
#endif
                m_LogFile = logFile;
                Log("======StoryScript Logger Start ({0}, {1})======", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }
            catch (Exception ex) {
                UnityEngine.Debug.LogErrorFormat("exception:{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
        public void Dispose()
        {
            lock (m_Lock) {
                if (m_LogBuilder.Length > 0) {
                    Flush(true);
                }
                m_LogBuilder.Clear();
            }
        }

        private void Flush(bool force)
        {
            if (force || m_LogBuilder.Length > c_FlushSize) {
                using (var logStream = new StreamWriter(m_LogFile, true)) {
                    logStream.Write(m_LogBuilder.ToString());
                    logStream.Close();
                }
            }
        }

        private object m_Lock = new object();
        private StringBuilder m_LogBuilder = new StringBuilder();
        private string m_LogFile = string.Empty;
        private bool m_Enabled = true;

        private const int c_FlushSize = 1 * 1024 * 1024;
    }
}
