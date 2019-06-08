using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using SkillSystem;
using StorySystem;

public class AiNeedChase : ISimpleStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryValuePlugin Clone()
    {
        return new AiNeedChase();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params)
    {
        ArrayList args = _params.Values;
        int objId = (int)args[0];
        SkillInfo skillInfo = args[1] as SkillInfo;
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc && null != skillInfo) {
            int targetId = npc.GetAiStateInfo().Target;
            if (targetId > 0) {
                EntityInfo target = PluginFramework.Instance.GetEntityById(targetId);
                if (null != target) {
                    float distSqr = Geometry.DistanceSquare(npc.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D());
                    if (distSqr > skillInfo.Distance * skillInfo.Distance) {
                        m_Proxy.Value = 1;
                        return;
                    }
                }
            }
        }
        m_Proxy.Value = 0;
    }

    private StoryValueResult m_Proxy = null;
}
