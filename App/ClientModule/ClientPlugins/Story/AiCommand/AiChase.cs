using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework.Plugin;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript;

public class AiChase : ISimpleStoryCommandPlugin
{
    public ISimpleStoryCommandPlugin Clone()
    {
        return new AiChase();
    }

    public void ResetState()
    {
        m_ChaseStarted = false;
    }

    public bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryFunctionParams _params, long delta)
    {
        var args = _params.Values;
        if (!m_ChaseStarted) {
            m_ChaseStarted = true;

            m_ObjId = args[0];
            m_SkillInfo = args[1].ObjectVal as SkillInfo;
        }
        EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
        if (null != npc && !npc.IsUnderControl()) {
            AiStateInfo info = npc.GetAiStateInfo();
            EntityInfo target = PluginFramework.Instance.GetEntityById(info.Target);
            if (null != target && null != m_SkillInfo) {
                info.Time += delta;
                if (info.Time > 100) {
                    info.Time = 0;
                } else {
                    return true;
                }
                ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
                ScriptRuntime.Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
                float distSqr = Geometry.DistanceSquare(srcPos, targetPos);
                if (distSqr > m_SkillInfo.Distance * m_SkillInfo.Distance) {
                    AiCommand.AiPursue(npc, targetPos);
                    return true;
                }
            }
        }
        return false;
    }

    private int m_ObjId = 0;
    private SkillInfo m_SkillInfo = null;
    private bool m_ChaseStarted = false;
}
