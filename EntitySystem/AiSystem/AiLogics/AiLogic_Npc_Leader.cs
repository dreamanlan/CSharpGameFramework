using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;

namespace GameFramework
{
    class AiLogic_Npc_Leader : AbstractAiStateLogic
    {
        protected override void OnInitStateHandlers()
        {
            SetStateHandler((int)AiStateId.Idle, this.IdleHandler);
            SetStateHandler((int)AiStateId.Combat, this.CombatHandler);
            SetStateHandler((int)AiStateId.SkillCommand, this.SkillCommandHandler);
            SetStateHandler((int)AiStateId.MoveCommand, this.MoveCommandHandler);
            SetStateHandler((int)AiStateId.PursuitCommand, this.PursuitCommandHandler);
            SetStateHandler((int)AiStateId.PatrolCommand, this.PatrolCommandHandler);
            SetStateHandler((int)AiStateId.WaitCommand, this.WaitCommandHandler);
        }

        protected override void OnStateLogicInit(EntityInfo entity, long deltaTime)
        {
            AiStateInfo info = entity.GetAiStateInfo();
            info.Time = 0;
            info.HomePos = entity.GetMovementStateInfo().GetPosition3D();
            info.Target = 0;
        }

        protected override bool OnStateLogicCheck(EntityInfo entity, long deltaTime)
        {
            if (entity.IsDead()) {
                if (entity.GetAiStateInfo().CurState != (int)AiStateId.Idle) {
                    NotifyAiStopPursue(entity);
                    ChangeToState(entity, (int)AiStateId.Idle);
                }
                return false;
            }
            return true;
        }

        private void IdleHandler(EntityInfo npc, long deltaTime)
        {
            AiData_Leader aiData = GetAiData(npc);
            SkillInfo skInfo = AiLogicUtility.NpcFindCanUseSkill(npc, aiData, aiData.IsAutoOperate);
            if (skInfo == null) {
                return;
            }
            CharacterRelation relation =
                    (skInfo.TargetType == SkillTargetType.Friend ||
                    skInfo.TargetType == SkillTargetType.RandFriend) ?
                    CharacterRelation.RELATION_FRIEND :
                    CharacterRelation.RELATION_ENEMY;

            EntityInfo attackTarget = AiLogicUtility.GetNearstTargetHelper(npc, aiData.IsAutoOperate ? npc.ViewRange : skInfo.Distance, relation);
            if (attackTarget != null) {
                ChangeToState(npc, (int)AiStateId.Combat);
            }
        }

        private void CombatHandler(EntityInfo npc, long deltaTime)
        {
            AiStateInfo aiInfo = npc.GetAiStateInfo();
            AiData_Leader aiData = GetAiData(npc);
            if (npc.GetSkillStateInfo().IsSkillActivated()) {
                return;
            }
            ///
            SkillStateInfo currSkInfo = npc.GetSkillStateInfo();
            ///找到可以使用的技能
            SkillInfo skInfo = AiLogicUtility.NpcFindCanUseSkill(npc, this.GetAiData(npc), aiData.IsAutoOperate);
            NotifyAiSelectSkill(npc, skInfo);
            if (skInfo == null) {
                //没有可以使用的技能就切换到Idle状态
                ChangeToState(npc, (int)AiStateId.Idle);
                return;
            }

            CharacterRelation relation =
                    (skInfo.TargetType == SkillTargetType.Friend ||
                    skInfo.TargetType == SkillTargetType.RandFriend) ?
                    CharacterRelation.RELATION_FRIEND :
                    CharacterRelation.RELATION_ENEMY;
            EntityInfo attackTarget = AiLogicUtility.GetNearstAttackerHelper(npc, relation, aiData);
            if (null != attackTarget) {
                NotifyAiTarget(npc, attackTarget);
                if (Geometry.DistanceSquare(npc.GetMovementStateInfo().GetPosition3D(), attackTarget.GetMovementStateInfo().GetPosition3D()) < skInfo.Distance * skInfo.Distance) {
                    aiInfo.Target = attackTarget.GetId();
                    NotifyAiStopPursue(npc);
                    NotifyAiSkill(npc, skInfo.SkillId);
                    return;
                }
            }
            attackTarget = AiLogicUtility.GetNearstTargetHelper(npc, skInfo.Distance, relation);
            if (attackTarget != null && null != skInfo) { //攻击范围内找到可攻击目标
                NotifyAiTarget(npc, attackTarget);
                aiInfo.Target = attackTarget.GetId();
                NotifyAiStopPursue(npc);
                NotifyAiSkill(npc, skInfo.SkillId); //攻击目标
                return;
            }
            if (aiData.IsAutoOperate) {
                attackTarget = AiLogicUtility.GetNearstTargetHelper(npc, npc.ViewRange, relation);
                if (attackTarget != null && null != skInfo) { //视野内找到可攻击目标
                    NotifyAiPursue(npc, attackTarget.GetMovementStateInfo().GetPosition3D());
                    return;
                }
            }

            ///退出战斗模式清理一下手动技能
            currSkInfo.SetCurSkillInfo(0);
            aiData.ManualSkillId = 0;
            NotifyAiStopPursue(npc);
            ChangeToState(npc, (int)AiStateId.Idle);
        }
        private void SkillCommandHandler(EntityInfo entity, long deltaTime)
        {
            AiData_General data = GetAiData(entity);
            if (null != data) {
                AiLogicUtility.DoSkillCommandState(entity, deltaTime, this, data.ManualSkillId);
                if (data.ManualSkillId > 0)
                    data.ManualSkillId = 0;
            } else {
                ChangeToState(entity, (int)AiStateId.Idle);
            }
        }
        private void MoveCommandHandler(EntityInfo entity, long deltaTime)
        {
            AiLogicUtility.DoMoveCommandState(entity, deltaTime, this);
        }
        private void PursuitCommandHandler(EntityInfo entity, long deltaTime)
        {
            AiLogicUtility.DoPursuitCommandState(entity, deltaTime, this);
        }
        private void PatrolCommandHandler(EntityInfo entity, long deltaTime)
        {
            AiLogicUtility.DoPatrolCommandState(entity, deltaTime, this);
        }
        private void WaitCommandHandler(EntityInfo entity, long deltaTime)
        {
        }
        private AiData_Leader GetAiData(EntityInfo entity)
        {
            AiData_Leader data = entity.GetAiStateInfo().AiDatas.GetData<AiData_Leader>();
            if (null == data) {
                data = new AiData_Leader();
                entity.GetAiStateInfo().AiDatas.AddData(data);
            }
            return data;
        }

        private const long c_IntervalTime = 200;
    }
}


