using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 10003: one-shot message when users enter a polygon.
    public sealed class SceneLogic_UserEnterArea : AbstractSceneLogic
    {
        private sealed class State { public bool Triggered; public HashSet<int> Users = new HashSet<int>(); }
        public override void Execute(SceneLogicInfo info, long deltaTime)
        {
            if (null == info || info.IsLogicFinished || info.IsLogicPaused || null == info.EntityManager) return;
            info.Time += deltaTime;
            if (info.Time < 100) return;
            info.Time = 0;
            State st = info.LogicDatas.GetData<State>();
            if (null == st) { st = new State(); info.LogicDatas.AddData<State>(st); }
            if (st.Triggered) return;
            List<float> vals = SceneLogicParamHelper.Numbers(info);
            int count = vals.Count / 2;
            string mode = SceneLogicParamHelper.LastWord(info, "any");
            int entered = 0;
            for (LinkedListNode<EntityInfo> node = info.EntityManager.Entities.FirstNode; null != node; node = node.Next) {
                EntityInfo e = node.Value;
                if (!SceneLogicParamHelper.IsUser(e) || e.IsDead()) continue;
                Vector3 pos = SceneLogicParamHelper.Position(e);
                if (!SceneLogicParamHelper.InPolygon(vals, count, pos.X, pos.Z)) continue;
                if (st.Users.Add(e.GetId())) {
                    ++entered;
                    if (mode != "all") SceneLogicSendStoryMessageStatic.Send(info, "anyuserenterarea:" + info.ConfigId, e.GetId());
                }
            }
            if (entered > 0 && mode == "all" && st.Users.Count >= 1) {
                SceneLogicSendStoryMessageStatic.Send2(info, "alluserenterarea:" + info.ConfigId, st.Users.Count, entered);
                st.Triggered = true;
                info.IsLogicFinished = true;
            } else if (entered > 0 && mode != "all") {
                st.Triggered = true;
                info.IsLogicFinished = true;
            }
        }
    }
}
