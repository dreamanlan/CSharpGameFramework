using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 10004: periodically report users standing on a circular revive point.
    public sealed class SceneLogic_RevivePoint : AbstractSceneLogic
    {
        private sealed class State { public HashSet<int> Reported = new HashSet<int>(); }
        public override void Execute(SceneLogicInfo info, long deltaTime)
        {
            if (null == info || info.IsLogicFinished || info.IsLogicPaused || null == info.EntityManager) return;
            info.Time += deltaTime;
            if (info.Time < 2000) return;
            info.Time = 0;
            State st = info.LogicDatas.GetData<State>();
            if (null == st) { st = new State(); info.LogicDatas.AddData<State>(st); }
            List<float> vals = SceneLogicParamHelper.Numbers(info);
            for (LinkedListNode<EntityInfo> node = info.EntityManager.Entities.FirstNode; null != node; node = node.Next) {
                EntityInfo e = node.Value;
                if (!SceneLogicParamHelper.IsUser(e) || e.IsDead()) continue;
                Vector3 pos = SceneLogicParamHelper.Position(e);
                if (!SceneLogicParamHelper.InCircle(vals, pos.X, pos.Z)) continue;
                if (st.Reported.Add(e.GetId())) {
                    SceneLogicSendStoryMessageStatic.Send(info, "revivepoint:" + info.ConfigId, e.GetId());
                }
            }
        }
    }
}
