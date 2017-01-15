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

public class AiCastSkill : ISimpleStoryCommandPlugin
{
    public ISimpleStoryCommandPlugin Clone()
    {
        return new AiCastSkill();
    }

    public void ResetState()
    {
        m_ParamReaded = false;
        m_SkillCasted = false;
    }

    public bool ExecCommand(StoryInstance instance, StoryValueParams _params, long delta)
    {
        ArrayList args = _params.Values;
        if (!m_ParamReaded) {
            m_ObjId = (int)args[0];
            m_SkillInfo = args[1] as SkillInfo;
        }
        if (!m_SkillCasted) {
            EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
            if (null != npc && !npc.IsUnderControl()) {
                int targetId = npc.GetAiStateInfo().Target;
                EntityInfo target = PluginFramework.Instance.GetEntityById(targetId);
                if (null != target && !target.IsDead() && Geometry.DistanceSquare(npc.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D()) <= m_SkillInfo.Distance * m_SkillInfo.Distance) {
                    ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
                    ScriptRuntime.Vector3 targetPos = target.GetMovementStateInfo().GetPosition3D();
                    float dir = Geometry.GetYRadian(srcPos, targetPos);
                    float curDir = npc.GetMovementStateInfo().GetFaceDir();
                    if (Mathf.Abs(dir - curDir) > 0.157f) {
                        npc.GetMovementStateInfo().SetWantedFaceDir(dir);
                    } else {
                        m_SkillCasted = true;
                        AiCommand.AiStopPursue(npc);
                        AiCommand.AiSkill(npc, m_SkillInfo.SkillId);
                    }
                    return true;
                } else if (!m_SkillInfo.ConfigData.skillData.needTarget) {
                    m_SkillCasted = true;
                    AiCommand.AiStopPursue(npc);
                    AiCommand.AiSkill(npc, m_SkillInfo.SkillId);
                }
            }
        } else {
            EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
            if (null != npc) {
                AiStateInfo info = npc.GetAiStateInfo();
                if (npc.GetSkillStateInfo().IsSkillActivated()) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        return false;
    }

    private int m_ObjId = 0;
    private SkillInfo m_SkillInfo = null;
    private bool m_SkillCasted = false;
    private bool m_ParamReaded = false;
}
