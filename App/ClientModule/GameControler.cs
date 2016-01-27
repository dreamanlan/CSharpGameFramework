//#define USE_DISK_LOG

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace GameFramework
{
    public sealed class GameControler
    {
        internal sealed class Logger : IDisposable
        {
            internal bool Enabled
            {
                get { return m_Enabled; }
                set { m_Enabled = value; }
            }
            internal void Log(string format, params object[] args)
            {
                if (!m_Enabled)
                    return;
                string msg = DateTime.Now.ToString("HH-mm-ss-fff:") + string.Format(format, args);
#if USE_DISK_LOG
        m_LogStream.WriteLine(msg);
        m_LogStream.Flush();
#else
                m_LogQueue.Enqueue(msg);
                if (!m_IsInRequestFlush && m_LogQueue.Count >= c_FlushCount) {
                    m_LastFlushTime = TimeUtility.GetLocalMilliseconds();

                    RequestFlush();
                }
#endif
            }
            internal void Init(string logPath)
            {
                string logFile = string.Format("{0}/Game.log", logPath);
                m_LogStream = new StreamWriter(logFile, false);
#if !USE_DISK_LOG
                m_LogQueue = m_LogQueues[m_CurQueueIndex];
                m_Thread.OnQuitEvent = OnThreadQuit;
                m_Thread.Start();
#endif
                Log("======GameLog Start ({0}, {1})======", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }
            public void Dispose()
            {
                Release();
            }
            internal void Tick()
            {
#if !USE_DISK_LOG
                long curTime = TimeUtility.GetLocalMilliseconds();
                if (m_LastFlushTime + 10000 < curTime) {
                    m_LastFlushTime = curTime;

                    RequestFlush();
                }
#endif
            }
            private void Release()
            {
#if !USE_DISK_LOG
                m_Thread.Stop();
#endif
                m_LogStream.Close();
                m_LogStream.Dispose();
            }
#if !USE_DISK_LOG
            private void RequestFlush()
            {
                m_IsInRequestFlush = true;
                lock (m_LogQueueLock) {
                    m_Thread.QueueActionWithDelegation((MyAction<Queue<string>>)FlushToFile, m_LogQueue);
                    m_CurQueueIndex = 1 - m_CurQueueIndex;
                    m_LogQueue = m_LogQueues[m_CurQueueIndex];
                }
                m_IsInRequestFlush = false;
            }
            private void OnThreadQuit()
            {
                FlushToFile(m_LogQueue);
            }
            private void FlushToFile(Queue<string> logQueue)
            {
                lock (m_LogQueueLock) {
                    while (logQueue.Count > 0) {
                        string msg = logQueue.Dequeue();
                        m_LogStream.WriteLine(msg);
                    }
                    m_LogStream.Flush();
                }
            }

            private Queue<string>[] m_LogQueues = new Queue<string>[] { new Queue<string>(), new Queue<string>() };
            private MyClientThread m_Thread = new MyClientThread();
            private int m_CurQueueIndex = 0;
            private Queue<string> m_LogQueue;
            private object m_LogQueueLock = new object();

            private long m_LastFlushTime = 0;
            private const int c_FlushCount = 4096;
            private bool m_IsInRequestFlush = false;
#endif
            private StreamWriter m_LogStream;
            private bool m_Enabled = true;
        }
        //----------------------------------------------------------------------
        // 标准接口
        //----------------------------------------------------------------------
        public static bool IsInited
        {
            get { return s_IsInited; }
        }
        public static void Init(string logPath, string dataPath)
        {
            s_IsInited = true;
            s_Logger.Init(logPath);
            HomePath.CurHomePath = dataPath;
            GlobalVariables.Instance.IsDebug = false;

            LogSystem.OnOutput = (Log_Type type, string msg) => {
#if DEBUG
                if (type == Log_Type.LT_Warn) {
                    Utility.GfxLog("{0}", msg);
                } else if (type == Log_Type.LT_Error) {
                    Utility.GfxErrorLog("{0}", msg);
                }
#endif
                s_Logger.Log("{0}", msg);
            };

            Utility.GfxLog("GameControler.Init");
        }
        public static void InitGame()
        {
            Utility.GfxLog("GameControler.InitGame");
            ClientModule.Instance.Init();
        }
        public static void PauseGame(bool isPause)
        {
            s_IsPaused = isPause;
        }
        public static void PauseGameForeground(bool isPause)
        {
            s_IsPausedForeground = isPause;
        }
        public static void Release()
        {
            Utility.GfxLog("GameControler.Release");
            ClientModule.Instance.Release();
            s_Logger.Dispose();
        }
        public static void TickGame()
        {
            try {
                UnityEngine.Profiler.BeginSample("GameController.TickGame");
                TimeUtility.GfxTime = UnityEngine.Time.time;
                TimeUtility.GfxTimeScale = UnityEngine.Time.timeScale;
                ClientModule.Instance.Tick();
                s_Logger.Tick();
            } finally {
                UnityEngine.Profiler.EndSample();
            }
        }

        internal static bool IsPaused
        {
            get
            {
                return s_IsPaused;
            }
        }
        internal static bool IsPausedForeground
        {
            get
            {
                return s_IsPausedForeground;
            }
        }

        private static Logger s_Logger = new Logger();
        private static bool s_IsInited = false;
        private static bool s_IsPaused = false;
        private static bool s_IsPausedForeground = false;
    }
}

