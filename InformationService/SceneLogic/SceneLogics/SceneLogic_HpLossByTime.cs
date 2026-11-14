using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 10007: periodic percentage Hp loss for users (param = ratio of HpMax).
    public sealed class SceneLogic_HpLossByTime : AbstractSceneLogic
    {
        private sealed class State { public long LastHpLossTime; }
        public override void Execute(SceneLogicInfo info, long deltaTime)
        {
            if (null == info || info.IsLogicFinished || info.IsLogicPaused || null == info.EntityManager) return;
            info.Time += deltaTime;
            if (info.Time < 1000) return;
            info.Time = 0;
            State st = info.LogicDatas.GetData<State>();
            if (null == st) { st = new State(); info.LogicDatas.AddData<State>(st); }
            st.LastHpLossTime += 1000;
            List<float> vals = SceneLogicParamHelper.Numbers(info);
            float ratio = vals.Count > 0 ? vals[vals.Count - 1] : 0.0f;
            for (LinkedListNode<EntityInfo> node = info.EntityManager.Entities.FirstNode; null != node; node = node.Next) {
                EntityInfo e = node.Value;
                if (!SceneLogicParamHelper.IsUser(e) || e.IsDead()) continue;
                int loss = (int)(e.HpMax * ratio);
                if (loss <= 0) continue;
                e.Hp = e.Hp - loss;
                SceneLogicSendStoryMessageStatic.Send2(info, "hplossbytime:" + info.ConfigId, e.GetId(), loss);
            }
        }
    }
}
