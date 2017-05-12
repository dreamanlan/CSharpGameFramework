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

public class AiDoMember : ISimpleStoryCommandPlugin
{
    public ISimpleStoryCommandPlugin Clone()
    {
        return new AiDoMember();
    }

    public void ResetState()
    {
        m_ParamReaded = false;
        m_EnableLearning = false;
    }

    public bool ExecCommand(StoryInstance instance, StoryValueParams _params, long delta)
    {
        ArrayList args = _params.Values;
        if (!m_ParamReaded) {
            m_ParamReaded = true;
            m_ObjId = (int)args[0];
            if (args.Count > 1) {
                m_EnableLearning = (int)args[1] != 0;
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
        if (npc.GetSkillStateInfo().IsSkillActivated()) {
            return true;
        }

        EntityInfo leader = AiLogicUtility.GetLivingCharacterInfoHelper(npc, info.LeaderId);
        ScriptRuntime.Vector3 srcPos = npc.GetMovementStateInfo().GetPosition3D();
        ScriptRuntime.Vector3 homePos = ScriptRuntime.Vector3.Zero;
        if (null != leader) {
            homePos = GetHomePos(npc.GetMovementStateInfo().FormationIndex, leader);
        }
        float distSqrToHome = Geometry.DistanceSquare(srcPos, homePos);
        if (distSqrToHome > npc.GohomeRange * npc.GohomeRange) {
            AiCommand.AiStopPursue(npc);
            info.ChangeToState((int)AiStateId.Gohome);
            return true;
        }

        ///
        EntityInfo attackTarget = null;
        SkillStateInfo currSkInfo = npc.GetSkillStateInfo();
        ///找到可以使用的技能
        SkillInfo skInfo = AiLogicUtility.NpcFindCanUseSkill(npc);
        AiCommand.AiSelectSkill(npc, skInfo);
        if (skInfo == null) {
            //没有可以使用的技能就切换到Idle状态
            info.ChangeToState((int)PredefinedAiStateId.Idle);
            return false;
        }

        CharacterRelation relation =
                (skInfo.TargetType == SkillTargetType.Friend ||
                skInfo.TargetType == SkillTargetType.RandFriend) ?
                CharacterRelation.RELATION_FRIEND :
                CharacterRelation.RELATION_ENEMY;

        attackTarget = AiLogicUtility.GetNearstTargetHelper(
            npc, skInfo.Distance, relation);

        if (attackTarget != null && null != skInfo) { //攻击范围内找到可攻击目标            
            info.Target = attackTarget.GetId();
            AiCommand.AiStopPursue(npc);
            AiCommand.AiSkill(npc, skInfo.SkillId); //攻击目标
            return true;
        }
        attackTarget = AiLogicUtility.GetNearstTargetHelper(
        npc, npc.ViewRange, relation);
        if (attackTarget != null) { //视野范围内找到可攻击目标
            AiCommand.AiPursue(npc, attackTarget.GetMovementStateInfo().GetPosition3D()); // 追赶目标
            return true;
        }

        currSkInfo.SetCurSkillInfo(0);
        AiCommand.AiStopPursue(npc);
        info.ChangeToState((int)AiStateId.Gohome);
        return true;
    }
    private bool GohomeHandler(EntityInfo entity, AiStateInfo info, long deltaTime)
    {
        info.Time += deltaTime;
        if (info.Time > c_IntervalTime) {
            info.Time = 0;

            EntityInfo leader = AiLogicUtility.GetLivingCharacterInfoHelper(entity, info.LeaderId);
            if (null != leader) {
                float minDist = entity.GetRadius() + leader.GetRadius();
                ScriptRuntime.Vector3 targetPos = GetHomePos(entity.GetMovementStateInfo().FormationIndex, leader);
                ScriptRuntime.Vector3 srcPos = entity.GetMovementStateInfo().GetPosition3D();
                float powDistToHome = Geometry.DistanceSquare(srcPos, targetPos);
                if (powDistToHome <= (minDist + 1) * (minDist + 1)) {
                    AiCommand.AiStopPursue(entity);
                    info.ChangeToState((int)PredefinedAiStateId.Idle);
                    return false;
                } else {
                    AiCommand.AiPursue(entity, targetPos);
                }
            } else {
                AiCommand.AiStopPursue(entity);
                info.ChangeToState((int)PredefinedAiStateId.Idle);
                return false;
            }
        }
        return true;
    }
    private int GetFormationId(EntityInfo leader)
    {
        int ret = 0;
        if (null != leader) {
            ret = leader.GetAiStateInfo().FormationId;
        }
        return ret;
    }
    private ScriptRuntime.Vector3 GetHomePos(int formationIndex, EntityInfo leader)
    {
        ScriptRuntime.Vector3 pos;
        int id = GetFormationId(leader);
        TableConfig.Formation formation = TableConfig.FormationProvider.Instance.GetFormation(id);
        if (null != formation) {
            TableConfig.Formation.PosDir posDir = formation.GetPosDir(formationIndex);
            float dir;
            pos = posDir.CalcPosDir(leader.GetMovementStateInfo().TargetPosition, leader.GetMovementStateInfo().GetFaceDir(), out dir);
        } else {
            pos = ScriptRuntime.Vector3.Zero;
        }
        return pos;
    }
    private bool IsLeaderDead(EntityInfo entity)
    {
        bool ret = true;
        AiStateInfo info = entity.GetAiStateInfo();
        EntityInfo leader = AiLogicUtility.GetLivingCharacterInfoHelper(entity, info.LeaderId);
        if (null != leader) {
            ret = leader.IsDead();
        }
        return ret;
    }

    private int m_ObjId = 0;
    private bool m_EnableLearning = false;
    private bool m_ParamReaded = false;
    private const long c_IntervalTime = 200;
}
