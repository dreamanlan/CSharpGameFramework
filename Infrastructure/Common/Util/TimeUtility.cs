﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ScriptableFramework
{
    public sealed class TimeUtility
    {
        public static int TimeCounterInterval
        {
            get { return s_TimeCounterInterval; }
        }
        public static double CurTimestamp
        {
            get { return (DateTime.Now.AddHours(-8) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds; }
        }
        public static long LobbyLastResponseTime
        {
            get { return s_LobbyLastResponseTime; }
            set { s_LobbyLastResponseTime = value; }
        }
        public static long LobbyAverageRoundtripTime
        {
            get { return s_LobbyAverageRoundtripTime; }
            set { s_LobbyAverageRoundtripTime = value; }
        }
        public static long LastResponseTime
        {
            get { return s_LastResponseTime; }
            set { s_LastResponseTime = value; }
        }
        public static long AverageRoundtripTime
        {
            get { return s_AverageRoundtripTime; }
            set { s_AverageRoundtripTime = value; }
        }
        public static long RemoteTimeOffset
        {
            get { return s_RemoteTimeOffset; }
            set { s_RemoteTimeOffset = value; }
        }
        public static float GfxFps
        {
            get { return s_GfxFps; }
            set { s_GfxFps = value; }
        }
        public static float GfxAvgFps
        {
            get { return s_GfxAvgFps; }
            set { s_GfxAvgFps = value; }
        }
        public static long LobbyGetMillisecondsFromLastResponse()
        {
            if (s_LobbyLastResponseTime <= 0)
                return 0;
            return GetLocalMilliseconds() - s_LobbyLastResponseTime;
        }
        public static long GetMillisecondsFromLastResponse()
        {
            if (s_LastResponseTime <= 0)
                return 0;
            return GetLocalMilliseconds() - s_LastResponseTime;
        }
        public static DateTime GetServerDateTime()
        {
            return DateTime.Now.AddMilliseconds(s_RemoteTimeOffset);
        }
        public static float GetTimeScale()
        {
            return s_GfxTimeScale;
        }
        public static long GetLocalMilliseconds()
        {
            if (GlobalVariables.Instance.IsClient)
                return s_GfxTime;
            else
                return (GetElapsedTimeUs() - s_Instance.m_StartTimeUs) / 1000;
        }
        public static long GetLocalRealMilliseconds()
        {
            if (GlobalVariables.Instance.IsClient)
                return s_GfxRealTime;
            else
                return (GetElapsedTimeUs() - s_Instance.m_StartTimeUs) / 1000;
        }
        public static long GetElapsedTimeUs()
        {
            //return DateTime.Now.Ticks / 10;
            return (long)(Stopwatch.GetTimestamp() / s_TickPerUs);
        }
        public static void UpdateGfxTime(float time, float realTime, float timeScale)
        {
            s_GfxTime = (long)(time * 1000);
            s_GfxRealTime = (long)(realTime * 1000);
            s_GfxTimeScale = timeScale;
        }

        private static long s_LobbyLastResponseTime = 0;
        private static long s_LobbyAverageRoundtripTime = 0;
        private static long s_LastResponseTime = 0;
        private static long s_AverageRoundtripTime = 0;
        private static long s_RemoteTimeOffset = 0;
        private static float s_GfxFps = 0;
        private static float s_GfxAvgFps = 0;
        private static long s_GfxTime = 0;
        private static long s_GfxRealTime = 0;
        private static float s_GfxTimeScale = 1.0f;
        private static double s_TickPerUs = Stopwatch.Frequency / 1000000.0;
        private static TimeUtility s_Instance = new TimeUtility();
        private static int s_TimeCounterInterval = 5000;

        private TimeUtility()
        {
            m_StartTimeUs = GetElapsedTimeUs();
        }
        private long m_StartTimeUs = 0;
    }

    public sealed class TimeSnapshot
    {
        public static void Start()
        {
            Instance.Start_();
        }
        public static long End()
        {
            return Instance.End_();
        }
        public static long DoCheckPoint()
        {
            return Instance.DoCheckPoint_();
        }

        private void Start_()
        {
            m_LastSnapshotTime = TimeUtility.GetElapsedTimeUs();
            m_StartTime = m_LastSnapshotTime;
        }
        private long End_()
        {
            m_EndTime = TimeUtility.GetElapsedTimeUs();
            return m_EndTime - m_StartTime;
        }
        private long DoCheckPoint_()
        {
            long curTime = TimeUtility.GetElapsedTimeUs();
            long ret = curTime - m_LastSnapshotTime;
            m_LastSnapshotTime = curTime;
            return ret;
        }

        private long m_StartTime = 0;
        private long m_LastSnapshotTime = 0;
        private long m_EndTime = 0;

        private static TimeSnapshot Instance
        {
            get
            {
                if (null == s_Instance) {
                    s_Instance = new TimeSnapshot();
                }
                return s_Instance;
            }
        }

        [ThreadStatic]
        private static TimeSnapshot s_Instance = null;
    }
}
