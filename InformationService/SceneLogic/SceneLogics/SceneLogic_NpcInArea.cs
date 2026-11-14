using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // 10006: kill NPCs that left the configured polygon.
    public sealed class SceneLogic_NpcInArea : AbstractSceneLogic
    {
        public override void Execute(SceneLogicInfo info, long deltaTime)
        {
            if (null == info || info.IsLogicFinished || info.IsLogicPaused || null == info.EntityManager) return;
            info.Time += deltaTime;
            if (info.Time < 100) return;
            info.Time = 0;
            List<float> vals = SceneLogicParamHelper.Numbers(info);
            int count = vals.Count / 2;
            for (LinkedListNode<EntityInfo> node = info.EntityManager.Entities.FirstNode; null != node; node = node.Next) {
                EntityInfo e = node.Value;
                if (null == e || e.IsDead() || SceneLogicParamHelper.IsUser(e)) continue;
                Vector3 pos = SceneLogicParamHelper.Position(e);
                if (SceneLogicParamHelper.InPolygon(vals, count, pos.X, pos.Z)) continue;
                e.Hp = 0;
                SceneLogicSendStoryMessageStatic.Send(info, "npcleavearea", e.GetId());
            }
        }
    }
}
