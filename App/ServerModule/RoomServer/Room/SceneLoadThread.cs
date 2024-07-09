﻿using System;
using System.Threading;
using System.Collections.Generic;

namespace ScriptableFramework
{
  public class SceneLoadThread : MyServerThread
  {
    protected override void OnStart()
    {      
      TickSleepTime = 10;
      LogSys.Log(ServerLogType.DEBUG, "scene load thread start.");
    }

    protected override void OnTick()
    {
      try {
        long curTime = TimeUtility.GetLocalMilliseconds();        
        if (m_LastTickTime != 0) {
          long elapsedTickTime = curTime - m_LastTickTime;
          if (elapsedTickTime > c_WarningTickTime) {
            LogSys.Log(ServerLogType.MONITOR, "SceneLoadThread Tick:{0}", elapsedTickTime);
          }
        }
        m_LastTickTime = curTime;

        if (m_LastLogTime + 60000 < curTime) {
          m_LastLogTime = curTime;

          DebugPoolCount((string msg) => {
            LogSys.Log(ServerLogType.INFO, "SceneLoadThread.ActionQueue {0}", msg);
          });
          LogSys.Log(ServerLogType.MONITOR, "SceneLoadThread.ActionQueue Current Action {0}", this.CurActionNum);
        }
      } catch (Exception ex) {
        LogSys.Log(ServerLogType.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
      }      
    }

    protected override void OnQuit()
    {
      
    }

    private long m_LastLogTime = 0;
    private const long c_WarningTickTime = 1000;
    private long m_LastTickTime = 0;

    public static SceneLoadThread Instance
    {
      get
      {
        return s_Instance;
      }
    }
    private static SceneLoadThread s_Instance = new SceneLoadThread();
  }
}
