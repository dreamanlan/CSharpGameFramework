using System;
using System.Collections.Generic;
using ScriptableFramework;
using GameFrameworkData;
using System.Text;

namespace Lobby
{
    enum GlobalParamEnum
    {
        LoginLotteryLastResetTime = 1,
        LootLastResetTime = 2,
        ConsumeId = 3,
        ConsumeResetTime = 4
    }
    /// <summary>
    /// Global data (data not owned by players) processing thread, where data such as mail, expeditions, gangs, etc. are processed.
    /// </summary>
    /// <remarks>
    /// Other threads should not call such methods directly, but should initiate calls through QueueAction.
    /// </remarks>
    internal sealed class GlobalProcessThread : MyServerThread
    {
        //=========================================================================================
        //Global data initialization method, called synchronously by other threads.
        //=========================================================================================

        //=========================================================================================
        //For the synchronous calling method part, other threads can call it directly (multi-thread safety needs to be considered).
        //=========================================================================================

        //=========================================================================================
        //The asynchronous calling method part needs to be called through QueueAction.
        //=========================================================================================

        //=========================================================================================
        //Implemented internally.
        //=========================================================================================
        protected override void OnStart()
        {
            TickSleepTime = 10;
            ActionNumPerTick = 10240;

        }
        protected override void OnTick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime != 0) {
                long elapsedTickTime = curTime - m_LastTickTime;
                if (elapsedTickTime > c_WarningTickTime) {
                    LogSys.Log(ServerLogType.MONITOR, "GlobalProcessThread Tick:{0}", elapsedTickTime);
                }
            }
            m_LastTickTime = curTime;
            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;

                DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "GlobalProcessThread.ActionQueue {0}", msg);
                });
                LogSys.Log(ServerLogType.MONITOR, "GlobalProcessThread.ActionQueue Current Action {0}", this.CurActionNum);
            }
        }
        protected override void OnQuit()
        {
        }

        private Dictionary<string, string> GetGlobalParams()
        {
            Dictionary<string, string> paramsDict = new Dictionary<string, string>();
            return paramsDict;
        }
        private const long c_WarningTickTime = 1000;
        private long m_LastTickTime = 0;
        private long m_LastLogTime = 0;
    }
}
