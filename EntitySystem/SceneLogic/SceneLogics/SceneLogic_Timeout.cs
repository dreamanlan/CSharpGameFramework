using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace GameFramework
{
  public class SceneLogic_Timeout : AbstractSceneLogic
  {
    public override void Execute(SceneLogicInfo info, long deltaTime)
    {
      if (null == info || info.IsLogicFinished || info.IsLogicPaused) return;
      info.Time += deltaTime;
      if (info.Time > 1000) {
        TimeoutLogicInfo data = info.LogicDatas.GetData<TimeoutLogicInfo>();
        if (null == data) {
          data = new TimeoutLogicInfo();
          info.LogicDatas.AddData<TimeoutLogicInfo>(data);

          SceneLogicConfig sc = info.SceneLogicConfig;
          if (null != sc) {
            data.m_Timeout = long.Parse(sc.m_Params[0]);
          }
        }
        data.m_CurTime += info.Time;
        info.Time = 0;
        //执行逻辑
        if (null != data && !data.m_IsTriggered && data.m_CurTime >= data.m_Timeout) {
          data.m_IsTriggered = true;
          SceneLogicSendStoryMessage(info, "timeout:" + info.ConfigId, data.m_Timeout);
        }
      }
    }
  }
  public class SceneLogic_SandClock : AbstractSceneLogic
  {
    public override void Execute(SceneLogicInfo info, long deltaTime)
    {
      if (null == info || info.IsLogicFinished || info.IsLogicPaused) return;
      info.Time += deltaTime;
      if (info.Time >= 1000) {
        info.Time = 0;
        SandClockLogicInfo data = info.LogicDatas.GetData<SandClockLogicInfo>();
        if (null == data) {
          data = new SandClockLogicInfo();
          info.LogicDatas.AddData<SandClockLogicInfo>(data);
        }
        //执行逻辑
        DateTime time = DateTime.Now;
        if (null != data && (time.Hour != data.m_LastHour || time.Minute != data.m_LastMinute)) {
          data.m_LastHour = time.Hour;
          data.m_LastMinute = time.Minute;
          SceneLogicSendStoryMessage(info, "sandclock:" + info.ConfigId, time.Day, (int)time.DayOfWeek, time.Hour, time.Minute);
        }
      }
    }
  }
}
