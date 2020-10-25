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

public class AiKeepAway : ISimpleStoryCommandPlugin
{
    public ISimpleStoryCommandPlugin Clone()
    {
        return new AiKeepAway();
    }

    public void ResetState()
    {
        m_KeepAwayStarted = false;
    }

    public bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params, long delta)
    {
        var args = _params.Values;
        if (!m_KeepAwayStarted) {
            m_KeepAwayStarted = true;

            m_ObjId = args[0];
            m_SkillInfo = args[1].ObjectVal as SkillInfo;
            m_Ratio = args[2].Get<float>();
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
                ScriptRuntime.Vector3 dir = srcPos - targetPos;
                dir.Normalize();
                targetPos = targetPos + dir * m_Ratio * m_SkillInfo.Distance;
                if (distSqr < m_Ratio * m_Ratio * m_SkillInfo.Distance * m_SkillInfo.Distance) {
                    AiCommand.AiPursue(npc, targetPos);
                    return true;
                }
            }
        }
        return false;
    }

    private int m_ObjId = 0;
    private SkillInfo m_SkillInfo = null;
    private float m_Ratio = 1.0f;
    private bool m_KeepAwayStarted = false;
}
