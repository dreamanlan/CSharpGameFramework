using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 10008: one-shot transport message per user entering a circular area.
    public sealed class SceneLogic_Transport : AbstractSceneLogic
    {
        private sealed class State { public HashSet<int> Triggered = new HashSet<int>(); }
        public override void Execute(SceneLogicInfo info, long deltaTime)
        {
            if (null == info || info.IsLogicFinished || info.IsLogicPaused || null == info.EntityManager) return;
            info.Time += deltaTime;
            if (info.Time < 1000) return;
            info.Time = 0;
            State st = info.LogicDatas.GetData<State>();
            if (null == st) { st = new State(); info.LogicDatas.AddData<State>(st); }
            List<float> vals = SceneLogicParamHelper.Numbers(info);
            for (LinkedListNode<EntityInfo> node = info.EntityManager.Entities.FirstNode; null != node; node = node.Next) {
                EntityInfo e = node.Value;
                if (!SceneLogicParamHelper.IsUser(e) || e.IsDead()) continue;
                Vector3 pos = SceneLogicParamHelper.Position(e);
                if (!SceneLogicParamHelper.InCircle(vals, pos.X, pos.Z)) continue;
                if (st.Triggered.Add(e.GetId())) {
                    SceneLogicSendStoryMessageStatic.Send(info, "transport:" + info.ConfigId, e.GetId());
                }
            }
        }
    }
}
