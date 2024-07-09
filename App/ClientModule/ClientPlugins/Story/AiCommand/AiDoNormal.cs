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

public class AiDoNormal : ISimpleStoryCommandPlugin
{
    public ISimpleStoryCommandPlugin Clone()
    {
        return new AiDoNormal();
    }

    public void ResetState()
    {
        m_ParamReaded = false;
        m_EnableLearning = false;
    }

    public bool ExecCommand(StoryInstance instance, StoryMessageHandler handler, StoryValueParams _params, long delta)
    {
        var args = _params.Values;
        if (!m_ParamReaded) {
            m_ParamReaded = true;
            m_ObjId = args[0];
            if (args.Count > 1) {
                m_EnableLearning = args[1].GetInt() != 0;
            }
        }
        EntityInfo npc = PluginFramework.Instance.GetEntityById(m_ObjId);
        if (null != npc && !npc.IsUnderControl()) {
            AiStateInfo info = npc.GetAiStateInfo();
            switch (info.CurState) {
                case (int)PredefinedAiStateId.Idle:
                    info.ChangeToState((int)AiStateId.Combat);
                    return true;
                case (int)AiStateId.Combat:
                    return CombatHandler(npc, info, delta);
                case (int)AiStateId.Gohome:
                    return GohomeHandler(npc, info, delta);
            }
        }
        return false;
    }

    private bool CombatHandler(EntityInfo npc, AiStateInfo info, long deltaTime)
    {
        info.Time += deltaTime;
        if (info.Time > 100) {
            info.Time = 0;
        } else {
            return true;
        }

        if (npc.GetSkillStateInfo().IsSkillActivated()) {
            return true;
        }

        ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
        float distSqrToHome = Geometry.DistanceSquare(srcPos, info.HomePos);
        if (distSqrToHome > npc.GohomeRange * npc.GohomeRange) {
            AiCommand.AiStopPursue(npc);
            info.ChangeToState((int)AiStateId.Gohome);
            return true;
        }

        ///
        EntityInfo attackTarget = null;
        SkillStateInfo currSkInfo = npc.GetSkillStateInfo();
        ///Find skills you can use
        SkillInfo skInfo = AiLogicUtility.NpcFindCanUseSkill(npc);
        AiCommand.AiSelectSkill(npc, skInfo);
        if (skInfo == null) {
            AiCommand.AiStopPursue(npc);
            info.ChangeToState((int)PredefinedAiStateId.Idle);
            return false;
        }

        CharacterRelation relation = skInfo.ConfigData.targetType == (int)SkillTargetType.Friend ?
                CharacterRelation.RELATION_FRIEND :
                CharacterRelation.RELATION_ENEMY;

        attackTarget = AiLogicUtility.GetNearstTargetHelper(
            npc, skInfo.Distance, relation);

        if (attackTarget != null && null != skInfo) //Find an attackable target within the attack range
            {
            info.Target = attackTarget.GetId();
            ScriptRuntime.Vector3 targetPos = attackTarget.GetMovementStateInfo().GetPosition3D();
            float dir = Geometry.GetYRadian(srcPos, targetPos);
            float curDir = npc.GetMovementStateInfo().GetFaceDir();
            if (Mathf.Abs(dir - curDir) > 0.157f) {
                npc.GetMovementStateInfo().SetWantedFaceDir(dir);
            } else {
                AiCommand.AiStopPursue(npc);
                AiCommand.AiSkill(npc, skInfo.SkillId); //attach the target
            }
            return true;
        }
        attackTarget = AiLogicUtility.GetNearstTargetHelper(
        npc, npc.ViewRange, relation);
        if (attackTarget != null) //Find an attackable target within your field of vision
            {
            AiCommand.AiPursue(npc, attackTarget.GetMovementStateInfo().GetPosition3D()); // chase the target
            return true;
        }

        currSkInfo.SetCurSkillInfo(0);
        AiCommand.AiStopPursue(npc);
        return true;
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
    private bool m_EnableLearning = false;
    private bool m_ParamReaded = false;
    private const long c_IntervalTime = 200;
}
