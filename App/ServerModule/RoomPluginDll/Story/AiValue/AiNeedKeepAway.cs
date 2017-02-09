using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using SkillSystem;
using StorySystem;

public class AiNeedKeepAway : ISimpleStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryValuePlugin Clone()
    {
        return new AiNeedKeepAway();
    }
    public void Evaluate(StoryInstance instance, StoryValueParams _params)
    {
        Scene scene = instance.Context as Scene;
        if (null != scene) {
            ArrayList args = _params.Values;
            int objId = (int)args[0];
            SkillInfo skillInfo = args[1] as SkillInfo;
            float ratio = (float)System.Convert.ChangeType(args[2], typeof(float));
            EntityInfo npc = scene.GetEntityById(objId);
            if (null != npc && null != skillInfo) {
                int targetId = npc.GetAiStateInfo().Target;
                if (targetId > 0) {
                    EntityInfo target = scene.GetEntityById(targetId);
                    if (null != target) {
                        float distSqr = Geometry.DistanceSquare(npc.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D());
                        if (distSqr < ratio * ratio * skillInfo.Distance * skillInfo.Distance) {
                            m_Proxy.Value = 1;
                            return;
                        }
                    }
                }
            }
            m_Proxy.Value = 0;
        }
    }

    private StoryValueResult m_Proxy = null;
}
