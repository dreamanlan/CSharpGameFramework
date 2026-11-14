using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
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
                //execution logic
                DateTime time = DateTime.Now;
                if (null != data && (time.Hour != data.m_LastHour || time.Minute != data.m_LastMinute)) {
                    data.m_LastHour = time.Hour;
                    data.m_LastMinute = time.Minute;
                    var args = SceneLogicNewBoxedValueList(info);
                    args.Add(time.Day);
                    args.Add((int)time.DayOfWeek);
                    args.Add(time.Hour);
                    args.Add(time.Minute);
                    SceneLogicSendStoryMessage(info, "sandclock:" + info.ConfigId, args);
                }
            }
        }
    }
}
