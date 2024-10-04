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

public class AiRandMove : ISimpleStoryCommandPlugin
{
    public ISimpleStoryCommandPlugin Clone()
    {
        return new AiRandMove();
    }

    public void ResetState()
    {
        m_ParamReaded = false;
        m_PursueInterval = 0;
    }

    public bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryFunctionParams _params, long delta)
    {
        var args = _params.Values;
        if (!m_ParamReaded) {
            m_ParamReaded = true;

            m_ObjId = args[0].GetInt();
            m_Time = args[1].GetInt();
            m_Radius = args[2].GetInt();

            EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
            if (null != npc && !npc.IsUnderControl()) {
                SelectTargetPos(npc);
                return true;
            }
        } else {
            EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
            if (null != npc && !npc.IsUnderControl()) {
                AiStateInfo info = npc.GetAiStateInfo();
                return RandMoveHandler(npc, info, delta);
            }
        }
        return false;
    }

    private bool RandMoveHandler(EntityInfo npc, AiStateInfo info, long deltaTime)
    {
        info.Time += deltaTime;
        m_PursueInterval += deltaTime;
        if (info.Time > m_Time) {
            info.Time = 0;
            npc.GetMovementStateInfo().IsMoving = false;
            AiCommand.AiStopPursue(npc);
            info.ChangeToState((int)PredefinedAiStateId.Idle);
            EntityInfo target = PluginFramework.Instance.GetEntityById(info.Target);
            if (null != target) {
                float dir = Geometry.GetYRadian(npc.GetMovementStateInfo().GetPosition3D(), target.GetMovementStateInfo().GetPosition3D());
                npc.GetMovementStateInfo().SetFaceDir(dir);
            }
            return false;
        }
        if (m_PursueInterval < 100) {
            return true;
        } else {
            m_PursueInterval = 0;
        }

        ScriptRuntime.Vector3 targetPos = npc.GetMovementStateInfo().TargetPosition;
        ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
        float distSqr = Geometry.DistanceSquare(srcPos, targetPos);
        if (distSqr <= 1) {
            if (npc.GetMovementStateInfo().IsMoving) {
                npc.GetMovementStateInfo().IsMoving = false;
                AiCommand.AiStopPursue(npc);
                info.ChangeToState((int)PredefinedAiStateId.Idle);
            }
        } else {
            npc.GetMovementStateInfo().IsMoving = true;
            AiCommand.AiPursue(npc, targetPos);
        }
        return true;
    }

    private void SelectTargetPos(EntityInfo npc)
    {
        ScriptRuntime.Vector3 pos = npc.GetMovementStateInfo().GetPosition3D();
        float dx = Helper.Random.Next(m_Radius) - m_Radius / 2;
        float dz = Helper.Random.Next(m_Radius) - m_Radius / 2;
        pos.X += dx;
        pos.Z += dz;
        npc.GetMovementStateInfo().TargetPosition = AiCommand.AiGetValidPosition(npc, pos, m_Radius);
    }

    private int m_ObjId = 0;
    private long m_Time = 0;
    private int m_Radius = 0;
    private bool m_ParamReaded = false;

    private long m_PursueInterval = 0;
}
