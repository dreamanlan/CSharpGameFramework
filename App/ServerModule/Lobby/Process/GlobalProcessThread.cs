using System;
using System.Collections.Generic;
using GameFramework;
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
  /// 全局数据（非玩家拥有的数据）处理线程，在这里处理邮件、远征、帮会等数据。
  /// </summary>
  /// <remarks>
  /// 其它线程不应直接调用此类方法，应通过QueueAction发起调用。
  /// </remarks>
  internal sealed class GlobalProcessThread : MyServerThread
  {
    //=========================================================================================
    //全局数据初始化方法，由其它线程同步调用。
    //=========================================================================================

    //=========================================================================================
    //同步调用方法部分，其它线程可直接调用(需要考虑多线程安全)。
    //=========================================================================================
    
    //=========================================================================================
    //异步调用方法部分，需要通过QueueAction调用。
    //=========================================================================================

    //=========================================================================================
    //内部实现。
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
