using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework.Plugin;
using ScriptableFramework.Skill;
using DotnetSkillScript;
using DotnetStoryScript;

public class AiGohome : ISimpleStoryCommandPlugin
{
    public ISimpleStoryCommandPlugin Clone()
    {
        return new AiGohome();
    }

    public void ResetState()
    {
        m_ParamReaded = false;
    }

    public bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params, long delta)
    {
        var args = _params.Values;
        if (!m_ParamReaded) {
            m_ParamReaded = true;
            m_ObjId = args[0];
        }
        EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
        if (null != npc) {
            AiStateInfo info = npc.GetAiStateInfo();
            return GohomeHandler(npc, info, delta);
        }
        return false;
    }

    private bool GohomeHandler(EntityInfo npc, AiStateInfo info, long deltaTime)
    {
        info.Time += deltaTime;
        if (info.Time > 100) {
            info.Time = 0;
        } else {
            return true;
        }

        ScriptRuntime.Vector3 targetPos = info.HomePos;
        ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
        float distSqr = Geometry.DistanceSquare(srcPos, info.HomePos);
        if (distSqr <= 1) {
            npc.GetMovementStateInfo().IsMoving = false;
            AiCommand.AiStopPursue(npc);
            info.ChangeToState((int)PredefinedAiStateId.Idle);
            return false;
        } else {
            npc.GetMovementStateInfo().IsMoving = true;
            npc.GetMovementStateInfo().TargetPosition = targetPos;
            AiCommand.AiPursue(npc, targetPos);
        }
        return true;
    }

    private int m_ObjId = 0;
    private bool m_ParamReaded = false;
}
