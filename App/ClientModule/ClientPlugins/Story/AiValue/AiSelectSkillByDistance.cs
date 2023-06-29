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

public class AiSelectSkillByDistance : ISimpleStoryValuePlugin
{
    public void SetProxy(StoryValueResult result)
    {
        m_Proxy = result;
    }
    public ISimpleStoryValuePlugin Clone()
    {
        return new AiSelectSkillByDistance();
    }
    public void Evaluate(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params)
    {
        var args = _params.Values;
        int objId = args[0];
        EntityInfo npc = PluginFramework.Instance.GetEntityById(objId);
        if (null != npc) {
            int targetId = npc.GetAiStateInfo().Target;
            if (targetId > 0) {
                EntityInfo target = PluginFramework.Instance.GetEntityById(targetId);
                if (null != target) {
                    float distToTarget = Geometry.Distance(npc.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D());
                    SkillInfo maxSkillInfo = null;
                    float diffDist = float.MaxValue;
                    SkillInfo targetSkillInfo = null;
                    for (int i = 0; i < npc.AutoSkillIds.Count; ++i) {
                        int skillId = npc.AutoSkillIds[i];
                        SkillInfo temp = npc.GetSkillStateInfo().GetSkillInfoById(skillId);
                        if (null != temp) {
                            float dist = temp.Distance;
                            float absDist = Mathf.Abs(distToTarget - dist);
                            if (diffDist > absDist) {
                                diffDist = absDist;
                                targetSkillInfo = temp;
                            }
                        }
                    }
                    if (null != targetSkillInfo)
                        m_Proxy.Value = BoxedValue.FromObject(targetSkillInfo);
                    else
                        m_Proxy.Value = BoxedValue.FromObject(maxSkillInfo);
                    return;
                }
            }
        }
        m_Proxy.Value = BoxedValue.NullObject;
    }

    private StoryValueResult m_Proxy = null;
}
