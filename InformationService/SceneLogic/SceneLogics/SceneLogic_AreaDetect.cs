using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 10005: one-shot area detection, collecting entity ids matching the configured target type.
    public sealed class SceneLogic_AreaDetect : AbstractSceneLogic
    {
        private sealed class State { public bool Triggered; public long CurTime; public long Timeout; }
        public override void Execute(SceneLogicInfo info, long deltaTime)
        {
            if (null == info || info.IsLogicFinished || info.IsLogicPaused || null == info.EntityManager) return;
            State st = info.LogicDatas.GetData<State>();
            if (null == st) {
                st = new State();
                info.LogicDatas.AddData<State>(st);
                List<float> vals = SceneLogicParamHelper.Numbers(info);
                st.Timeout = vals.Count > 0 ? (long)vals[vals.Count - 1] : 0;
            }
            if (st.Triggered) return;
            st.CurTime += deltaTime;
            if (st.CurTime < st.Timeout) return;
            st.Triggered = true;
            info.IsLogicFinished = true;
            string mode = SceneLogicParamHelper.LastWord(info, "all");
            List<float> vals2 = SceneLogicParamHelper.Numbers(info);
            int count = vals2.Count / 2;
            if (null == AbstractSceneLogic.OnSceneLogicSendStoryMessage || null == AbstractSceneLogic.OnSceneLogicNewBoxedValueList) return;
            BoxedValueList args = AbstractSceneLogic.OnSceneLogicNewBoxedValueList(info);
            if (null == args) return;
            for (LinkedListNode<EntityInfo> node = info.EntityManager.Entities.FirstNode; null != node; node = node.Next) {
                EntityInfo e = node.Value;
                if (null == e || e.IsDead()) continue;
                bool isUser = SceneLogicParamHelper.IsUser(e);
                if (mode == "user" && !isUser) continue;
                if (mode == "npc" && isUser) continue;
                Vector3 pos = SceneLogicParamHelper.Position(e);
                if (SceneLogicParamHelper.InPolygon(vals2, count, pos.X, pos.Z)) args.Add(e.GetId());
            }
            AbstractSceneLogic.OnSceneLogicSendStoryMessage(info, "area_detect:" + info.ConfigId, args);
        }
    }
}
